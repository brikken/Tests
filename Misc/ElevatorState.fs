module ElevatorState

    open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

    type Position = float<m>

    type Direction = Up | Down

    type Request = {
        position: Position
        direction: Direction
     }

    type Requests = private {
        current: Request
        rest: List<Request>
    }

    module Requests =
        let create List<Request> : Result<Requests,string>

    type ElevatorState =
        | AtInitialPosition
        | MovingInitialPosition of Position
        | Moving of Position * Requests