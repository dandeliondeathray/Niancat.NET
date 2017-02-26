open Suave
open Suave.Successful

open Niancat.Api
open Niancat.Persistence
open Niancat.Core
open Niancat.Core.Events
open Niancat.Utilities

let runServer wordlistFile eventsFile = async {
    let db = Db.onFile wordlistFile eventsFile
    let! app = Application.start db
    startWebServer defaultConfig (WebParts.niancat app)
}

[<EntryPoint>]
let main argv =

    let task =
        match List.ofArray argv with
        | wordlistFile :: eventsFile :: _ -> runServer wordlistFile eventsFile
        | _ -> async {
            printfn "Usage: Niancat.Api.exe path-to-saol.txt path-to-events.txt"
        }
    
    Async.RunSynchronously task

    0 // return an integer exit code
