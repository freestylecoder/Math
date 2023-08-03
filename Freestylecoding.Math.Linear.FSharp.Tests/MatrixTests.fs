namespace Freestylecoding.Math.Linear.FSharp.Tests

open Xunit
open Freestylecoding.Math
open Freestylecoding.Math.Linear

module Matrix =
    module ctor =
        [<Fact>]
        let Test1() =
            let x = new Matrix( 2, 2, Array.create 4 Real.Zero )
            Assert.True( Real.Zero = x.[0,0] )
            Assert.True( Real.Zero = x.[0,1] )
            Assert.True( Real.Zero = x.[1,0] )
            Assert.True( Real.Zero = x.[1,1] )

        [<Fact>]
        let Test2() =
            let v0 = new Vector( [| Real.Zero; Real.Unit |] )
            let v1 = new Vector( [| Real( 2, 0 ); Real( 3, 0 )|] )
            let x = new Matrix( [| v0; v1 |] )
            Assert.True( Real.Zero = x.[0,0] )
            Assert.True( Real.Unit = x.[0,1] )
            Assert.True( Real( 2, 0 ) = x.[1,0] )
            Assert.True( Real( 3, 0 ) = x.[1,1] )

        [<Fact>]
        let Test3() =
            let x = new Matrix( 2, 2, Real( 2, 0 ) )
            Assert.True( Real( 2, 0 ) = x.[0,0] )
            Assert.True( Real( 2, 0 ) = x.[0,1] )
            Assert.True( Real( 2, 0 ) = x.[1,0] )
            Assert.True( Real( 2, 0 ) = x.[1,1] )

        [<Fact>]
        let Test4() =
            let x = new Matrix( 2, 2, ( fun i -> Real( i, 0 ) ) )
            Assert.True( Real( 0, 0 ) = x.[0,0] )
            Assert.True( Real( 1, 0 ) = x.[0,1] )
            Assert.True( Real( 2, 0 ) = x.[1,0] )
            Assert.True( Real( 3, 0 ) = x.[1,1] )
            Assert.Equal( 2, x.Rows )
            Assert.Equal( 2, x.Columns )

    module GetSet =
        [<Fact>]
        let Geti() =
            let x = new Matrix( 2, 2, [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |] )

            Assert.Equal( Real.Parse( "0" ), x.[0] )
            Assert.Equal( Real.Parse( "1" ), x.[1] )
            Assert.Equal( Real.Parse( "2" ), x.[2] )
            Assert.Equal( Real.Parse( "3" ), x.[3] )

        [<Fact>]
        let Getij() =
            let x = new Matrix( 2, 2, [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |] )

            Assert.Equal( Real.Parse( "0" ), x.[0,0] )
            Assert.Equal( Real.Parse( "1" ), x.[0,1] )
            Assert.Equal( Real.Parse( "2" ), x.[1,0] )
            Assert.Equal( Real.Parse( "3" ), x.[1,1] )

        [<Fact>]
        let GetRow() =
            let x = new Matrix( 3, 3, [| Real( 0, 0 ) .. Real( 8, 0 ) |] )
            Assert.True( Vector.op_Equality(
                Vector( [| Real( 3, 0 ) .. Real( 5, 0 ) |] ),
                x.GetRow( 1 )
            ) )

        [<Fact>]
        let GetColumn() =
            let x = new Matrix( 3, 3, [| Real( 0, 0 ) .. Real( 8, 0 ) |] )
            Assert.True( Vector.op_Equality(
                Vector( [| Real( 1, 0 ); Real( 4, 0 ); Real( 7, 0 ) |] ),
                x.GetColumn( 1 )
            ) )

        [<Fact>]
        let Seti() =
            let x = new Matrix( 2, 2, [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |] )
            x.[2] <- Real( 4, 0 )

            Assert.Equal( Real.Parse( "0" ), x.[0] )
            Assert.Equal( Real.Parse( "1" ), x.[1] )
            Assert.Equal( Real.Parse( "4" ), x.[2] )
            Assert.Equal( Real.Parse( "3" ), x.[3] )

        [<Fact>]
        let Setij() =
            let x = new Matrix( 2, 2, [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |] )
            x.[1,0] <- Real( 4, 0 )

            Assert.Equal( Real.Parse( "0" ), x.[0,0] )
            Assert.Equal( Real.Parse( "1" ), x.[0,1] )
            Assert.Equal( Real.Parse( "4" ), x.[1,0] )
            Assert.Equal( Real.Parse( "3" ), x.[1,1] )

    module RowsColumns =
        [<Fact>]
        let Rows() =
            let m = new Matrix( 4, 5, Array.zeroCreate 20 )
            Assert.Equal( 4, m.Rows )

        [<Fact>]
        let Columns() =
            let m = new Matrix( 4, 5, Array.zeroCreate 20 )
            Assert.Equal( 5, m.Columns )

    module Equality =
        [<Fact>]
        let Equals() =
            let a = [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |]
            let x = new Matrix( 2, 2, a )
            let y = new Matrix( 2, 2, a )

            Assert.True( Matrix.op_Equality( x, y ) )

        [<Fact>]
        let NotEquals() =
            let a = [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |]
            let x = new Matrix( 2, 2, a )
            let y = new Matrix( 2, 2, Array.map (fun v -> v + Real.Unit ) a )

            Assert.False( Matrix.op_Equality( x, y ) )

    module Addition =
        [<Fact>]
        let Add() =
            let x = new Matrix( 2, 2, [| Real.Zero .. Real( 3, 0 ) |] )
            let y = new Matrix( 2, 2, [| Real( 4, 0 ) .. Real( 7, 0 ) |] )
            let z = new Matrix( 2, 2, [| Real( 4, 0 ); Real( 6, 0 ); Real( 8, 0 ); Real( 10, 0 ) |] )

            Assert.True( Matrix.op_Equality( z, x + y ) )

    module Multiplication =
        [<Fact>]
        let Multiply2x2() =
            let x = new Matrix( 2, 2, [| Real.Zero .. Real( 3, 0 ) |] )
            let y = new Matrix( 2, 2, [| Real( 4, 0 ) .. Real( 7, 0 ) |] )
            let z = new Matrix( 2, 2, [| Real( 6, 0 ); Real( 7, 0 ); Real( 26, 0 ); Real( 31, 0 ) |] )

            Assert.True( Matrix.op_Equality( z, x * y ) )

        [<Fact>]
        let Multiply3x3() =
            let x = new Matrix( 3, 3, [| Real.Zero .. Real( 8, 0 ) |] )
            let y = new Matrix( 3, 3, [| Real.Unit .. Real( 9, 0 ) |] )
            let z = new Matrix( 3, 3, [| Real( 18, 0 ); Real( 21, 0 ); Real( 24, 0 ); Real( 54, 0 ); Real( 66, 0 ); Real( 78, 0 ); Real( 90, 0 ); Real( 111, 0 ); Real( 132, 0 ) |] )

            Assert.True( Matrix.op_Equality( z, x * y ) )

        [<Fact>]
        let MultiplyOff() =
            let x = new Matrix( 2, 3, [| Real.Zero .. Real( 5, 0 ) |] )
            let y = new Matrix( 3, 2, [| Real.Unit .. Real( 6, 0 ) |] )
            let z = new Matrix( 2, 2, [| Real( 13, 0 ); Real( 16, 0 ); Real( 40, 0 ); Real( 52, 0 ) |] )

            Assert.True( Matrix.op_Equality( z, x * y ) )

        [<Fact>]
        let MultiplyMV() =
            let x = new Matrix( 3, 3, [| Real.Zero .. Real( 8, 0 ) |] )
            let y = new Vector( [| Real.Unit .. Real( 3, 0 ) |] )
            let z = new Vector( [| Real( 8, 0 ); Real( 26, 0 ); Real( 44, 0 ) |] )

            Assert.True( Vector.op_Equality( z, x * y ) )

        [<Fact>]
        let MultiplyVM() =
            let x = new Matrix( 3, 3, [| Real.Zero .. Real( 8, 0 ) |] )
            let y = new Vector( [| Real.Unit .. Real( 3, 0 ) |] )
            let z = new Vector( [| Real( 24, 0 ); Real( 30, 0 ); Real( 36, 0 ) |] )

            Assert.True( Vector.op_Equality( z, y * x ) )

    module Apply =
        [<Fact>]
        let Apply() =
            let x = new Matrix( 2, 2, [| Real.Zero .. Real( 3, 0 ) |] )
            let y = new Matrix( 2, 2, [| Real( 1, 0 ) .. Real( 4, 0 ) |] )
            let m a = a + Real.Unit

            Assert.True( Matrix.op_Equality( y, x.Apply( m ) ) )
