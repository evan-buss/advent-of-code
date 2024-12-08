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

    type T =
        { Direction: Direction
          Position: int * int }

        member private this.nextPosition() =
            let delta =
                match this.Direction with
                | Up -> (-1, 0)
                | Down -> (1, 0)
                | Left -> (0, -1)
                | Right -> (0, 1)

            fst this.Position + fst delta, snd this.Position + snd delta

        member this.rotate() =
            let newDirection =
                match this.Direction with
                | Up -> Right
                | Right -> Down
                | Down -> Left
                | Left -> Up

            { this with Direction = newDirection }


        member this.move map =
            let updated =
                match map |> Map.tryFind (this.nextPosition ()) with
                | Some(c) when c = '#' || c = 'O' -> this.rotate ()
                | _ -> this

            { updated with
                Position = updated.nextPosition () }



let toDirection c =
    match c with
    | '^' -> Guard.Up
    | 'V' -> Guard.Down
    | '<' -> Guard.Left
    | '>' -> Guard.Right
    | c -> failwith $"invalid direction symbol: {c}"

[<TailCall>]
let part1 () =
    let map = parsePuzzle puzzleFile |> puzzleMap
    let guardPos = Map.findKey (fun _ v -> v <> '.' && v <> '#') map
    let guardDir = Map.find guardPos map |> toDirection

    let guard: Guard.T =
        { Direction = guardDir
          Position = guardPos }

    let visited = Set.empty<(int * int)>

    let rec move map visited (guard: Guard.T) =
        // printfn $"G: %A{guard}"

        match map |> Map.tryFind guard.Position with
        | None -> visited
        | Some _ -> move map (Set.add guard.Position visited) (guard.move map)

    move map visited guard

let part2 () =
    let map = parsePuzzle puzzleFile |> puzzleMap
    let guardPos = Map.findKey (fun _ v -> v <> '.' && v <> '#') map
    let guardDir = Map.find guardPos map |> toDirection

    let guard: Guard.T =
        { Direction = guardDir
          Position = guardPos }

    let rec move map rotations (guard: Guard.T) : bool =
        match map |> Map.tryFind guard.Position with
        | None -> false
        | Some _ ->
            let origDirection = guard.Direction
            let newGuard = guard.move map

            if origDirection <> newGuard.Direction then
                match rotations |> Set.contains guard.Position with
                | true -> true // we looped
                | false -> move map (Set.add guard.Position rotations) newGuard
            else
                move map rotations newGuard

    part1 ()
    |> Set.filter (fun c -> c <> guard.Position)
    |> Seq.filter (fun position ->
        // printfn $"%A{position}"
        // for each position in the traveled path, place an obstacle and see if we get a loop
        let rotations = Set.empty<(int * int)>
        let obstacleMap = map |> Map.add position 'O'
        let output = move obstacleMap rotations guard
        output)
    |> Seq.length

printfn $"Part 1: {part1 () |> Set.count}"
printfn $"Part 2: {part2 ()}"
