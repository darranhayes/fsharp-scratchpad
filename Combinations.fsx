let rec distribute e = function
    | [] -> [[e]]
    | x::xs' as xs -> (e::xs)::[for xs in distribute e xs' -> x::xs]

let rec permute = function
    | [] -> [[]]
    | e::xs -> List.collect (distribute e) (permute xs)

let rec comb n l =
    match n, l with
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> List.map ((@) [x]) (comb (k-1) xs) @ comb k xs

permute [3; 3; 7; 11; 13]
comb 5 [3; 3; 7; 11; 13]
comb 4 [3; 3; 7; 11; 13]

// Pascal's triangle for moves in a 6*6 grid. From start position
// https://twitter.com/koya83184827/status/1655178859334053893?t=JC68P9PN8ArNQ8-nmrWNhg&s=09
// there's always 5 up moves, and 5 moves sideways, so there's 5 choices from 10:
// (10)
// ( 5) -> 252
comb 5 [1..10] |> List.length // 252