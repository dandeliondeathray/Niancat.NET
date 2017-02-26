namespace Niancat.Core

module Application =
    
    open Niancat.Core
    open Niancat.Core.Commands
    open Niancat.Core.Events
    open Niancat.Core.States
    open Niancat.Core.Types

    type Db = {
        wordlist : unit -> Async<Wordlist>
        events : unit -> Async<DomainEvent list>
        saveEvent : DomainEvent -> Async<unit>
    }

    type App = {
        getWordlist : unit -> Async<Word list>
        sendCommand : User -> Command -> Async<DomainEvent>
    }

    let start (db : Db) = async {

            let! wordlist = db.wordlist ()

            let mutable state = Started wordlist

            let sendCommand user cmd =
                async {
                    let event = CommandHandler.apply state user cmd
                    let state' = EventHandler.apply state event

                    state <- state'
                    do! db.saveEvent event
                    return event
                }

            let getWordlist () = async {
                let! words = db.wordlist ()
                return words 
                    |> Map.toSeq
                    |> Seq.collect snd
                    |> List.ofSeq
                    |> List.sort
            }
            
            return {
                getWordlist = getWordlist
                sendCommand = sendCommand
            }
        }
