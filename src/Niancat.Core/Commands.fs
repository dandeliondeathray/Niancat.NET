namespace Niancat.Core

module Commands =

    open Types

    type Command =
    | Initialize of Wordlist
    | SetProblem of Word
    | Solve of Word

