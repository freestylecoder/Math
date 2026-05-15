using System;
using System.Globalization;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class ParseString {
	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) =>
		Assert.Equal( new Natural( n ), Natural.Parse( s ) );

	[Fact]
	public void BiggerSanity() =>
		Assert.Equal(
			FListNatural( 0x112210F4u, 0x7DE98115u ),
			Natural.Parse( "1234567890123456789" )
		);

	[Fact]
	public void InsaneSanity() =>
		// This test case is for a very specialized test case
		// That's 1E80, which is roughly the number of atoms in the universe
		Assert.Equal(
			FListNatural( 863u, 2649374239u, 1809837936u, 3453057829u, 4020508874u, 1671571300u, 3468754944u, 0u, 0u ),
			Natural.Parse( "100000000000000000000000000000000000000000000000000000000000000000000000000000000" )
		);

	[Theory]
	[InlineData( " 1" )]
	[InlineData( "  1" )]
	[InlineData( "\t1" )]
	[InlineData( "\t 1" )]
	[InlineData( "\t\t1" )]
	[InlineData( "\n1" )]
	[InlineData( "\r1" )]
	[InlineData( "\r\n1" )]
	[InlineData( "\n\r1" )]
	public void AllowLeadingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse( s )
		);

	[Theory]
	[InlineData( "1 " )]
	[InlineData( "1  " )]
	[InlineData( "1\t" )]
	[InlineData( "1\t " )]
	[InlineData( "1\t\t" )]
	[InlineData( "1\n" )]
	[InlineData( "1\r" )]
	[InlineData( "1\r\n" )]
	[InlineData( "1\n\r" )]
	public void AllowTrailingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse( s )
		);

	[Theory]
	[InlineData( " 1 " )]
	[InlineData( "  1 " )]
	[InlineData( "\t1 " )]
	[InlineData( "\t 1\t" )]
	[InlineData( "\t\t1  " )]
	[InlineData( " 1\n" )]
	[InlineData( "\t1\r" )]
	[InlineData( "  1\r\n" )]
	[InlineData( "\t 1\n\r" )]
	public void AllowWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse( s )
		);

	[Fact]
	public void AllowLeadingPositive() =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse( $"{CurrentCultureNumberFormat.PositiveSign}1" )
		);

	[Fact]
	public void LeadingNegativeOverflow() =>
		Assert.IsType<OverflowException>(
			Record.Exception(
				() => Natural.Parse( $"{CurrentCultureNumberFormat.NegativeSign}1" )
			)
		);

	[Fact]
	public void AllowLeadingNegativeZero() =>
		Assert.Equal(
			Natural.Zero,
			Natural.Parse( $"{CurrentCultureNumberFormat.NegativeSign}0" )
		);

	[Fact]
	public void DisallowBothLeading() {
		// I would like for this to be a theory with the two cases separate
		// However, I can't do string interpolation in an attribute
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( $"{CurrentCultureNumberFormat.PositiveSign}{CurrentCultureNumberFormat.NegativeSign}0" )
			)
		);
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( $"{CurrentCultureNumberFormat.NegativeSign}{CurrentCultureNumberFormat.PositiveSign}0" )
			)
		);
	}

	[Fact]
	public void DisallowTrailingPositive() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( $"1{CurrentCultureNumberFormat.PositiveSign}" )
			)
		);

	[Fact]
	public void DisallowTrailingNegative() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( $"1{CurrentCultureNumberFormat.NegativeSign}" )
			)
		);

	[Fact]
	public void DisallowTrailingNegativeZero() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( $"0{CurrentCultureNumberFormat.NegativeSign}" )
			)
		);

	[Fact]
	public void DisallowParentheses() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( "(1)" )
			)
		);

	[Fact]
	public void DisallowParenthesesZero() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( "(0)" )
			)
		);

	[Fact]
	public void DisallowDecimalPoint() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( "1.0" )
			)
		);

	[Fact]
	public void DisallowDecimalPointZero() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( "1.0" )
			)
		);

	[Fact]
	public void AllowGroupSeparator() =>
		Assert.Equal(
			new Natural( 1234u ),
			Natural.Parse( $"1{CurrentCultureNumberFormat.NumberGroupSeparator}234" )
		);

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( $"10{exp}1" )
			)
		);

	[Fact]
	public void DisallowCurrencySymbolPrefix() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( $"{CurrentCultureNumberFormat.CurrencySymbol}1" )
			)
		);

	[Fact]
	public void DisallowCurrencySymbolPostfix() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( $"1{CurrentCultureNumberFormat.CurrencySymbol}" )
			)
		);

	[Fact]
	public void DisallowHex() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() => Natural.Parse( "1A" )
			)
		);

	[Fact]
	public void ReadBinaryAsDecimal() =>
		Assert.Equal(
			new Natural( 11u ),
			Natural.Parse( "11" )
		);
}

