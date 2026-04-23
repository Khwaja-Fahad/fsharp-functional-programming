namespace Calculator

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Client =

    let add (a: float) (b: float)      = a + b
    let subtract (a: float) (b: float) = a - b
    let multiply (a: float) (b: float) = a * b
    let divide (a: float) (b: float) =
        if b = 0.0 then None
        else Some (a / b)

    let calculate (a: float) (b: float) (op: string) =
        match op with
        | "+" -> Some (add a b)
        | "-" -> Some (subtract a b)
        | "*" -> Some (multiply a b)
        | "/" -> divide a b
        | _   -> None

    [<SPAEntryPoint>]
    let Main () =

        let num1Var   = Var.Create ""
        let num2Var   = Var.Create ""
        let resultVar = Var.Create "Click an operation below"

        let onCalculate (op: string) =
            match System.Double.TryParse(num1Var.Value),
                  System.Double.TryParse(num2Var.Value) with
            | (true, a), (true, b) ->
                match calculate a b op with
                | Some result ->
                    resultVar.Value <- $"{a} {op} {b} = {result}"
                | None ->
                    resultVar.Value <- "Cannot divide by zero!"
            | _ ->
                resultVar.Value <- "Please enter valid numbers!"

        div [] [
            h1 [] [ text "F# Calculator" ]

            p [] [ text "First Number:" ]
            Doc.InputType.Text [] num1Var

            p [] [ text "Second Number:" ]
            Doc.InputType.Text [] num2Var

            p [] [
                button [ on.click (fun _ _ -> onCalculate "+") ] [ text " + " ]
                button [ on.click (fun _ _ -> onCalculate "-") ] [ text " - " ]
                button [ on.click (fun _ _ -> onCalculate "*") ] [ text " * " ]
                button [ on.click (fun _ _ -> onCalculate "/") ] [ text " / " ]
            ]

            h2 [] [ textView resultVar.View ]
        ]
        |> Doc.RunById "main"