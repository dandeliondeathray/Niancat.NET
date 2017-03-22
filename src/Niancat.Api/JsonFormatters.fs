module Niancat.Api.JsonFormatters

open Suave.Operators
open Suave.Successful
open Suave.Writers

open Niancat.Core.Domain
open Niancat.Core.Events

open Niancat.Utilities
open Niancat.Utilities.Errors

[<AutoOpen>]
module DSL =
    open Newtonsoft.Json
    open Newtonsoft.Json.Linq

    let (.=) key (value : obj) = JProperty(key, value)
    let jobj props =
        let o = JObject()
        props |> List.iter o.Add
        o
    let jarr objs =
        let a = JArray()
        objs |> List.iter a.Add
        a

    let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

    let jtoken (x : string) = JToken.op_Implicit(x)

    let jstr : string -> string = JsonConvert.SerializeObject

let private withJsonMimeType v = (OK v >=> setMimeType "application/json")

let asyncNone _ = async.Return None
let problemAsJson = function
    | Some p -> p |> prettyProblem |> jstr |> withJsonMimeType
    | _ -> asyncNone

let eventAsJson = function
| Initialized wordlist ->
    jobj [
        "event" .= "initialized"
    ]
| ProblemSet (User user, problem) ->
    jobj [
        "event" .= "problem-set"
        "data" .= jobj [
            "problem" .= prettyProblem problem
        ]
    ]
| IncorrectGuess (User user, guess) ->
    jobj [
        "event" .= "incorrect-guess"
        "data" .= jobj [
            "user" .= user
            "guess" .= prettyGuess guess
        ]
    ]
| Solved (User user, Hash hash) ->
    jobj [
        "event" .= "problem-solved"
        "data" .= jobj [
            "user" .= user
            "hash" .= hash
        ]
    ]
| AlreadySolved (User user) ->
    jobj [
        "event" .= "problem-solved-again"
        "data" .= jobj [
            "user" .= user
        ]
    ]