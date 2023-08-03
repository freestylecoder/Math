namespace Freestylecoding.Math.Linear

open System
open Freestylecoding.Math

type public Matrix( rows:int32, columns:int32, data:Real array ) =
    let position i j = columns * i + j

    member internal this.Data = data

    member this.Rows
        with get () = rows
    member this.Columns
        with get () = columns

    member this.Item
        with get i = data.[i]
        and set i value = data.[i] <- value

    member this.Item
        with get ( i, j ) = this.[(position i j)]
        and set ( i, j ) value = this.[(position i j)] <- value

    new( vectors:Vector array ) = Matrix(
        vectors.Length,
        vectors[0].Data.Length,
        ( Array.collect ( fun (v:Vector) -> v.Data ) vectors )
    )
    new( rows:int32, columns:int32, fill:Real ) =
        Matrix( rows, columns, [| for _ in 1 .. (rows * columns) -> fill |] )
    new( rows:int32, columns:int32, fill:System.Func<int32,Real> ) =
        Matrix( rows, columns, [| for i in 0 .. ( ( rows * columns ) - 1 ) -> fill.Invoke( i ) |] )

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
                Matrix(
                    left.Rows,
                    left.Columns,
                    Array.map2 ( fun x y -> x + y ) left.Data right.Data
                )

        static member (*) (left:Real, right:Matrix) : Matrix =
            Matrix(
                right.Rows,
                right.Columns,
                Array.map ( fun x -> x * left ) right.Data
            )

        static member (*) (left:Matrix, right:Real) : Matrix =
            right * left

        static member (*) (left:Matrix, right:Matrix) : Matrix =
            if left.Columns <> right.Rows
            then raise (new System.ArgumentException( "Left.Columns must equal Right.Rows" ))

            let product = Array.zeroCreate ( left.Rows * right.Columns )
            for i in 0 .. (left.Rows - 1) do
                for j in 0 .. (right.Columns - 1) do
                    product[right.Columns * i + j] <- (left.GetRow( i ) * right.GetColumn( j ))

            Matrix( left.Rows, right.Columns, product )

        static member (*) (left:Vector, right:Matrix) : Vector =
            Vector( ( Matrix( 1, left.N, left.Data ) * right ).Data )

        static member (*) (left:Matrix, right:Vector) : Vector =
            Vector( ( left * Matrix( right.N, 1, right.Data ) ).Data )

        member this.GetRow( row:int32 ) : Vector =
            Vector(
                this.Data
                |> Array.skip ( row * this.Columns )
                |> Array.take this.Columns
            )

        // Yes, this one is really dirty
        // I don't like it either
        // However, F# Slices don't have the middle part
        member this.GetColumn( col:int32 ) : Vector =
            let mutable values = Array.Empty<Real>()
            for i in col .. this.Columns .. (this.Data.Length - 1) do
                values <- Array.append values [| this[i] |]
            Vector( values )

        member this.Apply (mapping:System.Func<Real,Real>) : Matrix =
            Matrix( this.Rows, this.Columns, Array.map ( fun x -> mapping.Invoke( x ) ) this.Data )

