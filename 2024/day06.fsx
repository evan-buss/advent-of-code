let puzzleFile =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let parsePuzzle puzzleFile =
    System.IO.File.ReadAllLines puzzleFile
    |> Array.map (fun line -> Seq.map (fun c -> c) line |> Seq.toArray)

let puzzleMap puzzleArray =
    puzzleArray
    |> Array.mapi (fun r row -> row |> Array.mapi (fun c cell -> ((r, c), cell)))
    |> Array.concat
    |> Map.ofArray

type Direction =
    | Up
    | Down
    | Left
    | Right

type Coordinate = int * int

type Guard =
    { Direction: Direction
      Position: Coordinate }

module Guard =

    // create from character and position
    let create c pos =
        { Direction =
            match c with
            | '^' -> Up
            | 'V' -> Down
            | '<' -> Left
            | '>' -> Right
            | _ -> failwith "Invalid guard character"
          Position = pos }

    let private nextPosition guard =
        let delta =
            match guard.Direction with
            | Up -> (-1, 0)
            | Down -> (1, 0)
            | Left -> (0, -1)
            | Right -> (0, 1)

        fst guard.Position + fst delta, snd guard.Position + snd delta

    let private rotate guard =
        let newDirection =
            match guard.Direction with
            | Up -> Right
            | Right -> Down
            | Down -> Left
            | Left -> Up

        { guard with Direction = newDirection }


    // a move is either a rotation or a position change based on next position
    let move guard map =
        match map |> Map.tryFind (nextPosition guard) with
        | Some(c) when c = '#' || c = 'O' -> rotate guard
        | _ ->
            { guard with
                Position = nextPosition guard }

let findGuard map =
    map
    |> Map.pick (fun pos c ->
        match c with
        | c when c <> '.' && c <> '#' -> Some(Guard.create c pos)
        | _ -> None)


let part1 () =
    let map = puzzleFile |> parsePuzzle |> puzzleMap
    let guard = findGuard map

    let visited = Set.empty<Coordinate>

    let rec move map visited (guard: Guard) =
        match Map.tryFind guard.Position map with
        | None -> visited
        | Some _ -> move map (Set.add guard.Position visited) (Guard.move guard map)

    move map visited guard

let part2 coords =
    let map = puzzleFile |> parsePuzzle |> puzzleMap
    let guard = findGuard map

    let rec move map rotations guard =
        match Map.tryFind guard.Position map with
        | None -> false // off map
        | Some _ ->
            let movedGuard = Guard.move guard map

            match Set.contains movedGuard rotations with
            | true -> true
            | false ->
                match movedGuard.Direction = guard.Direction with
                | true -> move map rotations movedGuard
                | false -> move map (Set.add movedGuard rotations) movedGuard

    // loop is detected with a set that stores the guard state.
    // if we get a duplicate guard (same position and direction), we are in a loop
    // only store rotations occurs to save memory

    coords
    |> Array.Parallel.filter (fun coord -> move (Map.add coord 'O' map) Set.empty guard)
    |> Array.length


let coords = part1 ()

printfn $"Part 1: {coords |> Set.count}"
printfn $"Part 2: {coords |> Set.toArray |> part2}"
