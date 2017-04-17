module Niancat.Application.Commanders

open Niancat.Utilities.Errors

open Niancat.Core.Domain
open Niancat.Core.Commands
open System.IO

type ValidationResult<'a> = Async<Result<'a, string>>
type Validator<'a,'b> = 'a -> ValidationResult<'b>

type Commander<'a, 'b> = {
    validate : Validator<'a, 'b>
    toCommand : 'b -> Command
}

type Commanders = {
    setProblem : Commander<Problem*User, Problem*User>
    makeGuess : Commander<Guess*User, Guess*User>
    initialize : Commander<string, string>
}

let internal commanders =
    let setProblemCommander = 
        let validateSetProblem (letters, user) = async {
            return
                match letters, user with
                | Problem null, _ | Problem "", _ -> fail "Problem får inte vara tomt"
                | _, User null | _, User "" -> fail "User får inte vara tomt"
                | p, u -> ok (letters, user)
        }

        let toCommand (letters, user) = SetProblem (letters, user)

        {
            validate = validateSetProblem
            toCommand = toCommand
        }

    let makeGuessCommander =
        let validateGuess (guess, user) = async {
            return
                match guess, user with
                | Guess null, _ | Guess "", _ -> fail "Guess får inte vara tomt"
                | _, User null | _, User "" -> fail "User får inte vara tomt"
                | g, u -> ok (g, u)
        }

        let toCommand (guess, user) = MakeGuess (guess, user)

        {
            validate = validateGuess
            toCommand = toCommand
        }

    let initializeCommander = 
        let validate wordlistFile = async {
            let fullPath = Path.GetFullPath wordlistFile

            if File.Exists fullPath
            then
                use stream = File.OpenRead fullPath
                let! contents = stream.AsyncRead(int stream.Length)

                let text = System.Text.Encoding.UTF8.GetString contents

                return ok text
            else
                return fail <| sprintf "Ingen ordlista hittades på '%s'" fullPath
        }

        let toCommand (wordlistFileContents : string) =
            let wordlist =
                wordlistFileContents.Split(System.Environment.NewLine.ToCharArray())
                |> List.ofArray
                |> wordlist

            Initialize wordlist
    
        {
            validate = validate
            toCommand = toCommand
        }

    {
        setProblem = setProblemCommander
        makeGuess = makeGuessCommander
        initialize = initializeCommander
    }
