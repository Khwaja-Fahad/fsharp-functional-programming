// =======================
// Discriminated Unions
// =======================

// Define DU
type Shape =
    | Circle of float
    | Rectangle of float * float
    | Square of float

// Function using pattern matching
let area shape =
    match shape with
    | Circle r -> 3.14 * r * r
    | Rectangle (w, h) -> w * h
    | Square s -> s * s

// Create shapes
let s1 = Circle 5.0
let s2 = Rectangle (4.0, 6.0)
let s3 = Square 3.0

// Print areas
printfn "Circle area: %.2f" (area s1)
printfn "Rectangle area: %.2f" (area s2)
printfn "Square area: %.2f" (area s3)