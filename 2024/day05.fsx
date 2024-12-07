let puzzleFile =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let parsePuzzle puzzleFile =
    let parts =
        System.IO.File.ReadAllLines puzzleFile
        |> Array.partition (fun line -> line.Contains("|"))

    let rules =
        parts
        |> fst
        |> Array.map (fun line ->
            let parts = line.Split("|")
            (int parts.[0], int parts.[1]))
        |> Array.fold
            (fun acc (before, after) ->
                match acc |> Map.tryFind before with
                | Some(set) -> acc.Add(before, (Set.add after set))
                | None -> acc.Add(before, Set.ofArray [| after |]))
            Map.empty<int, Set<int>>

    let pages =
        parts
        |> snd
        |> Array.skip (1)
        |> Array.map (fun line -> line.Split(",") |> Array.map int)

    rules, pages

let validPage rules page acc =
    match rules |> Map.tryFind page with
    | Some(s) -> not (Set.exists (fun x -> Seq.contains x acc) s)
    | None -> true

let validInstructions rules instructions =
    let output =
        instructions
        |> Array.mapi (fun i page ->
            match validPage rules page instructions[..i] with
            | true -> Some(page)
            | false -> None)
        |> Array.choose id

    output.Length = instructions.Length

let part1 () =
    let rules, pages = parsePuzzle puzzleFile

    pages
    |> Array.filter (validInstructions rules)
    |> Array.sumBy (fun x -> x.[x.Length / 2])

let part2 () =
    let rules, pages = parsePuzzle puzzleFile

    let ruleSorter ruleSet (left: int) (right: int) =
        let leftRules = Map.tryFind left ruleSet
        let rightRules = Map.tryFind right ruleSet

        match leftRules, rightRules with
        | Some(leftSet), Some(rightSet) ->
            if rightSet |> Set.contains left then -1
            elif leftSet |> Set.contains right then 1
            else 0
        | Some(rules), None ->
            match Set.contains right rules with
            | true -> 1
            | false -> 0
        | None, Some(rules) ->
            match Set.contains left rules with
            | true -> -1
            | false -> 0
        | None, None -> 0

    pages
    |> Array.filter (not << validInstructions rules)
    |> Array.map (Array.sortWith (ruleSorter rules))
    |> Array.sumBy (fun x -> x.[x.Length / 2])


printfn $"Part 1: {() |> part1}"
printfn $"Part 2: {() |> part2}"
