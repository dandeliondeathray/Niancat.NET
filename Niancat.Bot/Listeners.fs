module Niancat.Bot.Listeners

open SlackBotNet
open Slack
open SlackBotNet.State

open Niancat.Utilities.Errors
open Niancat.Core.Domain

let showProblem getProblem =
    let stateProblem (conv : Conversation) = async {
        let! problem = getProblem ()

        let answer =
            match problem with
            | Some p -> prettyProblem p
            | None -> "Nian är inte satt än!"
        
        do! reply conv answer
    }

    {
        matcher = Matchers.regex "[^@]?nian\s*$"
        action = stateProblem
    }

let setProblem handleCommand commander = 
    let saveProblem (conv : Conversation) = async {
        let data = Problem conv.Matches.[1].Text, User conv.From.Username
        
        let! result = handleCommand commander data

        match result with
        | Success (state, events) -> ()
        | Failure err ->
            do! reply conv err
    }

    {
        matcher = Matchers.regex "set (.*)$"
        action = saveProblem
    }

let makeGuess handleCommand commander =
    let guess (conv : Conversation) = async {
        let data = Guess conv.Matches.[1].Text, User conv.From.Username

        let! result = handleCommand commander data

        match result with
        | Success (state, events) -> ()
        | Failure err ->
            do! reply conv err
    }

    {
        matcher = Matchers.regex "(.*)"
        action = guess
    }