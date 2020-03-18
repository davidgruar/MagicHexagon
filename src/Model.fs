module Model

type Position = int * int
type Hexagon = int
type Space = { Position: Position; Value: Hexagon option }
type Row = Space list
type Board = Row list
type Diagonal = Position list

type RowState =
    | Complete
    | Completable
    | Invalid
