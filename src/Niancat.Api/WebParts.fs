namespace Niancat.Api

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Suave.Writers

module WebParts =

    open Niancat.Core
    open Niancat.Core.Application
    open Niancat.Core.Types
    open Niancat.Core.Events
    open Niancat.Core.Commands
    open Niancat.Utilities

    let private json v = Json.serialize v |> OK >=> setMimeType "application/json"

    let private fromBody<'a> (req : HttpRequest) =
        let body = req.rawForm |> System.Text.Encoding.UTF8.GetString
        Json.deserialize<'a> body

    let private getWords app ctx = async {
        let! allWords = app.getWordlist ()
        return! json allWords ctx
    }


    type SetProblemData = { user : string; letters : string }
    let private setProblem app req = async {
        let data = fromBody<SetProblemData> req
        let cmd = SetProblem (Solution.normalize data.letters)
        let! event = app.sendCommand data.user cmd
                
        return 
            match event with
            | NewProblemSet { user = user; letters = letters } -> OK (sprintf "user %s set the problem to %s!" user letters)
            | _ -> BAD_REQUEST "unknown event data"   
    }

    let niancat (app : App) =
        choose [
            GET >=> path "/" >=> OK "hello, world!"
            GET >=> path "/words" >=> warbler (fun _ -> getWords app)
            POST >=> path "/problem" >=> request (setProblem app)
        ]
