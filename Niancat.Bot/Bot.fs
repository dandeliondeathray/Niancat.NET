module Niancat.Bot.Bot

open Slack
open SlackBotNet.State
open SlackBotNet
open System.Threading
open System.Threading.Tasks

open Niancat.Core.Events
open Niancat.Application.Commanders
open Niancat.Application.CommandHandlers
open Niancat.Application.ApplicationServices
open Niancat.Utilities
open Niancat.Utilities.Errors
open Niancat.Application.CommandHandlers
open Niancat.Core.Domain

open Listeners
open Talkers
    
let accessToken = "xoxb-166667835235-utOgeBOu0lZVNMeZjXooOd11"

let logMatch (botPart, hubs) =
    { 
        botPart with 
            action = 
                fun conv -> async {
                    printfn "Matcher %A picked up when @%s said '%s'" botPart.matcher conv.From.Username conv.Text
                    do! botPart.action conv
                }
    }, hubs

let startBot (applicationService : NiancatApplication) = async {
    printfn "this is niancat.bot"
    printfn "token: %s" accessToken
    
    let show = showProblem applicationService.queries.problem.getCurrent
    let set = setProblem (handleCommand applicationService.eventStore applicationService.eventStream) applicationService.commanders.setProblem
    let guess = makeGuess (handleCommand applicationService.eventStore applicationService.eventStream) applicationService.commanders.makeGuess

    let parts = 
        [
            show, Some (HubType.DirectMessage ||| HubType.ObserveAllMessages)
            show, Some HubType.Channel
            set, None
            guess, Some (HubType.DirectMessage ||| HubType.ObserveAllMessages)            
        ]
        |> List.map logMatch

    let! bot = connect accessToken parts

    async {
        while true do
            let! events = Async.AwaitEvent(applicationService.eventStream.Publish)
            for event in events do
                do! talkAboutEvent bot applicationService.queries event
    } |> Async.Start
        

    return bot
}