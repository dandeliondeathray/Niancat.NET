module Niancat.Api.WebParts

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Suave.Writers

open Niancat.Api.CommandApi
open Niancat.Api.CommandHandlers
open Niancat.Persistence.Queries
open Niancat.Persistence.InMemory.EventStore

open Niancat.Utilities
open Niancat.Utilities.Errors

let private json v = Json.serialize v |> OK >=> setMimeType "application/json"

let commandApiHandler eventStore (context : HttpContext) = async {
    let payload = System.Text.Encoding.UTF8.GetString context.request.rawForm

    let! response = handleCommandRequest eventStore payload

    match response with
    | Success (state, events) ->
        return! json events context
    | Failure err -> 
        return! BAD_REQUEST err.Message context
}

let niancat eventStore =
    choose [
        path "/" >=> choose [
            GET >=> OK "hello, world!"
            METHOD_NOT_ALLOWED ""
        ]
        path "/command" >=> choose [
            POST >=> commandApiHandler eventStore
            METHOD_NOT_ALLOWED ""
        ]
        NOT_FOUND ""
    ]
