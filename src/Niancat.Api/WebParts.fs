module Niancat.Api.WebParts

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Suave.Writers

open Niancat.Api.CommandApi
open Niancat.Api.QueryApi
open Niancat.Api.JsonFormatters
open Niancat.Api.CommandHandlers
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
        return! BAD_REQUEST err.Message context
}

let queryApiHandler queries =
    GET
    >=> choose [
        path "/problem" >=> handleQueryRequest queries.problem.getCurrent problemAsJson
    ]

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
        NOT_FOUND ""
    ]
