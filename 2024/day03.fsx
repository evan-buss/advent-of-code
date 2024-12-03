open System.IO
open System.Text.RegularExpressions

let puzzleFile =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let part1 line =
    Regex(@"mul\((\d{1,3}),(\d{1,3})\)").Matches line
    |> Seq.sumBy (fun m -> int m.Groups.[1].Value * int m.Groups.[2].Value)

type Command =
    | Do
    | Dont
    | Mul of int * int

type Status =
    | On
    | Off

let part2 line =
    Regex(@"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)").Matches line
    |> Seq.map (fun v ->
        match v.Groups.[0].Value with
        | "do()" -> Do
        | "don't()" -> Dont
        | _ -> Mul(int v.Groups.[1].Value, int v.Groups.[2].Value))
    |> Seq.fold
        (fun acc v ->
            let acc, status = acc

            match v with
            | Do -> (acc, On)
            | Dont -> (acc, Off)
            | Mul(a, b) when status = On -> (acc + a * b, status)
            | Mul _ -> (acc, status))
        (0, On)
    |> fst

let line = puzzleFile |> File.ReadAllText

printfn $"Part 1: {line |> part1}"
printfn $"Part 2: {line |> part2}"
