using System;
using System.Linq;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;
using Integer = Freestylecoding.Math.Integer;

public class ObjectGetType {
	[Fact]
	public void Sanity() =>
		Assert.Equal(
			typeof( Integer ),
			Integer.Unit.GetType()
		);
}

public class ObjectEquals {
	[Theory]
	[InlineData( 0, 0, true )]
	[InlineData( 1, 0, false )]
	[InlineData( 0, 1, false )]
	[InlineData( 1, 1, true )]
	[InlineData( -1, 0, false )]
	[InlineData( 0, -1, false )]
	[InlineData( 1, -1, false )]
	[InlineData( -1, 1, false )]
	[InlineData( -1, -1, true )]
	public void Sanity( int left, int right, bool expected ) =>
		Assert.Equal(
			expected,
			new Integer( left ).Equals( new Integer( right ) )
		);

	[Fact]
	public void LargeIntegerIntegers() {
		Assert.True(
			FListInteger( false, 0xFu, 0x00000101u ).Equals( FListInteger( false, 0xFu, 0x00000101u ) )
		);

		Assert.True(
			FListInteger( true, 0xFu, 0x00000101u ).Equals( FListInteger( true, 0xFu, 0x00000101u ) )
		);

		Assert.False(
			FListInteger( true, 0xFu, 0x00000101u ).Equals( FListInteger( false, 0xFu, 0x00000101u ) )
		);
	}

	[Fact]
	public void HardcodedTypes() {
		Assert.True( Integer.Unit.Equals( (sbyte)1 ) );

		Assert.True( Integer.Unit.Equals( (Int16)1 ) );
		Assert.True( Integer.Unit.Equals( (Int32)1 ) );
		Assert.True( Integer.Unit.Equals( (Int64)1 ) );

		Assert.True( Integer.Unit.Equals( Int128.One ) );
		Assert.True( Integer.Unit.Equals( System.Numerics.BigInteger.One ) );
		Assert.True( Integer.Unit.Equals( Natural.Unit ) );
	}

	[Fact]
	public void NegativeHardcodedTypes() {
		Assert.True( Integer.NegativeOne.Equals( (sbyte)-1 ) );

		Assert.True( Integer.NegativeOne.Equals( (Int16)( -1 ) ) );
		Assert.True( Integer.NegativeOne.Equals( (Int32)( -1 ) ) );
		Assert.True( Integer.NegativeOne.Equals( (Int64)( -1 ) ) );

		Assert.True( Integer.NegativeOne.Equals( System.Int128.NegativeOne ) );
		Assert.True( Integer.NegativeOne.Equals( System.Numerics.BigInteger.MinusOne ) );
	}

	[Fact]
	public void UnsignedTypes() {
		Assert.True( Integer.Unit.Equals( (byte)1 ) );

		Assert.True( Integer.Unit.Equals( (UInt16)1 ) );
		Assert.True( Integer.Unit.Equals( (UInt32)1 ) );
		Assert.True( Integer.Unit.Equals( (UInt64)1 ) );

		Assert.True( Integer.Unit.Equals( System.UInt128.One ) );
	}

	[Fact]
	public void FloatingTypes() {
		Assert.True( Integer.Unit.Equals( 1f ) );

		Assert.True( Integer.Unit.Equals( 1.0 ) );
		Assert.True( Integer.Unit.Equals( 1m ) );

		Assert.True( Integer.Unit.Equals( new System.Numerics.Complex( 1, 0 ) ) );
	}

	[Fact]
	public void NegativeFloatingTypes() {
		Assert.True( Integer.NegativeOne.Equals( -1f ) );

		Assert.True( Integer.NegativeOne.Equals( -1.0 ) );
		Assert.True( Integer.NegativeOne.Equals( -1m ) );

		Assert.True( Integer.NegativeOne.Equals( new System.Numerics.Complex( -1, 0 ) ) );
	}

	[Fact]
	public void NotNumber() {
		Assert.False( Integer.Unit.Equals( "1" ) );
		Assert.False( Integer.Unit.Equals( true ) );
	}

	[Fact]
	public void FloatingTypesWithDecimals() {
		Assert.False( Integer.Unit.Equals( 1.1f ) );

		Assert.False( Integer.Unit.Equals( 1.1 ) );
		Assert.False( Integer.Unit.Equals( 1.1m ) );

		Assert.False( Integer.Unit.Equals( new System.Numerics.Complex( 1.1, 0 ) ) );
	}

