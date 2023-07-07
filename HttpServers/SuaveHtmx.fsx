#r "nuget: Suave"
#r "nuget: Feliz.ViewEngine.Htmx"

(*
    https://jkone27-3876.medium.com/htmx-and-f-c1ffdc18fbb5
*)

open System
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Feliz.ViewEngine
open Feliz.ViewEngine.Htmx

let body =
    [
        Html.h1 "HTMX is cool"
        Html.button [
            hx.get "/clicked"
            hx.swap.outerHTML
            hx.trigger "click"
            hx.target "#result"
            prop.text "HTTP GET TO SERVER HTML RESPONSE"
        ]
        Html.div [
            prop.id "result"
        ]
    ]

let mainLayout =
    Html.html [
        Html.head [
            Html.title "F# ❤ HTMX"
            Html.script [ prop.src "https://unpkg.com/htmx.org@1.6.0" ]
            Html.link [
                prop.rel "stylesheet"
                prop.href "https://unpkg.com/missing.css@1.0.9/dist/missing.min.css"
            ]
            Html.meta [ prop.charset.utf8 ]
        ]
        Html.body body
    ]
    |> Render.htmlView

let mainApp =
    path "/"
    >=> OK mainLayout

let ssr =
    Html.ul [
        Html.li "H"
        Html.li "T"
        Html.li "M"
        Html.li "X"
    ]
    |> Render.htmlView

let getResponseApp =
    path "/clicked"
    >=> OK ssr

let app =
    GET >=> choose [
        mainApp
        getResponseApp
    ]

startWebServer defaultConfig app
