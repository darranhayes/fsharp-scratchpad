(*
    https://jkone27-3876.medium.com/net-script-apis-92b3414365f5

    To add AspNetCode dependencies to be consumed by scripts:
        curl https://gist.githubusercontent.com/jkone27/e3f9ca1dbba69c602a3eaa3563ab2bdb/raw/c720d21208410c93fd72ec3dcef556ade065ecc4/generateAspnetcore.fsx > generate-sdk-references.fsx
        dotnet fsi generateAspnetcore.fsx # creates framework fsx files in runtime-scripts/
        echo "runtime-scripts/" >> .gitignore
*)

#load "../runtime-scripts/Microsoft.AspNetCore.App-7.0.7.fsx"
#r "nuget: Saturn"

open Saturn
open Giraffe

let app = application {
    use_router (text "Hello World from Saturn")
}

run app