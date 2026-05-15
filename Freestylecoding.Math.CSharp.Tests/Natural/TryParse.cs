using System;
using System.Globalization;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

file class Overload {
	// From Natural
	public static bool TryParse( string s, ref Natural result ) =>
		Natural.TryParse( s, ref result );
	public static bool TryParse( ReadOnlySpan<char> charSpan, ref Natural result ) =>
		Natural.TryParse( charSpan, ref result );
	public static bool TryParse( ReadOnlySpan<byte> byteSpan, ref Natural result ) =>
		Natural.TryParse( byteSpan, ref result );

	// From IParsable<Natural>
	public static bool TryParse<T>( string s, IFormatProvider format, ref T result ) where T : IParsable<T> =>
		T.TryParse( s, format, out result );
	//static member TryParse( s:string, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
	//    Overload.TryParse<Natural>( s, provider, ref result )

	// From ISpanParsable<Natural>
	public static bool TryParse<T>( ReadOnlySpan<char> charSpan, IFormatProvider format, ref T result ) where T : ISpanParsable<T> =>
		T.TryParse( charSpan, format, out result );
	//static member TryParse( charSpan:System.ReadOnlySpan<char>, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
	//    Overload.TryParse<Natural>( charSpan, provider, ref result )

	// From IUtf8SpanParsable<Natural>
	public static bool TryParse<T>( ReadOnlySpan<byte> byteSpan, IFormatProvider format, ref T result ) where T : IUtf8SpanParsable<T> =>
		T.TryParse( byteSpan, format, out result );
	//static member TryParse( byteSpan:System.ReadOnlySpan<byte>, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
	//    Overload.TryParse<Natural>( byteSpan, provider, ref result )

	// From INumberBase<Natural>
	public static bool TryParse<T>( string s, NumberStyles style, IFormatProvider format, ref T result ) where T : System.Numerics.INumberBase<T> =>
		T.TryParse( s, style, format, out result );
	public static bool TryParse<T>( ReadOnlySpan<char> charSpan, NumberStyles style, IFormatProvider format, ref T result ) where T : System.Numerics.INumberBase<T> =>
		T.TryParse( charSpan, style, format, out result );
	public static bool TryParse<T>( ReadOnlySpan<byte> byteSpan, NumberStyles style, IFormatProvider format, ref T result ) where T : System.Numerics.INumberBase<T> =>
		T.TryParse( byteSpan, style, format, out result );
	//static member TryParse( s:string, style:NumberStyles, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
	//    Overload.TryParse<Natural>( s, style, provider, ref result )
	//static member TryParse( charSpan:System.ReadOnlySpan<char>, style:NumberStyles, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
	//    Overload.TryParse<Natural>( charSpan, style, provider, ref result )
	//static member TryParse( byteSpan:System.ReadOnlySpan<byte>, style:NumberStyles, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
	//    Overload.TryParse<Natural>( byteSpan, style, provider, ref result )
}

public class TryParseString {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( s, ref result ) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( SmallNaturalString, ref result ) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( MediumNaturalString, ref result ) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( LargeNaturalString, ref result ) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1", ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{s}", ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1", ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{s}", ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1{s}", ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{CurrentCultureNumberFormat.PositiveSign}1", ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"{CurrentCultureNumberFormat.NegativeSign}1", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{CurrentCultureNumberFormat.NegativeSign}0", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowBothLeading() {
		// I would like for this to be a theory with the two cases separate
		// However, I can't do string interpolation in an attribute
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"{CurrentCultureNumberFormat.PositiveSign}{CurrentCultureNumberFormat.NegativeSign}0", ref result ) );
		Assert.Equal( Natural.Zero, result );

		result = DeadBeef;
		Assert.False( Overload.TryParse( $"{CurrentCultureNumberFormat.NegativeSign}{CurrentCultureNumberFormat.PositiveSign}0", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingPositive() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.PositiveSign}", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingNegative() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.NegativeSign}", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"0{CurrentCultureNumberFormat.NegativeSign}", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowParentheses() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( "(1)", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowParenthesesZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( "(0)", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowDecimalPoint() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.NumberDecimalSeparator}1", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.NumberDecimalSeparator}0", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowGroupSeparator() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{CurrentCultureNumberFormat.NumberGroupSeparator}234", ref result ) );
		Assert.Equal( new Natural( 1234u ), result );
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"10{exp}1", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"{CurrentCultureNumberFormat.CurrencySymbol}1", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.CurrencySymbol}", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowHex() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( "1A", ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( "11", ref result ) );
		Assert.Equal( new Natural( 11u ), result );
	}
}

