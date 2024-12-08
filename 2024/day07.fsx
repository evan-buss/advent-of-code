open System.Collections.Generic

let memoize (f: 'a -> 'b) =
    let cache = Dictionary<'a, 'b>()
    let lockObj = obj ()

    fun x ->
        lock lockObj (fun () ->
            match cache.TryGetValue(x) with
            | true, v -> v
            | false, _ ->
                let v = f x
                cache.Add(x, v)
                v)

let puzzleFile =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let parsePuzzle puzzleFile =
    System.IO.File.ReadAllLines puzzleFile
    |> Array.map (fun line -> line.Split(": ") |> (fun c -> (int64 c[0], (c[1].Split " " |> Array.map int64))))

type Operator =
    | Add
    | Multiply
    | Concat

let eval op a b =
    match op with
    | Add -> a + b
    | Multiply -> a * b
    | Concat -> int64 $"{a}{b}"

let compute operators numbers =
    operators
    |> Array.zip (Array.tail numbers)
    |> Array.fold (fun acc (num, op) -> eval op acc num) (Array.head numbers)


let rec permutations length items =
    if length = 0 then
        [| [||] |]
    elif length = 1 then
        items |> Array.map (fun x -> [| x |])
    else
        items
        |> Array.collect (fun x -> permutations (length - 1) items |> Array.map (fun p -> Array.append [| x |] p))

let memoizedPermutations =
    memoize (fun (length, items) -> permutations length items)

let calibrate operators puzzle =
    puzzle
    |> Array.Parallel.choose (fun (answer: int64, (numbers: int64 array)) ->
        memoizedPermutations (numbers.Length - 1, operators)
        |> Array.tryFind (fun ops -> compute ops numbers = answer)
        |> Option.map (fun _ -> answer))
    |> Array.sum

let part1 puzzle = calibrate [| Add; Multiply |] puzzle

let part2 puzzle =
    calibrate [| Add; Multiply; Concat |] puzzle

let puzzle = puzzleFile |> parsePuzzle

printfn $"Part 1: {part1 puzzle}"
printfn $"Part 2: {part2 puzzle}"
