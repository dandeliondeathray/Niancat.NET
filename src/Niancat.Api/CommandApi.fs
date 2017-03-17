module Niancat.Api.CommandApi

open Niancat.Api.CommandHandlers
open Niancat.Api.Commanders.SetProblem
open Niancat.Api.Commanders.Guess

open Niancat.Utilities.Errors

let handleCommandRequest eventStore eventsStream = function
    | SetProblemRequest problem -> handleCommand eventStore eventsStream problem setProblemCommander
    | GuessRequest guess -> handleCommand eventStore eventsStream guess makeGuessCommander
    | _ -> err "Invalid command" |> fail |> async.Return
