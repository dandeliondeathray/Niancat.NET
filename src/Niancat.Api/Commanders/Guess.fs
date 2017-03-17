module Niancat.Api.Commanders.Guess

open FSharp.Data
open System

open Niancat.Api.CommandHandlers
open Niancat.Core.Domain
open Niancat.Core.Commands

open Niancat.Utilities.Errors

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
        Some (data.Guess, data.User)
    with
        ex -> None

let validateGuess (guess, user) = async {
    return
        match guess, user with
        | null, _ | "", _ -> fail "Guess får inte vara tomt"
        | _, null | _, "" -> fail "User får inte vara tomt"
        | g, u -> ok (g, u)
}

let toCommand (guess, user) = MakeGuess (Guess guess, User user)

let makeGuessCommander = {
    validate = validateGuess
    toCommand = toCommand
}
