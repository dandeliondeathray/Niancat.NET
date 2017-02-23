namespace Niancat.Api

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Writers

module Serialization =

    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization

    let serializerSettings = new JsonSerializerSettings(ContractResolver = new CamelCasePropertyNamesContractResolver())

    let serialize v = JsonConvert.SerializeObject(v, serializerSettings)

    let toJson v = (serialize >> OK) v >=> setMimeType "application/json"
 

module WebParts =

    open Serialization

    let words wordlist = toJson (Niancat.Core.Solution.wordlist wordlist)

    let app wordlist =
        choose [
            GET >=> path "/" >=> OK "hello, world!"
            GET >=> path "/words" >=> warbler (fun _ -> words wordlist)
        ]
