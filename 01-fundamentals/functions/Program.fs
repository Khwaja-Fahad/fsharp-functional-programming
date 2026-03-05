// Simple function

let square x =
    x * x

let result = square 5

printfn "Square of 5 = %d" result


//Function With Two Parameters

let add a b =
    a + b

let sum = add 10 7

printfn "Sum = %d" sum


//Function Returning String

let checkEven x =
    if x % 2 = 0 then
        "Even"
    else
        "Odd"

printfn "%s" (checkEven 6)
printfn "%s" (checkEven 9)



//Function With Explicit Type

let multiply (a:int) (b:int) : int =
    a * b

printfn "%d" (multiply 4 6)