namespace Niancat.Core.Tests

open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote
open System.Text.RegularExpressions

open Niancat.Utilities
open Niancat.Core.Domain
open Generators

module DomainTests =

    let private unpack (Key s) = s

    let private proveIdempotence f w =
        unpack (key w) =! (unpack >> f) (key w)

    [<Property(Arbitrary=[|typeof<NonNullDomainStrings>|])>]
    let ``Solution.key sorts alphabetically`` word =
        proveIdempotence String.sortChars word

    [<Property(Arbitrary=[|typeof<NonNullDomainStrings>|])>]
    let ``Solution.key is uppercase`` word =
        proveIdempotence String.toUpper word

    [<Property(Arbitrary=[|typeof<NonNullDomainStrings>|])>]
    let ``Solution.key trims whitespace`` word =
        proveIdempotence String.trim word

    [<Property(Arbitrary=[|typeof<NonNullDomainStrings>|])>]
    let ``Solution.normalize removes all illegal characters`` word =
        let s = (key >> unpack) word
        Regex.IsMatch(normalize s, "^[A-ZÅÄÖ]*$") =! true

    [<Fact>]
    let ``Solution.normalize handles Swedish characters correctly`` () =
        normalize "åäöÅÄÖéèÉÈ" =! "ÅÄÖÅÄÖEEEE"

    [<Fact>]
    let ``Solution.hash uses the correct algorithm`` () =
        let expected = Hash "2831cdcd01d9dce3bc39652bd3ed73bfd901e97341245b06a8a33ce3c45d345c" // echo -n FOOBARBAZtlycken | sha256sum
        hash (Guess "fooBarBaz") (User "Tlycken") =! expected

    [<Property(Arbitrary=[|typeof<NonNullDomainStrings>|])>]
    let ``No word is valid with an empty wordlist`` word =
        let wordlist = Map.empty<Key, Word list>

        isWord wordlist word =! false

    [<Theory>]
    [<InlineData("foo", true)>]
    [<InlineData("foob", false)>]
    [<InlineData("foobar", true)>]
    let ``Words in wordlist are words`` word shouldMatch =
        let wordlist = wordlist ["foo"; "bar"; "foobar"]

        isWord wordlist (Guess word) =! shouldMatch
