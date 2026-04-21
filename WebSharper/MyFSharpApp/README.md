# WebSharper — Even/Odd Checker

My first WebSharper web application built with F#.

## What It Does

- User enters a number
- Clicks **Check** button
- App displays whether the number is **Even** or **Odd**

## Technologies

- F#
- WebSharper SPA
- .NET SDK 10

## How to Run

```bash
cd WebSharper/MyFSharpApp
dotnet build
dotnet run
```

Then open: `http://localhost:5000`

## What I Learned

- WebSharper project structure (`Client.fs`, `index.html`, `Startup.fs`)
- Reactive variables (`Var.Create`)
- Binding input to UI (`Doc.InputType.Text`)
- Handling button click events (`on.click`)
- Auto-updating UI with `textView` and `View`
