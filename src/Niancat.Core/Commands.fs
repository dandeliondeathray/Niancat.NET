module Niancat.Core.Commands

open Domain

type Command =
| Initialize of Wordlist
| SetProblem of Word * User
| Guess of Word * User
