(*
    https://demystifyfp.gitbook.io/fstoolkit-errorhandling/
*)

#r "nuget: FsToolkit.ErrorHandling"

open System
open FsToolkit.ErrorHandling

type DomainError =
| Null
| Whitespace of string
| Empty

let notNull<'a when 'a : null> (s: 'a) =
    if isNull s then Error Null else Ok s

let notWhitespace (s: string) =
    if not (isNull s) && String.IsNullOrWhiteSpace s then Error (Whitespace s) else Ok s

let notEmpty (s: string) =
    if not (isNull s) && String.IsNullOrEmpty s then Error Empty else Ok s

type AccountId =
    private | AccountId of string with
    member this.Value =
        let (AccountId accountId) = this
        accountId
    static member Parse (accountId: string) =
        result {
            let! notNull = notNull accountId
            let! notEmpty = notEmpty notNull
            let! notWhitespace = notWhitespace notEmpty
            return AccountId notWhitespace
        }

let result =
    match AccountId.Parse ("123") with
    | Ok id -> id.Value
    | Error reason -> string reason
