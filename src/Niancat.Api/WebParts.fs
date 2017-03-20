module Niancat.Api.WebParts

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Suave.Writers
open Suave.WebSocket
open Suave.Sockets
open Suave.Sockets.Control

open Niancat.Api.CommandApi
open Niancat.Api.QueryApi
open Niancat.Api.JsonFormatters
open Niancat.Api.CommandHandlers

open Niancat.Core.Events

open Niancat.Persistence.Queries
open Niancat.Persistence.InMemory.EventStore

open Niancat.Utilities
open Niancat.Utilities.Errors

let commandApiHandler eventStore eventsStream (context : HttpContext) = async {
    let payload = System.Text.Encoding.UTF8.GetString context.request.rawForm

    let! response = handleCommandRequest eventStore eventsStream payload

    match response with
    | Success (state, events) ->
        return! OK "" context
    | Failure err ->
        return! (BAD_REQUEST (jstr err.Message) >=> setMimeType "application/json") context
}

let queryApiHandler queries =
    GET
    >=> choose [
        path "/problem" >=> handleQueryRequest queries.problem.getCurrent problemAsJson
    ]

let publishEventsOnWebsockets (eventsStream : Control.Event<Event list>) (ws : WebSocket) cx = socket {
    while true do
        let! events =
            Control.Async.AwaitEvent(eventsStream.Publish)
            |> Suave.Sockets.SocketOp.ofAsync

        for event in events do
            let eventData =
                event |> eventAsJson |> string |> System.Text.Encoding.UTF8.GetBytes |> ByteSegment

            do! ws.send Text eventData true
}


let niancat eventStore eventsStream =
    choose [
        path "/" >=> choose [
            GET >=> OK "hello, world!"
            METHOD_NOT_ALLOWED ""
        ]
        path "/command" >=> choose [
            POST >=> commandApiHandler eventStore eventsStream
            METHOD_NOT_ALLOWED ""
        ]
        GET >=> queryApiHandler inMemoryQueries
        path "/events" >=>
            handShake (publishEventsOnWebsockets eventsStream)
        NOT_FOUND ""
    ]
