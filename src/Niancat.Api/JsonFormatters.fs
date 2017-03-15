module Niancat.Api.JsonFormatters

open Suave.Operators
open Suave.Successful
open Suave.Writers

open Niancat.Core.Domain

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
    | Some (Word w) -> (jstr w |> withJsonMimeType)
    | _ -> asyncNone
