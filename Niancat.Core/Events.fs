module Niancat.Core.Events

open Niancat.Core.Domain

type Event =
| Initialized of Wordlist
| ProblemSet of User * Problem
| Solved of User * Hash
| AlreadySolved of User
| IncorrectGuess of User * Guess
