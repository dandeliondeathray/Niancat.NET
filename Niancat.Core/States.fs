module Niancat.Core.States

open Domain
open Events

type ProblemData = {
    letters : Problem
    solvers : Solver list
    date : System.DateTime
}

type SetData = {
    problem : ProblemData
    wordlist : Wordlist
    previous : ProblemData list
}

let newProblem word =
    { letters = word; solvers = []; date = System.DateTime.Today }

let newSetData wordlist word =
    {
        problem = newProblem word
        wordlist = wordlist
        previous = []
    }

let nextSetData setData word =
    {
        problem = newProblem word
        wordlist = setData.wordlist
        previous = setData.problem :: setData.previous
    }

let solved setData user hash =
    { setData with
        problem =
        { setData.problem with
            solvers = (user, hash) :: setData.problem.solvers
        }
    }

type ApplicationState =
| TabulaRasa
| Started of Wordlist
| Set of SetData

let apply state event =
    match state, event with
    | TabulaRasa, Initialized wordlist -> Started wordlist
    | Started wordlist, ProblemSet (_, word) -> newSetData wordlist word |> Set
    | Set data, ProblemSet (_, word) -> nextSetData data word |> Set
    | Set data, Solved (user, hash) -> solved data user hash |> Set

    // These should never affect the state
    | state, IncorrectGuess (user, guess) -> state
    | state, AlreadySolved _ -> state

    // These should never happen, but are here to indicate we know that
    // (instead of having a catch-all that will hide warnings when we do need to add a case)
    | state, ProblemSet _ ->
        printfn "WARNING: ProblemSet was triggered in an invalid state %A" state
        state
    | state, Solved _ ->
        printfn "WARNING: Solved was triggered in an invalid state %A" state
        state
    | state, Initialized _ ->
        printfn "WARNING: Initialized was triggered in an invalid state %A" state
        state