public class TryParseStringFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( s, CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( SmallNaturalString, CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( MediumNaturalString, CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( LargeNaturalString, CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1", CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{s}", CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1", CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{s}", CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1{s}", CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( $"{culture.NumberFormat.PositiveSign}1", culture, ref result ) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"{culture.NumberFormat.NegativeSign}1", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( $"{culture.NumberFormat.NegativeSign}0", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False( Overload.TryParse( $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.PositiveSign}", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingNegative() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.NegativeSign}", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"0{culture.NumberFormat.NegativeSign}", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowParentheses() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( "(1)", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowParenthesesZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( "(0)", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowDecimalPoint() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.NumberDecimalSeparator}1", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.NumberDecimalSeparator}0", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( $"1{culture.NumberFormat.NumberGroupSeparator}234", culture, ref result ) );
			Assert.Equal( new Natural( 1234u ), result );
		}
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"10{exp}1", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"{culture.NumberFormat.CurrencySymbol}1", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.CurrencySymbol}", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowHex() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( "1A", culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( "11", culture, ref result ) );
			Assert.Equal( new Natural( 11u ), result );
		}
	}
}

public class TryParseStringStyleFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				s,
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				SmallNaturalString,
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				MediumNaturalString,
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				LargeNaturalString,
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"{s}1",
				NumberStyles.AllowLeadingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"1{s}",
				NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"{s}1",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"1{s}",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"{s}1{s}",
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"{culture.NumberFormat.PositiveSign}1",
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"{culture.NumberFormat.NegativeSign}1",
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"{culture.NumberFormat.NegativeSign}0",
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0",
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0",
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowTrailingPositive() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"1{culture.NumberFormat.PositiveSign}",
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void TrailingNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"1{culture.NumberFormat.NegativeSign}",
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowTrailingNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"0{culture.NumberFormat.NegativeSign}",
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothTrailing() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}",
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}",
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					"(1)",
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowParenthesesNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					"(0)",
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOnlyOpen() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					"(0",
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOnlyClose() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					"0)",
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOutOfOrder() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					")0(",
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesMoreThanOne() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					"((0))",
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DecimalPointOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"1{culture.NumberFormat.NumberDecimalSeparator}1",
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowDecimalPointZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"1{culture.NumberFormat.NumberDecimalSeparator}0",
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void DecimalMoreThanOne() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"1{culture.NumberFormat.NumberDecimalSeparator}0{culture.NumberFormat.NumberDecimalSeparator}0",
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890",
					NumberStyles.AllowThousands,
					culture,
					ref result
				) );
			Assert.Equal( SmallNatural, result );
		}
	}

	[Fact]
	public void AllowExponent() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1e1",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 10u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"2E2",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 200u ), result );
	}

	[Fact]
	public void AllowExponentWithDecimal() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1.2e1",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 12u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"2.01E2",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 201u ), result );
	}

	[Fact]
	public void AllowNegativeExponentWithDecimal() {
		// Yes, these look silly, but they are technically valid
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"10.0e-1",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"200.0E-2",
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 2u ), result );
	}

	[Fact]
	public void AllowExponentWithPositiveSign() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1e+1",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 10u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"2E+2",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 200u ), result );
	}

	[Fact]
	public void AllowExponentWithNegativeSign() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"10e-1",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"200E-2",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 2u ), result );
	}

	[Fact]
	public void ExponentWithNegativeSignOverflow() {
		Natural result = DeadBeef;
		Assert.False(
			Overload.TryParse(
				"1e-1",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Zero, result );

		result = DeadBeef;
		Assert.False(
			Overload.TryParse(
				"2E-2",
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"{culture.NumberFormat.CurrencySymbol}1",
					NumberStyles.AllowCurrencySymbol,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void AllowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"1{culture.NumberFormat.CurrencySymbol}",
					NumberStyles.AllowCurrencySymbol,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void AllowHex() {
		// If you're curious, that's equal to "1,311,768,467,294,899,695"
		// It was verified with the Windows Calculator app
		Natural value = FListNatural(  305419896u, 2427178479u  );

		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1234567890ABCDEF",
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( value, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1234567890ABCDEF",
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( value, result );
	}

	[Fact]
	public void AllowBinary() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"10101100",
				NumberStyles.AllowBinarySpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 172u ), result );
	}
}

public class TryParseSpan {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( s.AsSpan(), ref result ) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( SmallNaturalString.AsSpan(), ref result ) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( MediumNaturalString.AsSpan(), ref result ) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( LargeNaturalString.AsSpan(), ref result ) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1".AsSpan(), ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{s}".AsSpan(), ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1".AsSpan(), ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{s}".AsSpan(), ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1{s}".AsSpan(), ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{CurrentCultureNumberFormat.PositiveSign}1".AsSpan(), ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"{CurrentCultureNumberFormat.NegativeSign}1".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{CurrentCultureNumberFormat.NegativeSign}0".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowBothLeading() {
		// I would like for this to be a theory with the two cases separate
		// However, I can't do string interpolation in an attribute
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"{CurrentCultureNumberFormat.PositiveSign}{CurrentCultureNumberFormat.NegativeSign}0".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );

		result = DeadBeef;
		Assert.False( Overload.TryParse( $"{CurrentCultureNumberFormat.NegativeSign}{CurrentCultureNumberFormat.PositiveSign}0".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingPositive() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.PositiveSign}".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingNegative() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.NegativeSign}".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"0{CurrentCultureNumberFormat.NegativeSign}".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowParentheses() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( "(1)".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowParenthesesZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( "(0)".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowDecimalPoint() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.NumberDecimalSeparator}1".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.NumberDecimalSeparator}0".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowGroupSeparator() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{CurrentCultureNumberFormat.NumberGroupSeparator}234".AsSpan(), ref result ) );
		Assert.Equal( new Natural( 1234u ), result );
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"10{exp}1".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"{CurrentCultureNumberFormat.CurrencySymbol}1".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( $"1{CurrentCultureNumberFormat.CurrencySymbol}".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowHex() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( "1A".AsSpan(), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( "11".AsSpan(), ref result ) );
		Assert.Equal( new Natural( 11u ), result );
	}
}

public class TryParseSpanFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( s.AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( SmallNaturalString.AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( MediumNaturalString.AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( LargeNaturalString.AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1".AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{s}".AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1".AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( $"1{s}".AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( $"{s}1{s}".AsSpan(), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( $"{culture.NumberFormat.PositiveSign}1".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"{culture.NumberFormat.NegativeSign}1".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( $"{culture.NumberFormat.NegativeSign}0".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False( Overload.TryParse( $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.PositiveSign}".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingNegative() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.NegativeSign}".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"0{culture.NumberFormat.NegativeSign}".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowParentheses() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( "(1)".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowParenthesesZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( "(0)".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowDecimalPoint() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.NumberDecimalSeparator}1".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( $"1{culture.NumberFormat.NumberGroupSeparator}234".AsSpan(), culture, ref result ) );
			Assert.Equal( new Natural( 1234u ), result );
		}
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"10{exp}1".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"{culture.NumberFormat.CurrencySymbol}1".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( $"1{culture.NumberFormat.CurrencySymbol}".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowHex() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( "1A".AsSpan(), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( "11".AsSpan(), culture, ref result ) );
			Assert.Equal( new Natural( 11u ), result );
		}
	}
}

public class TryParseSpanStyleFormat {
	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				s.AsSpan(),
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				SmallNaturalString.AsSpan(),
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				MediumNaturalString.AsSpan(),
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				LargeNaturalString.AsSpan(),
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"{s}1".AsSpan(),
				NumberStyles.AllowLeadingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"1{s}".AsSpan(),
				NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"{s}1".AsSpan(),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"1{s}".AsSpan(),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				$"{s}1{s}".AsSpan(),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"{culture.NumberFormat.PositiveSign}1".AsSpan(),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"{culture.NumberFormat.NegativeSign}1".AsSpan(),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"{culture.NumberFormat.NegativeSign}0".AsSpan(),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0".AsSpan(),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowTrailingPositive() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"1{culture.NumberFormat.PositiveSign}".AsSpan(),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void TrailingNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"1{culture.NumberFormat.NegativeSign}".AsSpan(),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowTrailingNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"0{culture.NumberFormat.NegativeSign}".AsSpan(),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothTrailing() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}".AsSpan(),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}".AsSpan(),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					"(1)".AsSpan(),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowParenthesesNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					"(0)".AsSpan(),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOnlyOpen() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					"(0".AsSpan(),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOnlyClose() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					"0)".AsSpan(),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOutOfOrder() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					")0(".AsSpan(),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesMoreThanOne() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					"((0))".AsSpan(),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DecimalPointOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"1{culture.NumberFormat.NumberDecimalSeparator}1".AsSpan(),
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowDecimalPointZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(),
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void DecimalMoreThanOne() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					$"1{culture.NumberFormat.NumberDecimalSeparator}0{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(),
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890".AsSpan(),
					NumberStyles.AllowThousands,
					culture,
					ref result
				) );
			Assert.Equal( SmallNatural, result );
		}
	}

	[Fact]
	public void AllowExponent() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1e1".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 10u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"2E2".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 200u ), result );
	}

	[Fact]
	public void AllowExponentWithDecimal() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1.2e1".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 12u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"2.01E2".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 201u ), result );
	}

	[Fact]
	public void AllowNegativeExponentWithDecimal() {
		// Yes, these look silly, but they are technically valid
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"10.0e-1".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"200.0E-2".AsSpan(),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 2u ), result );
	}

	[Fact]
	public void AllowExponentWithPositiveSign() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1e+1".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 10u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"2E+2".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 200u ), result );
	}

	[Fact]
	public void AllowExponentWithNegativeSign() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"10e-1".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"200E-2".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 2u ), result );
	}

	[Fact]
	public void ExponentWithNegativeSignOverflow() {
		Natural result = DeadBeef;
		Assert.False(
			Overload.TryParse(
				"1e-1".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Zero, result );

		result = DeadBeef;
		Assert.False(
			Overload.TryParse(
				"2E-2".AsSpan(),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"{culture.NumberFormat.CurrencySymbol}1".AsSpan(),
					NumberStyles.AllowCurrencySymbol,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void AllowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					$"1{culture.NumberFormat.CurrencySymbol}".AsSpan(),
					NumberStyles.AllowCurrencySymbol,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void AllowHex() {
		// If you're curious, that's equal to "1,311,768,467,294,899,695"
		// It was verified with the Windows Calculator app
		Natural value = FListNatural(  305419896u, 2427178479u  );

		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1234567890ABCDEF".AsSpan(),
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( value, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"1234567890ABCDEF".AsSpan(),
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( value, result );
	}

	[Fact]
	public void AllowBinary() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				"10101100".AsSpan(),
				NumberStyles.AllowBinarySpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 172u ), result );
	}
}

public class TryParseUtf8 {
	private static ReadOnlySpan<byte> toSpan( string s ) =>
		new ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes( s ) );

	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( s ), ref result ) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( SmallNaturalString ), ref result ) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( MediumNaturalString ), ref result ) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( LargeNaturalString ), ref result ) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"{s}1" ), ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"1{s}" ), ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"{s}1" ), ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"1{s}" ), ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"{s}1{s}" ), ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"{CurrentCultureNumberFormat.PositiveSign}1" ), ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"{CurrentCultureNumberFormat.NegativeSign}1" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"{CurrentCultureNumberFormat.NegativeSign}0" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowBothLeading() {
		// I would like for this to be a theory with the two cases separate
		// However, I can't do string interpolation in an attribute
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"{CurrentCultureNumberFormat.PositiveSign}{CurrentCultureNumberFormat.NegativeSign}0" ), ref result ) );
		Assert.Equal( Natural.Zero, result );

		result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"{CurrentCultureNumberFormat.NegativeSign}{CurrentCultureNumberFormat.PositiveSign}0" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingPositive() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"1{CurrentCultureNumberFormat.PositiveSign}" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingNegative() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"1{CurrentCultureNumberFormat.NegativeSign}" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"0{CurrentCultureNumberFormat.NegativeSign}" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowParentheses() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( "(1)" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowParenthesesZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( "(0)" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowDecimalPoint() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"1{CurrentCultureNumberFormat.NumberDecimalSeparator}1" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"1{CurrentCultureNumberFormat.NumberDecimalSeparator}0" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowGroupSeparator() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"1{CurrentCultureNumberFormat.NumberGroupSeparator}234" ), ref result ) );
		Assert.Equal( new Natural( 1234u ), result );
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"10{exp}1" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"{CurrentCultureNumberFormat.CurrencySymbol}1" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( $"1{CurrentCultureNumberFormat.CurrencySymbol}" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void DisallowHex() {
		Natural result = DeadBeef;
		Assert.False( Overload.TryParse( toSpan( "1A" ), ref result ) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( "11" ), ref result ) );
		Assert.Equal( new Natural( 11u ), result );
	}
}

