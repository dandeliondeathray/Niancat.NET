module Niancat.Bot.Talkers

open SlackBotNet
open Slack

open Niancat.Core.Domain
open Niancat.Core.Events
open Niancat.Persistence.Queries

let commonChannel = "#general"

let talkAboutNewProblem slack queries (User user) problem = 
    [
        slack.send (sprintf "@%s" user) "OK!"
        async {
            let! solutionsHint = async {
                let! count = queries.problem.getSolutionCount problem
                let count' =
                    match count with
                    | Some c' -> c'
                    | None -> 0
                return 
                    if count' > 1
                    then sprintf "\n(det finns %d lösningar)" count'
                    else ""
            }
            do! sprintf "%s satte nian till %s.%s" user (prettyProblem problem) solutionsHint |> slack.send commonChannel
        }
    ] 
    |> Async.Parallel
    |> Async.Ignore
    
    

let talkAboutEvent slack (queries : Queries) = function
    | ProblemSet (user, problem) -> talkAboutNewProblem slack queries user problem
    | IncorrectGuess (User user, Guess guess) ->
        slack.send (sprintf "@%s" user) (sprintf "%s är inte rätt lösning, försök igen!" guess)
    | AlreadySolved (User user) ->
        slack.send (sprintf "@%s" user) (sprintf "Du har redan hittat den lösningen!")
    | Initialized _ -> async.Return ()
    | Solved (User user, Hash hash) ->
        Async.Parallel [
            slack.send (sprintf "@%s" user) "Det är en korrekt lösning - bra jobbat!"
            slack.send commonChannel (sprintf "%s löste nian: %s" user hash)
        ] |> Async.Ignore

