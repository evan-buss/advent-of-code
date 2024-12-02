open System.IO

let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

type Report = int seq seq
type Level = int seq
type Safety = { GapValid: bool; OrderValid: bool }
type LevelSafety = Safety seq

let input file : Report =
    file |> File.ReadLines |> Seq.map (fun l -> l.Split(" ") |> Seq.map int)

let computeSafety (level: Level) =
    let differences = level |> Seq.pairwise |> Seq.map (fun (prev, curr) -> curr - prev)

    let validGap = differences |> Seq.map (fun d -> abs d >= 1 && abs d <= 3)

    let validOrder =
        differences
        |> Seq.pairwise
        |> Seq.map (fun (prev, curr) -> sign prev = sign curr)
        |> Seq.append [ true ]

    // the first item is always in order and pairwise outputs seq length - 1 items, so we fill the gap so the zip works
    Seq.zip validGap validOrder
    |> Seq.map (fun (gap, order) -> { GapValid = gap; OrderValid = order })

let isSafe (safety: LevelSafety) =
    safety |> Seq.forall (fun safety -> safety.GapValid && safety.OrderValid)

let filterUnsafe (safety: LevelSafety) index : LevelSafety =
    safety |> Seq.indexed |> Seq.filter (fun (i, _) -> i <> index) |> Seq.map snd

let dampener safety =
    let errorIndexes =
        safety
        |> Seq.indexed
        |> Seq.filter (fun (_, (safety)) -> not safety.GapValid || not safety.OrderValid)
        |> Seq.map fst

    if Seq.isEmpty errorIndexes then
        Seq.ofList [ safety ]
    else
        errorIndexes |> Seq.map (filterUnsafe safety)

let part1 (input: Report) =
    input |> Seq.filter (computeSafety >> isSafe) |> Seq.length

let part2 (input: Report) =
    input
    |> Seq.filter (computeSafety >> dampener >> Seq.exists isSafe)
    |> Seq.length

let lines = input file |> Seq.toList

printfn $"Part 1: {lines |> part1}"
printfn $"Part 2: {lines |> part2}"
