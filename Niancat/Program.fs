open Niancat.Api.Server
open Niancat.Bot.Bot

open Niancat.Core.Events
open Niancat.Persistence.InMemory.EventStore
open Niancat.Application.ApplicationServices
open Niancat.Utilities

let startNiancat wordlistFile (cancellationTokenSource : System.Threading.CancellationTokenSource) = async {
    let eventStore = inMemoryEventStore ()
    let projections = inMemoryActions
    let queries = inMemoryQueries

    let! app = niancat queries projections eventStore wordlistFile

    return!
        match app with
        | Some app' -> async {
                //do! startServer app cancellationTokenSource.Token
                do! startBot app' |> Async.Ignore
                return None
            }
        | None -> Some () |> async.Return
}


[<EntryPoint>]
let main argv = 

    match List.ofArray argv with
    | [wordlistFile] -> 
        Program.waitForCtrlC (startNiancat wordlistFile)
    | _ ->
        printfn "Usage: Niancat.Api.exe path-to-saol.txt"

    0
