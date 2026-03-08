// =======================
// Recursion in F#
// =======================

// Recursive factorial function
let rec factorial n =
    if n = 0 then
        1
    else
        n * factorial (n - 1)

// Ask user for input
printf "Enter a number: "

let input = System.Console.ReadLine()
let number = int input

// Calculate factorial
let result = factorial number

// Print result
printfn "Factorial of %d = %d" number result