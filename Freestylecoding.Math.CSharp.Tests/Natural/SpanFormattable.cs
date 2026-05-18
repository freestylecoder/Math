using System;
using System.Linq;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class SpanFormattable {
	private static void assertEqual<T>( string expected, T value, string format, IFormatProvider formatProvider ) where T : ISpanFormattable {
		int expectedLength = expected.Length;
		char[] destBuffer = new string( ' ', expectedLength ).ToCharArray();
		Span<char> destination = new Span<char>( destBuffer );
		int charsWritten = 0;

		Assert.True( value.TryFormat( destination, out charsWritten, format, formatProvider ) );
		Assert.Equal( expectedLength, charsWritten );

		// The TrimEnd is because I filled the original buffer with spaces
		Assert.Equal( expected, destination.ToString().TrimEnd() );
	}

	private static bool TryFormat<T>( T value, Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider formatProvider ) where T : ISpanFormattable =>
		value.TryFormat( destination, out charsWritten, format, formatProvider );

	[Fact]
	public void Sanity() {
		// Yes, it does the test twice.
		// It also double checks the shortcut test method
		assertEqual( "1", Natural.Unit, "", null );

		char[] destBuffer = new[] { ' ', ' ', ' ', ' ', ' ' };
		Span<char> destination = new Span<char>( destBuffer );
		int charsWritten = 0;

		Assert.True( TryFormat( Natural.Unit, destination, ref charsWritten, "", null ) );
		Assert.Equal( 1, charsWritten );

		// The TrimEnd is because I filled the original buffer with spaces
		Assert.Equal( "1", destination.ToString().TrimEnd() );
	}

	[Fact]
	public void FillToAvailable() {
		char[] destBuffer = new[] { ' ', ' ', ' ', ' ', ' ' };
		Span<char> destination = new Span<char>( destBuffer );
		int charsWritten = 0;

		Assert.False( TryFormat( SmallNatural, destination, ref charsWritten, "", null ) );
		Assert.Equal( 5, charsWritten );

		// The TrimEnd is because I filled the original buffer with spaces
		Assert.Equal( SmallNaturalString.Substring( 0, 5 ), destination.ToString() );
	}

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
	public void StandardFormatSpecifiers( string format ) {
		char[] actualBuffer = new string( ' ', 32 ).ToCharArray();
		Span<char> actual = new Span<char>( actualBuffer );
		int actualChars = 0;

		char[] expectedBuffer = new string( ' ', 32 ).ToCharArray();
		Span<char> expected = new Span<char>( expectedBuffer );
		int expectedChars = 0;

		Assert.True( TryFormat( 0x4996_02D2u, expected, ref expectedChars, format, null ) );
		Assert.True( TryFormat( SmallNatural, actual, ref actualChars, format, null ) );

		Assert.Equal( expected.ToString(), actual.ToString() );
	}

	[Fact]
	public void GeneralAndRoundTrip() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		assertEqual( SmallNaturalString, SmallNatural, "G", null );
		assertEqual( SmallNaturalString, SmallNatural, "g", null );
		assertEqual( SmallNaturalString, SmallNatural, "R", null );
		assertEqual( SmallNaturalString, SmallNatural, "r", null );

		assertEqual( MediumNaturalString, MediumNatural, "G", null );
		assertEqual( MediumNaturalString, MediumNatural, "g", null );
		assertEqual( MediumNaturalString, MediumNatural, "R", null );
		assertEqual( MediumNaturalString, MediumNatural, "r", null );

		assertEqual( LargeNaturalString, LargeNatural, "G", null );
		assertEqual( LargeNaturalString, LargeNatural, "g", null );
		assertEqual( LargeNaturalString, LargeNatural, "R", null );
		assertEqual( LargeNaturalString, LargeNatural, "r", null );
	}

	[Fact]
	public void GeneralAndRoundTrip_IgnorePrecision() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		foreach( int i in Enumerable.Range( 0, 20 ) ) {
			assertEqual( SmallNaturalString, SmallNatural, $"G{i}", null );
			assertEqual( SmallNaturalString, SmallNatural, $"g{i}", null );
			assertEqual( SmallNaturalString, SmallNatural, $"R{i}", null );
			assertEqual( SmallNaturalString, SmallNatural, $"r{i}", null );

			assertEqual( MediumNaturalString, MediumNatural, $"G{i}", null );
			assertEqual( MediumNaturalString, MediumNatural, $"g{i}", null );
			assertEqual( MediumNaturalString, MediumNatural, $"R{i}", null );
			assertEqual( MediumNaturalString, MediumNatural, $"r{i}", null );

			assertEqual( LargeNaturalString, LargeNatural, $"G{i}", null );
			assertEqual( LargeNaturalString, LargeNatural, $"g{i}", null );
			assertEqual( LargeNaturalString, LargeNatural, $"R{i}", null );
			assertEqual( LargeNaturalString, LargeNatural, $"r{i}", null );
		}
	}

	[Fact]
	public void GeneralAndRoundTrip_IgnoreFormatProvider() {
		// These are all the same as Object.ToString
		// Thus, I'm lumping the tests together
		foreach( System.Globalization.CultureInfo culture in Cultures ) {
			assertEqual( SmallNaturalString, SmallNatural, "G", culture );
			assertEqual( SmallNaturalString, SmallNatural, "g", culture );
			assertEqual( SmallNaturalString, SmallNatural, "R", culture );
			assertEqual( SmallNaturalString, SmallNatural, "r", culture );

			assertEqual( MediumNaturalString, MediumNatural, "G", culture );
			assertEqual( MediumNaturalString, MediumNatural, "g", culture );
			assertEqual( MediumNaturalString, MediumNatural, "R", culture );
			assertEqual( MediumNaturalString, MediumNatural, "r", culture );

			assertEqual( LargeNaturalString, LargeNatural, "G", culture );
			assertEqual( LargeNaturalString, LargeNatural, "g", culture );
			assertEqual( LargeNaturalString, LargeNatural, "R", culture );
			assertEqual( LargeNaturalString, LargeNatural, "r", culture );
		}
	}

	[Fact]
	public void Decimal() {
		assertEqual( SmallNaturalString, SmallNatural, "D", null );
		assertEqual( SmallNaturalString, SmallNatural, "d", null );

		assertEqual( MediumNaturalString, MediumNatural, "D", null );
		assertEqual( MediumNaturalString, MediumNatural, "d", null );

		assertEqual( LargeNaturalString, LargeNatural, "D", null );
		assertEqual( LargeNaturalString, LargeNatural, "d", null );
	}

	[Fact]
	public void Decimal_Precision() {
		assertEqual( SmallNaturalString, SmallNatural, "D0", null );
		assertEqual( SmallNaturalString, SmallNatural, "D4", null );
		assertEqual( SmallNaturalString, SmallNatural, "D8", null );
		assertEqual( "001234567890", SmallNatural, "D12", null );
	}

	[Fact]
	public void Decimal_IgnoreFormatProvider() {
		foreach( System.Globalization.CultureInfo culture in Cultures ) {
			assertEqual( SmallNaturalString, SmallNatural, "D", culture );
			assertEqual( SmallNaturalString, SmallNatural, "d", culture );

			assertEqual( MediumNaturalString, MediumNatural, "D", culture );
			assertEqual( MediumNaturalString, MediumNatural, "d", culture );

			assertEqual( LargeNaturalString, LargeNatural, "D", culture );
			assertEqual( LargeNaturalString, LargeNatural, "d", culture );
		}
	}

	[Fact]
	public void Hexadecimal() {
		assertEqual( "499602D2", SmallNatural, "X", null );
		assertEqual( "499602d2", SmallNatural, "x", null );

		assertEqual( "AB54A98CEB1F0AD2", MediumNatural, "X", null );
		assertEqual( "ab54a98ceb1f0ad2", MediumNatural, "x", null );

		assertEqual( "18EE90FF6C373E0EE4E3F0AD2", LargeNatural, "X", null );
		assertEqual( "18ee90ff6c373e0ee4e3f0ad2", LargeNatural, "x", null );
	}

	[Fact]
	public void Hexadecimal_Precision() {
		assertEqual( "499602D2", SmallNatural, "X0", null );
		assertEqual( "499602D2", SmallNatural, "X4", null );
		assertEqual( "499602D2", SmallNatural, "X8", null );
		assertEqual( "0000499602D2", SmallNatural, "X12", null );
	}

	[Fact]
	public void Hexadecimal_IgnoreFormatProvider() {
		foreach( System.Globalization.CultureInfo culture in Cultures ) {
			assertEqual( "499602D2", SmallNatural, "X", culture );
			assertEqual( "499602d2", SmallNatural, "x", culture );

			assertEqual( "AB54A98CEB1F0AD2", MediumNatural, "X", culture );
			assertEqual( "ab54a98ceb1f0ad2", MediumNatural, "x", culture );

			assertEqual( "18EE90FF6C373E0EE4E3F0AD2", LargeNatural, "X", culture );
			assertEqual( "18ee90ff6c373e0ee4e3f0ad2", LargeNatural, "x", culture );
		}
	}

	[Fact]
	public void Binary() {
		assertEqual( "1001001100101100000001011010010", SmallNatural, "B", null );
		assertEqual( "1001001100101100000001011010010", SmallNatural, "b", null );

		assertEqual( "1010101101010100101010011000110011101011000111110000101011010010", MediumNatural, "B", null );
		assertEqual( "1010101101010100101010011000110011101011000111110000101011010010", MediumNatural, "b", null );

		assertEqual( "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010", LargeNatural, "B", null );
		assertEqual( "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010", LargeNatural, "b", null );
	}

	[Fact]
	public void Binary_Precision() {
		assertEqual( "1001001100101100000001011010010", SmallNatural, "B0", null );
		assertEqual( "1001001100101100000001011010010", SmallNatural, "B8", null );
		assertEqual( "1001001100101100000001011010010", SmallNatural, "B16", null );
		assertEqual( "01001001100101100000001011010010", SmallNatural, "B32", null );
	}

	[Fact]
	public void Binary_IgnoreFormatProvider() {
		foreach( System.Globalization.CultureInfo culture in Cultures ) {
			assertEqual( "1001001100101100000001011010010", SmallNatural, "B", culture );
			assertEqual( "1001001100101100000001011010010", SmallNatural, "b", culture );

			assertEqual( "1010101101010100101010011000110011101011000111110000101011010010", MediumNatural, "B", culture );
			assertEqual( "1010101101010100101010011000110011101011000111110000101011010010", MediumNatural, "b", culture );

			assertEqual( "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010", LargeNatural, "B", culture );
			assertEqual( "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010", LargeNatural, "b", culture );
		}
	}

	[Fact]
	public void Currency() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		assertEqual( $"$1,234,567,890{defaultDigits}", SmallNatural, "C", null );
		assertEqual( $"$1,234,567,890{defaultDigits}", SmallNatural, "c", null );

		assertEqual( $"$12,345,678,901,234,567,890{defaultDigits}", MediumNatural, "C", null );
		assertEqual( $"$12,345,678,901,234,567,890{defaultDigits}", MediumNatural, "c", null );

		assertEqual( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNatural, "C", null );
		assertEqual( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNatural, "c", null );
	}

	[Fact]
	public void Currency_Precision() {
		assertEqual( "$1,234,567,890", SmallNatural, "C0", null );
		assertEqual( "$1,234,567,890.0000", SmallNatural, "C4", null );
		assertEqual( "$1,234,567,890.00000000", SmallNatural, "C8", null );
		assertEqual( "$1,234,567,890.000000000000", SmallNatural, "C12", null );
	}

	[Fact]
	public void Currency_FormatProvider() {
		assertEqual( "$1,234.00", new Natural( 1234u ), "C", US_Culture );
		assertEqual( "£1,234.00", new Natural( 1234u ), "C", UK_Culture );
		assertEqual( "1 234,00 €", new Natural( 1234u ), "C", FR_Culture );
		assertEqual( "1.234,00 €", new Natural( 1234u ), "C", LU_Culture );
	}

	[Fact]
	public void Currency_Patterns() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.CurrencyPositivePattern = 0;
		assertEqual( "$1,234.00", new Natural( 1234u ), "C", custom );

		custom.CurrencyPositivePattern = 1;
		assertEqual( "1,234.00$", new Natural( 1234u ), "C", custom );

		custom.CurrencyPositivePattern = 2;
		assertEqual( "$ 1,234.00", new Natural( 1234u ), "C", custom );

		custom.CurrencyPositivePattern = 3;
		assertEqual( "1,234.00 $", new Natural( 1234u ), "C", custom );
	}

	[Fact]
	public void Currency_GroupSizes() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.CurrencyGroupSizes = new[] { 5, 4, 3 };
		assertEqual( "$12,345,678,901,2345,67890.00", MediumNatural, "C", custom );
	}

	[Fact]
	public void FixedPoint() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}";

		assertEqual( $"1234567890{defaultDigits}", SmallNatural, "F", null );
		assertEqual( $"1234567890{defaultDigits}", SmallNatural, "f", null );

		assertEqual( $"12345678901234567890{defaultDigits}", MediumNatural, "F", null );
		assertEqual( $"12345678901234567890{defaultDigits}", MediumNatural, "f", null );

		assertEqual( $"123456789012345678901234567890{defaultDigits}", LargeNatural, "F", null );
		assertEqual( $"123456789012345678901234567890{defaultDigits}", LargeNatural, "f", null );
	}

	[Fact]
	public void FixedPoint_Precision() {
		assertEqual( "1234567890.", SmallNatural, "F0", null );
		assertEqual( "1234567890.0000", SmallNatural, "F4", null );
		assertEqual( "1234567890.00000000", SmallNatural, "F8", null );
		assertEqual( "1234567890.000000000000", SmallNatural, "F12", null );
	}

	[Fact]
	public void FixedPoint_FormatProvider() {
		assertEqual( "1234.00", new Natural( 1234u ), "F", US_Culture );
		assertEqual( "1234.000", new Natural( 1234u ), "F", UK_Culture );
		assertEqual( "1234,000", new Natural( 1234u ), "F", FR_Culture );
		assertEqual( "1234,000", new Natural( 1234u ), "F", LU_Culture );
	}

	[Fact]
	public void Number() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}";

		assertEqual( $"1,234,567,890{defaultDigits}", SmallNatural, "N", null );
		assertEqual( $"1,234,567,890{defaultDigits}", SmallNatural, "n", null );

		assertEqual( $"12,345,678,901,234,567,890{defaultDigits}", MediumNatural, "N", null );
		assertEqual( $"12,345,678,901,234,567,890{defaultDigits}", MediumNatural, "n", null );

		assertEqual( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNatural, "N", null );
		assertEqual( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", LargeNatural, "n", null );
	}

	[Fact]
	public void Number_Precision() {
		assertEqual( "1,234,567,890", SmallNatural, "N0", null );
		assertEqual( "1,234,567,890.0000", SmallNatural, "N4", null );
		assertEqual( "1,234,567,890.00000000", SmallNatural, "N8", null );
		assertEqual( "1,234,567,890.000000000000", SmallNatural, "N12", null );
	}

	[Fact]
	public void Number_FormatProvider() {
		assertEqual( "1,234.00", new Natural( 1234u ), "N", US_Culture );
		assertEqual( "1,234.000", new Natural( 1234u ), "N", UK_Culture );
		assertEqual( "1 234,000", new Natural( 1234u ), "N", FR_Culture );
		assertEqual( "1.234,000", new Natural( 1234u ), "N", LU_Culture );
	}

	[Fact]
	public void Number_GroupSizes() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.NumberGroupSizes = [5, 4, 3];
		assertEqual( "12,345,678,901,2345,67890.00", MediumNatural, "N", custom );
	}

	[Fact]
	public void Percent() {
		string defaultDigits = $".{new string( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}";

		assertEqual( $"123,456,789,000{defaultDigits}%", SmallNatural, "P", null );
		assertEqual( $"123,456,789,000{defaultDigits}%", SmallNatural, "p", null );

		assertEqual( $"1,234,567,890,123,456,789,000{defaultDigits}%", MediumNatural, "P", null );
		assertEqual( $"1,234,567,890,123,456,789,000{defaultDigits}%", MediumNatural, "p", null );

		assertEqual( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%", LargeNatural, "P", null );
		assertEqual( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%", LargeNatural, "p", null );
	}

	[Fact]
	public void Percent_Precision() {
		assertEqual( "123,456,789,000%", SmallNatural, "P0", null );
		assertEqual( "123,456,789,000.0000%", SmallNatural, "P4", null );
		assertEqual( "123,456,789,000.00000000%", SmallNatural, "P8", null );
		assertEqual( "123,456,789,000.000000000000%", SmallNatural, "P12", null );
	}

	[Fact]
	public void Percent_FormatProvider() {
		assertEqual( "123,400.00%", new Natural( 1234u ), "P", US_Culture );
		assertEqual( "123,400.000%", new Natural( 1234u ), "P", UK_Culture );
		assertEqual( "123 400,000 %", new Natural( 1234u ), "P", FR_Culture );
		assertEqual( "123.400,000 %", new Natural( 1234u ), "P", LU_Culture );
	}

	[Fact]
	public void Percent_Patterns() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo("en-US").NumberFormat;

		custom.PercentPositivePattern = 0;
		assertEqual( "123,400.00 %", new Natural( 1234u ), "P", custom );

		custom.PercentPositivePattern = 1;
		assertEqual( "123,400.00%", new Natural( 1234u ), "P", custom );

		custom.PercentPositivePattern = 2;
		assertEqual( "%123,400.00", new Natural( 1234u ), "P", custom );

		custom.PercentPositivePattern = 3;
		assertEqual( "% 123,400.00", new Natural( 1234u ), "P", custom );
	}

	[Fact]
	public void Percent_GroupSizes() {
		System.Globalization.NumberFormatInfo custom = new System.Globalization.CultureInfo( "en-US" ).NumberFormat;

		custom.PercentGroupSizes = [5, 4, 3];
		assertEqual( "1,234,567,890,123,4567,89000.00%", MediumNatural, "P", custom );
	}

	[Fact]
	public void Exponential() {
		assertEqual( "1.234568E+009", SmallNatural, "E", null );
		assertEqual( "1.234568e+009", SmallNatural, "e", null );

		assertEqual( "1.234568E+019", MediumNatural, "E", null );
		assertEqual( "1.234568e+019", MediumNatural, "e", null );

		assertEqual( "1.234568E+029", LargeNatural, "E", null );
		assertEqual( "1.234568e+029", LargeNatural, "e", null );
	}

	[Fact]
	public void Exponential_Precision() {
		assertEqual( "1.E+009", SmallNatural, "E0", null );
		// Case E2 added to test no rounding
		assertEqual( "1.23E+009", SmallNatural, "E2", null );
		assertEqual( "1.2346E+009", SmallNatural, "E4", null );
		assertEqual( "1.23456789E+009", SmallNatural, "E8", null );
		// Case E9 added to test Exponent = Precision
		assertEqual( "1.234567890E+009", SmallNatural, "E9", null );
		assertEqual( "1.234567890000E+009", SmallNatural, "E12", null );
	}

	[Fact]
	public void Exponential_FormatProvider() {
		assertEqual( "1.234568E+009", SmallNatural, "E", US_Culture );
		assertEqual( "1.234568E+009", SmallNatural, "E", UK_Culture );
		assertEqual( "1,234568E+009", SmallNatural, "E", FR_Culture );
		assertEqual( "1,234568E+009", SmallNatural, "E", LU_Culture );
	}

	[Fact]
	public void UnknownFormatSpecifier() {
		char[] destBuffer = new[] { ' ', ' ', ' ', ' ', ' ' };
		Span<char> destination = new Span<char>( destBuffer );
		int charsWritten = 0;

		try {
			SpanFormattable.TryFormat( Natural.Unit, destination, ref charsWritten, "Z", null );
			Assert.Fail( "Expected System.FormatException" );
		} catch( FormatException _ ) {
			return;
		} catch( Exception exp ) {
			Assert.Fail( $"Expected System.FormatException, Actual {exp.GetType().Name}" );
		}
	}
}