module TypeTest

let func (a:int) =
    a + 10

let func2 (b:float) =
    b * 2.0

let var1 = 10:int

let func3 c : float =
    c + 10.0

let func4 (d:int) : float =
    float d

let func5 a =
    if a > 10 then 
        Some (a - 1)
    else
        None

type MaybeBuilder() =

    member this.Bind(x, f) = 
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) = 
        Some x
   
let maybe = new MaybeBuilder()

let func6 a = maybe {
    let! b = func5 a
    let! c = func5 b
    return c
}

let func7 a = id a
