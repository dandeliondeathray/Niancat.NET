namespace Niancat.Core

module States =

    open Types

    type ProblemData = {
        letters : Word
        solvers : User list
        date : System.DateTime
    }

    type SetData = {
        problem : ProblemData
        wordlist : Wordlist
        previous : ProblemData list
    }

    type ApplicationState =
    | Started of Types.Wordlist
    | Set of SetData
