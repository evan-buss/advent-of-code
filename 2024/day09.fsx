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

let chunkType chunk =
    match chunk |> List.forall (fun c -> c = chunk[0]) with
    | true -> chunk[0]
    | false -> failwith "invalid chunk"


let defrag (diskI: Block list list) =
    let mutable disk = diskI

    for blockIndex in (disk.Length - 1) .. 0 do
        printfn "iterating?"
        // iterate left to right to find open spaces
        let mutable found = false

        for openIndex in 0..blockIndex do
            if chunkType disk[openIndex] = Free && not found then
                printfn "FREE"
                let openLength = disk[openIndex].Length
                let blockLength = disk[blockIndex].Length

                if openLength <= blockLength then
                    disk <- List.updateAt openIndex disk[blockIndex] disk // Update open with block value
                    disk <- List.updateAt blockIndex (List.replicate blockLength Free) disk // Free block space

                    // Add another zero segment if we didn't use all available space
                    if openLength < blockLength then
                        disk <- List.insertAt (openIndex + 1) (List.replicate (openLength - blockLength) Free) disk

                    found <- true

    disk

// while l < r do
//     if chunkType disk[l] = Free && chunkType disk[r] = File then
//         // if we can fit the chunk move it to the free space
//         if disk[l].Length <= disk[r].Length  then
// insert all right blocks in front of free space
// shrink current free space if we didn't use it all or delete if we did
// if we did increment left again since there is an extra element
// zero out right block space
// otherwise
// otherwise, increase left until we get to next free chunk
// otherwise decrease right  until we get to next file chunk

let checksum (disk: Block seq) =
    disk
    |> Seq.indexed
    |> Seq.sumBy (fun (i, b) ->
        (match b with
         | File id -> id
         | Free -> 0)
        * (int64 i))

// let part1 = frag puzzle |> checksum
// printfn $"%A{part1}"

printfn "TEST"
test |> defrag
