﻿using System;
using Microsoft.FSharp.Core;
using Xunit;
using Freestylecoding.Math;

namespace Freestylecoding.Math.CSharp.Tests {

	// NOTE: The sanity tests are here to do low level tests of a few parts
	// This lets me isolate those tests so I can use those parts in the rest of the tests
	//
	// In other words:
	// IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
	[Trait( "Type", "Sanity" )]
	public class Sanity {
		[Fact]
		public void LeftShift() =>
			Assert.Equal( new Natural( new[] { 4u, 0x8000_0000u, 0u } ), new Natural( new[] { 9u } ) << 63 );

		[Fact]
		public void RightShift() =>
			Assert.Equal( new Natural( new[] { 9u } ), new Natural( new[] { 4u, 0x8000_0000u, 0u } ) >> 63 );

		[Fact]
		public void GreaterThanTrue() =>
			Assert.True( new Natural( new[] { 0xDEADBEEFu, 0xBADu } ) > new Natural( new[] { 0xBADu, 0xDEADBEEFu, } ) );

		[Fact]
		public void GreaterThanFalseByLessThan() =>
			Assert.False( new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) > new Natural( new[] { 0xDEADBEEFu, 0xBADu } ) );

		[Fact]
		public void GreaterThanFalseByEquals() =>
			Assert.False( new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) > new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void Addition() =>
			Assert.Equal( new Natural( new[] { 2u, 0u, 0u } ), new Natural( new[] { 1u, 1u } ) + new Natural( new[] { 1u, uint.MaxValue - 1u, uint.MaxValue } ) );

		[Fact]
		public void Subtraction() =>
			Assert.Equal( new Natural( new[] { 0xFFFF_FFFFu, 0xFFFF_FFFFu } ), new Natural( new[] { 1u, 0u, 0u } ) - new Natural( new[] { 1u } ) );

		[Fact]
		public void Multiplication() =>
			Assert.Equal( new Natural( new[] { 0x75CD9046u, 0x541D5980u } ), new Natural( new[] { 0xFEDCBA98u } ) * new Natural( new[] { 0x76543210u } ) );

		[Fact]
		public void ToString_() =>
			Assert.Equal( "1234567890123456789", new Natural( new[] { 0x112210F4u, 0x7DE98115u } ).ToString() );

