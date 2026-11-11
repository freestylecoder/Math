namespace Integer

open Xunit
open Freestylecoding.Math

type public Implicit() =
    // Yes, these look a little funny
    // F# doesn't handle Implicit conversions very well
    // These methods mostly exist for C#

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
    member public this.Int8 (actual:int8) =
        Assert.Equal( Integer( actual ), Integer.op_Implicit( actual ) )

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
    member public this.Int16 (actual:int16) =
        Assert.Equal( Integer( actual ), Integer.op_Implicit( actual ) )

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
    member public this.Int32 (actual:int32) =
        Assert.Equal( Integer( actual ), Integer.op_Implicit( actual ) )

    [<Theory>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000L, false )>]
    [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001L, false )>]
    [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000L, false )>]
    [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001L, false )>]
    [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078L, false )>]
    [<InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0L, false )>]
    [<InlineData( 0x0001_0000u, 0x0000_0000u, 0xFFFF_0000_0000_0000L, true  )>]
    [<InlineData( 0x0000_1111u, 0x2222_3334u, 0xFFFF_EEEE_DDDD_CCCCL, true  )>]
    member public this.Int64 (expectedHigh:uint32) (expectedLow:uint32) (actual:int64) (neg:bool) =
        Assert.Equal( Integer( [expectedHigh; expectedLow], neg ), Integer.op_Implicit( actual ) )

    [<Theory>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0000uL, false )>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0001uL, false )>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0001_0000_0000uL, false )>]
    [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0001uL, 0x0000_0000_0000_0000uL, false )>]
    [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001_0000_0000uL, 0x0000_0000_0000_0000uL, false )>]
    [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL, 0x0000_0001_0000_0001uL, false )>]
    [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u, 0x1200_0340_0560_0078uL, 0x1234_5678_9ABC_DEF0uL, false )>]
    [<InlineData( 0x0000_1111u, 0x2222_3333u, 0xBBBB_CCCCu, 0xDDDD_EEEFu, 0xFFFF_EEEE_DDDD_CCCCuL, 0x4444_3333_2222_1111uL, true  )>]
    member public this.Int128 (i0:uint32) (i1:uint32) (i2:uint32) (i3:uint32) (l0:uint64) (l1:uint64) (neg:bool) =
        Assert.Equal( Integer( [i0; i1; i2; i3], neg ), Integer.op_Implicit( System.Int128( l0, l1 ) ) )

    [<Theory>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u )>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u )>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u )>]
    [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u )>]
    [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u )>]
    [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u )>]
    [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u )>]
    [<InlineData( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0x4444_3333u, 0x2222_1111u )>]
    member public this.Natural (i0:uint32) (i1:uint32) (i2:uint32) (i3:uint32) =
        Assert.Equal( Integer( [i0; i1; i2; i3], false ), Integer.op_Implicit( Natural( [i0; i1; i2; i3] ) ) )

    [<Fact>]
    member public this.IntegerToBigInteger () =
        Assert.Equal(
            System.Numerics.BigInteger.Zero,
            Integer.op_Implicit(
                Integer.Zero
            )
        )
        Assert.Equal(
            System.Numerics.BigInteger.One,
            Integer.op_Implicit(
                Integer.Unit
            )
        )
        Assert.Equal(
            System.Numerics.BigInteger.MinusOne,
            Integer.op_Implicit(
                Integer.NegativeOne
            )
        )

        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                |],
                true,
                true
            ),
            Integer.op_Implicit(
                Integer( [0x0000_0000u; 0x0000_0000u; 0x0000_0001u; 0x0000_0000u] )
            )
        )
        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                |],
                true,
                true
            ),
            Integer.op_Implicit(
                Integer( [0x0000_0000u; 0x0000_0001u; 0x0000_0000u; 0x0000_0000u] )
            )
        )
        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                |],
                true,
                true
            ),
            Integer.op_Implicit(
                Integer( [0x0000_0001u; 0x0000_0000u; 0x0000_0000u; 0x0000_0000u] )
            )
        )
        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    0x00uy; 0x00uy; 0x00uy; 0x01uy;
                |],
                true,
                true
            ),
            Integer.op_Implicit(
                Integer( [0x0000_0001u; 0x0000_0001u; 0x0000_0001u; 0x0000_0001u] )
            )
        )
        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0x12uy; 0x00uy; 0x03uy; 0x40uy;
                    0x05uy; 0x60uy; 0x00uy; 0x78uy;
                    0x12uy; 0x34uy; 0x56uy; 0x78uy;
                    0x9Auy; 0xBCuy; 0xDEuy; 0xF0uy;
                |],
                true,
                true
            ),
            Integer.op_Implicit(
                Integer( [0x1200_0340u; 0x0560_0078u; 0x1234_5678u; 0x9ABC_DEF0u] )
            )
        )

        // NOTE: The different between these last two are whether the BI is negative
        // This is done with the change of the IsUnsigned paramater
        // That is why the "same" BigInteger produces different Integers
        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0xFFuy; 0xFFuy; 0xEEuy; 0xEEuy;
                    0xDDuy; 0xDDuy; 0xCCuy; 0xCCuy;
                    0x44uy; 0x44uy; 0x33uy; 0x33uy;
                    0x22uy; 0x22uy; 0x11uy; 0x11uy;
                |],
                true,
                true
            ),
            Integer.op_Implicit(
                Integer( [0xFFFF_EEEEu; 0xDDDD_CCCCu; 0x4444_3333u; 0x2222_1111u] )
            )
        )
        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0xFFuy; 0xFFuy; 0xEEuy; 0xEEuy;
                    0xDDuy; 0xDDuy; 0xCCuy; 0xCCuy;
                    0x44uy; 0x44uy; 0x33uy; 0x33uy;
                    0x22uy; 0x22uy; 0x11uy; 0x11uy;
                |],
                false,
                true
            ),
            Integer.op_Implicit(
                Integer( [0x0000_1111u; 0x2222_3333u; 0xBBBB_CCCCu; 0xDDDD_EEEFu], true )
            )
        )

    [<Fact>]
    member public this.BigIntegerToInteger () =
        Assert.Equal(
            Integer.Zero,
            Integer.op_Implicit(
                System.Numerics.BigInteger.Zero
            )
        )
        Assert.Equal(
            Integer.Unit,
            Integer.op_Implicit(
                System.Numerics.BigInteger.One
            )
        )
        Assert.Equal(
            Integer.NegativeOne,
            Integer.op_Implicit(
                System.Numerics.BigInteger.MinusOne
            )
        )

        Assert.Equal(
            Integer( [0x0000_0000u; 0x0000_0000u; 0x0000_0001u; 0x0000_0000u] ),
            Integer.op_Implicit(
                System.Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Integer( [0x0000_0000u; 0x0000_0001u; 0x0000_0000u; 0x0000_0000u] ),
            Integer.op_Implicit(
                System.Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Integer( [0x0000_0001u; 0x0000_0000u; 0x0000_0000u; 0x0000_0000u] ),
            Integer.op_Implicit(
                System.Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Integer( [0x0000_0001u; 0x0000_0001u; 0x0000_0001u; 0x0000_0001u] ),
            Integer.op_Implicit(
                System.Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Integer( [0x1200_0340u; 0x0560_0078u; 0x1234_5678u; 0x9ABC_DEF0u] ),
            Integer.op_Implicit(
                System.Numerics.BigInteger(
                    [|
                        0x12uy; 0x00uy; 0x03uy; 0x40uy;
                        0x05uy; 0x60uy; 0x00uy; 0x78uy;
                        0x12uy; 0x34uy; 0x56uy; 0x78uy;
                        0x9Auy; 0xBCuy; 0xDEuy; 0xF0uy;
                    |],
                    true,
                    true
                )
            )
        )

        // NOTE: The different between these last two are whether the Integer is negative
        Assert.Equal(
            Integer( [0xFFFF_EEEEu; 0xDDDD_CCCCu; 0x4444_3333u; 0x2222_1111u] ),
            Integer.op_Implicit(
                System.Numerics.BigInteger(
                    [|
                        0xFFuy; 0xFFuy; 0xEEuy; 0xEEuy;
                        0xDDuy; 0xDDuy; 0xCCuy; 0xCCuy;
                        0x44uy; 0x44uy; 0x33uy; 0x33uy;
                        0x22uy; 0x22uy; 0x11uy; 0x11uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Integer( [0x0000_1111u; 0x2222_3333u; 0xBBBB_CCCCu; 0xDDDD_EEEFu], true ),
            Integer.op_Implicit(
                System.Numerics.BigInteger(
                    [|
                        0xFFuy; 0xFFuy; 0xEEuy; 0xEEuy;
                        0xDDuy; 0xDDuy; 0xCCuy; 0xCCuy;
                        0x44uy; 0x44uy; 0x33uy; 0x33uy;
                        0x22uy; 0x22uy; 0x11uy; 0x11uy;
                    |],
                    false,
                    true
                )
            )
        )
