// A canonical modulus operator
let (%%) a b =
    let c = a % b;
    if ((c < 0 && b > 0) || (c > 0 && b < 0)) then
        c + b;
    else
        c

let list =
    List.init 10 (fun x -> x - 5)

// remainer
list |> List.map (fun x -> x % 10)
// [-5; -4; -3; -2; -1; 0; 1; 2; 3; 4]

// modulus
list |> List.map (fun x -> x %% 10)
// [5; 6; 7; 8; 9; 0; 1; 2; 3; 4]

// https://learn.microsoft.com/en-us/archive/blogs/ericlippert/whats-the-difference-remainder-vs-modulus

// we want 123/4 to be 30 with a remainder of 3, not 31 with a remainder of -1.
(123 / 4, 123 % 4) // = (30, 3)
(123 / 4, 123 %% 4) // = (30, 3)
// If -123/4 is -30, then what is the remainder? It must obey the identity, so the remainder is -3.
(-123 / 4, -123 % 4) // = (-30, -3)
// That is not the canonical item associated with the equivalence class that contains -123; that canonical item is 1.
(-123 / 4, -123 %% 4) // = (-30, 1)

// https://numerics.mathdotnet.com/Euclid
(*
// remainder
 5 %  3 // =  2, such that 5 = 1*3 + 2
-5 %  3 // = -2, such that -5 = -1*3 - 2
 5 % -3 // =  2, such that 5 = -1*-3 + 2
-5 % -3 // = -2, such that -5 = 1*-3 - 2

// modulus
 5 %% 3 // =  2, congruent modulo 3 by 5 - 1*3
-5 %% 3 // =  1, congruent modulo 3 by -5 + 2*3
 5 %% -3 // = -1, congruent modulo -3 by 5 + 2*-3
-5 %% -3 // = -2, congruent modulo -3 by -5 - 1*-3
*)