using System;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class EquatableNatural {
    [Theory]
    [InlineData( 0u, 0u, true )]
    [InlineData( 1u, 0u, false )]
    [InlineData( 0u, 1u, false )]
    [InlineData( 1u, 1u, true )]
    public void Sanity( uint left, uint right, bool expected ) =>
        Assert.Equal(
            expected,
            ( new Natural( left ) as IEquatable<Natural> ).Equals( new Natural( right ) )
        );

    [Fact]
    public void LargeNaturalsTrue() =>
        Assert.True(
            ( FListNatural( 0xFu, 0x00000101u ) as IEquatable<Natural> ).Equals( FListNatural( 0xFu, 0x00000101u ) )
        );

    [Fact]
    public void LargeNaturalsFalse() =>
        Assert.False(
            ( FListNatural( 0x8u, 0x00000101u ) as IEquatable<Natural> ).Equals( FListNatural( 0xFu, 0x00000101u ) )
        );
}