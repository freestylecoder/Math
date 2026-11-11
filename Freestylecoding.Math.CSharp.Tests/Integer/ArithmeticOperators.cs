using System;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Integer = Freestylecoding.Math.Integer;

public class Addition {
	[Theory]
	[InlineData( 1, 1, 2 )]        // Sanity
	[InlineData( -1, 1, 0 )]        // Sanity
	[InlineData( 1, -1, 0 )]        // Sanity
	[InlineData( -1, -1, -2 )]        // Sanity
	[InlineData( 1, 0, 1 )]        // Sanity
	[InlineData( -1, 0, -1 )]        // Sanity
	[InlineData( 0, 1, 1 )]        // Sanity
	[InlineData( 0, -1, -1 )]        // Sanity
	[InlineData( 1, 5, 6 )]        // l < r
	[InlineData( -1, 5, 4 )]        // l < r
	[InlineData( 1, -5, -4 )]        // l < r
	[InlineData( -1, -5, -6 )]        // l < r
	[InlineData( 9, 2, 11 )]        // l > r
	[InlineData( -9, 2, -7 )]        // l > r
	[InlineData( 9, -2, 7 )]        // l > r
	[InlineData( -9, -2, -11 )]        // l > r
	public void Sanity( int l, int r, int s ) =>
		Assert.Equal( new Integer( s ), new Integer( l ) + new Integer( r ) );

	[Fact]
	public void Overflow() =>
		Assert.Equal( FListInteger( 1u, 2u ), FListInteger( System.UInt32.MaxValue ) + new Integer( 3 ) );

	[Fact]
	public void leftBiggerNoOverflow() =>
		Assert.Equal( FListInteger( 0xFu, 0xFF0Fu ), FListInteger( 0xFu, 0xFu ) + FListInteger( 0xFF00u ) );

	[Fact]
	public void rightBiggerNoOverflow() =>
		Assert.Equal( FListInteger( 0xFu, 0xFF0Fu ), FListInteger( 0xFF00u ) + FListInteger( 0xFu, 0xFu ) );

	[Fact]
	public void CascadingOverflow() =>
		Assert.Equal( FListInteger( 1u, 0u, 0u ), new Integer( 1 ) + FListInteger( System.UInt32.MaxValue, System.UInt32.MaxValue ) );

	[Fact]
	public void OverflowCausedOverflow() =>
		Assert.Equal( FListInteger( 2u, 0u, 0u ), FListInteger( 1u, 1u ) + FListInteger( 1u, System.UInt32.MaxValue - 1u, System.UInt32.MaxValue ) );

	[Fact]
	public void OverflowNegative() =>
		Assert.Equal( FListInteger( true, 1u, 2u ), new Integer( System.UInt32.MaxValue, true ) + new Integer( 3u, true ) );

	[Fact]
	public void leftBiggerNoOverflowNegative() =>
		Assert.Equal( FListInteger( true, 0xFu, 0xFF0Fu ), FListInteger( true, 0xFu, 0xFu ) + new Integer( 0xFF00u, true ) );

	[Fact]
	public void rightBiggerNoOverflowNegative() =>
		Assert.Equal( FListInteger( true, 0xFu, 0xFF0Fu ), new Integer( 0xFF00u, true ) + FListInteger( true, 0xFu, 0xFu ) );

	[Fact]
	public void CascadingOverflowNegative() =>
		Assert.Equal( FListInteger( true, 1u, 0u, 0u ), new Integer( 1u, true ) + FListInteger( true, System.UInt32.MaxValue, System.UInt32.MaxValue ) );

	[Fact]
	public void OverflowCausedOverflowNegative() =>
		Assert.Equal( FListInteger( true, 2u, 0u, 0u ), FListInteger( true, 1u, 1u ) + FListInteger( true, 1u, System.UInt32.MaxValue - 1u, System.UInt32.MaxValue ) );
}

public class Subtraction {
	[Theory]
	[InlineData( 1, 1, 0 )]             // Sanity
	[InlineData( -1, 1, -2 )]             // Sanity
	[InlineData( 1, -1, 2 )]             // Sanity
	[InlineData( -1, -1, 0 )]             // Sanity
	[InlineData( 1, 0, 1 )]             // Sanity
	[InlineData( -1, 0, -1 )]             // Sanity
	[InlineData( 0, 1, -1 )]             // Sanity
	[InlineData( 0, -1, 1 )]             // Sanity
	[InlineData( 9, 2, 7 )]             // l > r
	[InlineData( -9, 2, -11 )]             // l > r
	[InlineData( 9, -2, 11 )]             // l > r
	[InlineData( -9, -2, -7 )]             // l > r
	[InlineData( 1, 5, -4 )]             // l < r
	[InlineData( -1, 5, -6 )]             // l < r
	[InlineData( 1, -5, 6 )]             // l < r
	[InlineData( -1, -5, 4 )]             // l < r
	public void Sanity( int l, int r, int s ) =>
		Assert.Equal( new Integer( s ), new Integer( l ) - new Integer( r ) );

