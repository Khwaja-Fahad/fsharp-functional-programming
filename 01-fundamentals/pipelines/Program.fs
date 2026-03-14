// =======================
// Pipelines in F#
// =======================

let numbers = [1;2;3;4;5]

let result =
    numbers
    |> List.map (fun x -> x * 2)
    |> List.filter (fun x -> x > 5)
    |> List.sum

printfn "Result = %d" result
