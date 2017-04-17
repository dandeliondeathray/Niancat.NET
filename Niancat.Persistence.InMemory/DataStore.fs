module Niancat.Persistence.InMemory.DataStore

open Niancat.Core.Domain

type Store = {
    mutable currentProblem : Problem option
    mutable currentSolutionCount : int option
    mutable wordlist : Wordlist option
}

let readModelState = {
    currentProblem = None
    currentSolutionCount = None
    wordlist = None
}
