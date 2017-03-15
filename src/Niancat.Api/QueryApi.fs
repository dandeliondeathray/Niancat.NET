module Niancat.Api.QueryApi

open Suave

let handleQueryRequest getModel wp (context : HttpContext) = async {
    let! model = getModel ()
    return! wp model context
}
