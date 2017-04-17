module Seq.InChunksOf.Tests

open FsCheck.Xunit
open Niancat.Utilities
open Swensen.Unquote

let private valid chunkSize = max (abs chunkSize) 1

[<Property>]
let ``Seq.inChunksOf n >> Seq.collect id returns unchanged list`` (s : int list) chunkSize =
    let chunked = Seq.inChunksOf (valid chunkSize) s
    let collected = Seq.collect id chunked
    let result = collected |> List.ofSeq

    s =! result

[<Property>]
let ``Seq.inChunksOf n yields correct number of lists`` (s : int list) chunkSize =
    let n = valid chunkSize
    let chunked = Seq.inChunksOf n s
    let expected =
        if Seq.length s % n = 0
        then Seq.length s / n
        else Seq.length s / n + 1
    
    Seq.length chunked =! expected


[<Property>]
let ``Seq.inChunksOf n yields lists where all but last are exactly n long`` (s : int list) chunkSize =
    let n = valid chunkSize


    let rec checkLength n = function
        | [] -> ()
        | [_] -> ()
        | h::t ->
            Seq.length h =! n
            checkLength n t
    
    s
    |> Seq.inChunksOf n
    |> List.ofSeq
    |> checkLength n
