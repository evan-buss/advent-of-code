let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let puzzle file =
    System.IO.File.ReadAllLines file
    |> Array.mapi (fun row line ->
        Seq.map (fun c -> c) line
        |> Seq.mapi (fun col cell -> ((row, col), System.Char.GetNumericValue cell |> int)))
    |> Seq.concat
    |> Seq.toArray
    |> fun tiles ->
        let heads = tiles |> Seq.filter (fun (_, c) -> c = 0) |> Seq.map fst
        let map = tiles |> Map.ofSeq
        (heads, map)

let getNeighbors (row, col) =
    [| (row + 1, col); (row - 1, col); (row, col + 1); (row, col - 1) |]

let rec hike map pos visited =
    match Set.contains pos visited with
    | true -> []
    | false ->
        match Map.tryFind pos map with
        | None -> []
        | Some height ->
            if height = 9 then
                [ pos ]
            else
                getNeighbors pos
                |> Array.choose (fun next ->
                    Map.tryFind next map
                    |> Option.filter (fun nextHeight -> nextHeight = height + 1)
                    |> Option.map (fun _ -> hike map next (Set.add pos visited)))
                |> List.concat

let heads, map = puzzle file

let part1 =
    heads
    |> Seq.sumBy (fun pos -> hike map pos Set.empty |> Set.ofList |> Set.count)

printfn $"Part 1: %A{part1}"

let part2 = heads |> Seq.sumBy (fun pos -> hike map pos Set.empty |> List.length)

printfn $"Part 2: %A{part2}"
