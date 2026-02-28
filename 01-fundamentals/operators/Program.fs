// =======================
// F# Operators Examples
// =======================

let a = 12
let b = 5

// Arithmetic
printfn "Arithmetic Operators:"
printfn "a + b = %d" (a + b)
printfn "a - b = %d" (a - b)
printfn "a * b = %d" (a * b)
printfn "a / b = %d" (a / b)
printfn "a %% b = %d" (a % b)

printfn "-------------------"

// Comparison
printfn "Comparison Operators:"
printfn "a = b : %b" (a = b)
printfn "a <> b : %b" (a <> b)
printfn "a > b : %b" (a > b)
printfn "a < b : %b" (a < b)
printfn "a >= b : %b" (a >= b)
printfn "a <= b : %b" (a <= b)

printfn "-------------------"

// Logical
let x = true
let y = false

printfn "Logical Operators:"
printfn "x && y : %b" (x && y)
printfn "x || y : %b" (x || y)
printfn "not x : %b" (not x)