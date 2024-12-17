let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let puzzle file =
    System.IO.File.ReadAllText file |> (fun s -> s.Split(" ")) |> Array.map uint64

let (|EvenDigits|_|) number =
    let numString = $"{number}"

    if numString.Length % 2 = 0 then
        let splitAt = numString.Length / 2
        Some([| uint64 numString[.. splitAt - 1]; uint64 numString[splitAt..] |])
    else
        None

let applyRule =
    function
    | 0UL -> [| 1UL |]
    | EvenDigits stones -> stones
    | number -> [| number * 2024UL |]

let rec blink iterations (stones: uint64 array) =
    match iterations with
    | 0 -> stones
    | n -> blink (n - 1) (Array.collect applyRule stones)

let stones = puzzle file

let part1 = blink 25 stones
printfn $"Part 1: %A{part1 |> Array.length}"

let part2 = blink 75 stones
printfn $"Part 2: %A{part2 |> Array.length}"