type public Matrix<'T when 'T :> System.Numerics.INumber<'T>>( rows:int32, columns:int32, data:'T array ) =
    let position i j = columns * i + j

    member internal this.Data = data

    member this.Rows
        with get () = rows
    member this.Columns
        with get () = columns

    member this.Item
        with get i = data.[i]
        and set i value = data.[i] <- value

    member this.Item
        with get ( i, j ) = this.[(position i j)]
        and set ( i, j ) value = this.[(position i j)] <- value

    new( vectors:Vector<'T> array ) = Matrix<'T>(
        vectors.Length,
        vectors[0].Data.Length,
        ( Array.collect ( fun (v:Vector<'T>) -> v.Data ) vectors )
    )
    new( rows:int32, columns:int32, fill:'T ) =
        Matrix<'T>( rows, columns, [| for _ in 1 .. (rows * columns) -> fill |] )
    new( rows:int32, columns:int32, fill:System.Func<int32,'T> ) =
        Matrix<'T>( rows, columns, [| for i in 0 .. ( ( rows * columns ) - 1 ) -> fill.Invoke( i ) |] )

    with
        static member op_Equality (left:Matrix<'T>, right:Matrix<'T>) : bool =
            if
                left.Rows <> right.Rows || left.Columns <> right.Columns
            then
                false
            else
                Array.map2 ( fun (x:'T) (y:'T) -> x.Equals( y ) ) left.Data right.Data
                    |> Array.reduce ( fun x y -> x && y )

        static member (*) (left:'T, right:Matrix<'T>) : Matrix<'T> =
            Matrix<'T>(
                right.Rows,
                right.Columns,
                Array.map ( fun (x:'T) -> x * left ) right.Data
            )

        static member (*) (left:Matrix<'T>, right:'T) : Matrix<'T> =
            Matrix<'T>(
                left.Rows,
                left.Columns,
                Array.map ( fun (x:'T) -> x * right ) left.Data
            )

        static member (/) (left:'T, right:Matrix<'T>) : Matrix<'T> =
            Matrix<'T>(
                right.Rows,
                right.Columns,
                Array.map ( fun (x:'T) -> x / left ) right.Data
            )

        static member (/) (left:Matrix<'T>, right:'T) : Matrix<'T> =
            Matrix<'T>(
                left.Rows,
                left.Columns,
                Array.map ( fun (x:'T) -> x / right ) left.Data
            )

        static member (+) (left:Matrix<'T>, right:Matrix<'T>) : Matrix<'T> =
            if
                left.Rows <> right.Rows || left.Columns <> right.Columns
            then
                raise (new System.ArgumentException( "Matricies must be the same size" ))
            else
                Matrix<'T>(
                    left.Rows,
                    left.Columns,
                    Array.map2 ( fun (x:'T) (y:'T) -> x + y ) left.Data right.Data
                )

        static member (-) (left:Matrix<'T>, right:Matrix<'T>) : Matrix<'T> =
            if
                left.Rows <> right.Rows || left.Columns <> right.Columns
            then
                raise (new System.ArgumentException( "Matricies must be the same size" ))
            else
                Matrix<'T>(
                    left.Rows,
                    left.Columns,
                    Array.map2 ( fun (x:'T) (y:'T) -> x - y ) left.Data right.Data
                )

        static member (*) (left:Matrix<'T>, right:Matrix<'T>) : Matrix<'T> =
            if left.Columns <> right.Rows
            then raise (new System.ArgumentException( "Left.Columns must equal Right.Rows" ))

            let product = Array.zeroCreate ( left.Rows * right.Columns )
            for i in 0 .. (left.Rows - 1) do
                for j in 0 .. (right.Columns - 1) do
                    product[right.Columns * i + j] <- (left.GetRow( i ) * right.GetColumn( j ))

            Matrix<'T>( left.Rows, right.Columns, product )

        static member (*) (left:Vector<'T>, right:Matrix<'T>) : Vector<'T> =
            Vector<'T>( ( Matrix<'T>( 1, left.N, left.Data ) * right ).Data )

        static member (*) (left:Matrix<'T>, right:Vector<'T>) : Vector<'T> =
            Vector<'T>( ( left * Matrix<'T>( right.N, 1, right.Data ) ).Data )

        member this.GetRow( row:int32 ) : Vector<'T> =
            Vector<'T>(
                this.Data
                |> Array.skip ( row * this.Columns )
                |> Array.take this.Columns
            )

        // Yes, this one is really dirty
        // I don't like it either
        // However, F# Slices don't have the middle part
        member this.GetColumn( col:int32 ) : Vector<'T> =
            let mutable values = Array.Empty<'T>()
            for i in col .. this.Columns .. (this.Data.Length - 1) do
                values <- Array.append values [| this[i] |]
            Vector<'T>( values )

        member this.Apply (mapping:System.Func<'T,'T>) : Matrix<'T> =
            Matrix<'T>( this.Rows, this.Columns, Array.map ( fun x -> mapping.Invoke( x ) ) this.Data )

        member this.Apply (mapping:System.Func<int32,'T,'T>) : Matrix<'T> =
            Matrix<'T>( this.Rows, this.Columns, Array.mapi ( fun i x -> mapping.Invoke( i, x ) ) this.Data )

        override this.ToString () =
            let strings = Array.map ( fun t -> t.ToString() ) this.Data

            let maxLength = 1 + (
                strings
                |> Array.map ( fun s -> s.Length )
                |> Array.max
            )

            let paddedStrings =
                strings
                |> Array.map ( fun s -> s.PadLeft( maxLength ) )

            let s = (new System.Text.StringBuilder())
            for i in 0 .. ( rows - 1 ) do
                s.Append( "|  " ) |> ignore
                for j in 0 .. ( columns - 1 ) do
                    s.Append( paddedStrings.[position i j] ) |> ignore
                    s.Append( "  " ) |> ignore
                s.AppendLine( "|" ) |> ignore

            s.ToString()

        interface System.IFormattable with
            member this.ToString(format:string, formatProvider:System.IFormatProvider) : string =
                let strings = Array.map ( fun (t:'T) -> t.ToString( format, formatProvider ) ) this.Data

                let maxLength = 1 + (
                    strings
                    |> Array.map ( fun s -> s.Length )
                    |> Array.max
                )

                let paddedStrings =
                    strings
                    |> Array.map ( fun s -> s.PadLeft( maxLength ) )

                let s = (new System.Text.StringBuilder())
                for i in 0 .. ( rows - 1 ) do
                    s.Append( "|  " ) |> ignore
                    for j in 0 .. ( columns - 1 ) do
                        s.Append( paddedStrings.[position i j] ) |> ignore
                        s.Append( "  " ) |> ignore
                    s.AppendLine( "|" ) |> ignore

                s.ToString()
