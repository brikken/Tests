// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open TurtleInterpreter

[<EntryPoint>]
let main argv = 
    printfn "%A" TurtleInterpreter.interpret
    0 // return an integer exit code
