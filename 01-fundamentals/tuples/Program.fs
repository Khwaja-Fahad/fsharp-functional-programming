// =======================
// Tuples in F#
// =======================

let person = ("Fahad", 22)

printfn "Person: %A" person

let (name, age) = person

printfn "Name: %s" name
printfn "Age: %d" age

let calculate a b =
    (a + b, a * b)

let (sum, product) = calculate 3 4

printfn "Sum: %d" sum
printfn "Product: %d" product

let students =
    [
        ("Ali", 90)
        ("Sara", 85)
        ("John", 78)
    ]

students
|> List.iter (fun (name, score) ->
    printfn "%s scored %d" name score)