	[Fact]
	public void MultiItemNoUnderflow() {
		Integer l = FListInteger( 3u, 4u );
		Integer r = FListInteger( 1u, 2u );
		Assert.Equal( FListInteger( 2u, 2u ), l - r );
	}

	[Fact]
	public void MultiItemNegativeNoUnderflow() {
		Integer l = FListInteger( true, 3u, 4u );
		Integer r = FListInteger( true, 1u, 2u );
		Assert.Equal( FListInteger( true, 2u, 2u ), l - r );
	}

	[Fact]
	public void MultiItemMixedLeftNoUnderflow() {
		Integer l = FListInteger( true, 3u, 4u );
		Integer r = FListInteger( 1u, 2u );
		Assert.Equal( FListInteger( true, 4u, 6u ), l - r );
	}

	[Fact]
	public void MultiItemMixedRightNoUnderflow() {
		Integer l = FListInteger( 3u, 4u );
		Integer r = FListInteger( true, 1u, 2u );
		Assert.Equal( FListInteger( 4u, 6u ), l - r );
	}

	[Fact]
	public void MultiItemUnderflow() {
		Integer l = FListInteger( 4u, 2u );
		Integer r = FListInteger( 1u, 3u );
		Assert.Equal( FListInteger( 0x2u, 0xFFFFFFFFu ), l - r );
	}

	[Fact]
	public void MultiItemCascadingUnderflow() {
		Integer l = FListInteger( 1u, 0u, 0u );
		Integer r = new Integer( 1 );
		Assert.Equal( FListInteger( 0xFFFFFFFFu, 0xFFFFFFFFu ), l - r );
	}

	[Fact]
	public void MultiItemNegativeOverflow() {
		Integer l = FListInteger( true, 0x2u, 0xFFFFFFFFu );
		Integer r = FListInteger( 1u, 3u );
		Assert.Equal( FListInteger( true, 4u, 2u ), l - r );
	}

	[Fact]
	public void MultiItemCascadingNegativeOverflow() {
		Integer l = FListInteger( true, 0xFFFFFFFFu, 0xFFFFFFFFu );
		Integer r = new Integer( 1 );
		Assert.Equal( FListInteger( true, 1u, 0u, 0u ), l - r );
	}
}

public class Multiply {
	[Theory]
	[InlineData( 1, 1, 1 )]       // Sanity
	[InlineData( -1, 1, -1 )]       // Sanity
	[InlineData( 1, -1, -1 )]       // Sanity
	[InlineData( -1, -1, 1 )]       // Sanity
	[InlineData( 1, 0, 0 )]       // Sanity
	[InlineData( -1, 0, 0 )]       // Sanity
	[InlineData( 0, 1, 0 )]       // Sanity
	[InlineData( 0, -1, 0 )]       // Sanity
	[InlineData( 6, 7, 42 )]       // multiple bits
	[InlineData( -6, 7, -42 )]       // multiple bits
	[InlineData( 6, -7, -42 )]       // multiple bits
	[InlineData( -6, -7, 42 )]       // multiple bits
	public void Sanity( int l, int r, int p ) =>
		Assert.Equal( new Integer( p ), new Integer( l ) * new Integer( r ) );

	[Fact]
	public void Big() =>
		Assert.Equal( FListInteger( 0x75CD9046u, 0x541D5980u ), FListInteger( 0xFEDCBA98u ) * FListInteger( 0x76543210u ) );

	[Fact]
	public void BigMixedLeft() =>
		Assert.Equal( FListInteger( true, 0x75CD9046u, 0x541D5980u ), new Integer( 0xFEDCBA98u, true ) * FListInteger( 0x76543210u ) );

	[Fact]
	public void BigMixedRight() =>
		Assert.Equal( FListInteger( true, 0x75CD9046u, 0x541D5980u ), FListInteger( 0xFEDCBA98u ) * new Integer( 0x76543210u, true ) );

	[Fact]
	public void BigNegative() =>
		Assert.Equal( FListInteger( 0x75CD9046u, 0x541D5980u ), new Integer( 0xFEDCBA98u, true ) * new Integer( 0x76543210u, true ) );
}

