type User = { Name: string; Age: int }

let users = [
    { Name = "Alice"; Age = 25 }
    { Name = "Bob"; Age = 30 }
    { Name = "Charlie"; Age = 35 }
]

let found = List.contains { Name = "Bob"; Age = 30 } users

printfn "%A" found