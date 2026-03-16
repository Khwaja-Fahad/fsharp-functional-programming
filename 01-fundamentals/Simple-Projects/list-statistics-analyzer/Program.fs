// ==============================
// List Statistics Analyzer
// ==============================

printfn "Enter Number separated by space: "

let input = System.Console.ReadLine()

// Convert string input into list of integers

let numbers =
    input.Split(' ')
    |> Array.map int
    |> Array.toList

printfn "NUmbers: %A" numbers

// Count numbers
let listSize = List.length numbers
printfn "Size of List: %d" listSize

// Sum of Numbers
let sum = List.sum numbers
printfn "Sum of numbers: %d" sum

// Average

let average = List.averageBy float numbers
printfn "Average of numbers: %.2f" average

// Even Numbers 

let evenNumber =
    numbers
    |> List.filter (fun x -> x % 2 = 0)

printfn "Even Numbers in List: %A" evenNumber


// Odd Numbers 

let oddNumber =
    numbers
    |> List.filter (fun x -> x % 2 <> 0)

printfn "Odd Numbers in List: %A" oddNumber

// Square numbers
let squares =
    numbers
    |> List.map (fun x -> x * x)

printfn "Squares: %A" squares

// Maximum
let maxValue = List.max numbers
printfn "Maximum: %d" maxValue

// Minimum
let minValue = List.min numbers
printfn "Minimum: %d" minValue