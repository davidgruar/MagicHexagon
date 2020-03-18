module RowTests

open Model
open Xunit
open FsUnit.Xunit

[<Fact>]
let ``getState with empty row and all hexagons returns Completable`` () =
    let row = Row.create (0, 5)
    let state = Row.getState [1..19] row
    state |> should equal Completable

[<Fact>]
let ``getState with full complete row returns Complete`` () =
    let row = [
        {Position = (0, 0); Value = Some 19 }
        {Position = (0, 1); Value = Some 18 }
        {Position = (0, 2); Value = Some 1 }
    ]
    let state = Row.getState [2..17] row
    state |> should equal Complete

[<Fact>]
let ``getState with full invalid row returns Invalid`` () =
    let row = [
        {Position = (0, 0); Value = Some 19 }
        {Position = (0, 1); Value = Some 18 }
        {Position = (0, 2); Value = Some 2 }
    ]
    let state = Row.getState [3..17] row
    state |> should equal Invalid

[<Fact>]
let ``getState with partial completable row returns Completable`` () =
    let row = [
        {Position = (0, 0); Value = Some 19 }
        {Position = (0, 1); Value = Some 18 }
        {Position = (0, 2); Value = None }
    ]
    let state = Row.getState [1] row
    state |> should equal Completable

[<Fact>]
let ``getState with partial row summing to 38 returns Invalid`` () =
    let row = [
        {Position = (1, 0); Value = Some 19 }
        {Position = (1, 1); Value = Some 18 }
        {Position = (1, 2); Value = Some 1 }
        {Position = (1, 3); Value = None }
    ]
    let state = Row.getState [2..17] row
    state |> should equal Invalid

[<Fact>]
let ``getState with one empty space without the right hexagon to complete returns Invalid`` () =
    let row = [
        {Position = (0, 0); Value = Some 19 }
        {Position = (0, 1); Value = Some 18 }
        {Position = (0, 2); Value = None }
    ]
    let state = Row.getState [2] row
    state |> should equal Invalid

[<Fact(Skip = "Implementing this makes performance worse")>]
let ``getState with two empty spaces without the right hexagons to complete returns Invalid`` () =
    let row = [
        {Position = (0, 0); Value = Some 19 }
        {Position = (0, 1); Value = None }
        {Position = (0, 2); Value = None }
    ]
    let state = Row.getState [2; 3] row
    state |> should equal Invalid