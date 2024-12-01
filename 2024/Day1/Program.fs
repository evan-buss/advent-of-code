open System
open System.IO

let parseInput file sort =
    let parseLine (line: string) =
        match line.Split(' ', StringSplitOptions.RemoveEmptyEntries) with
        | [| l; r |] -> int l, int r
        | _ -> failwithf $"Invalid input: {line}"

    let l, r = File.ReadAllLines file |> Array.map parseLine |> Array.unzip

    match sort with
    | true -> Array.sort l, Array.sort r
    | false -> l, r

[<EntryPoint>]
let main =
    function
    | [| file |] ->
        let part1 =
            parseInput file true ||> Array.zip |> Array.sumBy (fun (x, y) -> abs (y - x))

        let part2 =
            let l, r = parseInput file false
            let counts = r |> Array.countBy id |> Map

            l
            |> Array.sumBy (fun v ->
                counts
                |> Map.tryFind v
                |> Option.map (fun count -> count * v) // Some v * count
                |> Option.defaultValue 0) // None -> 0

        printfn $"Part 1: %d{part1}"
        printfn $"Part 2: %d{part2}"
        0
    | _ ->
        eprintfn "Usage: dotnet run <input file>"
        1
