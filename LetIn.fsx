// https://stackoverflow.com/questions/25249332/desugared-form-of-let-in

// let x = <val> in <expr>
// is syntactically equivalent to:
// (fun x -> <expr>) <val>

// Lightweight syntax:
let f1 x =
    let a = 1
    let b = 2
    x + a + b

// Verbose syntax
let f2 x =
    let a = 1 in
    let b = 2 in
    x + a + b

// But, there are some differences with generics, example below does not compile.
// (id cannot be generalised, its type is restricted to first instance of int):

// (fun id -> ignore (id 1); ignore (id "A"))

// This does compile:
let id = (fun x -> x) in
    ignore (id 1);
    ignore (id "A")