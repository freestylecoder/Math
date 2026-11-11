using System;
using Xunit;

namespace Integer.Tests;

using Integer = Freestylecoding.Math.Integer;
using static Freestylecoding.Math.CSharp.Tests.Helpers;

// NOTE: The sanity tests are here to do low level tests of a few parts
// This vars me isolate those tests so I can use those parts in the rest of the tests
//
// In other words:
// IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
[Trait( "Type", "Sanity" )]
public class Sanity {
    [Fact]
    public void LeftShift() =>
        Assert.Equal( FListInteger( true, 4u, 0x8000_0000u, 0u ), new Integer( 9u, true ) << 63 );

    [Fact]
    public void RightShift() =>
        Assert.Equal( new Integer( 9u, true ), FListInteger( true, 4u, 0x8000_0000u, 0u ) >> 63 );

    [Fact]
    public void GreaterThanTrue() =>
        Assert.True( FListInteger( 0xBADu, 0xDEADBEEFu ) > FListInteger( true, 0xDEADBEEFu, 0xBADu ) );

    [Fact]
    public void GreaterThanFalseByLessThan() =>
        Assert.False( FListInteger( true, 0xBADu, 0xDEADBEEFu ) > FListInteger( 0xDEADBEEFu, 0xBADu ) );

    [Fact]
    public void GreaterThanFalseByEquals() =>
        Assert.False( FListInteger( true, 0xBADu, 0xDEADBEEFu ) > FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

    [Fact]
    public void Addition() =>
        Assert.Equal( FListInteger( 0xFFFF_FFFFu, 0xFFFF_FFFFu ), FListInteger( 1u, 0u, 0u ) + new Integer( 1u, true ) );

    [Fact]
    public void Subtraction() =>
        Assert.Equal( FListInteger( 2u, 0u, 0u ), FListInteger( 1u, 1u ) - FListInteger( true, 1u, System.UInt32.MaxValue - 1u, System.UInt32.MaxValue ) );

    [Fact]
    public void Multiplication() =>
        Assert.Equal( FListInteger( true, 0x75CD9046u, 0x541D5980u ), new Integer( 0xFEDCBA98u, true ) * FListInteger( 0x76543210u ) );

    [Fact]
    public void DivisionModulo() =>
        Assert.Equal(
            new Tuple<Integer,Integer>( FListInteger( 0xFEDCBA98u ), new Integer( 0x12345678u, true ) ),
            Integer.op_DividePercent( FListInteger( true, 0x75CD9046u, 0x6651AFF8u ), new Integer( 0x76543210u, true ) )
        );

    [Fact( DisplayName = "ToString" )]
    public void ToStringTest() =>
        Assert.Equal( "-1234567890123456789", FListInteger( true, 0x112210F4u, 0x7DE98115u ).ToString() );

    [Fact]
    public void Parse() =>
        Assert.Equal( FListInteger( true, 0x112210F4u, 0x7DE98115u ), Integer.Parse( "-1234567890123456789" ) );
}