public class TryParseUtf8Format {
	private static ReadOnlySpan<byte> toSpan( string s ) =>
		new ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes( s ) );

	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( s ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( SmallNaturalString ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( MediumNaturalString ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( LargeNaturalString ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"{s}1" ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"1{s}" ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"{s}1" ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"1{s}" ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True( Overload.TryParse( toSpan( $"{s}1{s}" ), CurrentCultureNumberFormat, ref result ) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( toSpan( $"{culture.NumberFormat.PositiveSign}1" ), culture, ref result ) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"{culture.NumberFormat.NegativeSign}1" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( toSpan( $"{culture.NumberFormat.NegativeSign}0" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingPositive() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"1{culture.NumberFormat.PositiveSign}" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingNegative() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"1{culture.NumberFormat.NegativeSign}" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowTrailingNegativeZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"0{culture.NumberFormat.NegativeSign}" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowParentheses() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( "(1)" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowParenthesesZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( "(0)" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowDecimalPoint() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"1{culture.NumberFormat.NumberDecimalSeparator}1" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowDecimalPointZero() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"1{culture.NumberFormat.NumberDecimalSeparator}0" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( toSpan( $"1{culture.NumberFormat.NumberGroupSeparator}234" ), culture, ref result ) );
			Assert.Equal( new Natural( 1234u ), result );
		}
	}

	[Theory]
	[InlineData( "e" )]
	[InlineData( "E" )]
	[InlineData( "e-" )]
	[InlineData( "E-" )]
	public void DisallowExponent( string exp ) {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"10{exp}1" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"{culture.NumberFormat.CurrencySymbol}1" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( $"1{culture.NumberFormat.CurrencySymbol}" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowHex() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.False( Overload.TryParse( toSpan( "1A" ), culture, ref result ) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ReadBinaryAsDecimal() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True( Overload.TryParse( toSpan( "11" ), culture, ref result ) );
			Assert.Equal( new Natural( 11u ), result );
		}
	}
}

public class TryParseUtf8StyleFormat {
	private static ReadOnlySpan<byte> toSpan( string s ) =>
		new ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes( s ) );

	public static readonly System.Collections.Generic.IEnumerable<object[]> Whitespace = Helpers.Whitespace;

	[Theory]
	[InlineData( 0u, "0" )]          // Sanity
	[InlineData( 1u, "1" )]          // Sanity
	[InlineData( 123u, "123" )]      // multiple bits
	[InlineData( 45678u, "45678" )]  // rev
	public void Sanity( uint n, string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( s ),
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( n ), result );
	}

	[Fact]
	public void BiggerSanity() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( SmallNaturalString ),
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( SmallNatural, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( MediumNaturalString ),
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( MediumNatural, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( LargeNaturalString ),
				NumberStyles.Integer,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( LargeNatural, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowLeadingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( $"{s}1" ),
				NumberStyles.AllowLeadingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowTrailingWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( $"1{s}" ),
				NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Theory]
	[MemberData( nameof( Whitespace ) )]
	public void AllowWhiteSpace( string s ) {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( $"{s}1" ),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( $"1{s}" ),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( $"{s}1{s}" ),
				NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );
	}

	[Fact]
	public void AllowLeadingPositive() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( $"{culture.NumberFormat.PositiveSign}1" ),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void LeadingNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( $"{culture.NumberFormat.NegativeSign}1" ),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowLeadingNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( $"{culture.NumberFormat.NegativeSign}0" ),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothLeading() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0" ),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0" ),
					NumberStyles.AllowLeadingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowTrailingPositive() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( $"1{culture.NumberFormat.PositiveSign}" ),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void TrailingNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( $"1{culture.NumberFormat.NegativeSign}" ),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowTrailingNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( $"0{culture.NumberFormat.NegativeSign}" ),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DisallowBothTrailing() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}" ),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );

			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( $"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}" ),
					NumberStyles.AllowTrailingSign,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesNegativeOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( "(1)" ),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowParenthesesNegativeZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( "(0)" ),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOnlyOpen() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( "(0" ),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOnlyClose() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( "0 )" ),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesOutOfOrder() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( " )0(" ),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void ParenthesesMoreThanOne() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( "((0))" ),
					NumberStyles.AllowParentheses,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void DecimalPointOverflow() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( $"1{culture.NumberFormat.NumberDecimalSeparator}1" ),
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowDecimalPointZero() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( $"1{culture.NumberFormat.NumberDecimalSeparator}0" ),
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void DecimalMoreThanOne() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.False(
				Overload.TryParse(
					toSpan( $"1{culture.NumberFormat.NumberDecimalSeparator}0{culture.NumberFormat.NumberDecimalSeparator}0" ),
					NumberStyles.AllowDecimalPoint,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Zero, result );
		}
	}

	[Fact]
	public void AllowGroupSeparator() {
		Natural result = DeadBeef;
		foreach( CultureInfo culture in Cultures ) {
			result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890" ),
					NumberStyles.AllowThousands,
					culture,
					ref result
				) );
			Assert.Equal( SmallNatural, result );
		}
	}

	[Fact]
	public void AllowExponent() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "1e1" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 10u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "2E2" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 200u ), result );
	}

	[Fact]
	public void AllowExponentWithDecimal() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "1.2e1" ),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 12u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "2.01E2" ),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 201u ), result );
	}

	[Fact]
	public void AllowNegativeExponentWithDecimal() {
		// Yes, these look silly, but they are technically valid
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "10.0e-1" ),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "200.0E-2" ),
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 2u ), result );
	}

	[Fact]
	public void AllowExponentWithPositiveSign() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "1e+1" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 10u ), result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "2E+2" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 200u ), result );
	}

	[Fact]
	public void AllowExponentWithNegativeSign() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "10e-1" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Unit, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "200E-2" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 2u ), result );
	}

	[Fact]
	public void ExponentWithNegativeSignOverflow() {
		Natural result = DeadBeef;
		Assert.False(
			Overload.TryParse(
				toSpan( "1e-1" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Zero, result );

		result = DeadBeef;
		Assert.False(
			Overload.TryParse(
				toSpan( "2E-2" ),
				NumberStyles.AllowExponent,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( Natural.Zero, result );
	}

	[Fact]
	public void AllowCurrencySymbolPrefix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( $"{culture.NumberFormat.CurrencySymbol}1" ),
					NumberStyles.AllowCurrencySymbol,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void AllowCurrencySymbolPostfix() {
		foreach( CultureInfo culture in Cultures ) {
			Natural result = DeadBeef;
			Assert.True(
				Overload.TryParse(
					toSpan( $"1{culture.NumberFormat.CurrencySymbol}" ),
					NumberStyles.AllowCurrencySymbol,
					culture,
					ref result
				) );
			Assert.Equal( Natural.Unit, result );
		}
	}

	[Fact]
	public void AllowHex() {
		// If you're curious, that's equal to "1,311,768,467,294,899,695"
		// It was verified with the Windows Calculator app
		Natural value = FListNatural(  305419896u, 2427178479u  );

		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "1234567890ABCDEF" ),
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( value, result );

		result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "1234567890ABCDEF" ),
				NumberStyles.AllowHexSpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( value, result );
	}

	[Fact]
	public void AllowBinary() {
		Natural result = DeadBeef;
		Assert.True(
			Overload.TryParse(
				toSpan( "10101100" ),
				NumberStyles.AllowBinarySpecifier,
				CurrentCultureNumberFormat,
				ref result
			) );
		Assert.Equal( new Natural( 172u ), result );
	}
}