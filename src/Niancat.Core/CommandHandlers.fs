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

let setIfValid wordlist problem user =
    if Map.containsKey (key (P problem)) wordlist
    then ok [ProblemSet (user, problem)]
    else InvalidProblem problem |> fail

let handleSetProblem word user = function
    | Started wordlist -> setIfValid wordlist word user 
    | Set { problem = { letters = prev; solvers = _; date = _ }; wordlist = wordlist; previous = _ } -> 
        if word <> prev
        then setIfValid wordlist word user
        else fail AlreadySet
    | TabulaRasa -> fail Uninitialized

let handleGuess guess user = function
    | Set { problem = { letters = letters; solvers = _; date = _ }; wordlist = wordlist; previous = _ } ->
        if key (P letters) = key (G guess)
        then 
            if isWord wordlist guess
            then ok [Solved (user, hash guess user)]
            else fail (IncorrectGuess guess)
        else fail (InvalidGuess (guess, letters))
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
