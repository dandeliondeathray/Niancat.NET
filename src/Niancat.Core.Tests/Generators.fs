module Generators

open FsCheck
open Niancat.Core.Domain

type NonNullDomainStrings =

    static member Word () =
        Arb.Default.Derive () |> Arb.filter (function Word null -> false | _ -> true)
    
    static member Key () =
        Arb.Default.Derive () |> Arb.filter (function Key null -> false | _ -> true)

    static member User () =
        Arb.Default.Derive () |> Arb.filter (function User null -> false | _ -> true)
    static member Hash () =
        Arb.Default.Derive () |> Arb.filter (function Hash null -> false | _ -> true)
