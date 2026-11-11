using System;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;
using Integer = Freestylecoding.Math.Integer;

public class EquatableInteger {
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
	public void Sanity( int left, int right, bool expected ) =>
		Assert.Equal(
			expected,
			( new Integer( left ) as System.IEquatable<Integer> ).Equals( new Integer( right ) )
		);

	[Fact]
	public void LargeIntegersTrue() =>
		Assert.True(
			( FListInteger( 0xFu, 0x00000101u ) as System.IEquatable<Integer> ).Equals( FListInteger( 0xFu, 0x00000101u ) )
		);

	[Fact]
	public void LargeIntegersFalse() =>
		Assert.False(
			( FListInteger( 0x8u, 0x00000101u ) as System.IEquatable<Integer> ).Equals( FListInteger( 0xFu, 0x00000101u ) )
		);

	[Fact]
	public void LargeNegativeIntegersTrue() =>
		Assert.True(
			( FListInteger( true, 0xFu, 0x00000101u ) as System.IEquatable<Integer> ).Equals( FListInteger( true, 0xFu, 0x00000101u ) )
		);

	[Fact]
	public void LargeNegativeIntegersFalse() =>
		Assert.False(
			( FListInteger( true, 0x8u, 0x00000101u ) as System.IEquatable<Integer> ).Equals( FListInteger( true, 0xFu, 0x00000101u ) )
		);

	[Fact]
	public void LargeMixedIntegersTrue() {
		Assert.False(
			( FListInteger( false, 0xFu, 0x00000101u ) as System.IEquatable<Integer> ).Equals( FListInteger( true, 0xFu, 0x00000101u ) )
		);

		Assert.False(
			( FListInteger( true, 0xFu, 0x00000101u ) as System.IEquatable<Integer> ).Equals( FListInteger( false, 0xFu, 0x00000101u ) )
		);
	}
}