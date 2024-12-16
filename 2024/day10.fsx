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

let matrix = [| (1, 0); (-1, 0); (0, 1); (0, -1) |]

let validHeight curr next =
    match Map.tryFind next map with
    | Some nextHeight when nextHeight > curr && (abs (nextHeight - curr) = 1) -> true
    | _ -> false

let hike pos =
    let rec hike' pos visited summits =
        match Set.contains pos visited with
        | true -> summits
        | false ->
            match Map.tryFind pos map with
            | None -> summits // invalid position
            | Some n ->
                if n = 9 then
                    Set.add pos summits
                else
                    let visitedU = Set.add pos visited
                    let row, col = pos

                    let next =
                        matrix
                        |> Seq.map (fun (dR, dC) -> (row + dR, col + dC))
                        |> Seq.filter (fun next -> validHeight n next)
                        |> Seq.map (fun next -> hike' next visitedU summits)

                    Set.unionMany next

    hike' pos Set.empty Set.empty

let part1 = trails |> Seq.map (fun c -> hike (c |> fst) |> Set.count) |> Seq.sum

printfn "Part 1: %A" part1
