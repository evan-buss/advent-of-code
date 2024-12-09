let path =
    fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultValue "sample.txt"

let area = System.IO.File.ReadAllLines path |> Array.map (Seq.map id >> Seq.toArray)

let antennas =
    area
    |> Array.mapi (fun y row ->
        row
        |> Array.mapi (fun x c ->
            if System.Char.IsLetterOrDigit c then
                Some(c, (x + 1, y + 1))
            else
                None))
    |> Array.collect id
    |> Array.choose id
    |> Array.groupBy fst
    |> Array.map (fun (_, coords) -> Array.map snd coords)

let pairs coords =
    Array.allPairs coords coords |> Array.filter (fun (a, b) -> a < b)

let outOfBounds (x, y) =
    x < 1 || x > area[0].Length || y < 1 || y > area.Length

let calcSlope (x1, y1) (x2, y2) = (x2 - x1, y2 - y1)

// Compute next position in the line until out of bounds
let slopeSeq (dx, dy) (x, y) =
    Seq.unfold
        (fun (cx, cy) ->
            let nx, ny = cx + dx, cy + dy

            if outOfBounds (nx, ny) then
                None
            else
                Some((nx, ny), (nx, ny)))
        (x, y)

let antinodes a b =
    let slope = calcSlope a b

    [ Seq.tryHead (slopeSeq slope b)
      Seq.tryHead (slopeSeq (-fst slope, -snd slope) a) ]
    |> Seq.choose id


let resonance a b =
    let slope = calcSlope a b
    Seq.append (slopeSeq slope b) (Seq.append [ a; b ] (slopeSeq (-fst slope, -snd slope) a))

let part1 =
    antennas
    |> Seq.collect pairs // Get all combinations of antennas
    |> Seq.collect ((<||) antinodes) // Find antinode position for each
    |> Seq.filter (not << outOfBounds) // Remove any out of bounds positions
    |> Set.ofSeq // Get unique positions (some overlap)

let part2 =
    antennas
    |> Seq.collect pairs
    |> Seq.collect ((<||) resonance)
    |> Seq.filter (not << outOfBounds)
    |> Set.ofSeq

printfn $"Part 1: {part1 |> Set.count}"
printfn $"Part 2: {part2 |> Set.count}"
