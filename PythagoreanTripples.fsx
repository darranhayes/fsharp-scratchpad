/// Original taken from
/// https://twitter.com/loddity/status/1670441316281401346?t=lywnyy4kyAZdD9Nb4TH-dQ&s=09
let rec findPythagoreanTriplesOriginal z = // no recursion needed
    let rec findSquare a b =
        let beta = (a * a) + (b * b)
        match beta with
        | beta' when beta' = z * z ->
            Some (a, b)
        | beta' when beta' > z * z ->
            None
        | _ ->
            match beta with
            | beta'' when beta'' > z * z -> None // duplicated case, no need for nested match
            | _ ->
                findSquare (a + 1) b
                |> Option.orElse (findSquare a (b + 1))
    findSquare 1 1, z // added tuple value z for reporting purposes

let findPythagoreanTriples z =
    let rec findSquare a b =
        let beta = (a * a) + (b * b)
        match beta with
        | beta' when beta' = z * z ->
            Some (a, b)
        | beta' when beta' > z * z ->
            None
        | _ ->
            findSquare (a + 1) b
            |> Option.orElse (findSquare a (b + 1))
    findSquare 1 1, z

let report = function
    | Some(x, y), z -> printfn "x = %d, y = %d, z = %d" x y z
    | None, z -> printfn "No solution found for z = %d" z

[1..20] |> List.iter (findPythagoreanTriplesOriginal >> report)
[1..20] |> List.iter (findPythagoreanTriples >> report)