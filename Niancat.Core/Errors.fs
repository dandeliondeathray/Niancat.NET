module Niancat.Core.Errors

open Domain

type Error =
| Uninitialized
| AlreadyInitialized
| AlreadySet
| InvalidProblem of Problem
| InvalidGuess of Guess * Problem
| NoProblemSet

let toErrorString = function
| Uninitialized -> "The application has not been initialized yet."
| AlreadyInitialized -> "The application has already been initialized."
| AlreadySet -> "Dagens nian är redan satt!"
| InvalidProblem p -> p |> Domain.prettyProblem |> sprintf "%s är inte en giltig nia."
| InvalidGuess (_, problem) ->
    sprintf "Din gissning matchar inte dagens nian (%s)." (Domain.prettyProblem problem)
| NoProblemSet -> "Dagens nian är inte satt än!"
