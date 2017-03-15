module Niancat.Api.CommandApi

open Niancat.Api.CommandHandlers
open Niancat.Api.Commanders.SetProblem

open Niancat.Utilities.Errors

let handleCommandRequest eventStore = function
    | SetProblemRequest problem -> handleCommand eventStore problem setProblemCommander
    | _ -> err "Invalid command" |> fail |> async.Return
