module Niancat.Utilities.Program

open System
open System.Threading
open System.Threading.Tasks

let optionalize program cts = async {
    do! program cts
    return Some ()
}

let waitForCtrlC program =
    let cts = new CancellationTokenSource()

    Console.CancelKeyPress.Add(fun _ -> cts.Cancel())

    let cancel =
        printfn "Press Ctrl-C to quit"
        Task
            .Delay(Timeout.Infinite, cts.Token)
            .ContinueWith(fun _ -> Some ())
        |> Async.AwaitTask

    [program cts; cancel]
    |> Async.Choice
    |> Async.Ignore
    |> Async.RunSynchronously
