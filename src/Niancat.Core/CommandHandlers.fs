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
    if isValid wordlist problem
    then
        ok [ProblemSet (user, normalize problem)]
    else InvalidProblem problem |> fail

let handleSetProblem problem user = function
    | Started wordlist -> setIfValid wordlist problem user
    | Set { problem = { letters = prev; solvers = _; date = _ }; wordlist = wordlist; previous = _ } ->
        if not <| sameProblem prev problem
        then setIfValid wordlist problem user
        else fail AlreadySet
    | TabulaRasa -> fail Uninitialized

let handleGuess guess user = function
    | Set { problem = { letters = letters; solvers = _; date = _ }; wordlist = wordlist; previous = _ } ->
        if key (P letters) = key (G guess)
        then
            if isWord wordlist guess
            then ok [Solved (user, hash guess user)]
            else ok [IncorrectGuess (user, guess)]
        else fail (InvalidGuess (guess, letters))
    | _ -> fail NoProblemSet

let execute = function
    | Initialize wordlist -> handleInitialize wordlist
    | SetProblem (problem, user) -> handleSetProblem problem user
    | MakeGuess (guess, user) -> handleGuess guess user

let evolve command state =
    match execute command state with
    | Success events ->
        let state' = List.fold apply state events
        (state', events) |> ok
    | Failure error -> fail error
