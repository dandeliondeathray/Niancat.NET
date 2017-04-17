module Niancat.Api.CommandApi

open Suave
open Suave.Successful
open Suave.RequestErrors
open Suave.Writers
open Suave.Operators
open JsonFormatters

open Niancat.Api.RequestParsers

open Niancat.Application.ApplicationServices
open Niancat.Application.Commanders
open Niancat.Application.CommandHandlers
open Niancat.Utilities.Errors
open Niancat.Core.Domain
open Niancat.Core.Events
open Niancat.Core.States

let commandApiHandler applicationService (context : HttpContext) = async {
    let payload = System.Text.Encoding.UTF8.GetString context.request.rawForm

    let! response = 
        match payload with
        | SetProblemRequest (problem, user) -> 
            handleCommand applicationService.eventStore applicationService.eventStream applicationService.commanders.setProblem (problem, user)
        | GuessRequest (guess, user) ->
            handleCommand applicationService.eventStore applicationService.eventStream applicationService.commanders.makeGuess (guess, user)
        | input -> 
            sprintf "Invalid command: %s" input |> fail |> async.Return

    match response with
    | Success (state, events) ->
        return! OK "" context
    | Failure err ->
        return! (BAD_REQUEST (jstr err) >=> setMimeType "application/json") context
}
