let path =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

type Block =
    | File of Id: int64
    | Free

let charToInt = System.Char.GetNumericValue >> int

let puzzle =
    System.IO.File.ReadAllText path
    |> Seq.map charToInt
    |> Seq.chunkBySize 2
    |> Seq.mapi (fun id c ->
        [| for _ in 1 .. c[0] do
               File id
           for _ in 1 .. Array.tryItem 1 c |> Option.defaultValue 0 do
               Free |])
    |> Array.concat

printfn "%A" puzzle.Length

let test =
    puzzle
    |> Array.fold
        (fun (acc, currentChunk) item ->
            match currentChunk with
            | [] -> (acc, [ item ])
            | head :: _ when head = item -> (acc, item :: currentChunk)
            | _ -> (List.rev currentChunk :: acc, [ item ]))
        ([], [])
    |> fun (acc, lastChunk) -> (List.rev lastChunk :: acc)
    |> List.rev

let frag original =
    let mutable l = 0
    let mutable r = (Array.length original) - 1

    let disk = Array.copy original

    while l < r do
        if disk[r] = Free then
            r <- r - 1
        else if disk[l] = Free then
            disk[l] <- disk[r]
            disk[r] <- Free
            l <- l + 1
            r <- r - 1
        else
            l <- l + 1

    disk

let chunkType (chunk: Block list) =
    // match chunk |> List.forall (fun c -> c = chunk[0]) with
    // | true -> chunk[0]
    // | false -> failwith "invalid chunk"

    chunk[0]


let defrag (diskI: Block list list) =
    let mutable disk = diskI

    printfn "disk length %A" disk.Length

    for blockIndex in (disk.Length - 1) .. - 1 .. 0 do

        // if blockIndex % 1000 = 0 then
        //     printfn "Block %A" blockIndex

        // printfn "iterating?"
        // iterate left to right to find open spaces
        let mutable found = false

        let mutable firstOpen = 0

        if chunkType disk[blockIndex] <> Free then
            let mutable openIndex = firstOpen

            while openIndex < blockIndex && not found do

                if chunkType disk[openIndex] = Free && not found then
                    let openLength = disk[openIndex].Length
                    let blockLength = disk[blockIndex].Length

                    if openLength >= blockLength then
                        disk <- List.updateAt openIndex disk[blockIndex] disk // Update open with block value
                        disk <- List.updateAt blockIndex (List.replicate blockLength Free) disk // Free block space

                        // Add another zero segment if we didn't use all available space
                        if openLength > blockLength then
                            disk <- List.insertAt (openIndex + 1) (List.replicate (openLength - blockLength) Free) disk
                            firstOpen <- openIndex + 1
                        else
                            firstOpen <- openIndex

                        printfn "%A\n\n" disk

                        found <- true

                openIndex <- openIndex + 1

    disk

let checksum (disk: Block seq) =
    disk
    |> Seq.mapi (fun i b ->
        (match b with
         | File id -> id
         | Free -> 0)
        * (int64 i))
    |> Seq.sum

// let part1 = frag puzzle |> checksum
// printfn $"%A{part1}"

printfn "TEST"
let part2 = test |> defrag |> List.concat |> checksum
printfn "%A" part2
