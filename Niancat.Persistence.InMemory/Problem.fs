module Niancat.Persistence.InMemory.Problem

open Niancat.Persistence.ReadModels
open Niancat.Persistence.Projections
open Niancat.Persistence.Queries

open Niancat.Core.Domain
open Niancat.Core.Errors
open Niancat.Utilities.Errors

open DataStore

let getCurrentProblem () = readModelState.currentProblem |> async.Return

let getSolutionCount problem = 
    match readModelState.wordlist with
    | Some wordlist -> 
        if isValid wordlist problem
        then wordlist.[problem |> P |> key] |> List.length |> Some
        else Some 0
    | _ -> None
    |> async.Return

let setCurrentProblem word = 
    readModelState.currentProblem <- Some word
    async.Return ()

let problemActions = {
    setProblem = setCurrentProblem
}

let problemQueries = {
    getCurrent = getCurrentProblem
    getSolutionCount = getSolutionCount
}
