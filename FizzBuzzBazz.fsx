let rules = [(3, "Fizz"); (5, "Buzz"); (9, "Bazz");]

let fizzBuzz n =
    let folder s (d, r) =
        if n % d = 0 then
            s + r
        else
            s

    let str = rules |> List.fold folder ""

    if str = "" then
        string n
    else
        str

[1..100] |> List.map fizzBuzz |> printfn "%A"