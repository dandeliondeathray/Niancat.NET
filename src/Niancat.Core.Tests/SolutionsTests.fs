namespace Niancat.Core.Tests

open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote
open System.Text.RegularExpressions

open Niancat.Utilities
open Niancat.Core.Solution
open Niancat.Core.Types

module SolutionsTests =

    [<Property>]
    let ``Solution.key sorts alphabetically`` (input : NonNull<Word>) =
        (key input.Get) =! String.sortChars (key input.Get)

    [<Property>]
    let ``Solution.key is uppercase`` (input : NonNull<Word>) =
        (key input.Get) =! (key input.Get).ToUpper()

    [<Property>]
    let ``Solution.key trims whitespace`` (input : NonNull<Word>) =
        (key input.Get) =! (key input.Get).Trim()

    [<Property>]
    let ``Solution.normalize removes all illegal characters`` (input : NonNull<Word>) =
        Regex.IsMatch(normalize input.Get, "^[A-ZÅÄÖ]*$") =! true

    [<Fact>]
    let ``Solution.normalize handles Swedish characters correctly`` () =
        normalize "åäöÅÄÖéèÉÈ" =! "ÅÄÖÅÄÖEEEE"

    [<Fact>]
    let ``Solution.hash uses the correct algorithm`` () =
        let expected = "2831cdcd01d9dce3bc39652bd3ed73bfd901e97341245b06a8a33ce3c45d345c" // echo -n FOOBARBAZtlycken | sha256sum
        hash "FOOBARBAZ" "Tlycken" =! expected

    [<Property>]
    let ``No word is valid with an empty wordlist`` (input : NonNull<Word>) =
        let wordlist = Map.empty<string, string list>

        isWord wordlist input.Get =! false

    [<Theory>]
    [<InlineData("foo", true)>]
    [<InlineData("foob", false)>]
    [<InlineData("foobar", true)>]
    let ``Words in wordlist are words`` word shouldMatch =
        let wordlist = wordlist ["foo"; "bar"; "foobar"]

        isWord wordlist word =! shouldMatch