	[Fact]
	public void Imaginary() =>
		// I'm making an assumption that an imaginary number is not an integer
		// This explicitly checks that assumption
		Assert.False( Integer.Unit.Equals( new System.Numerics.Complex( 1, 1 ) ) );
}

public class ObjectGetHashCode {
	[Fact]
	public void Sanity() {
		Assert.Equal(
			Integer.Unit.GetHashCode(),
			Integer.Unit.GetHashCode()
		);

		Assert.NotEqual(
			Integer.Zero.GetHashCode(),
			Integer.Unit.GetHashCode()
		);

		Assert.NotEqual(
			Integer.NegativeOne.GetHashCode(),
			Integer.Unit.GetHashCode()
		);
	}
}

public class ObjectToString {
	[Theory]
	[InlineData( 0, "0" )] // Sanity
	[InlineData( 1, "1" )] // Sanity
	[InlineData( 123, "123" )] // multiple bits
	[InlineData( 45678, "45678" )] // rev
	[InlineData( -1, "-1" )] // Sanity
	[InlineData( -123, "-123" )] // multiple bits
	[InlineData( -45678, "-45678" )] // rev
	public void Sanity( int actual, string expected ) =>
		Assert.Equal(
			expected,
			new Integer( actual ).ToString()
		);

	[Fact]
	public void Bigger() =>
		Assert.Equal(
			"1234567890123456789",
			FListInteger( 0x112210F4u, 0x7DE98115u ).ToString()
		);

	[Fact]

	public void BiggerNegative() =>
		Assert.Equal(
			"-1234567890123456789",
			FListInteger( true, 0x112210F4u, 0x7DE98115u ).ToString()
		);
}

public class ToStringFormat {
	static readonly System.Numerics.BigInteger  SmallBI =  SmallInteger;
	static readonly System.Numerics.BigInteger MediumBI = MediumInteger;
	static readonly System.Numerics.BigInteger  LargeBI =  LargeInteger;

	static readonly System.Numerics.BigInteger  SmallNegativeBI =  SmallNegativeInteger;
	static readonly System.Numerics.BigInteger MediumNegativeBI = MediumNegativeInteger;
	static readonly System.Numerics.BigInteger  LargeNegativeBI =  LargeNegativeInteger;

	[Fact]
	public void Sanity() =>
		Assert.Equal(
			"1",
			Integer.Unit.ToString( null )
		);

	[Theory]
	[InlineData( "B" )]
	[InlineData( "C" )]
	[InlineData( "D" )]
	[InlineData( "E" )]
	[InlineData( "F" )]
	[InlineData( "G" )]
	[InlineData( "N" )]
	[InlineData( "P" )]
	[InlineData( "R" )]
	[InlineData( "X" )]
	[InlineData( "b" )]
	[InlineData( "c" )]
	[InlineData( "d" )]
	[InlineData( "e" )]
	[InlineData( "f" )]
	[InlineData( "g" )]
	[InlineData( "n" )]
	[InlineData( "p" )]
	[InlineData( "r" )]
	[InlineData( "x" )]
	public void StandardFormatSpecifiers( string format ) =>
		Assert.Equal(
			0x4996_02D2.ToString( format ),
			SmallInteger.ToString( format )
		);

	[Fact]
	public void GeneralAndRoundTrip() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		Assert.Equal( SmallString, SmallInteger.ToString( "G" ) );
		Assert.Equal( SmallString, SmallInteger.ToString( "g" ) );
		Assert.Equal( SmallString, SmallInteger.ToString( "R" ) );
		Assert.Equal( SmallString, SmallInteger.ToString( "r" ) );

		Assert.Equal( MediumString, MediumInteger.ToString( "G" ) );
		Assert.Equal( MediumString, MediumInteger.ToString( "g" ) );
		Assert.Equal( MediumString, MediumInteger.ToString( "R" ) );
		Assert.Equal( MediumString, MediumInteger.ToString( "r" ) );