public class ParseStringStyle {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) =>
		Assert.Equal(
			new Natural( n ),
			Natural.Parse(
				s,
				NumberStyles.Integer
			)
		);

	[Fact]
	public void BiggerSanity() =>
		Assert.Equal(
			FListNatural( 0x112210F4u, 0x7DE98115u ),
			Natural.Parse(
				"1234567890123456789",
				NumberStyles.Integer
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowLeadingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"{s}1",
				NumberStyles.AllowLeadingWhite
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowTrailingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"1{s}",
				NumberStyles.AllowTrailingWhite
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowWhiteSpace( string s ) {
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"{s}1",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
			)
		);
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"1{s}",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
			)
		);
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"{s}1{s}",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
			)
		);
	}

	[Fact]
	public void AllowLeadingPositive() =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"{CurrentCultureNumberFormat.PositiveSign}1",
				NumberStyles.AllowLeadingSign
			)
		);

	[Fact]
	public void LeadingNegativeOverflow() =>
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					Natural.Parse(
						$"{CurrentCultureNumberFormat.NegativeSign}1",
						NumberStyles.AllowLeadingSign
					)
			)
		);

	[Fact]
	public void AllowLeadingNegativeZero() =>
		Assert.Equal(
			Natural.Zero,
			Natural.Parse(
				$"{CurrentCultureNumberFormat.NegativeSign}0",
				NumberStyles.AllowLeadingSign
			)
		);

	[Fact]
	public void DisallowBothLeading() {
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						$"{CurrentCultureNumberFormat.PositiveSign}{CurrentCultureNumberFormat.NegativeSign}0",
						NumberStyles.AllowLeadingSign
					)
			)
		);
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						$"{CurrentCultureNumberFormat.NegativeSign}{CurrentCultureNumberFormat.PositiveSign}0",
						NumberStyles.AllowLeadingSign
					)
			)
		);
	}

	[Fact]
	public void AllowTrailingPositive() =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"1{CurrentCultureNumberFormat.PositiveSign}",
				NumberStyles.AllowTrailingSign
			)
		);

	[Fact]
	public void TrailingNegativeOverflow() =>
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					Natural.Parse(
						$"1{CurrentCultureNumberFormat.NegativeSign}",
						NumberStyles.AllowTrailingSign
					)
			)
		);

	[Fact]
	public void AllowTrailingNegativeZero() =>
		Assert.Equal(
			Natural.Zero,
			Natural.Parse(
				$"0{CurrentCultureNumberFormat.NegativeSign}",
				NumberStyles.AllowTrailingSign
			)
		);

	[Fact]
	public void DisallowBothTrailing() {
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						$"0{CurrentCultureNumberFormat.PositiveSign}{CurrentCultureNumberFormat.NegativeSign}",
						NumberStyles.AllowTrailingSign
					)
			)
		);
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						$"0{CurrentCultureNumberFormat.NegativeSign}{CurrentCultureNumberFormat.PositiveSign}",
						NumberStyles.AllowTrailingSign
					)
			)
		);
	}

	[Fact]
	public void ParenthesesNegativeOverflow() =>
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"(1)",
						NumberStyles.AllowParentheses
					)
			)
		);

	[Fact]
	public void AllowParenthesesNegativeZero() =>
			Assert.Equal(
				Natural.Zero,
				Natural.Parse(
					"(0)",
					NumberStyles.AllowParentheses
				)
			);

	[Fact]
	public void ParenthesesOnlyOpen() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"(0",
						NumberStyles.AllowParentheses
					)
			)
		);

	[Fact]
	public void ParenthesesOnlyClose() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"0)",
						NumberStyles.AllowParentheses
					)
			)
		);

	[Fact]
	public void ParenthesesOutOfOrder() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						")0(",
						NumberStyles.AllowParentheses
					)
			)
		);

	[Fact]
	public void ParenthesesMoreThanOne() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"((0))",
						NumberStyles.AllowParentheses
					)
			)
		);

	[Fact]
	public void DecimalPointOverflow() =>
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					Natural.Parse(
						$"1{CurrentCultureNumberFormat.NumberDecimalSeparator}1",
						NumberStyles.AllowDecimalPoint
					)
			)
		);

	[Fact]
	public void AllowDecimalPointZero() =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"1{CurrentCultureNumberFormat.NumberDecimalSeparator}0",
				NumberStyles.AllowDecimalPoint
			)
		);

	[Fact]
	public void DecimalMoreThanOne() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						$"1{CurrentCultureNumberFormat.NumberDecimalSeparator}0{CurrentCultureNumberFormat.NumberDecimalSeparator}0",
						NumberStyles.AllowDecimalPoint
					)
			)
		);

	[Fact]
	public void AllowGroupSeparator() =>
		Assert.Equal(
			SmallNatural,
			Natural.Parse(
				$"1{CurrentCultureNumberFormat.NumberGroupSeparator}234{CurrentCultureNumberFormat.NumberGroupSeparator}567{CurrentCultureNumberFormat.NumberGroupSeparator}890",
				NumberStyles.AllowThousands
			)
		);

	[Fact]
	public void AllowExponent() {
		Assert.Equal(
			new Natural( 10u ),
			Natural.Parse(
				"1e1",
				NumberStyles.AllowExponent
			)
		);

		Assert.Equal(
			new Natural( 200u ),
			Natural.Parse(
				"2E2",
				NumberStyles.AllowExponent
			)
		);
	}

	[Fact]
	public void AllowLargeExponent() {
		// This test is mainly to hit a few areas in the base pasre code
		Assert.Equal(
			FListNatural( 0x2u, 0x540B_E400u ),
			Natural.Parse(
				"1e10",
				NumberStyles.AllowExponent
			)
		);

		// 1E80 is roughly the number of atoms in the universe
		Assert.Equal(
			FListNatural( 863u, 2649374239u, 1809837936u, 3453057829u, 4020508874u, 1671571300u, 3468754944u, 0u, 0u ),
			Natural.Parse(
				"1e80",
				NumberStyles.AllowExponent
			)
		);
	}

	[Fact]
	public void AllowExponentWithDecimal() {
		Assert.Equal(
			new Natural( 12u ),
			Natural.Parse(
				"1.2e1",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint
			)
		);

		Assert.Equal(
			new Natural( 201u ),
			Natural.Parse(
				"2.01E2",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint
			)
		);
	}

	[Fact]
	public void AllowNegativeExponentWithDecimal() {
		// Yes, these look silly, but they are technically valid
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				"10.0e-1",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint
			)
		);

		Assert.Equal(
			new Natural( 2u ),
			Natural.Parse(
				"200.0E-2",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint
			)
		);
	}

	[Fact]
	public void AllowExponentWithPositiveSign() {
		Assert.Equal(
			new Natural( 10u ),
			Natural.Parse(
				"1e+1",
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign
			)
		);

		Assert.Equal(
			new Natural( 200u ),
			Natural.Parse(
				"2E+2",
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign
			)
		);
	}

	[Fact]
	public void AllowExponentWithNegativeSign() {
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				"10e-1",
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign
			)
		);

		Assert.Equal(
			new Natural( 2u ),
			Natural.Parse(
				"200E-2",
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign
			)
		);
	}

	[Fact]
	public void ExponentWithNegativeSignOverflow() {
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"1e-1",
						NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign
					)
			)
		);

		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"2E-2",
						NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign
					)
			)
		);
	}

	[Fact]
	public void AllowCurrencySymbolPrefix() =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"{CurrentCultureNumberFormat.CurrencySymbol}1",
				NumberStyles.AllowCurrencySymbol
			)
		);

	[Fact]
	public void AllowCurrencySymbolPostfix() =>
		Assert.Equal(
			Natural.Unit,
			Natural.Parse(
				$"1{CurrentCultureNumberFormat.CurrencySymbol}",
				NumberStyles.AllowCurrencySymbol
			)
		);

	[Fact]
	public void AllowHex() {
		// If you're curious, that's equal to "1,311,768,467,294,899,695"
		// It was verified with the Windows Calculator app
		Natural value = FListNatural(  305419896u, 2427178479u  );

		Assert.Equal(
			value,
			Natural.Parse(
				"01234567890ABCDEF",
				NumberStyles.AllowHexSpecifier
			)
		);

		Assert.Equal(
			value,
			Natural.Parse(
				"01234567890abcdef",
				NumberStyles.AllowHexSpecifier
			)
		);
	}

	[Fact]
	public void AllowHex_BadInput() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"1234567890ABCDEFG",
						NumberStyles.AllowHexSpecifier
					)
			)
		);

	[Fact]
	public void AllowBinary() =>
		Assert.Equal(
			new Natural( 172u ),
			Natural.Parse(
				"10101100",
				NumberStyles.AllowBinarySpecifier
			)
		);

	[Fact]
	public void AllowBinary_BadInput() =>
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"012",
						NumberStyles.AllowBinarySpecifier
					)
			)
		);

	[Fact]
	public void DisallowBothBinaryAndHex() =>
		Assert.IsType<ArgumentException>(
			Record.Exception(
				() =>
					Natural.Parse(
						"0",
						NumberStyles.AllowBinarySpecifier | NumberStyles.AllowHexSpecifier
					)
			)
		);
}

