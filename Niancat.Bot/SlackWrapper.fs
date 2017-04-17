module Slack

open SlackBotNet
open SlackBotNet.State

type BotPart = {
    matcher : MessageMatcher
    action : Conversation -> Async<unit>
} 

type Slack = {
    send : string -> string -> Async<unit>
}

let reply (conv : Conversation) text = conv.ReplyAsync(text) |> Async.AwaitTask |> Async.Ignore

let config (cfg : ISlackBotConfig) =
    cfg.WhenHandlerMatchMode <- WhenHandlerMatchMode.FirstMatch
    ()

let connect token parts = async {
    let! bot = SlackBot.InitializeAsync(token, (fun cfg -> config(cfg))) |> Async.AwaitTask

    let invoke action conv = async {
        do! action conv
    }

    parts
    |> Seq.iter (fun (part, hubs) ->
        match hubs with
        | Some hubs' -> bot.When(part.matcher, hubs', (fun conv -> Async.Start(invoke part.action conv))) |> ignore
        | None -> bot.When(part.matcher, (fun conv -> Async.Start(invoke part.action conv))) |> ignore)


    let send recipient text =
        let hub = bot.State.GetHub(recipient)
        bot.SendAsync(hub, text) |> Async.AwaitTask

    return {
        send = send
    }
}

module Matchers =
    
    let text text = MatchFactory.Matches.TextContaining text

    let regex pattern = MatchFactory.Matches.Regex pattern

    let message predicate = MatchFactory.Matches.Message predicate

    let anything = MatchFactory.Matches.Message (fun msg -> true)
