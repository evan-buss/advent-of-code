let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

type Pos = int * int

type Piece =
    | Wall
    | Robot
    | Box
    | WideBox of Pos * Pos
    | Empty

module Piece =
    let parse: char -> Piece =
        function
        | '#' -> Wall
        | '@' -> Robot
        | 'O' -> Box
        | '.' -> Empty
        | v -> failwithf "invalid piece %c" v

type Direction =
    | Left
    | Right
    | Up
    | Down


module Direction =
    let parse =
        function
        | '<' -> Left
        | '>' -> Right
        | '^' -> Up
        | 'v' -> Down
        | v -> failwithf "invalid direction %c" v

    let move (row, col) direction =
        match direction with
        | Up -> (row - 1, col)
        | Down -> (row + 1, col)
        | Left -> (row, col - 1)
        | Right -> (row, col + 1)

let boardLines, directionLines =
    System.IO.File.ReadAllLines file |> Array.partition (fun c -> c.StartsWith("#"))

let board =
    boardLines
    |> Array.mapi (fun row line -> Seq.map id line |> Seq.mapi (fun col cell -> ((row, col), Piece.parse cell)))
    |> Seq.concat
    |> Map.ofSeq

let wideBoard =
    boardLines
    |> Array.mapi (fun row line ->
        line.Replace("#", "##").Replace(".", "..").Replace("O", "[]").Replace("@", "@.")
        |> Seq.chunkBySize 2
        |> Seq.mapi (fun col pair ->
            let left = row, col * 2
            let right = row, (col * 2) + 1

            match pair with
            | [| '['; ']' |] -> [| left, WideBox(left, right); right, WideBox(left, right) |]
            | [| l; r |] -> [| left, Piece.parse l; right, Piece.parse r |]
            | _ -> failwith "invalid input")

        |> Seq.concat)
    |> Seq.concat
    |> Map.ofSeq

let instructions =
    directionLines
    |> Array.skip (1)
    |> Array.map (fun line -> line |> Seq.map Direction.parse |> Seq.toArray)
    |> Array.concat

let robot = (board |> Seq.find (fun kvp -> kvp.Value = Robot)).Key

let print board =
    let (height, width) = Map.keys board |> Seq.max

    for row = 0 to height do
        for col = 0 to width do
            let pos = (row, col)

            match Map.tryFind pos board with
            | Some(Wall) -> "#"
            | Some(Box) -> "0"
            | Some(WideBox(l, r)) ->
                if pos = l then "["
                else if pos = r then "]"
                else ""
            | Some(Robot) -> "@"
            | _ -> " "
            |> printf "%s"

        printf "\n"

    printf "\n"

let rec push board pos direction =
    let next = Direction.move pos direction

    match Map.tryFind next board with
    | Some(Empty) -> Some(next)
    | Some(Wall) -> None
    | _ -> push board next direction

let moveRobot board pos direction =
    let next = Direction.move pos direction

    match Map.tryFind next board with
    | Some(Empty) -> (board |> Map.add pos Empty |> Map.add next Robot, next)
    | Some(Wall) -> (board, pos)
    | Some(Box) ->
        match push board next direction with
        | Some(behind) -> (board |> Map.add behind Box |> Map.add pos Empty |> Map.add next Robot, next)
        | None -> (board, pos)
    | v -> failwithf "invalid state %A" v

let boxPositions board =
    board |> Map.filter (fun _ v -> v = Box) |> Map.keys |> Seq.toArray

let gps (row, col) = (row * 100) + col

let part1 =
    instructions
    |> Seq.scan (fun (board, pos) instruction -> moveRobot board pos instruction) (board, robot)
    |> Seq.last
    |> fst
    |> boxPositions
    |> Seq.sumBy gps

printfn "Part 1: %i" part1

// |> Seq.iteri (fun i b ->k
//     printfn "Iteration %i -> %A\n" i instructions[i]
//     print dimensions b)

let part2 =
    instructions
    |> Seq.scan (fun (board, pos) instruction -> moveRobot board pos instruction) (wideBoard, robot)
    |> Seq.last
    |> fst
    |> boxPositions
    |> Seq.sumBy gps

printfn "Part 2: %i" part2
