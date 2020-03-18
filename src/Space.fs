module Space
open Model

let isEmpty (space: Space) = Option.isNone space.Value

let getValue (space: Space) = space.Value