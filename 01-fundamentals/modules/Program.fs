// =======================
// Modules in F#
// =======================

module Math =
    let add x y = x + y
    let square x = x * x

module Greeting =
    let sayHello name = "Hello " + name

printfn "%d" (Math.add 2 3)
printfn "%d" (Math.square 4)
printfn "%s" (Greeting.sayHello "Fahad")