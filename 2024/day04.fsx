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

let safeGet (r, c) (map: Map<(int * int), char>) =
    match Map.tryFind (r, c) map with
    | Some cell -> cell
    | None -> '.'

let part1 =
    let arr = parsePuzzle puzzleFile
    let map = puzzleMap arr

    let directions =
        [| for x = -1 to 1 do
               for y = -1 to 1 do
                   if x <> 0 || y <> 0 then
                       yield (x, y) |]

    let letterSequence =
        [| 'X', Some('M'); 'M', Some('A'); 'A', Some('S'); 'S', None |] |> Map.ofArray


    let rec findWord (r, c) (dx, dy) (letter: char option) =
        match letter with
        | None -> true // Found the word
        | Some(nextLetter) ->
            if safeGet (r, c) map = nextLetter then // Next letter matches
                findWord (r + dx, c + dy) (dx, dy) letterSequence.[nextLetter]
            else
                false

    map
    |> Map.fold
        (fun acc (r, c) cell ->
            match cell with
            | 'X' ->
                directions
                |> Array.fold
                    (fun acc (dx, dy) ->
                        if findWord (r + dx, c + dy) (dx, dy) (letterSequence.['X']) then
                            acc + 1
                        else
                            acc)
                    acc
            | _ -> acc)
        0

let part2 =
    let arr = parsePuzzle puzzleFile
    let map = puzzleMap arr
    let pair = [| 'M'; 'S' |] |> Set.ofArray

    map
    |> Map.fold
        (fun acc (r, c) cell ->
            match cell with
            | 'A' ->
                let tL = safeGet (r - 1, c - 1) map
                let tR = safeGet (r - 1, c + 1) map
                let bL = safeGet (r + 1, c - 1) map
                let bR = safeGet (r + 1, c + 1) map

                if
                    (tL <> bR && Set.contains tL pair && Set.contains bR pair)
                    && (tR <> bL && Set.contains tR pair && Set.contains bL pair)
                then
                    acc + 1
                else
                    acc

            | _ -> acc)
        0


printfn $"Part 1: {part1}"
printfn $"Part 2: {part2}"
