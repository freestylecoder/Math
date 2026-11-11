using System;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Integer = Freestylecoding.Math.Integer;

public class BitwiseAnd {
	[Theory]
	[InlineData( 0, 0, 0 )]
	[InlineData( 1, 0, 0 )]
	[InlineData( 0, 1, 0 )]
	[InlineData( 1, 1, 1 )]
	[InlineData( -1, 1, 1 )]
	[InlineData( 1, -1, 1 )]
	[InlineData( -1, -1, -1 )]
	public void Sanity( int l, int r, int x ) =>
		Assert.Equal( new Integer( x ), new Integer( l ) & new Integer( r ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.Equal( Integer.Unit, FListInteger( 0xFu, 0x00000101u ) & new Integer( 0x00010001 ) );

	[Fact]
	public void BiggerRight() =>
		Assert.Equal( Integer.Unit, new Integer( 0x00010001 ) & FListInteger( 0xFu, 0x00000101u ) );

	[Fact]
	public void Large() =>
		Assert.Equal( FListInteger( 1u, 0u, 0u, 0u ), FListInteger( 1u, 1u, 0u, 0u ) & FListInteger( 1u, 0u, 1u, 0u ) );
}

public class BitwiseOr {
	[Theory]
	[InlineData( 0, 0, 0 )]
	[InlineData( 1, 0, 1 )]
	[InlineData( 0, 1, 1 )]
	[InlineData( 1, 1, 1 )]
	[InlineData( -1, 1, -1 )]
	[InlineData( 1, -1, -1 )]
	[InlineData( -1, -1, -1 )]
	public void Sanity( int l, int r, int x ) =>
		Assert.Equal( new Integer( x ), new Integer( l ) | new Integer( r ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.Equal( FListInteger( 0xFu, 0x10101u ), FListInteger( 0xFu, 0x00000101u ) | new Integer( 0x00010001 ) );

	[Fact]
	public void BiggerRight() =>
		Assert.Equal( FListInteger( 0xFu, 0x10101u ), new Integer( 0x00010001 ) | FListInteger( 0xFu, 0x00000101u ) );

	[Fact]
	public void Large() =>
		Assert.Equal( FListInteger( 1u, 1u, 1u, 0u ), FListInteger( 1u, 1u, 0u, 0u ) | FListInteger( 1u, 0u, 1u, 0u ) );
}

public class BitwiseXor {
	[Theory]
	[InlineData( 0, 0, 0 )]
	[InlineData( 1, 0, 1 )]
	[InlineData( 0, 1, 1 )]
	[InlineData( 1, 1, 0 )]
	[InlineData( -1, 0, -1 )]
	[InlineData( 0, -1, -1 )]
	[InlineData( -1, -1, 0 )]
	[InlineData( -1, 1, 0 )]
	[InlineData( 1, -1, 0 )]
	[InlineData( 1, 2, 3 )]
	[InlineData( -1, 2, -3 )]
	[InlineData( 1, -2, -3 )]
	[InlineData( -1, -2, 3 )]
	public void Sanity( int l, int r, int x ) =>
		Assert.Equal( new Integer( x ), new Integer( l ) ^ new Integer( r ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.Equal( FListInteger( 0xFu, 0x10100u ), FListInteger( 0xFu, 0x00000101u ) ^ new Integer( 0x00010001 ) );

	[Fact]
	public void BiggerRight() =>
		Assert.Equal( FListInteger( 0xFu, 0x10100u ), new Integer( 0x00010001 ) ^ FListInteger( 0xFu, 0x00000101u ) );

	[Fact]
	public void Large() =>
		Assert.Equal( FListInteger( 0u, 1u, 1u, 0u ), FListInteger( 1u, 1u, 0u, 0u ) ^ FListInteger( 1u, 0u, 1u, 0u ) );
}

public class BitwiseNot {
	[Theory]
	[InlineData( 0xFFFFFFFEu, 1u )]
	[InlineData( 1u, 0xFFFFFFFEu )]
	public void Sanity( uint r, uint x ) =>
		Assert.Equal<Integer>( FListInteger( true, x ), ~FListInteger( false, r ) );

	[Fact]
	public void Bigger() =>
		Assert.Equal<Integer>( FListInteger( true, 0xF0123456u, 0x789ABCDEu ), ~FListInteger( false, 0x0FEDCBA9u, 0x87654321u ) );

	[Theory]
	[InlineData( true, false )]
	[InlineData( false, true )]
	public void Sign( bool expected, bool initial ) =>
		Assert.Equal<Integer>( FListInteger( expected, 0xF0123456u, 0x789ABCDEu ), ~FListInteger( initial, 0x0FEDCBA9u, 0x87654321u ) );
}

public class LeftShift {
	[Theory]
	[InlineData( 1, 1, 2 )]
	[InlineData( -1, 1, -2 )]
	public void Sanity( int l, int r, int s ) =>
		Assert.Equal( new Integer( s ), new Integer( l ) << r );

	[Fact]
	public void Multiple() =>
		Assert.Equal( FListInteger( 0x3Cu ), FListInteger( 0xFu ) << 2 );

	[Fact]
	public void Overflow() =>
		Assert.Equal( FListInteger( 1u, 0xFFFFFFFEu ), FListInteger( 0xFFFFFFFFu ) << 1 );

	[Fact]
	public void MultipleOverflow() =>
		Assert.Equal( FListInteger( 0x5u, 0xFFFFFFF8u ), FListInteger( 0xBFFFFFFFu ) << 3 );

	[Fact]
	public void OverOneUInt() =>
		Assert.Equal( FListInteger( 8u, 0u, 0u ), Integer.Unit << 67 );

	[Fact]
	public void OverflowNegative() =>
		Assert.Equal( FListInteger( true, 1u, 0xFFFFFFFEu ), new Integer( 0xFFFFFFFFu, true ) << 1 );

	[Fact]
	public void MultipleOverflowNegative() =>
		Assert.Equal( FListInteger( true, 0x5u, 0xFFFFFFF8u ), new Integer( 0xBFFFFFFFu, true ) << 3 );

	[Fact]
	public void OverOneUIntNegative() =>
		Assert.Equal( FListInteger( true, 8u, 0u, 0u ), Integer.NegativeOne << 67 );
}

public class RightShift {
	[Theory]
	[InlineData( 1, 1, 0 )]
	[InlineData( 15, 2, 3 )]
	[InlineData( 60, 2, 15 )]
	[InlineData( 60, 1, 30 )]
	[InlineData( -1, 1, 0 )]
	[InlineData( -2, 1, -1 )]
	[InlineData( -15, 2, -3 )]
	[InlineData( -60, 2, -15 )]
	[InlineData( -60, 1, -30 )]
	public void Sanity( int l, int r, int s ) =>
		Assert.Equal( new Integer( s ), new Integer( l ) >> r );

	[Fact]
	public void Underflow() =>
		Assert.Equal( FListInteger( 0x7FFFFFFFu ), FListInteger( 0xFFFFFFFFu ) >> 1 );

	[Fact]
	public void MultipleUnderflow() =>
		Assert.Equal( FListInteger( 0x1u, 0x5FFFFFFFu ), FListInteger( 0xAu, 0xFFFFFFFFu ) >> 3 );

	[Fact]
	public void OverOneUInt() =>
		Assert.Equal( Integer.Unit, FListInteger( 0x10u, 0u, 0u ) >> 68 );

	[Fact]
	public void ReduceToZero() =>
		Assert.Equal( Integer.Zero, FListInteger( 0x10u, 0u, 0u ) >> 99 );

	[Fact]
	public void UnderflowNegative() =>
		Assert.Equal( new Integer( 0x7FFFFFFFu, true ), new Integer( 0xFFFFFFFFu, true ) >> 1 );

	[Fact]
	public void MultipleUnderflowNegative() =>
		Assert.Equal( FListInteger( true, 0x1u, 0x5FFFFFFFu ), FListInteger( true, 0xAu, 0xFFFFFFFFu ) >> 3 );

	[Fact]
	public void OverOneUIntNegative() =>
		Assert.Equal( Integer.NegativeOne, FListInteger( true, 0x10u, 0u, 0u ) >> 68 );

	[Fact]
	public void RetainsNegative() =>
		Assert.Equal( new Integer( 1u, true ), new Integer( 2u, true ) >> 1 );

	[Fact]
	public void NegativeReduceToZero() =>
		Assert.Equal( Integer.Zero, FListInteger( true, 0x10u, 0u, 0u ) >> 99 );
}