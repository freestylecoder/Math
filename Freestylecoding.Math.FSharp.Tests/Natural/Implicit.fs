namespace Natural

open Xunit
open Freestylecoding.Math

type public Implicit() =
    // Yes, these look a little funny
    // F# doesn't handle Implicit conversions very well
    // These methods mostly exist for C#

    [<Theory>]
    [<InlineData(   0uy )>]
    [<InlineData(   1uy )>]
    [<InlineData(   2uy )>]
    [<InlineData(   5uy )>]
    [<InlineData( 100uy )>]
    member public this.Uint8 (actual:uint8) =
        Assert.Equal( Natural( actual ), Natural.op_Implicit( actual ) )

    [<Theory>]
    [<InlineData(   0us )>]
    [<InlineData(   1us )>]
    [<InlineData(   2us )>]
    [<InlineData(   5us )>]
    [<InlineData( 100us )>]
    member public this.Uint16 (actual:uint16) =
        Assert.Equal( Natural( actual ), Natural.op_Implicit( actual ) )

    [<Theory>]
    [<InlineData(   0u )>]
    [<InlineData(   1u )>]
    [<InlineData(   2u )>]
    [<InlineData(   5u )>]
    [<InlineData( 100u )>]
    member public this.Uint32 (actual:uint32) =
        Assert.Equal( Natural( [actual] ), Natural.op_Implicit( actual ) )

    [<Theory>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL )>]
    [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001uL )>]
    [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000uL )>]
    [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL )>]
    [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078uL )>]
    [<InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0uL )>]
    [<InlineData( 0xFFFF_0000u, 0x0000_0000u, 0xFFFF_0000_0000_0000uL )>]
    [<InlineData( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0xFFFF_EEEE_DDDD_CCCCuL )>]
    member public this. Uint64 (expectedHigh:uint32) (expectedLow:uint32) (actual:uint64) =
        Assert.Equal( Natural( [expectedHigh; expectedLow] ), Natural.op_Implicit( actual ) )

    [<Theory>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0000uL )>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0001uL )>]
    [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0001_0000_0000uL )>]
    [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0001uL, 0x0000_0000_0000_0000uL )>]
    [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001_0000_0000uL, 0x0000_0000_0000_0000uL )>]
    [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL, 0x0000_0001_0000_0001uL )>]
    [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u, 0x1200_0340_0560_0078uL, 0x1234_5678_9ABC_DEF0uL )>]
    member public this.Uint128 (i0:uint32) (i1:uint32) (i2:uint32) (i3:uint32) (l0:uint64) (l1:uint64) =
        Assert.Equal( Natural( [i0; i1; i2; i3] ), Natural.op_Implicit( System.UInt128( l0, l1 ) ) )

    [<Fact>]
    member public this.BigInteger () =
        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                |],
                true,
                true
            ),
            Natural.op_Implicit(
                Natural( [0x0000_0000u; 0x0000_0000u; 0x0000_0000u; 0x0000_0000u] )
            )
        )
        Assert.Equal(
            System.Numerics.BigInteger(
                [|
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    0x00uy; 0x00uy; 0x00uy; 0x01uy;
                |],
                true,
                true
            ),
            Natural.op_Implicit(
                Natural( [0x0000_0000u; 0x0000_0000u; 0x0000_0000u; 0x0000_0001u] )
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
            Natural.op_Implicit(
                Natural( [0x0000_0000u; 0x0000_0000u; 0x0000_0001u; 0x0000_0000u] )
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
            Natural.op_Implicit(
                Natural( [0x0000_0000u; 0x0000_0001u; 0x0000_0000u; 0x0000_0000u] )
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
            Natural.op_Implicit(
                Natural( [0x0000_0001u; 0x0000_0000u; 0x0000_0000u; 0x0000_0000u] )
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
            Natural.op_Implicit(
                Natural( [0x0000_0001u; 0x0000_0001u; 0x0000_0001u; 0x0000_0001u] )
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
            Natural.op_Implicit(
                Natural( [0x1200_0340u; 0x0560_0078u; 0x1234_5678u; 0x9ABC_DEF0u] )
            )
        )
