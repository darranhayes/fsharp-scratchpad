#!/usr/bin/env -S dotnet fsi
#r "nuget: FSharp.SystemCommandLine,0.17.0-beta4"

(*
    https://twitter.com/angel_d_munoz/status/1677392041544654872?t=Nxp7nt9EvLo7BfYNuIDwhg&s=09
*)
open System.IO
open FSharp.SystemCommandLine

let handler (readFile: FileInfo, printContents) =
    let fileContents = File.ReadAllText(readFile.FullName)

    if printContents then
        printfn $"File contents:\n{fileContents}"
    else
        printfn $"File Extension: {readFile.Extension}"

#if COMPILED
[<EntryPoint>]
#endif
let main argv =
    let readFile =
        Input.Argument<FileInfo>("Read a file from disk")

    let printContents =
        Input.Option<bool>(
            [ "-p"; "--print" ],
            defaultValue = false,
            description = "Print the contents of the file"
        )

    rootCommand argv {
        description "Reads a file using .NET APIs."
        inputs (readFile, printContents)
        setHandler handler
    }

#if INTERACTIVE
(*
    Examples:
    ./CliScript.fsx                # displays help
    ./CliScript.fsx Testing.fsx    # displays file extension
    ./CliScript.fsx Testing.fsx -p # displays file contents
*)
let args = fsi.CommandLineArgs
main (args[1..])
#endif