module Niancat.Persistence.Queries

open ReadModels
open Niancat.Core.Domain
open Niancat.Core.States
open Niancat.Utilities.Errors

type ProblemQueries = {
    getCurrent : unit -> Async<Problem option>
}

type WordlistQueries = {
    getDictionary : unit -> Async<Wordlist option>
}

type Queries = {
    problem : ProblemQueries
    wordlist : WordlistQueries
}
