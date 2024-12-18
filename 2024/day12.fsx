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

[<TailCall>]
let fill pos items visited =

    let rec fill' pos items visited =

        if Set.contains pos visited then
            []
        else if not (Set.contains pos items) then
            []
        else
            pos
            :: (getNeighbors pos
                |> List.filter (fun next -> Set.contains next items)
                |> List.collect (fun next -> fill' next items (Set.add pos visited))
                |> List.distinct) // Not sure why this is needed, but the "B" crop has repeated items

    fill' pos items visited

let rec partition work =
    match Set.count work with
    | 0 -> [] // no more items
    | _ ->
        // printfn "work %A" work
        let next = fill (Set.minElement work) work Set.empty
        // printfn "next %A" next
        let remaining = Set.difference work (next |> Set.ofList)
        // printfn "remaining %A" remaining
        next :: partition remaining


let area region = Seq.length region

let perimeter region =
    region
    |> Seq.sumBy (fun pos ->
        getNeighbors pos
        |> Seq.filter (fun next -> not <| Set.contains next region)
        |> Seq.length)

printfn "%A" groups

let part1 =
    groups
    |> Seq.map (snd >> Array.ofSeq)
    |> Array.ofSeq
    |> Array.Parallel.map (fun crop ->
        crop
        |> Set.ofSeq
        |> partition
        |> Seq.map (fun part ->
            printfn "%A" part
            part)
        |> Seq.map (fun region -> area region * perimeter (Set.ofList region))
        |> Seq.map (fun a ->
            // printfn "%A" a
            a))
    |> Seq.concat
    |> Seq.sum

printfn "Part 1: %A" part1
