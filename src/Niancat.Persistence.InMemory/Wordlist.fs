module Niancat.Persistence.InMemory.Wordlist

open Niancat.Persistence.ReadModels
open Niancat.Persistence.Projections
open Niancat.Persistence.Queries

open Niancat.Core.Domain

open Niancat.Utilities.Errors

let mutable private wordlist : Wordlist option = None

let getWordlist () = wordlist |> async.Return

let setWordlist w =
    wordlist <- Some w
    async.Return ()

let wordlistQueries = {
    getDictionary = getWordlist
}

let wordlistActions = {
    setWordlist = setWordlist
}