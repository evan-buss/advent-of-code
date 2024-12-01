open System
open System.IO

let file =
    match fsi.CommandLineArgs with
    | [| _; file |] -> file
    | _ -> "sample.txt"

let input file =
    file
    |> File.ReadAllLines
    |> Array.map _.Split(' ', StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun parts ->
        match parts with
        | [| l; r |] -> int l, int r
        | _ -> failwithf $"Invalid input line format: %A{parts}")
    |> Array.unzip
    ||> (fun l r -> Array.sort l, Array.sort r)

let part1 left right =
    Array.zip left right |> Array.sumBy (fun (x, y) -> abs (y - x))

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
