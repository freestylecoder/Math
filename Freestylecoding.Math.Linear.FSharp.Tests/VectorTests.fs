namespace Freestylecoding.Math.Linear.FSharp.Tests

open Xunit
open Freestylecoding.Math
open Freestylecoding.Math.Linear

module Vector =
    module ctor =
        [<Fact>]
        let Test1() =
            let x = new Vector( Array.create 4 Real.Zero )
            Assert.True( Real.Zero = x.[0] )
            Assert.True( Real.Zero = x.[1] )
            Assert.True( Real.Zero = x.[2] )
            Assert.True( Real.Zero = x.[3] )

        [<Fact>]
        let Test2() =
            let x = new Vector( 4, Real.Zero )
            Assert.True( Real.Zero = x.[0] )
            Assert.True( Real.Zero = x.[1] )
            Assert.True( Real.Zero = x.[2] )
            Assert.True( Real.Zero = x.[3] )

        [<Fact>]
        let Test3() =
            let x = new Vector( 4, ( fun i -> Real( i, 0 ) ) )
            Assert.True( Real( 0, 0 ) = x.[0] )
            Assert.True( Real( 1, 0 ) = x.[1] )
            Assert.True( Real( 2, 0 ) = x.[2] )
            Assert.True( Real( 3, 0 ) = x.[3] )
            Assert.Equal( 4, x.Length )

    module GetSet =
        [<Fact>]
        let Get() =
            let x = new Vector( [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |] )

            Assert.Equal( Real.Parse( "0" ), x.[0] )
            Assert.Equal( Real.Parse( "1" ), x.[1] )
            Assert.Equal( Real.Parse( "2" ), x.[2] )
            Assert.Equal( Real.Parse( "3" ), x.[3] )

        [<Fact>]
        let Set() =
            let x = new Vector( [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |] )
            x.[2] <- Real( 4, 0 )

            Assert.Equal( Real.Parse( "0" ), x.[0] )
            Assert.Equal( Real.Parse( "1" ), x.[1] )
            Assert.Equal( Real.Parse( "4" ), x.[2] )
            Assert.Equal( Real.Parse( "3" ), x.[3] )

    module Length =
        [<Fact>]
        let Length() =
            let v = new Vector( 5, Real.Unit )
            Assert.Equal( 5, v.Length )

    module Equality =
        [<Fact>]
        let Equals() =
            let a = [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |]
            let x = new Vector( a )
            let y = new Vector( a )

            Assert.True( Vector.op_Equality( x, y ) )

        [<Fact>]
        let NotEquals() =
            let a = [| Real( 0, 0 ); Real( 1, 0 ); Real( 2, 0 ); Real( 3, 0 ) |]
            let x = new Vector( a )
            let y = new Vector( Array.map (fun v -> v + Real.Unit ) a )

            Assert.False( Vector.op_Equality( x, y ) )

    module Addition =
        [<Fact>]
        let Add() =
            let x = new Vector( [| Real.Zero .. Real( 3, 0 ) |] )
            let y = new Vector( [| Real( 4, 0 ) .. Real( 7, 0 ) |] )
            let z = new Vector( [| Real( 4, 0 ); Real( 6, 0 ); Real( 8, 0 ); Real( 10, 0 ) |] )

            Assert.True( Vector.op_Equality( z, x + y ) )

    module Multiplication =
        [<Fact>]
        let Multiply2() =
            let x = new Vector( [| Real.Zero; Real.Unit |] )
            let y = new Vector( [| Real( 2, 0 ); Real( 3, 0 ) |] )
            let z = Real( 3, 0 )

            Assert.True( Real.op_Equality( z, x * y ) )

        [<Fact>]
        let Multiply3() =
            let x = new Vector( [| Real.Zero .. Real( 2, 0 ) |] )
            let y = new Vector( [| Real.Unit .. Real( 3, 0 ) |] )
            let z = Real( 8, 0 )

            Assert.True( Real.op_Equality( z, x * y ) )

        [<Fact>]
        let MultiplyRV() =
            let x = Real( 2, 0 )
            let y = new Vector( [| Real.Unit .. Real( 3, 0 ) |] )
            let z = new Vector( [| Real( 2, 0 ); Real( 4, 0 ); Real( 6, 0 ) |] )

            Assert.True( Vector.op_Equality( z, x * y ) )

        [<Fact>]
        let MultiplyVR() =
            let x = Real( 2, 0 )
            let y = new Vector( [| Real.Unit .. Real( 3, 0 ) |] )
            let z = new Vector( [| Real( 2, 0 ); Real( 4, 0 ); Real( 6, 0 ) |] )

            Assert.True( Vector.op_Equality( z, y * x ) )

    module Apply =
        [<Fact>]
        let Apply() =
            let x = new Vector( [| Real.Zero .. Real( 3, 0 ) |] )
            let y = new Vector( [| Real( 1, 0 ) .. Real( 4, 0 ) |] )
            let m a = a + Real.Unit

            Assert.True( Vector.op_Equality( y, x.Apply( m ) ) )
