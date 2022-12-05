namespace Freestylecoding.Math.Linear.FSharp.Tests

open Xunit
open Freestylecoding.Math
open Freestylecoding.Math.Linear

module Matrix =
    module ctor =
        [<Fact>]
        let Test1() =
            let x = new Matrix( 2, 2 )
            Assert.True( Natural.Zero = x.[0,0] )
            Assert.True( Natural.Zero = x.[0,1] )
            Assert.True( Natural.Zero = x.[1,0] )
            Assert.True( Natural.Zero = x.[1,1] )

    module GetSet =
        [<Fact>]
        let Get() =
            let x = new Matrix( 2, 2 )
            x.[0,0] <- Natural.Parse( "0" )
            x.[0,1] <- Natural.Parse( "1" )
            x.[1,0] <- Natural.Parse( "2" )
            x.[1,1] <- Natural.Parse( "3" )

            Assert.Equal( Natural.Parse( "0" ), x.[0,0] )
            Assert.Equal( Natural.Parse( "1" ), x.[0,1] )
            Assert.Equal( Natural.Parse( "2" ), x.[1,0] )
            Assert.Equal( Natural.Parse( "3" ), x.[1,1] )

        [<Fact>]
        let Set() =
            let x = new Matrix( 2, 2 )
            x.[0,0] <- Natural.Parse( "0" )
            x.[0,1] <- Natural.Parse( "1" )
            x.[1,0] <- Natural.Parse( "2" )
            x.[1,1] <- Natural.Parse( "3" )

            Assert.Equal( Natural.Parse( "0" ), x.[0,0] )
            Assert.Equal( Natural.Parse( "1" ), x.[0,1] )
            Assert.Equal( Natural.Parse( "2" ), x.[1,0] )
            Assert.Equal( Natural.Parse( "3" ), x.[1,1] )

    module Equality =
        [<Fact>]
        let Equals() =
            let x = new Matrix( 2, 2 )
            x.[0,0] <- Natural.Parse( "0" )
            x.[0,1] <- Natural.Parse( "1" )
            x.[1,0] <- Natural.Parse( "2" )
            x.[1,1] <- Natural.Parse( "3" )

            let y = new Matrix( 2, 2 )
            y.[0,0] <- Natural.Parse( "0" )
            y.[0,1] <- Natural.Parse( "1" )
            y.[1,0] <- Natural.Parse( "2" )
            y.[1,1] <- Natural.Parse( "3" )

            Assert.True( Matrix.op_Equality( x, y ) )

        [<Fact>]
        let NotEquals() =
            let x = new Matrix( 2, 2 )
            x.[0,0] <- Natural.Parse( "0" )
            x.[0,1] <- Natural.Parse( "1" )
            x.[1,0] <- Natural.Parse( "2" )
            x.[1,1] <- Natural.Parse( "3" )

            let y = new Matrix( 2, 2 )
            y.[0,0] <- Natural.Parse( "1" )
            y.[0,1] <- Natural.Parse( "1" )
            y.[1,0] <- Natural.Parse( "2" )
            y.[1,1] <- Natural.Parse( "3" )

            Assert.False( Matrix.op_Equality( x, y ) )

    module Addition =
        [<Fact>]
        let Add() =
            let x = new Matrix( 2, 2 )
            x.[0,0] <- Natural.Parse( "0" )
            x.[0,1] <- Natural.Parse( "1" )
            x.[1,0] <- Natural.Parse( "2" )
            x.[1,1] <- Natural.Parse( "3" )

            let y = new Matrix( 2, 2 )
            y.[0,0] <- Natural.Parse( "4" )
            y.[0,1] <- Natural.Parse( "5" )
            y.[1,0] <- Natural.Parse( "6" )
            y.[1,1] <- Natural.Parse( "7" )

            let z = new Matrix( 2, 2 )
            z.[0,0] <- Natural.Parse( "4" )
            z.[0,1] <- Natural.Parse( "6" )
            z.[1,0] <- Natural.Parse( "8" )
            z.[1,1] <- Natural.Parse( "10" )

            Assert.True( Matrix.op_Equality( z, x + y ) )