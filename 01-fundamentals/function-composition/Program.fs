// =======================
// Function Composition
// =======================

let add1 x = x + 1
let double x = x * 2
let square x = x * x

// Composition
let process = add1 >> double >> square

printfn "Result: %d" (process 3)

// String example
let toUpper (s:string) = s.ToUpper()
let addHello s = "Hello " + s

let greet = toUpper >> addHello

printfn "%s" (greet "fahad")