module Niancat.Api.RequestParsers

open FSharp.Data
open Niancat.Core.Domain

[<Literal>]
let GuessJson = """{
    "guess": {
        "user": "niancat",
        "guess": "NIANCAT"
    }
}
"""
type GuessReq = JsonProvider<GuessJson>

let (|GuessRequest|_|) payload =
    try
        let data = GuessReq.Parse(payload).Guess
        Some (Guess data.Guess, User data.User)
    with
        ex -> None


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
        Some (Problem data.Problem, User data.User)
    with
        ex -> None
