﻿namespace Natural

open Xunit
open Freestylecoding.Math

type public Constructors() =
    // Yes, it's kinda hacky that we're using Parse here
    // There is no "good" way to put in a large Natural the default way
    //  and compare it against the expected value
    // This is why Parse is part of the sanity tests
    [<Theory>]
    [<InlineData(                    "0", 0u, 0u, 0u )>]
    [<InlineData(           "4294967297", 0u, 1u, 1u )>]
    [<InlineData( "18446744073709551616", 1u, 0u, 0u )>]
    [<InlineData( "18446744073709551617", 1u, 0u, 1u )>]
    member public this.DefaultCtor (expected:string) (a:uint32) (b:uint32) (c:uint32) =
        Assert.Equal( Natural.Parse( expected ), Natural( [a; b; c] ) )

    [<Fact>]
    member public this.EmptyCtor () =
        Assert.Equal( Natural.Zero, Natural() )

    [<Theory>]
    [<InlineData( 0u, 0u, 0u )>]
    [<InlineData( 0u, 1u, 1u )>]
    [<InlineData( 1u, 0u, 0u )>]
    [<InlineData( 1u, 0u, 1u )>]
    member public this.CopyCtor (a:uint32) (b:uint32) (c:uint32) =
        let expected = Natural( [a; b; c] )
        Assert.Equal( expected, Natural( expected ) )

    [<Theory>]
    [<InlineData(   0u )>]
    [<InlineData(   1u )>]
    [<InlineData(   2u )>]
    [<InlineData(   5u )>]
    [<InlineData( 100u )>]
    member public this.Uint32Ctro (actual:uint32) =
        Assert.Equal( Natural( [actual] ), Natural( actual ) )

    [<Theory>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL )>]
    [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001uL )>]
    [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000uL )>]
    [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL )>]
    [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078uL )>]
    [<InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0uL )>]
    [<InlineData( 0xFFFF_0000u, 0x0000_0000u, 0xFFFF_0000_0000_0000uL )>]
    [<InlineData( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0xFFFF_EEEE_DDDD_CCCCuL )>]
    member public this.Uint64Ctor (expectedHigh:uint32) (expectedLow:uint32) (actual:uint64) =
        Assert.Equal( Natural( [expectedHigh; expectedLow] ), Natural( actual ) )

    [<Theory>]
    [<InlineData( 0u, 0u, 0u )>]
    [<InlineData( 0u, 0u, 1u )>]
    [<InlineData( 0u, 1u, 0u )>]
    [<InlineData( 0u, 1u, 1u )>]
    [<InlineData( 1u, 0u, 0u )>]
    [<InlineData( 1u, 0u, 1u )>]
    [<InlineData( 1u, 1u, 0u )>]
    [<InlineData( 1u, 1u, 1u )>]
    member public this.Uint32Sequence (a:uint32) (b:uint32) (c:uint32) =
        Assert.Equal( Natural( [ a; b; c ] ), Natural( seq { a; b; c } ) )
