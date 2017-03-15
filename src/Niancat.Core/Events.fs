module Niancat.Core.Events

open Niancat.Core.Domain

type Event =
| Initialized of Wordlist
| ProblemSet of User * Word
| Solved of User * Hash
