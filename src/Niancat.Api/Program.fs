open Suave
open Suave.Successful

open Niancat.Api
open Niancat.Persistence
open Niancat.Core
open Niancat.Core.Events
open Niancat.Utilities
open Niancat.Utilities.Errors

open Niancat.Persistence.Projections
open Niancat.Persistence.InMemory.EventStore
open Niancat.Api.CommandHandlers
open Niancat.Api.Commanders.Initialize

open Events

let project event =
    projectReadModel inMemoryActions event
    |> Async.RunSynchronously |> ignore

let projectEvents = List.iter project

let startServer wordlistFile = async {
    let eventStore = inMemoryEventStore ()
    let eventsStream = new Control.Event<Event list>()

    eventsStream.Publish.Add projectEvents

    let! initialized = handleCommand eventStore eventsStream wordlistFile initializeCommander
    match initialized with
    | Success (state, events) ->
        startWebServer defaultConfig (WebParts.niancat eventStore eventsStream)
    | Failure err ->
        printfn "%s" err.Message
}

[<EntryPoint>]
let main argv =

    match List.ofArray argv with
    | [wordlistFile] ->
        startServer wordlistFile |> Async.RunSynchronously
    | _ ->
        printfn "Usage: Niancat.Api.exe path-to-saol.txt"

    0
