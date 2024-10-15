
namespace Natural

open Xunit
open Freestylecoding.Math

type public ObjectGetType() =
    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            typeof<Natural>,
            Natural.Unit.GetType()
        )

type public ObjectEquals() =
    [<Theory>]
    [<InlineData(  0u,  0u, true)>]
    [<InlineData(  1u,  0u, false)>]
    [<InlineData(  0u,  1u, false)>]
    [<InlineData(  1u,  1u, true)>]
    member public this.Sanity left right expected =
        Assert.Equal(
            expected,
            Natural( [left] ).Equals( Natural( [right] ) )
        )

    [<Fact>]
    member public this.LargeNaturals () =
        Assert.True(
            Natural( [0xFu; 0x00000101u] ).Equals( Natural( [0xFu; 0x00000101u] ) )
        )

    [<Fact>]
    member public this.HardcodedTypes () =
        Assert.True( Natural.Unit.Equals( byte   1 ) )
        Assert.True( Natural.Unit.Equals( uint16 1 ) )
        Assert.True( Natural.Unit.Equals( uint32 1 ) )
        Assert.True( Natural.Unit.Equals( uint64 1 ) )

        Assert.True( Natural.Unit.Equals( System.UInt128( 0uL, 1uL ) ) )
        Assert.True( Natural.Unit.Equals( System.Numerics.BigInteger 1 ) )

    [<Fact>]
    member public this.IntegralTypes () =
        Assert.True( Natural.Unit.Equals( sbyte 1 ) )
        Assert.True( Natural.Unit.Equals( int16 1 ) )
        Assert.True( Natural.Unit.Equals( int32 1 ) )
        Assert.True( Natural.Unit.Equals( int64 1 ) )

        Assert.True( Natural.Unit.Equals( System.Int128( 0uL, 1uL ) ) )

    [<Fact>]
    member public this.FloatingTypes () =
        Assert.True( Natural.Unit.Equals( float32 1 ) )
        Assert.True( Natural.Unit.Equals( float   1 ) )
        Assert.True( Natural.Unit.Equals( decimal 1 ) )

        Assert.True( Natural.Unit.Equals( System.Numerics.Complex( 1, 0 ) ) )

    [<Fact>]
    member public this.NotNumber () =
        Assert.False( Natural.Unit.Equals( "1" ) )
        Assert.False( Natural.Unit.Equals( true ) )

    [<Fact>]
    member public this.NegativeIntegralTypes () =
        Assert.False( Natural.Unit.Equals( sbyte -1 ) )
        Assert.False( Natural.Unit.Equals( int16 -1 ) )
        Assert.False( Natural.Unit.Equals( int32 -1 ) )
        Assert.False( Natural.Unit.Equals( int64 -1 ) )

        Assert.False( Natural.Unit.Equals( -System.Int128( 0uL, 1uL ) ) )
        Assert.False( Natural.Unit.Equals( -System.Numerics.BigInteger( 1m ) ) )

    [<Fact>]
    member public this.FloatingTypesWithDecimals () =
        Assert.False( Natural.Unit.Equals( float32 1.1 ) )
        Assert.False( Natural.Unit.Equals( float   1.1 ) )
        Assert.False( Natural.Unit.Equals( decimal 1.1 ) )

        Assert.False( Natural.Unit.Equals( System.Numerics.Complex( 1.1, 0 ) ) )

    [<Fact>]
    member public this.Imaginary () =
        // I'm making an assumption that an imaginary number is not an integer
        // This explicitly checks that assumption
        Assert.False( Natural.Unit.Equals( System.Numerics.Complex( 1, 1 ) ) )

type public ObjectGetHashCode() =
    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            Natural.Unit.GetHashCode(),
            Natural.Unit.GetHashCode()
        )

        Assert.NotEqual(
            Natural.Zero.GetHashCode(),
            Natural.Unit.GetHashCode()
        )

