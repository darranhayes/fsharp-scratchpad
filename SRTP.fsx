module NameOps =
    let inline private (|HasName|) x = (^a : (member Name: string) x)
    let inline private (|HasAge|) x = (^a : (member Age: int) x)
    let inline printName (HasName name & HasAge age) = sprintf $"{name} is %i{age} years old"

type Person = {
    Name: string
    Age: int
}

let p1 = { Name = "Alice"; Age = 40}
let p2 = {| Name = "Bob"; Age = 41 |}
let random = {| Name = "John" |}

NameOps.printName p1
NameOps.printName p2
// NameOps.printName random