public class Division {
	[Theory]
	[InlineData( 1, 1, 1 )]      // Sanity
	[InlineData( -1, 1, -1 )]      // Sanity
	[InlineData( 1, -1, -1 )]      // Sanity
	[InlineData( -1, -1, 1 )]      // Sanity
	[InlineData( 0, 1, 0 )]      // Sanity
	[InlineData( 0, -1, 0 )]      // Sanity
	[InlineData( 44, 7, 6 )]      // multiple bits
	[InlineData( -44, 7, -6 )]      // multiple bits
	[InlineData( 44, -7, -6 )]      // multiple bits
	[InlineData( -44, -7, 6 )]      // multiple bits
	[InlineData( 52, 5, 10 )]      // rev
	[InlineData( -52, 5, -10 )]      // rev
	[InlineData( 52, -5, -10 )]      // rev
	[InlineData( -52, -5, 10 )]      // rev
	[InlineData( 52, 10, 5 )]      // rev
	[InlineData( -52, 10, -5 )]      // rev
	[InlineData( 52, -10, -5 )]      // rev
	[InlineData( -52, -10, 5 )]      // rev
	public void Sanity( int dividend, int divisor, int quotient ) =>
		Assert.Equal( new Integer( quotient ), new Integer( dividend ) / new Integer( divisor ) );

	[Fact]
	public void Zero() =>
		Assert.Equal( new Integer( 0 ), new Integer( 5 ) / new Integer( 10 ) );

	[Fact]
	public void DivideByZero() =>
		Assert.Throws<DivideByZeroException>( () => ( Integer.Unit / Integer.Zero ) );

	[Fact]
	public void Big() =>
		Assert.Equal( FListInteger( 0xFEDCBA98u ), FListInteger( 0x75CD9046u, 0x6651AFF8u ) / new Integer( 0x76543210 ) );

	[Fact]
	public void BigMixedLeft() =>
		Assert.Equal( new Integer( 0xFEDCBA98u, true ), FListInteger( true, 0x75CD9046u, 0x6651AFF8u ) / new Integer( 0x76543210 ) );

	[Fact]
	public void BigMixedRight() =>
		Assert.Equal( new Integer( 0xFEDCBA98u, true ), FListInteger( 0x75CD9046u, 0x6651AFF8u ) / new Integer( 0x76543210u, true ) );

	[Fact]
	public void BigNegative() =>
		Assert.Equal( FListInteger( 0xFEDCBA98u ), FListInteger( true, 0x75CD9046u, 0x6651AFF8u ) / new Integer( 0x76543210u, true ) );
}

public class Modulo {
	[Theory]
	[InlineData( 1, 1, 0 )]      // Sanity
	[InlineData( -1, 1, 0 )]      // Sanity
	[InlineData( 1, -1, 0 )]      // Sanity
	[InlineData( -1, -1, 0 )]      // Sanity
	[InlineData( 0, 1, 0 )]      // Sanity
	[InlineData( 0, -1, 0 )]      // Sanity
	[InlineData( 44, 7, 2 )]      // multiple bits
	[InlineData( -44, 7, -2 )]      // multiple bits
	[InlineData( 44, -7, 2 )]      // multiple bits
	[InlineData( -44, -7, -2 )]      // multiple bits
	[InlineData( 52, 5, 2 )]      // rev
	[InlineData( -52, 5, -2 )]      // rev
	[InlineData( 52, -5, 2 )]      // rev
	[InlineData( -52, -5, -2 )]      // rev
	[InlineData( 52, 10, 2 )]      // rev
	[InlineData( -52, 10, -2 )]      // rev
	[InlineData( 52, -10, 2 )]      // rev
	[InlineData( -52, -10, -2 )]      // rev
	public void Sanity( int dividend, int divisor, int remainder ) =>
		Assert.Equal( new Integer( remainder ), new Integer( dividend ) % new Integer( divisor ) );

	[Fact]
	public void Zero() =>
		Assert.Equal( new Integer( 0 ), new Integer( 20 ) % new Integer( 10 ) );

	[Fact]
	public void DivideByZero() =>
		Assert.Throws<DivideByZeroException>( () => ( Integer.Unit % Integer.Zero ) );

	[Fact]
	public void Big() =>
		Assert.Equal( new Integer( 0x12345678 ), FListInteger( 0x75CD9046u, 0x6651AFF8u ) % new Integer( 0x76543210 ) );

