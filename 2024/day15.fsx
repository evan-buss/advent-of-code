let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"


type Piece =
    | Wall
    | Robot
    | Box
    | Empty

module Piece =
    let parse =
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

let instructions =
    directionLines
    |> Array.skip (1)
    |> Array.map (fun line -> line |> Seq.map Direction.parse |> Seq.toArray)
    |> Array.concat

let dimensions = board.Keys |> Seq.max
let robot = (board |> Seq.find (fun kvp -> kvp.Value = Robot)).Key

let print (height, width) board =

    for row = 0 to height do
        for col = 0 to width do
            match Map.tryFind (row, col) board with
            | Some(Wall) -> '#'
            | Some(Box) -> '0'
            | Some(Robot) -> '@'
            | _ -> ' '
            |> printf "%c"

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


let boards =
    instructions
    |> Seq.scan (fun (board, pos) instruction -> moveRobot board pos instruction) (board, robot)
    |> Seq.map fst
// |> Seq.iteri (fun i b ->
//     printfn "Iteration %i -> %A\n" i instructions[i]
//     print dimensions b)

let result = boards |> Seq.last

print dimensions result

printfn "Part 1: %i" (result |> boxPositions |> Seq.sumBy gps)
