// ===============================
// Even / Odd Analyzer
// ===============================

// Function to classify number as Even or Odd

let ClassifyNumber n =
    match n % 2 with
    | 0 -> "Even"
    | _ -> "Odd"


// Function to analyze number size

let analyzeSize n =
    if n > 10 then
        "Greater than 10"
    elif n = 10 then
        "Equal to 10"
    else
      "Less than 10"


// Ask user for input

printf "Enter a Number: "
let input = System.Console.ReadLine()
let number = int input


// Get results
let kind = ClassifyNumber number
let size = analyzeSize number

// Print output

printfn ""
printfn "%d is %s" number kind
printfn "%d is %s" number size