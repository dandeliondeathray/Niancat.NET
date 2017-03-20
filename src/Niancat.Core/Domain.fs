module Niancat.Core.Domain

type Word = Word of string

type Problem = Problem of string

type Guess = Guess of string

type WordBase =
| W of Word
| P of Problem
| G of Guess

let asW = Word >> W
let asP = Problem >> P
let asG = Guess >> G

type Key = Key of string

type User = User of string

type Hash = Hash of string

type Wordlist = Map<Key, Word list>

open Niancat.Utilities

let private _normalize =
    String.removeControlChars
    >> (String.replace "é|è|É|È" "E")
    >> (String.remove "[^A-Za-zÅÄÖåäö]")
    >> String.toUpper

let normalize (Problem t) = Problem (_normalize t)

let private _key = _normalize >> String.sortChars
let key = function
| W (Word w) | P (Problem w) | G (Guess w) -> w |> _key |> Key

let sameProblem (Problem p) (Problem q) = _key p = _key q

let isValid wordlist (Problem problem) =
    let normalized = _normalize problem
    String.length normalized = 9
    &&
    Map.containsKey (key (P (Problem problem))) wordlist


let wordlist : string list -> Wordlist =
    Seq.groupBy _key
    >> Seq.map (fun (k, w) ->
        Key k,
        List.ofSeq w |> List.map Word)
    >> Map.ofSeq

let isWord (wordlist : Wordlist) (Guess guess) =
    match Map.tryFind (key (G (Guess guess))) wordlist with
    | Some words -> List.exists (fun (Word w) -> w = guess) words
    | None -> false

let hash (Guess guess) (User user) =
    sprintf "%s%s" (guess.ToUpper()) (user.ToLower()) |> String.sha256 |> Hash

let prettyProblem (Problem word) =
    let word' = _normalize word
    if String.length word' = 9
    then
        word' |> String.asChars |> Seq.inChunksOf 3 |> Seq.map String.ofChars |> String.concat " "
    else word'

let prettyGuess (Guess guess) = _normalize guess
