module Niancat.Persistence.ReadModels

open Niancat.Core.Domain

type CurrentProblem =
| NotSet
| Set of Word

type Wordlist =
| NotSet
| Set of Wordlist
