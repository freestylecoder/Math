using System;
using System.Linq;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class ObjectGetType {
	[Fact]
	public void Sanity() =>
		Assert.Equal(
			typeof( Natural ),
			Natural.Unit.GetType()
		);
}

public class ObjectEquals {
	[Theory]
	[InlineData( 0u, 0u, true )]
	[InlineData( 1u, 0u, false )]
	[InlineData( 0u, 1u, false )]
	[InlineData( 1u, 1u, true )]
	public void Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal(
			expected,
			new Natural( left ).Equals( new Natural( right ) )
		);

	[Fact]
	public void LargeNaturalNaturals() =>
		Assert.True(
			FListNatural( 0xFu, 0x00000101u ).Equals( FListNatural( 0xFu, 0x00000101u ) )
		);

	[Fact]
	public void HardcodedTypes() {
		Assert.True( Natural.Unit.Equals( (byte)1 ) );
		Assert.True( Natural.Unit.Equals( (ushort)1 ) );
		Assert.True( Natural.Unit.Equals( (uint)1 ) );
		Assert.True( Natural.Unit.Equals( (ulong)1 ) );

		Assert.True( Natural.Unit.Equals( System.UInt128.One ) );
		Assert.True( Natural.Unit.Equals( System.Numerics.BigInteger.One ) );
	}

	[Fact]
	public void IntegralTypes() {
		Assert.True( Natural.Unit.Equals( (sbyte)1 ) );
		Assert.True( Natural.Unit.Equals( (short)1 ) );
		Assert.True( Natural.Unit.Equals( (int)1 ) );
		Assert.True( Natural.Unit.Equals( (long)1 ) );

		Assert.True( Natural.Unit.Equals( System.Int128.One ) );
	}

	[Fact]
	public void FloatingTypes() {
		Assert.True( Natural.Unit.Equals( (float)1 ) );
		Assert.True( Natural.Unit.Equals( (double)1 ) );
		Assert.True( Natural.Unit.Equals( (decimal)1 ) );

		Assert.True( Natural.Unit.Equals( System.Numerics.Complex.One ) );
	}

	[Fact]
	public void NotNumber() {
		Assert.False( Natural.Unit.Equals( "1" ) );
		Assert.False( Natural.Unit.Equals( true ) );
	}

	[Fact]
	public void NegativeIntegralTypes() {
		Assert.False( Natural.Unit.Equals( (sbyte)-1 ) );
		Assert.False( Natural.Unit.Equals( (short)-1 ) );
		Assert.False( Natural.Unit.Equals( (int)-1 ) );
		Assert.False( Natural.Unit.Equals( (long)-1 ) );

		Assert.False( Natural.Unit.Equals( -System.Int128.One ) );
		Assert.False( Natural.Unit.Equals( -System.Numerics.BigInteger.One ) );
	}

	[Fact]
	public void FloatingTypesWithDecimals() {
		Assert.False( Natural.Unit.Equals( (float)1.1 ) );
		Assert.False( Natural.Unit.Equals( (double)1.1 ) );
		Assert.False( Natural.Unit.Equals( (decimal)1.1 ) );

		Assert.False( Natural.Unit.Equals( new System.Numerics.Complex( 1.1, 0 ) ) );
	}

	[Fact]
	public void Imaginary() =>
		// I'm making an assumption that an imaginary number is not an integer
		// This explicitly checks that assumption
		Assert.False( Natural.Unit.Equals( new System.Numerics.Complex( 1, 1 ) ) );
}

public class ObjectGetHashCode {
	[Fact]
	public void Sanity() {
		Assert.Equal(
			Natural.Unit.GetHashCode(),
			Natural.Unit.GetHashCode()
		);

		Assert.NotEqual(
			Natural.Zero.GetHashCode(),
			Natural.Unit.GetHashCode()
		);
	}
}

