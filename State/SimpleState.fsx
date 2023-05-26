// Simple stateful objects by Ronald Schlenker
let counter (initalValue: int) incrementValue =
    let mutable state = initalValue - incrementValue
    fun () ->
        state <- state + incrementValue
        state

let composedCounter () =
    let c0By1 = counter 0 1
    let c100By10 = counter 100 10
    fun () ->
        let v1 = c0By1()
        let v2 = c100By10()
        v1 + v2

let cc = composedCounter ()

cc() // revist/call/evaluate this every time for next value

// downsides: error prone manual composition