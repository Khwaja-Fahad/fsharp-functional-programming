// ===============================
// Higher-Order Functions in F#
// ===============================

// Example list
let numbers = [1;2;3;4;5]

// -------------------------------
// MAP
// -------------------------------
// Apply a function to every element

let squared =
    numbers
    |> List.map (fun x -> x * x)

printfn "Squared numbers: %A" squared


// -------------------------------
// FILTER
// -------------------------------
// Keep elements that satisfy a condition

let evenNumbers =
    numbers
    |> List.filter (fun x -> x % 2 = 0)

printfn "Even numbers: %A" evenNumbers


// -------------------------------
// FOLD
// -------------------------------
// Reduce list to a single value

let sum =
    numbers
    |> List.fold (fun acc x -> acc + x) 0

printfn "Sum of numbers: %d" sum


// -------------------------------
// ITER
// -------------------------------
// Execute function for each element

printfn "Printing elements:"
numbers
|> List.iter (fun x -> printfn "Value: %d" x)
