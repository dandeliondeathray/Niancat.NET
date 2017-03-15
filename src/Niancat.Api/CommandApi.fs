module Niancat.Api.CommandApi

open Niancat.Api.CommandHandlers
open Niancat.Api.Commanders.SetProblem

open Niancat.Utilities.Errors

let handleCommandRequest eventStore eventsStream = function
    | SetProblemRequest problem -> handleCommand eventStore eventsStream problem setProblemCommander
    | _ -> err "Invalid command" |> fail |> async.Return
