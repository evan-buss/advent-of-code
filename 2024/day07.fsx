let puzzleFile =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let parsePuzzle puzzleFile =
    System.IO.File.ReadAllLines puzzleFile
    |> Array.map (fun line -> line.Split(": ") |> (fun c -> (int64 c[0], (c[1].Split " " |> Array.map int64))))

let compute numbers answer concat =

    let rec inner (numbers: int64 array) (curr: int64) i =
        if i = Array.length numbers then
            answer = curr
        else if curr > answer then
            false
        else
            inner numbers (curr + numbers[i]) (i + 1)
            || inner numbers (curr * numbers[i]) (i + 1)
            || (concat && inner numbers (int64 $"{curr}{numbers[i]}") (i + 1))

    inner numbers numbers[0] 1

let calibrate puzzle concat =
    puzzle
    |> Array.sumBy (fun (answer: int64, (numbers: int64 array)) ->
        match compute numbers answer concat with
        | true -> answer
        | false -> 0)

let puzzle = puzzleFile |> parsePuzzle

printfn $"Part 1: {calibrate puzzle false}"
printfn $"Part 2: {calibrate puzzle true}"
