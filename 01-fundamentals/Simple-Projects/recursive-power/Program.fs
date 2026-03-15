// ===============================
// Recursive Power Calculator
// ===============================

// Recursive function to calculate power

let rec power baseNum exponent =
    if exponent = 0 then
        1
    else
        baseNum * power baseNum (exponent - 1)

// Ask user for input
printf "Enter base number: "
let baseNum = int (System.Console.ReadLine())


printf "Enter Exponent: "
let exponent = int (System.Console.ReadLine())

// Calculate result

let result = power baseNum exponent

// Print result
printfn ""
printfn "%d ^ %d is %d" baseNum exponent result