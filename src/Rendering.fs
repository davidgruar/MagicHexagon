module Rendering
open System
open Model

let renderHex hex =
    match hex with
    | None -> " - "
    | Some i when i > 9 -> sprintf "%i " i
    | Some i -> sprintf " %i " i

let renderRow (row: Row) =
    let rowStr = String.Join("", List.map (Space.getValue >> renderHex) row)
    let padding = String.replicate (5 - row.Length) " "
    sprintf "%s%s" padding rowStr

let renderBoard (board: Board) = String.Join(Environment.NewLine, List.map renderRow board)

let printBoard = renderBoard >> printfn "%s"
