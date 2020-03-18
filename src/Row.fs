module Row
open Model

let MagicNumber = 38

let create (index, spaces): Row = List.init spaces (fun i -> { Position = (index, i); Value = None })

let countEmptySpaces (row: Row) = row |> List.filter Space.isEmpty |> List.length

let getState (hexagons: Hexagon list) (row: Row) =
    let (fullSpaces, emptySpaces) = row |> List.map Space.getValue |> List.partition Option.isSome
    let sum = fullSpaces |> List.sumBy Option.get |> int
    if List.isEmpty emptySpaces then
        if sum = MagicNumber then Complete
        else Invalid
    else
        let target = MagicNumber - sum
        if hexagons |> List.exists (fun h -> h <= target) then Completable
        else Invalid
