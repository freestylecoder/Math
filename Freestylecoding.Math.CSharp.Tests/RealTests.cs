using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests {
	public class RealCtor {
		[Theory]
		[InlineData( " 0", " 1", "     0" )]
		[InlineData( " 1", " 1", "    10" )]
		[InlineData( "-1", " 1", "   -10" )]
		[InlineData( " 1", " 2", "   100" )]
		[InlineData( "-1", " 2", "  -100" )]
		[InlineData( " 2", " 4", " 20000" )]
		[InlineData( "-2", " 4", "-20000" )]
		[InlineData( " 0", "-1", "     0" )]
		[InlineData( " 1", "-1", "     0.1" )]
		[InlineData( "-1", "-1", "    -0.1" )]
		[InlineData( " 1", "-2", "     0.01" )]
		[InlineData( "-1", "-2", "    -0.01" )]
		[InlineData( " 2", "-4", "     0.0002" )]
		[InlineData( "-2", "-4", "    -0.0002" )]
		[InlineData( "12", "-1", "     1.2" )]
		public void DefaultCtor( string n, string d, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Integer.Parse( n ), Integer.Parse( d ) )
			);

		[Theory]
		[InlineData( "0", "0" )]
		[InlineData( "1", "1" )]
		[InlineData( "2", "2" )]
		[InlineData( "12", "12" )]
		public void NaturalCtor( string n, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Natural.Parse( n ) )
			);

		[Theory]
		[InlineData( "0", "0", "0" )]
		[InlineData( "1", "0", "1" )]
		[InlineData( "2", "0", "2" )]
		[InlineData( "12", "0", "12" )]
		[InlineData( "0", "1", "0" )]
		[InlineData( "1", "1", "10" )]
		[InlineData( "2", "1", "20" )]
		[InlineData( "12", "1", "120" )]
		[InlineData( "0", "5", "0" )]
		[InlineData( "1", "5", "100000" )]
		[InlineData( "2", "5", "200000" )]
		[InlineData( "12", "5", "1200000" )]
		public void NaturalNaturalCtor( string s, string e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Natural.Parse( s ), Natural.Parse( e ) )
			);

		[Theory]
		[InlineData( "0", "0", "0" )]
		[InlineData( "1", "0", "1" )]
		[InlineData( "2", "0", "2" )]
		[InlineData( "12", "0", "12" )]
		[InlineData( "0", "1", "0" )]
		[InlineData( "1", "1", "10" )]
		[InlineData( "2", "1", "20" )]
		[InlineData( "12", "1", "120" )]
		[InlineData( "0", "5", "0" )]
		[InlineData( "1", "5", "100000" )]
		[InlineData( "2", "5", "200000" )]
		[InlineData( "12", "5", "1200000" )]
		[InlineData( "0", "-1", "0" )]
		[InlineData( "1", "-1", "0.1" )]
		[InlineData( "2", "-1", "0.2" )]
		[InlineData( "12", "-1", "1.2" )]
		[InlineData( "0", "-5", "0" )]
		[InlineData( "1", "-5", "0.00001" )]
		[InlineData( "2", "-5", "0.00002" )]
		[InlineData( "12", "-5", "0.00012" )]
		public void NaturalIntegerCtor( string s, string e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Natural.Parse( s ), Integer.Parse( e ) )
			);

		[Theory]
		[InlineData( "0", "0" )]
		[InlineData( "1", "1" )]
		[InlineData( "2", "2" )]
		[InlineData( "12", "12" )]
		[InlineData( "-1", "-1" )]
		[InlineData( "-2", "-2" )]
		[InlineData( "-12", "-12" )]
		public void IntegerCtor( string i, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Integer.Parse( i ) )
			);

		[Theory]
		[InlineData( "0", "0", "0" )]
		[InlineData( "1", "0", "1" )]
		[InlineData( "2", "0", "2" )]
		[InlineData( "12", "0", "12" )]
		[InlineData( "-1", "0", "-1" )]
		[InlineData( "-2", "0", "-2" )]
		[InlineData( "-12", "0", "-12" )]
		[InlineData( "0", "1", "0" )]
		[InlineData( "1", "1", "10" )]
		[InlineData( "2", "1", "20" )]
		[InlineData( "12", "1", "120" )]
		[InlineData( "-1", "1", "-10" )]
		[InlineData( "-2", "1", "-20" )]
		[InlineData( "-12", "1", "-120" )]
		[InlineData( "0", "5", "0" )]
		[InlineData( "1", "5", "100000" )]
		[InlineData( "2", "5", "200000" )]
		[InlineData( "12", "5", "1200000" )]
		[InlineData( "-1", "5", "-100000" )]
		[InlineData( "-2", "5", "-200000" )]
		[InlineData( "-12", "5", "-1200000" )]
		public void IntegerNaturalCtor( string i, string n, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Integer.Parse( i ), Natural.Parse( n ) )
			);

		[Theory]
		[InlineData( 0u, 0u, "0" )]
		[InlineData( 1u, 0u, "1" )]
		[InlineData( 2u, 0u, "2" )]
		[InlineData( 12u, 0u, "12" )]
		[InlineData( 0u, 1u, "0" )]
		[InlineData( 1u, 1u, "10" )]
		[InlineData( 2u, 1u, "20" )]
		[InlineData( 12u, 1u, "120" )]
		[InlineData( 0u, 5u, "0" )]
		[InlineData( 1u, 5u, "100000" )]
		[InlineData( 2u, 5u, "200000" )]
		[InlineData( 12u, 5u, "1200000" )]
		public void Uint32Uint32Ctor( uint s, uint e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( s, e )
			);

		[Theory]
		[InlineData( 0u, 0, "0" )]
		[InlineData( 1u, 0, "1" )]
		[InlineData( 2u, 0, "2" )]
		[InlineData( 12u, 0, "12" )]
		[InlineData( 0u, 1, "0" )]
		[InlineData( 1u, 1, "10" )]
		[InlineData( 2u, 1, "20" )]
		[InlineData( 12u, 1, "120" )]
		[InlineData( 0u, 5, "0" )]
		[InlineData( 1u, 5, "100000" )]
		[InlineData( 2u, 5, "200000" )]
		[InlineData( 12u, 5, "1200000" )]
		[InlineData( 0u, -1, "0" )]
		[InlineData( 1u, -1, "0.1" )]
		[InlineData( 2u, -1, "0.2" )]
		[InlineData( 12u, -1, "1.2" )]
		[InlineData( 0u, -5, "0" )]
		[InlineData( 1u, -5, "0.00001" )]
		[InlineData( 2u, -5, "0.00002" )]
		[InlineData( 12u, -5, "0.00012" )]
		public void Unit32Int32Ctor( uint s, int e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( s, e )
			);

		[Theory]
		[InlineData( 0, 0u, "0" )]
		[InlineData( 1, 0u, "1" )]
		[InlineData( 2, 0u, "2" )]
		[InlineData( 12, 0u, "12" )]
		[InlineData( -1, 0u, "-1" )]
		[InlineData( -2, 0u, "-2" )]
		[InlineData( -12, 0u, "-12" )]
		[InlineData( 0, 1u, "0" )]
		[InlineData( 1, 1u, "10" )]
		[InlineData( 2, 1u, "20" )]
		[InlineData( 12, 1u, "120" )]
		[InlineData( -1, 1u, "-10" )]
		[InlineData( -2, 1u, "-20" )]
		[InlineData( -12, 1u, "-120" )]
		[InlineData( 0, 5u, "0" )]
		[InlineData( 1, 5u, "100000" )]
		[InlineData( 2, 5u, "200000" )]
		[InlineData( 12, 5u, "1200000" )]
		[InlineData( -1, 5u, "-100000" )]
		[InlineData( -2, 5u, "-200000" )]
		[InlineData( -12, 5u, "-1200000" )]
		public void Int32Uint32Ctor( int i, uint n, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( i, n )
			);

		[Theory]
		[InlineData( 0, 1, "     0" )]
		[InlineData( 1, 1, "    10" )]
		[InlineData( -1, 1, "   -10" )]
		[InlineData( 1, 2, "   100" )]
		[InlineData( -1, 2, "  -100" )]
		[InlineData( 2, 4, " 20000" )]
		[InlineData( -2, 4, "-20000" )]
		[InlineData( 0, -1, "     0" )]
		[InlineData( 1, -1, "     0.1" )]
		[InlineData( -1, -1, "    -0.1" )]
		[InlineData( 1, -2, "     0.01" )]
		[InlineData( -1, -2, "    -0.01" )]
		[InlineData( 2, -4, "     0.0002" )]
		[InlineData( -2, -4, "    -0.0002" )]
		[InlineData( 12, -1, "     1.2" )]
		public void Int32Int32Ctor( int s, int e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( s, e )
			);

		[Theory]
		[InlineData( "0", "1", "0" )]
		[InlineData( "1", "1", "1" )]
		[InlineData( "2", "1", "2" )]
		[InlineData( "12", "1", "12" )]
		[InlineData( "-1", "1", "-1" )]
		[InlineData( "-2", "1", "-2" )]
		[InlineData( "-12", "1", "-12" )]
		[InlineData( "0", "5", "0" )]
		[InlineData( "1", "5", "0.2" )]
		[InlineData( "2", "5", "0.4" )]
		[InlineData( "12", "5", "2.4" )]
		[InlineData( "-1", "5", "-0.2" )]
		[InlineData( "-2", "5", "-0.4" )]
		[InlineData( "-12", "5", "-2.4" )]
		public void RationalCtor( string n, string d, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( new Rational( Integer.Parse( n ), Natural.Parse( d ) ) )
			);

		[Theory]
		[InlineData( "0", "1", "0", "0" )]
		[InlineData( "1", "1", "0", "1" )]
		[InlineData( "2", "1", "0", "2" )]
		[InlineData( "12", "1", "0", "12" )]
		[InlineData( "-1", "1", "0", "-1" )]
		[InlineData( "-2", "1", "0", "-2" )]
		[InlineData( "-12", "1", "0", "-12" )]
		[InlineData( "0", "5", "0", "0" )]
		[InlineData( "1", "5", "0", "0.2" )]
		[InlineData( "2", "5", "0", "0.4" )]
		[InlineData( "12", "5", "0", "2.4" )]
		[InlineData( "-1", "5", "0", "-0.2" )]
		[InlineData( "-2", "5", "0", "-0.4" )]
		[InlineData( "-12", "5", "0", "-2.4" )]
		[InlineData( "0", "1", "1", "0" )]
		[InlineData( "1", "1", "1", "10" )]
		[InlineData( "2", "1", "1", "20" )]
		[InlineData( "12", "1", "1", "120" )]
		[InlineData( "-1", "1", "1", "-10" )]
		[InlineData( "-2", "1", "1", "-20" )]
		[InlineData( "-12", "1", "1", "-120" )]
		[InlineData( "0", "5", "1", "0" )]
		[InlineData( "1", "5", "1", "2" )]
		[InlineData( "2", "5", "1", "4" )]
		[InlineData( "12", "5", "1", "24" )]
		[InlineData( "-1", "5", "1", "-2" )]
		[InlineData( "-2", "5", "1", "-4" )]
		[InlineData( "-12", "5", "1", "-24" )]
		[InlineData( "0", "1", "5", "0" )]
		[InlineData( "1", "1", "5", "100000" )]
		[InlineData( "2", "1", "5", "200000" )]
		[InlineData( "12", "1", "5", "1200000" )]
		[InlineData( "-1", "1", "5", "-100000" )]
		[InlineData( "-2", "1", "5", "-200000" )]
		[InlineData( "-12", "1", "5", "-1200000" )]
		[InlineData( "0", "5", "5", "0" )]
		[InlineData( "1", "5", "5", "20000" )]
		[InlineData( "2", "5", "5", "40000" )]
		[InlineData( "12", "5", "5", "240000" )]
		[InlineData( "-1", "5", "5", "-20000" )]
		[InlineData( "-2", "5", "5", "-40000" )]
		[InlineData( "-12", "5", "5", "-240000" )]
		public void RationalNaturalCtor( string n, string d, string e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( new Rational( Integer.Parse( n ), Natural.Parse( d ) ), Natural.Parse( e ) )
			);

		[Theory]
		[InlineData( "0", "1", "0", "0" )]
		[InlineData( "1", "1", "0", "1" )]
		[InlineData( "2", "1", "0", "2" )]
		[InlineData( "12", "1", "0", "12" )]
		[InlineData( "-1", "1", "0", "-1" )]
		[InlineData( "-2", "1", "0", "-2" )]
		[InlineData( "-12", "1", "0", "-12" )]
		[InlineData( "0", "5", "0", "0" )]
		[InlineData( "1", "5", "0", "0.2" )]
		[InlineData( "2", "5", "0", "0.4" )]
		[InlineData( "12", "5", "0", "2.4" )]
		[InlineData( "-1", "5", "0", "-0.2" )]
		[InlineData( "-2", "5", "0", "-0.4" )]
		[InlineData( "-12", "5", "0", "-2.4" )]
		[InlineData( "0", "1", "1", "0" )]
		[InlineData( "1", "1", "1", "10" )]
		[InlineData( "2", "1", "1", "20" )]
		[InlineData( "12", "1", "1", "120" )]
		[InlineData( "-1", "1", "1", "-10" )]
		[InlineData( "-2", "1", "1", "-20" )]
		[InlineData( "-12", "1", "1", "-120" )]
		[InlineData( "0", "5", "1", "0" )]
		[InlineData( "1", "5", "1", "2" )]
		[InlineData( "2", "5", "1", "4" )]
		[InlineData( "12", "5", "1", "24" )]
		[InlineData( "-1", "5", "1", "-2" )]
		[InlineData( "-2", "5", "1", "-4" )]
		[InlineData( "-12", "5", "1", "-24" )]
		[InlineData( "0", "1", "5", "0" )]
		[InlineData( "1", "1", "5", "100000" )]
		[InlineData( "2", "1", "5", "200000" )]
		[InlineData( "12", "1", "5", "1200000" )]
		[InlineData( "-1", "1", "5", "-100000" )]
		[InlineData( "-2", "1", "5", "-200000" )]
		[InlineData( "-12", "1", "5", "-1200000" )]
		[InlineData( "0", "5", "5", "0" )]
		[InlineData( "1", "5", "5", "20000" )]
		[InlineData( "2", "5", "5", "40000" )]
		[InlineData( "12", "5", "5", "240000" )]
		[InlineData( "-1", "5", "5", "-20000" )]
		[InlineData( "-2", "5", "5", "-40000" )]
		[InlineData( "-12", "5", "5", "-240000" )]
		[InlineData( "0", "1", "-1", "0" )]
		[InlineData( "1", "1", "-1", "0.1" )]
		[InlineData( "2", "1", "-1", "0.2" )]
		[InlineData( "12", "1", "-1", "1.2" )]
		[InlineData( "-1", "1", "-1", "-0.1" )]
		[InlineData( "-2", "1", "-1", "-0.2" )]
		[InlineData( "-12", "1", "-1", "-1.2" )]
		[InlineData( "0", "5", "-1", "0" )]
		[InlineData( "1", "5", "-1", "0.02" )]
		[InlineData( "2", "5", "-1", "0.04" )]
		[InlineData( "12", "5", "-1", "0.24" )]
		[InlineData( "-1", "5", "-1", "-0.02" )]
		[InlineData( "-2", "5", "-1", "-0.04" )]
		[InlineData( "-12", "5", "-1", "-0.24" )]
		[InlineData( "0", "1", "-5", "0" )]
		[InlineData( "1", "1", "-5", "0.00001" )]
		[InlineData( "2", "1", "-5", "0.00002" )]
		[InlineData( "12", "1", "-5", "0.00012" )]
		[InlineData( "-1", "1", "-5", "-0.00001" )]
		[InlineData( "-2", "1", "-5", "-0.00002" )]
		[InlineData( "-12", "1", "-5", "-0.00012" )]
		[InlineData( "0", "5", "-5", "0" )]
		[InlineData( "1", "5", "-5", "0.000002" )]
		[InlineData( "2", "5", "-5", "0.000004" )]
		[InlineData( "12", "5", "-5", "0.000024" )]
		[InlineData( "-1", "5", "-5", "-0.000002" )]
		[InlineData( "-2", "5", "-5", "-0.000004" )]
		[InlineData( "-12", "5", "-5", "-0.000024" )]
		public void RationalIntegerCtor( string n, string d, string e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( new Rational( Integer.Parse( n ), Natural.Parse( d ) ), Integer.Parse( e ) )
			);

		[Theory]
		[InlineData( "0" )]
		[InlineData( "1" )]
		[InlineData( "2" )]
		[InlineData( "12" )]
		[InlineData( "-1" )]
		[InlineData( "-2" )]
		[InlineData( "-12" )]
		[InlineData( "0.2" )]
		[InlineData( "0.4" )]
		[InlineData( "2.4" )]
		[InlineData( "-0.2" )]
		[InlineData( "-0.4" )]
		[InlineData( "-2.4" )]
		public void Real_Ctor( string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Real.Parse( r ) )
			);

		[Theory]
		[InlineData( "0", "0", "0" )]
		[InlineData( "2", "0", "2" )]
		[InlineData( "-2", "0", "-2" )]
		[InlineData( "0.2", "0", "0.2" )]
		[InlineData( "-0.2", "0", "-0.2" )]
		[InlineData( "0", "2", "0" )]
		[InlineData( "2", "2", "200" )]
		[InlineData( "-2", "2", "-200" )]
		[InlineData( "0.2", "2", "20" )]
		[InlineData( "-0.2", "2", "-20" )]
		public void RealNaturalCtor( string s, string e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Real.Parse( s ), Natural.Parse( e ) )
			);

		[Theory]
		[InlineData( "0", "0", "0" )]
		[InlineData( "2", "0", "2" )]
		[InlineData( "-2", "0", "-2" )]
		[InlineData( "0.2", "0", "0.2" )]
		[InlineData( "-0.2", "0", "-0.2" )]
		[InlineData( "0", "2", "0" )]
		[InlineData( "2", "2", "200" )]
		[InlineData( "-2", "2", "-200" )]
		[InlineData( "0.2", "2", "20" )]
		[InlineData( "-0.2", "2", "-20" )]
		[InlineData( "0", "-2", "0" )]
		[InlineData( "2", "-2", "0.02" )]
		[InlineData( "-2", "-2", "-0.02" )]
		[InlineData( "0.2", "-2", "0.002" )]
		[InlineData( "-0.2", "-2", "-0.002" )]
		public void RealIntegerCtor( string s, string e, string r ) =>
			Assert.Equal(
				Real.Parse( r ),
				new Real( Real.Parse( s ), Integer.Parse( e ) )
			);
	}

	public class RealEquality {
		[Theory]
		[InlineData( " 0", " 0", true )]
		[InlineData( " 0", " 1", false )]
		[InlineData( " 1", " 0", false )]
		[InlineData( " 1", " 1", true )]
		[InlineData( " 0", "-1", false )]
		[InlineData( "-1", " 0", false )]
		[InlineData( "-1", " 1", false )]
		[InlineData( " 1", "-1", false )]
		[InlineData( "-1", "-1", true )]
		[InlineData( " 1", "10", false )]
		[InlineData( " 1", "0.1", false )]
		[InlineData( "10", "0.1", false )]
		[InlineData( "10", "100", false )]
		public void Sanity( string l, string r, bool e ) =>
			Assert.Equal( e, Real.Parse( l ) == Real.Parse( r ) );

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
		public void CheckSignificand( int l, int r, bool e ) =>
			Assert.Equal( e, new Real( l, 0 ) == new Real( r, 0 ) );

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
		public void CheckExponent( int l, int r, bool e ) =>
			Assert.Equal( e, new Real( 1, l ) == new Real( 1, r ) );
	}

	public class RealGreaterThan {
		[Theory]
		[InlineData( " 0", " 1", false )]
		[InlineData( " 1", " 0", true )]
		[InlineData( " 0", "-1", true )]
		[InlineData( "-1", " 0", false )]
		[InlineData( " 1", " 1", false )]
		[InlineData( "-1", " 1", false )]
		[InlineData( " 1", "-1", true )]
		[InlineData( "-1", "-1", false )]
		[InlineData( " 0.5", " 0.5", false )]
		[InlineData( "-0.5", " 0.5", false )]
		[InlineData( " 0.5", "-0.5", true )]
		[InlineData( "-0.5", "-0.5", false )]
		[InlineData( " 0.33", " 0.5", false )]
		[InlineData( "-0.33", " 0.5", false )]
		[InlineData( " 0.33", "-0.5", true )]
		[InlineData( "-0.33", "-0.5", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Real.Parse( l ) > Real.Parse( r ) );

		[Theory]
		[InlineData( "0.01", "0.011", false )]
		[InlineData( "0.011", "0.01", true )]
		[InlineData( "1111", "100", true )]
		public void Normalization( string l, string r, bool x ) =>
			Assert.Equal( x, Real.Parse( l ) > Real.Parse( r ) );
	}

	public class RealLessThan {
		[Theory]
		[InlineData( " 0", " 1", true )]
		[InlineData( " 1", " 0", false )]
		[InlineData( " 0", "-1", false )]
		[InlineData( "-1", " 0", true )]
		[InlineData( " 1", " 1", false )]
		[InlineData( "-1", " 1", true )]
		[InlineData( " 1", "-1", false )]
		[InlineData( "-1", "-1", false )]
		[InlineData( " 0.5", " 0.5", false )]
		[InlineData( "-0.5", " 0.5", true )]
		[InlineData( " 0.5", "-0.5", false )]
		[InlineData( "-0.5", "-0.5", false )]
		[InlineData( " 0.33", " 0.5", true )]
		[InlineData( "-0.33", " 0.5", true )]
		[InlineData( " 0.33", "-0.5", false )]
		[InlineData( "-0.33", "-0.5", false )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Real.Parse( l ) < Real.Parse( r ) );

		[Theory]
		[InlineData( "0.01", "0.011", true )]
		[InlineData( "0.011", "0.01", false )]
		[InlineData( "1111", "100", false )]
		public void Normalization( string l, string r, bool x ) =>
			Assert.Equal( x, Real.Parse( l ) < Real.Parse( r ) );
	}

	public class RealGreaterThanOrEqual {
		[Theory]
		[InlineData( " 0", " 1", false )]
		[InlineData( " 1", " 0", true )]
		[InlineData( " 0", "-1", true )]
		[InlineData( "-1", " 0", false )]
		[InlineData( " 1", " 1", true )]
		[InlineData( "-1", " 1", false )]
		[InlineData( " 1", "-1", true )]
		[InlineData( "-1", "-1", true )]
		[InlineData( " 0.5", " 0.5", true )]
		[InlineData( "-0.5", " 0.5", false )]
		[InlineData( " 0.5", "-0.5", true )]
		[InlineData( "-0.5", "-0.5", true )]
		[InlineData( " 0.33", " 0.5", false )]
		[InlineData( "-0.33", " 0.5", false )]
		[InlineData( " 0.33", "-0.5", true )]
		[InlineData( "-0.33", "-0.5", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Real.Parse( l ) >= Real.Parse( r ) );
	}

	public class RealLessThanOrEqual {
		[Theory]
		[InlineData( " 0", " 1", true )]
		[InlineData( " 1", " 0", false )]
		[InlineData( " 0", "-1", false )]
		[InlineData( "-1", " 0", true )]
		[InlineData( " 1", " 1", true )]
		[InlineData( "-1", " 1", true )]
		[InlineData( " 1", "-1", false )]
		[InlineData( "-1", "-1", true )]
		[InlineData( " 0.5", " 0.5", true )]
		[InlineData( "-0.5", " 0.5", true )]
		[InlineData( " 0.5", "-0.5", false )]
		[InlineData( "-0.5", "-0.5", true )]
		[InlineData( " 0.33", " 0.5", true )]
		[InlineData( "-0.33", " 0.5", true )]
		[InlineData( " 0.33", "-0.5", false )]
		[InlineData( "-0.33", "-0.5", false )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Real.Parse( l ) <= Real.Parse( r ) );
	}

	public class RealInequality {
		[Theory]
		[InlineData( " 0", " 1", true )]
		[InlineData( " 1", " 0", true )]
		[InlineData( " 0", "-1", true )]
		[InlineData( "-1", " 0", true )]
		[InlineData( " 1", " 1", false )]
		[InlineData( "-1", " 1", true )]
		[InlineData( " 1", "-1", true )]
		[InlineData( "-1", "-1", false )]
		[InlineData( " 0.5", " 0.5", false )]
		[InlineData( "-0.5", " 0.5", true )]
		[InlineData( " 0.5", "-0.5", true )]
		[InlineData( "-0.5", "-0.5", false )]
		[InlineData( " 0.33", " 0.5", true )]
		[InlineData( "-0.33", " 0.5", true )]
		[InlineData( " 0.33", "-0.5", true )]
		[InlineData( "-0.33", "-0.5", true )]
		public void Sanity( string l, string r, bool x ) =>
			Assert.Equal( x, Real.Parse( l ) != Real.Parse( r ) );

		[Theory]
		[InlineData( 0, 0, false )]
		[InlineData( 0, 1, true )]
		[InlineData( 1, 0, true )]
		[InlineData( 1, 1, false )]
		[InlineData( 0, -1, true )]
		[InlineData( -1, 0, true )]
		[InlineData( -1, 1, true )]
		[InlineData( 1, -1, true )]
		[InlineData( -1, -1, false )]
		public void CheckSignificand( int l, int r, bool e ) =>
			Assert.Equal( e, new Real( l, 0 ) != new Real( r, 0 ) );

		[Theory]
		[InlineData( 0, 0, false )]
		[InlineData( 0, 1, true )]
		[InlineData( 1, 0, true )]
		[InlineData( 1, 1, false )]
		[InlineData( 0, -1, true )]
		[InlineData( -1, 0, true )]
		[InlineData( -1, 1, true )]
		[InlineData( 1, -1, true )]
		[InlineData( -1, -1, false )]
		public void CheckExponent( int l, int r, bool e ) =>
			Assert.Equal( e, new Real( 1, l ) != new Real( 1, r ) );
	}

	public class RealAddition {
		[Theory]
		[InlineData( "0", "0", "0" )]             // Sanity
		[InlineData( "1", "0", "1" )]             // Sanity
		[InlineData( "0", "1", "1" )]             // Sanity
		[InlineData( "1", "1", "2" )]             // Sanity
		[InlineData( "10", "10", "20" )]            // Sanity
		[InlineData( ".1", ".1", ".2" )]            // Sanity
		[InlineData( "12300", "45000", "57300" )]         // Sanity
		[InlineData( "0.00123", "0.000045", "0.001275" )]      // Sanity
		[InlineData( "12300", "0.000045", "12300.000045" )]  // Sanity
		[InlineData( "0.00123", "45000", "45000.00123" )]   // Sanity
		[InlineData( "-1", "0", "-1" )]            // Sanity
		[InlineData( "0", "-1", "-1" )]            // Sanity
		[InlineData( "-1", "-1", "-2" )]           // Sanity
		[InlineData( "-10", "-10", "-20" )]           // Sanity
		[InlineData( "-0.1", "-0.1", "-0.2" )]          // Sanity
		[InlineData( "-12300", "-45000", "-57300" )]        // Sanity
		[InlineData( "-0.00123", "-0.000045", "-0.001275" )]     // Sanity
		[InlineData( "-12300", "-0.000045", "-12300.000045" )] // Sanity
		[InlineData( "-0.00123", "-45000", "-45000.00123" )]  // Sanity
		public void Sanity( string l, string r, string s ) =>
			Assert.Equal( Real.Parse( s ), Real.Parse( l ) + Real.Parse( r ) );
	}

	public class RealSubtraction {
		[Theory]
		[InlineData( "0", "0", "0" )]             // Sanity
		[InlineData( "1", "0", "1" )]             // Sanity
		[InlineData( "0", "1", "-1" )]            // Sanity
		[InlineData( "1", "1", "0" )]             // Sanity
		[InlineData( "10", "10", "0" )]             // Sanity
		[InlineData( ".1", ".1", "0" )]             // Sanity
		[InlineData( "12300", "45000", "-32700" )]        // Sanity
		[InlineData( "0.00123", "0.000045", "0.001185" )]      // Sanity
		[InlineData( "12300", "0.000045", "12299.999955" )]  // Sanity
		[InlineData( "0.00123", "45000", "-44999.99877" )]  // Sanity
		[InlineData( "-1", "0", "-1" )]            // Sanity
		[InlineData( "0", "-1", "1" )]             // Sanity
		[InlineData( "-1", "-1", "0" )]             // Sanity
		[InlineData( "-10", "-10", "0" )]             // Sanity
		[InlineData( "-0.1", "-0.1", "0" )]             // Sanity
		[InlineData( "-12300", "-45000", "32700" )]         // Sanity
		[InlineData( "-0.00123", "-0.000045", "-0.001185" )]     // Sanity
		[InlineData( "-12300", "-0.000045", "-12299.999955" )] // Sanity
		[InlineData( "-0.00123", "-45000", "44999.99877" )]   // Sanity
		public void Sanity( string l, string r, string s ) =>
			Assert.Equal( Real.Parse( s ), Real.Parse( l ) - Real.Parse( r ) );
	}

	public class RealMultiply {
		[Theory]
		[InlineData( "0", "0", "0" )]      // Sanity
		[InlineData( "0", "1", "0" )]      // Sanity
		[InlineData( "1", "0", "0" )]      // Sanity
		[InlineData( "1", "1", "1" )]      // Sanity
		[InlineData( "-1", "1", "-1" )]      // Sanity
		[InlineData( "1", "-1", "-1" )]      // Sanity
		[InlineData( "-1", "-1", "1" )]      // Sanity
		[InlineData( "1", "42", "42" )]        // Sanity
		[InlineData( "1", "3.14", "3.14" )]        // Sanity
		[InlineData( "1", "0.056", "0.056" )]        // Sanity
		[InlineData( "42", "1", "42" )]        // Sanity
		[InlineData( "3.14", "1", "3.14" )]        // Sanity
		[InlineData( "0.056", "1", "0.056" )]        // Sanity
		[InlineData( "3.14159265", "2.7182818", "8.53973412350877" )]
		[InlineData( "1234567890", "987654321", "1219326311126352690" )]
		[InlineData( ".123456789", ".0987654321", "0.0121932631112635269" )]
		[InlineData( "1234567890", ".0987654321", "121932631.112635269" )]
		[InlineData( "2e100", "1e-100", "2" )]
		[InlineData( "2e101", "1e-100", "20" )]
		[InlineData( "2e100", "1e-101", "0.2" )]
		public void Sanity( string l, string r, string p ) =>
			Assert.Equal( Real.Parse( p ), Real.Parse( l ) * Real.Parse( r ) );

		[Fact]
		public void Inverse() =>
			Assert.Equal(
				Real.Parse( "1" ),
				Real.Parse( "0.5" ) * Real.Parse( "2" )
			);

		[Fact]
		public void Associtivity() =>
			Assert.Equal(
				( Real.Parse( "0.5" ) * Real.Parse( "0.25" ) ) * Real.Parse( "0.125" ),
				Real.Parse( "0.5" ) * ( Real.Parse( "0.25" ) * Real.Parse( "0.125" ) )
			);

		[Fact]
		public void Communtivity() =>
			Assert.Equal(
				Real.Parse( "0.5" ) * Real.Parse( "0.25" ),
				Real.Parse( "0.25" ) * Real.Parse( "0.5" )
			);

		[Fact]
		public void Distributive() =>
			Assert.Equal(
				( Real.Parse( "0.5" ) * Real.Parse( "0.25" ) ) + ( Real.Parse( "0.5" ) * Real.Parse( "0.125" ) ),
				Real.Parse( "0.5" ) * ( Real.Parse( "0.25" ) + Real.Parse( "0.125" ) )
			);
	}

	public class RealDivision {
		[Theory]
		[InlineData( " 0.5", " 1", " 0.5" )]      // Sanity
		[InlineData( "-0.5", " 1", "-0.5" )]      // Sanity
		[InlineData( " 0.5", "-1", "-0.5" )]      // Sanity
		[InlineData( "-0.5", "-1", " 0.5" )]      // Sanity
		[InlineData( " 1  ", " 0.5", " 2" )]        // Sanity
		[InlineData( "-1  ", " 0.5", "-2" )]        // Sanity
		[InlineData( " 1  ", "-0.5", "-2" )]        // Sanity
		[InlineData( "-1  ", "-0.5", " 2" )]        // Sanity
		[InlineData( " 0", " 0.5", " 0" )]        // Sanity
		[InlineData( " 0", "-0.5", " 0" )]        // Sanity
		[InlineData( " 0.5", " 0.5", " 1" )]        // Sanity
		[InlineData( "-0.5", " 0.5", "-1" )]        // Sanity
		[InlineData( " 0.5", "-0.5", "-1" )]        // Sanity
		[InlineData( "-0.5", "-0.5", " 1" )]        // Sanity
		[InlineData( " 1", " 2", " 0.5" )]      // Sanity
		[InlineData( "-1", " 2", "-0.5" )]      // Sanity
		[InlineData( " 1", "-2", "-0.5" )]      // Sanity
		[InlineData( "-1", "-2", " 0.5" )]      // Sanity
		public void Sanity( string dividend, string divisor, string quotient ) =>
			Assert.Equal( Real.Parse( quotient ), Real.Parse( dividend ) / Real.Parse( divisor ) );

		[Fact]
		public void DivideByZero() =>
			Assert.Throws<System.DivideByZeroException>( () => Real.Unit / Real.Zero );

		[Theory]
		[InlineData( " 1", "3", "0.333333333333333333333333333333" )]
		[InlineData( "10", "3", "3.33333333333333333333333333333" )]
		[InlineData( " 0.1", "3", "0.0333333333333333333333333333333" )]
		public void DefaultPreceision( string dividend, string divisor, string quotient ) =>
			Assert.Equal( Real.Parse( quotient ), Real.Parse( dividend ) / Real.Parse( divisor ) );

		[Theory]
		[InlineData( " 2", "3", "0.666666666666666666666666666667" )]
		[InlineData( "20", "3", "6.66666666666666666666666666667" )]
		[InlineData( " 0.2", "3", "0.0666666666666666666666666666667" )]
		[InlineData( "10", "11", "0.90909090909090909090909090909" )]
		public void Rounding( string dividend, string divisor, string quotient ) =>
			Assert.Equal( Real.Parse( quotient ), Real.Parse( dividend ) / Real.Parse( divisor ) );
	}

	public class RealModulo {
		[Theory]
		[InlineData( " 0.5", " 1", " 0.5" )]      // Sanity
		[InlineData( "-0.5", " 1", "-0.5" )]      // Sanity
		[InlineData( " 0.5", "-1", " 0.5" )]      // Sanity
		[InlineData( "-0.5", "-1", "-0.5" )]      // Sanity
		[InlineData( " 1  ", " 0.5", " 0" )]        // Sanity
		[InlineData( "-1  ", " 0.5", " 0" )]        // Sanity
		[InlineData( " 1  ", "-0.5", " 0" )]        // Sanity
		[InlineData( "-1  ", "-0.5", " 0" )]        // Sanity
		[InlineData( " 0", " 0.5", " 0" )]        // Sanity
		[InlineData( " 0", "-0.5", " 0" )]        // Sanity
		[InlineData( " 3.8", " 1.3", " 1.2" )]     // multiple bits
		[InlineData( "-3.8", " 1.3", "-1.2" )]     // multiple bits
		[InlineData( " 3.8", "-1.3", " 1.2" )]     // multiple bits
		[InlineData( "-3.8", "-1.3", "-1.2" )]     // multiple bits
		public void Sanity( string dividend, string divisor, string remainder ) =>
			Assert.Equal(
				Real.Parse( remainder ),
				Real.Parse( dividend ) % Real.Parse( divisor )
			);

		[Fact]
		public void DivideByZero() =>
			Assert.Throws<System.DivideByZeroException>( () => Real.Unit % Real.Zero );

		[Fact]
		public void Inverse() =>
			Assert.Equal(
				Real.Parse( "0" ),
				Real.Parse( "0.5" ) % Real.Parse( "0.5" )
			);
	}

	public class RealDivisionModulo {
		[Theory]
		[InlineData( " 0.5", " 1", " 0.5", " 0.5" )]      // Sanity
		[InlineData( "-0.5", " 1", "-0.5", "-0.5" )]      // Sanity
		[InlineData( " 0.5", "-1", "-0.5", " 0.5" )]      // Sanity
		[InlineData( "-0.5", "-1", " 0.5", "-0.5" )]      // Sanity
		[InlineData( " 1  ", " 0.5", " 2", " 0" )]        // Sanity
		[InlineData( "-1  ", " 0.5", "-2", " 0" )]        // Sanity
		[InlineData( " 1  ", "-0.5", "-2", " 0" )]        // Sanity
		[InlineData( "-1  ", "-0.5", " 2", " 0" )]        // Sanity
		[InlineData( " 0", " 0.5", " 0", " 0" )]        // Sanity
		[InlineData( " 0", "-0.5", " 0", " 0" )]        // Sanity
		[InlineData( " 3.8", " 1.3", " 2.923076923076923076923076923076", " 1.2" )]     // multiple bits
		[InlineData( "-3.8", " 1.3", "-2.923076923076923076923076923076", "-1.2" )]     // multiple bits
		[InlineData( " 3.8", "-1.3", "-2.923076923076923076923076923076", " 1.2" )]     // multiple bits
		[InlineData( "-3.8", "-1.3", " 2.923076923076923076923076923076", "-1.2" )]     // multiple bits
		public void Sanity( string dividend, string divisor, string quotient, string remainder ) =>
			Assert.Equal(
				new Tuple<Real, Real>( Real.Parse( quotient ), Real.Parse( remainder ) ),
				Real.op_DividePercent( Real.Parse( dividend ), Real.Parse( divisor ) )
			);

		[Fact]
		public void DivideByZero() =>
			Assert.Throws<System.DivideByZeroException>( () => Real.op_DividePercent( Real.Unit, Real.Zero ) );

		[Fact]
		public void Inverse() =>
			Assert.Equal(
				new Tuple<Real, Real>( Real.Parse( "1" ), Real.Parse( "0" ) ),
				Real.op_DividePercent( Real.Parse( "0.5" ), Real.Parse( "0.5" ) )
			);
	}

	public class RealNegation {
		[Theory]
		[InlineData( "1", "-1" )] // Sanity
		[InlineData( "-1", " 1" )] // Sanity
		[InlineData( "0", " 0" )] // Sanity
		public void Sanity( string l, string r ) =>
			Assert.Equal(
				Real.Parse( l ),
				-Real.Parse( r )
			);
	}

	public class RealEquals {
		[Theory]
		[InlineData( 1, 1, true )]    // Sanity
		[InlineData( 1, -1, false )]   // Sanity
		[InlineData( -1, 1, false )]   // Sanity
		[InlineData( -1, -1, true )]    // Sanity
		public void Sanity( int l, int r, bool e ) =>
			Assert.Equal(
				e,
				new Real( new Integer( l ), Integer.Unit ).Equals( new Real( new Integer( r ), Integer.Unit ) )
			);

		[Fact]
		public void NaturalEquals() =>
			Assert.True( Real.Unit.Equals( Natural.Unit ) );

		[Fact]
		public void NaturalNotEquals() =>
			Assert.False( Real.Unit.Equals( Natural.Zero ) );

		[Fact]
		public void NaturalSignNotEquals() =>
			Assert.False( ( -Real.Unit ).Equals( Natural.Unit ) );

		[Fact]
		public void IntegerEquals() =>
			Assert.True( Real.Unit.Equals( Integer.Unit ) );

		[Fact]
		public void IntegerNotEquals() =>
			Assert.False( Real.Unit.Equals( Integer.Zero ) );

		[Fact]
		public void IntegerSignNotEquals() =>
			Assert.False( ( -Real.Unit ).Equals( Integer.Unit ) );

		[Fact]
		public void RationalEquals() =>
			Assert.True( Real.Unit.Equals( Rational.Unit ) );

		[Fact]
		public void RationalNotEquals() =>
			Assert.False( Real.Unit.Equals( Rational.Zero ) );

		[Fact]
		public void RationalSignNotEquals() =>
			Assert.False( ( -Real.Unit ).Equals( Rational.Unit ) );

		[Fact]
		public void RationalConversionEquals() =>
			Assert.True( Real.Parse( "0.5" ).Equals( new Rational( 1, 2u ) ) );

		[Fact]
		public void RationalConversionEqualsWithRepeating() =>
			Assert.True( Real.Parse( "0.333333333333333333333333333333" ).Equals( new Rational( 1, 3u ) ) );
	}

	public class RealToString {
		[Theory]
		[InlineData( 0, 0, "0" )]
		[InlineData( 1, 0, "1" )]
		[InlineData( -1, 0, "-1" )]
		[InlineData( 1, 1, "10" )]
		[InlineData( -1, 1, "-10" )]
		[InlineData( 1, -1, "0.1" )]
		[InlineData( -1, -1, "-0.1" )]
		[InlineData( 12345, 0, "12345" )]
		[InlineData( -12345, 0, "-12345" )]
		[InlineData( 12345, -3, "12.345" )]
		[InlineData( -12345, -3, "-12.345" )]
		[InlineData( 12000, 0, "12000" )]
		[InlineData( -12000, 0, "-12000" )]
		public void Sanity( int s, int e, string expected ) =>
			Assert.Equal( expected, new Real( s, e ).ToString() );

		[Fact]
		public void SimplePositiveLarge() =>
			Assert.Equal( "11111111111111111111111111", Real.Parse( "11111111111111111111111111" ).ToString() );

		[Fact]
		public void SimpleNegativeLarge() =>
			Assert.Equal( "-11111111111111111111111111", Real.Parse( "-11111111111111111111111111" ).ToString() );

		[Fact]
		public void SimplePositiveLargeRounded() =>
			Assert.Equal( "77777777777777777777777777", Real.Parse( "77777777777777777777777777" ).ToString() );

		[Fact]
		public void SimpleNegativeLargeRounded() =>
			Assert.Equal( "-77777777777777777777777777", Real.Parse( "-77777777777777777777777777" ).ToString() );

		[Fact]
		public void SimplePositiveSmall() =>
			Assert.Equal( "0.00000000000000000000000011111111111111111111111111", Real.Parse( "0.00000000000000000000000011111111111111111111111111" ).ToString() );

		[Fact]
		public void SimpleNegativeSmall() =>
			Assert.Equal( "-0.00000000000000000000000011111111111111111111111111", Real.Parse( "-0.00000000000000000000000011111111111111111111111111" ).ToString() );
	}

	public class RealParse {
		[Theory]
		[InlineData( "4321", 4321, 0 )]        // Sanity
		[InlineData( "43.21", 4321, -2 )]        // Sanity
		[InlineData( "432100", 4321, 2 )]        // Sanity
		[InlineData( "0.04321", 4321, -5 )]        // Sanity
		[InlineData( ".04321", 4321, -5 )]        // Sanity
		[InlineData( "-4321", -4321, 0 )]        // Sanity
		[InlineData( "-43.21", -4321, -2 )]        // Sanity
		[InlineData( "-432100", -4321, 2 )]        // Sanity
		[InlineData( "-0.04321", -4321, -5 )]        // Sanity
		[InlineData( "-.04321", -4321, -5 )]        // Sanity
		[InlineData( "4.321e3", 4321, 0 )]        // Sanity
		[InlineData( "4.32e3", 432, 1 )]        // Sanity
		[InlineData( "43.21e-3", 4321, -5 )]        // Sanity
		[InlineData( "43.2e-1", 432, -2 )]        // Sanity
		public void Sanity( string str, int n, int d ) =>
			Assert.Equal( new Real( n, d ), Real.Parse( str ) );

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
				Record.Exception( () => Real.Parse( str ) )
			);

		[Theory]
		[InlineData( "1.-2" )]
		[InlineData( "1.2.3" )]
		public void FormatEx( string str ) =>
			Assert.IsType<System.FormatException>(
				Record.Exception( () => Real.Parse( str ) )
			);
	}
}
