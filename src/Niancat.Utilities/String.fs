namespace Niancat.Utilities

module String =

    open System.Text.RegularExpressions

    let asChars (s : string) = s.ToCharArray() |> List.ofArray

    let ofChars (cs : char seq) = String.collect (sprintf "%c") ""

    let toUpper (s : string) = s.ToUpper()

    let trim (s : string) = s.Trim()

    let sortChars = asChars >> Seq.sort >> System.String.Concat

    let isEmpty s = System.String.IsNullOrEmpty(s)

    let private sha256hasher = new System.Security.Cryptography.SHA256Managed()

    let sha256 : string -> string =
        System.Text.Encoding.UTF8.GetBytes
        >> sha256hasher.ComputeHash
        >> Seq.collect (sprintf "%02x")
        >> System.String.Concat

    let replace pattern (replacement : string) s = Regex.Replace(s, pattern, replacement)

    let remove pattern s = Regex.Replace(s, pattern, "")

    let removeControlChars = asChars >> Seq.filter (System.Char.IsControl >> not) >> System.String.Concat
