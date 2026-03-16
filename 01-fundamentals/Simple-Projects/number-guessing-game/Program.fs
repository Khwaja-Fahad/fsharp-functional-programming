// ==============================
// Number Guessing Game
// ==============================

open System

// Generate random number between 1 and 100

let random = Random()
let secretNumber = random.Next(1,101)

//Recursive Game Loop
let rec guessLoop attempts =
    printf "Enter your guess (1-100): "
    let input = Console.ReadLine()
    let guess = int input
    
    if guess < secretNumber then
        printfn "Low"
        guessLoop (attempts + 1)
    elif guess > secretNumber then
        printfn "High"
        guessLoop (attempts + 1)
    else
        printfn "Correct! You guessed the number in %d attempts." attempts


printfn "Welcome to the Number guessing game!"
printfn "I have selected a number between 1 and 100."

guessLoop 1