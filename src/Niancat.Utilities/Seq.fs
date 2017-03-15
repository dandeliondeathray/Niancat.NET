module Niancat.Utilities.Seq

let rec takeUpTo count sequence = seq {
    if count <= 0 || Seq.isEmpty sequence
    then
        yield! Seq.empty
    else
        yield Seq.head sequence
        yield! takeUpTo (count-1) (sequence |> Seq.skip 1)
}

let skipUpTo count =
    Seq.mapi (fun i t -> i,t)
    >> Seq.skipWhile (fun (i,t) -> i < count)
    >> Seq.map snd

let rec inChunksOf count sequence = seq {
    if Seq.isEmpty sequence
    then
        yield! Seq.empty
    else
        let chunk = sequence |> takeUpTo count |> List.ofSeq
        yield chunk
        yield! (sequence |> skipUpTo count) |> inChunksOf count
}
