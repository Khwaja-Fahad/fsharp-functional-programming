// =======================
// Pattern Matching in F#
// =======================

// Value matching
let number = 3

match number with
| 1 -> printfn "One"
| 2 -> printfn "Two"
| 3 -> printfn "Three"
| _ -> printfn "Other"


// Tuple matching
let point = (0, 5)

match point with
| (0, y) -> printfn "On Y axis at %d" y
| (x, 0) -> printfn "On X axis at %d" x
| (x, y) -> printfn "Point (%d,%d)" x y


// List matching
let numbers = [1;2;3]

match numbers with
| [] -> printfn "Empty list"
| [x] -> printfn "Single element: %d" x
| head :: tail -> printfn "Head: %d Tail: %A" head tail