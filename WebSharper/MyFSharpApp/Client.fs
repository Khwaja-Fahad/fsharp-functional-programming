namespace MyFSharpApp

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Client =

    // Pure F# function — same logic as your console app!
    let checkEvenOdd (n: int) =
        if n % 2 = 0 then "Even ✅"
        else "Odd ❌"

    // This is the UI — runs in the browser
    [<SPAEntryPoint>]
    let Main () =

        // Reactive variable — holds whatever user types
        let inputVar = Var.Create ""
        let resultVar = Var.Create "Enter a number above 👆"

        // What happens when button is clicked
        let onCheck () =
            let text = inputVar.Value
            match System.Int32.TryParse(text) with
            | true, number ->
                let result = checkEvenOdd number
                resultVar.Value <- $"Number {number} is: {result}"
            | false, _ ->
                resultVar.Value <- "⚠️ Please enter a valid number!"

        // Build the UI
        div [] [
            h1 [] [ text "Even / Odd Checker" ]

            p [] [ text "Enter a number:" ]

            // Input box — linked to inputVar
            Doc.Input [] inputVar

            // Button
            button [
                on.click (fun _ _ -> onCheck ())
            ] [ text "Check" ]

            // Result display — updates automatically!
            p [] [ textView resultVar.View ]
        ]
        |> Doc.RunById "main"