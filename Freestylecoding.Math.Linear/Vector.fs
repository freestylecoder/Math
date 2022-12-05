namespace Freestylecoding.Math.Linear

open Freestylecoding.Math

type public Vector<'T when 'T : equality>( input:'T[] ) =
    member internal this.N = input.Length
    member internal this.Data = input

    member this.Item
        with get i = this.Data.[i]
    
    new( input:Vector<'T> ) = Vector( input.Data )
    new( n:int32, fill:'T ) = Vector( [| for _ in 1 .. n -> fill |] )
    new( n:int32, fill:(int32 -> 'T) ) = Vector( [| for i in 1 .. n -> fill i |] )

    with
        static member op_Equality (left:Vector<'T>, right:Vector<'T>) : bool =
            if left.N <> right.N
            then false
            else
                Array.map2 ( fun x y -> x = y ) left.Data right.Data
                |> Array.reduce ( fun x y -> x && y )

        static member (+) (left:Vector<Real>, right:Vector<Real>) : Vector<Real> =
            if left.N <> right.N
            then raise (new System.ArgumentException( "Vectors must be the same size" ))
            else
                Vector( Array.map2 ( fun x y -> x + y ) left.Data right.Data )

        static member (+) (left:Vector<Rational>, right:Vector<Rational>) : Vector<Rational> =
            if left.N <> right.N
            then raise (new System.ArgumentException( "Vectors must be the same size" ))
            else
                Vector( Array.map2 ( fun x y -> x + y ) left.Data right.Data )

        static member (+) (left:Vector<Integer>, right:Vector<Integer>) : Vector<Integer> =
            if left.N <> right.N
            then raise (new System.ArgumentException( "Vectors must be the same size" ))
            else
                Vector( Array.map2 ( fun x y -> x + y ) left.Data right.Data )

    //    static member (*) (left:Real, right:Vector) : Vector =
    //        Vector( right.N ).Copy( Array.map ( fun r -> left * r ) right.Data )

    //    static member (*) (left:Vector, right:Real) : Vector =
    //        right * left

    //    static member (*) (left:Vector, right:Vector) : Real =
    //        if left.N <> right.N
    //        then raise (new System.ArgumentException( "Vectors must be the same size" ))
    //        else
    //            Array.map2 ( fun x y -> x * y ) left.Data right.Data
    //            |> Array.sum
