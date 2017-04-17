module Niancat.Api.QueryApi

open Suave
open Suave.Filters
open Suave.Operators

open JsonFormatters

open Niancat.Persistence.Queries

let handleQueryRequest getModel wp (context : HttpContext) = async {
    let! model = getModel ()
    return! wp model context
}

let queryApiHandler queries =
    GET >=> choose [
        path "/problem" >=> handleQueryRequest queries.problem.getCurrent problemAsJson
    ]
