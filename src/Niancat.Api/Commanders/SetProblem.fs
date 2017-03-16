module Niancat.Api.Commanders.SetProblem

open FSharp.Data
open Niancat.Core.Domain
open System

open Niancat.Api.CommandHandlers
open Niancat.Core.Commands

[<Literal>]
let SetProblemJson = """{
    "setProblem": {
        "problem": "NIANCAT",
        "user": "niancat"
    }
}"""
type SetProblemReq = JsonProvider<SetProblemJson>

let (|SetProblemRequest|_|) payload =
    try
        let data = SetProblemReq.Parse(payload).SetProblem
        Some (data.Problem, data.User)
    with
        ex -> None

let validateSetProblem (problem, user) = async {
    return
        match problem, user with
        | null, _ | "", _ -> Choice2Of2 "Problem får inte vara tomt"
        | _, null | _, "" -> Choice2Of2 "User får inte vara tomt"
        | p, u -> Choice1Of2 (p, u)
}

let toCommand (problem, user) = SetProblem (Problem problem, User user)

let setProblemCommander = {
    validate = validateSetProblem
    toCommand = toCommand
}
