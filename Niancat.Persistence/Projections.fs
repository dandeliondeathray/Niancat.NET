module Niancat.Persistence.Projections

open Niancat.Core.Events
open Niancat.Core.Domain
open Niancat.Persistence.Queries

type ProblemActions = {
    setProblem : Problem -> Async<unit>
}

type WordlistActions = {
    setWordlist : Wordlist -> Async<unit>
}

type ProjectionActions = {
    problem : ProblemActions
    wordlist : WordlistActions
}

let projectReadModel actions (queries : Queries) = Async.Ignore << Async.Parallel << function
    | ProblemSet (_, problem) -> [actions.problem.setProblem problem]
    | Initialized wordlist -> [actions.wordlist.setWordlist wordlist]
    | _ -> List.empty<Async<unit>> // uninteresting event
