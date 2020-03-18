module BoardTests

open Model
open FsUnit
open Xunit
open System.Text

[<Fact>]
let ``Update sets values`` () =
    let board = Board.create()

    let updated = Board.update (0, 0) 1 board

    updated.[0].[0].Value |> should equal (Some 1)
    updated.[0].[1].Value |> should equal None
    updated.[1].[0].Value |> should equal None

[<Fact(Timeout = 20000)>]
let ``Solve returns result`` () =
    let board = Board.create()
    let nullLog _ _ = ()

    let solved = Board.solve [1..19] nullLog board

    solved |> should not' (equal None)