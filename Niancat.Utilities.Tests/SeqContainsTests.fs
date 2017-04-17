module SeqContainsTests

open FsCheck.Xunit
open Niancat.Utilities
open Swensen.Unquote

[<Property>]
let ``Empty list contains whatever is false`` element =
    let list = List.empty<int>

    list |> Seq.contains element =! false

[<Property>]
let ``List with matching elements filtered away does not contain`` (list : int list) element =
    let list' = List.filter (fun x -> x <> element) list

    list' |> Seq.contains element =! false

[<Property>]
let ``List with element added does contain`` (list : int list) element =
    
    // taken from https://www.rosettacode.org/wiki/Knuth_shuffle#F.23
    let KnuthShuffle (lst : 'a[]) =
        let Swap i j =                                      // Standard swap
            let item = lst.[i]
            lst.[i] <- lst.[j]
            lst.[j] <- item
        let rnd = new System.Random()
        let ln = lst.Length
        [0..(ln - 2)]                                       // For all indices except the last
        |> Seq.iter (fun i -> Swap i (rnd.Next(i, ln)))     // swap th item at the index with a random one following it (or itself)
        lst                                                 // Return the list shuffled in place

    let list' =
        element :: list
        |> Array.ofList
        |> KnuthShuffle
        |> List.ofArray

    list' |> Seq.contains element =! true