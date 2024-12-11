let path =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

type Block =
    | File of int64
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

let debug disk =
    System.String.Concat(
        disk
        |> Seq.concat
        |> Seq.map (fun c ->
            match c with
            | File id -> string id
            | Free -> ".")
    )

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
    match chunk |> List.forall (fun c -> c = chunk[0]) with
    | true -> chunk[0]
    | false -> failwith "invalid chunk"

let defrag original =
    // Split into file and free chunks of the same type and File id
    // Ex. [File 1; File 1; Free; Free; File 3;] becomes [[File 1; File1;]; [Free; Free;]; [File 3;]]
    let disk =
        ResizeArray(
            original
            |> Array.fold
                (fun (acc, currentChunk) item ->
                    match currentChunk with
                    | [] -> (acc, [ item ])
                    | head :: _ when head = item -> (acc, item :: currentChunk)
                    | _ -> (List.rev currentChunk :: acc, [ item ]))
                ([], [])
            |> fun (acc, lastChunk) -> (List.rev lastChunk :: acc)
            |> List.rev
            |> List.map (fun block -> (false, block))
        )

    let chunk (chunk: bool * Block list) = chunk |> snd
    let chunkMoved (chunk: bool * Block list) = chunk |> fst

    let mutable blockIndex = disk.Count - 1

    while blockIndex > 0 do
        let mutable found = false

        let currentChunk = disk[blockIndex]

        // File chunk that hasn't been moved yet
        if not (chunkMoved currentChunk) && chunkType (chunk currentChunk) <> Free then
            let mutable openIndex = 0

            while openIndex < blockIndex && not found do
                if chunkType (chunk disk[openIndex]) = Free then
                    let openLength = (chunk disk[openIndex]).Length
                    let blockLength = (chunk currentChunk).Length

                    if openLength >= blockLength then
                        // printfn "%A" <| debug disk
                        found <- true
                        disk[openIndex] <- (true, chunk currentChunk)
                        disk[blockIndex] <- (true, List.replicate blockLength Free)

                        // Add another zero segment if we didn't use all available space
                        if openLength > blockLength then
                            disk.Insert(openIndex + 1, (false, List.replicate (openLength - blockLength) Free))
                            blockIndex <- blockIndex + 1 // keep same block index as we increased size

                openIndex <- openIndex + 1

        blockIndex <- blockIndex - 1

    disk |> Seq.map snd |> Seq.concat

let checksum (disk: Block seq) =
    disk
    |> Seq.mapi (fun i b ->
        (match b with
         | File id -> id
         | Free -> 0)
        * (int64 i))
    |> Seq.sum

let part1 = puzzle |> frag |> checksum
printfn "Part 1: %d" part1

let part2 = puzzle |> defrag |> checksum
printfn "Part 2: %d" part2
