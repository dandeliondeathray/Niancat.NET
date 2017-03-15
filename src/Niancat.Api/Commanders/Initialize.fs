module Niancat.Api.Commanders.Initialize

open Niancat.Core.Domain
open System
open System.IO

open Niancat.Api.CommandHandlers
open Niancat.Core.Commands

let validate wordlistFile = async {
    if File.Exists wordlistFile
    then
        use stream = File.OpenRead wordlistFile
        let! contents = stream.AsyncRead(int stream.Length)

        let text = System.Text.Encoding.UTF8.GetString contents

        return Choice1Of2 text
    else
        return Choice2Of2 "Wordlist file does not exist"        
}

let toCommand (wordlistFileContents : string) =
    let wordlist =
        wordlistFileContents.Split(System.Environment.NewLine.ToCharArray())
        |> List.ofArray
        |> wordlist
    
    Initialize wordlist

let initializeCommander = {
    validate = validate
    toCommand = toCommand
}