open System.Text.RegularExpressions

let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

type Input = { X: decimal; Y: decimal }

let parseButton line =
    let m = Regex(@"X\+(\d+), Y\+(\d+)").Match line

    { X = decimal m.Groups[1].Value
      Y = decimal m.Groups[2].Value }

let parsePrize line =
    let m = Regex(@"X=(\d+), Y=(\d+)").Match line

    { X = decimal m.Groups[1].Value
      Y = decimal m.Groups[2].Value }

let puzzle file =
    System.IO.File.ReadAllLines file
    |> Array.chunkBySize 4
    |> Array.map (fun chunk -> [| parseButton chunk[0]; parseButton chunk[1]; parsePrize chunk[2] |])

let solve (equation: Input array) =
    let a = equation[0]
    let b = equation[1]
    let p = equation[2]
    let sX = (p.X * b.Y - p.Y * b.X) / (a.X * b.Y - a.Y * b.X)
    let sY = (p.Y * a.X - a.Y * p.X) / (a.X * b.Y - a.Y * b.X)
    sX, sY

let valid (a, b) =
    System.Decimal.IsInteger(a) && System.Decimal.IsInteger(b)

let tokens (a, b) = (3M * a) + b

let part1 =
    puzzle file |> Array.map solve |> Array.filter valid |> Array.sumBy tokens

let part2 =
    puzzle file
    |> Array.map (fun i ->
        i[2] <-
            { X = i[2].X + 10000000000000M
              Y = i[2].Y + 10000000000000M }

        i)
    |> Array.map solve
    |> Array.filter valid
    |> Array.sumBy tokens

printfn $"Part 1: {part1}"
printfn $"Part 2: {part2}"
