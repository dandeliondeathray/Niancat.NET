module Niancat.Core.CommandHandlers

open Domain
open Events
open Errors
open Commands
open States

open Niancat.Utilities.Errors

let handleInitialize wordlist = function
    | TabulaRasa -> ok [Initialized wordlist]
    | _ -> fail AlreadyInitialized

let setIfValid wordlist word user =
    if Map.containsKey (key word) wordlist
    then ok [ProblemSet (user, word)]
    else InvalidProblem word |> fail

let handleSetProblem word user = function
    | Started wordlist -> setIfValid wordlist word user 
    | Set { problem = { letters = prev; solvers = _; date = _ }; wordlist = wordlist; previous = _ } -> 
        if word <> prev
        then setIfValid wordlist word user
        else fail AlreadySet
    | TabulaRasa -> fail Uninitialized

let handleGuess word user = function
    | Set { problem = { letters = letters; solvers = _; date = _ }; wordlist = wordlist; previous = _ } ->
        if key letters = key word 
        then 
            if isWord wordlist word
            then ok [Solved (user, hash word user)]
            else fail (IncorrectGuess word)
        else fail (InvalidGuess (letters, word))
    | _ -> fail NoProblemSet

let execute = function
    | Initialize wordlist -> handleInitialize wordlist
    | SetProblem (word, user) -> handleSetProblem word user
    | Guess (word, user) -> handleGuess word user

let evolve command state =
    match execute command state with
    | Success events ->
        let state' = List.fold apply state events
        (state', events) |> ok
    | Failure error -> fail error
