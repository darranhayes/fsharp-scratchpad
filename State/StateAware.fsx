
(*
    Ronald Schlenker
    Goal to implement something like this:

let counter : StateAware<int> =
    let! c1 : int = (counter 0 1) : StateAware<int>
    let! c2 : int = (counter 100 10) : StateAware<int>
    return (c1 + c2)
*)

// takes optional initial state, returns current value and next state
type StateAware<'value, 'state> = 'state option -> 'value * 'state

let bind (f: 'v1 -> StateAware<'v2, 's2>) (x: StateAware<'v1,'s1>) : StateAware<'v2, 's1 * 's2> =
    fun state ->
        let lastXState, lastFState =
            match state with
            | Some(s1, s2) -> Some s1, Some s2
            | None -> None, None

        let xResult, newXState = x lastXState

        let fStateAware = f xResult
        let fResult, newFState = fStateAware lastFState

        fResult, (newXState, newFState)

let ret x = fun _ -> x, x

type StateAwareBuilder () =
    member _.Bind(m, f) = bind f m
    member _.Return (x) = ret x

let stateAware = StateAwareBuilder()

let counter (initial: int) increment : StateAware<int, int> =
    fun (state: int option) ->
        let state = state |> Option.defaultValue (initial - increment)
        let newState = state + increment
        newState, newState

let compositeCounter =
    stateAware {
        let! c1 = counter 0 1
        let! c2 = counter 100 10
        let! c3 = counter 1000 100
        return c1 + c2 + c3
    }

let toSeq stateAware =
    seq {
        let mutable state = None
        while true do
            let value, newState = stateAware state
            do state <- Some newState
            yield value
    }

compositeCounter |> toSeq |> Seq.take 5