type public ObjectToString() =
    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity actual expected =
        Assert.Equal(
            expected,
            Natural( [actual] ).ToString()
        )

    [<Fact>]
    member public this.Bigger () =
        Assert.Equal(
            "1234567890123456789",
            Natural( [0x112210F4u; 0x7DE98115u] ).ToString()
        )

type public ToStringFormat() =
    let small  = Natural( [0x4996_02D2u] )
    let smallStr = "1234567890"

    let medium = Natural( [0xAB54_A98Cu; 0xEB1F_0AD2u] )
    let mediumStr = "12345678901234567890"

    let large  = Natural( [0x0000_0001u; 0x8EE9_0FF6u; 0xC373_E0EEu; 0x4E3F_0AD2u] )
    let largeStr = "123456789012345678901234567890"

    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            "1",
            Natural.Unit.ToString( null )
        )

    [<Theory>]
    [<InlineData( "B" )>]//, Skip = "Not Supported until dotnet8" )>]
    [<InlineData( "C" )>]
    [<InlineData( "D" )>]
    [<InlineData( "E" )>]
    [<InlineData( "F" )>]
    [<InlineData( "G" )>]
    [<InlineData( "N" )>]
    [<InlineData( "P" )>]
    [<InlineData( "R" )>]
    [<InlineData( "D" )>]
    [<InlineData( "b" )>]//, Skip = "Not Supported until dotnet8" )>]
    [<InlineData( "c" )>]
    [<InlineData( "d" )>]
    [<InlineData( "e" )>]
    [<InlineData( "f" )>]
    [<InlineData( "g" )>]
    [<InlineData( "n" )>]
    [<InlineData( "p" )>]
    [<InlineData( "r" )>]
    [<InlineData( "x" )>]
    member public this.StandardFormatSpecifiers (format:string) =
        Assert.Equal(
            0x4996_02D2u.ToString( format ),
            small.ToString( format )
        )

    [<Fact>]
    member public this.GeneralAndRoundTrip () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        Assert.Equal( smallStr, small.ToString( "G" ) )
        Assert.Equal( smallStr, small.ToString( "g" ) )
        Assert.Equal( smallStr, small.ToString( "R" ) )
        Assert.Equal( smallStr, small.ToString( "r" ) )

        Assert.Equal( mediumStr, medium.ToString( "G" ) )
        Assert.Equal( mediumStr, medium.ToString( "g" ) )
        Assert.Equal( mediumStr, medium.ToString( "R" ) )
        Assert.Equal( mediumStr, medium.ToString( "r" ) )

        Assert.Equal( largeStr, large.ToString( "G" ) )
        Assert.Equal( largeStr, large.ToString( "g" ) )
        Assert.Equal( largeStr, large.ToString( "R" ) )
        Assert.Equal( largeStr, large.ToString( "r" ) )

    [<Fact>]
    member public this.GeneralAndRoundTrip_IgnorePrecision () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        for i in 0 .. 20 do
            Assert.Equal( smallStr, small.ToString( $"G{i}" ) )
            Assert.Equal( smallStr, small.ToString( $"g{i}" ) )
            Assert.Equal( smallStr, small.ToString( $"R{i}" ) )
            Assert.Equal( smallStr, small.ToString( $"r{i}" ) )

            Assert.Equal( mediumStr, medium.ToString( $"G{i}" ) )
            Assert.Equal( mediumStr, medium.ToString( $"g{i}" ) )
            Assert.Equal( mediumStr, medium.ToString( $"R{i}" ) )
            Assert.Equal( mediumStr, medium.ToString( $"r{i}" ) )

            Assert.Equal( largeStr, large.ToString( $"G{i}" ) )
            Assert.Equal( largeStr, large.ToString( $"g{i}" ) )
            Assert.Equal( largeStr, large.ToString( $"R{i}" ) )
            Assert.Equal( largeStr, large.ToString( $"r{i}" ) )

    [<Fact>]
    member public this.Decimal () =
        Assert.Equal( smallStr, small.ToString( "D" ) )
        Assert.Equal( smallStr, small.ToString( "d" ) )

        Assert.Equal( mediumStr, medium.ToString( "D" ) )
        Assert.Equal( mediumStr, medium.ToString( "d" ) )

        Assert.Equal( largeStr, large.ToString( "D" ) )
        Assert.Equal( largeStr, large.ToString( "d" ) )

    [<Fact>]
    member public this.Decimal_Precision () =
        Assert.Equal(       smallStr, small.ToString(  "D0" ) )
        Assert.Equal(       smallStr, small.ToString(  "D4" ) )
        Assert.Equal(       smallStr, small.ToString(  "D8" ) )
        Assert.Equal( "001234567890", small.ToString( "D12" ) )

    [<Fact>]
    member public this.Hexadecimal () =
        Assert.Equal( "499602D2", small.ToString( "X" ) )
        Assert.Equal( "499602d2", small.ToString( "x" ) )

        Assert.Equal( "AB54A98CEB1F0AD2", medium.ToString( "X" ) )
        Assert.Equal( "ab54a98ceb1f0ad2", medium.ToString( "x" ) )

        Assert.Equal( "18EE90FF6C373E0EE4E3F0AD2", large.ToString( "X" ) )
        Assert.Equal( "18ee90ff6c373e0ee4e3f0ad2", large.ToString( "x" ) )

    [<Fact>]
    member public this.Hexadecimal_Precision () =
        Assert.Equal(     "499602D2", small.ToString(  "X0" ) )
        Assert.Equal(     "499602D2", small.ToString(  "X4" ) )
        Assert.Equal(     "499602D2", small.ToString(  "X8" ) )
        Assert.Equal( "0000499602D2", small.ToString( "X12" ) )

    [<Fact>]
    member public this.Binary () =
        Assert.Equal(
            "1001001100101100000001011010010",
            small.ToString( "B" )
        )
        Assert.Equal(
            "1001001100101100000001011010010",
            small.ToString( "b" )
        )

        Assert.Equal(
            "1010101101010100101010011000110011101011000111110000101011010010",
            medium.ToString( "B" )
        )
        Assert.Equal(
            "1010101101010100101010011000110011101011000111110000101011010010",
            medium.ToString( "b" )
        )

        Assert.Equal(
            "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
            large.ToString( "B" )
        )
        Assert.Equal(
            "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
            large.ToString( "b" )
        )

    [<Fact>]
    member public this.Binary_Precision () =
        Assert.Equal(  "1001001100101100000001011010010", small.ToString(  "B0" ) )
        Assert.Equal(  "1001001100101100000001011010010", small.ToString(  "B8" ) )
        Assert.Equal(  "1001001100101100000001011010010", small.ToString( "B16" ) )
        Assert.Equal( "01001001100101100000001011010010", small.ToString( "B32" ) )

    [<Fact>]
    member public this.Currency () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        Assert.Equal( $"$1,234,567,890{defaultDigits}", small.ToString( "C" ) )
        Assert.Equal( $"$1,234,567,890{defaultDigits}", small.ToString( "c" ) )

        Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "C" ) )
        Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "c" ) )

        Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "C" ) )
        Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "c" ) )

    [<Fact>]
    member public this.Currency_Precision () =
        Assert.Equal( "$1,234,567,890"             , small.ToString(  "C0" ) )
        Assert.Equal( "$1,234,567,890.0000"        , small.ToString(  "C4" ) )
        Assert.Equal( "$1,234,567,890.00000000"    , small.ToString(  "C8" ) )
        Assert.Equal( "$1,234,567,890.000000000000", small.ToString( "C12" ) )

    [<Fact>]
    member public this.FixedPoint () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}"

        Assert.Equal( $"1234567890{defaultDigits}", small.ToString( "F" ) )
        Assert.Equal( $"1234567890{defaultDigits}", small.ToString( "f" ) )

        Assert.Equal( $"12345678901234567890{defaultDigits}", medium.ToString( "F" ) )
        Assert.Equal( $"12345678901234567890{defaultDigits}", medium.ToString( "f" ) )

        Assert.Equal( $"123456789012345678901234567890{defaultDigits}", large.ToString( "F" ) )
        Assert.Equal( $"123456789012345678901234567890{defaultDigits}", large.ToString( "f" ) )

    [<Fact>]
    member public this.FixedPoint_Precision () =
        Assert.Equal( "1234567890."            , small.ToString(  "F0" ) )
        Assert.Equal( "1234567890.0000"        , small.ToString(  "F4" ) )
        Assert.Equal( "1234567890.00000000"    , small.ToString(  "F8" ) )
        Assert.Equal( "1234567890.000000000000", small.ToString( "F12" ) )

    [<Fact>]
    member public this.Number () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        Assert.Equal( $"1,234,567,890{defaultDigits}", small.ToString( "N" ) )
        Assert.Equal( $"1,234,567,890{defaultDigits}", small.ToString( "n" ) )

        Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "N" ) )
        Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "n" ) )

        Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "N" ) )
        Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "n" ) )

    [<Fact>]
    member public this.Number_Precision () =
        Assert.Equal( "1,234,567,890"             , small.ToString(  "N0" ) )
        Assert.Equal( "1,234,567,890.0000"        , small.ToString(  "N4" ) )
        Assert.Equal( "1,234,567,890.00000000"    , small.ToString(  "N8" ) )
        Assert.Equal( "1,234,567,890.000000000000", small.ToString( "N12" ) )

    [<Fact>]
    member public this.Percent () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}"

        Assert.Equal( $"123,456,789,000{defaultDigits}%%", small.ToString( "P" ) )
        Assert.Equal( $"123,456,789,000{defaultDigits}%%", small.ToString( "p" ) )

        Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%%", medium.ToString( "P" ) )
        Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%%", medium.ToString( "p" ) )

        Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", large.ToString( "P" ) )
        Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", large.ToString( "p" ) )

    [<Fact>]
    member public this.Percent_Precision () =
        Assert.Equal( "123,456,789,000%"             , small.ToString(  "P0" ) )
        Assert.Equal( "123,456,789,000.0000%"        , small.ToString(  "P4" ) )
        Assert.Equal( "123,456,789,000.00000000%"    , small.ToString(  "P8" ) )
        Assert.Equal( "123,456,789,000.000000000000%", small.ToString( "P12" ) )

    [<Fact>]
    member public this.Exponential () =
        Assert.Equal( "1.234568E+009", small.ToString( "E" ) )
        Assert.Equal( "1.234568e+009", small.ToString( "e" ) )

        Assert.Equal( "1.234568E+019", medium.ToString( "E" ) )
        Assert.Equal( "1.234568e+019", medium.ToString( "e" ) )

        Assert.Equal( "1.234568E+029", large.ToString( "E" ) )
        Assert.Equal( "1.234568e+029", large.ToString( "e" ) )

    [<Fact>]
    member public this.Exponential_Precision () =
        Assert.Equal( "1.E+009"            , small.ToString(  "E0" ) )
        // Case E2 added to test no rounding
        Assert.Equal( "1.23E+009"          , small.ToString(  "E2" ) )
        Assert.Equal( "1.2346E+009"        , small.ToString(  "E4" ) )
        Assert.Equal( "1.23456789E+009"    , small.ToString(  "E8" ) )
        // Case E9 added to test Exponent = Precision
        Assert.Equal( "1.234567890E+009"   , small.ToString(  "E9" ) )
        Assert.Equal( "1.234567890000E+009", small.ToString( "E12" ) )

    [<Fact>]
    member public this.UnknownFormatSpecifier () =
        let exc = Record.Exception(
            fun () ->
                Natural.Unit.ToString( "Z" )
                |> ignore
        )

        Assert.NotNull( exc )
        Assert.IsType<System.FormatException>( exc )