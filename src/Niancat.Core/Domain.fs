module Niancat.Core.Domain

type Word = Word of string

type Key = Key of string

type User = User of string

type Hash = Hash of string

type Wordlist = Map<Key, Word list>

open Niancat.Utilities

let normalize =
    String.removeControlChars
    >> (String.replace "é|è|É|È" "E")
    >> (String.remove "[^A-Za-zÅÄÖåäö]")
    >> String.toUpper

let key (Word w) = w |> normalize |> String.sortChars |> Key

let wordlist : string list -> Wordlist =
    Seq.map Word
    >> Seq.groupBy key
    >> Seq.map (fun (k, w) -> k, List.ofSeq w)
    >> Map.ofSeq

let isWord (wordlist : Wordlist) word =
    match Map.tryFind (key word) wordlist with
    | Some words -> List.exists (fun w -> w = word) words
    | None -> false

let hash (Word guess) (User user) =
    sprintf "%s%s" (guess.ToUpper()) (user.ToLower()) |> String.sha256 |> Hash

let prettyProblem (Word word) =
    if String.length word = 9
    then
        word |> String.asChars |> Seq.inChunksOf 3 |> Seq.map String.ofChars |> String.concat " "
    else word