public class ParseStringFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	private static T ParsableParse<T>( string s, IFormatProvider info ) where T : IParsable<T> =>
		T.Parse( s, info );

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) =>
		Assert.Equal(
			new Natural( n ),
			ParsableParse<Natural>(
				s,
				CurrentCultureNumberFormat
			)
		);

	[Fact]
	public void BiggerSanity() =>
		Assert.Equal(
			FListNatural( 0x112210F4u, 0x7DE98115u ),
			ParsableParse<Natural>(
				"1234567890123456789",
				CurrentCultureNumberFormat
			)
		);

	[Fact]
	public void NullFormatIsCurrentCulture() {
		Assert.Equal(
			ParsableParse<Natural>( SmallNaturalString, CurrentCultureNumberFormat ),
			ParsableParse<Natural>( SmallNaturalString, null )
		);
		Assert.Equal(
			ParsableParse<Natural>( MediumNaturalString, CurrentCultureNumberFormat ),
			ParsableParse<Natural>( MediumNaturalString, null )
		);
		Assert.Equal(
			ParsableParse<Natural>( LargeNaturalString, CurrentCultureNumberFormat ),
			ParsableParse<Natural>( LargeNaturalString, null )
		);
	}

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowLeadingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			ParsableParse<Natural>(
				$"{s}1",
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowTrailingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			ParsableParse<Natural>(
				$"1{s}",
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowWhiteSpace( string s ) {
		Assert.Equal(
			Natural.Unit,
			ParsableParse<Natural>(
				$"{s}1",
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			ParsableParse<Natural>(
				$"1{s}",
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			ParsableParse<Natural>(
				$"{s}1{s}",
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				ParsableParse<Natural>(
					$"{culture.NumberFormat.PositiveSign}1",
					culture
				)
			);
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						ParsableParse<Natural>(
							$"{culture.NumberFormat.NegativeSign}1",
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				ParsableParse<Natural>(
					$"{culture.NumberFormat.NegativeSign}0",
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						ParsableParse<Natural>(
							$"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0",
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						ParsableParse<Natural>(
							$"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0",
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( $"1{culture.NumberFormat.PositiveSign}", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingNegative() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( $"1{culture.NumberFormat.NegativeSign}", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( $"0{culture.NumberFormat.NegativeSign}", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowParentheses() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( "(1)", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowParenthesesZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( "(0)", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowDecimalPoint() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( $"1{culture.NumberFormat.NumberDecimalSeparator}0", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( $"1{culture.NumberFormat.NumberDecimalSeparator}0", culture )
				)
			);
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				new Natural( 1234u ),
				ParsableParse<Natural>( $"1{culture.NumberFormat.NumberGroupSeparator}234", culture )
			);
		}
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( $"10{exp}1", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( $"{culture.NumberFormat.CurrencySymbol}1", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( $"1{CurrentCultureNumberFormat.CurrencySymbol}", culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowHex() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => ParsableParse<Natural>( "1A", culture )
				)
			);
		}
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				new Natural( 11u ),
				ParsableParse<Natural>( "11", culture )
			);
		}
	}
}

public class ParseStringStyleFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	private static T NumberBaseParse<T>( string s, NumberStyles style, IFormatProvider info ) where T : System.Numerics.INumberBase<T> =>
		T.Parse( s, style, info );

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) =>
		Assert.Equal(
			new Natural( n ),
			NumberBaseParse<Natural>(
				s,
				NumberStyles.Integer,
				CurrentCultureNumberFormat
			)
		);

	[Fact]
	public void BiggerSanity() =>
		Assert.Equal(
			FListNatural( 0x112210F4u, 0x7DE98115u ),
			NumberBaseParse<Natural>(
				"1234567890123456789",
				NumberStyles.Integer,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowLeadingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"{s}1",
				NumberStyles.AllowLeadingWhite,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowTrailingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"1{s}",
				NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowWhiteSpace( string s ) {
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"{s}1",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"1{s}",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"{s}1{s}",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"{culture.NumberFormat.PositiveSign}1",
					NumberStyles.AllowLeadingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"{culture.NumberFormat.NegativeSign}1",
							NumberStyles.AllowLeadingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					$"{culture.NumberFormat.NegativeSign}0",
					NumberStyles.AllowLeadingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0",
							NumberStyles.AllowLeadingSign,
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0",
							NumberStyles.AllowLeadingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"1{culture.NumberFormat.PositiveSign}",
					NumberStyles.AllowTrailingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void TrailingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"1{culture.NumberFormat.NegativeSign}",
							NumberStyles.AllowTrailingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					$"0{culture.NumberFormat.NegativeSign}",
					NumberStyles.AllowTrailingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothTrailing() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}",
							NumberStyles.AllowTrailingSign,
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}",
							NumberStyles.AllowTrailingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							"(1)",
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowParenthesesNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					"(0)",
					NumberStyles.AllowParentheses,
					culture
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOnlyOpen() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							"(0",
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOnlyClose() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							"0)",
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOutOfOrder() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							")0(",
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesMoreThanOne() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							"((0))",
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void DecimalPointOverflow() {
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						$"1{CurrentCultureNumberFormat.NumberDecimalSeparator}1",
						NumberStyles.AllowDecimalPoint,
						CurrentCultureNumberFormat
					)
			)
		);
	}

	[Fact]
	public void AllowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"1{culture.NumberFormat.NumberDecimalSeparator}0",
					NumberStyles.AllowDecimalPoint,
					culture
				)
			);
		}
	}

	[Fact]
	public void DecimalMoreThanOne() {
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						$"1{CurrentCultureNumberFormat.NumberDecimalSeparator}0{CurrentCultureNumberFormat.NumberDecimalSeparator}0",
						NumberStyles.AllowDecimalPoint,
						CurrentCultureNumberFormat
					)
			)
		);
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				SmallNatural,
				NumberBaseParse<Natural>(
					$"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890",
					NumberStyles.AllowThousands,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowExponent() {
		Assert.Equal(
			new Natural( 10u ),
			NumberBaseParse<Natural>(
				"1e1",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 200u ),
			NumberBaseParse<Natural>(
				"2E2",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithDecimal() {
		Assert.Equal(
			new Natural( 12u ),
			NumberBaseParse<Natural>(
				"1.2e1",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 201u ),
			NumberBaseParse<Natural>(
				"2.01E2",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowNegativeExponentWithDecimal() {
		// Yes, these look silly, but they are technically valid
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				"10.0e-1",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 2u ),
			NumberBaseParse<Natural>(
				"200.0E-2",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithPositiveSign() {
		Assert.Equal(
			new Natural( 10u ),
			NumberBaseParse<Natural>(
				"1e+1",
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 200u ),
			NumberBaseParse<Natural>(
				"2E+2",
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithNegativeSign() {
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				"10e-1",
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 2u ),
			NumberBaseParse<Natural>(
				"200E-2",
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void ExponentWithNegativeSignOverflow() {
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						"1e-1",
						NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
						CurrentCultureNumberFormat
					)
			)
		);

		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						"2E-2",
						NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
						CurrentCultureNumberFormat
					)
			)
		);
	}

	[Fact]
	public void AllowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"{culture.NumberFormat.CurrencySymbol}1",
					NumberStyles.AllowCurrencySymbol,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"1{culture.NumberFormat.CurrencySymbol}",
					NumberStyles.AllowCurrencySymbol,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowHex() {
		// If you're curious, that's equal to "1,311,768,467,294,899,695"
		// It was verified with the Windows Calculator app
		Natural value = FListNatural(  305419896u, 2427178479u  );

		Assert.Equal(
			value,
			NumberBaseParse<Natural>(
				"1234567890ABCDEF",
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			value,
			NumberBaseParse<Natural>(
				"1234567890abcdef",
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowBinary() {
		Assert.Equal(
			new Natural( 172u ),
			NumberBaseParse<Natural>(
				"10101100",
				NumberStyles.AllowBinarySpecifier,
				CurrentCultureNumberFormat
			)
		);
	}
}

public class ParseSpanFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	private static T SpanParsableParse<T>( ReadOnlySpan<char> s, IFormatProvider info ) where T : ISpanParsable<T> =>
		T.Parse( s, info );

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) =>
		Assert.Equal(
			new Natural( n ),
			SpanParsableParse<Natural>(
				s.AsSpan(),
				CurrentCultureNumberFormat
			)
		);

	[Fact]
	public void BiggerSanity() =>
		Assert.Equal(
			FListNatural( 0x112210F4u, 0x7DE98115u ),
			SpanParsableParse<Natural>(
				"1234567890123456789".AsSpan(),
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowLeadingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			SpanParsableParse<Natural>(
				$"{s}1".AsSpan(),
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowTrailingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			SpanParsableParse<Natural>(
				$"1{s}".AsSpan(),
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowWhiteSpace( string s ) {
		Assert.Equal(
			Natural.Unit,
			SpanParsableParse<Natural>(
				$"{s}1".AsSpan(),
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			SpanParsableParse<Natural>(
				$"1{s}".AsSpan(),
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			SpanParsableParse<Natural>(
				$"{s}1{s}".AsSpan(),
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				SpanParsableParse<Natural>(
					$"{culture.NumberFormat.PositiveSign}1".AsSpan(),
					culture
				)
			);
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						SpanParsableParse<Natural>(
							$"{culture.NumberFormat.NegativeSign}1".AsSpan(),
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				SpanParsableParse<Natural>(
					$"{culture.NumberFormat.NegativeSign}0".AsSpan(),
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						SpanParsableParse<Natural>(
							$"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(),
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						SpanParsableParse<Natural>(
							$"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0".AsSpan(),
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( $"1{culture.NumberFormat.PositiveSign}".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingNegative() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( $"1{culture.NumberFormat.NegativeSign}".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( $"0{culture.NumberFormat.NegativeSign}".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowParentheses() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( "(1)".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowParenthesesZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( "(0)".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowDecimalPoint() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( $"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( $"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				new Natural( 1234u ),
				SpanParsableParse<Natural>( $"1{culture.NumberFormat.NumberGroupSeparator}234".AsSpan(), culture )
			);
		}
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( $"10{exp}1".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( $"{culture.NumberFormat.CurrencySymbol}1".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( $"1{CurrentCultureNumberFormat.CurrencySymbol}".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowHex() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => SpanParsableParse<Natural>( "1A".AsSpan(), culture )
				)
			);
		}
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				new Natural( 11u ),
				SpanParsableParse<Natural>( "11".AsSpan(), culture )
			);
		}
	}
}

public class ParseSpanStyleFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	private static T NumberBaseParse<T>( ReadOnlySpan<char> s, NumberStyles style, IFormatProvider info ) where T : System.Numerics.INumberBase<T> =>
		T.Parse( s, style, info );

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) =>
		Assert.Equal(
			new Natural( n ),
			NumberBaseParse<Natural>(
				s.AsSpan(),
				NumberStyles.Integer,
				CurrentCultureNumberFormat
			)
		);

	[Fact]
	public void BiggerSanity() =>
		Assert.Equal(
			MediumNatural,
			NumberBaseParse<Natural>(
				MediumNaturalString.AsSpan(),
				NumberStyles.Integer,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowLeadingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"{s}1".AsSpan(),
				NumberStyles.AllowLeadingWhite,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowTrailingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"1{s}".AsSpan(),
				NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowWhiteSpace( string s ) {
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"{s}1".AsSpan(),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"1{s}".AsSpan(),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				$"{s}1{s}".AsSpan(),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"{culture.NumberFormat.PositiveSign}1".AsSpan(),
					NumberStyles.AllowLeadingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"{culture.NumberFormat.NegativeSign}1".AsSpan(),
							NumberStyles.AllowLeadingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					$"{culture.NumberFormat.NegativeSign}0".AsSpan(),
					NumberStyles.AllowLeadingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(),
							NumberStyles.AllowLeadingSign,
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0".AsSpan(),
							NumberStyles.AllowLeadingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"1{culture.NumberFormat.PositiveSign}".AsSpan(),
					NumberStyles.AllowTrailingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void TrailingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"1{culture.NumberFormat.NegativeSign}".AsSpan(),
							NumberStyles.AllowTrailingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					$"0{culture.NumberFormat.NegativeSign}".AsSpan(),
					NumberStyles.AllowTrailingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothTrailing() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}".AsSpan(),
							NumberStyles.AllowTrailingSign,
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							$"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}".AsSpan(),
							NumberStyles.AllowTrailingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							"(1)".AsSpan(),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowParenthesesNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					"(0)".AsSpan(),
					NumberStyles.AllowParentheses,
					culture
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOnlyOpen() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							"(0".AsSpan(),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOnlyClose() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							"0)".AsSpan(),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOutOfOrder() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							")0(".AsSpan(),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesMoreThanOne() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							"((0))".AsSpan(),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void DecimalPointOverflow() {
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						$"1{CurrentCultureNumberFormat.NumberDecimalSeparator}1".AsSpan(),
						NumberStyles.AllowDecimalPoint,
						CurrentCultureNumberFormat
					)
			)
		);
	}

	[Fact]
	public void AllowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(),
					NumberStyles.AllowDecimalPoint,
					culture
				)
			);
		}
	}

	[Fact]
	public void DecimalMoreThanOne() {
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						$"1{CurrentCultureNumberFormat.NumberDecimalSeparator}0{CurrentCultureNumberFormat.NumberDecimalSeparator}0".AsSpan(),
						NumberStyles.AllowDecimalPoint,
						CurrentCultureNumberFormat
					)
			)
		);
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				SmallNatural,
				NumberBaseParse<Natural>(
					$"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890".AsSpan(),
					NumberStyles.AllowThousands,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowExponent() {
		Assert.Equal(
			new Natural( 10u ),
			NumberBaseParse<Natural>(
				"1e1".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 200u ),
			NumberBaseParse<Natural>(
				"2E2".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithDecimal() {
		Assert.Equal(
			new Natural( 12u ),
			NumberBaseParse<Natural>(
				"1.2e1".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 201u ),
			NumberBaseParse<Natural>(
				"2.01E2".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowNegativeExponentWithDecimal() {
		// Yes, these look silly, but they are technically valid
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				"10.0e-1".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 2u ),
			NumberBaseParse<Natural>(
				"200.0E-2".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithPositiveSign() {
		Assert.Equal(
			new Natural( 10u ),
			NumberBaseParse<Natural>(
				"1e+1".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 200u ),
			NumberBaseParse<Natural>(
				"2E+2".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithNegativeSign() {
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				"10e-1".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 2u ),
			NumberBaseParse<Natural>(
				"200E-2".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void ExponentWithNegativeSignOverflow() {
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						"1e-1".AsSpan(),
						NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
						CurrentCultureNumberFormat
					)
			)
		);

		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						"2E-2".AsSpan(),
						NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
						CurrentCultureNumberFormat
					)
			)
		);
	}

	[Fact]
	public void AllowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"{culture.NumberFormat.CurrencySymbol}1".AsSpan(),
					NumberStyles.AllowCurrencySymbol,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					$"1{culture.NumberFormat.CurrencySymbol}".AsSpan(),
					NumberStyles.AllowCurrencySymbol,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowHex() {
		// If you're curious, that's equal to "1,311,768,467,294,899,695"
		// It was verified with the Windows Calculator app
		Natural value = FListNatural(  305419896u, 2427178479u  );

		Assert.Equal(
			value,
			NumberBaseParse<Natural>(
				"1234567890ABCDEF".AsSpan(),
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			value,
			NumberBaseParse<Natural>(
				"1234567890abcdef".AsSpan(),
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowBinary() {
		Assert.Equal(
			new Natural( 172u ),
			NumberBaseParse<Natural>(
				"10101100".AsSpan(),
				NumberStyles.AllowBinarySpecifier,
				CurrentCultureNumberFormat
			)
		);
	}
}

public class ParseUtf8Format {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	private static T UtfSpanParsableParse<T>( ReadOnlySpan<byte> s, IFormatProvider info ) where T : IUtf8SpanParsable<T> =>
		T.Parse( s, info );

	private static ReadOnlySpan<byte> ToSpan( string s ) =>
		new ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes( s ) );

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) =>
		Assert.Equal(
			new Natural( n ),
			UtfSpanParsableParse<Natural>(
				ToSpan( s ),
				CurrentCultureNumberFormat
			)
		);

	[Fact]
	public void BiggerSanity() =>
		Assert.Equal(
			FListNatural( 0x112210F4u, 0x7DE98115u ),
			UtfSpanParsableParse<Natural>(
				ToSpan( "1234567890123456789" ),
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowLeadingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			UtfSpanParsableParse<Natural>(
				ToSpan( $"{s}1" ),
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowTrailingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			UtfSpanParsableParse<Natural>(
				ToSpan( $"1{s}" ),
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowWhiteSpace( string s ) {
		Assert.Equal(
			Natural.Unit,
			UtfSpanParsableParse<Natural>(
				ToSpan( $"{s}1" ),
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			UtfSpanParsableParse<Natural>(
				ToSpan( $"1{s}" ),
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			UtfSpanParsableParse<Natural>(
				ToSpan( $"{s}1{s}" ),
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				UtfSpanParsableParse<Natural>(
					ToSpan( $"{culture.NumberFormat.PositiveSign}1" ),
					culture
				)
			);
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						UtfSpanParsableParse<Natural>(
							ToSpan( $"{culture.NumberFormat.NegativeSign}1" ),
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				UtfSpanParsableParse<Natural>(
					ToSpan( $"{culture.NumberFormat.NegativeSign}0" ),
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						UtfSpanParsableParse<Natural>(
							ToSpan( $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0" ),
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						UtfSpanParsableParse<Natural>(
							ToSpan( $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0" ),
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( $"1{culture.NumberFormat.PositiveSign}" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingNegative() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( $"1{culture.NumberFormat.NegativeSign}" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( $"0{culture.NumberFormat.NegativeSign}" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowParentheses() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( "(1 )" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowParenthesesZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( "(0 )" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowDecimalPoint() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( $"1{culture.NumberFormat.NumberDecimalSeparator}0" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( $"1{culture.NumberFormat.NumberDecimalSeparator}0" ), culture )
				)
			);
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				new Natural( 1234u ),
				UtfSpanParsableParse<Natural>( ToSpan( $"1{culture.NumberFormat.NumberGroupSeparator}234" ), culture )
			);
		}
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( $"10{exp}1" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( $"{culture.NumberFormat.CurrencySymbol}1" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( $"1{CurrentCultureNumberFormat.CurrencySymbol}" ), culture )
				)
			);
		}
	}

	[Fact]
	public void DisallowHex() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() => UtfSpanParsableParse<Natural>( ToSpan( "1A" ), culture )
				)
			);
		}
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				new Natural( 11u ),
				UtfSpanParsableParse<Natural>( ToSpan( "11" ), culture )
			);
		}
	}
}

public class ParseUtf8StyleFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	private static T NumberBaseParse<T>( ReadOnlySpan<byte> s, NumberStyles style, IFormatProvider info ) where T : System.Numerics.INumberBase<T> =>
		T.Parse( s, style, info );

	private static ReadOnlySpan<byte> ToSpan( string s ) =>
		new ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes( s ) );

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) =>
		Assert.Equal(
			new Natural( n ),
			NumberBaseParse<Natural>(
				ToSpan( s ),
				NumberStyles.Integer,
				CurrentCultureNumberFormat
			)
		);

	[Fact]
	public void BiggerSanity() =>
		Assert.Equal(
			MediumNatural,
			NumberBaseParse<Natural>(
				ToSpan( MediumNaturalString ),
				NumberStyles.Integer,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowLeadingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				ToSpan( $"{s}1" ),
				NumberStyles.AllowLeadingWhite,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowTrailingWhiteSpace( string s ) =>
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				ToSpan( $"1{s}" ),
				NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);

	[Theory]
	[MemberData( nameof( Whitespace) )]
	public void AllowWhiteSpace( string s ) {
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				ToSpan( $"{s}1" ),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				ToSpan( $"1{s}" ),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				ToSpan( $"{s}1{s}" ),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					ToSpan( $"{culture.NumberFormat.PositiveSign}1" ),
					NumberStyles.AllowLeadingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( $"{culture.NumberFormat.NegativeSign}1" ),
							NumberStyles.AllowLeadingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					ToSpan( $"{culture.NumberFormat.NegativeSign}0" ),
					NumberStyles.AllowLeadingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0" ),
							NumberStyles.AllowLeadingSign,
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0" ),
							NumberStyles.AllowLeadingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					ToSpan( $"1{culture.NumberFormat.PositiveSign}" ),
					NumberStyles.AllowTrailingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void TrailingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( $"1{culture.NumberFormat.NegativeSign}" ),
							NumberStyles.AllowTrailingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					ToSpan( $"0{culture.NumberFormat.NegativeSign}" ),
					NumberStyles.AllowTrailingSign,
					culture
				)
			);
		}
	}

	[Fact]
	public void DisallowBothTrailing() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}" ),
							NumberStyles.AllowTrailingSign,
							culture
						)
			   )
			);
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( $"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}" ),
							NumberStyles.AllowTrailingSign,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<OverflowException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( "(1)" ),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void AllowParenthesesNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Zero,
				NumberBaseParse<Natural>(
					ToSpan( "(0)" ),
					NumberStyles.AllowParentheses,
					culture
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOnlyOpen() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( "(0" ),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOnlyClose() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( "0 )" ),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesOutOfOrder() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( " )0(" ),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void ParenthesesMoreThanOne() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.IsType<FormatException>(
				Record.Exception(
					() =>
						NumberBaseParse<Natural>(
							ToSpan( "((0 ))" ),
							NumberStyles.AllowParentheses,
							culture
						)
				)
			);
		}
	}

	[Fact]
	public void DecimalPointOverflow() {
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						ToSpan( $"1{CurrentCultureNumberFormat.NumberDecimalSeparator}1" ),
						NumberStyles.AllowDecimalPoint,
						CurrentCultureNumberFormat
					)
			)
		);
	}


	[Fact]
	public void AllowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					ToSpan( $"1{culture.NumberFormat.NumberDecimalSeparator}0" ),
					NumberStyles.AllowDecimalPoint,
					culture
				)
			);
		}
	}

	[Fact]
	public void DecimalMoreThanOne() {
		Assert.IsType<FormatException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						ToSpan( $"1{CurrentCultureNumberFormat.NumberDecimalSeparator}0{CurrentCultureNumberFormat.NumberDecimalSeparator}0" ),
						NumberStyles.AllowDecimalPoint,
						CurrentCultureNumberFormat
					)
			)
		);
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				SmallNatural,
				NumberBaseParse<Natural>(
					ToSpan( $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890" ),
					NumberStyles.AllowThousands,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowExponent() {
		Assert.Equal(
			new Natural( 10u ),
			NumberBaseParse<Natural>(
				ToSpan( "1e1" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 200u ),
			NumberBaseParse<Natural>(
				ToSpan( "2E2" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithDecimal() {
		Assert.Equal(
			new Natural( 12u ),
			NumberBaseParse<Natural>(
				ToSpan( "1.2e1" ),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 201u ),
			NumberBaseParse<Natural>(
				ToSpan( "2.01E2" ),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowNegativeExponentWithDecimal() {
		// Yes, these look silly, but they are technically valid
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				ToSpan( "10.0e-1" ),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 2u ),
			NumberBaseParse<Natural>(
				ToSpan( "200.0E-2" ),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithPositiveSign() {
		Assert.Equal(
			new Natural( 10u ),
			NumberBaseParse<Natural>(
				ToSpan( "1e+1" ),
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 200u ),
			NumberBaseParse<Natural>(
				ToSpan( "2E+2" ),
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowExponentWithNegativeSign() {
		Assert.Equal(
			Natural.Unit,
			NumberBaseParse<Natural>(
				ToSpan( "10e-1" ),
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			new Natural( 2u ),
			NumberBaseParse<Natural>(
				ToSpan( "200E-2" ),
				NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void ExponentWithNegativeSignOverflow() {
		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						ToSpan( "1e-1" ),
						NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
						CurrentCultureNumberFormat
					)
			)
		);

		Assert.IsType<OverflowException>(
			Record.Exception(
				() =>
					NumberBaseParse<Natural>(
						ToSpan( "2E-2" ),
						NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
						CurrentCultureNumberFormat
					)
			)
		);
	}

	[Fact]
	public void AllowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					ToSpan( $"{culture.NumberFormat.CurrencySymbol}1" ),
					NumberStyles.AllowCurrencySymbol,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Assert.Equal(
				Natural.Unit,
				NumberBaseParse<Natural>(
					ToSpan( $"1{culture.NumberFormat.CurrencySymbol}" ),
					NumberStyles.AllowCurrencySymbol,
					culture
				)
			);
		}
	}

	[Fact]
	public void AllowHex() {
		// If you're curious, that's equal to "1,311,768,467,294,899,695"
		// It was verified with the Windows Calculator app
		Natural value = FListNatural(  305419896u, 2427178479u  );

		Assert.Equal(
			value,
			NumberBaseParse<Natural>(
				ToSpan( "1234567890ABCDEF" ),
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat
			)
		);

		Assert.Equal(
			value,
			NumberBaseParse<Natural>(
				ToSpan( "1234567890abcdef" ),
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat
			)
		);
	}

	[Fact]
	public void AllowBinary() {
		Assert.Equal(
			new Natural( 172u ),
			NumberBaseParse<Natural>(
				ToSpan( "10101100" ),
				NumberStyles.AllowBinarySpecifier,
				CurrentCultureNumberFormat
			)
		);
	}
}