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


module Guard =
    type Direction =
        | Up
        | Down
        | Left
        | Right

    type Coordinate = int * int

    type T =
        { Direction: Direction
          Position: Coordinate }

        // create from character and position
        static member create c pos =
            { Direction =
                match c with
                | '^' -> Up
                | 'V' -> Down
                | '<' -> Left
                | '>' -> Right
                | _ -> failwith "Invalid guard character"
              Position = pos }

        member private this.nextPosition() =
            let delta =
                match this.Direction with
                | Up -> (-1, 0)
                | Down -> (1, 0)
                | Left -> (0, -1)
                | Right -> (0, 1)

            fst this.Position + fst delta, snd this.Position + snd delta

        member private this.rotate() =
            let newDirection =
                match this.Direction with
                | Up -> Right
                | Right -> Down
                | Down -> Left
                | Left -> Up

            { this with Direction = newDirection }


        // a move is either a rotation or a position change based on next position
        member this.move map =
            match map |> Map.tryFind (this.nextPosition ()) with
            | Some(c) when c = '#' || c = 'O' -> this.rotate ()
            | _ ->
                { this with
                    Position = this.nextPosition () }

let findGuard map =
    map
    |> Map.pick (fun pos c ->
        match c with
        | c when c <> '.' && c <> '#' -> Some(Guard.T.create c pos)
        | _ -> None)


let part1 () =
    let map = parsePuzzle puzzleFile |> puzzleMap
    let guard = findGuard map

    let visited = Set.empty<(int * int)>

    let rec move map visited (guard: Guard.T) =
        match map |> Map.tryFind guard.Position with
        | None -> visited
        | Some _ -> move map (Set.add guard.Position visited) (guard.move map)

    move map visited guard

let part2 () =
    let map = parsePuzzle puzzleFile |> puzzleMap
    let guard = findGuard map

    let rec move map rotations (guard: Guard.T) =
        match map |> Map.tryFind guard.Position with
        | None -> false // off map
        | Some _ ->
            let movedGuard = guard.move map

            match rotations |> Set.contains movedGuard with
            | true -> true
            | false -> move map (rotations |> Set.add movedGuard) movedGuard

    // loop is detected with a set that stores the guard state.
    // if we get a duplicate guard (same position and direction), we are in a loop

    part1 ()
    |> Set.toArray
    |> Array.Parallel.filter (fun position -> move (map |> Map.add position 'O') Set.empty<Guard.T> guard)
    |> Seq.length

printfn $"Part 1: {part1 () |> Set.count}"
printfn $"Part 2: {part2 ()}"
