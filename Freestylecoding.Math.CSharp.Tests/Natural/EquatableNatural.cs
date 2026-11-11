using System;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class EquatableNatural : Freestylecoding.Math.Interface.Tests.TEquatableT<Natural> {
	public override Natural item1 => Natural.Unit;
	public override Natural item2 => Natural.Unit;
	public override Natural item3 => Natural.Unit;
	public override Natural different1 => Natural.Zero;
	public override Natural different2 => FListNatural( int.MaxValue );

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