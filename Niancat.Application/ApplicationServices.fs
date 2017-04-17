module Niancat.Application.ApplicationServices

open Niancat.Core.Events
open Niancat.Core.States
open Niancat.Application.Commanders
open Niancat.Application.CommandHandlers

open Niancat.Persistence.EventStore
open Niancat.Persistence.Queries
open Niancat.Persistence.Projections

open Niancat.Utilities.Errors

type NiancatApplication = {
    queries : Queries
    commanders : Commanders
    eventStream : Event<Event list>
    eventStore : EventStore
}

let niancat 
    queries 
    projections
    eventStore
    wordlistFile = async {

    let project event =
        projectReadModel projections queries event
        |> Async.RunSynchronously |> ignore

    let projectEvents = List.iter project

    let eventStream = Event<Event list> ()

    let logEvents = List.iter (fun event -> printfn "%A" event)

    eventStream.Publish.Add logEvents
    eventStream.Publish.Add projectEvents

    let! initialized = handleCommand eventStore eventStream commanders.initialize wordlistFile

    return 
        match initialized with
        | Success (state, events) -> 
            Some {
                queries = queries
                commanders = commanders
                eventStream = eventStream
                eventStore = eventStore
            }
        | Failure err -> 
            printfn "Initialization failed: %s" err
            None
}