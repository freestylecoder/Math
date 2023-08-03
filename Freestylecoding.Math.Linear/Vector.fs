namespace Freestylecoding.Math.Linear

open System.Numerics
open Freestylecoding.Math

type public Vector( input:Real array ) =

    member internal this.N = input.Length
    member internal this.Data = input

    member this.Item
        with get i = this.Data.[i]
        and set i value = this.Data.[i] <- value
    
    member this.Length
        with get () = this.Data.Length

    new( input:Vector ) = Vector( input.Data )
    new( n:int32, fill:Real ) = Vector( [| for _ in 1 .. n -> fill |] )
    new( n:int32, fill:(int32 -> Real) ) = Vector( [| for i in 0 .. ( n - 1 ) -> fill i |] )

    with
        static member op_Equality (left:Vector, right:Vector) : bool =
            if left.N <> right.N
            then false
            else
                Array.map2 ( fun x y -> x = y ) left.Data right.Data
                |> Array.reduce ( fun x y -> x && y )

        static member (+) (left:Vector, right:Vector) : Vector =
            if left.N <> right.N
            then raise (new System.ArgumentException( "Vectors must be the same size" ))
            else
                Vector( Array.map2 ( fun x y -> x + y ) left.Data right.Data )

        static member (*) (left:Vector, right:Vector) : Real =
            if left.N <> right.N
            then raise (new System.ArgumentException( "Vectors must be the same size" ))
            else
                Array.map2 ( fun x y -> x * y ) left.Data right.Data
                |> Array.sum

        static member (*) (left:Real, right:Vector) : Vector =
            Vector( Array.map ( fun x -> left * x ) right.Data )

        static member (*) (left:Vector, right:Real) : Vector =
            right * left

        member this.Apply (mapping:System.Func<Real,Real>) : Vector =
            Vector( Array.map ( fun x -> mapping.Invoke( x ) ) this.Data )

type public Vector<'T when 'T :> System.Numerics.INumber<'T>>( input:'T array ) =
    member internal this.N = input.Length
    member internal this.Data = input

    member this.Item
        with get i = this.Data.[i]
        and set i value = this.Data.[i] <- value
    
    member this.Length
        with get () = this.Data.Length

    new( input:Vector<'T> ) = Vector<'T>( input.Data )
    new( n:int32, fill:'T ) = Vector<'T>( [| for _ in 1 .. n -> fill |] )
    new( n:int32, fill:System.Func<int32,'T> ) = Vector<'T>( [| for i in 0 .. ( n - 1 ) -> fill.Invoke( i ) |] )

    with
        static member op_Equality<'T> (left:Vector<'T>, right:Vector<'T>) : bool =
            if left.N <> right.N
            then false
            else
                Array.map2 ( fun (x:'T) (y:'T) -> x.Equals( y ) ) left.Data right.Data
                |> Array.reduce ( fun x y -> x && y )

        static member (*) (left:'T, right:Vector<'T>) : Vector<'T> =
            Vector<'T>( Array.map ( fun (x:'T) -> left * x ) right.Data )

        static member (*) (left:Vector<'T>, right:'T) : Vector<'T> =
            Vector<'T>( Array.map ( fun (x:'T) -> right * x ) left.Data )

        static member (/) (left:'T, right:Vector<'T>) : Vector<'T> =
            Vector<'T>( Array.map ( fun (x:'T) -> left / x ) right.Data )

        static member (/) (left:Vector<'T>, right:'T) : Vector<'T> =
            Vector<'T>( Array.map ( fun (x:'T) -> right / x ) left.Data )

        static member (+) (left:Vector<'T>, right:Vector<'T>) : Vector<'T> =
            if left.N <> right.N
            then raise (new System.ArgumentException( "Vectors must be the same size" ))
            else
                Vector<'T>( Array.map2 ( fun (x:'T) (y:'T) -> x + y ) left.Data right.Data )

        static member (-) (left:Vector<'T>, right:Vector<'T>) : Vector<'T> =
            if left.N <> right.N
            then raise (new System.ArgumentException( "Vectors must be the same size" ))
            else
                Vector<'T>( Array.map2 ( fun (x:'T) (y:'T) -> x - y ) left.Data right.Data )

        static member (*) (left:Vector<'T>, right:Vector<'T>) : 'T =
            if left.N <> right.N
            then raise (new System.ArgumentException( "Vectors must be the same size" ))
            else
                Array.map2 ( fun (x:'T) (y:'T) -> x * y ) left.Data right.Data
                |> Array.sum

        member this.Apply (mapping:System.Func<'T,'T>) : Vector<'T> =
            Vector<'T>( Array.map ( fun x -> mapping.Invoke( x ) ) this.Data )

        member this.Apply (mapping:System.Func<int32,'T,'T>) : Vector<'T> =
            Vector<'T>( Array.mapi ( fun i x -> mapping.Invoke( i, x ) ) this.Data )

        override this.ToString () =
            let r = Array.map ( fun x -> x.ToString() ) this.Data
            let maxLength =
                r
                |> Array.map ( fun s -> s.Length )
                |> Array.max

            System.String.Format(
                "<  {0}  >",
                System.String.Join(
                    "  ",
                    ( Array.map ( fun (s:string) -> s.PadLeft( maxLength + 1 ) ) r )
                )
            )

        interface System.IFormattable with
            member this.ToString(format:string, formatProvider:System.IFormatProvider) : string =
                let r = Array.map ( fun (x:'T) -> x.ToString( format, formatProvider ) ) this.Data
                let maxLength =
                    r
                    |> Array.map ( fun s -> s.Length )
                    |> Array.max

                System.String.Format(
                    "<  {0}  >",
                    System.String.Join(
                        "  ",
                        ( Array.map ( fun (s:string) -> s.PadLeft( maxLength + 1 ) ) r )
                    )
                )
