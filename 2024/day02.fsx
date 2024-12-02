open System.IO

let file =
    match fsi.CommandLineArgs with
    | [| _; file |] -> file
    | _ -> "sample.txt"

type Report = int seq seq
type Level = int seq
type LevelSafety = (bool * bool) seq

let input file : Report =
    file |> File.ReadLines |> Seq.map (fun c -> c.Split(" ") |> Seq.map int)

let computeSafety (level: Level) : LevelSafety =
    let diff =
        level
        |> Seq.windowed 2
        |> Seq.map (function
            | [| l; r |] -> r - l
            | _ -> 0)

    let validGap = diff |> Seq.map (fun d -> abs d <= 3 && abs d >= 1)

    let validOrder =
        diff |> Seq.pairwise |> Seq.map (fun (prev, curr) -> sign prev = sign curr)

    // the first item is always in order and pairwise outputs seq length - 1 items, so we fill the gap so the zip works
    Seq.append [ true ] validOrder |> Seq.zip validGap

let isSafe (safety: LevelSafety) =
    safety |> (fun res -> (Seq.forall fst res && Seq.forall snd res))

let filterUnsafe (safety: LevelSafety) index : LevelSafety =
    safety |> Seq.indexed |> Seq.filter (fun (i, _) -> i <> index) |> Seq.map snd

let dampener safety =
    let errorIndexes =
        safety
        |> Seq.indexed
        |> Seq.filter (fun (_, (gap, order)) -> not gap || not order)
        |> Seq.map fst

    if Seq.isEmpty errorIndexes then
        Seq.ofList ([ safety ])
    else
        errorIndexes |> Seq.map (fun i -> filterUnsafe safety i)

let part1 (input: Report) =
    input
    |> Seq.filter (fun report -> report |> computeSafety |> isSafe)
    |> Seq.length

let part2 (input: Report) =
    input
    |> Seq.filter (fun report -> (report |> computeSafety |> dampener |> Seq.exists isSafe))
    |> Seq.length

let lines = input file |> Seq.toList

printfn $"Part 1: {lines |> part1}"
printfn $"Part 2: {lines |> part2}"
