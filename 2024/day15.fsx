let file =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"


type Piece =
    | Wall
    | Robot
    | Box
    | Empty

let parsePiece =
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

let parseDirection =
    function
    | '<' -> Left
    | '>' -> Right
    | '^' -> Up
    | 'v' -> Down
    | v -> failwithf "invalid direction %c" v

let boardLines, directionLines =
    System.IO.File.ReadAllLines file |> Array.partition (fun c -> c.StartsWith("#"))

let board =
    boardLines
    |> Array.mapi (fun row line -> Seq.map id line |> Seq.mapi (fun col cell -> ((row, col), parsePiece cell)))
    |> Seq.concat
    |> Seq.filter (fun p -> snd p <> Empty)
    |> Map.ofSeq

let instructions =
    directionLines
    |> Array.skip (1)
    |> Array.map (fun line -> line |> Seq.map (parseDirection) |> Seq.toArray)
    |> Array.concat

let dimensions = board.Keys |> Seq.max

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

instructions |> Seq.scan (fun state instruction -> 
    match instruction with 
    | Up -> 
    ) board


print dimensions board
