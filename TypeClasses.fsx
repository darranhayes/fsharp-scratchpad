// https://thautwarm.github.io/Site-32/PL/HKT-typeclass-FSharp.html

module HKT =
    open System
    open System.Reflection

    let private ts  = Array.zeroCreate<Type> 0

    type hkt<'K, 'T> = interface end

    [<GeneralizableValue>]
    let getsig<'a> =
        let t = typeof<'a>
        let f = t.GetConstructor(
                    BindingFlags.Instance ||| BindingFlags.Public,
                    null,
                    CallingConventions.HasThis,
                    ts,
                    null)
        let o = f.Invoke([||])
        o :?> 'a


    let inline wrap<'o, ^f, 'a when ^f : (static member wrap : 'o -> hkt<'f, 'a>)> (o: 'o) : hkt< ^f, 'a> =
        (^f : (static member wrap : 'o -> hkt<'f, 'a>) o)

    let inline unwrap<'o, ^f, 'a when ^f : (static member unwrap : hkt<'f, 'a> -> 'o)> (f : hkt< ^f, 'a>) : 'o =
        (^f : (static member unwrap : hkt<'f, 'a> -> 'o) f)

module Show =
    open HKT

    type show<'s> =
        interface
            abstract member show<'a> : hkt<'s, 'a> -> string
        end

    let show<'a, 's when 's :> show<'s>> (a: hkt<'s, 'a>) = getsig<'s>.show a

module Functor =
    open HKT

    [<AbstractClass>]
    type functor<'F>() =
        abstract member fmap<'a, 'b> :
            ('a -> 'b) -> hkt<'F, 'a> -> hkt<'F, 'b>
        abstract member ``<$``<'a, 'b> : 'a -> hkt<'F, 'b> -> hkt<'F, 'a>
        default si.``<$`` a b =
            let const' a _ = a
            (si.fmap << const') a b

    let fmap<'a, 'b, 'F when 'F :> functor<'F>> :
        ('a -> 'b) -> hkt<'F, 'a> -> hkt<'F, 'b> =
        getsig<'F>.fmap


    let ``<$``<'a, 'b, 'F when 'F :> functor<'F> > :
        'a -> hkt<'F, 'b> -> hkt<'F, 'a> = getsig<'F>.``<$``

    let (<<|) a b = ``<$`` a b


module TestTypes =
    open HKT
    open Show

    type TestMap<'k, 'v when 'k : comparison> = {
        Data: Map<'k, 'v>
    } with
        static member inline ( .* ) (l: TestMap<_,_>, r: TestMap<_,_>) =
            TestExpr.HadamardProduct (l, TestExpr.TestMap r)

        static member inline ( .* ) (l: TestMap<_,_>, r: TestExpr<_,_>) =
            TestExpr.HadamardProduct (l, r)

    and TestExpr<'k, 'v when 'k : comparison> =
        | TestMap of TestMap<'k, 'v>
        | HadamardProduct of TestMap<'k, 'v> * TestExpr<'k, 'v>

open TestTypes

(*
    Define a TypeClass that can be applied to Chicken
    Make the TypeClass a parameter of the TestTypes.TestMap type
*)

type Chicken = {
    Size : int
} with
    static member ( * ) (c: Chicken, i: int) =
        { Size = c.Size * i }
    static member ( + ) (c1: Chicken, c2: Chicken) =
        Flock [c1; c2]
    static member ( + ) (c: Chicken, Flock f) =
        Flock (c::f)
and Flock = Flock of Chicken list

let m1 = { Data = Map [(1, 1); (2, 2)]}
let m2 = { Data = Map [(1, { Size = 1 }); (2, { Size = 3 })]}
let m3 = { Data = Map [(1, 3); (2, 4)]}

// let x = m1 .* m2 .* m3 // Compiler gives a type constraint mismatch