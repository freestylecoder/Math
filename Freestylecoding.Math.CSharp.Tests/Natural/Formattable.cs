using System;
using System.Linq;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class Formattable {
	IFormattable one = Natural.Unit as IFormattable;
	IFormattable small  = SmallNatural as IFormattable;
	IFormattable medium = MediumNatural as IFormattable;
	IFormattable large  = LargeNatural as IFormattable;

	[Fact]
	public void Sanity() =>
		Assert.Equal(
			"1",
			one.ToString( null, null )
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
			0x4996_02D2u.ToString( format, null ),
			small.ToString( format, null )
		);

	[Fact]
	public void GeneralAndRoundTrip() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		Assert.Equal( SmallNaturalString, small.ToString( "G", null ) );
		Assert.Equal( SmallNaturalString, small.ToString( "g", null ) );
		Assert.Equal( SmallNaturalString, small.ToString( "R", null ) );
		Assert.Equal( SmallNaturalString, small.ToString( "r", null ) );

		Assert.Equal( MediumNaturalString, medium.ToString( "G", null ) );
		Assert.Equal( MediumNaturalString, medium.ToString( "g", null ) );
		Assert.Equal( MediumNaturalString, medium.ToString( "R", null ) );
		Assert.Equal( MediumNaturalString, medium.ToString( "r", null ) );

		Assert.Equal( LargeNaturalString, large.ToString( "G", null ) );
		Assert.Equal( LargeNaturalString, large.ToString( "g", null ) );
		Assert.Equal( LargeNaturalString, large.ToString( "R", null ) );
		Assert.Equal( LargeNaturalString, large.ToString( "r", null ) );
	}

	[Fact]
	public void GeneralAndRoundTrip_IgnorePrecision() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		foreach( int i in Enumerable.Range( 0, 20 ) ) {
			Assert.Equal( SmallNaturalString, small.ToString( $"G{i}", null ) );
			Assert.Equal( SmallNaturalString, small.ToString( $"g{i}", null ) );
			Assert.Equal( SmallNaturalString, small.ToString( $"R{i}", null ) );
			Assert.Equal( SmallNaturalString, small.ToString( $"r{i}", null ) );

			Assert.Equal( MediumNaturalString, medium.ToString( $"G{i}", null ) );
			Assert.Equal( MediumNaturalString, medium.ToString( $"g{i}", null ) );
			Assert.Equal( MediumNaturalString, medium.ToString( $"R{i}", null ) );
			Assert.Equal( MediumNaturalString, medium.ToString( $"r{i}", null ) );

			Assert.Equal( LargeNaturalString, large.ToString( $"G{i}", null ) );
			Assert.Equal( LargeNaturalString, large.ToString( $"g{i}", null ) );
			Assert.Equal( LargeNaturalString, large.ToString( $"R{i}", null ) );
			Assert.Equal( LargeNaturalString, large.ToString( $"r{i}", null ) );
		}
	}

	[Fact]
	public void GeneralAndRoundTrip_IgnoreFormatProvider() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		foreach( System.Globalization.CultureInfo culture in Cultures ) {
			Assert.Equal( SmallNaturalString, small.ToString( "G", culture ) );
			Assert.Equal( SmallNaturalString, small.ToString( "g", culture ) );
			Assert.Equal( SmallNaturalString, small.ToString( "R", culture ) );
			Assert.Equal( SmallNaturalString, small.ToString( "r", culture ) );

			Assert.Equal( MediumNaturalString, medium.ToString( "G", culture ) );
			Assert.Equal( MediumNaturalString, medium.ToString( "g", culture ) );
			Assert.Equal( MediumNaturalString, medium.ToString( "R", culture ) );
			Assert.Equal( MediumNaturalString, medium.ToString( "r", culture ) );

			Assert.Equal( LargeNaturalString, large.ToString( "G", culture ) );
			Assert.Equal( LargeNaturalString, large.ToString( "g", culture ) );
			Assert.Equal( LargeNaturalString, large.ToString( "R", culture ) );
			Assert.Equal( LargeNaturalString, large.ToString( "r", culture ) );
		}
	}

	[Fact]
	public void Decimal() {
		Assert.Equal( SmallNaturalString, small.ToString( "D", null ) );
		Assert.Equal( SmallNaturalString, small.ToString( "d", null ) );

		Assert.Equal( MediumNaturalString, medium.ToString( "D", null ) );
		Assert.Equal( MediumNaturalString, medium.ToString( "d", null ) );

		Assert.Equal( LargeNaturalString, large.ToString( "D", null ) );
		Assert.Equal( LargeNaturalString, large.ToString( "d", null ) );
	}

	[Fact]
	public void Decimal_Precision() {
		Assert.Equal( SmallNaturalString, small.ToString( "D0", null ) );
		Assert.Equal( SmallNaturalString, small.ToString( "D4", null ) );
		Assert.Equal( SmallNaturalString, small.ToString( "D8", null ) );
		Assert.Equal( "001234567890", small.ToString( "D12", null ) );
	}

	[Fact]
	public void Decimal_IgnoreFormatProvider() {
		foreach( System.Globalization.CultureInfo culture in Cultures ) {
			Assert.Equal( SmallNaturalString, small.ToString( "D", culture ) );
			Assert.Equal( SmallNaturalString, small.ToString( "d", culture ) );

			Assert.Equal( MediumNaturalString, medium.ToString( "D", culture ) );
			Assert.Equal( MediumNaturalString, medium.ToString( "d", culture ) );

			Assert.Equal( LargeNaturalString, large.ToString( "D", culture ) );
			Assert.Equal( LargeNaturalString, large.ToString( "d", culture ) );
		}
	}

	[Fact]
	public void Hexadecimal() {
		Assert.Equal( "499602D2", small.ToString( "X", null ) );
		Assert.Equal( "499602d2", small.ToString( "x", null ) );

		Assert.Equal( "AB54A98CEB1F0AD2", medium.ToString( "X", null ) );
		Assert.Equal( "ab54a98ceb1f0ad2", medium.ToString( "x", null ) );

		Assert.Equal( "18EE90FF6C373E0EE4E3F0AD2", large.ToString( "X", null ) );
		Assert.Equal( "18ee90ff6c373e0ee4e3f0ad2", large.ToString( "x", null ) );
	}

	[Fact]
	public void Hexadecimal_Precision() {
		Assert.Equal( "499602D2", small.ToString( "X0", null ) );
		Assert.Equal( "499602D2", small.ToString( "X4", null ) );
		Assert.Equal( "499602D2", small.ToString( "X8", null ) );
		Assert.Equal( "0000499602D2", small.ToString( "X12", null ) );
	}

	[Fact]
	public void Hexadecimal_IgnoreFormatProvider() {
		foreach( System.Globalization.CultureInfo culture in Cultures ) {
			Assert.Equal( "499602D2", small.ToString( "X", culture ) );
			Assert.Equal( "499602d2", small.ToString( "x", culture ) );

			Assert.Equal( "AB54A98CEB1F0AD2", medium.ToString( "X", culture ) );
			Assert.Equal( "ab54a98ceb1f0ad2", medium.ToString( "x", culture ) );

			Assert.Equal( "18EE90FF6C373E0EE4E3F0AD2", large.ToString( "X", culture ) );
			Assert.Equal( "18ee90ff6c373e0ee4e3f0ad2", large.ToString( "x", culture ) );
		}
	}

	[Fact]
	public void Binary() {
		Assert.Equal(
			"1001001100101100000001011010010",
			small.ToString( "B", null )
		);
		Assert.Equal(
			"1001001100101100000001011010010",
			small.ToString( "b", null )
		);

		Assert.Equal(
			"1010101101010100101010011000110011101011000111110000101011010010",
			medium.ToString( "B", null )
		);
		Assert.Equal(
			"1010101101010100101010011000110011101011000111110000101011010010",
			medium.ToString( "b", null )
		);

		Assert.Equal(
			"1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
			large.ToString( "B", null )
		);
		Assert.Equal(
			"1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
			large.ToString( "b", null )
		);
	}

	[Fact]
	public void Binary_Precision() {
		Assert.Equal( "1001001100101100000001011010010", small.ToString( "B0", null ) );
		Assert.Equal( "1001001100101100000001011010010", small.ToString( "B8", null ) );
		Assert.Equal( "1001001100101100000001011010010", small.ToString( "B16", null ) );
		Assert.Equal( "01001001100101100000001011010010", small.ToString( "B32", null ) );
	}

	[Fact]
	public void Binary_IgnoreFormatProvider() {
		foreach( System.Globalization.CultureInfo culture in Cultures ) {
			Assert.Equal(
				"1001001100101100000001011010010",
				small.ToString( "B", culture )
			);
			Assert.Equal(
				"1001001100101100000001011010010",
				small.ToString( "b", culture )
			);

			Assert.Equal(
				"1010101101010100101010011000110011101011000111110000101011010010",
				medium.ToString( "B", culture )
			);
			Assert.Equal(
				"1010101101010100101010011000110011101011000111110000101011010010",
				medium.ToString( "b", culture )
			);

			Assert.Equal(
				"1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
				large.ToString( "B", culture )
			);
			Assert.Equal(
				"1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
				large.ToString( "b", culture )
			);
		}
	}

	[Fact]
	public void Currency() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		Assert.Equal( $"$1,234,567,890{defaultDigits}", small.ToString( "C", null ) );
		Assert.Equal( $"$1,234,567,890{defaultDigits}", small.ToString( "c", null ) );

		Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "C", null ) );
		Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "c", null ) );

		Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "C", null ) );
		Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "c", null ) );
	}

	[Fact]
	public void Currency_Precision() {
		Assert.Equal( "$1,234,567,890", small.ToString( "C0", null ) );
		Assert.Equal( "$1,234,567,890.0000", small.ToString( "C4", null ) );
		Assert.Equal( "$1,234,567,890.00000000", small.ToString( "C8", null ) );
		Assert.Equal( "$1,234,567,890.000000000000", small.ToString( "C12", null ) );
	}

	[Fact]
	public void Currency_FormatProvider() {
		IFormattable testVal = new Natural( 1234u ) as IFormattable;

		Assert.Equal( "$1,234.00", testVal.ToString( "C", US_Culture ) );
		Assert.Equal( "£1,234.00", testVal.ToString( "C", UK_Culture ) );
		Assert.Equal( "1 234,00 €", testVal.ToString( "C", FR_Culture ) );
		Assert.Equal( "1.234,00 €", testVal.ToString( "C", LU_Culture ) );
	}

	[Fact]
	public void Currency_Patterns() {
		IFormattable testVal = new Natural( 1234u ) as IFormattable;
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.CurrencyPositivePattern = 0;
		Assert.Equal( "$1,234.00", testVal.ToString( "C", custom ) );

		custom.CurrencyPositivePattern = 1;
		Assert.Equal( "1,234.00$", testVal.ToString( "C", custom ) );

		custom.CurrencyPositivePattern = 2;
		Assert.Equal( "$ 1,234.00", testVal.ToString( "C", custom ) );

		custom.CurrencyPositivePattern = 3;
		Assert.Equal( "1,234.00 $", testVal.ToString( "C", custom ) );
	}

	[Fact]
	public void Currency_GroupSizes() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.CurrencyGroupSizes = [5, 4, 3];
		Assert.Equal( "$12,345,678,901,2345,67890.00", medium.ToString( "C", custom ) );
	}

	[Fact]
	public void FixedPoint() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}";

		Assert.Equal( $"1234567890{defaultDigits}", small.ToString( "F", null ) );
		Assert.Equal( $"1234567890{defaultDigits}", small.ToString( "f", null ) );

		Assert.Equal( $"12345678901234567890{defaultDigits}", medium.ToString( "F", null ) );
		Assert.Equal( $"12345678901234567890{defaultDigits}", medium.ToString( "f", null ) );

		Assert.Equal( $"123456789012345678901234567890{defaultDigits}", large.ToString( "F", null ) );
		Assert.Equal( $"123456789012345678901234567890{defaultDigits}", large.ToString( "f", null ) );
	}

	[Fact]
	public void FixedPoint_Precision() {
		Assert.Equal( "1234567890.", small.ToString( "F0", null ) );
		Assert.Equal( "1234567890.0000", small.ToString( "F4", null ) );
		Assert.Equal( "1234567890.00000000", small.ToString( "F8", null ) );
		Assert.Equal( "1234567890.000000000000", small.ToString( "F12", null ) );
	}

	[Fact]
	public void FixedPoint_FormatProvider() {
		IFormattable testVal = new Natural( 1234u ) as IFormattable;

		Assert.Equal( "1234.00", testVal.ToString( "F", US_Culture ) );
		Assert.Equal( "1234.000", testVal.ToString( "F", UK_Culture ) );
		Assert.Equal( "1234,000", testVal.ToString( "F", FR_Culture ) );
		Assert.Equal( "1234,000", testVal.ToString( "F", LU_Culture ) );
	}

	[Fact]
	public void Number() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		Assert.Equal( $"1,234,567,890{defaultDigits}", small.ToString( "N", null ) );
		Assert.Equal( $"1,234,567,890{defaultDigits}", small.ToString( "n", null ) );

		Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "N", null ) );
		Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "n", null ) );

		Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "N", null ) );
		Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "n", null ) );
	}

	[Fact]
	public void Number_Precision() {
		Assert.Equal( "1,234,567,890", small.ToString( "N0", null ) );
		Assert.Equal( "1,234,567,890.0000", small.ToString( "N4", null ) );
		Assert.Equal( "1,234,567,890.00000000", small.ToString( "N8", null ) );
		Assert.Equal( "1,234,567,890.000000000000", small.ToString( "N12", null ) );
	}

	[Fact]
	public void Number_FormatProvider() {
		IFormattable testVal = new Natural( 1234u ) as IFormattable;

		Assert.Equal( "1,234.00", testVal.ToString( "N", US_Culture ) );
		Assert.Equal( "1,234.000", testVal.ToString( "N", UK_Culture ) );
		Assert.Equal( "1 234,000", testVal.ToString( "N", FR_Culture ) );
		Assert.Equal( "1.234,000", testVal.ToString( "N", LU_Culture ) );
	}

	[Fact]
	public void Number_GroupSizes() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.NumberGroupSizes = [5, 4, 3];
		Assert.Equal( "12,345,678,901,2345,67890.00", medium.ToString( "N", custom ) );
	}

	[Fact]
	public void Number_GroupSizes_Empty() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.NumberGroupSizes = new int[] { };
		Assert.Equal( "12345678901234567890.00", medium.ToString( "N", custom ) );
	}

	[Fact]
	public void Percent() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}";

		Assert.Equal( $"123,456,789,000{defaultDigits}%", small.ToString( "P", null ) );
		Assert.Equal( $"123,456,789,000{defaultDigits}%", small.ToString( "p", null ) );

		Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%", medium.ToString( "P", null ) );
		Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%", medium.ToString( "p", null ) );

		Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%", large.ToString( "P", null ) );
		Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%", large.ToString( "p", null ) );
	}

	[Fact]
	public void Percent_Precision() {
		Assert.Equal( "123,456,789,000%", small.ToString( "P0", null ) );
		Assert.Equal( "123,456,789,000.0000%", small.ToString( "P4", null ) );
		Assert.Equal( "123,456,789,000.00000000%", small.ToString( "P8", null ) );
		Assert.Equal( "123,456,789,000.000000000000%", small.ToString( "P12", null ) );
	}

	[Fact]
	public void Percent_FormatProvider() {
		IFormattable testVal = new Natural( 1234u ) as IFormattable;

		Assert.Equal( "123,400.00%", testVal.ToString( "P", US_Culture ) );
		Assert.Equal( "123,400.000%", testVal.ToString( "P", UK_Culture ) );
		Assert.Equal( "123 400,000 %", testVal.ToString( "P", FR_Culture ) );
		Assert.Equal( "123.400,000 %", testVal.ToString( "P", LU_Culture ) );
	}

	[Fact]
	public void Percent_Patterns() {
		IFormattable testVal = new Natural( 1234u ) as IFormattable;
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.PercentPositivePattern = 0;
		Assert.Equal( "123,400.00 %", testVal.ToString( "P", custom ) );

		custom.PercentPositivePattern = 1;
		Assert.Equal( "123,400.00%", testVal.ToString( "P", custom ) );

		custom.PercentPositivePattern = 2;
		Assert.Equal( "%123,400.00", testVal.ToString( "P", custom ) );

		custom.PercentPositivePattern = 3;
		Assert.Equal( "% 123,400.00", testVal.ToString( "P", custom ) );
	}

	[Fact]
	public void Percent_GroupSizes() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.PercentGroupSizes = [5, 4, 3];
		Assert.Equal( "1,234,567,890,123,4567,89000.00%", medium.ToString( "P", custom ) );
	}

	[Fact]
	public void Exponential() {
		Assert.Equal( "1.234568E+009", small.ToString( "E", null ) );
		Assert.Equal( "1.234568e+009", small.ToString( "e", null ) );

		Assert.Equal( "1.234568E+019", medium.ToString( "E", null ) );
		Assert.Equal( "1.234568e+019", medium.ToString( "e", null ) );

		Assert.Equal( "1.234568E+029", large.ToString( "E", null ) );
		Assert.Equal( "1.234568e+029", large.ToString( "e", null ) );
	}

	[Fact]
	public void Exponential_Precision() {
		Assert.Equal( "1.E+009", small.ToString( "E0", null ) );
		// Case E2 added to test no rounding
		Assert.Equal( "1.23E+009", small.ToString( "E2", null ) );
		Assert.Equal( "1.2346E+009", small.ToString( "E4", null ) );
		Assert.Equal( "1.23456789E+009", small.ToString( "E8", null ) );
		// Case E9 added to test Exponent = Precision
		Assert.Equal( "1.234567890E+009", small.ToString( "E9", null ) );
		Assert.Equal( "1.234567890000E+009", small.ToString( "E12", null ) );
	}

	[Fact]
	public void Exponential_FormatProvider() {
		Assert.Equal( "1.234568E+009", small.ToString( "E", US_Culture ) );
		Assert.Equal( "1.234568E+009", small.ToString( "E", UK_Culture ) );
		Assert.Equal( "1,234568E+009", small.ToString( "E", FR_Culture ) );
		Assert.Equal( "1,234568E+009", small.ToString( "E", LU_Culture ) );
	}

	[Fact]
	public void UnknownFormatSpecifier() {
		Exception exc = Record.Exception(
			() => one.ToString( "Z",  null )
		);

		Assert.NotNull( exc );
		Assert.IsType<System.FormatException>( exc );
	}
}