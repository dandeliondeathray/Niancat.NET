namespace Niancat.Persistence

module FileDb =

    open Niancat.Core
    open Niancat.Core.Application
    open Niancat.Core.Events
    open Niancat.Utilities
    open Niancat.Utilities.RegEx

    let readWordlist wordlistFile () = async {
        return wordlistFile |> File.readAllLines |> Solution.wordlist
    }

    let private deserialize = function
        | Match @"NewProblemSet (.*)" [json] -> Some (NewProblemSet (Json.deserialize<NewProblemData> json))
        | Match @"Solved (.*)" [json] -> Some (Solved (Json.deserialize<SolvedData> json))
        | line ->
            printfn "%s: Could not parse line as a domain event: '%s" (System.DateTime.UtcNow.ToString("O")) line
            None

    let private serialize = function
        | NewProblemSet data -> sprintf "NewProblemSet %s" (Json.serialize data)
        | _ -> ""

    let readEvents eventsFile () = async {
        return eventsFile
        |> File.readAllLines
        |> List.choose deserialize
    }

    let saveEvent eventsFile event = async {
        do serialize event |> File.appendLine eventsFile
    }

    let init wordlistFile eventsFile =
        {
            wordlist = readWordlist wordlistFile
            events = readEvents eventsFile
            saveEvent = saveEvent eventsFile
        }
