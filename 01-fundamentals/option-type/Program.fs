// =======================
// Option Type in F#
// =======================

let safeDivide a b =
    if b = 0 then
        None
    else
        Some (a / b)

let testDivision a b =
    match safeDivide a b with
    | Some result -> printfn "%d / %d = %d" a b result
    | None -> printfn "Cannot divide %d by zero" a

testDivision 10 2
testDivision 10 0
