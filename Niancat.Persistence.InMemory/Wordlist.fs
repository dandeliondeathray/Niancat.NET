module Niancat.Persistence.InMemory.Wordlist

open Niancat.Persistence.ReadModels
open Niancat.Persistence.Projections
open Niancat.Persistence.Queries

open Niancat.Persistence.InMemory.DataStore

open Niancat.Core.Domain
open Niancat.Utilities.Errors

let getWordlist () =  readModelState.wordlist |> async.Return

let setWordlist w =
    readModelState.wordlist <- Some w
    async.Return ()

let wordlistQueries = {
    getDictionary = getWordlist
}

let wordlistActions = {
    setWordlist = setWordlist
}
