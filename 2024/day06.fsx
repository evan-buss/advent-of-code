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
                | Some('#') -> this.rotate ()
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
        | None -> Set.count visited
        | Some _ -> move map (Set.add guard.Position visited) (guard.move map)

    move map visited guard

printfn $"Part 1: {part1 ()}"
