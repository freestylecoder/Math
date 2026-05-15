using System;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

// This type allows the tests to have direct access to the INumberBase tings
file class NBDirect {
	// Properties
	public static T One<T>() where T : System.Numerics.INumberBase<T> =>
		T.One;
	public static T Zero<T>() where T : System.Numerics.INumberBase<T> =>
		T.Zero;
	public static int Radix<T>() where T : System.Numerics.INumberBase<T> =>
		T.Radix;

	// Property Checkers
	public static bool IsCanonical<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsCanonical( value );
	public static bool IsComplexNumber<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsComplexNumber( value );
	public static bool IsFinite<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsFinite( value );
	public static bool IsImaginaryNumber<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsImaginaryNumber( value );
	public static bool IsInfinity<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsInfinity( value );
	public static bool IsInteger<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsInteger( value );
	public static bool IsNaN<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsNaN( value );
	public static bool IsNegative<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsNegative( value );
	public static bool IsNegativeInfinity<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsNegativeInfinity( value );
	public static bool IsNormal<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsNormal( value );
	public static bool IsPositive<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsPositive( value );
	public static bool IsPositiveInfinity<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsPositiveInfinity( value );
	public static bool IsRealNumber<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsRealNumber( value );
	public static bool IsSubnormal<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsSubnormal( value );

	// Unary operators/checkers
	public static T Abs<T>( T value ) where T : System.Numerics.INumberBase<T> => T.Abs( value );
	public static bool IsEvenInteger<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsEvenInteger( value );
	public static bool IsOddInteger<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsOddInteger( value );
	public static bool IsZero<T>( T value ) where T : System.Numerics.INumberBase<T> => T.IsZero( value );

	// Binary operators/checkers
	public static T MaxMagnitude<T>( T x, T y ) where T : System.Numerics.INumberBase<T> => T.MaxMagnitude( x, y );
	public static T MaxMagnitudeNumber<T>( T x, T y ) where T : System.Numerics.INumberBase<T> => T.MaxMagnitudeNumber( x, y );
	public static T MinMagnitude<T>( T x, T y ) where T : System.Numerics.INumberBase<T> => T.MinMagnitude( x, y );
	public static T MinMagnitudeNumber<T>( T x, T y ) where T : System.Numerics.INumberBase<T> => T.MinMagnitudeNumber( x, y );

	// Other stuff
	public static T CreateChecked<T, TOther>( TOther value )
		where T : System.Numerics.INumberBase<T>
		where TOther : System.Numerics.INumberBase<TOther> =>
				T.CreateChecked( value );
	public static T CreateSaturating<T, TOther>( TOther value )
		where T : System.Numerics.INumberBase<T>
		where TOther : System.Numerics.INumberBase<TOther> =>
				T.CreateSaturating( value );
	public static T CreateTruncating<T, TOther>( TOther value )
		where T : System.Numerics.INumberBase<T>
		where TOther : System.Numerics.INumberBase<TOther> =>
				T.CreateTruncating( value );
}

public class NumberBase {
	// This is just an absurdly large value that will overflow anything that could overflow
	// If you're curious how I came up with it:
	//  2^10 is roughly equal to 10^30
	//  Double.MaxValue is between 1E308 and 2E308
	//  Take a number slightly bigger that 308 (I use 312)
	//  Divide by 3 (104)
	//  Multiply by 10 (1040)
	//  Double it, just ot be sure (2080)
	//  Smooth it to a power of 32, because I know how <<< works (2048)
	private static readonly Natural OverflowValue = Natural.Unit << 2_048;

	// Properties
	[Fact] public void One() => Assert.Equal( new Natural( 1UL ), NBDirect.One<Natural>() );
	[Fact] public void Radix() => Assert.Equal( 2, NBDirect.Radix<Natural>() );
	[Fact] public void Zero() => Assert.Equal( new Natural( 0UL ), NBDirect.Zero<Natural>() );

