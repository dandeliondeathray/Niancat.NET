module Niancat.Persistence.InMemory.Queries

open Niancat.Persistence.Queries
open Niancat.Persistence.InMemory.Problem
open Niancat.Persistence.InMemory.Wordlist

let queries = {
    problem = problemQueries
    wordlist = wordlistQueries
}