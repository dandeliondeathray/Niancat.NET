module ErrorsTests

open Xunit
open Swensen.Unquote

open Niancat.Utilities.Errors

module TestFunctions =
    let getString () = Success "foo"
    let modifyString s = Success (s + "bar")
    let fail () = Failure "failed"

[<Fact>]
let ``Can use the safely computational expression syntax`` () =
    let result = safely {
        let! foo = TestFunctions.getString ()
        let! bar = TestFunctions.modifyString foo
        return bar
    }

    result =! Success "foobar"

[<Fact>]
let ``Safely propagates errors`` () =
    let result = safely {
        let! foo = TestFunctions.fail ()
        let! bar = TestFunctions.modifyString foo
        return bar
    }

    result =! Failure "failed"

[<Fact>]
let ``Safely does not call functions that come after failure in the chain`` () =
    let mutable called = false

    let impureFunction = fun f -> 
        called <- true
        Success f

    let result = safely {
        let! foo = TestFunctions.fail ()
        let! bar = impureFunction foo
        return bar
    }

    called =! false