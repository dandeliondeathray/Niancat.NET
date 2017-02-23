open Suave
open Suave.Successful

open Niancat.Api


let readFile = System.IO.File.ReadAllLines >> List.ofArray >> List.filter (String.isEmpty >> not)

let runServer words =
    startWebServer defaultConfig (WebParts.app words)

[<EntryPoint>]
let main argv =

    match List.ofArray argv with
    | wordlistFile :: _ ->  runServer (readFile wordlistFile)
    | _ -> printfn "Usage: Niancat.Api.exe path-to-saol.txt"

    0 // return an integer exit code
