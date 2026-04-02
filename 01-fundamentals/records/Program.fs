// =======================
// Records in F#
// =======================

type Student = {
    Name: string
    Age: int
    Grade: float
}

// Create records
let student1 = { Name = "Ali"; Age = 21; Grade = 85.5 }
let student2 = { Name = "Sara"; Age = 22; Grade = 91.0 }

// Print
let printStudent s =
    printfn "Name: %s" s.Name
    printfn "Age: %d" s.Age
    printfn "Grade: %.2f" s.Grade
    printfn "------------------"

printStudent student1
printStudent student2

// Update record
let updatedStudent = { student1 with Grade = 90.0 }

printfn "Updated Student:"
printStudent updatedStudent

// List of records
let students = [student1; student2]

// Filter students with grade > 90
let topStudents =
    students
    |> List.filter (fun s -> s.Grade > 90.0)

printfn "Top Students:"
topStudents |> List.iter printStudent