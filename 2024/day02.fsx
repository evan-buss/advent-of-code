open System.IO

let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

type Report = int seq seq
type Level = int seq
type LevelSafety = bool seq

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

    Seq.zip validGap validOrder |> Seq.map (fun (gap, order) -> gap && order)

let isSafe (safety: LevelSafety) = safety |> Seq.forall id

let dampener safety =
    match safety |> isSafe with
    | true -> true
    | false ->
        let unsafeIndices =
            safety |> Seq.indexed |> Seq.filter (fun (_, s) -> not s) |> Seq.map fst

        // check if any of the unsafe indices can be removed and make the level safe
        unsafeIndices
        |> Seq.exists (fun unsafeI ->
            safety
            |> Seq.indexed
            |> Seq.filter (fun (i, _) -> i <> unsafeI)
            |> Seq.map snd
            |> isSafe)

let part1 (input: Report) =
    input |> Seq.filter (computeSafety >> isSafe) |> Seq.length

let part2 (input: Report) =
    input |> Seq.filter (computeSafety >> dampener) |> Seq.length

let lines = input file |> Seq.toList

printfn $"Part 1: {lines |> part1}"
printfn $"Part 2: {lines |> part2}"