	[Fact]
	public void BigMixedLeft() =>
		Assert.Equal( new Integer( 0x12345678u, true ), FListInteger( true, 0x75CD9046u, 0x6651AFF8u ) % new Integer( 0x76543210 ) );

	[Fact]
	public void BigMixedRight() =>
		Assert.Equal( new Integer( 0x12345678 ), FListInteger( 0x75CD9046u, 0x6651AFF8u ) % new Integer( 0x76543210u, true ) );

	[Fact]
	public void BigNegative() =>
		Assert.Equal( new Integer( 0x12345678u, true ), FListInteger( true, 0x75CD9046u, 0x6651AFF8u ) % new Integer( 0x76543210u, true ) );
}

public class DivisionModulo {
	[Theory]
	[InlineData( 1, 1, 1, 0 )]      // Sanity
	[InlineData( -1, 1, -1, 0 )]      // Sanity
	[InlineData( 1, -1, -1, 0 )]      // Sanity
	[InlineData( -1, -1, 1, 0 )]      // Sanity
	[InlineData( 0, 1, 0, 0 )]      // Sanity
	[InlineData( 0, -1, 0, 0 )]      // Sanity
	[InlineData( 44, 7, 6, 2 )]      // multiple bits
	[InlineData( -44, 7, -6, -2 )]      // multiple bits
	[InlineData( 44, -7, -6, 2 )]      // multiple bits
	[InlineData( -44, -7, 6, -2 )]      // multiple bits
	[InlineData( 52, 5, 10, 2 )]      // rev
	[InlineData( -52, 5, -10, -2 )]      // rev
	[InlineData( 52, -5, -10, 2 )]      // rev
	[InlineData( -52, -5, 10, -2 )]      // rev
	[InlineData( 52, 10, 5, 2 )]      // rev
	[InlineData( -52, 10, -5, -2 )]      // rev
	[InlineData( 52, -10, -5, 2 )]      // rev
	[InlineData( -52, -10, 5, -2 )]      // rev
	public void Sanity( int dividend, int divisor, int quotient, int remainder ) =>
		Assert.Equal(
			new Tuple<Integer, Integer>( new Integer( quotient ), new Integer( remainder ) ),
			Integer.op_DividePercent( new Integer( dividend ), new Integer( divisor ) )
		);

	[Fact]
	public void DivideByZero() =>
		Assert.Throws<DivideByZeroException>( () => Integer.op_DividePercent( Integer.Unit, Integer.Zero ) );

	[Fact]
	public void Big() =>
		Assert.Equal(
			new Tuple<Integer, Integer>( FListInteger( 0xFEDCBA98u ), new Integer( 0x12345678 ) ),
			Integer.op_DividePercent( FListInteger( 0x75CD9046u, 0x6651AFF8u ), new Integer( 0x76543210 ) )
		);

	[Fact]
	public void BigMixedLeft() =>
		Assert.Equal(
			new Tuple<Integer, Integer>( new Integer( 0xFEDCBA98u, true ), new Integer( 0x12345678u, true ) ),
			Integer.op_DividePercent( FListInteger( true, 0x75CD9046u, 0x6651AFF8u ), new Integer( 0x76543210 ) )
		);

	[Fact]
	public void BigMixedRight() =>
		Assert.Equal(
			new Tuple<Integer, Integer>( new Integer( 0xFEDCBA98u, true ), new Integer( 0x12345678 ) ),
			Integer.op_DividePercent( FListInteger( 0x75CD9046u, 0x6651AFF8u ), new Integer( 0x76543210u, true ) )
		);

	[Fact]
	public void BigNegative() =>
		Assert.Equal(
			new Tuple<Integer, Integer>( FListInteger( 0xFEDCBA98u ), new Integer( 0x12345678u, true ) ),
			Integer.op_DividePercent( FListInteger( true, 0x75CD9046u, 0x6651AFF8u ), new Integer( 0x76543210u, true ) )
		);
}

public class Negation {
	[Theory]
	[InlineData( 1, -1 )]           // Sanity
	[InlineData( -1, 1 )]           // Sanity
	[InlineData( 0, 0 )]           // Sanity
	public void Sanity( int a, int e ) =>
		Assert.Equal( new Integer( e ), -new Integer( a ) );

	[Fact]
	public void Big() =>
		Assert.Equal( FListInteger( true, 0xFEDCBA9u, 0x76543210u ), -FListInteger( 0xFEDCBA9u, 0x76543210u ) );

	[Fact]
	public void BigNegative() =>
		Assert.Equal( FListInteger( 0xFEDCBA9u, 0x76543210u ), -FListInteger( true, 0xFEDCBA9u, 0x76543210u ) );
}