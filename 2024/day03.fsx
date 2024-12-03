open System.IO
open System.Text.RegularExpressions

let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let line = file |> File.ReadAllLines |> String.concat ""

let part1 =
    let parser = Regex(@"mul\((\d{1,3}),(\d{1,3})\)")

    parser.Matches line
    |> Seq.map (fun m -> int m.Groups.[1].Value, int m.Groups.[2].Value)
    |> Seq.fold (fun acc (a, b) -> acc + a * b) 0

type Command =
    | Do
    | Dont
    | Mul of int * int

type Status =
    | On
    | Off

let part2 =
    let parser = Regex(@"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)")

    parser.Matches line
    |> Seq.map (fun v ->
        match v.Groups.[0].Value with
        | "do()" -> Do
        | "don't()" -> Dont
        | _ -> Mul(int v.Groups.[1].Value, int v.Groups.[2].Value))
    |> Seq.scan
        (fun acc v ->
            let (acc, status) = acc

            match v with
            | Mul(a, b) when status = On -> (acc + a * b, status)
            | Mul _ -> (acc, status) // Off don't do anything
            | Dont -> (acc, Off)
            | Do -> (acc, On))
        (0, On)
    |> Seq.last
    |> fst

printfn $"Part 1: %A{part1}"
printfn $"Part 2: %A{part2}"