public class ObjectToString {
	[Theory]
	[InlineData( 0u, "0" )]         // Sanity
	[InlineData( 1u, "1" )]         // Sanity
	[InlineData( 123u, "123" )]     // multiple bits
	[InlineData( 45678u, "45678" )] // rev
	public void Sanity( uint actual, string expected ) =>
		Assert.Equal(
			expected,
			new Natural( actual ).ToString()
		);

	[Fact]
	public void Bigger() =>
		Assert.Equal(
			"1234567890123456789",
			FListNatural( 0x112210F4u, 0x7DE98115u ).ToString()
		);
}

public class ToStringFormat {
	[Fact]
	public void Sanity() =>
		Assert.Equal(
			"1",
			Natural.Unit.ToString( null )
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
			0x4996_02D2u.ToString( format ),
			SmallNatural.ToString( format )
		);

	[Fact]
	public void GeneralAndRoundTrip() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "G" ) );
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "g" ) );
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "R" ) );
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "r" ) );

		Assert.Equal( MediumNaturalString, MediumNatural.ToString( "G" ) );
		Assert.Equal( MediumNaturalString, MediumNatural.ToString( "g" ) );
		Assert.Equal( MediumNaturalString, MediumNatural.ToString( "R" ) );
		Assert.Equal( MediumNaturalString, MediumNatural.ToString( "r" ) );

		Assert.Equal( LargeNaturalString, LargeNatural.ToString( "G" ) );
		Assert.Equal( LargeNaturalString, LargeNatural.ToString( "g" ) );
		Assert.Equal( LargeNaturalString, LargeNatural.ToString( "R" ) );
		Assert.Equal( LargeNaturalString, LargeNatural.ToString( "r" ) );
	}

	[Fact]
	public void GeneralAndRoundTrip_IgnorePrecision() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		foreach( int i in Enumerable.Range( 0, 20 ) ) {
			Assert.Equal( SmallNaturalString, SmallNatural.ToString( $"G{i}" ) );
			Assert.Equal( SmallNaturalString, SmallNatural.ToString( $"g{i}" ) );
			Assert.Equal( SmallNaturalString, SmallNatural.ToString( $"R{i}" ) );
			Assert.Equal( SmallNaturalString, SmallNatural.ToString( $"r{i}" ) );

			Assert.Equal( MediumNaturalString, MediumNatural.ToString( $"G{i}" ) );
			Assert.Equal( MediumNaturalString, MediumNatural.ToString( $"g{i}" ) );
			Assert.Equal( MediumNaturalString, MediumNatural.ToString( $"R{i}" ) );
			Assert.Equal( MediumNaturalString, MediumNatural.ToString( $"r{i}" ) );

			Assert.Equal( LargeNaturalString, LargeNatural.ToString( $"G{i}" ) );
			Assert.Equal( LargeNaturalString, LargeNatural.ToString( $"g{i}" ) );
			Assert.Equal( LargeNaturalString, LargeNatural.ToString( $"R{i}" ) );
			Assert.Equal( LargeNaturalString, LargeNatural.ToString( $"r{i}" ) );
		}
	}

	[Fact]
	public void Decimal() {
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "D" ) );
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "d" ) );

		Assert.Equal( MediumNaturalString, MediumNatural.ToString( "D" ) );
		Assert.Equal( MediumNaturalString, MediumNatural.ToString( "d" ) );

		Assert.Equal( LargeNaturalString, LargeNatural.ToString( "D" ) );
		Assert.Equal( LargeNaturalString, LargeNatural.ToString( "d" ) );
	}

	[Fact]
	public void Decimal_Precision() {
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "D0" ) );
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "D4" ) );
		Assert.Equal( SmallNaturalString, SmallNatural.ToString( "D8" ) );
		Assert.Equal( "001234567890", SmallNatural.ToString( "D12" ) );
	}

	[Fact]
	public void Hexadecimal() {
		Assert.Equal( "499602D2", SmallNatural.ToString( "X" ) );
		Assert.Equal( "499602d2", SmallNatural.ToString( "x" ) );

		Assert.Equal( "AB54A98CEB1F0AD2", MediumNatural.ToString( "X" ) );
		Assert.Equal( "ab54a98ceb1f0ad2", MediumNatural.ToString( "x" ) );

		Assert.Equal( "18EE90FF6C373E0EE4E3F0AD2", LargeNatural.ToString( "X" ) );
		Assert.Equal( "18ee90ff6c373e0ee4e3f0ad2", LargeNatural.ToString( "x" ) );
	}

	[Fact]
	public void Hexadecimal_Precision() {
		Assert.Equal( "499602D2", SmallNatural.ToString( "X0" ) );
		Assert.Equal( "499602D2", SmallNatural.ToString( "X4" ) );
		Assert.Equal( "499602D2", SmallNatural.ToString( "X8" ) );
		Assert.Equal( "0000499602D2", SmallNatural.ToString( "X12" ) );
	}

	[Fact]
	public void Binary() {
		Assert.Equal(
			"1001001100101100000001011010010",
			SmallNatural.ToString( "B" )
		);
		Assert.Equal(
			"1001001100101100000001011010010",
			SmallNatural.ToString( "b" )
		);

		Assert.Equal(
			"1010101101010100101010011000110011101011000111110000101011010010",
			MediumNatural.ToString( "B" )
		);
		Assert.Equal(
			"1010101101010100101010011000110011101011000111110000101011010010",
			MediumNatural.ToString( "b" )
		);

		Assert.Equal(
			"1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
			LargeNatural.ToString( "B" )
		);
		Assert.Equal(
			"1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
			LargeNatural.ToString( "b" )
		);
	}

	[Fact]
	public void Binary_Precision() {
		Assert.Equal( "1001001100101100000001011010010", SmallNatural.ToString( "B0" ) );
		Assert.Equal( "1001001100101100000001011010010", SmallNatural.ToString( "B8" ) );
		Assert.Equal( "1001001100101100000001011010010", SmallNatural.ToString( "B16" ) );
		Assert.Equal( "01001001100101100000001011010010", SmallNatural.ToString( "B32" ) );
	}

	[Fact]
	public void Currency() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		Assert.Equal( $"$1,234,567,890{defaultDigits}", SmallNatural.ToString( "C" ) );
		Assert.Equal( $"$1,234,567,890{defaultDigits}", SmallNatural.ToString( "c" ) );

		Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", MediumNatural.ToString( "C" ) );
		Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", MediumNatural.ToString( "c" ) );

		Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNatural.ToString( "C" ) );
		Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNatural.ToString( "c" ) );
	}

	[Fact]
	public void Currency_Precision() {
		Assert.Equal( "$1,234,567,890", SmallNatural.ToString( "C0" ) );
		Assert.Equal( "$1,234,567,890.0000", SmallNatural.ToString( "C4" ) );
		Assert.Equal( "$1,234,567,890.00000000", SmallNatural.ToString( "C8" ) );
		Assert.Equal( "$1,234,567,890.000000000000", SmallNatural.ToString( "C12" ) );
	}

	[Fact]
	public void FixedPoint() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}";

		Assert.Equal( $"1234567890{defaultDigits}", SmallNatural.ToString( "F" ) );
		Assert.Equal( $"1234567890{defaultDigits}", SmallNatural.ToString( "f" ) );

		Assert.Equal( $"12345678901234567890{defaultDigits}", MediumNatural.ToString( "F" ) );
		Assert.Equal( $"12345678901234567890{defaultDigits}", MediumNatural.ToString( "f" ) );

		Assert.Equal( $"123456789012345678901234567890{defaultDigits}", LargeNatural.ToString( "F" ) );
		Assert.Equal( $"123456789012345678901234567890{defaultDigits}", LargeNatural.ToString( "f" ) );
	}

	[Fact]
	public void FixedPoint_Precision() {
		Assert.Equal( "1234567890.", SmallNatural.ToString( "F0" ) );
		Assert.Equal( "1234567890.0000", SmallNatural.ToString( "F4" ) );
		Assert.Equal( "1234567890.00000000", SmallNatural.ToString( "F8" ) );
		Assert.Equal( "1234567890.000000000000", SmallNatural.ToString( "F12" ) );
	}

	[Fact]
	public void Number() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		Assert.Equal( $"1,234,567,890{defaultDigits}", SmallNatural.ToString( "N" ) );
		Assert.Equal( $"1,234,567,890{defaultDigits}", SmallNatural.ToString( "n" ) );

		Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", MediumNatural.ToString( "N" ) );
		Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", MediumNatural.ToString( "n" ) );

		Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNatural.ToString( "N" ) );
		Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNatural.ToString( "n" ) );
	}

	[Fact]
	public void Number_Precision() {
		Assert.Equal( "1,234,567,890", SmallNatural.ToString( "N0" ) );
		Assert.Equal( "1,234,567,890.0000", SmallNatural.ToString( "N4" ) );
		Assert.Equal( "1,234,567,890.00000000", SmallNatural.ToString( "N8" ) );
		Assert.Equal( "1,234,567,890.000000000000", SmallNatural.ToString( "N12" ) );
	}

	[Fact]
	public void Percent() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}";

		Assert.Equal( $"123,456,789,000{defaultDigits}%", SmallNatural.ToString( "P" ) );
		Assert.Equal( $"123,456,789,000{defaultDigits}%", SmallNatural.ToString( "p" ) );

		Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%", MediumNatural.ToString( "P" ) );
		Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%", MediumNatural.ToString( "p" ) );

		Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%", LargeNatural.ToString( "P" ) );
		Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%", LargeNatural.ToString( "p" ) );
	}

	[Fact]
	public void Percent_Precision() {
		Assert.Equal( "123,456,789,000%", SmallNatural.ToString( "P0" ) );
		Assert.Equal( "123,456,789,000.0000%", SmallNatural.ToString( "P4" ) );
		Assert.Equal( "123,456,789,000.00000000%", SmallNatural.ToString( "P8" ) );
		Assert.Equal( "123,456,789,000.000000000000%", SmallNatural.ToString( "P12" ) );
	}

	[Fact]
	public void Exponential() {
		Assert.Equal( "1.234568E+009", SmallNatural.ToString( "E" ) );
		Assert.Equal( "1.234568e+009", SmallNatural.ToString( "e" ) );

		Assert.Equal( "1.234568E+019", MediumNatural.ToString( "E" ) );
		Assert.Equal( "1.234568e+019", MediumNatural.ToString( "e" ) );

		Assert.Equal( "1.234568E+029", LargeNatural.ToString( "E" ) );
		Assert.Equal( "1.234568e+029", LargeNatural.ToString( "e" ) );
	}

	[Fact]
	public void Exponential_Precision() {
		Assert.Equal( "1.E+009", SmallNatural.ToString( "E0" ) );
		// Case E2 added to test no rounding
		Assert.Equal( "1.23E+009", SmallNatural.ToString( "E2" ) );
		Assert.Equal( "1.2346E+009", SmallNatural.ToString( "E4" ) );
		Assert.Equal( "1.23456789E+009", SmallNatural.ToString( "E8" ) );
		// Case E9 added to test Exponent = Precision
		Assert.Equal( "1.234567890E+009", SmallNatural.ToString( "E9" ) );
		Assert.Equal( "1.234567890000E+009", SmallNatural.ToString( "E12" ) );
	}

	[Fact]
	public void UnknownFormatSpecifier() {
		Exception exc = Record.Exception(
			() => Natural.Unit.ToString( "Z" )
		);

		Assert.NotNull( exc );
		Assert.IsType<System.FormatException>( exc );
	}
}