	// These all return const values based on the nature of a Natural number
	[Fact] public void IsCanonical() => Assert.True( NBDirect.IsCanonical( Natural.Unit ) );
	[Fact] public void IsComplexNumber() => Assert.False( NBDirect.IsComplexNumber( Natural.Unit ) );
	[Fact] public void IsFinite() => Assert.True( NBDirect.IsFinite( Natural.Unit ) );
	[Fact] public void IsImaginaryNumber() => Assert.False( NBDirect.IsImaginaryNumber( Natural.Unit ) );
	[Fact] public void IsInfinity() => Assert.False( NBDirect.IsInfinity( Natural.Unit ) );
	[Fact] public void IsInteger() => Assert.True( NBDirect.IsInteger( Natural.Unit ) );
	[Fact] public void IsNaN() => Assert.False( NBDirect.IsNaN( Natural.Unit ) );
	[Fact] public void IsNegative() => Assert.False( NBDirect.IsNegative( Natural.Unit ) );
	[Fact] public void IsNegativeInfinity() => Assert.False( NBDirect.IsNegativeInfinity( Natural.Unit ) );
	[Fact] public void IsNormal() => Assert.True( NBDirect.IsNormal( Natural.Unit ) );
	[Fact] public void IsPositive() => Assert.True( NBDirect.IsPositive( Natural.Unit ) );
	[Fact] public void IsPositiveInfinity() => Assert.False( NBDirect.IsPositiveInfinity( Natural.Unit ) );
	[Fact] public void IsRealNumber() => Assert.True( NBDirect.IsRealNumber( Natural.Unit ) );
	[Fact] public void IsSubnormal() => Assert.False( NBDirect.IsSubnormal( Natural.Unit ) );

	[Fact] public void Abs() => Assert.Equal( Natural.Unit, NBDirect.Abs( Natural.Unit ) );
	[Fact] public void IsEven() => Assert.True( NBDirect.IsEvenInteger( new Natural( 2UL ) ) );
	[Fact] public void IsEvenZero() => Assert.True( NBDirect.IsEvenInteger( Natural.Zero ) );
	[Fact] public void IsNotEven() => Assert.False( NBDirect.IsEvenInteger( Natural.Unit ) );
	[Fact] public void IsOdd() => Assert.True( NBDirect.IsOddInteger( Natural.Unit ) );
	[Fact] public void IsOddZero() => Assert.False( NBDirect.IsOddInteger( Natural.Zero ) );
	[Fact] public void IsNotOdd() => Assert.False( NBDirect.IsOddInteger( new Natural( 2UL ) ) );
	[Fact] public void IsZero() => Assert.False( NBDirect.IsZero( Natural.Unit ) );
	[Fact] public void IsZeroZero() => Assert.True( NBDirect.IsZero( Natural.Zero ) );

	[Theory]
	[InlineData( 1UL, 1UL, 1UL )]
	[InlineData( 0UL, 1UL, 1UL )]
	[InlineData( 1UL, 0UL, 1UL )]
	[InlineData( 0UL, 0UL, 0UL )]
	public void MaxMagnitude( ulong x, ulong y, ulong expected ) =>
		Assert.Equal( new Natural( expected ), NBDirect.MaxMagnitude( new Natural( x ), new Natural( y ) ) );

	[Theory]
	[InlineData( 1UL, 1UL, 1UL )]
	[InlineData( 0UL, 1UL, 1UL )]
	[InlineData( 1UL, 0UL, 1UL )]
	[InlineData( 0UL, 0UL, 0UL )]
	public void MaxMagnitudeNumber( ulong x, ulong y, ulong expected ) =>
		Assert.Equal( new Natural( expected ), NBDirect.MaxMagnitudeNumber( new Natural( x ), new Natural( y ) ) );

