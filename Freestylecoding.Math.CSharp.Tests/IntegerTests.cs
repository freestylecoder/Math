using System;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests {
	// NOTE: The sanity tests are here to do low level tests of a few parts
	// This lets me isolate those tests so I can use those parts in the rest of the tests
	//
	// In other words:
	// IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
	[Trait( "Type", "Sanity" )]
	public class IntegerSanity {
		[Fact]
		public void LeftShift() =>
			Assert.Equal( new Integer( new[] { 4u, 0x8000_0000u, 0u }, true ), new Integer( new[] { 9u }, true ) << 63 );

		[Fact]
		public void RightShift() =>
			Assert.Equal( new Integer( new[] { 9u }, true ), new Integer( new[] { 4u, 0x8000_0000u, 0u }, true ) >> 63 );

		[Fact]
		public void GreaterThanTrue() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) > new Integer( new[] { 0xDEADBEEFu, 0xBADu }, true ) );

		[Fact]
		public void GreaterThanFalseByLessThan() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) > new Integer( new[] { 0xDEADBEEFu, 0xBADu } ) );

		[Fact]
		public void GreaterThanFalseByEquals() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) > new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void Addition() =>
			Assert.Equal( new Integer( new[] { 0xFFFF_FFFFu, 0xFFFF_FFFFu } ), new Integer( new[] { 1u, 0u, 0u } ) + new Integer( new[] { 1u }, true ) );

		[Fact]
		public void Subtraction() =>
			Assert.Equal( new Integer( new[] { 2u, 0u, 0u } ), new Integer( new[] { 1u, 1u } ) - new Integer( new[] { 1u, uint.MaxValue - 1u, uint.MaxValue }, true ) );

		[Fact]
		public void Multiplication() =>
			Assert.Equal( new Integer( new[] { 0x75CD9046u, 0x541D5980u }, true ), new Integer( new[] { 0xFEDCBA98u }, true ) * new Integer( new[] { 0x76543210u } ) );

		[Fact]
		public void ToStringTest() =>
			Assert.Equal( "-1234567890123456789", new Integer( new[] { 0x112210F4u, 0x7DE98115u }, true ).ToString() );

		[Fact]
		public void Parse() =>
			Assert.Equal( new Integer( new[] { 0x112210F4u, 0x7DE98115u }, true ), Integer.Parse( "-1234567890123456789" ) );
	}

	public class IntegerAnd {
		[Theory]
		[InlineData( "0", "0", "0" )]
		[InlineData( "1", "0", "0" )]
		[InlineData( "0", "1", "0" )]
		[InlineData( "1", "1", "1" )]
		[InlineData( "-1", "1", "1" )]
		[InlineData( "1", "-1", "1" )]
		[InlineData( "-1", "-1", "-1" )]
		public void Sanity( string l, string r, string x ) =>
			Assert.Equal( Integer.Parse( x ), Integer.Parse( l ) & Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.Equal( Integer.Unit, new Integer( new[] { 0xFu, 0x00000101u } ) & new Integer( new[] { 0x00010001u } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.Equal( Integer.Unit, new Integer( new[] { 0x00010001u } ) & new Integer( new[] { 0xFu, 0x00000101u } ) );
	}

	public class IntegerOr {
		[Theory]
		[InlineData( "0", "0", "0" )]
		[InlineData( "1", "0", "1" )]
		[InlineData( "0", "1", "1" )]
		[InlineData( "1", "1", "1" )]
		[InlineData( "-1", "1", "-1" )]
		[InlineData( "1", "-1", "-1" )]
		[InlineData( "-1", "-1", "-1" )]
		public void Sanity( string l, string r, string x ) =>
			Assert.Equal( Integer.Parse( x ), Integer.Parse( l ) | Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.Equal( new Integer( new[] { 0xFu, 0x10101u } ), new Integer( new[] { 0xFu, 0x00000101u } ) | new Integer( new[] { 0x00010001u } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.Equal( new Integer( new[] { 0xFu, 0x10101u } ), new Integer( new[] { 0x00010001u } ) | new Integer( new[] { 0xFu, 0x00000101u } ) );
	}

	public class IntegerXor {
		[Theory]
		[InlineData( "0", "0", "0" )]
		[InlineData( "1", "0", "1" )]
		[InlineData( "0", "1", "1" )]
		[InlineData( "1", "1", "0" )]
		[InlineData( "-1", "0", "-1" )]
		[InlineData( "0", "-1", "-1" )]
		[InlineData( "-1", "-1", "0" )]
		[InlineData( "-1", "1", "0" )]
		[InlineData( "1", "-1", "0" )]
		[InlineData( "1", "2", "3" )]
		[InlineData( "-1", "2", "-3" )]
		[InlineData( "1", "-2", "-3" )]
		[InlineData( "-1", "-2", "3" )]
		public void Sanity( string l, string r, string x ) =>
			Assert.Equal( Integer.Parse( x ), Integer.Parse( l ) ^ Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.Equal( new Integer( new[] { 0xFu, 0x10100u } ), new Integer( new[] { 0xFu, 0x00000101u } ) ^ new Integer( new[] { 0x00010001u } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.Equal( new Integer( new[] { 0xFu, 0x10100u } ), new Integer( new[] { 0x00010001u } ) ^ new Integer( new[] { 0xFu, 0x00000101u } ) );
	}

	//public class IntegerBitwiseNot {
	//	// NOTE: These are not going through Parse because it would be
	//	// more difficult to casually tell what is going on.
	//	[Theory]
	//	[InlineData( 0xFFFFFFFEu, 1u )]
	//	[InlineData( 1u, 0xFFFFFFFEu )]
	//	public void Sanity( uint r, uint x ) =>
	//		Assert.Equal( new Integer( new[] { x }, true ), ~ new Integer( new[] { r }, false) );

	//       [Fact]
	//	public void Bigger() =>
	//		Assert.Equal( new Integer( new[] { 0xF0123456u, 0x789ABCDEu }, true ), ~ new Integer( new[] { 0x0FEDCBA9u, 0x87654321u }, false ) );

	//	[Theory]
	//	[InlineData( true, false )]
	//	[InlineData( false, true )]
	//	let Sign expected initial =
	//		Assert.Equal( new Integer(new[] { 0xF0123456u, 0x789ABCDEu }, expected), ~~~ new Integer(new[] { 0x0FEDCBA9u, 0x87654321u }, initial) );
	//}

	public class IntegerLeftShift {
		[Theory]
		[InlineData( "1", 1, "2" )]
		[InlineData( "-1", 1, "-2" )]
		public void Sanity( string l, int r, string s ) =>
			Assert.Equal( Integer.Parse( s ), Integer.Parse( l ) << r );

		[Fact]
		public void Multiple() =>
			Assert.Equal( new Integer( new[] { 0x3Cu } ), new Integer( new[] { 0xFu } ) << 2 );

		[Fact]
		public void Overflow() =>
			Assert.Equal( new Integer( new[] { 1u, 0xFFFFFFFEu } ), new Integer( new[] { 0xFFFFFFFFu } ) << 1 );

		[Fact]
		public void MultipleOverflow() =>
			Assert.Equal( new Integer( new[] { 0x5u, 0xFFFFFFF8u } ), new Integer( new[] { 0xBFFFFFFFu } ) << 3 );

		[Fact]
		public void OverOneUInt() =>
			Assert.Equal( new Integer( new[] { 8u, 0u, 0u } ), Integer.Unit << 67 );

		[Fact]
		public void OverflowNegative() =>
			Assert.Equal( new Integer( new[] { 1u, 0xFFFFFFFEu }, true ), new Integer( new[] { 0xFFFFFFFFu }, true ) << 1 );

		[Fact]
		public void MultipleOverflowNegative() =>
			Assert.Equal( new Integer( new[] { 0x5u, 0xFFFFFFF8u }, true ), new Integer( new[] { 0xBFFFFFFFu }, true ) << 3 );

		[Fact]
		public void OverOneUIntNegative() =>
			Assert.Equal( new Integer( new[] { 8u, 0u, 0u }, true ), new Integer( Natural.Unit, true ) << 67 );
	}

	public class IntegerRightShift {
		[Theory]
		[InlineData( "1", 1, "0" )]
		[InlineData( "15", 2, "3" )]
		[InlineData( "60", 2, "15" )]
		[InlineData( "60", 1, "30" )]
		[InlineData( "-1", 1, "0" )]
		[InlineData( "-2", 1, "-1" )]
		[InlineData( "-15", 2, "-3" )]
		[InlineData( "-60", 2, "-15" )]
		[InlineData( "-60", 1, "-30" )]
		public void Sanity( string l, int r, string s ) =>
			Assert.Equal( Integer.Parse( s ), Integer.Parse( l ) >> r );

		[Fact]
		public void Underflow() =>
			Assert.Equal( new Integer( new[] { 0x7FFFFFFFu } ), new Integer( new[] { 0xFFFFFFFFu } ) >> 1 );

		[Fact]
		public void MultipleUnderflow() =>
			Assert.Equal( new Integer( new[] { 0x1u, 0x5FFFFFFFu } ), new Integer( new[] { 0xAu, 0xFFFFFFFFu } ) >> 3 );

		[Fact]
		public void OverOneUInt() =>
			Assert.Equal( Integer.Unit, new Integer( new[] { 0x10u, 0u, 0u } ) >> 68 );

		[Fact]
		public void ReduceToZero() =>
			Assert.Equal( Integer.Zero, new Integer( new[] { 0x10u, 0u, 0u } ) >> 99 );

		[Fact]
		public void UnderflowNegative() =>
			Assert.Equal( new Integer( new[] { 0x7FFFFFFFu }, true ), new Integer( new[] { 0xFFFFFFFFu }, true ) >> 1 );

		[Fact]
		public void MultipleUnderflowNegative() =>
			Assert.Equal( new Integer( new[] { 0x1u, 0x5FFFFFFFu }, true ), new Integer( new[] { 0xAu, 0xFFFFFFFFu }, true ) >> 3 );

		[Fact]
		public void OverOneUIntNegative() =>
			Assert.Equal( new Integer( Natural.Unit, true ), new Integer( new[] { 0x10u, 0u, 0u }, true ) >> 68 );

		[Fact]
		public void RetainsNegative() =>
			Assert.Equal( new Integer( new[] { 1u }, true ), new Integer( new[] { 2u }, true ) >> 1 );

		[Fact]
		public void NegativeReduceToZero() =>
			Assert.Equal( Integer.Zero, new Integer( new[] { 0x10u, 0u, 0u }, true ) >> 99 );
	}

	public class IntegerEquality {
		[Theory]
		[InlineData( "0", "0", true )]
		[InlineData( "0", "1", false )]
		[InlineData( "1", "0", false )]
		[InlineData( "1", "1", true )]
		[InlineData( "0", "-1", false )]
		[InlineData( "-1", "0", false )]
		[InlineData( "-1", "1", false )]
		[InlineData( "1", "-1", false )]
		[InlineData( "-1", "-1", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Integer.Parse( l ) == Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) == new Integer( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu } ) == new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) );
	}

	public class IntegerGreaterThan {
		[Theory]
		[InlineData( "0", "1", false )]
		[InlineData( "1", "0", true )]
		[InlineData( "0", "-1", true )]
		[InlineData( "-1", "0", false )]
		[InlineData( "1", "1", false )]
		[InlineData( "-1", "1", false )]
		[InlineData( "1", "-1", true )]
		[InlineData( "-1", "-1", false )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Integer.Parse( l ) > Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) > new Integer( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu } ) > new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void CascadeGreaterThan() =>
			Assert.True( new Integer( new[] { 1u, 1u } ) > new Integer( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeEqual() =>
			Assert.False( new Integer( new[] { 1u, 0u } ) > new Integer( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeLessThan() =>
			Assert.False( new Integer( new[] { 1u, 0u } ) > new Integer( new[] { 1u, 1u } ) );

		[Fact]
		public void BiggerLeftNegative() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) > new Integer( new[] { 0xDEADBEEFu }, true ) );

		[Fact]
		public void BiggerRightNegative() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu }, true ) > new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void CascadeGreaterThanNegative() =>
			Assert.False( new Integer( new[] { 1u, 1u }, true ) > new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeEqualNegative() =>
			Assert.False( new Integer( new[] { 1u, 0u }, true ) > new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeLessThanNegative() =>
			Assert.True( new Integer( new[] { 1u, 0u }, true ) > new Integer( new[] { 1u, 1u }, true ) );

		[Fact]
		public void BiggerLeftMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) > new Integer( new[] { 0xDEADBEEFu }, false ) );

		[Fact]
		public void BiggerRightMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu }, true ) > new Integer( new[] { 0xBADu, 0xDEADBEEFu }, false ) );

		[Fact]
		public void CascadeGreaterThanMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 1u, 1u }, true ) > new Integer( new[] { 1u, 0u }, false ) );

		[Fact]
		public void CascadeEqualMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 1u, 0u }, true ) > new Integer( new[] { 1u, 0u }, false ) );

		[Fact]
		public void CascadeLessThanMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 1u, 0u }, true ) > new Integer( new[] { 1u, 1u }, false ) );

		[Fact]
		public void BiggerLeftMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, false ) > new Integer( new[] { 0xDEADBEEFu }, true ) );

		[Fact]
		public void BiggerRightMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu }, false ) > new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void CascadeGreaterThanMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 1u, 1u }, false ) > new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeEqualMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 1u, 0u }, false ) > new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeLessThanMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 1u, 0u }, false ) > new Integer( new[] { 1u, 1u }, true ) );
	}

	public class IntegerLessThan {
		[Theory]
		[InlineData( "0", "1", true )]
		[InlineData( "1", "0", false )]
		[InlineData( "0", "-1", false )]
		[InlineData( "-1", "0", true )]
		[InlineData( "1", "1", false )]
		[InlineData( "-1", "1", true )]
		[InlineData( "1", "-1", false )]
		[InlineData( "-1", "-1", false )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Integer.Parse( l ) < Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) < new Integer( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu } ) < new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void CascadeGreaterThan() =>
			Assert.False( new Integer( new[] { 1u, 1u } ) < new Integer( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeEqual() =>
			Assert.False( new Integer( new[] { 1u, 0u } ) < new Integer( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeLessThan() =>
			Assert.True( new Integer( new[] { 1u, 0u } ) < new Integer( new[] { 1u, 1u } ) );

		[Fact]
		public void BiggerLeftNegative() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) < new Integer( new[] { 0xDEADBEEFu }, true ) );

		[Fact]
		public void BiggerRightNegative() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu }, true ) < new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void CascadeGreaterThanNegative() =>
			Assert.True( new Integer( new[] { 1u, 1u }, true ) < new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeEqualNegative() =>
			Assert.False( new Integer( new[] { 1u, 0u }, true ) < new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeLessThanNegative() =>
			Assert.False( new Integer( new[] { 1u, 0u }, true ) < new Integer( new[] { 1u, 1u }, true ) );

		[Fact]
		public void BiggerLeftMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) < new Integer( new[] { 0xDEADBEEFu }, false ) );

		[Fact]
		public void BiggerRightMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu }, true ) < new Integer( new[] { 0xBADu, 0xDEADBEEFu }, false ) );

		[Fact]
		public void CascadeGreaterThanMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 1u, 1u }, true ) < new Integer( new[] { 1u, 0u }, false ) );

		[Fact]
		public void CascadeEqualMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 1u, 0u }, true ) < new Integer( new[] { 1u, 0u }, false ) );

		[Fact]
		public void CascadeLessThanMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 1u, 0u }, true ) < new Integer( new[] { 1u, 1u }, false ) );

		[Fact]
		public void BiggerLeftMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, false ) < new Integer( new[] { 0xDEADBEEFu }, true ) );

		[Fact]
		public void BiggerRightMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu }, false ) < new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void CascadeGreaterThanMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 1u, 1u }, false ) < new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeEqualMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 1u, 0u }, false ) < new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeLessThanMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 1u, 0u }, false ) < new Integer( new[] { 1u, 1u }, true ) );
	}

	public class IntegerGreaterThanOrEqual {
		[Theory]
		[InlineData( "0", "1", false )]
		[InlineData( "1", "0", true )]
		[InlineData( "0", "-1", true )]
		[InlineData( "-1", "0", false )]
		[InlineData( "1", "1", true )]
		[InlineData( "-1", "1", false )]
		[InlineData( "1", "-1", true )]
		[InlineData( "-1", "-1", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Integer.Parse( l ) >= Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) >= new Integer( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu } ) >= new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void CascadeGreaterThan() =>
			Assert.True( new Integer( new[] { 1u, 1u } ) >= new Integer( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeEqual() =>
			Assert.True( new Integer( new[] { 1u, 0u } ) >= new Integer( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeLessThan() =>
			Assert.False( new Integer( new[] { 1u, 0u } ) >= new Integer( new[] { 1u, 1u } ) );

		[Fact]
		public void BiggerLeftNegative() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) >= new Integer( new[] { 0xDEADBEEFu }, true ) );

		[Fact]
		public void BiggerRightNegative() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu }, true ) >= new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void CascadeGreaterThanNegative() =>
			Assert.False( new Integer( new[] { 1u, 1u }, true ) >= new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeEqualNegative() =>
			Assert.True( new Integer( new[] { 1u, 0u }, true ) >= new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeLessThanNegative() =>
			Assert.True( new Integer( new[] { 1u, 0u }, true ) >= new Integer( new[] { 1u, 1u }, true ) );

		[Fact]
		public void BiggerLeftMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) >= new Integer( new[] { 0xDEADBEEFu }, false ) );

		[Fact]
		public void BiggerRightMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu }, true ) >= new Integer( new[] { 0xBADu, 0xDEADBEEFu }, false ) );

		[Fact]
		public void CascadeGreaterThanMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 1u, 1u }, true ) >= new Integer( new[] { 1u, 0u }, false ) );

		[Fact]
		public void CascadeEqualMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 1u, 0u }, true ) >= new Integer( new[] { 1u, 0u }, false ) );

		[Fact]
		public void CascadeLessThanMixedNegativeLeft() =>
			Assert.False( new Integer( new[] { 1u, 0u }, true ) >= new Integer( new[] { 1u, 1u }, false ) );

		[Fact]
		public void BiggerLeftMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, false ) >= new Integer( new[] { 0xDEADBEEFu }, true ) );

		[Fact]
		public void BiggerRightMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu }, false ) >= new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void CascadeGreaterThanMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 1u, 1u }, false ) >= new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeEqualMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 1u, 0u }, false ) >= new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeLessThanMixedNegativeRight() =>
			Assert.True( new Integer( new[] { 1u, 0u }, false ) >= new Integer( new[] { 1u, 1u }, true ) );
	}

	public class IntegerLessThanOrEqual {
		[Theory]
		[InlineData( "0", "1", true )]
		[InlineData( "1", "0", false )]
		[InlineData( "0", "-1", false )]
		[InlineData( "-1", "0", true )]
		[InlineData( "1", "1", true )]
		[InlineData( "-1", "1", true )]
		[InlineData( "1", "-1", false )]
		[InlineData( "-1", "-1", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Integer.Parse( l ) <= Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) <= new Integer( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu } ) <= new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void CascadeGreaterThan() =>
			Assert.False( new Integer( new[] { 1u, 1u } ) <= new Integer( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeEqual() =>
			Assert.True( new Integer( new[] { 1u, 0u } ) <= new Integer( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeLessThan() =>
			Assert.True( new Integer( new[] { 1u, 0u } ) <= new Integer( new[] { 1u, 1u } ) );

		[Fact]
		public void BiggerLeftNegative() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) <= new Integer( new[] { 0xDEADBEEFu }, true ) );

		[Fact]
		public void BiggerRightNegative() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu }, true ) <= new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void CascadeGreaterThanNegative() =>
			Assert.True( new Integer( new[] { 1u, 1u }, true ) <= new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeEqualNegative() =>
			Assert.True( new Integer( new[] { 1u, 0u }, true ) <= new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeLessThanNegative() =>
			Assert.False( new Integer( new[] { 1u, 0u }, true ) <= new Integer( new[] { 1u, 1u }, true ) );

		[Fact]
		public void BiggerLeftMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) <= new Integer( new[] { 0xDEADBEEFu }, false ) );

		[Fact]
		public void BiggerRightMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu }, true ) <= new Integer( new[] { 0xBADu, 0xDEADBEEFu }, false ) );

		[Fact]
		public void CascadeGreaterThanMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 1u, 1u }, true ) <= new Integer( new[] { 1u, 0u }, false ) );

		[Fact]
		public void CascadeEqualMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 1u, 0u }, true ) <= new Integer( new[] { 1u, 0u }, false ) );

		[Fact]
		public void CascadeLessThanMixedNegativeLeft() =>
			Assert.True( new Integer( new[] { 1u, 0u }, true ) <= new Integer( new[] { 1u, 1u }, false ) );

		[Fact]
		public void BiggerLeftMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 0xBADu, 0xDEADBEEFu }, false ) <= new Integer( new[] { 0xDEADBEEFu }, true ) );

		[Fact]
		public void BiggerRightMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 0xDEADBEEFu }, false ) <= new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ) );

		[Fact]
		public void CascadeGreaterThanMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 1u, 1u }, false ) <= new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeEqualMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 1u, 0u }, false ) <= new Integer( new[] { 1u, 0u }, true ) );

		[Fact]
		public void CascadeLessThanMixedNegativeRight() =>
			Assert.False( new Integer( new[] { 1u, 0u }, false ) <= new Integer( new[] { 1u, 1u }, true ) );
	}

	public class IntegerInequality {
		[Theory]
		[InlineData( "0", "0", false )]
		[InlineData( "0", "1", true )]
		[InlineData( "1", "0", true )]
		[InlineData( "1", "1", false )]
		[InlineData( "0", "-1", true )]
		[InlineData( "-1", "0", true )]
		[InlineData( "-1", "1", true )]
		[InlineData( "1", "-1", true )]
		[InlineData( "-1", "-1", false )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Integer.Parse( l ) != Integer.Parse( r ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.True( new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) != new Integer( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.True( new Integer( new[] { 0xDEADBEEFu } ) != new Integer( new[] { 0xBADu, 0xDEADBEEFu } ) );
	}

	public class IntegerAddition {
		[Theory]
		[InlineData( "1", "1", "2" )]        // Sanity
		[InlineData( "-1", "1", "0" )]        // Sanity
		[InlineData( "1", "-1", "0" )]        // Sanity
		[InlineData( "-1", "-1", "-2" )]        // Sanity
		[InlineData( "1", "0", "1" )]        // Sanity
		[InlineData( "-1", "0", "-1" )]        // Sanity
		[InlineData( "0", "1", "1" )]        // Sanity
		[InlineData( "0", "-1", "-1" )]        // Sanity
		[InlineData( "1", "5", "6" )]        // l < r
		[InlineData( "-1", "5", "4" )]        // l < r
		[InlineData( "1", "-5", "-4" )]        // l < r
		[InlineData( "-1", "-5", "-6" )]        // l < r
		[InlineData( "9", "2", "11" )]       // l > r
		[InlineData( "-9", "2", "-7" )]       // l > r
		[InlineData( "9", "-2", "7" )]       // l > r
		[InlineData( "-9", "-2", "-11" )]       // l > r
		public void Sanity( string l, string r, string s ) =>
			Assert.Equal( Integer.Parse( s ), Integer.Parse( l ) + Integer.Parse( r ) );

		[Fact]
		public void Overflow() =>
			Assert.Equal( new Integer( new[] { 1u, 2u } ), new Integer( new[] { uint.MaxValue } ) + new Integer( new[] { 3u } ) );

		[Fact]
		public void LeftBiggerNoOverflow() =>
			Assert.Equal( new Integer( new[] { 0xFu, 0xFF0Fu } ), new Integer( new[] { 0xFu, 0xFu } ) + new Integer( new[] { 0xFF00u } ) );

		[Fact]
		public void RightBiggerNoOverflow() =>
			Assert.Equal( new Integer( new[] { 0xFu, 0xFF0Fu } ), new Integer( new[] { 0xFF00u } ) + new Integer( new[] { 0xFu, 0xFu } ) );

		[Fact]
		public void CascadingOverflow() =>
			Assert.Equal( new Integer( new[] { 1u, 0u, 0u } ), new Integer( new[] { 1u } ) + new Integer( new[] { uint.MaxValue, uint.MaxValue } ) );

		[Fact]
		public void OverflowCausedOverflow() =>
			Assert.Equal( new Integer( new[] { 2u, 0u, 0u } ), new Integer( new[] { 1u, 1u } ) + new Integer( new[] { 1u, uint.MaxValue - 1u, uint.MaxValue } ) );

		[Fact]
		public void OverflowNegative() =>
			Assert.Equal( new Integer( new[] { 1u, 2u }, true ), new Integer( new[] { uint.MaxValue }, true ) + new Integer( new[] { 3u }, true ) );

		[Fact]
		public void LeftBiggerNoOverflowNegative() =>
			Assert.Equal( new Integer( new[] { 0xFu, 0xFF0Fu }, true ), new Integer( new[] { 0xFu, 0xFu }, true ) + new Integer( new[] { 0xFF00u }, true ) );

		[Fact]
		public void RightBiggerNoOverflowNegative() =>
			Assert.Equal( new Integer( new[] { 0xFu, 0xFF0Fu }, true ), new Integer( new[] { 0xFF00u }, true ) + new Integer( new[] { 0xFu, 0xFu }, true ) );

		[Fact]
		public void CascadingOverflowNegative() =>
			Assert.Equal( new Integer( new[] { 1u, 0u, 0u }, true ), new Integer( new[] { 1u }, true ) + new Integer( new[] { uint.MaxValue, uint.MaxValue }, true ) );

		[Fact]
		public void OverflowCausedOverflowNegative() =>
			Assert.Equal( new Integer( new[] { 2u, 0u, 0u }, true ), new Integer( new[] { 1u, 1u }, true ) + new Integer( new[] { 1u, uint.MaxValue - 1u, uint.MaxValue }, true ) );
	}

	public class IntegerSubtraction {
		[Theory]
		[InlineData( "1", "1", "0" )]                            // Sanity
		[InlineData( "-1", "1", "-2" )]                            // Sanity
		[InlineData( "1", "-1", "2" )]                            // Sanity
		[InlineData( "-1", "-1", "0" )]                            // Sanity
		[InlineData( "1", "0", "1" )]                            // Sanity
		[InlineData( "-1", "0", "-1" )]                            // Sanity
		[InlineData( "0", "1", "-1" )]                            // Sanity
		[InlineData( "0", "-1", "1" )]                            // Sanity
		[InlineData( "9", "2", "7" )]                            // l > r
		[InlineData( "-9", "2", "-11" )]                            // l > r
		[InlineData( "9", "-2", "11" )]                            // l > r
		[InlineData( "-9", "-2", "-7" )]                            // l > r
		[InlineData( "1", "5", "-4" )]                            // l < r
		[InlineData( "-1", "5", "-6" )]                            // l < r
		[InlineData( "1", "-5", "6" )]                            // l < r
		[InlineData( "-1", "-5", "4" )]                            // l < r
		public void Sanity( string l, string r, string s ) =>
			Assert.Equal( Integer.Parse( s ), Integer.Parse( l ) - Integer.Parse( r ) );

		[Fact]
		public void MultiItemNoUnderflow() =>
			Assert.Equal( new Integer( new[] { 2u, 2u } ), new Integer( new[] { 3u, 4u } ) - new Integer( new[] { 1u, 2u } ) );

		[Fact]
		public void MultiItemNegativeNoUnderflow() =>
			Assert.Equal( new Integer( new[] { 2u, 2u }, true ), new Integer( new[] { 3u, 4u }, true ) - new Integer( new[] { 1u, 2u }, true ) );

		[Fact]
		public void MultiItemMixedLeftNoUnderflow() =>
			Assert.Equal( new Integer( new[] { 4u, 6u }, true ), new Integer( new[] { 3u, 4u }, true ) - new Integer( new[] { 1u, 2u } ) );

		[Fact]
		public void MultiItemMixedRightNoUnderflow() =>
			Assert.Equal( new Integer( new[] { 4u, 6u } ), new Integer( new[] { 3u, 4u } ) - new Integer( new[] { 1u, 2u }, true ) );

		[Fact]
		public void MultiItemUnderflow() =>
			Assert.Equal( new Integer( new[] { 0x2u, 0xFFFFFFFFu } ), new Integer( new[] { 4u, 2u } ) - new Integer( new[] { 1u, 3u } ) );

		[Fact]
		public void MultiItemCascadingUnderflow() =>
			Assert.Equal( new Integer( new[] { 0xFFFFFFFFu, 0xFFFFFFFFu } ), new Integer( new[] { 1u, 0u, 0u } ) - new Integer( new[] { 1u } ) );

		[Fact]
		public void MultiItemNegativeOverflow() =>
			Assert.Equal( new Integer( new[] { 4u, 2u }, true ), new Integer( new[] { 0x2u, 0xFFFFFFFFu }, true ) - new Integer( new[] { 1u, 3u } ) );

		[Fact]
		public void MultiItemCascadingNegativeOverflow() =>
			Assert.Equal( new Integer( new[] { 1u, 0u, 0u }, true ), new Integer( new[] { 0xFFFFFFFFu, 0xFFFFFFFFu }, true ) - new Integer( new[] { 1u } ) );
	}

	public class IntegerMultiply {
		[Theory]
		[InlineData( "1", "1", "1" )]        // Sanity
		[InlineData( "-1", "1", "-1" )]        // Sanity
		[InlineData( "1", "-1", "-1" )]        // Sanity
		[InlineData( "-1", "-1", "1" )]        // Sanity
		[InlineData( "1", "0", "0" )]        // Sanity
		[InlineData( "-1", "0", "0" )]        // Sanity
		[InlineData( "0", "1", "0" )]        // Sanity
		[InlineData( "0", "-1", "0" )]        // Sanity
		[InlineData( "6", "7", "42" )]       // multiple bits
		[InlineData( "-6", "7", "-42" )]       // multiple bits
		[InlineData( "6", "-7", "-42" )]       // multiple bits
		[InlineData( "-6", "-7", "42" )]       // multiple bits
		public void Sanity( string l, string r, string p ) =>
			Assert.Equal( Integer.Parse( p ), Integer.Parse( l ) * Integer.Parse( r ) );

		[Fact]
		public void Big() =>
			Assert.Equal( new Integer( new[] { 0x75CD9046u, 0x541D5980u } ), new Integer( new[] { 0xFEDCBA98u } ) * new Integer( new[] { 0x76543210u } ) );

		[Fact]
		public void BigMixedLeft() =>
			Assert.Equal( new Integer( new[] { 0x75CD9046u, 0x541D5980u }, true ), new Integer( new[] { 0xFEDCBA98u }, true ) * new Integer( new[] { 0x76543210u } ) );

		[Fact]
		public void BigMixedRight() =>
			Assert.Equal( new Integer( new[] { 0x75CD9046u, 0x541D5980u }, true ), new Integer( new[] { 0xFEDCBA98u } ) * new Integer( new[] { 0x76543210u }, true ) );

		[Fact]
		public void BigNegative() =>
			Assert.Equal( new Integer( new[] { 0x75CD9046u, 0x541D5980u } ), new Integer( new[] { 0xFEDCBA98u }, true ) * new Integer( new[] { 0x76543210u }, true ) );
	}

	public class IntegerDivision {
		[Theory]
		[InlineData( "1", "1", "1" )]        // Sanity
		[InlineData( "-1", "1", "-1" )]        // Sanity
		[InlineData( "1", "-1", "-1" )]        // Sanity
		[InlineData( "-1", "-1", "1" )]        // Sanity
		[InlineData( "0", "1", "0" )]        // Sanity
		[InlineData( "0", "-1", "0" )]        // Sanity
		[InlineData( "44", "7", "6" )]       // multiple bits
		[InlineData( "-44", "7", "-6" )]       // multiple bits
		[InlineData( "44", "-7", "-6" )]       // multiple bits
		[InlineData( "-44", "-7", "6" )]       // multiple bits
		[InlineData( "52", "5", "10" )]      // rev
		[InlineData( "-52", "5", "-10" )]      // rev
		[InlineData( "52", "-5", "-10" )]      // rev
		[InlineData( "-52", "-5", "10" )]      // rev
		[InlineData( "52", "10", "5" )]      // rev
		[InlineData( "-52", "10", "-5" )]      // rev
		[InlineData( "52", "-10", "-5" )]      // rev
		[InlineData( "-52", "-10", "5" )]      // rev
		public void Sanity( string dividend, string divisor, string quotient ) =>
			Assert.Equal( Integer.Parse( quotient ), Integer.Parse( dividend ) / Integer.Parse( divisor ) );

		[Fact]
		public void Zero() =>
			Assert.Equal( new Integer( new[] { 0u } ), new Integer( new[] { 5u } ) / new Integer( new[] { 10u } ) );

		[Fact]
		public void DivideByZero() =>
			Assert.Throws<DivideByZeroException>( () => Integer.Unit / Integer.Zero );

		[Fact]
		public void Big() =>
			Assert.Equal( new Integer( new[] { 0xFEDCBA98u } ), new Integer( new[] { 0x75CD9046u, 0x6651AFF8u } ) / new Integer( new[] { 0x76543210u } ) );

		[Fact]
		public void BigMixedLeft() =>
			Assert.Equal( new Integer( new[] { 0xFEDCBA98u }, true ), new Integer( new[] { 0x75CD9046u, 0x6651AFF8u }, true ) / new Integer( new[] { 0x76543210u } ) );

		[Fact]
		public void BigMixedRight() =>
			Assert.Equal( new Integer( new[] { 0xFEDCBA98u }, true ), new Integer( new[] { 0x75CD9046u, 0x6651AFF8u } ) / new Integer( new[] { 0x76543210u }, true ) );

		[Fact]
		public void BigNegative() =>
			Assert.Equal( new Integer( new[] { 0xFEDCBA98u } ), new Integer( new[] { 0x75CD9046u, 0x6651AFF8u }, true ) / new Integer( new[] { 0x76543210u }, true ) );
	}

	public class IntegerModulo {
		[Theory]
		[InlineData( "1", "1", "0" )]        // Sanity
		[InlineData( "-1", "1", "0" )]        // Sanity
		[InlineData( "1", "-1", "0" )]        // Sanity
		[InlineData( "-1", "-1", "0" )]        // Sanity
		[InlineData( "0", "1", "0" )]        // Sanity
		[InlineData( "0", "-1", "0" )]        // Sanity
		[InlineData( "44", "7", "2" )]       // multiple bits
		[InlineData( "-44", "7", "-2" )]       // multiple bits
		[InlineData( "44", "-7", "2" )]       // multiple bits
		[InlineData( "-44", "-7", "-2" )]       // multiple bits
		[InlineData( "52", "5", "2" )]      // rev
		[InlineData( "-52", "5", "-2" )]      // rev
		[InlineData( "52", "-5", "2" )]      // rev
		[InlineData( "-52", "-5", "-2" )]      // rev
		[InlineData( "52", "10", "2" )]      // rev
		[InlineData( "-52", "10", "-2" )]      // rev
		[InlineData( "52", "-10", "2" )]      // rev
		[InlineData( "-52", "-10", "-2" )]      // rev
		public void Sanity( string dividend, string divisor, string remainder ) =>
			Assert.Equal( Integer.Parse( remainder ), Integer.Parse( dividend ) % Integer.Parse( divisor ) );

		[Fact]
		public void Zero() =>
			Assert.Equal( new Integer( new[] { 0u } ), new Integer( new[] { 20u } ) % new Integer( new[] { 10u } ) );

		[Fact]
		public void DivideByZero() =>
			Assert.Throws<DivideByZeroException>( () => Integer.Unit % Integer.Zero );

		[Fact]
		public void Big() =>
			Assert.Equal( new Integer( new[] { 0x12345678u } ), new Integer( new[] { 0x75CD9046u, 0x6651AFF8u } ) % new Integer( new[] { 0x76543210u } ) );

		[Fact]
		public void BigMixedLeft() =>
			Assert.Equal( new Integer( new[] { 0x12345678u }, true ), new Integer( new[] { 0x75CD9046u, 0x6651AFF8u }, true ) % new Integer( new[] { 0x76543210u } ) );

		[Fact]
		public void BigMixedRight() =>
			Assert.Equal( new Integer( new[] { 0x12345678u } ), new Integer( new[] { 0x75CD9046u, 0x6651AFF8u } ) % new Integer( new[] { 0x76543210u }, true ) );

		[Fact]
		public void BigNegative() =>
			Assert.Equal( new Integer( new[] { 0x12345678u }, true ), new Integer( new[] { 0x75CD9046u, 0x6651AFF8u }, true ) % new Integer( new[] { 0x76543210u }, true ) );
	}

	public class IntegerNegation {
		[Theory]
		[InlineData( "1", "-1" )]                            // Sanity
		[InlineData( "-1", "1" )]                            // Sanity
		[InlineData( "0", "0" )]                            // Sanity
		public void Sanity( string a, string e ) =>
			Assert.Equal( Integer.Parse( e ), -Integer.Parse( a ) );

		[Fact]
		public void Big() =>
			Assert.Equal( new Integer( new[] { 0xFEDCBA9u, 0x76543210u }, true ), -new Integer( new[] { 0xFEDCBA9u, 0x76543210u } ) );

		[Fact]
		public void BigNegative() =>
			Assert.Equal( new Integer( new[] { 0xFEDCBA9u, 0x76543210u } ), -new Integer( new[] { 0xFEDCBA9u, 0x76543210u }, true ) );
	}

	public class IntegerToString {
		[Theory]
		[InlineData( 0u, false, "0" )]          // Sanity
		[InlineData( 1u, false, "1" )]          // Sanity
		[InlineData( 123u, false, "123" )]      // multiple bits
		[InlineData( 45678u, false, "45678" )]  // rev
		[InlineData( 1u, true, "-1" )]          // Sanity
		[InlineData( 123u, true, "-123" )]      // multiple bits
		[InlineData( 45678u, true, "-45678" )]  // rev
		public void Sanity( uint data, bool negative, string expected ) =>
			Assert.Equal( expected, new Integer( new[] { data }, negative ).ToString() );

		[Fact]
		public void Bigger() =>
			Assert.Equal( "1234567890123456789", new Integer( new[] { 0x112210F4u, 0x7DE98115u } ).ToString() );

		[Fact]
		public void BiggerNegative() =>
			Assert.Equal( "-1234567890123456789", new Integer( new[] { 0x112210F4u, 0x7DE98115u }, true ).ToString() );
	}

	public class IntegerParse {
		[Theory]
		[InlineData( "0", 0u, false )]          // Sanity
		[InlineData( "1", 1u, false )]          // Sanity
		[InlineData( "123", 123u, false )]      // multiple bits
		[InlineData( "45678", 45678u, false )]  // rev
		[InlineData( "-0", 0u, false )]          // Sanity
		[InlineData( "-1", 1u, true )]          // Sanity
		[InlineData( "-123", 123u, true )]      // multiple bits
		[InlineData( "-45678", 45678u, true )]  // rev
		public void Sanity( string str, uint data, bool negative ) =>
			Assert.Equal( new Integer( new[] { data }, negative ), Integer.Parse( str ) );

		[Fact]
		public void Bigger() =>
			Assert.Equal( new Integer( new[] { 0x112210F4u, 0x7DE98115u } ), Integer.Parse( "1234567890123456789" ) );

		[Fact]
		public void BiggerNegative() =>
			Assert.Equal( new Integer( new[] { 0x112210F4u, 0x7DE98115u }, true ), Integer.Parse( "-1234567890123456789" ) );
	}
}
