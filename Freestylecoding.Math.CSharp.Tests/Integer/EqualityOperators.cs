using System;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Integer = Freestylecoding.Math.Integer;
using Natural = Freestylecoding.Math.Natural;

public class EqualityOperators {
    static bool Equality<T>( T left, T right ) where T : System.Numerics.IEqualityOperators<T,T,bool> =>
        left == right;
    static bool Inequality<T>( T left, T right ) where T : System.Numerics.IEqualityOperators<T,T,bool> =>
        left != right;

    [Theory]
    [InlineData(  0,  0, true)]
    [InlineData(  1,  0, false)]
    [InlineData(  0,  1, false)]
    [InlineData(  1,  1, true)]
    [InlineData( -1,  0, false)]
    [InlineData(  0, -1, false)]
    [InlineData(  1, -1, false)]
    [InlineData( -1,  1, false)]
    [InlineData( -1, -1, true)]
    public void EqualitySanity( int left, int right, bool expected ) =>
        Assert.Equal(
            expected,
            Equality(
                new Integer( left ),
                new Integer( right )
            )
        );

    [Fact]
    public void EqualityLargeIntegersTrue() =>
        Assert.True(
            Equality(
                FListInteger( 0xFu, 0x00000101u ),
                FListInteger( 0xFu, 0x00000101u )
            )
        );

    [Fact]
    public void EqualityLargeIntegersFalse() =>
        Assert.False(
            Equality(
                FListInteger( 0x8u, 0x00000101u ),
                FListInteger( 0xFu, 0x00000101u )
            )
        );

    [Fact]
    public void EqualityLargeNegativeIntegersTrue() =>
        Assert.True(
            Equality(
                FListInteger( true, 0xFu, 0x00000101u ),
                FListInteger( true, 0xFu, 0x00000101u )
            )
        );

    [Fact]
    public void EqualityLargeNegativeIntegersFalse() =>
        Assert.False(
            Equality(
                FListInteger( true, 0x8u, 0x00000101u ),
                FListInteger( true, 0xFu, 0x00000101u )
            )
        );

    [Fact]
    public void EqualityLargeMixedIntegersTrue() {
        Assert.False(
            Equality(
                FListInteger( false, 0xFu, 0x00000101u ),
                FListInteger( true, 0xFu, 0x00000101u )
            )
        );

        Assert.False(
            Equality(
                FListInteger( true, 0xFu, 0x00000101u ),
                FListInteger( false, 0xFu, 0x00000101u )
            )
        );
    }

    [Theory]
    [InlineData( 0, 0, false )]
    [InlineData( 1, 0, true )]
    [InlineData( 0, 1, true )]
    [InlineData( 1, 1, false )]
    [InlineData( -1, 0, true )]
    [InlineData( 0, -1, true )]
    [InlineData( 1, -1, true )]
    [InlineData( -1, 1, true )]
    [InlineData( -1, -1, false )]
    public void InequalitySanity( int left, int right, bool expected ) =>
        Assert.Equal(
            expected,
            Inequality(
                new Integer( left ),
                new Integer( right )
            )
        );

    [Fact]
    public void InequalityLargeIntegersFalse() =>
        Assert.False(
            Inequality(
                FListInteger( 0xFu, 0x00000101u ),
                FListInteger( 0xFu, 0x00000101u )
            )
        );

    [Fact]
    public void InequalityLargeIntegersTrue() =>
        Assert.True(
            Inequality(
                FListInteger( 0x8u, 0x00000101u ),
                FListInteger( 0xFu, 0x00000101u )
            )
        );

    [Fact]
    public void InequalityLargeNegativeIntegersFalse() =>
        Assert.False(
            Inequality(
                FListInteger( true, 0xFu, 0x00000101u ),
                FListInteger( true, 0xFu, 0x00000101u )
            )
        );

    [Fact]
    public void InequalityLargeNegativeIntegersTrue() =>
        Assert.True(
            Inequality(
                FListInteger( true, 0x8u, 0x00000101u ),
                FListInteger( true, 0xFu, 0x00000101u )
            )
        );

    [Fact]
    public void InequalityLargeMixedIntegersTrue() {
        Assert.True(
            Inequality(
                FListInteger( false, 0xFu, 0x00000101u ),
                FListInteger( true, 0xFu, 0x00000101u )
            )
        );

        Assert.True(
            Inequality(
                FListInteger( true, 0xFu, 0x00000101u ),
                FListInteger( false, 0xFu, 0x00000101u )
            )
        );
    }
}