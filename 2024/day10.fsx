let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let puzzle file =
    System.IO.File.ReadAllLines file
    |> Array.mapi (fun row line ->
        Seq.map (fun c -> c) line
        |> Seq.mapi (fun col cell -> ((row, col), System.Char.GetNumericValue cell |> int)))
    |> Seq.concat
    |> Seq.toArray
    |> (fun tiles -> (tiles |> Seq.filter (fun (_, c) -> c = 0), tiles |> Map.ofSeq))

let trails, map = puzzle file

let hike2 pos =
    let matrix = [| (1, 0); (-1, 0); (0, 1); (0, -1) |]

    let rec hike' pos visited summits =
        match Set.contains pos visited with
        | true -> summits
        | false ->
            match Map.tryFind pos map with
            | None -> summits // invalid position
            | Some height ->
                if height = 9 then
                    List.append [ pos ] summits
                else
                    let visitedU = Set.add pos visited
                    let row, col = pos

                    let next =
                        matrix
                        |> Seq.map (fun (dR, dC) -> (row + dR, col + dC))
                        |> Seq.choose (fun next ->
                            Map.tryFind next map
                            |> Option.filter (fun nextHeight -> nextHeight - height = 1)
                            |> Option.map (fun _ -> next))
                        |> Seq.map (fun next -> hike' next visitedU summits)

                    List.concat next

    hike' pos Set.empty List.empty

let part1 =
    trails |> Seq.map fst |> Seq.map (hike2 >> Set.ofSeq >> Seq.length) |> Seq.sum

printfn $"Part 1: %A{part1}"

let part2 = trails |> Seq.map fst |> Seq.map (hike2 >> Seq.length) |> Seq.sum

printfn $"Part 2: %A{part2}"
