open System

let calculateDateRanges (nextDateCalculator: DateTime -> DateTime) (startDate: DateTime) (endDate: DateTime) =
    let format (dt: DateTime) = dt.ToString "yyyy-MM-dd HH:mm:ss"

    let generator = fun (state : DateTime) ->
        if state > endDate then
            None
        else
            Some (state, nextDateCalculator state)

    startDate
    |> List.unfold generator
    |> List.map (fun x -> (x, (nextDateCalculator x).AddSeconds(-1)))
    |> List.map (fun (x, y) -> (format x, format y))

let threeMonthCalculator = fun (x: DateTime) -> x.AddMonths(3)

calculateDateRanges threeMonthCalculator (DateTime.Parse "2022-10-19 00:00:00") DateTime.Today
