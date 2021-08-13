#r "nuget: Unquote"
#r "nuget: Xunit"

(*
    BDD EventSourcing and testing Computation Expression
    https://twitter.com/akhansari/status/1422343744561590277
*)
module SpecTest =
    open Swensen.Unquote

    type SpecState<'State, 'Outcome> = private {
        State: 'State
        Outcome: 'Outcome
    }

    type GivenState<'State, 'Outcome> = private GivenState of SpecState<'State, 'Outcome>
    type WhenState<'State, 'Outcome> = private WhenState of SpecState<'State, 'Outcome>
    type ThenState<'State, 'Outcome> = private ThenState of SpecState<'State, 'Outcome>

    type Spec<'State, 'Command, 'Event>
        (   initialState: 'State,
            decide: 'Command -> 'State -> 'Event list,
            evolve: 'State -> 'Event -> 'State ) =

        let evolver state events = List.fold evolve state events

        member _.Yield _ = GivenState {
            State = initialState
            Outcome = []
        }

        [<CustomOperation "Given">]
        member _.Given (GivenState spec, events) =
            WhenState
                { spec with
                    State = evolver spec.State events }

        [<CustomOperation "When">]
        member _.When (WhenState spec, command) =
            let events = decide command spec.State
            ThenState
                { spec with
                    State = evolver spec.State events
                    Outcome = events }

        [<CustomOperation "Then">]
        member _.Then (ThenState spec, expected) =
            let events = spec.Outcome
            test <@ events = expected @>
            ThenState spec

        [<CustomOperation "ThenState">]
        member _.ThenState (ThenState spec, expected) =
            let state = spec.State
            test <@ state = expected @>
            ThenState spec

module Domain =
    module Commands =
        type AgreementCommand =
            | Create of CreateAgreement
            | SignAndPublish of SignAgreement
        and CreateAgreement = {
            Creator: string
        }
        and SignAgreement = {
            Signatury: string
        }

    module Events =
        type AgreementEvent =
            | Created of AgreementCreated
            | Signed of AgreementSigned
        and AgreementCreated = {
            CreatedBy: string
        }
        and AgreementSigned = {
            SignedBy: string
        }

    module State =
        type AgreementState =
            | Initial
            | Created of CreatedState
            | Signed of SignedState
        and CreatedState = {
            CreatedBy: string
        }
        and SignedState = {
            CreatedBy: string
            SignedBy: string
        }

open Domain.Commands
open Domain.Events
open Domain.State

let decide (command: AgreementCommand) (state: AgreementState) =
    match command with
    | Create cmd -> [ AgreementEvent.Created({ AgreementCreated.CreatedBy = cmd.Creator }) ]
    | SignAndPublish cmd -> [ AgreementEvent.Signed({ AgreementSigned.SignedBy = cmd.Signatury }) ]

let evolve (state: AgreementState) (event: AgreementEvent) =
    match (event, state) with
    | (AgreementEvent.Created event, Initial) ->
        Created({ CreatedBy = event.CreatedBy })
    | (AgreementEvent.Signed event, Created state) ->
        Signed({ CreatedBy = state.CreatedBy; SignedBy = event.SignedBy })
    | _,_ -> failwith $"Invalid event & state combination. Event: %O{event}; State: %O{state}"

open SpecTest
open Xunit

let spec = Spec (AgreementState.Initial, decide, evolve)

[<Fact>]
let ``Create an agreement`` () =
    spec {
        Given [ ]
        When (
            AgreementCommand.Create({ Creator = "Darran" })
        )
        Then [
            AgreementEvent.Created({ AgreementCreated.CreatedBy = "Darran" })
        ]
        ThenState (
            AgreementState.Created({ CreatedBy = "Darran" })
        )
    }

[<Fact>]
let ``Sign an agreement`` () =
    spec {
        Given [
            AgreementEvent.Created({ CreatedBy = "Darran" })
        ]
        When (
            AgreementCommand.SignAndPublish({ Signatury = "Ann"})
        )
        Then [
            AgreementEvent.Signed({ SignedBy = "Ann"})
        ]
        ThenState (
            AgreementState.Signed({ CreatedBy = "Darran"; SignedBy = "Ann" })
        )
    }

``Create an agreement``()
``Sign an agreement``()

(*
TODO:
- Improve modularisation of commands, events, states
- Add Applicative Smart Constructors on Command types
*)