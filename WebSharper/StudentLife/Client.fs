namespace StudentLife

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Client =

    type Priority = Urgent | Normal | Safe

    type Exam = {
        Id       : int
        Subject  : string
        Date     : string
        Priority : Priority
    }

    type Assignment = {
        Id       : int
        Title    : string
        Subject  : string
        Deadline : string
        Done     : bool
    }

    type TransactionType = Income | Expense

    type Transaction = {
        Id          : int
        Description : string
        Amount      : float
        Category    : string
        TType       : TransactionType
    }

    type Habit = {
        Id        : int
        Name      : string
        Completed : bool
        Streak    : int
    }

    let mutable nextId = 1
    let getNextId () =
        let id = nextId
        nextId <- nextId + 1
        id

    let priorityText (p: Priority) =
        match p with
        | Urgent -> "Urgent"
        | Normal -> "Normal"
        | Safe   -> "Safe"

    let parsePriority (s: string) =
        match s with
        | "Urgent" -> Urgent
        | "Safe"   -> Safe
        | _        -> Normal

    let calculateBalance (transactions: Transaction list) =
        transactions |> List.fold (fun acc t ->
            match t.TType with
            | Income  -> acc + t.Amount
            | Expense -> acc - t.Amount
        ) 0.0

    let totalIncome (transactions: Transaction list) =
        transactions
        |> List.filter (fun t -> t.TType = Income)
        |> List.sumBy (fun t -> t.Amount)

    let totalExpense (transactions: Transaction list) =
        transactions
        |> List.filter (fun t -> t.TType = Expense)
        |> List.sumBy (fun t -> t.Amount)

    let activeTab    = Var.Create "exams"
    let exams        = Var.Create ([] : Exam list)
    let assignments  = Var.Create ([] : Assignment list)
    let transactions = Var.Create ([] : Transaction list)
    let habits       = Var.Create ([] : Habit list)
    let timerSeconds = Var.Create 1500
    let timerRunning = Var.Create false
    let timerSubject = Var.Create ""
    let studyLog     = Var.Create ([] : string list)

    let examModule () =
        let subjectVar  = Var.Create ""
        let dateVar     = Var.Create ""
        let priorityVar = Var.Create "Normal"
        let msgVar      = Var.Create ""

        let addExam () =
            if subjectVar.Value = "" || dateVar.Value = "" then
                msgVar.Value <- "Please fill all fields!"
            else
                let exam = {
                    Id       = getNextId ()
                    Subject  = subjectVar.Value
                    Date     = dateVar.Value
                    Priority = parsePriority priorityVar.Value
                }
                exams.Value      <- exams.Value @ [exam]
                subjectVar.Value <- ""
                dateVar.Value    <- ""
                msgVar.Value     <- "Exam added!"

        let removeExam (id: int) =
            exams.Value <- exams.Value |> List.filter (fun e -> e.Id <> id)

        div [ attr.``class`` "module-container" ] [
            h2 [] [ text "Exam Planner" ]
            div [ attr.``class`` "form-group" ] [
                Doc.InputType.Text [ attr.placeholder "Subject name" ] subjectVar
                Doc.InputType.Text [ attr.placeholder "Exam date (e.g. 2026-05-10)" ] dateVar
                div [ attr.``class`` "priority-btns" ] [
                    button [ attr.``class`` "btn-urgent"; on.click (fun _ _ -> priorityVar.Value <- "Urgent") ] [ text "Urgent" ]
                    button [ attr.``class`` "btn-normal"; on.click (fun _ _ -> priorityVar.Value <- "Normal") ] [ text "Normal" ]
                    button [ attr.``class`` "btn-safe";   on.click (fun _ _ -> priorityVar.Value <- "Safe")   ] [ text "Safe"   ]
                ]
                button [ attr.``class`` "btn-primary"; on.click (fun _ _ -> addExam ()) ] [ text "Add Exam" ]
            ]
            p [ attr.``class`` "msg" ] [ textView msgVar.View ]
            div [ attr.``class`` "list-container" ] [
                exams.View |> Doc.BindView (fun examList ->
                    if examList.IsEmpty then
                        p [ attr.``class`` "empty-msg" ] [ text "No exams added yet." ] :> Doc
                    else
                        examList |> List.map (fun e ->
                            div [ attr.``class`` "list-item" ] [
                                div [ attr.``class`` "item-info" ] [
                                    strong [] [ text e.Subject ]
                                    span [ attr.``class`` "badge" ] [ text (priorityText e.Priority) ]
                                    span [ attr.``class`` "date"  ] [ text e.Date ]
                                ]
                                button [ attr.``class`` "btn-danger"; on.click (fun _ _ -> removeExam e.Id) ] [ text "Remove" ]
                            ] :> Doc
                        ) |> Doc.Concat
                )
            ]
        ]

    let assignmentModule () =
        let titleVar    = Var.Create ""
        let subjectVar  = Var.Create ""
        let deadlineVar = Var.Create ""
        let msgVar      = Var.Create ""

        let addAssignment () =
            if titleVar.Value = "" || subjectVar.Value = "" || deadlineVar.Value = "" then
                msgVar.Value <- "Please fill all fields!"
            else
                let a = {
                    Id       = getNextId ()
                    Title    = titleVar.Value
                    Subject  = subjectVar.Value
                    Deadline = deadlineVar.Value
                    Done     = false
                }
                assignments.Value <- assignments.Value @ [a]
                titleVar.Value    <- ""
                subjectVar.Value  <- ""
                deadlineVar.Value <- ""
                msgVar.Value      <- "Assignment added!"

        let toggleDone (id: int) =
            assignments.Value <- assignments.Value |> List.map (fun a ->
                if a.Id = id then { a with Done = not a.Done } else a)

        let removeAssignment (id: int) =
            assignments.Value <- assignments.Value |> List.filter (fun a -> a.Id <> id)

        div [ attr.``class`` "module-container" ] [
            h2 [] [ text "Assignment Tracker" ]
            div [ attr.``class`` "form-group" ] [
                Doc.InputType.Text [ attr.placeholder "Assignment title"          ] titleVar
                Doc.InputType.Text [ attr.placeholder "Subject"                   ] subjectVar
                Doc.InputType.Text [ attr.placeholder "Deadline (e.g. 2026-05-01)"] deadlineVar
                button [ attr.``class`` "btn-primary"; on.click (fun _ _ -> addAssignment ()) ] [ text "Add Assignment" ]
            ]
            p [ attr.``class`` "msg" ] [ textView msgVar.View ]
            div [ attr.``class`` "list-container" ] [
                assignments.View |> Doc.BindView (fun aList ->
                    if aList.IsEmpty then
                        p [ attr.``class`` "empty-msg" ] [ text "No assignments added yet." ] :> Doc
                    else
                        aList |> List.map (fun a ->
                            div [ attr.``class`` (if a.Done then "list-item done" else "list-item") ] [
                                div [ attr.``class`` "item-info" ] [
                                    strong [] [ text a.Title ]
                                    span [ attr.``class`` "badge" ] [ text a.Subject  ]
                                    span [ attr.``class`` "date"  ] [ text a.Deadline ]
                                ]
                                div [ attr.``class`` "btn-group" ] [
                                    button [ attr.``class`` "btn-success"; on.click (fun _ _ -> toggleDone a.Id)        ] [ text (if a.Done then "Undo" else "Done") ]
                                    button [ attr.``class`` "btn-danger";  on.click (fun _ _ -> removeAssignment a.Id)  ] [ text "Remove" ]
                                ]
                            ] :> Doc
                        ) |> Doc.Concat
                )
            ]
        ]

    let budgetModule () =
        let descVar   = Var.Create ""
        let amountVar = Var.Create ""
        let catVar    = Var.Create "Food"
        let typeVar   = Var.Create "Expense"
        let msgVar    = Var.Create ""

        let addTransaction () =
            match System.Double.TryParse(amountVar.Value) with
            | true, amt when amt > 0.0 ->
                let t = {
                    Id          = getNextId ()
                    Description = descVar.Value
                    Amount      = amt
                    Category    = catVar.Value
                    TType       = if typeVar.Value = "Income" then Income else Expense
                }
                transactions.Value <- transactions.Value @ [t]
                descVar.Value      <- ""
                amountVar.Value    <- ""
                msgVar.Value       <- "Transaction added!"
            | _ ->
                msgVar.Value <- "Enter a valid amount!"

        let removeTransaction (id: int) =
            transactions.Value <- transactions.Value |> List.filter (fun t -> t.Id <> id)

        div [ attr.``class`` "module-container" ] [
            h2 [] [ text "Budget Tracker" ]
            div [ attr.``class`` "form-group" ] [
                Doc.InputType.Text [ attr.placeholder "Description" ] descVar
                Doc.InputType.Text [ attr.placeholder "Amount"      ] amountVar
                div [ attr.``class`` "priority-btns" ] [
                    button [ attr.``class`` "btn-cat"; on.click (fun _ _ -> catVar.Value <- "Food")      ] [ text "Food"      ]
                    button [ attr.``class`` "btn-cat"; on.click (fun _ _ -> catVar.Value <- "Transport") ] [ text "Transport" ]
                    button [ attr.``class`` "btn-cat"; on.click (fun _ _ -> catVar.Value <- "Books")     ] [ text "Books"     ]
                    button [ attr.``class`` "btn-cat"; on.click (fun _ _ -> catVar.Value <- "Fun")       ] [ text "Fun"       ]
                    button [ attr.``class`` "btn-cat"; on.click (fun _ _ -> catVar.Value <- "Other")     ] [ text "Other"     ]
                ]
                div [ attr.``class`` "priority-btns" ] [
                    button [ attr.``class`` "btn-danger";  on.click (fun _ _ -> typeVar.Value <- "Expense") ] [ text "Expense" ]
                    button [ attr.``class`` "btn-success"; on.click (fun _ _ -> typeVar.Value <- "Income")  ] [ text "Income"  ]
                ]
                button [ attr.``class`` "btn-primary"; on.click (fun _ _ -> addTransaction ()) ] [ text "Add" ]
            ]
            p [ attr.``class`` "msg" ] [ textView msgVar.View ]
            div [ attr.``class`` "stats-bar" ] [
                transactions.View |> Doc.BindView (fun tList ->
                    let balance = calculateBalance tList
                    let income  = totalIncome tList
                    let expense = totalExpense tList
                    div [ attr.``class`` "stats-inner" ] [
                        span [ attr.``class`` "stat-green" ] [ text $"Income: {income}"  ]
                        span [ attr.``class`` "stat-red"   ] [ text $"Expense: {expense}" ]
                        span [ attr.``class`` (if balance >= 0.0 then "stat-green" else "stat-red") ] [ text $"Balance: {balance}" ]
                    ] :> Doc
                )
            ]
            div [ attr.``class`` "list-container" ] [
                transactions.View |> Doc.BindView (fun tList ->
                    if tList.IsEmpty then
                        p [ attr.``class`` "empty-msg" ] [ text "No transactions yet." ] :> Doc
                    else
                        tList |> List.map (fun t ->
                            div [ attr.``class`` "list-item" ] [
                                div [ attr.``class`` "item-info" ] [
                                    strong [] [ text t.Description ]
                                    span [ attr.``class`` "badge" ] [ text t.Category ]
                                    span [ attr.``class`` (if t.TType = Income then "stat-green" else "stat-red") ] [ text $"{t.Amount}" ]
                                ]
                                button [ attr.``class`` "btn-danger"; on.click (fun _ _ -> removeTransaction t.Id) ] [ text "Remove" ]
                            ] :> Doc
                        ) |> Doc.Concat
                )
            ]
        ]

    let habitModule () =
        let habitVar = Var.Create ""
        let msgVar   = Var.Create ""

        let addHabit () =
            if habitVar.Value = "" then
                msgVar.Value <- "Enter a habit name!"
            else
                let h = {
                    Id        = getNextId ()
                    Name      = habitVar.Value
                    Completed = false
                    Streak    = 0
                }
                habits.Value   <- habits.Value @ [h]
                habitVar.Value <- ""
                msgVar.Value   <- "Habit added!"

        let toggleHabit (id: int) =
            habits.Value <- habits.Value |> List.map (fun h ->
                if h.Id = id then
                    let newStreak = if not h.Completed then h.Streak + 1 else max 0 (h.Streak - 1)
                    { h with Completed = not h.Completed; Streak = newStreak }
                else h)

        let removeHabit (id: int) =
            habits.Value <- habits.Value |> List.filter (fun h -> h.Id <> id)

        div [ attr.``class`` "module-container" ] [
            h2 [] [ text "Habit Tracker" ]
            div [ attr.``class`` "form-group" ] [
                Doc.InputType.Text [ attr.placeholder "New habit (e.g. Study 2 hours)" ] habitVar
                button [ attr.``class`` "btn-primary"; on.click (fun _ _ -> addHabit ()) ] [ text "Add Habit" ]
            ]
            p [ attr.``class`` "msg" ] [ textView msgVar.View ]
            div [ attr.``class`` "list-container" ] [
                habits.View |> Doc.BindView (fun hList ->
                    if hList.IsEmpty then
                        p [ attr.``class`` "empty-msg" ] [ text "No habits added yet." ] :> Doc
                    else
                        hList |> List.map (fun h ->
                            div [ attr.``class`` (if h.Completed then "list-item done" else "list-item") ] [
                                div [ attr.``class`` "item-info" ] [
                                    strong [] [ text h.Name ]
                                    span [ attr.``class`` "badge" ] [ text $"Streak: {h.Streak}" ]
                                ]
                                div [ attr.``class`` "btn-group" ] [
                                    button [ attr.``class`` "btn-success"; on.click (fun _ _ -> toggleHabit h.Id) ] [ text (if h.Completed then "Undo" else "Done") ]
                                    button [ attr.``class`` "btn-danger";  on.click (fun _ _ -> removeHabit h.Id) ] [ text "Remove" ]
                                ]
                            ] :> Doc
                        ) |> Doc.Concat
                )
            ]
        ]

    let timerModule () =
        let formatTime (seconds: int) =
            let m = seconds / 60
            let s = seconds % 60
            $"{m:D2}:{s:D2}"

        let startTimer () =
            if not timerRunning.Value then
                timerRunning.Value <- true
                let rec tick () =
                    if timerRunning.Value && timerSeconds.Value > 0 then
                        JS.SetTimeout (fun () ->
                            timerSeconds.Value <- timerSeconds.Value - 1
                            tick ()
                        ) 1000 |> ignore
                    elif timerSeconds.Value = 0 then
                        timerRunning.Value <- false
                        let log = $"Completed: {timerSubject.Value} - 25 min session"
                        studyLog.Value     <- log :: studyLog.Value
                        timerSeconds.Value <- 1500
                tick ()

        let stopTimer ()  = timerRunning.Value <- false
        let resetTimer () =
            timerRunning.Value <- false
            timerSeconds.Value <- 1500

        div [ attr.``class`` "module-container" ] [
            h2 [] [ text "Study Timer - Pomodoro" ]
            div [ attr.``class`` "timer-display" ] [
                timerSeconds.View |> Doc.BindView (fun s ->
                    h1 [ attr.``class`` "timer-text" ] [ text (formatTime s) ] :> Doc
                )
            ]
            div [ attr.``class`` "form-group" ] [
                Doc.InputType.Text [ attr.placeholder "What are you studying?" ] timerSubject
            ]
            div [ attr.``class`` "btn-group" ] [
                button [ attr.``class`` "btn-primary";   on.click (fun _ _ -> startTimer ()) ] [ text "Start" ]
                button [ attr.``class`` "btn-danger";    on.click (fun _ _ -> stopTimer  ()) ] [ text "Pause" ]
                button [ attr.``class`` "btn-secondary"; on.click (fun _ _ -> resetTimer ()) ] [ text "Reset" ]
            ]
            h3 [] [ text "Study Log" ]
            div [ attr.``class`` "list-container" ] [
                studyLog.View |> Doc.BindView (fun logs ->
                    if logs.IsEmpty then
                        p [ attr.``class`` "empty-msg" ] [ text "No study sessions yet." ] :> Doc
                    else
                        logs |> List.map (fun l ->
                            p [ attr.``class`` "log-item" ] [ text l ] :> Doc
                        ) |> Doc.Concat
                )
            ]
        ]

    [<SPAEntryPoint>]
    let Main () =

        let tabBtn (id: string) (label: string) =
            activeTab.View |> Doc.BindView (fun current ->
                button [
                    attr.``class`` (if current = id then "tab active" else "tab")
                    on.click (fun _ _ -> activeTab.Value <- id)
                ] [ text label ] :> Doc
            )

        let content () =
            activeTab.View |> Doc.BindView (fun tab ->
                match tab with
                | "exams"       -> examModule ()       :> Doc
                | "assignments" -> assignmentModule () :> Doc
                | "budget"      -> budgetModule ()     :> Doc
                | "habits"      -> habitModule ()      :> Doc
                | "timer"       -> timerModule ()      :> Doc
                | _             -> examModule ()       :> Doc
            )

        div [ attr.``class`` "app" ] [
            div [ attr.``class`` "header" ] [
                span [ attr.``class`` "label-left"  ] [ text "VEL6D3" ]
                span [ attr.``class`` "label-right" ] [ text "Fahad Muhammad" ]
                h1 [] [ text "StudentLife Manager" ]
                p  [] [ text "Your complete university life, in one place" ]
            ]
            div [ attr.``class`` "tabs" ] [
                tabBtn "exams"       "Exams"
                tabBtn "assignments" "Assignments"
                tabBtn "budget"      "Budget"
                tabBtn "habits"      "Habits"
                tabBtn "timer"       "Timer"
            ]
            div [ attr.``class`` "content" ] [
                content ()
            ]
        ]
        |> Doc.RunById "main"