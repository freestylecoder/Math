namespace Integer

open Xunit
open Freestylecoding.Math

type public Constructors() =
    // Yes, it's kinda hacky that we're using Parse here
    // There is no "good" way to put in a large Natural the default way
    //  and compare it against the expected value
    // This is why Parse is part of the sanity tests
    [<Theory>]
    [<InlineData(                     "0", 0u, 0u, 0u, false )>]
    [<InlineData(            "4294967297", 0u, 1u, 1u, false )>]
    [<InlineData(  "18446744073709551616", 1u, 0u, 0u, false )>]
    [<InlineData(  "18446744073709551617", 1u, 0u, 1u, false )>]
    [<InlineData(                    "-0", 0u, 0u, 0u, false )>]
    [<InlineData(           "-4294967297", 0u, 1u, 1u, true  )>]
    [<InlineData( "-18446744073709551616", 1u, 0u, 0u, true  )>]
    [<InlineData( "-18446744073709551617", 1u, 0u, 1u, true  )>]
    member public this.DefaultCtor (expected:string) (a:uint32) (b:uint32) (c:uint32) (neg:bool) =
        Assert.Equal( Integer.Parse( expected ), Integer( [a; b; c], neg ) )

    [<Fact>]
    member public this.EmptyCtor () =
        Assert.Equal( Integer.Zero, Integer() )

    [<Theory>]
    [<InlineData( 0u, 0u, 0u, false )>]
    [<InlineData( 0u, 1u, 1u, false )>]
    [<InlineData( 1u, 0u, 0u, false )>]
    [<InlineData( 1u, 0u, 1u, false )>]
    [<InlineData( 0u, 1u, 1u, true )>]
    [<InlineData( 1u, 0u, 0u, true )>]
    [<InlineData( 1u, 0u, 1u, true )>]
    member public this.CopyCtor (a:uint32) (b:uint32) (c:uint32) (neg:bool) =
        let expected = Integer( [a; b; c], neg )
        Assert.Equal( expected, Integer( expected ) )

    [<Theory>]
    [<InlineData( 0u, 0u, 0u )>]
    [<InlineData( 0u, 1u, 1u )>]
    [<InlineData( 1u, 0u, 0u )>]
    [<InlineData( 1u, 0u, 1u )>]
    member public this.Natural (a:uint32) (b:uint32) (c:uint32) =
        let expected = Integer( [a; b; c], false )
        Assert.Equal( expected, Integer( Natural( [a; b; c] ) ) )

    [<Theory>]
    [<InlineData( 0u, 0u, 0u, false )>]
    [<InlineData( 0u, 1u, 1u, false )>]
    [<InlineData( 1u, 0u, 0u, false )>]
    [<InlineData( 1u, 0u, 1u, false )>]
    [<InlineData( 0u, 1u, 1u, true )>]
    [<InlineData( 1u, 0u, 0u, true )>]
    [<InlineData( 1u, 0u, 1u, true )>]
    member public this.NaturalSign (a:uint32) (b:uint32) (c:uint32) (neg:bool) =
        let expected = Integer( [a; b; c], neg )
        Assert.Equal( expected, Integer( Natural( [a; b; c] ), neg ) )

    [<Theory>]
    [<InlineData(    0y )>]
    [<InlineData(    1y )>]
    [<InlineData(    2y )>]
    [<InlineData(    5y )>]
    [<InlineData(  100y )>]
    [<InlineData(   -1y )>]
    [<InlineData(   -2y )>]
    [<InlineData(   -5y )>]
    [<InlineData( -100y )>]
    member public this.Int8Ctor (value:int8) =
        let magnitude:uint32 = System.Convert.ToUInt32( System.SByte.Abs(value) )
        let negative = System.SByte.IsNegative( value )
        Assert.Equal( Integer( [magnitude], negative ), Integer( value ) )

    [<Theory>]
    [<InlineData(    0s )>]
    [<InlineData(    1s )>]
    [<InlineData(    2s )>]
    [<InlineData(    5s )>]
    [<InlineData(  100s )>]
    [<InlineData(   -1s )>]
    [<InlineData(   -2s )>]
    [<InlineData(   -5s )>]
    [<InlineData( -100s )>]
    member public this.Int16Ctor (value:int16) =
        let magnitude:uint32 = System.Convert.ToUInt32( System.Int16.Abs(value) )
        let negative = System.Int16.IsNegative( value )
        Assert.Equal( Integer( [magnitude], negative ), Integer( value ) )

    [<Theory>]
    [<InlineData(    0 )>]
    [<InlineData(    1 )>]
    [<InlineData(    2 )>]
    [<InlineData(    5 )>]
    [<InlineData(  100 )>]
    [<InlineData(   -1 )>]
    [<InlineData(   -2 )>]
    [<InlineData(   -5 )>]
    [<InlineData( -100 )>]
    member public this.Uint32Ctor (value:int32) =
        let magnitude:uint32 = System.Convert.ToUInt32( System.Int32.Abs(value) )
        let negative = System.Int32.IsNegative( value )
        Assert.Equal( Integer( [magnitude], negative ), Integer( value ) )

    [<Theory>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000L, false )>]
    [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001L, false )>]
    [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000L, false )>]
    [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001L, false )>]
    [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078L, false )>]
    [<InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0L, false )>]
    [<InlineData( 0x0001_0000u, 0x0000_0000u, 0xFFFF_0000_0000_0000L, true  )>]
    [<InlineData( 0x0000_1111u, 0x2222_3334u, 0xFFFF_EEEE_DDDD_CCCCL, true  )>]
    member public this.Int64Ctor (expectedHigh:uint32) (expectedLow:uint32) (value:int64) (negative:bool) =
        Assert.Equal( Integer( [expectedHigh; expectedLow], negative ), Integer( value ) )

    [<Theory>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0000uL, false )>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0001uL, false )>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0001_0000_0000uL, false )>]
    [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0001uL, 0x0000_0000_0000_0000uL, false )>]
    [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001_0000_0000uL, 0x0000_0000_0000_0000uL, false )>]
    [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL, 0x0000_0001_0000_0001uL, false )>]
    [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u, 0x1200_0340_0560_0078uL, 0x1234_5678_9ABC_DEF0uL, false )>]
    [<InlineData( 0x0000_1111u, 0x2222_3333u, 0xBBBB_CCCCu, 0xDDDD_EEEFu, 0xFFFF_EEEE_DDDD_CCCCuL, 0x4444_3333_2222_1111uL, true  )>]
    member public this.Int128Ctor (i0:uint32) (i1:uint32) (i2:uint32) (i3:uint32) (l0:uint64) (l1:uint64) (neg:bool) =
        Assert.Equal( Integer( [i0; i1; i2; i3], neg ), Integer( System.Int128( l0, l1 ) ) )

    [<Theory>]
    [<InlineData(                     "0", 0u, 0u, 0u )>]
    [<InlineData(            "4294967297", 0u, 1u, 1u )>]
    [<InlineData(  "18446744073709551616", 1u, 0u, 0u )>]
    [<InlineData(  "18446744073709551617", 1u, 0u, 1u )>]
    member public this.UInt32List (expected:string) (a:uint32) (b:uint32) (c:uint32) =
        Assert.Equal( Integer.Parse( expected ), Integer( [a; b; c] ) )

    [<Theory>]
    [<InlineData( 0u, 0u, 0u )>]
    [<InlineData( 0u, 0u, 1u )>]
    [<InlineData( 0u, 1u, 0u )>]
    [<InlineData( 0u, 1u, 1u )>]
    [<InlineData( 1u, 0u, 0u )>]
    [<InlineData( 1u, 0u, 1u )>]
    [<InlineData( 1u, 1u, 0u )>]
    [<InlineData( 1u, 1u, 1u )>]
    member public this.UInt32Sequence (a:uint32) (b:uint32) (c:uint32) =
        Assert.Equal( Integer( [ a; b; c ] ), Integer( seq { a; b; c } ) )

    [<Theory>]
    [<InlineData( 0u, 0u, 0u, false )>]
    [<InlineData( 0u, 0u, 1u, false )>]
    [<InlineData( 0u, 1u, 0u, false )>]
    [<InlineData( 0u, 1u, 1u, false )>]
    [<InlineData( 1u, 0u, 0u, false )>]
    [<InlineData( 1u, 0u, 1u, false )>]
    [<InlineData( 1u, 1u, 0u, false )>]
    [<InlineData( 1u, 1u, 1u, false )>]
    [<InlineData( 0u, 0u, 1u, true  )>]
    [<InlineData( 0u, 1u, 0u, true  )>]
    [<InlineData( 0u, 1u, 1u, true  )>]
    [<InlineData( 1u, 0u, 0u, true  )>]
    [<InlineData( 1u, 0u, 1u, true  )>]
    [<InlineData( 1u, 1u, 0u, true  )>]
    [<InlineData( 1u, 1u, 1u, true  )>]
    member public this.UInt32SequenceNegative (a:uint32) (b:uint32) (c:uint32) (n:bool) =
        Assert.Equal( Integer( [ a; b; c ], n ), Integer( seq { a; b; c }, n ) )
