open System.Text.RegularExpressions

let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

type Vector = { X: int; Y: int }

type Robot = { Position: Vector; Velocity: Vector }

module Robot =
    let print (height, width) robots =
        let positions = Seq.countBy (fun r -> r.Position) robots |> Map.ofSeq

        for row = 0 to height - 1 do
            for col = 0 to width - 1 do
                match Map.tryFind { Y = row; X = col } positions with
                | Some(_) -> printf "X"
                | None -> printf " "

            printf "\n"

        printf "\n"

    let private move (height, width) robot =
        let clamp value max =
            match value with
            | v when v < 0 -> max + v
            | v when v >= max -> v - max
            | v -> v

        { robot with
            Position =
                { X = clamp (robot.Position.X + robot.Velocity.X) width
                  Y = clamp (robot.Position.Y + robot.Velocity.Y) height } }

    let rec simulate dimensions robot iterations =
        match iterations with
        | 0 -> robot
        | i -> simulate dimensions (move dimensions robot) (iterations - 1)

let parse line =
    let m = Regex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)").Match line

    { Position =
        { X = int m.Groups[1].Value
          Y = int m.Groups[2].Value }
      Velocity =
        { X = int m.Groups[3].Value
          Y = int m.Groups[4].Value } }

let count (height, width) robots =
    let midH = height / 2
    let midW = width / 2

    let filter minH maxH minW maxW =
        robots
        |> Seq.filter (fun r ->
            r.Position.X >= minW
            && r.Position.X < maxW
            && r.Position.Y >= minH
            && r.Position.Y < maxH)
        |> Seq.length


    let nw = filter 0 (midH) 0 (midW)
    let ne = filter 0 (midH) (width - midW) width
    let sw = filter (height - midH) height 0 (midW)
    let se = filter (height - midH) height (width - midW) width

    nw * ne * sw * se

let puzzle file =
    System.IO.File.ReadAllLines file |> Array.map parse


// let dimensions = (7, 11)
let dimensions = (103, 101)

let input = puzzle file

let part1 =
    input
    |> Array.map (fun r -> Robot.simulate dimensions r 100)
    |> count dimensions

printfn $"Part 1: %A{part1}"

let part2 =
    // for i = 0 to 10_000 do
    //     if i % 101 = 62 || i % 103 = 25 then
    //         let simRobots = input |> Array.map (fun r -> Robot.simulate dimensions r i)
    //         Robot.print dimensions simRobots
    //         printfn "SECONDS %i" i

    input |> Array.map (fun r -> Robot.simulate dimensions r 7132)

printfn "Part 2: 7132"
Robot.print dimensions part2
