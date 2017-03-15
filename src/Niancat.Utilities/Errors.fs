module Niancat.Utilities.Errors

type Result<'result, 'error> =
| Success of 'result
| Failure of 'error

let ok = Success
let fail = Failure

module Failable =
    let bind f = function
        | Success result -> f result
        | Failure error -> Failure error

type SafeBuilder() =
    member this.Bind(s, f) = Failable.bind f s

    member this.Return(x) = Success x

let safely = SafeBuilder()
