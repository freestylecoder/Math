namespace Natural

open Xunit
open Freestylecoding.Math

type public Implicit() =
    // Yes, these look a little funny
    // F# doesn't handle Implicit conversions very well
    // These methods mostly exist for C#

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
