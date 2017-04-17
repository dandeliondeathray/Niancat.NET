open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

[<EntryPoint>]
let main argv = 

    let app =
        choose [
            POST >=> path "/setnian"
        ]

    startWebServer defaultConfig app

    0 // return an integer exit code
