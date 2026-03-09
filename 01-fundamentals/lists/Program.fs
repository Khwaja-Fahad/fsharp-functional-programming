// =======================
// Lists in F#
// =======================

let numbers = [10; 20; 30; 40]

printfn "Original list: %A" numbers

// Add at front
let frontAdded = 5 :: numbers
printfn "Add at front: %A" frontAdded

// Add at end
let endAdded = numbers @ [50]
printfn "Add at end: %A" endAdded

// Get item at index
let itemAt2 = List.item 2 numbers
printfn "Item at index 2: %d" itemAt2

// Update item at index 1
let updated =
    numbers
    |> List.mapi (fun i x -> if i = 1 then 99 else x)

printfn "Updated index 1: %A" updated

// Insert at index 2
let inserted =
    (numbers |> List.take 2) @ [25] @ (numbers |> List.skip 2)

printfn "Insert 25 at index 2: %A" inserted

// Remove index 1
let removed =
    (numbers |> List.take 1) @ (numbers |> List.skip 2)

printfn "Remove index 1: %A" removed

// Head and tail
printfn "Head: %d" (List.head numbers)
printfn "Tail: %A" (List.tail numbers)

// Length and sum
printfn "Length: %d" (List.length numbers)
printfn "Sum: %d" (List.sum numbers)

// Contains
printfn "Contains 30: %b" (List.contains 30 numbers)

// Map
let doubled = List.map (fun x -> x * 2) numbers
printfn "Doubled: %A" doubled

// Filter
let greaterThan20 = List.filter (fun x -> x > 20) numbers
printfn "Greater than 20: %A" greaterThan20