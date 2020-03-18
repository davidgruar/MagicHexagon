module Board
open System.Text
open Model

let diagonals1 = [
    [0,0; 1,0; 2,0]
    [0,1; 1,1; 2,1; 3,0]
    [0,2; 1,2; 2,2; 3,1; 4,0]
    [1,3; 2,3; 3,2; 4,1]
    [2,4; 3,3; 4,2]
]

let diagonals2 = [
    [0,2; 1,3; 2,4]
    [0,1; 1,2; 2,3; 3,3]
    [0,0; 1,1; 2,2; 3,2; 4,2]
    [1,0; 2,1; 3,1; 4,1]
    [2,0; 3,0; 4,0]
]

let create() =
    [3; 4; 5; 4; 3] |> List.mapi (fun i spaces -> Row.create (i, spaces))

// Can this be made simpler?
let update ((rowIndex, index): Position) (value: Hexagon) (board: Board): Board =
    let row = board.[rowIndex]
    let updatedRow = row |> List.mapi (fun i elem -> if i = index then { elem with Value = Some value } else elem)
    board |> List.mapi (fun i elem -> if i = rowIndex then updatedRow else elem)


let getDiagonal (diagonal: Diagonal) (board: Board): Row =
    diagonal |> List.map (fun (row, index) -> board.[row].[index])

let getDiagonals (diagonals: Diagonal seq) (board: Board) =
    diagonals |> Seq.map (fun d -> getDiagonal d board)

let getAllRows (board: Board) =
    Seq.concat [seq board; (getDiagonals diagonals1 board); (getDiagonals diagonals2 board)]

let getEmptyPosition board: Position option =
    getAllRows board
    |> Seq.map (fun row -> (row, Row.countEmptySpaces row))
    |> Seq.filter (fun (_, count) -> count > 0)
    |> Seq.sortBy snd
    |> Seq.tryHead
    |> Option.map (fun (row, _) ->
                    let space = row |> List.find Space.isEmpty
                    space.Position)

let getState (hexagons: Hexagon list) (board: Board) =
    let rowStates = board
                    |> getAllRows
                    |> Seq.map (Row.getState hexagons)
    if Seq.contains Invalid rowStates then Invalid
    elif Seq.contains Completable rowStates then Completable
    else Complete

let rec solve (hexagons: Hexagon list) (log: Board -> Hexagon list -> Unit) board =
    let emptyPosition = getEmptyPosition board
    match (emptyPosition, hexagons) with
    | (None, []) -> Some board
    | (None, _) -> invalidOp "Board is full, but there are still hexagons to place"
    | (Some _, []) -> invalidOp "Board is not full, but there are no hexagons left"
    | (Some pos, _) ->
        let rec inner nextHex rest =
            let nextBoard = update pos nextHex board
            log nextBoard rest
            match getState rest nextBoard with
            | Complete -> Some nextBoard
            | Completable -> solve rest log nextBoard
            | Invalid -> None

        let validHexes = List.headsAndTails hexagons
        let solved = validHexes |> Seq.map (fun (hd, tl) -> inner hd tl)
        let res = solved |> Seq.tryPick id
        res