		Assert.Equal( LargeString, LargeInteger.ToString( "G" ) );
		Assert.Equal( LargeString, LargeInteger.ToString( "g" ) );
		Assert.Equal( LargeString, LargeInteger.ToString( "R" ) );
		Assert.Equal( LargeString, LargeInteger.ToString( "r" ) );
	}

	[Fact]
	public void GeneralAndRoundTrip_IgnorePrecision() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		foreach( int i in Enumerable.Range( 0, 20 ) ) {
			Assert.Equal( SmallString, SmallInteger.ToString( $"G{i}" ) );
			Assert.Equal( SmallString, SmallInteger.ToString( $"g{i}" ) );
			Assert.Equal( SmallString, SmallInteger.ToString( $"R{i}" ) );
			Assert.Equal( SmallString, SmallInteger.ToString( $"r{i}" ) );

			Assert.Equal( MediumString, MediumInteger.ToString( $"G{i}" ) );
			Assert.Equal( MediumString, MediumInteger.ToString( $"g{i}" ) );
			Assert.Equal( MediumString, MediumInteger.ToString( $"R{i}" ) );
			Assert.Equal( MediumString, MediumInteger.ToString( $"r{i}" ) );

			Assert.Equal( LargeString, LargeInteger.ToString( $"G{i}" ) );
			Assert.Equal( LargeString, LargeInteger.ToString( $"g{i}" ) );
			Assert.Equal( LargeString, LargeInteger.ToString( $"R{i}" ) );
			Assert.Equal( LargeString, LargeInteger.ToString( $"r{i}" ) );
		}
	}

	[Fact]
	public void NegativeGeneralAndRoundTrip() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "G" ) );
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "g" ) );
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "R" ) );
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "r" ) );

		Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( "G" ) );
		Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( "g" ) );
		Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( "R" ) );
		Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( "r" ) );

		Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( "G" ) );
		Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( "g" ) );
		Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( "R" ) );
		Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( "r" ) );
	}

	[Fact]
	public void NegativeGeneralAndRoundTrip_IgnorePrecision() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		foreach( int i in Enumerable.Range( 0, 20 ) ) {
			Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( $"G{i}" ) );
			Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( $"g{i}" ) );
			Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( $"R{i}" ) );
			Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( $"r{i}" ) );

			Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( $"G{i}" ) );
			Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( $"g{i}" ) );
			Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( $"R{i}" ) );
			Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( $"r{i}" ) );

			Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( $"G{i}" ) );
			Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( $"g{i}" ) );
			Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( $"R{i}" ) );
			Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( $"r{i}" ) );
		}
	}

	[Fact]
	public void Decimal() {
		Assert.Equal( SmallString, SmallInteger.ToString( "D" ) );
		Assert.Equal( SmallString, SmallInteger.ToString( "d" ) );

		Assert.Equal( MediumString, MediumInteger.ToString( "D" ) );
		Assert.Equal( MediumString, MediumInteger.ToString( "d" ) );

		Assert.Equal( LargeString, LargeInteger.ToString( "D" ) );
		Assert.Equal( LargeString, LargeInteger.ToString( "d" ) );
	}

	[Fact]
	public void Decimal_Precision() {
		Assert.Equal( SmallString, SmallInteger.ToString( "D0" ) );
		Assert.Equal( SmallString, SmallInteger.ToString( "D4" ) );
		Assert.Equal( SmallString, SmallInteger.ToString( "D8" ) );
		Assert.Equal( "001234567890", SmallInteger.ToString( "D12" ) );
	}

	[Fact]
	public void NegativeDecimal() {
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "D" ) );
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "d" ) );

		Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( "D" ) );
		Assert.Equal( MediumNegativeString, MediumNegativeInteger.ToString( "d" ) );

		Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( "D" ) );
		Assert.Equal( LargeNegativeString, LargeNegativeInteger.ToString( "d" ) );
	}

	[Fact]
	public void NegativeDecimal_Precision() {
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "D0" ) );
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "D4" ) );
		Assert.Equal( SmallNegativeString, SmallNegativeInteger.ToString( "D8" ) );
		Assert.Equal( "001234567890", SmallNegativeInteger.ToString( "D12" ) );
	}

	[Fact]
	public void Hexadecimal() {
		Assert.Equal( SmallBI.ToString( "X" ), SmallInteger.ToString( "X" ) );
		Assert.Equal( SmallBI.ToString( "x" ), SmallInteger.ToString( "x" ) );

		Assert.Equal( MediumBI.ToString( "X" ), MediumInteger.ToString( "X" ) );
		Assert.Equal( MediumBI.ToString( "x" ), MediumInteger.ToString( "x" ) );

		Assert.Equal( LargeBI.ToString( "X" ), LargeInteger.ToString( "X" ) );
		Assert.Equal( LargeBI.ToString( "x" ), LargeInteger.ToString( "x" ) );
	}

	[Fact]
	public void Hexadecimal_Precision() {
		Assert.Equal( SmallBI.ToString( "X0" ), SmallInteger.ToString( "X0" ) );
		Assert.Equal( SmallBI.ToString( "X4" ), SmallInteger.ToString( "X4" ) );
		Assert.Equal( SmallBI.ToString( "X8" ), SmallInteger.ToString( "X8" ) );
		Assert.Equal( SmallBI.ToString( "X12" ), SmallInteger.ToString( "X12" ) );
		Assert.Equal( SmallBI.ToString( "X16" ), SmallInteger.ToString( "X16" ) );
	}

	[Fact]
	public void NegativeHexadecimal() {
		Assert.Equal( SmallNegativeBI.ToString( "X" ), SmallNegativeInteger.ToString( "X" ) );
		Assert.Equal( SmallNegativeBI.ToString( "x" ), SmallNegativeInteger.ToString( "x" ) );

		Assert.Equal( MediumNegativeBI.ToString( "X" ), MediumNegativeInteger.ToString( "X" ) );
		Assert.Equal( MediumNegativeBI.ToString( "x" ), MediumNegativeInteger.ToString( "x" ) );

		Assert.Equal( LargeNegativeBI.ToString( "X" ), LargeNegativeInteger.ToString( "X" ) );
		Assert.Equal( LargeNegativeBI.ToString( "x" ), LargeNegativeInteger.ToString( "x" ) );
	}

	[Fact]
	public void NegativeHexadecimal_Precision() {
		Assert.Equal( SmallNegativeBI.ToString( "X0" ), SmallNegativeInteger.ToString( "X0" ) );
		Assert.Equal( SmallNegativeBI.ToString( "X4" ), SmallNegativeInteger.ToString( "X4" ) );
		Assert.Equal( SmallNegativeBI.ToString( "X8" ), SmallNegativeInteger.ToString( "X8" ) );
		Assert.Equal( SmallNegativeBI.ToString( "X12" ), SmallNegativeInteger.ToString( "X12" ) );
		Assert.Equal( SmallNegativeBI.ToString( "X16" ), SmallNegativeInteger.ToString( "X16" ) );
	}

	[Fact]
	public void Binary() {
		Assert.Equal( SmallBI.ToString( "B" ), SmallInteger.ToString( "B" ) );
		Assert.Equal( SmallBI.ToString( "b" ), SmallInteger.ToString( "b" ) );

		Assert.Equal( MediumBI.ToString( "B" ), MediumInteger.ToString( "B" ) );
		Assert.Equal( MediumBI.ToString( "b" ), MediumInteger.ToString( "b" ) );

		Assert.Equal( LargeBI.ToString( "B" ), LargeInteger.ToString( "B" ) );
		Assert.Equal( LargeBI.ToString( "b" ), LargeInteger.ToString( "b" ) );
	}

	[Fact]
	public void Binary_Precision() {
		Assert.Equal( SmallBI.ToString( "B0" ), SmallInteger.ToString( "B0" ) );
		Assert.Equal( SmallBI.ToString( "B8" ), SmallInteger.ToString( "B8" ) );
		Assert.Equal( SmallBI.ToString( "B16" ), SmallInteger.ToString( "B16" ) );
		Assert.Equal( SmallBI.ToString( "B32" ), SmallInteger.ToString( "B32" ) );
		Assert.Equal( SmallBI.ToString( "B64" ), SmallInteger.ToString( "B64" ) );
	}

	[Fact]
	public void NegativeBinary() {
		Assert.Equal( SmallNegativeBI.ToString( "B" ), SmallNegativeInteger.ToString( "B" ) );
		Assert.Equal( SmallNegativeBI.ToString( "b" ), SmallNegativeInteger.ToString( "b" ) );

		Assert.Equal( MediumNegativeBI.ToString( "B" ), MediumNegativeInteger.ToString( "B" ) );
		Assert.Equal( MediumNegativeBI.ToString( "b" ), MediumNegativeInteger.ToString( "b" ) );

		Assert.Equal( LargeNegativeBI.ToString( "B" ), LargeNegativeInteger.ToString( "B" ) );
		Assert.Equal( LargeNegativeBI.ToString( "b" ), LargeNegativeInteger.ToString( "b" ) );
	}

	[Fact]
	public void NegativeBinary_Precision() {
		Assert.Equal( SmallNegativeBI.ToString( "B0" ), SmallNegativeInteger.ToString( "B0" ) );
		Assert.Equal( SmallNegativeBI.ToString( "B8" ), SmallNegativeInteger.ToString( "B8" ) );
		Assert.Equal( SmallNegativeBI.ToString( "B16" ), SmallNegativeInteger.ToString( "B16" ) );
		Assert.Equal( SmallNegativeBI.ToString( "B32" ), SmallNegativeInteger.ToString( "B32" ) );
		Assert.Equal( SmallNegativeBI.ToString( "B64" ), SmallNegativeInteger.ToString( "B64" ) );
	}

	[Fact]
	public void Currency() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		Assert.Equal( $"$1,234,567,890{defaultDigits}", SmallInteger.ToString( "C" ) );
		Assert.Equal( $"$1,234,567,890{defaultDigits}", SmallInteger.ToString( "c" ) );

		Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", MediumInteger.ToString( "C" ) );
		Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", MediumInteger.ToString( "c" ) );

		Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeInteger.ToString( "C" ) );
		Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeInteger.ToString( "c" ) );
	}

	[Fact]
	public void Currency_Precision() {
		Assert.Equal( "$1,234,567,890", SmallInteger.ToString( "C0" ) );
		Assert.Equal( "$1,234,567,890.0000", SmallInteger.ToString( "C4" ) );
		Assert.Equal( "$1,234,567,890.00000000", SmallInteger.ToString( "C8" ) );
		Assert.Equal( "$1,234,567,890.000000000000", SmallInteger.ToString( "C12" ) );
	}

	[Fact]
	public void NegativeCurrency() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		Assert.Equal( $"$-1,234,567,890{defaultDigits}", SmallNegativeInteger.ToString( "C" ) );
		Assert.Equal( $"$-1,234,567,890{defaultDigits}", SmallNegativeInteger.ToString( "c" ) );

		Assert.Equal( $"$-12,345,678,901,234,567,890{defaultDigits}", MediumNegativeInteger.ToString( "C" ) );
		Assert.Equal( $"$-12,345,678,901,234,567,890{defaultDigits}", MediumNegativeInteger.ToString( "c" ) );

		Assert.Equal( $"$-123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNegativeInteger.ToString( "C" ) );
		Assert.Equal( $"$-123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNegativeInteger.ToString( "c" ) );
	}

	[Fact]
	public void NegativeCurrency_Precision() {
		Assert.Equal( "$-1,234,567,890", SmallNegativeInteger.ToString( "C0" ) );
		Assert.Equal( "$-1,234,567,890.0000", SmallNegativeInteger.ToString( "C4" ) );
		Assert.Equal( "$-1,234,567,890.00000000", SmallNegativeInteger.ToString( "C8" ) );
		Assert.Equal( "$-1,234,567,890.000000000000", SmallNegativeInteger.ToString( "C12" ) );
	}

	[Fact]
	public void FixedPoint() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}";

		Assert.Equal( $"1234567890{defaultDigits}", SmallInteger.ToString( "F" ) );
		Assert.Equal( $"1234567890{defaultDigits}", SmallInteger.ToString( "f" ) );

		Assert.Equal( $"12345678901234567890{defaultDigits}", MediumInteger.ToString( "F" ) );
		Assert.Equal( $"12345678901234567890{defaultDigits}", MediumInteger.ToString( "f" ) );

		Assert.Equal( $"123456789012345678901234567890{defaultDigits}", LargeInteger.ToString( "F" ) );
		Assert.Equal( $"123456789012345678901234567890{defaultDigits}", LargeInteger.ToString( "f" ) );
	}

	[Fact]
	public void FixedPoint_Precision() {
		Assert.Equal( "1234567890.", SmallInteger.ToString( "F0" ) );
		Assert.Equal( "1234567890.0000", SmallInteger.ToString( "F4" ) );
		Assert.Equal( "1234567890.00000000", SmallInteger.ToString( "F8" ) );
		Assert.Equal( "1234567890.000000000000", SmallInteger.ToString( "F12" ) );
	}

	[Fact]
	public void NegativeFixedPoint() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}";

		Assert.Equal( $"-1234567890{defaultDigits}", SmallNegativeInteger.ToString( "F" ) );
		Assert.Equal( $"-1234567890{defaultDigits}", SmallNegativeInteger.ToString( "f" ) );

		Assert.Equal( $"-12345678901234567890{defaultDigits}", MediumNegativeInteger.ToString( "F" ) );
		Assert.Equal( $"-12345678901234567890{defaultDigits}", MediumNegativeInteger.ToString( "f" ) );

		Assert.Equal( $"-123456789012345678901234567890{defaultDigits}", LargeNegativeInteger.ToString( "F" ) );
		Assert.Equal( $"-123456789012345678901234567890{defaultDigits}", LargeNegativeInteger.ToString( "f" ) );
	}

	[Fact]
	public void NegativeFixedPoint_Precision() {
		Assert.Equal( "-1234567890.", SmallNegativeInteger.ToString( "F0" ) );
		Assert.Equal( "-1234567890.0000", SmallNegativeInteger.ToString( "F4" ) );
		Assert.Equal( "-1234567890.00000000", SmallNegativeInteger.ToString( "F8" ) );
		Assert.Equal( "-1234567890.000000000000", SmallNegativeInteger.ToString( "F12" ) );
	}

	[Fact]
	public void Number() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		Assert.Equal( $"1,234,567,890{defaultDigits}", SmallInteger.ToString( "N" ) );
		Assert.Equal( $"1,234,567,890{defaultDigits}", SmallInteger.ToString( "n" ) );

		Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", MediumInteger.ToString( "N" ) );
		Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", MediumInteger.ToString( "n" ) );

		Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeInteger.ToString( "N" ) );
		Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeInteger.ToString( "n" ) );
	}

	[Fact]
	public void Number_Precision() {
		Assert.Equal( "1,234,567,890", SmallInteger.ToString( "N0" ) );
		Assert.Equal( "1,234,567,890.0000", SmallInteger.ToString( "N4" ) );
		Assert.Equal( "1,234,567,890.00000000", SmallInteger.ToString( "N8" ) );
		Assert.Equal( "1,234,567,890.000000000000", SmallInteger.ToString( "N12" ) );
	}

    [Fact]
	public void NegativeNumber() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		Assert.Equal( $"-1,234,567,890{defaultDigits}", SmallNegativeInteger.ToString( "N" ) );
		Assert.Equal( $"-1,234,567,890{defaultDigits}", SmallNegativeInteger.ToString( "n" ) );

		Assert.Equal( $"-12,345,678,901,234,567,890{defaultDigits}", MediumNegativeInteger.ToString( "N" ) );
		Assert.Equal( $"-12,345,678,901,234,567,890{defaultDigits}", MediumNegativeInteger.ToString( "n" ) );

		Assert.Equal( $"-123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNegativeInteger.ToString( "N" ) );
		Assert.Equal( $"-123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNegativeInteger.ToString( "n" ) );
	}

	[Fact]
	public void NegativeNumber_Precision() {
		Assert.Equal( "-1,234,567,890", SmallNegativeInteger.ToString( "N0" ) );
		Assert.Equal( "-1,234,567,890.0000", SmallNegativeInteger.ToString( "N4" ) );
		Assert.Equal( "-1,234,567,890.00000000", SmallNegativeInteger.ToString( "N8" ) );
		Assert.Equal( "-1,234,567,890.000000000000", SmallNegativeInteger.ToString( "N12" ) );
	}

	[Fact]
	public void Percent() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}";

		Assert.Equal( $"123,456,789,000{defaultDigits}%", SmallInteger.ToString( "P" ) );
		Assert.Equal( $"123,456,789,000{defaultDigits}%", SmallInteger.ToString( "p" ) );

		Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%", MediumInteger.ToString( "P" ) );
		Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%", MediumInteger.ToString( "p" ) );

		Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%", LargeInteger.ToString( "P" ) );
		Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%", LargeInteger.ToString( "p" ) );
	}

	[Fact]
	public void Percent_Precision() {
		Assert.Equal( "123,456,789,000%", SmallInteger.ToString( "P0" ) );
		Assert.Equal( "123,456,789,000.0000%", SmallInteger.ToString( "P4" ) );
		Assert.Equal( "123,456,789,000.00000000%", SmallInteger.ToString( "P8" ) );
		Assert.Equal( "123,456,789,000.000000000000%", SmallInteger.ToString( "P12" ) );
	}

	[Fact]
	public void NegalivePercent() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}";

		Assert.Equal( $"-123,456,789,000{defaultDigits}%%", SmallNegativeInteger.ToString( "P" ) );
		Assert.Equal( $"-123,456,789,000{defaultDigits}%%", SmallNegativeInteger.ToString( "p" ) );

		Assert.Equal( $"-1,234,567,890,123,456,789,000{defaultDigits}%%", MediumNegativeInteger.ToString( "P" ) );
		Assert.Equal( $"-1,234,567,890,123,456,789,000{defaultDigits}%%", MediumNegativeInteger.ToString( "p" ) );

		Assert.Equal( $"-12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", LargeNegativeInteger.ToString( "P" ) );
		Assert.Equal( $"-12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", LargeNegativeInteger.ToString( "p" ) );
	}

	[Fact]
	public void NegativePercent_Precision() {
		Assert.Equal( "-123,456,789,000%", SmallNegativeInteger.ToString( "P0" ) );
		Assert.Equal( "-123,456,789,000.0000%", SmallNegativeInteger.ToString( "P4" ) );
		Assert.Equal( "-123,456,789,000.00000000%", SmallNegativeInteger.ToString( "P8" ) );
		Assert.Equal( "-123,456,789,000.000000000000%", SmallNegativeInteger.ToString( "P12" ) );
	}

	[Fact]
	public void Exponential() {
		Assert.Equal( "1.234568E+009", SmallInteger.ToString( "E" ) );
		Assert.Equal( "1.234568e+009", SmallInteger.ToString( "e" ) );

		Assert.Equal( "1.234568E+019", MediumInteger.ToString( "E" ) );
		Assert.Equal( "1.234568e+019", MediumInteger.ToString( "e" ) );

		Assert.Equal( "1.234568E+029", LargeInteger.ToString( "E" ) );
		Assert.Equal( "1.234568e+029", LargeInteger.ToString( "e" ) );
	}

	[Fact]
	public void Exponential_Precision() {
		Assert.Equal( "1.E+009", SmallInteger.ToString( "E0" ) );
		// Case E2 added to test no rounding
		Assert.Equal( "1.23E+009", SmallInteger.ToString( "E2" ) );
		Assert.Equal( "1.2346E+009", SmallInteger.ToString( "E4" ) );
		Assert.Equal( "1.23456789E+009", SmallInteger.ToString( "E8" ) );
		// Case E9 added to test Exponent = Precision
		Assert.Equal( "1.234567890E+009", SmallInteger.ToString( "E9" ) );
		Assert.Equal( "1.234567890000E+009", SmallInteger.ToString( "E12" ) );
	}

	[Fact]
	public void NegativeExponential() {
		Assert.Equal( "-1.234568E+009", SmallNegativeInteger.ToString( "E" ) );
		Assert.Equal( "-1.234568e+009", SmallNegativeInteger.ToString( "e" ) );

		Assert.Equal( "-1.234568E+019", MediumNegativeInteger.ToString( "E" ) );
		Assert.Equal( "-1.234568e+019", MediumNegativeInteger.ToString( "e" ) );

		Assert.Equal( "-1.234568E+029", LargeNegativeInteger.ToString( "E" ) );
		Assert.Equal( "-1.234568e+029", LargeNegativeInteger.ToString( "e" ) );
	}

	[Fact]
	public void NegativeExponential_Precision() {
		Assert.Equal( "-1.E+009", SmallNegativeInteger.ToString( "E0" ) );
		// Case E2 added to test no rounding
		Assert.Equal( "-1.23E+009", SmallNegativeInteger.ToString( "E2" ) );
		Assert.Equal( "-1.2346E+009", SmallNegativeInteger.ToString( "E4" ) );
		Assert.Equal( "-1.23456789E+009", SmallNegativeInteger.ToString( "E8" ) );
		// Case E9 added to test Exponent = Precision
		Assert.Equal( "-1.234567890E+009", SmallNegativeInteger.ToString( "E9" ) );
		Assert.Equal( "-1.234567890000E+009", SmallNegativeInteger.ToString( "E12" ) );
	}

	[Fact]
	public void UnknownFormatSpecifier() {
		Exception exc = Record.Exception(
			() =>
				Integer.Unit.ToString( "Z" )
		);

		Assert.NotNull( exc );
		Assert.IsType<System.FormatException>( exc );
	}
}