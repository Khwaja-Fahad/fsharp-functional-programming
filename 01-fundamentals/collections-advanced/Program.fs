// =======================
// Map, Set, Seq in F#
// =======================

// MAP
let ages =
    Map.ofList [
        ("Ali", 21)
        ("Sara", 22)
    ]

printfn "Ali age: %d" ages.["Ali"]

match Map.tryFind "John" ages with
| Some age -> printfn "John: %d" age
| None -> printfn "John not found"

// SET
let numbers = Set.ofList [1; 2; 2; 3; 4]

printfn "Set: %A" numbers
printfn "Contains 3: %b" (Set.contains 3 numbers)

// SEQ
let seqNumbers = seq { 1 .. 10 }

let doubled =
    seqNumbers
    |> Seq.map (fun x -> x * 2)
    |> Seq.toList

printfn "Doubled: %A" doubled