	[Theory]
	[InlineData( 1UL, 1UL, 1UL )]
	[InlineData( 0UL, 1UL, 0UL )]
	[InlineData( 1UL, 0UL, 0UL )]
	[InlineData( 0UL, 0UL, 0UL )]
	public void MinMagnitude( ulong x, ulong y, ulong expected ) =>
		Assert.Equal( new Natural( expected ), NBDirect.MinMagnitude( new Natural( x ), new Natural( y ) ) );

	[Theory]
	[InlineData( 1UL, 1UL, 1UL )]
	[InlineData( 0UL, 1UL, 0UL )]
	[InlineData( 1UL, 0UL, 0UL )]
	[InlineData( 0UL, 0UL, 0UL )]
	public void MinMagnitudeNumber( ulong x, ulong y, ulong expected ) =>
		Assert.Equal( new Natural( expected ), NBDirect.MinMagnitudeNumber( new Natural( x ), new Natural( y ) ) );

	/*
        This set of CreateChecked basically test the TryConvertFromX methods in Natural
    */

	[Fact]
	public void CreateChecked() {
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, byte>( (byte)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, ushort>( (ushort)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, uint>( (uint)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, ulong>( (ulong)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, UInt128>( new UInt128( 0UL, 1UL ) ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, Natural>( new Natural( 1u ) ) );
	}

	[Fact]
	public void CreateCheckedNonNative() {
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, sbyte>( (sbyte)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, short>( (short)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, int>( 1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, long>( 1L ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, Int128>( new Int128( 0UL, 1UL ) ) );

		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, float>( 1.0f ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, double>( 1.0 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, decimal>( 1.0m ) );

		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, System.Numerics.Complex>( new System.Numerics.Complex( 1, 0 ) ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural, System.Numerics.BigInteger>( new System.Numerics.BigInteger( 1 ) ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateCheckedNonNative() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural,Integer>( new Integer( 1 ) ) );
		//Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural,Rational>( new Rational( 1, 1u ) ) );
		//Assert.Equal( Natural.Unit, NBDirect.CreateChecked<Natural,Real>( new Real( 1, 0 ) ) );
	}

	[Fact]
	public void CreateCheckedNonNativeNegative() {
		static OverflowException AssertExc<T>( T value ) where T : System.Numerics.INumberBase<T> =>
			Assert.IsType<OverflowException>(
				Record.Exception(
					() => NBDirect.CreateChecked<Natural, T>( value )
				)
			);

		AssertExc<sbyte>( (sbyte)-1 );
		AssertExc<short>( (short)-1 );
		AssertExc<int>( -1 );
		AssertExc<long>( -1L );
		AssertExc<Int128>( Int128.NegativeOne );

		AssertExc<float>( -1.0f );
		AssertExc<double>( -1.0 );
		AssertExc<decimal>( -1.0m );

		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( -1.0, 0 ) );
		AssertExc<System.Numerics.BigInteger>( System.Numerics.BigInteger.MinusOne );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateCheckedNonNativeNegative() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//AssertExc<Integer>( new Integer( -1 ) );
		//AssertExc<Rational>( new Rational( -1, 1u ) );
		//AssertExc<Real>( new Real( -1, 0 ) );
	}

	[Fact]
	public void CreateCheckedNonNativeDecimal() {
		static OverflowException AssertExc<T>( T value ) where T : System.Numerics.INumberBase<T> =>
			Assert.IsType<OverflowException>(
				Record.Exception(
					() => NBDirect.CreateChecked<Natural, T>( value )
				)
			);

		AssertExc<float>( 1.1f );
		AssertExc<double>( 1.1 );
		AssertExc<decimal>( 1.1m );
		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( 1.1, 0 ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateCheckedNonNativeDecimal() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//AssertExc<Rational>( new Rational( 1, 2u ) );
		//AssertExc<Real>( new Real( 1, -1 ) );
	}

	[Fact]
	public void CreateCheckedNonNativeWeird() {
		static OverflowException AssertExc<T>( T value ) where T : System.Numerics.INumberBase<T> =>
			Assert.IsType<OverflowException>(
				Record.Exception(
					() => NBDirect.CreateChecked<Natural, T>( value )
				)
			);

		AssertExc<double>( Double.PositiveInfinity );
		AssertExc<double>( Double.NegativeInfinity );
		AssertExc<double>( Double.NaN );
		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( 1, 1 ) );
		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( 0, 1 ) );
	}

	[Fact]
	public void CreateSaturating() {
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, byte>( (byte)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, ushort>( (ushort)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, uint>( (uint)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, ulong>( (ulong)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, UInt128>( new UInt128( 0UL, 1UL ) ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, Natural>( new Natural( 1u ) ) );
	}

	[Fact]
	public void CreateSaturatingNonNative() {
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, sbyte>( (sbyte)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, short>( (short)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, int>( (int)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, long>( (long)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, Int128>( new Int128( 0UL, 1UL ) ) );

		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, float>( 1.0f ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, double>( 1.0 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, decimal>( 1.0m ) );

		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, System.Numerics.Complex>( new System.Numerics.Complex( 1, 0 ) ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural, System.Numerics.BigInteger>( new System.Numerics.BigInteger( 1 ) ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateSaturatingNonNative() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural,Integer>( new Integer( 1 ) ) );
		//Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural,Rational>( new Rational( 1, 1u ) ) );
		//Assert.Equal( Natural.Unit, NBDirect.CreateSaturating<Natural,Real>( new Real( 1, 0 ) ) );
	}

	[Fact]
	public void CreateSaturatingNonNativeNegative() {
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, sbyte>( (sbyte)-1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, short>( (short)-1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, int>( (int)-1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, long>( (long)-1L ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, Int128>( Int128.NegativeOne ) );

		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, float>( -1.0f ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, double>( -1.0 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, decimal>( -1.0m ) );

		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, System.Numerics.Complex>( new System.Numerics.Complex( -1.0, 0 ) ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, System.Numerics.BigInteger>( System.Numerics.BigInteger.MinusOne ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateSaturatingNonNativeNegative() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural,Integer>( new Integer( -1 ) ) );
		//Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural,Rational>( new Rational( -1, 1u ) ) );
		//Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural,Real>( new Real( -1, 0 ) ) );
	}

	[Fact]
	public void CreateSaturatingNonNativeDecimal() {
		static OverflowException AssertExc<T>( T value ) where T : System.Numerics.INumberBase<T> =>
		Assert.IsType<OverflowException>(
			Record.Exception(
				() => NBDirect.CreateSaturating<Natural, T>( value )
			)
		);

		AssertExc<float>( 1.1f );
		AssertExc<double>( 1.1 );
		AssertExc<decimal>( 1.1m );
		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( 1.1, 0 ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateSaturatingNonNativeDecimal() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//AssertExc<Rational>( new Rational( 1, 2u ) );
		//AssertExc<Real>( new Real( 1, -1 ) );
	}

	[Fact]
	public void CreateSaturatingNonNativeNegativeDecimal() {
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, float>( -1.1f ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, double>( -1.1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, decimal>( -1.1m ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, System.Numerics.Complex>( ( new System.Numerics.Complex( -1.1, 0 ) ) ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateSaturatingNonNativeNegativeDecimal() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural,Rational>( new Rational( -1, 2u ) ) );
		//Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural,Real>( new Real( -1, -1 ) ) );
	}

	[Fact]
	public void CreateSaturatingNonNativeWeird() {
		static OverflowException AssertExc<T>( T value ) where T : System.Numerics.INumberBase<T> =>
			Assert.IsType<OverflowException>(
				Record.Exception(
					() => NBDirect.CreateSaturating<Natural, T>( value )
				)
			);

		Assert.Equal( Natural.Zero, NBDirect.CreateSaturating<Natural, double>( Double.NegativeInfinity ) );

		AssertExc<double>( Double.NaN );
		AssertExc<double>( Double.PositiveInfinity );
		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( 1, 1 ) );
		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( 0, 1 ) );
	}

	[Fact]
	public void CreateTruncating() {
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, byte>( (byte)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, ushort>( (ushort)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, uint>( (uint)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, ulong>( (ulong)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, UInt128>( new UInt128( 0UL, 1UL ) ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, Natural>( new Natural( 1u ) ) );
	}

	[Fact]
	public void CreateTruncatingNonNative() {
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, sbyte>( (sbyte)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, short>( (short)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, int>( (int)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, long>( (long)1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, Int128>( new Int128( 0UL, 1UL ) ) );

		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, float>( 1.0f ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, double>( 1.0 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, decimal>( 1.0m ) );

		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, System.Numerics.Complex>( new System.Numerics.Complex( 1, 0 ) ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, System.Numerics.BigInteger>( new System.Numerics.BigInteger( 1 ) ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateTruncatingNonNative() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural,Integer>( new Integer( 1 ) ) );
		//Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural,Rational>( new Rational( 1, 1u ) ) );
		//Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural,Real>( new Real( 1, 0 ) ) );
	}

	[Fact]
	public void CreateTruncatingNonNativeNegative() {
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, sbyte>( (sbyte)-1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, short>( (short)-1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, int>( (int)-1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, long>( (long)-1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, Int128>( Int128.NegativeOne ) );

		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, float>( -1.0f ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, double>( -1.0 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, decimal>( -1.0m ) );

		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, System.Numerics.Complex>( new System.Numerics.Complex( -1.0, 0 ) ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, System.Numerics.BigInteger>( new System.Numerics.BigInteger( -1 ) ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateTruncatingNonNativeNegative() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural,Integer>( new Integer( -1 ) );
		//Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural,Rational>( new Rational( -1, 1u ) );
		//Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural,Real>
	}

	[Fact]
	public void CreateTruncatingNonNativeDecimal() {
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, float>( 1.1f ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, double>( 1.1 ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, decimal>( 1.1m ) );
		Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural, System.Numerics.Complex>( new System.Numerics.Complex( 1.1, 0 ) ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateTruncatingNonNativeDecimal() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural,Rational>( new Rational( 3, 2u ) ) );
		//Assert.Equal( Natural.Unit, NBDirect.CreateTruncating<Natural,Real>( new Real( 11, -1 ) ) );
	}

	[Fact]
	public void CreateTruncatingNonNativeNegativeDecimal() {
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, float>( -1.1f ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, double>( -1.1 ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, decimal>( -1.1m ) );
		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, System.Numerics.Complex>( new System.Numerics.Complex( -1.1, 0 ) ) );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateTruncatingNonNativeNegativeDecimal() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural,Rational>( new Rational( -1, 2u ) ) );
		//Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural,Real>( new Real( -11, -1 ) ) );
	}

	[Fact]
	public void CreateTruncatingNonNativeWeird() {
		static OverflowException AssertExc<T>( T value ) where T : System.Numerics.INumberBase<T> =>
			Assert.IsType<OverflowException>(
				Record.Exception(
					() => NBDirect.CreateTruncating<Natural, T>( value )
				)
			);

		Assert.Equal( Natural.Zero, NBDirect.CreateTruncating<Natural, double>( Double.NegativeInfinity ) );

		AssertExc<double>( Double.NaN );
		AssertExc<double>( Double.PositiveInfinity );
		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( 1, 1 ) );
		AssertExc<System.Numerics.Complex>( new System.Numerics.Complex( 0, 1 ) );
	}

	/*
        This set of CreateX basically test the TryConvertToX methods in Natural
    */

	[Fact]
	public void CreateCheckedFromOther() {
		static void AssertCheckedEqual<TOther>( Natural value ) where TOther : System.Numerics.INumberBase<TOther> =>
			Assert.Equal( TOther.One, TOther.CreateChecked( value ) );

		AssertCheckedEqual<Byte>( Natural.Unit );
		AssertCheckedEqual<UInt16>( Natural.Unit );
		AssertCheckedEqual<UInt32>( Natural.Unit );
		AssertCheckedEqual<UInt64>( Natural.Unit );
		AssertCheckedEqual<UInt128>( Natural.Unit );

		AssertCheckedEqual<SByte>( Natural.Unit );
		AssertCheckedEqual<Int16>( Natural.Unit );
		AssertCheckedEqual<Int32>( Natural.Unit );
		AssertCheckedEqual<Int64>( Natural.Unit );
		AssertCheckedEqual<Int128>( Natural.Unit );

		AssertCheckedEqual<Single>( Natural.Unit );
		AssertCheckedEqual<Double>( Natural.Unit );
		AssertCheckedEqual<Decimal>( Natural.Unit );

		AssertCheckedEqual<System.Numerics.Complex>( Natural.Unit );
		AssertCheckedEqual<System.Numerics.BigInteger>( Natural.Unit );

		AssertCheckedEqual<Natural>( Natural.Unit );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateCheckedFromOther() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//AssertCheckedEqual<Integer> ( Natural.Unit );
		//AssertCheckedEqual<Rational>( Natural.Unit );
		//AssertCheckedEqual<Real>    ( Natural.Unit );
	}

	[Fact]
	public void CreateCheckedFromOtherOverflow() {
		static void AssertCheckedOverflow<TOther>( Natural value ) where TOther : System.Numerics.INumberBase<TOther> =>
			Assert.IsType<OverflowException>(
				Record.Exception(
					() => TOther.CreateChecked( value )
				)
			);

		AssertCheckedOverflow<Byte>( NumberBase.OverflowValue );
		AssertCheckedOverflow<UInt16>( NumberBase.OverflowValue );
		AssertCheckedOverflow<UInt32>( NumberBase.OverflowValue );
		AssertCheckedOverflow<UInt64>( NumberBase.OverflowValue );
		AssertCheckedOverflow<UInt128>( NumberBase.OverflowValue );

		AssertCheckedOverflow<SByte>( NumberBase.OverflowValue );
		AssertCheckedOverflow<Int16>( NumberBase.OverflowValue );
		AssertCheckedOverflow<Int32>( NumberBase.OverflowValue );
		AssertCheckedOverflow<Int64>( NumberBase.OverflowValue );
		AssertCheckedOverflow<Int128>( NumberBase.OverflowValue );

		AssertCheckedOverflow<Single>( NumberBase.OverflowValue );
		AssertCheckedOverflow<Double>( NumberBase.OverflowValue );
		AssertCheckedOverflow<Decimal>( NumberBase.OverflowValue );

		AssertCheckedOverflow<System.Numerics.Complex>( NumberBase.OverflowValue );

		// All of these can't overflow
		//  1) Numerics.BigInteger
		//  2) Natural
		//  3) Integer
		//  4) Rational
		//  5) Real

		// To be fair, I'd _like_ to test that BigInteger took the big number
		// However:
		//  1) It's in the magnitude of 600 digits long
		//  2) I'd need to know the value to compute the BigInteger
	}

	[Fact]
	public void CreateSaturatingFromOther() {
		static void AssertSaturatingEqual<TOther>( Natural value ) where TOther : System.Numerics.INumberBase<TOther> =>
			Assert.Equal( TOther.One, TOther.CreateSaturating( value ) );

		AssertSaturatingEqual<Byte>( Natural.Unit );
		AssertSaturatingEqual<UInt16>( Natural.Unit );
		AssertSaturatingEqual<UInt32>( Natural.Unit );
		AssertSaturatingEqual<UInt64>( Natural.Unit );
		AssertSaturatingEqual<UInt128>( Natural.Unit );

		AssertSaturatingEqual<SByte>( Natural.Unit );
		AssertSaturatingEqual<Int16>( Natural.Unit );
		AssertSaturatingEqual<Int32>( Natural.Unit );
		AssertSaturatingEqual<Int64>( Natural.Unit );
		AssertSaturatingEqual<Int128>( Natural.Unit );

		AssertSaturatingEqual<Single>( Natural.Unit );
		AssertSaturatingEqual<Double>( Natural.Unit );
		AssertSaturatingEqual<Decimal>( Natural.Unit );

		AssertSaturatingEqual<System.Numerics.Complex>( Natural.Unit );
		AssertSaturatingEqual<System.Numerics.BigInteger>( Natural.Unit );

		AssertSaturatingEqual<Natural>( Natural.Unit );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateSaturatingFromOther() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//AssertSaturatingEqual<Integer> ( Natural.Unit )
		//AssertSaturatingEqual<Rational>( Natural.Unit )
		//AssertSaturatingEqual<Real>    ( Natural.Unit )
	}

	[Fact]
	public void CreateSaturatingFromOtherSaturating() {
		static void AssertSaturatingEqual<TOther>( Natural value, TOther expected ) where TOther : System.Numerics.INumberBase<TOther> =>
			Assert.Equal( expected, TOther.CreateSaturating( value ) );

		AssertSaturatingEqual<Byte>( NumberBase.OverflowValue, Byte.MaxValue );
		AssertSaturatingEqual<UInt16>( NumberBase.OverflowValue, UInt16.MaxValue );
		AssertSaturatingEqual<UInt32>( NumberBase.OverflowValue, UInt32.MaxValue );
		AssertSaturatingEqual<UInt64>( NumberBase.OverflowValue, UInt64.MaxValue );
		AssertSaturatingEqual<UInt128>( NumberBase.OverflowValue, UInt128.MaxValue );

		AssertSaturatingEqual<SByte>( NumberBase.OverflowValue, SByte.MaxValue );
		AssertSaturatingEqual<Int16>( NumberBase.OverflowValue, Int16.MaxValue );
		AssertSaturatingEqual<Int32>( NumberBase.OverflowValue, Int32.MaxValue );
		AssertSaturatingEqual<Int64>( NumberBase.OverflowValue, Int64.MaxValue );
		AssertSaturatingEqual<Int128>( NumberBase.OverflowValue, Int128.MaxValue );

		AssertSaturatingEqual<Single>( NumberBase.OverflowValue, Single.MaxValue );
		AssertSaturatingEqual<Double>( NumberBase.OverflowValue, Double.MaxValue );
		AssertSaturatingEqual<Decimal>( NumberBase.OverflowValue, Decimal.MaxValue );

		AssertSaturatingEqual<System.Numerics.Complex>( NumberBase.OverflowValue, new System.Numerics.Complex( Double.MaxValue, 0.0 ) );

		// All of these can't overflow
		//  1) Numerics.BigInteger
		//  2) Natural
		//  3) Integer
		//  4) Rational
		//  5) Real

		// To be fair, I'd _like_ to test that BigInteger took the big number
		// However:
		//  1) It's in the magnitude of 600 digits long
		//  2) I'd need to know the value to compute the BigInteger
	}

	[Fact]
	public void CreateTruncatingFromOther() {
		static void AssertTruncatingEqual<TOther>( Natural value ) where TOther : System.Numerics.INumberBase<TOther> =>
			Assert.Equal( TOther.One, TOther.CreateTruncating( value ) );

		AssertTruncatingEqual<Byte>( Natural.Unit );
		AssertTruncatingEqual<UInt16>( Natural.Unit );
		AssertTruncatingEqual<UInt32>( Natural.Unit );
		AssertTruncatingEqual<UInt64>( Natural.Unit );
		AssertTruncatingEqual<UInt128>( Natural.Unit );

		AssertTruncatingEqual<SByte>( Natural.Unit );
		AssertTruncatingEqual<Int16>( Natural.Unit );
		AssertTruncatingEqual<Int32>( Natural.Unit );
		AssertTruncatingEqual<Int64>( Natural.Unit );
		AssertTruncatingEqual<Int128>( Natural.Unit );

		AssertTruncatingEqual<Single>( Natural.Unit );
		AssertTruncatingEqual<Double>( Natural.Unit );
		AssertTruncatingEqual<Decimal>( Natural.Unit );

		AssertTruncatingEqual<System.Numerics.Complex>( Natural.Unit );
		AssertTruncatingEqual<System.Numerics.BigInteger>( Natural.Unit );

		AssertTruncatingEqual<Natural>( Natural.Unit );
	}

	[Fact( Skip = "Relies on future development" )]
	public void SkippedCreateTruncatingFromOther() {
		// TODO:		These are not INumberBase yet, so it's failing for that
		//			As they are developed, move into real test.
		//			Delete methos when everything is moved

		//NumberBase.AssertTruncatingEqual<Integer> ( Natural.Unit )
		//NumberBase.AssertTruncatingEqual<Rational>( Natural.Unit )
		//NumberBase.AssertTruncatingEqual<Real>    ( Natural.Unit )
	}

	[Fact]
	public void CreateTruncatingFromOtherTruncating() {
		static void AssertTruncatingEqualOne<TOther>( Natural value ) where TOther : System.Numerics.INumberBase<TOther> =>
			Assert.Equal( TOther.One, TOther.CreateTruncating( value ) );

		static void AssertTruncatingEqual<TOther>( Natural value, TOther expected ) where TOther : System.Numerics.INumberBase<TOther> =>
			Assert.Equal( expected, TOther.CreateTruncating( value ) );

		// If you're curious, that's equal to:
		// 32317006071311007300714876688669951960444102669715484032130345427524655138867890893197201411522913463688717960921898019494119559150490921095088152386448283120630877367300996091750197750389652106796057638384067568276792218642619756161838094338476170470581645852036305042887575891541065808607552399123930385521914333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230657
		// Knowing that is how I calculated the "correct" values for the floating point numbers
		Natural OverflowPlusOne = NumberBase.OverflowValue + Natural.Unit;
		System.Diagnostics.Trace.WriteLine( OverflowPlusOne );

		AssertTruncatingEqualOne<Byte>( OverflowPlusOne );
		AssertTruncatingEqualOne<UInt16>( OverflowPlusOne );
		AssertTruncatingEqualOne<UInt32>( OverflowPlusOne );
		AssertTruncatingEqualOne<UInt64>( OverflowPlusOne );
		AssertTruncatingEqualOne<UInt128>( OverflowPlusOne );

		AssertTruncatingEqualOne<SByte>( OverflowPlusOne );
		AssertTruncatingEqualOne<Int16>( OverflowPlusOne );
		AssertTruncatingEqualOne<Int32>( OverflowPlusOne );
		AssertTruncatingEqualOne<Int64>( OverflowPlusOne );
		AssertTruncatingEqualOne<Int128>( OverflowPlusOne );

		AssertTruncatingEqualOne<Decimal>( OverflowPlusOne );

		// 318478604952148193555853611059596230657
		AssertTruncatingEqual<Single>( OverflowPlusOne, Single.Parse( "318478604952148193555853611059596230657" ) );
		// 14333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230657
		AssertTruncatingEqual<Double>( OverflowPlusOne, Double.Parse( "14333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230657" ) );
		AssertTruncatingEqual<System.Numerics.Complex>( OverflowPlusOne, new System.Numerics.Complex( Double.Parse( "14333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230657" ), 0.0 ) );

		// All of these can't overflow
		//  1) Numerics.BigInteger
		//  2) Natural
		//  3) Integer
		//  4) Rational
		//  5) Real

		// To be fair, I'd _like_ to test that BigInteger took the big number
		// However:
		//  1) It's in the magnitude of 600 digits long
		//  2) I'd need to know the value to compute the BigInteger
	}
}