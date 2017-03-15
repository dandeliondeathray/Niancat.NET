module Niancat.Core.Errors

open Domain

type Error =
| Uninitialized
| AlreadyInitialized
| AlreadySet
| InvalidProblem of Word
| InvalidGuess of Word * Word
| IncorrectGuess of Word
| NoProblemSet

let toErrorString = function
| Uninitialized -> "The application has not been initialized yet."
| AlreadyInitialized -> "The application has already been initialized."
| AlreadySet -> "Dagens nian är redan satt!"
| InvalidProblem (Word w) -> sprintf "Ogiltigt problem; %s finns inte i ordlistan." w
| InvalidGuess (_, problem) ->
    sprintf "Din gissning matchar inte dagens nian (%s)." (Domain.prettyProblem problem)
| IncorrectGuess (Word guess) -> sprintf "%s är inte rätt lösning." guess
| NoProblemSet -> "Dagens nian är inte satt än!"
