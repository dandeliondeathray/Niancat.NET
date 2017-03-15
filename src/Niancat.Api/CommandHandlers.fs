module Niancat.Api.CommandHandlers

open Niancat.Persistence.EventStore
open Niancat.Persistence.Queries
open Niancat.Core.Domain
open Niancat.Core.Commands
open Niancat.Core.CommandHandlers
open Niancat.Core.Errors

open Niancat.Utilities.Errors

type Commander<'a, 'b> = {
    validate : 'a -> Async<Choice<'b, string>>
    toCommand : 'b -> Command
}

type ErrorResponse = {
    Message : string
}
let err msg = { Message = msg }

let private _handle eventStore validatedData commander = async {
    let command = commander.toCommand validatedData
    let! state = eventStore.getState ()
    match evolve command state with
    | Success (state, events) -> 
        do! eventStore.saveEvents events
        return ok (state, events)
    | Failure error -> return (error |> toErrorString |> err |> fail)
}

let handleCommand eventStore commandData commander = async {
    let! validationResult = commander.validate commandData
    
    match validationResult with
    | Choice1Of2 validatedCommandData ->
        return! _handle eventStore validatedCommandData commander
    | Choice2Of2 errorMessage ->
        return errorMessage |> err |> fail
}
