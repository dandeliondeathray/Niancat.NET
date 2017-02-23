namespace Niancat.Core

open Niancat.Utilities

module Solution =

    open Types

    let normalize =
        String.removeControlChars
        >> String.replace "é|è|É|È" "E"
        >> String.remove "[^A-Za-zÅÄÖåäö]"
        >> String.toUpper

    let key = normalize >> String.sortChars

    let wordlist : string list -> Wordlist =
        Seq.groupBy key
        >> Seq.map (fun (key, words) -> key, List.ofSeq words)
        >> Map.ofSeq
    let isWord wordlist word =
        match Map.tryFind (key word) wordlist with
        | Some words -> List.exists (fun w -> w = word) words
        | None -> false

    let hash (guess : Word) (user : User) =
        sprintf "%s%s" (guess.ToUpper()) (user.ToLower()) |> String.sha256

module CommandHandler = 

    open Commands
    open Events
    open Solution
    open States
    open Types

    let private setProblem letters newLetters user =
        if key letters = key newLetters
        then AlreadySet
        else NewProblemSet { letters = newLetters; user = user }

    let private initialize wordlist = AppInitiated wordlist

    let private solve wordlist letters guess user =
        if key guess = key letters && isWord wordlist guess
        then Solved { user = user; hash = Solution.hash guess user }
        else IncorrectGuess

    let apply state command user =
        match state, command with
        | Initial, Initialize wordlist -> initialize wordlist
        | Started _, SetProblem letters -> setProblem "" letters user
        | Set
            {
                wordlist = wordlist
                problem = { letters = letters; solvers = _; date = _ }
                previous = _
            }, SetProblem newLetters -> setProblem letters newLetters user
        | Set
            {
                wordlist = wordlist
                problem = { letters = letters; solvers = _; date = _ }
                previous = _
            },
            Solve guess -> solve wordlist letters guess user
        | _ -> InvalidCommand

module EventHandler =

    open Events
    open States

    let private setFirstProblem wordlist letters =
        let problem = {
            letters = letters
            solvers = []
            date = System.DateTime.Today
        }
        
        Set {
            problem = problem
            wordlist = wordlist
            previous = []
        }
        

    let apply state = function
        | AppInitiated wordlist -> Started wordlist
        | _ -> state