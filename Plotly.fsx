#r "nuget: FSharp.Data"
#r "nuget: Plotly.Net"

(*
    https://twitter.com/adelarsq/status/1402483476469854213?s=09
*)

open FSharp.Data
open Plotly.NET

let historicalClosePricesUrl = $"https://api.coindesk.com/v1/bpi/historical/close.json?start=2019-08-10&end=2020-08-11&currency=usd"

type Provider = JsonProvider<"""{"bpi":{"2019-08-10":11289.49,"2019-08-11":11561.71}, "disclaimer":"text","time":{"updated":"Aug 12, 2020 00:03:00 UTC","updatedISO":"2020-08-12T00:03:00+00:00"}}""">

// let parsed = Provider.Parse(result)
let parsed =
    Provider.AsyncLoad historicalClosePricesUrl
    |> Async.RunSynchronously

let xyData = [|
    for k, v in parsed.Bpi.JsonValue.Properties() ->
        k, v.AsDecimal()
|]

let myChart = Chart.Line(xyData)

myChart |> Chart.show