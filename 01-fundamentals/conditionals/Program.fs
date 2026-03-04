// =======================
// F# Conditionals
// =======================

//Basic IF

let age = 20

if age >= 18 then
    printfn "You are an adult"


//IF ELSE

let temperature = 15

if temperature > 25 then
    printfn "It's hot"
else
    printfn "It's not hot"



//IF as an Expression

let number = 7

let result =
    if number % 2 = 0 then
        "Even"
    else
        "Odd"

printfn "Number is %s" result


//Nested IF

let score = 85

if score >= 90 then
    printfn "Grade A"
elif score >= 75 then
    printfn "Grade B"
elif score >= 60 then
    printfn "Grade C"
else
    printfn "Fail"



//MATCH

let number1 = 3

match number1 with
| 1 -> printfn "One"
| 2 -> printfn "Two"
| 3 -> printfn "Three"
| _ -> printfn "Other"