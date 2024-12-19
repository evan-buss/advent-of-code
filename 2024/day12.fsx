let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let puzzle file =
    System.IO.File.ReadAllLines file
    |> Array.mapi (fun row line -> Seq.map (fun c -> c) line |> Seq.mapi (fun col cell -> ((row, col), cell)))
    |> Seq.concat

let groups =
    puzzle file
    |> Seq.groupBy snd
    |> Seq.map (fun (_, v) -> v |> Seq.map fst |> Set.ofSeq)

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

    fill' [ Set.minElement items ] [] Set.empty |> Set.ofList

/// Partition set of coordinates into list of independent flood filled sets
let partition work =
    let rec partition' acc work =
        match Set.count work with
        | 0 -> acc
        | _ ->
            let next = fill work
            let remaining = Set.difference work next
            partition' (next :: acc) remaining


    partition' [] work

let area region = Seq.length region

let perimeter region =
    region
    |> Seq.sumBy (fun pos ->
        getNeighbors pos
        |> Seq.filter (fun next -> not <| Set.contains next region)
        |> Seq.length)

let corners (row, col) region =
    let positions =
        [| for dR = -1 to 1 do
               for dC = -1 to 1 do
                   ((row + dR, col), (row, col + dC)) |]

    let between (rowV, _) (_, colV) = (rowV, colV)

    let convex v h =
        not <| Set.contains v region && not <| Set.contains h region

    let concave v h =
        (Set.contains v region
         && Set.contains h region
         && not (Set.contains (between v h) region))


    positions |> Seq.filter (fun (v, h) -> concave v h || convex v h) |> Seq.length

let sides region =
    region |> Seq.sumBy (fun pos -> corners pos region)

let part1 =
    groups |> Seq.sumBy (partition >> Seq.sumBy (fun r -> area r * perimeter r))

printfn "Part 1: %A" part1

let part2 = groups |> Seq.sumBy (partition >> Seq.sumBy (fun r -> area r * sides r))
printfn "Part 2: %A" part2
