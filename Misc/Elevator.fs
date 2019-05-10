module Elevator

open System

type [<Measure>] Meters
type [<Measure>] Seconds

type Position = float<Meters>

type Distance = float<Meters>

type Direction = Up | Down

type Request = {
    position: Position
    direction: Direction
 }

type Run = {
    duration: float<Seconds>
}

type State =
    | Stopped
    | Moving of direction: Direction

type ElevatorState = {
    position: Position
    state: State
    requests: List<Request>
}

let initialPosition = 0.0<Meters>

let initialState = {
    position = initialPosition
    state = Stopped
    requests = []
}

let speed = 3.0<Meters> / 1.0<Seconds>

let positionPredicate direction =
    match direction with
        | Down -> (>=)
        | Up -> (<)

let distance direction absoluteDistance =
    match direction with
        | Down -> -absoluteDistance
        | Up -> absoluteDistance

let getDirection position destination =
    if position < destination then Up else Down

let addRequest request state : ElevatorState =
    {state with requests = request::state.requests }

let absoluteDistance position destination =
    if position > destination then position - destination else destination - position

let nearestRequest state : option<Request> =
    match state.requests with
        | []  -> None
        | _ ->
            match state.state with
                | Stopped ->
                    let nearest =
                        state.requests
                        |> List.minBy (fun r -> absoluteDistance r.position state.position)
                    Some { nearest with direction = getDirection state.position nearest.position }
                | Moving direction ->
                    let nearest =
                        state.requests
                        |> List.filter (fun r -> ((positionPredicate direction) state.position) r.position)
                        |> List.minBy (fun r -> absoluteDistance r.position state.position)
                    Some { nearest with direction = getDirection state.position nearest.position }

let rec runElevator run state : ElevatorState =
    let stateNew = { state with requests = List.filter (fun r -> r.position <> state.position) state.requests }
    if run.duration <= 0.0<Seconds> then
        stateNew
    else
        match nearestRequest stateNew with
            | None -> runElevator run { stateNew with state = Stopped }
            | Some r -> 
        //let duration = run.duration
        //let destination =
        //    match nearestRequest stateNew with
        //        | None -> { position = initialPosition; direction = getDirection stateNew.position initialPosition }
        //        | Some r -> r
        //let timeToTravel = (absoluteDistance stateNew.position destination.position) / speed
        //let actualTime = if duration < timeToTravel then duration else timeToTravel
        //let absoluteDistance = actualTime * speed
        //let actualDistance = distance destination.direction absoluteDistance
        //let stateNew2 = { stateNew with position = stateNew.position + actualDistance }
        //let runNew = { run with duration = run.duration - actualTime }
        //runElevator runNew stateNew2