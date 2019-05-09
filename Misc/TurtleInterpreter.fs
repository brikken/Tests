module TurtleInterpreter

    type TurtleCommand =
        | Move of Distance
        | Turn of Angle
        | PenUp
        | PenDown
        | SetColor of PenColor

    and Distance = int

    and PenColor =
        | Blue
        | Red
        | Green

    and Angle = int


    type TurtleResponse =
        | Moved of MoveResponse
        | Turned
        | PenWentUp
        | PenWentDown
        | ColorSet of SetColorResponse

    and MoveResponse =
        | MoveResponse of Distance

    and SetColorResponse =
        | PenStuck
        | ColorChosen of PenColor


    type TurtleProgram =
        | Move of Distance * (MoveResponse -> TurtleProgram)
        | Turn of Angle * (unit -> TurtleProgram)
        | PenUp of        (unit -> TurtleProgram)
        | PenDown of      (unit -> TurtleProgram)
        | SetColor of PenColor * (SetColorResponse -> TurtleProgram)
        | Stop

    let drawTriangle =
        Move (100, fun _ ->
        Turn (120, fun _ ->
        Move (100, fun _ ->
        Turn (120, fun _ ->
        Move (100, fun _ ->
        Stop)))))

    type InterpreterDistance = (Distance -> TurtleProgram) -> Distance
    let rec interpreterDist state program =
        match program with
            | Move (dist, next) ->
                let stateNew = state + dist
                let programNew = next (MoveResponse dist)
                interpreterDist stateNew programNew
            | Turn (angle, next) ->
                let programNew = next ()
                interpreterDist state programNew
            | PenUp (next) ->
                let programNew = next ()
                interpreterDist state programNew
            | PenDown (next) ->
                let programNew = next ()
                interpreterDist state programNew
            | SetColor (clr, next) ->
                let programNew = next (ColorChosen clr)
                interpreterDist state programNew
            | Stop ->
                state

    let program = drawTriangle
    let interpreter = interpreterDist 0
    let interpret = interpreter program
