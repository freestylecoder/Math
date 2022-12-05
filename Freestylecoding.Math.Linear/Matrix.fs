namespace Freestylecoding.Math.Linear

open System
open Freestylecoding.Math

type public Matrix(rows:int32, columns:int32) =
    let position i j = columns * i + j
    let data = [| for i in 1 .. ( rows * columns ) -> Natural.Zero|]

    member internal this.Rows = rows
    member internal this.Columns = columns
    member internal this.Data = data

    member this.Item
        with get i = data.[i]
        and set i value = data.[i] <- value

    member this.Item
        with get ( i, j ) = this.[(position i j)]
        and set ( i, j ) value = this.[(position i j)] <- value

    with
        static member op_Equality (left:Matrix, right:Matrix) : bool =
            if
                left.Rows <> right.Rows || left.Columns <> right.Columns
            then
                false
            else
                Array.map2 ( fun x y -> x = y ) left.Data right.Data
                    |> Array.reduce ( fun x y -> x && y )

        static member (+) (left:Matrix, right:Matrix) : Matrix =
            if
                left.Rows <> right.Rows || left.Columns <> right.Columns
            then
                raise (new System.ArgumentException( "Matricies must be the same size" ))
            else
                let sum = Matrix( left.Rows, left.Columns )
                Array.iteri2 ( fun i x y -> sum.Data.[i] <- x + y ) left.Data right.Data
                sum

        static member (*) (left:Natural, right:Matrix) : Matrix =
            let product = Matrix( right.Rows, right.Columns )
            Array.iteri ( fun i x -> product.Data.[i] <- x * left ) right.Data
            product

        static member (*) (left:Matrix, right:Natural) : Matrix =
            right * left
