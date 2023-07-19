#r "nuget: Akkling"

(*
    Getting started with Akka by Onur Gümüş
    https://twitter.com/OnurGumusDev/status/1387719573521289219
*)

open System
open Akkling

let system = System.create "basic-sys" <| Configuration.defaultConfig()

let behaviour (m: Actor<_>) =
    let rec loop () = actor {
        let! msg = m.Receive()

        match msg with
        | "stop" -> return Stop
        | "unhandle" -> return Unhandled
        | x ->
            printfn "%s" x
            return! loop ()
    }
    loop ()

let helloRef = spawnAnonymous system (props behaviour)
helloRef <! "ok"
