module Niancat.Persistence.Projections

open Niancat.Core.Events
open Niancat.Core.Domain

type ProblemActions = {
    setProblem : Word -> Async<unit>
}

type WordlistActions = {
    setWordlist : Wordlist -> Async<unit>
}

type ProjectionActions = {
    problem : ProblemActions
    wordlist : WordlistActions
}

let projectReadModel actions = function
    | ProblemSet (_, word) -> [actions.problem.setProblem word] |> Async.Parallel
    | Initialized wordlist -> [actions.wordlist.setWordlist wordlist] |> Async.Parallel
    | _ ->
        // uninteresting event
        async.Return [|()|]
