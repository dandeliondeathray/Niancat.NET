module Niancat.Persistence.Queries

open ReadModels
open Niancat.Core.Domain
open Niancat.Core.States

type ProblemQueries = {
    getCurrent : unit -> Async<Word option>
}

type WordlistQueries = {
    getDictionary : unit -> Async<Wordlist option>
}

type Queries = {
    problem : ProblemQueries
    wordlist : WordlistQueries
}
