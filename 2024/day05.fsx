let puzzleFile =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let parts =
    System.IO.File.ReadAllLines puzzleFile
    |> Array.partition (fun line -> line.Contains("|"))

let rules =
    parts
    |> fst
    |> Array.map (fun line ->
        let parts = line.Split("|")
        (int parts[0], int parts[1]))
    |> Set.ofArray

let pages =
    parts
    |> snd
    |> Array.skip (1)
    |> Array.map (fun line -> line.Split(",") |> Array.map int)

let comparer a b =
    if Set.contains (a, b) rules then -1
    else if Set.contains (b, a) rules then 1
    else 0

let isValid instructions =
    instructions |> Seq.pairwise |> Seq.forall (fun (a, b) -> comparer a b < 0)

let middleElement (arr: int array) = arr[arr.Length / 2]

let part1 = pages |> Array.filter isValid |> Array.sumBy (middleElement)

let part2 =
    pages
    |> Array.filter (isValid >> not)
    |> Array.map (Array.sortWith comparer)
    |> Array.sumBy (middleElement)


printfn $"Part 1: {part1}"
printfn $"Part 2: {part2}"
