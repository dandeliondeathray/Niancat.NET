module Niancat.Core.States

open Domain
open Events

type ProblemData = {
    letters : Problem
    solvers : User list
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

let solved setData user =
    { setData with
        problem = 
        { setData.problem with
            solvers = user :: setData.problem.solvers
        }        
    }

type ApplicationState =
| TabulaRasa
| Started of Wordlist
| Set of SetData

let apply state event =
    match state, event with
    | TabulaRasa, Initialized wordlist -> Started wordlist
    | Started wordlist, ProblemSet (user, word) -> newSetData wordlist word |> Set
    | Set data, ProblemSet (user, word) -> nextSetData data word |> Set
    | Set data, Solved (user, hash) -> solved data user |> Set
    | _ -> failwith "todo event handling"
