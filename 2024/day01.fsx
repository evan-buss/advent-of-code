open System
open System.IO

let file =
    match fsi.CommandLineArgs with
    | [| _; file |] -> file
    | _ -> "sample.txt"

let input file =
    file
    |> File.ReadAllLines
    |> Array.map _.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line -> int line[0], int line[1])
    |> Array.unzip

let part1 left right =
    Array.zip (Array.sort left) (Array.sort right)
    |> Array.sumBy (fun (x, y) -> abs (y - x))

let part2 left right =
    let counts = right |> Array.countBy id |> Map

    left
    |> Array.sumBy (fun v ->
        counts
        |> Map.tryFind v
        |> Option.map (fun count -> count * v) // Some v * count
        |> Option.defaultValue 0) // None -> 0

let puzzle = input file

printfn $"Part 1: {puzzle ||> part1}"
printfn $"Part 2: {puzzle ||> part2}"
