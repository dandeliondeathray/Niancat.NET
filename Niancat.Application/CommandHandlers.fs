module Niancat.Application.CommandHandlers

open Niancat.Persistence.EventStore
open Niancat.Persistence.Queries
open Niancat.Core.Domain
open Niancat.Core.Commands
open Niancat.Core.Events
open Niancat.Core.CommandHandlers
open Niancat.Core.Errors

open Niancat.Application.Commanders

open Niancat.Utilities.Errors
open Niancat.Core.States

type CommandResult = Async<Result<ApplicationState * Event list, string>>

let private _handle eventStore (eventsStream : Event<Event list>) command = async {
    let! state = eventStore.getState ()
    match evolve command state with
    | Success (state, events) -> 
        eventsStream.Trigger events
        do! eventStore.saveEvents events
        return ok (state, events)
    | Failure error -> return (error |> toErrorString |> fail)
}

let handleCommand<'a,'b> eventStore eventsStream (commander : Commander<'a, 'b>) (data : 'a) = async {
    let! validationResult = commander.validate data
    
    match validationResult with
    | Success validatedCommandData ->
        let command = commander.toCommand validatedCommandData 
        return! _handle eventStore eventsStream command
    | Failure errorMessage ->
        return errorMessage |> fail
}
