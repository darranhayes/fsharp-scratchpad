(*
    https://www.bartoszsypytkowski.com/dealing-with-complex-dependency-injection-in-f/
*)

/// <summary>Shared infrastructure interfaces and logic</summary>
/// <remarks>No concrete dependencies here</remarks>
module Shared =
    [<Interface>]
    type ILogger =
        abstract Info: string -> unit
        abstract Debug: string -> unit
        abstract Warn: string -> unit
        abstract Error: string -> unit

    [<Interface>]
    type ILog =
        abstract Logger: ILogger

    [<Interface>]
    type IDatabase =
        abstract Load: int -> string
        abstract Save: string -> unit

    [<Interface>]
    type IDB =
        abstract Database: IDatabase

    [<Interface>]
    type INotifier =
        abstract Send: string -> unit

    module Log =
        let info (env: ILog) fmt =
            Printf.kprintf env.Logger.Info fmt
        let debug (env: ILog) fmt =
            Printf.kprintf env.Logger.Debug fmt
        let warn (env: ILog) fmt =
            Printf.kprintf env.Logger.Warn fmt
        let error (env: ILog) fmt =
            Printf.kprintf env.Logger.Error fmt

    module DB =
        let load (env: IDB) i =
            env.Database.Load i
        let save (env: IDB) str =
            env.Database.Save str

    module Notifier =
        let send (env: INotifier) e =
            env.Send e

/// <summary>API / controller functions</summary>
/// <remarks>No concrete dependencies</remarks>
module Api =
    open Shared

    let private getById (env: 'a) (id: int) : string =
        Log.debug env "Loading... %i" id
        let existing =
            DB.load env id
        Log.debug env "Loaded: \"%s\"" existing
        existing

    let private save (env: 'a) (newValue: string) : string =
        Log.debug env "Updating... %s" newValue
        DB.save env newValue
        Log.debug env "Updated with: %s" newValue
        newValue

    let private send (env: 'a) (entity: string) : unit =
        Log.debug env "Sending notification for: %s..." entity
        Notifier.send env (sprintf "Dispatched: %s" entity)
        Log.debug env "Sent"

    let sendNotification (env: 'a) (entityId: int) : Result<string, string> =
        Log.info env "Begin Send Notification"

        let result =
            if entityId > 0 then
                let existing = getById env entityId
                send env existing
                Ok(sprintf "Notication sent: %i" entityId)
            else
                Log.debug env "Skipping %i" entityId
                Error(sprintf "Invalid id: %i" entityId)

        Log.info env "End Send Notification"
        result

    let updateEntity (env: 'a) (entityId: int) : Result<string, string> =
        Log.info env "Begin Update"

        let result =
            if entityId > 0 then
                let existing = getById env entityId
                let updated = save env (existing + "_updated")
                Ok("Saved: " + updated)
            else
                Log.debug env "Skipping %i" entityId
                Error(sprintf "Invalid id: %i" entityId)

        Log.info env "End Update"
        result

/// <summary>Runtime Host container</summary>
/// <remarks>Builds container root and supplies concrete dependencies to API functions</remarks>
module RuntimeHost =
    open Shared

    type Logger() =
        interface ILogger with
            member _.Info str = printf "INFO : %s\n" str
            member _.Debug str = printf "DEBUG: %s\n" str
            member _.Warn str = printf "WARN: %s\n " str
            member _.Error str = printf "ERROR: %s\n" str

    type DB() =
        interface IDatabase with
            member _.Load i = ("Entity_" + i.ToString())
            member _.Save _ = ()

    let realNotifier (str: string) : unit =
        ()

    type AppEnv() =
        interface ILog with
            member _.Logger = Logger()
        interface IDB with
            member _.Database = DB()
        interface INotifier with
            member _.Send s = realNotifier s

module TestInfrastructure =
    open Shared

    type TestLogger() =
        interface ILogger with
            member _.Info str = printf "TEST INFO : %s\n" str
            member _.Debug str = printf "TEST DEBUG: %s\n" str
            member _.Warn str = printf "TEST WARN: %s\n " str
            member _.Error str = printf "TEST ERROR: %s\n" str

    type TestDB() =
        interface IDatabase with
            member _.Load i = ("TEST_Entity_" + i.ToString())
            member _.Save _ = ()

/// <summary>Test Scenario Host container</summary>
/// <remarks>Builds container root and supplies concrete dependencies to API functions</remarks>
module TestEntityWorkflow =
    open Shared
    open TestInfrastructure

    type Env() =
        interface ILog with
            member _.Logger = TestLogger()
        interface IDB with
            member _.Database = TestDB()

/// <summary>Test Scenario Host container</summary>
/// <remarks>Builds container root and supplies concrete dependencies to API functions</remarks>
module TestNotificationWorkflow =
    open Shared
    open TestInfrastructure

    type Env() =
        interface ILog with
            member _.Logger = TestLogger()
        interface IDB with
            member _.Database = TestDB()
        interface INotifier with
            member _.Send _ = ()

let entityId = 99
let runtimeEnv = RuntimeHost.AppEnv()

Api.updateEntity runtimeEnv entityId
Api.sendNotification runtimeEnv entityId

Api.updateEntity (TestEntityWorkflow.Env()) entityId
Api.sendNotification (TestNotificationWorkflow.Env()) entityId
