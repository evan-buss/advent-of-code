let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let puzzle file =
    System.IO.File.ReadAllText file |> fun s -> s.Split(" ") |> Array.map uint64

let (|EvenDigits|_|) number =
    let s = string number

    if s.Length % 2 = 0 then
        let mid = s.Length / 2
        Some([| uint64 s[.. mid - 1]; uint64 s[mid..] |])
    else
        None

let applyRule =
    function
    | 0UL -> [| 1UL |]
    | EvenDigits stones -> stones
    | number -> [| number * 2024UL |]

let batchRules stones =
    stones
    |> Map.fold
        (fun state key value ->
            applyRule key
            |> Array.fold
                (fun m newKey ->
                    Map.change
                        newKey
                        (function
                        | None -> Some(value)
                        | Some existing -> Some(existing + value))
                        m)
                state)
        Map.empty

let rec blink iterations stones =
    match iterations with
    | 0 -> stones
    | n -> blink (n - 1) (batchRules stones)

let stones =
    puzzle file
    |> Array.groupBy id
    |> Array.map (fun (k, v) -> (k, v |> Array.length |> uint64))
    |> Map.ofArray

let part1 = blink 25 stones
printfn $"Part 1: {part1 |> Map.values |> Seq.sum}"

let part2 = blink 75 stones
printfn $"Part 2: {part2 |> Map.values |> Seq.sum}"
