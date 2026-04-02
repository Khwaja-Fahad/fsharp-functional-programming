open System

// =====================================
// Functional Banking System in F#
// =====================================

// -------------------------------
// Domain Types
// -------------------------------

type Account = {
    AccountNumber: int
    HolderName: string
    Balance: float
}

type Bank = Account list

// -------------------------------
// Helper Functions
// -------------------------------

let printLine () =
    printfn "----------------------------------------"

let showAccount account =
    printfn "Account Number: %d" account.AccountNumber
    printfn "Holder Name: %s" account.HolderName
    printfn "Balance: %.2f" account.Balance
    printLine ()

let rec findAccount accountNumber bank =
    match bank with
    | [] -> None
    | account :: rest ->
        if account.AccountNumber = accountNumber then
            Some account
        else
            findAccount accountNumber rest

let updateAccount updatedAccount bank =
    bank
    |> List.map (fun acc ->
        if acc.AccountNumber = updatedAccount.AccountNumber then
            updatedAccount
        else
            acc
    )

let accountExists accountNumber bank =
    bank
    |> List.exists (fun acc -> acc.AccountNumber = accountNumber)

// -------------------------------
// Bank Operations
// -------------------------------

let createAccount bank =
    printLine ()
    printf "Enter account number: "
    let accountNumber = int (Console.ReadLine())

    if accountExists accountNumber bank then
        printfn "An account with this number already exists."
        bank
    else
        printf "Enter holder name: "
        let holderName = Console.ReadLine()

        printf "Enter initial balance: "
        let initialBalance = float (Console.ReadLine())

        let newAccount = {
            AccountNumber = accountNumber
            HolderName = holderName
            Balance = initialBalance
        }

        printfn "Account created successfully."
        newAccount :: bank

let deposit bank =
    printLine ()
    printf "Enter account number: "
    let accountNumber = int (Console.ReadLine())

    match findAccount accountNumber bank with
    | None ->
        printfn "Account not found."
        bank
    | Some account ->
        printf "Enter deposit amount: "
        let amount = float (Console.ReadLine())

        if amount <= 0.0 then
            printfn "Deposit amount must be greater than zero."
            bank
        else
            let updatedAccount = { account with Balance = account.Balance + amount }
            printfn "Deposit successful. New balance: %.2f" updatedAccount.Balance
            updateAccount updatedAccount bank

let withdraw bank =
    printLine ()
    printf "Enter account number: "
    let accountNumber = int (Console.ReadLine())

    match findAccount accountNumber bank with
    | None ->
        printfn "Account not found."
        bank
    | Some account ->
        printf "Enter withdrawal amount: "
        let amount = float (Console.ReadLine())

        if amount <= 0.0 then
            printfn "Withdrawal amount must be greater than zero."
            bank
        elif amount > account.Balance then
            printfn "Insufficient balance."
            bank
        else
            let updatedAccount = { account with Balance = account.Balance - amount }
            printfn "Withdrawal successful. New balance: %.2f" updatedAccount.Balance
            updateAccount updatedAccount bank

let checkBalance bank =
    printLine ()
    printf "Enter account number: "
    let accountNumber = int (Console.ReadLine())

    match findAccount accountNumber bank with
    | None ->
        printfn "Account not found."
    | Some account ->
        printfn "Current balance for %s is %.2f" account.HolderName account.Balance

    bank

let showAllAccounts bank =
    printLine ()
    match bank with
    | [] ->
        printfn "No accounts found."
    | _ ->
        bank
        |> List.iter showAccount
    bank

let transfer bank =
    printLine ()
    printf "Enter sender account number: "
    let senderNumber = int (Console.ReadLine())

    printf "Enter receiver account number: "
    let receiverNumber = int (Console.ReadLine())

    if senderNumber = receiverNumber then
        printfn "Sender and receiver accounts must be different."
        bank
    else
        match findAccount senderNumber bank, findAccount receiverNumber bank with
        | None, _ ->
            printfn "Sender account not found."
            bank
        | _, None ->
            printfn "Receiver account not found."
            bank
        | Some sender, Some receiver ->
            printf "Enter transfer amount: "
            let amount = float (Console.ReadLine())

            if amount <= 0.0 then
                printfn "Transfer amount must be greater than zero."
                bank
            elif amount > sender.Balance then
                printfn "Insufficient balance in sender account."
                bank
            else
                let updatedSender = { sender with Balance = sender.Balance - amount }
                let updatedReceiver = { receiver with Balance = receiver.Balance + amount }

                bank
                |> updateAccount updatedSender
                |> updateAccount updatedReceiver
                |> fun updatedBank ->
                    printfn "Transfer successful."
                    updatedBank

// -------------------------------
// Menu
// -------------------------------

let showMenu () =
    printLine ()
    printfn "Functional Banking System"
    printLine ()
    printfn "1. Create Account"
    printfn "2. Deposit"
    printfn "3. Withdraw"
    printfn "4. Check Balance"
    printfn "5. Show All Accounts"
    printfn "6. Transfer Money"
    printfn "0. Exit"
    printLine ()
    printf "Choose an option: "

// -------------------------------
// Main Loop
// -------------------------------

let rec bankingLoop bank =
    showMenu ()

    let choice = Console.ReadLine()

    let updatedBank =
        match choice with
        | "1" -> createAccount bank
        | "2" -> deposit bank
        | "3" -> withdraw bank
        | "4" -> checkBalance bank
        | "5" -> showAllAccounts bank
        | "6" -> transfer bank
        | "0" ->
            printfn "Exiting the system. Goodbye!"
            bank
        | _ ->
            printfn "Invalid choice. Please try again."
            bank

    match choice with
    | "0" -> ()
    | _ -> bankingLoop updatedBank

// -------------------------------
// Start Program
// -------------------------------

let initialBank : Bank = []

bankingLoop initialBank