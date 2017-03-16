module Niancat.Persistence.InMemory.Problem

open Niancat.Persistence.ReadModels
open Niancat.Persistence.Projections
open Niancat.Persistence.Queries

open Niancat.Core.Domain
open Niancat.Core.Errors
open Niancat.Utilities.Errors

let mutable private currentProblem : Problem option = None

let getCurrentProblem () = currentProblem |> async.Return

let setCurrentProblem word = 
    currentProblem <- Some word
    async.Return ()

let problemActions = {
    setProblem = setCurrentProblem
}

let problemQueries = {
    getCurrent = getCurrentProblem
}
