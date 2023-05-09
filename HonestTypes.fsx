(*
    https://demystifyfp.gitbook.io/fstoolkit-errorhandling/
    https://www.compositional-it.com/news-blog/validation-with-f-5-and-fstoolkit/
*)

#r "nuget: FsToolkit.ErrorHandling"

open System
open FsToolkit.ErrorHandling

type DomainError =
| Null
| Whitespace of string
| Empty
| MinLength of int

let notNull<'a when 'a : null> (s: 'a) =
    if isNull s then
        Error Null
    else
        Ok s

let notWhitespace (s: string) =
    if not (isNull s) && String.IsNullOrWhiteSpace s then
        Error (Whitespace s)
    else
        Ok s

let notEmpty (s: string) =
    if not (isNull s) && String.IsNullOrEmpty s then
        Error Empty
    else
        Ok s

let minChars (length: int) (s: string) =
    if s.Length < length then
        Error (MinLength length)
    else
        Ok s

type AccountId =
    private | AccountId of string with
    member this.Value =
        let (AccountId accountId) = this
        accountId
    static member Parse (accountId: string) =
        validation {
            let! _ = notNull accountId
            and! _ = notEmpty accountId
            and! _ = notWhitespace accountId
            and! _ = minChars 3 accountId
            return accountId
        }

match AccountId.Parse ("  f ") with
| Ok id -> id
| Error reason -> string reason
