let file = fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "day12.txt"

let puzzle file =
    System.IO.File.ReadAllLines file
    |> Array.mapi (fun row line -> Seq.map (fun c -> c) line |> Seq.mapi (fun col cell -> ((row, col), cell)))
    |> Seq.concat

let groups =
    puzzle file |> Seq.groupBy snd |> Seq.map (fun (k, v) -> (k, v |> Seq.map fst))

(*
    1. Get a set containing positions of the same plant (letter)
    2. Divide first (seed) and remaining positions from set
    3. Check if any of the remaining positions are l/r/u/d from first.
    4. For each of those positions recurse and the same as above until no more remaining
*)

let getNeighbors (row, col) =
    [ (row + 1, col); (row - 1, col); (row, col + 1); (row, col - 1) ]

let fill items =
    let rec fill' todo acc visited =
        match todo with
        | [] -> acc
        | pos :: rest ->
            if Set.contains pos visited then
                fill' rest acc visited
            else if not (Set.contains pos items) then
                fill' rest acc visited
            else
                fill' (rest @ getNeighbors pos) (pos :: acc) (Set.add pos visited)

    fill' [ Set.minElement items ] [] Set.empty

let partition work =
    let rec partition' acc work =
        match Set.count work with
        | 0 -> acc
        | _ ->
            let next = fill work
            let remaining = Set.difference work (Set.ofList next)
            partition' (next :: acc) remaining

    partition' [] work

let area region = Seq.length region

let perimeter region =
    region
    |> Seq.sumBy (fun pos ->
        getNeighbors pos
        |> Seq.filter (fun next -> not <| Set.contains next region)
        |> Seq.length)

let part1 =
    groups
    |> Seq.map (fun (_, positions) ->
        positions
        |> Set.ofSeq
        |> partition
        |> Seq.map (fun region -> area region * perimeter (Set.ofList region)))
    |> Seq.concat
    |> Seq.sum

printfn "Part 1: %A" part1
