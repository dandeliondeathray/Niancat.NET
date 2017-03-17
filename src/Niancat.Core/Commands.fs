module Niancat.Core.Commands

open Domain

type Command =
| Initialize of Wordlist
| SetProblem of Problem * User
| MakeGuess of Guess * User