		[Fact]
		public void Parse() =>
			Assert.Equal( new Natural( new[] { 0x112210F4u, 0x7DE98115u } ), Natural.Parse( "1234567890123456789" ) );
	}

	public class And {
		[Theory]
		[InlineData( 0u, 0u, 0u )]
		[InlineData( 1u, 0u, 0u )]
		[InlineData( 0u, 1u, 0u )]
		[InlineData( 1u, 1u, 1u )]
		public void Sanity( uint l, uint r, uint x ) =>
			Assert.Equal( new Natural( new[] { x } ), new Natural( new[] { l } ) & new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.Equal( Natural.Unit, new Natural( new[] { 0xFu, 0x00000101u } ) & new Natural( new[] { 0x00010001u } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.Equal( Natural.Unit, new Natural( new[] { 0x00010001u } ) & new Natural( new[] { 0xFu, 0x00000101u } ) );
	}

	public class Or {
		[Theory]
		[InlineData( 0u, 0u, 0u )]
		[InlineData( 1u, 0u, 1u )]
		[InlineData( 0u, 1u, 1u )]
		[InlineData( 1u, 1u, 1u )]
		public void Sanity( uint l, uint r, uint x ) =>
			Assert.Equal( new Natural( new[] { x } ), new Natural( new[] { l } ) | new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.Equal( new Natural( new[] { 0xFu, 0x10101u } ), new Natural( new[] { 0xFu, 0x00000101u } ) | new Natural( new[] { 0x00010001u } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.Equal( new Natural( new[] { 0xFu, 0x10101u } ), new Natural( new[] { 0x00010001u } ) | new Natural( new[] { 0xFu, 0x00000101u } ) );
	}

	public class Xor {
		[Theory]
		[InlineData( 0u, 0u, 0u )]
		[InlineData( 1u, 0u, 1u )]
		[InlineData( 0u, 1u, 1u )]
		[InlineData( 1u, 1u, 0u )]
		public void Sanity( uint l, uint r, uint x ) =>
			Assert.Equal( new Natural( new[] { x } ), new Natural( new[] { l } ) ^ new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.Equal( new Natural( new[] { 0xFu, 0x10100u } ), new Natural( new[] { 0xFu, 0x00000101u } ) ^ new Natural( new[] { 0x00010001u } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.Equal( new Natural( new[] { 0xFu, 0x10100u } ), new Natural( new[] { 0x00010001u } ) ^ new Natural( new[] { 0xFu, 0x00000101u } ) );
	}

	// C# only allows this for certain types
	// Need to loop back later and figure out how to convince it

	//public class BitwiseNot {
	//	[Theory]
	//	[InlineData( 0xFFFFFFFEu, 1u )]
	//	[InlineData( 1u, 0xFFFFFFFEu )]
	//	public void Sanity( uint l, uint x ) {
	//		Natural tmp = new Natural( new[] { l } );
	//		Assert.Equal( new Natural( new[] { x } ), ~tmp );
	//	}

	//	[Fact]
	//	public void Bigger() =>
	//		Assert.Equal( new Natural( new[] { 0xF0123456u, 0x789ABCDEu } ), ~new Natural( new[] { 0x0FEDCBA9u, 0x87654321u } ) );
	//}

	public class LeftShift {
		[Theory]
		[InlineData( 1u, 1, 2u )]        // Sanity
		[InlineData( 0xFu, 2, 0x3Cu )]   // multiple bits
		public void Sanity( uint l, int r, uint s ) =>
			Assert.Equal( new Natural( new[] { s } ), new Natural( new[] { l } ) << r );

		[Fact]
		public void Overflow() =>
			Assert.Equal( new Natural( new[] { 1u, 0xFFFFFFFEu } ), new Natural( new[] { 0xFFFFFFFFu } ) << 1 );

		[Fact]
		public void MultipleOverflow() =>
			Assert.Equal( new Natural( new[] { 0x5u, 0xFFFFFFF8u } ), new Natural( new[] { 0xBFFFFFFFu } ) << 3 );

		[Fact]
		public void OverOneUInt() =>
			Assert.Equal( new Natural( new[] { 8u, 0u, 0u } ), Natural.Unit << 67 );
	}

	public class RightShift {
		[Theory]
		[InlineData( 1u, 1, 0u )]        // Sanity
		[InlineData( 0xFu, 2, 0x3u )]    // multiple bits
		[InlineData( 0x3Cu, 2, 0xFu )]   // multiple bits
		public void Sanity( uint l, int r, uint s ) =>
			Assert.Equal( new Natural( new[] { s } ), new Natural( new[] { l } ) >> r );

		[Fact]
		public void Underflow() =>
			Assert.Equal( new Natural( new[] { 0x7FFFFFFFu } ), new Natural( new[] { 0xFFFFFFFFu } ) >> 1 );

		[Fact]
		public void MultipleUnderflow() =>
			Assert.Equal( new Natural( new[] { 0x1u, 0x5FFFFFFFu } ), new Natural( new[] { 0xAu, 0xFFFFFFFFu } ) >> 3 );

		[Fact]
		public void OverOneUInt() =>
			Assert.Equal( Natural.Unit, new Natural( new[] { 0x10u, 0u, 0u } ) >> 68 );

		[Fact]
		public void ReduceToZero() =>
			Assert.Equal( new Natural( new[] { 0u } ), new Natural( new[] { 0x10u, 0u, 0u } ) >> 99 );
	}

	public class Equality {
		[Theory]
		[InlineData( 0u, 0u, true )]
		[InlineData( 0u, 1u, false )]
		[InlineData( 1u, 0u, false )]
		[InlineData( 1u, 1u, true )]
		public void Sanity( uint l, uint r, bool x ) =>
			Assert.Equal( x, new Natural( new[] { l } ) == new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.False( new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) == new Natural( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.False( new Natural( new[] { 0xDEADBEEFu } ) == new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) );
	}

	public class GreaterThan {
		[Theory]
		[InlineData( 0u, 1u, false )]
		[InlineData( 1u, 0u, true )]
		[InlineData( 1u, 1u, false )]
		public void Sanity( uint l, uint r, bool x ) =>
			Assert.Equal( x, new Natural( new[] { l } ) > new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.True( new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) > new Natural( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.False( new Natural( new[] { 0xDEADBEEFu } ) > new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void CascadeGreaterThan() =>
			Assert.True( new Natural( new[] { 1u, 1u } ) > new Natural( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeEqual() =>
			Assert.False( new Natural( new[] { 1u, 0u } ) > new Natural( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeLessThan() =>
			Assert.False( new Natural( new[] { 1u, 0u } ) > new Natural( new[] { 1u, 1u } ) );
	}

	public class LessThan {
		[Theory]
		[InlineData( 0u, 1u, true )]
		[InlineData( 1u, 0u, false )]
		[InlineData( 1u, 1u, false )]
		public void Sanity( uint l, uint r, bool x ) =>
			Assert.Equal( x, new Natural( new[] { l } ) < new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.False( new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) < new Natural( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.True( new Natural( new[] { 0xDEADBEEFu } ) < new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void CascadeGreaterThan() =>
			Assert.False( new Natural( new[] { 1u, 1u } ) < new Natural( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeEqual() =>
			Assert.False( new Natural( new[] { 1u, 0u } ) < new Natural( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeLessThan() =>
			Assert.True( new Natural( new[] { 1u, 0u } ) < new Natural( new[] { 1u, 1u } ) );
	}

	public class GreaterThanOrEqual {
		[Theory]
		[InlineData( 0u, 1u, false )]
		[InlineData( 1u, 0u, true )]
		[InlineData( 1u, 1u, true )]
		public void Sanity( uint l, uint r, bool x ) =>
			Assert.Equal( x, new Natural( new[] { l } ) >= new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.True( new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) >= new Natural( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.False( new Natural( new[] { 0xDEADBEEFu } ) >= new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void CascadeGreaterThan() =>
			Assert.True( new Natural( new[] { 1u, 1u } ) >= new Natural( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeEqual() =>
			Assert.True( new Natural( new[] { 1u, 0u } ) >= new Natural( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeLessThan() =>
			Assert.False( new Natural( new[] { 1u, 0u } ) >= new Natural( new[] { 1u, 1u } ) );
	}

	public class LessThanOrEqual {
		[Theory]
		[InlineData( 0u, 1u, true )]
		[InlineData( 1u, 0u, false )]
		[InlineData( 1u, 1u, true )]
		public void Sanity( uint l, uint r, bool x ) =>
			Assert.Equal( x, new Natural( new[] { l } ) <= new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.False( new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) <= new Natural( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.True( new Natural( new[] { 0xDEADBEEFu } ) <= new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) );

		[Fact]
		public void CascadeGreaterThan() =>
			Assert.False( new Natural( new[] { 1u, 1u } ) <= new Natural( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeEqual() =>
			Assert.True( new Natural( new[] { 1u, 0u } ) <= new Natural( new[] { 1u, 0u } ) );

		[Fact]
		public void CascadeLessThan() =>
			Assert.True( new Natural( new[] { 1u, 0u } ) <= new Natural( new[] { 1u, 1u } ) );
	}

	public class Inequality {
		[Theory]
		[InlineData( 0u, 0u, false )]
		[InlineData( 0u, 1u, true )]
		[InlineData( 1u, 0u, true )]
		[InlineData( 1u, 1u, false )]
		public void Sanity( uint l, uint r, bool x ) =>
			Assert.Equal( x, new Natural( new[] { l } ) != new Natural( new[] { r } ) );

		[Fact]
		public void BiggerLeft() =>
			Assert.True( new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) != new Natural( new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRight() =>
			Assert.True( new Natural( new[] { 0xDEADBEEFu } ) != new Natural( new[] { 0xBADu, 0xDEADBEEFu } ) );
	}

	public class Addition {
		[Theory]
		[InlineData( 1u, 1u, 2u )]        // Sanity
		[InlineData( 1u, 0u, 1u )]        // Sanity
		[InlineData( 0u, 1u, 1u )]        // Sanity
		[InlineData( 1u, 5u, 6u )]        // l < r
		[InlineData( 9u, 2u, 11u )]       // l > r
		public void Sanity( uint l, uint r, uint s ) {
			Natural left = new Natural( new[] { l });
			Natural right = new Natural( new []{ r });
			Assert.Equal( new Natural( new[] { s } ), left + right );
		}

		[Fact]
		public void Overflow() {
			Natural l = new Natural( new[] { uint.MaxValue });
			Natural r = new Natural( new[] { 3u });
			Assert.Equal( new Natural( new[] { 1u, 2u } ), l + r );
		}

		[Fact]
		public void LeftBiggerNoOverflow() {
			Natural l = new Natural( new[] {0xFu,0xFu});
			Natural r = new Natural( new[] {0xFF00u});
			Assert.Equal( new Natural( new[] { 0xFu, 0xFF0Fu } ), l + r );
		}

		[Fact]
		public void RightBiggerNoOverflow() {
			Natural l = new Natural( new[] { 0xFF00u });
			Natural r = new Natural( new[] {0xFu,0xFu});
			Assert.Equal( new Natural( new[] { 0xFu, 0xFF0Fu } ), l + r );
		}

		[Fact]
		public void CascadingOverflow() {
			Natural l = new Natural( new[] { 1u });
			Natural r = new Natural( new[] {uint.MaxValue,uint.MaxValue});
			Assert.Equal( new Natural( new[] { 1u, 0u, 0u } ), l + r );
		}

		[Fact]
		public void OverflowCausedOverflow() {
			Natural l = new Natural( new[] { 1u, 1u } );
			Natural r = new Natural( new[] { 1u, uint.MaxValue - 1u, uint.MaxValue });
			Assert.Equal( new Natural( new[] { 2u, 0u, 0u } ), l + r );
		}
	}

	public class Subtraction {
		[Theory]
		[InlineData( 1u, 1u, 0u )]                            // Sanity
		[InlineData( 1u, 0u, 1u )]                            // Sanity
		[InlineData( 9u, 2u, 7u )]                            // l > r
		public void Sanity( uint l, uint r, uint s ) {
			Natural left = new Natural( new[] { l });
			Natural right = new Natural( new[] {r});
			Assert.Equal( new Natural( new[] { s } ), left - right );
		}

		[Fact]
		public void SingleItemBadUnderflow() {
			Natural l = new Natural( new[] { 0u });
			Natural r = new Natural( new[] {1u});
			Assert.Equal( new Natural( new[] { 0xFFFFFFFFu, 0xFFFFFFFFu } ), l - r );
		}
		[Fact]
		public void MultiItemNoUnderflow() {
			Natural l = new Natural( new[] {3u, 4u});
			Natural r = new Natural( new[] {1u, 2u});
			Assert.Equal( new Natural( new[] { 2u, 2u } ), l - r );
		}
		[Fact]
		public void MultiItemSafeUnderflow() {
			Natural l = new Natural( new [] { 4u, 2u } );
			Natural r = new Natural( new[] {1u, 3u});
			Assert.Equal( new Natural( new[] { 0x2u, 0xFFFFFFFFu } ), l - r );
		}
		[Fact]
		public void MultiItemSafeCascadingUnderflow() {
			Natural l = new Natural( new[] { 1u, 0u, 0u } );
			Natural r = new Natural( new[] {1u});

			Assert.Equal( new Natural( new[] { 0xFFFFFFFFu, 0xFFFFFFFFu } ), l - r );
		}

		[Fact]
		public void MultiItemUnsafeUnderflow() {
			Natural l = new Natural( new[] { 1u, 2u } );
			Natural r = new Natural( new[] {1u, 3u});
			Assert.Equal( new Natural( new[] { 0xFFFFFFFFu, 0xFFFFFFFFu, 0xFFFFFFFFu } ), l - r );
		}
	}

	public class Multiply {
		[Theory]
		[InlineData( 1u, 1u, 1u )]        // Sanity
		[InlineData( 1u, 0u, 0u )]        // Sanity
		[InlineData( 0u, 1u, 0u )]        // Sanity
		[InlineData( 6u, 7u, 42u )]       // multiple bits
		public void Sanity( uint l, uint r, uint p ) =>
			Assert.Equal( new Natural( new[] { p } ), new Natural( new[] { l } ) * new Natural( new[] { r } ) );

		[Fact]
		public void Big() =>
			Assert.Equal( new Natural( new[] { 0x75CD9046u, 0x541D5980u } ), new Natural( new[] { 0xFEDCBA98u } ) * new Natural( new[] { 0x76543210u } ) );
	}

	public class Division {
		[Theory]
		[InlineData( 1u, 1u, 1u )]        // Sanity
		[InlineData( 0u, 1u, 0u )]        // Sanity
		[InlineData( 42u, 7u, 6u )]       // multiple bits
		[InlineData( 50u, 5u, 10u )]      // rev
		[InlineData( 50u, 10u, 5u )]      // rev
		[InlineData( 54u, 5u, 10u )]      // has remainder
		public void Sanity( uint dividend, uint divisor, uint quotient ) =>
			Assert.Equal( new Natural( new[] { quotient } ), new Natural( new[] { dividend } ) / new Natural( new[] { divisor } ) );

		[Fact]
		public void Zero() =>
			Assert.Equal( new Natural( new[] { 0u } ), new Natural( new[] { 5u } ) / new Natural( new[] { 10u } ) );

		[Fact]
		public void DivideByZero() =>
				Assert.IsType<DivideByZeroException>( Record.Exception( () => Natural.Unit / Natural.Zero ) );

		[Fact]

		public void Big() =>
			Assert.Equal( new Natural( new[] { 0xFEDCBA98u } ), new Natural( new[] { 0x75CD9046u, 0x541D5980u } ) / new Natural( new[] { 0x76543210u } ) );
	}
	public class Modulo {
		[Theory]
		[InlineData( 1u, 1u, 0u )]        // Sanity
		[InlineData( 0u, 1u, 0u )]        // Sanity
		[InlineData( 44u, 7u, 2u )]       // multiple bits
		[InlineData( 52u, 5u, 2u )]       // rev
		[InlineData( 52u, 10u, 2u )]      // rev
		public void Sanity( uint dividend, uint divisor, uint remainder ) =>
			Assert.Equal( new Natural( new[] { remainder } ), new Natural( new[] { dividend } ) % new Natural( new[] { divisor } ) );

		[Fact]
		public void Zero() =>
			Assert.Equal( new Natural( new[] { 0u } ), new Natural( new[] { 20u } ) % new Natural( new[] { 10u } ) );

		[Fact]
		public void DivideByZero() =>
			Assert.IsType<DivideByZeroException>( Record.Exception( () => Natural.Unit % Natural.Zero ) );

		[Fact]


		public void Big() =>
			Assert.Equal( new Natural( new[] { 0x12345678u } ), new Natural( new[] { 0x75CD9046u, 0x6651AFF8u } ) % new Natural( new[] { 0x76543210u } ) );
	}

	public class ToString {
		[Theory]
		[InlineData( 0u, "0" )]          // Sanity
		[InlineData( 1u, "1" )]          // Sanity
		[InlineData( 123u, "123" )]      // multiple bits
		[InlineData( 45678u, "45678" )]  // rev
		public void Sanity( uint n, string s ) =>
			Assert.Equal( s, new Natural( new[] { n } ).ToString() );

		[Fact]
		public void Bigger() =>
			Assert.Equal( "1234567890123456789", new Natural( new[] { 0x112210F4u, 0x7DE98115u } ).ToString() );
	}
	public class Parse {
		[Theory]
		[InlineData( 0u, "0" )]          // Sanity
		[InlineData( 1u, "1" )]          // Sanity
		[InlineData( 123u, "123" )]      // multiple bits
		[InlineData( 45678u, "45678" )]  // rev
		public void Sanity( uint n, string s ) =>
			Assert.Equal( new Natural( new[] { n } ), Natural.Parse( s ) );

		[Fact]
		public void Bigger() =>
			Assert.Equal( new Natural( new[] { 0x112210F4u, 0x7DE98115u } ), Natural.Parse( "1234567890123456789" ) );
	}
}