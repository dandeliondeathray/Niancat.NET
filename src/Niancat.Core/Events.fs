namespace Niancat.Core

open Types

module Events =

    type SolvedData = {
        user : User
        hash : Hash
    }

    type NewProblemData = {
        user : User
        letters : string
    }

    type DomainEvent =
    | AppInitiated of Wordlist
    | Solved of SolvedData
    | NewProblemSet of NewProblemData
    | IncorrectGuess
    | AlreadySet
    | InvalidCommand
