open Model
open System
open System.Text
open System.Diagnostics
open System.IO

let stringBuilderLogger (sb: StringBuilder) =
    fun (board: Board) (hexagons: Hexagon list) ->
        sb.AppendLine(Rendering.renderBoard board) |> ignore
        sb.AppendLine(String.Join(",", seq hexagons)) |> ignore
        sb.AppendLine() |> ignore

let shuffle list =
    let rnd = Random()
    list |> List.sortBy (fun _ -> rnd.Next())

[<EntryPoint>]
let main argv =
    let board = Board.create()
    let sb = StringBuilder()
    let log = stringBuilderLogger sb

    let hexes = [1..19] // |> shuffle
    printfn "%s" (String.Join(",", hexes))
    
    let sw = Stopwatch.StartNew()
    let solved = Board.solve hexes log board
    sw.Stop()
    printfn "%i ms" sw.ElapsedMilliseconds

    File.WriteAllText("hexagons2.txt", sb.ToString())

    match solved with
    | Some res -> Rendering.printBoard res
    | None -> ()

    0
