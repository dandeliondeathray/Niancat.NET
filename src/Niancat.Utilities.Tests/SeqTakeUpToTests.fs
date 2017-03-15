module Seq.TakeUpTo.Tests

open FsCheck.Xunit
open Swensen.Unquote

open Niancat.Utilities

[<Property>]
let ``Seq.takeUpTo 0 returns empty sequence`` (s : int list) =
    Seq.takeUpTo 0 s |> List.ofSeq =! List.empty

[<Property>]
let ``Seq.takeUpTo n returns at most n elements`` (s : int list) n =
    let result = Seq.takeUpTo (abs n) s

    Seq.length result <=! abs n

[<Property>]
let ``Seq.takeUpTo n with seq of n elements returns entire sequence`` (s : int list) =
    let n = Seq.length s
    let result = Seq.takeUpTo n s

    result |> List.ofSeq =! s

[<Property>]
let ``Seq.takeUpTo n with seq of less than n elements returns entire sequence`` (s : int list) =
    let n = Seq.length s + 3
    let result = Seq.takeUpTo n s

    result |> List.ofSeq =! s
