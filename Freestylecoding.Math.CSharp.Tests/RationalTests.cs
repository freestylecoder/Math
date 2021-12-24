using System;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests {
	[Trait( "Type", "Sanity" )]
	public class RationalSanity {
		[Fact]
		public void GreaterThanTrue() =>
			Assert.True(
				new Rational(
					new Integer( new[] { 0xBADu, 0xDEADBEEFu } ),
					new Natural( new[] { 2u } )
				) > new Rational(
					new Integer( new[] { 0xDEADBEEFu, 0xBADu }, true ),
					new Natural( new[] { 2u } )
				)
			);

		[Fact]
		public void GreaterThanFalseByLessThan() =>
			Assert.False(
				new Rational(
					new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ),
					new Natural( new[] { 2u } )
				) > new Rational(
					new Integer( new[] { 0xDEADBEEFu, 0xBADu } ),
					new Natural( new[] { 2u } )
				)
			);

		[Fact]
		public void GreaterThanFalseByEquals() =>
			Assert.False(
				new Rational(
					new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ),
					new Natural( new[] { 2u } )
				) > new Rational(
					new Integer( new[] { 0xBADu, 0xDEADBEEFu }, true ),
					new Natural( new[] { 2u } )
				)
			);

		[Fact]
		public void Addition() =>
			Assert.Equal(
				new Rational( new Integer( 5 ), new Natural( 4u ) ),
				new Rational( Integer.Unit, new Natural( 2u ) ) + new Rational( new Integer( 3 ), new Natural( 4u ) )
			);

		[Fact]
		public void Subtraction() =>
			Assert.Equal(
				new Rational( Integer.Unit, new Natural( 4u ) ),
				new Rational( new Integer( 3 ), new Natural( 4u ) ) - new Rational( Integer.Unit, new Natural( 2u ) )
			);

		[Fact]
		public void Multiplication() =>
			Assert.Equal(
				new Rational( new Integer( 3 ), new Natural( 8u ) ),
				new Rational( Integer.Unit, new Natural( 2u ) ) * new Rational( new Integer( 3 ), new Natural( 4u ) )
			);

		[Fact]
		public void DivisionModulo() =>
			Assert.Equal(
				new Tuple<Integer, Rational>(
					Integer.Unit,
					new Rational( Integer.Unit, new Natural( 2u ) )
				),
				Rational.op_DividePercent(
					new Rational( new Integer( 3 ), new Natural( 4u ) ),
					new Rational( Integer.Unit, new Natural( 2u ) )
				)
			);

		[Fact]
		public void ToStringSanity() =>
			Assert.Equal( "-5 / 7", new Rational( new Integer( -5 ), new Natural( 7u ) ).ToString() );

		[Fact]
		public void Parse() =>
			Assert.Equal(
				new Rational( new Integer( new[] { 0x112210F4u, 0x7DE98115u }, true ), new Natural( 41u ) ),
				Rational.Parse( "-1234567890123456789 / 41" )
			);
	}

	public class RationalCtor {
		[Theory]
		[InlineData( " 0", "1", " 0" )]
		[InlineData( " 1", "1", " 1" )]
		[InlineData( "-1", "1", "-1" )]
		[InlineData( " 1", "2", " 1/2" )]
		[InlineData( "-1", "2", "-1/2" )]
		[InlineData( " 2", "4", " 1/2" )]
		[InlineData( "-2", "4", "-1/2" )]
		[InlineData( " 6", "15", " 2/5" )]
		[InlineData( "-6", "15", "-2/5" )]
		public void Sanity( string n, string d, string r ) =>
			Assert.Equal(
				Rational.Parse( r ),
				new Rational( Integer.Parse( n ), Natural.Parse( d ) )
			);

		[Fact]
		public void DivideByZero() =>
			Assert.IsType<System.DivideByZeroException>(
				Record.Exception( () => new Rational( Integer.Unit, Natural.Zero ) )
			);
	}

	public class RationalEquality {
		[Theory]
		[InlineData( 0, 0, true )]
		[InlineData( 0, 1, false )]
		[InlineData( 1, 0, false )]
		[InlineData( 1, 1, true )]
		[InlineData( 0, -1, false )]
		[InlineData( -1, 0, false )]
		[InlineData( -1, 1, false )]
		[InlineData( 1, -1, false )]
		[InlineData( -1, -1, true )]
		public void Sanity( int l, int r, bool e ) =>
			Assert.Equal( e, new Rational( new Integer( l ), Natural.Unit ) == new Rational( new Integer( r ), Natural.Unit ) );

		[Fact]
		public void Big() =>
			Assert.True(
				new Rational( new[] { 0x4u, 0x5u, 0x6u }, new[] { 0x1u, 0x2u, 0x3u } )
				==
				new Rational( new[] { 0x4u, 0x5u, 0x6u }, new[] { 0x1u, 0x2u, 0x3u } )
			);

		[Fact]
		public void BigNegative() =>
			Assert.True(
				new Rational( new Integer( new[] { 0x4u, 0x5u, 0x6u }, true ), new Natural( new[] { 0x1u, 0x2u, 0x3u } ) )
				==
				new Rational( new Integer( new[] { 0x4u, 0x5u, 0x6u }, true ), new Natural( new[] { 0x1u, 0x2u, 0x3u } ) )
			);

		[Fact]
		public void BiggerLeftNumerator() =>
			Assert.False(
				new Rational( new[] { 0xBADu, 0xDEADBEEFu }, new[] { 20u } )
				==
				new Rational( new[] { 0xDEADBEEFu }, new[] { 20u } ) );

		[Fact]
		public void BiggerRightNumerator() =>
			Assert.False( new Rational( new[] { 0xDEADBEEFu }, new[] { 20u } ) == new Rational( new[] { 0xBADu, 0xDEADBEEFu }, new[] { 20u } ) );

		[Fact]
		public void BiggerLeftDenominator() =>
		Assert.False( new Rational( new[] { 20u }, new[] { 0xBADu, 0xDEADBEEFu } ) == new Rational( new[] { 20u }, new[] { 0xDEADBEEFu } ) );

		[Fact]
		public void BiggerRightDenominator() =>
			Assert.False(
				new Rational( new[] { 20u }, new[] { 0xDEADBEEFu } )
				==
				new Rational( new[] { 20u }, new[] { 0xBADu, 0xDEADBEEFu } )
			);
	}

	public class RationalGreaterThan {
		[Theory]
		[InlineData( " 0", " 1", false )]
		[InlineData( " 1", " 0", true )]
		[InlineData( " 0", "-1", true )]
		[InlineData( "-1", " 0", false )]
		[InlineData( " 1", " 1", false )]
		[InlineData( "-1", " 1", false )]
		[InlineData( " 1", "-1", true )]
		[InlineData( "-1", "-1", false )]
		[InlineData( " 1/2", " 1/2", false )]
		[InlineData( "-1/2", " 1/2", false )]
		[InlineData( " 1/2", "-1/2", true )]
		[InlineData( "-1/2", "-1/2", false )]
		[InlineData( " 1/3", " 1/2", false )]
		[InlineData( "-1/3", " 1/2", false )]
		[InlineData( " 1/3", "-1/2", true )]
		[InlineData( "-1/3", "-1/2", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Rational.Parse( l ) > Rational.Parse( r ) );
	}

	public class RationalLessThan {
		[Theory]
		[InlineData( " 0", " 1", true )]
		[InlineData( " 1", " 0", false )]
		[InlineData( " 0", "-1", false )]
		[InlineData( "-1", " 0", true )]
		[InlineData( " 1", " 1", false )]
		[InlineData( "-1", " 1", true )]
		[InlineData( " 1", "-1", false )]
		[InlineData( "-1", "-1", false )]
		[InlineData( " 1/2", " 1/2", false )]
		[InlineData( "-1/2", " 1/2", true )]
		[InlineData( " 1/2", "-1/2", false )]
		[InlineData( "-1/2", "-1/2", false )]
		[InlineData( " 1/3", " 1/2", true )]
		[InlineData( "-1/3", " 1/2", true )]
		[InlineData( " 1/3", "-1/2", false )]
		[InlineData( "-1/3", "-1/2", false )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Rational.Parse( l ) < Rational.Parse( r ) );
	}

	public class RationalGreaterThanOrEqual {
		[Theory]
		[InlineData( " 0", " 1", false )]
		[InlineData( " 1", " 0", true )]
		[InlineData( " 0", "-1", true )]
		[InlineData( "-1", " 0", false )]
		[InlineData( " 1", " 1", true )]
		[InlineData( "-1", " 1", false )]
		[InlineData( " 1", "-1", true )]
		[InlineData( "-1", "-1", true )]
		[InlineData( " 1/2", " 1/2", true )]
		[InlineData( "-1/2", " 1/2", false )]
		[InlineData( " 1/2", "-1/2", true )]
		[InlineData( "-1/2", "-1/2", true )]
		[InlineData( " 1/3", " 1/2", false )]
		[InlineData( "-1/3", " 1/2", false )]
		[InlineData( " 1/3", "-1/2", true )]
		[InlineData( "-1/3", "-1/2", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Rational.Parse( l ) >= Rational.Parse( r ) );
	}

	public class RationalLessThanOrEqual {
		[Theory]
		[InlineData( " 0", " 1", true )]
		[InlineData( " 1", " 0", false )]
		[InlineData( " 0", "-1", false )]
		[InlineData( "-1", " 0", true )]
		[InlineData( " 1", " 1", true )]
		[InlineData( "-1", " 1", true )]
		[InlineData( " 1", "-1", false )]
		[InlineData( "-1", "-1", true )]
		[InlineData( " 1/2", " 1/2", true )]
		[InlineData( "-1/2", " 1/2", true )]
		[InlineData( " 1/2", "-1/2", false )]
		[InlineData( "-1/2", "-1/2", true )]
		[InlineData( " 1/3", " 1/2", true )]
		[InlineData( "-1/3", " 1/2", true )]
		[InlineData( " 1/3", "-1/2", false )]
		[InlineData( "-1/3", "-1/2", false )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Rational.Parse( l ) <= Rational.Parse( r ) );
	}

	public class RationalInequality {
		[Theory]
		[InlineData( " 0", " 1", true )]
		[InlineData( " 1", " 0", true )]
		[InlineData( " 0", "-1", true )]
		[InlineData( "-1", " 0", true )]
		[InlineData( " 1", " 1", false )]
		[InlineData( "-1", " 1", true )]
		[InlineData( " 1", "-1", true )]
		[InlineData( "-1", "-1", false )]
		[InlineData( " 1/2", " 1/2", false )]
		[InlineData( "-1/2", " 1/2", true )]
		[InlineData( " 1/2", "-1/2", true )]
		[InlineData( "-1/2", "-1/2", false )]
		[InlineData( " 1/3", " 1/2", true )]
		[InlineData( "-1/3", " 1/2", true )]
		[InlineData( " 1/3", "-1/2", true )]
		[InlineData( "-1/3", "-1/2", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Rational.Parse( l ) != Rational.Parse( r ) );
	}

	public class RationalAddition {
		[Theory]
		[InlineData( " 1/2", " 1/2", " 1" )]     // Sanity
		[InlineData( "-1/2", " 1/2", " 0" )]     // Sanity
		[InlineData( " 1/2", "-1/2", " 0" )]     // Sanity
		[InlineData( "-1/2", "-1/2", "-1" )]     // Sanity
		[InlineData( " 1/2", " 0", " 1/2" )]   // Sanity
		[InlineData( "-1/2", " 0", "-1/2" )]   // Sanity
		[InlineData( " 0", " 1/2", " 1/2" )]   // Sanity
		[InlineData( " 0", "-1/2", "-1/2" )]   // Sanity
		[InlineData( " 1/6", " 5/6", " 1" )]     // l < r
		[InlineData( "-1/6", " 5/6", " 2/3" )]   // l < r
		[InlineData( " 1/6", "-5/6", "-2/3" )]   // l < r
		[InlineData( "-1/6", "-5/6", "-1" )]     // l < r
		[InlineData( " 9/11", " 2/11", " 1" )]     // l > r
		[InlineData( "-9/11", " 2/11", "-7/11" )]  // l > r
		[InlineData( " 9/11", "-2/11", " 7/11" )]  // l > r
		[InlineData( "-9/11", "-2/11", "-1" )]     // l > r
		public void Sanity( string l, string r, string s ) =>
			Assert.Equal( Rational.Parse( s ), Rational.Parse( l ) + Rational.Parse( r ) );

		[Fact]
		public void DifferentDenominator() =>
			Assert.Equal(
				Rational.Parse( "5/6" ),
				Rational.Parse( "1/2" ) + Rational.Parse( "1/3" )
			);
	}

	public class RationalSubtraction {
		[Theory]
		[InlineData( " 1/2", " 1/2", " 0" )]     // Sanity
		[InlineData( "-1/2", " 1/2", "-1" )]     // Sanity
		[InlineData( " 1/2", "-1/2", " 1" )]     // Sanity
		[InlineData( "-1/2", "-1/2", " 0" )]     // Sanity
		[InlineData( " 1/2", " 0", " 1/2" )]   // Sanity
		[InlineData( "-1/2", " 0", "-1/2" )]   // Sanity
		[InlineData( " 0", " 1/2", "-1/2" )]   // Sanity
		[InlineData( " 0", "-1/2", " 1/2" )]   // Sanity
		[InlineData( " 1/6", " 5/6", "-2/3" )]   // l < r
		[InlineData( "-1/6", " 5/6", "-1" )]     // l < r
		[InlineData( " 1/6", "-5/6", " 1" )]     // l < r
		[InlineData( "-1/6", "-5/6", " 2/3" )]   // l < r
		[InlineData( " 9/11", " 2/11", " 7/11" )]  // l > r
		[InlineData( "-9/11", " 2/11", "-1" )]     // l > r
		[InlineData( " 9/11", "-2/11", " 1" )]     // l > r
		[InlineData( "-9/11", "-2/11", "-7/11" )]  // l > r
		public void Sanity( string l, string r, string s ) =>
			Assert.Equal( Rational.Parse( s ), Rational.Parse( l ) - Rational.Parse( r ) );

		[Fact]
		public void DifferentDenominator() =>
			Assert.Equal(
				Rational.Parse( "1/6" ),
				Rational.Parse( "1/2" ) - Rational.Parse( "1/3" )
			);
	}

	public class RationalMultiply {
		[Theory]
		[InlineData( " 1/2", " 1", " 1/2" )]      // Sanity
		[InlineData( "-1/2", " 1", "-1/2" )]      // Sanity
		[InlineData( " 1/2", "-1", "-1/2" )]      // Sanity
		[InlineData( "-1/2", "-1", " 1/2" )]      // Sanity
		[InlineData( " 1  ", " 1/2", " 1/2" )]      // Sanity
		[InlineData( "-1  ", " 1/2", "-1/2" )]      // Sanity
		[InlineData( " 1  ", "-1/2", "-1/2" )]      // Sanity
		[InlineData( "-1  ", "-1/2", " 1/2" )]      // Sanity
		[InlineData( " 1/2", " 0", " 0" )]        // Sanity
		[InlineData( "-1/2", " 0", " 0" )]        // Sanity
		[InlineData( " 0", " 1/2", " 0" )]        // Sanity
		[InlineData( " 0", "-1/2", " 0" )]        // Sanity
		[InlineData( " 6/11", " 7/13", " 42/143" )]   // multiple bits
		[InlineData( "-6/11", " 7/13", "-42/143" )]   // multiple bits
		[InlineData( " 6/11", "-7/13", "-42/143" )]   // multiple bits
		[InlineData( "-6/11", "-7/13", " 42/143" )]   // multiple bits
		public void Sanity( string l, string r, string p ) =>
			Assert.Equal( Rational.Parse( p ), Rational.Parse( l ) * Rational.Parse( r ) );

		[Fact]
		public void Inverse() =>
			Assert.Equal(
				Rational.Parse( "1" ),
				Rational.Parse( "1/2" ) * Rational.Parse( "6/3" )
			);

		[Fact]
		public void Associtivity() =>
			Assert.Equal(
				( Rational.Parse( "1/2" ) * Rational.Parse( "3/4" ) ) * Rational.Parse( "5/6" ),
				Rational.Parse( "1/2" ) * ( Rational.Parse( "3/4" ) * Rational.Parse( "5/6" ) )
			);

		[Fact]
		public void Communtivity() =>
			Assert.Equal(
				Rational.Parse( "1/2" ) * Rational.Parse( "3/4" ),
				Rational.Parse( "3/4" ) * Rational.Parse( "1/2" )
			);

		[Fact]
		public void Distributive() =>
			Assert.Equal(
				( Rational.Parse( "1/2" ) * Rational.Parse( "3/4" ) ) + ( Rational.Parse( "1/2" ) * Rational.Parse( "5/6" ) ),
				Rational.Parse( "1/2" ) * ( Rational.Parse( "3/4" ) + Rational.Parse( "5/6" ) )
			);
	}

	public class RationalDivision {
		[Theory]
		[InlineData( " 1/2", " 1", " 1/2" )]      // Sanity
		[InlineData( "-1/2", " 1", "-1/2" )]      // Sanity
		[InlineData( " 1/2", "-1", "-1/2" )]      // Sanity
		[InlineData( "-1/2", "-1", " 1/2" )]      // Sanity
		[InlineData( " 1  ", " 1/2", " 2" )]        // Sanity
		[InlineData( "-1  ", " 1/2", "-2" )]        // Sanity
		[InlineData( " 1  ", "-1/2", "-2" )]        // Sanity
		[InlineData( "-1  ", "-1/2", " 2" )]        // Sanity
		[InlineData( " 0", " 1/2", " 0" )]        // Sanity
		[InlineData( " 0", "-1/2", " 0" )]        // Sanity
		[InlineData( " 6/11", " 7/13", " 78/77" )]    // multiple bits
		[InlineData( "-6/11", " 7/13", "-78/77" )]    // multiple bits
		[InlineData( " 6/11", "-7/13", "-78/77" )]    // multiple bits
		[InlineData( "-6/11", "-7/13", " 78/77" )]    // multiple bits
		public void Sanity( string dividend, string divisor, string quotient ) =>
			Assert.Equal( Rational.Parse( quotient ), Rational.Parse( dividend ) / Rational.Parse( divisor ) );

		[Fact]
		public void DivideByZero() =>
			Assert.Throws<System.DivideByZeroException>( () => ( Rational.Unit / Rational.Zero ) );

		[Fact]
		public void Inverse() =>
			Assert.Equal(
				Rational.Parse( "1" ),
				Rational.Parse( "1/2" ) / Rational.Parse( "1/2" )
			);
	}

	public class RationalModulo {
		[Theory]
		[InlineData( " 1/2", " 1", " 1/2" )]      // Sanity
		[InlineData( "-1/2", " 1", "-1/2" )]      // Sanity
		[InlineData( " 1/2", "-1", "-1/2" )]      // Sanity
		[InlineData( "-1/2", "-1", " 1/2" )]      // Sanity
		[InlineData( " 1  ", " 1/2", " 0" )]        // Sanity
		[InlineData( "-1  ", " 1/2", " 0" )]        // Sanity
		[InlineData( " 1  ", "-1/2", " 0" )]        // Sanity
		[InlineData( "-1  ", "-1/2", " 0" )]        // Sanity
		[InlineData( " 0", " 1/2", " 0" )]        // Sanity
		[InlineData( " 0", "-1/2", " 0" )]        // Sanity
		[InlineData( " 6/11", " 7/13", " 1/77" )]     // multiple bits
		[InlineData( "-6/11", " 7/13", "-1/77" )]     // multiple bits
		[InlineData( " 6/11", "-7/13", "-1/77" )]     // multiple bits
		[InlineData( "-6/11", "-7/13", " 1/77" )]     // multiple bits
		public void Sanity( string dividend, string divisor, string remainder ) =>
			Assert.Equal(
				Rational.Parse( remainder ),
				Rational.Parse( dividend ) % Rational.Parse( divisor )
			);

		[Fact]
		public void DivideByZero() =>
			Assert.Throws<System.DivideByZeroException>( () => ( Rational.Unit % Rational.Zero ) );

		[Fact]
		public void Inverse() =>
			Assert.Equal(
				Rational.Parse( "0" ),
				Rational.Parse( "1/2" ) % Rational.Parse( "1/2" )
			);
	}

	public class RationalDivisionModulo {
		[Theory]
		[InlineData( " 1/2", " 1", " 0", " 1/2" )]      // Sanity
		[InlineData( "-1/2", " 1", " 0", "-1/2" )]      // Sanity
		[InlineData( " 1/2", "-1", " 0", "-1/2" )]      // Sanity
		[InlineData( "-1/2", "-1", " 0", " 1/2" )]      // Sanity
		[InlineData( " 1  ", " 1/2", " 2", " 0" )]        // Sanity
		[InlineData( "-1  ", " 1/2", "-2", " 0" )]        // Sanity
		[InlineData( " 1  ", "-1/2", "-2", " 0" )]        // Sanity
		[InlineData( "-1  ", "-1/2", " 2", " 0" )]        // Sanity
		[InlineData( " 0", " 1/2", " 0", " 0" )]        // Sanity
		[InlineData( " 0", "-1/2", " 0", " 0" )]        // Sanity
		[InlineData( " 6/11", " 7/13", " 1", " 1/77" )]     // multiple bits
		[InlineData( "-6/11", " 7/13", "-1", "-1/77" )]     // multiple bits
		[InlineData( " 6/11", "-7/13", "-1", "-1/77" )]     // multiple bits
		[InlineData( "-6/11", "-7/13", " 1", " 1/77" )]     // multiple bits
		public void Sanity( string dividend, string divisor, string quotient, string remainder ) =>
			Assert.Equal(
				new Tuple<Integer, Rational>( Integer.Parse( quotient ), Rational.Parse( remainder ) ),
				Rational.op_DividePercent( Rational.Parse( dividend ), Rational.Parse( divisor ) )
			);

		[Fact]
		public void DivideByZero() =>
			Assert.Throws<System.DivideByZeroException>( () => Rational.op_DividePercent( Rational.Unit, Rational.Zero ) );

		[Fact]
		public void Inverse() =>
			Assert.Equal(
				new Tuple<Integer, Rational>( Integer.Parse( "1" ), Rational.Parse( "0" ) ),
				Rational.op_DividePercent( Rational.Parse( "1/2" ), Rational.Parse( "1/2" ) )
			);
	}

	public class RationalNegation {
		[Theory]
		[InlineData( 1, 1u, -1, 1u )] // Sanity
		[InlineData( -1, 1u, 1, 1u )] // Sanity
		[InlineData( 0, 1u, 0, 1u )] // Sanity
		public void Sanity( int n, uint d, int en, uint ed ) =>
			Assert.Equal(
				new Rational( new Integer( en ), new Natural( ed ) ),
				-new Rational( new Integer( n ), new Natural( d ) )
			);

		[Fact]
		public void Big() =>
			Assert.Equal(
				new Rational( new Integer( new[] { 0xFEDCBA9u, 0x76543210u }, true ), Natural.Unit ),
				-new Rational( new Integer( new[] { 0xFEDCBA9u, 0x76543210u }, false ), Natural.Unit )
			);

		[Fact]
		public void BigNegative() =>
			Assert.Equal(
				new Rational( new Integer( new[] { 0xFEDCBA9u, 0x76543210u }, false ), Natural.Unit ),
				-new Rational( new Integer( new[] { 0xFEDCBA9u, 0x76543210u }, true ), Natural.Unit )
			);

		[Fact]
		public void Small() =>
			Assert.Equal(
				new Rational( -Integer.Unit, new Natural( new[] { 0xFEDCBA9u, 0x76543210u } ) ),
				-new Rational( Integer.Unit, new Natural( new[] { 0xFEDCBA9u, 0x76543210u } ) )
			);

		[Fact]
		public void SmallNegative() =>
			Assert.Equal(
				new Rational( Integer.Unit, new Natural( new[] { 0xFEDCBA9u, 0x76543210u } ) ),
				-new Rational( -Integer.Unit, new Natural( new[] { 0xFEDCBA9u, 0x76543210u } ) )
			);
	}

	public class RationalEquals {
		[Theory]
		[InlineData( 1, 1, true )]    // Sanity
		[InlineData( 1, -1, false )]   // Sanity
		[InlineData( -1, 1, false )]   // Sanity
		[InlineData( -1, -1, true )]    // Sanity
		public void Sanity( int l, int r, bool e ) =>
			Assert.Equal( e, new Rational( new Integer( l ), Natural.Unit ).Equals( new Rational( new Integer( r ), Natural.Unit ) ) );

		[Fact]
		public void NaturalEquals() =>
			Assert.True( Rational.Unit.Equals( Natural.Unit ) );

		[Fact]
		public void NaturalNotEquals() =>
			Assert.False( Rational.Unit.Equals( Natural.Zero ) );

		[Fact]
		public void NaturalSignNotEquals() =>
			Assert.False( ( -Rational.Unit ).Equals( Natural.Unit ) );

		[Fact]
		public void IntegerEquals() =>
			Assert.True( Rational.Unit.Equals( Integer.Unit ) );

		[Fact]
		public void IntegerNotEquals() =>
			Assert.False( Rational.Unit.Equals( Integer.Zero ) );

		[Fact]
		public void IntegerSignNotEquals() =>
			Assert.False( ( -Rational.Unit ).Equals( Integer.Unit ) );
	}

	public class RationalToString {
		[Theory]
		[InlineData( 0, 1u, "0" )]
		[InlineData( 1, 1u, "1" )]
		[InlineData( -1, 1u, "-1" )]
		[InlineData( 1, 2u, "1 / 2" )]
		[InlineData( -1, 2u, "-1 / 2" )]
		[InlineData( 5, 2u, "5 / 2" )]
		[InlineData( 41, 15526u, "41 / 15526" )]
		public void Sanity( int num, uint denom, string expected ) =>
			Assert.Equal( expected, new Rational( new Integer( num ), new Natural( denom ) ).ToString() );

		[Fact]
		public void Bigger() =>
			Assert.Equal(
				"1234567890123456789",
				new Rational( new Integer( new[] { 0x112210F4u, 0x7DE98115u } ), Natural.Unit ).ToString()
			);

		[Fact]
		public void BiggerNegative() =>
			Assert.Equal(
				"-1234567890123456789",
				new Rational( new Integer( new[] { 0x112210F4u, 0x7DE98115u }, true ), Natural.Unit ).ToString()
			);

		[Fact]
		public void Smaller() =>
			Assert.Equal(
				"1 / 1234567890123456789",
				new Rational( Integer.Unit, new Natural( new[] { 0x112210F4u, 0x7DE98115u } ) ).ToString()
			);

		[Fact]
		public void SmallerNegative() =>
			Assert.Equal(
				"-1 / 1234567890123456789",
				new Rational( -Integer.Unit, new Natural( new[] { 0x112210F4u, 0x7DE98115u } ) ).ToString()
			);
	}

	public class RationalParse {
		[Theory]
		[InlineData( "0", 0, 1u )]        // Sanity
		[InlineData( "1", 1, 1u )]        // Sanity
		[InlineData( "-1", -1, 1u )]        // Sanity
		[InlineData( "1/2", 1, 2u )]        // Sanity
		[InlineData( "1 /2", 1, 2u )]        // Sanity
		[InlineData( "1/ 2", 1, 2u )]        // Sanity
		[InlineData( "1 / 2", 1, 2u )]        // Sanity
		[InlineData( "-1 / 2", -1, 2u )]        // Sanity
		[InlineData( "1 / -2", -1, 2u )]        // Sanity
		[InlineData( "-1 / -2", 1, 2u )]        // Sanity
		[InlineData( "0 / -2", 0, 2u )]        // Sanity
		public void Sanity( string str, int n, uint d ) =>
			Assert.Equal( new Rational( n, d ), Rational.Parse( str ) );

		[Fact]
		public void Bigger() =>
			Assert.Equal(
				new Rational(
					new Integer( new[] { 0x112210F4u, 0x7DE98115u } ),
					new Natural( 41u )
				),
				Rational.Parse( "1234567890123456789/41" )
			);

		[Fact]
		public void BiggerNegative() =>
			Assert.Equal(
				new Rational(
					new Integer( new[] { 0x112210F4u, 0x7DE98115u }, true ),
					new Natural( 41u )
				),
				Rational.Parse( "-1234567890123456789/41" )
			);

		[Theory]
		[InlineData( "" )]
		[InlineData( " " )]
		[InlineData( "\t" )]
		[InlineData( "\n" )]
		[InlineData( "\r" )]
		[InlineData( null )]
		[InlineData( "\r\n" )]
		public void ArgEmpty( string str ) =>
			Assert.IsType<System.ArgumentNullException>(
				Record.Exception( () => Rational.Parse( str ) )
			);

		[Fact]
		public void FormatEx() =>
			Assert.IsType<System.FormatException>(
				Record.Exception( () => Rational.Parse( "1/2/3" ) )
			);
	}
}
