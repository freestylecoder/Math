namespace Natural

open Xunit
open Freestylecoding.Math

type public Formattable() =
    let small  = Natural( [0x4996_02D2u] )
    let smallStr = "1234567890"

    let medium = Natural( [0xAB54_A98Cu; 0xEB1F_0AD2u] )
    let mediumStr = "12345678901234567890"

    let large  = Natural( [0x0000_0001u; 0x8EE9_0FF6u; 0xC373_E0EEu; 0x4E3F_0AD2u] )
    let largeStr = "123456789012345678901234567890"

    let usCulture = System.Globalization.CultureInfo( "en-US" ) // US
    let ukCulture = System.Globalization.CultureInfo( "en-GB" ) // UK
    let frCulture = System.Globalization.CultureInfo( "fr-FR" ) // France
    let luCulture = System.Globalization.CultureInfo( "fr-LU" ) // Luxembourg

    let cultures = [| usCulture; ukCulture; frCulture; luCulture|]

    static member Format( this:'T when 'T :> System.IFormattable, format:string, formatProvider:System.IFormatProvider ) : string =
        this.ToString( format, formatProvider )

    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            "1",
            Formattable.Format( Natural.Unit, null, null )
        )

    [<Theory>]
    [<InlineData( "B", Skip = "Not Supported until dotnet8" )>]
    [<InlineData( "C" )>]
    [<InlineData( "D" )>]
    [<InlineData( "E" )>]
    [<InlineData( "F" )>]
    [<InlineData( "G" )>]
    [<InlineData( "N" )>]
    [<InlineData( "P" )>]
    [<InlineData( "R" )>]
    [<InlineData( "D" )>]
    [<InlineData( "b", Skip = "Not Supported until dotnet8" )>]
    [<InlineData( "c" )>]
    [<InlineData( "d" )>]
    [<InlineData( "e" )>]
    [<InlineData( "f" )>]
    [<InlineData( "g" )>]
    [<InlineData( "n" )>]
    [<InlineData( "p" )>]
    [<InlineData( "r" )>]
    [<InlineData( "x" )>]
    member public this.StandardFormatSpecifiers format =
        Assert.Equal(
            Formattable.Format( 0x4996_02D2u, format, null ),
            Formattable.Format( small, format, null )
        )

    [<Fact>]
    member public this.GeneralAndRoundTrip () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        Assert.Equal( smallStr, Formattable.Format( small, "G", null ) )
        Assert.Equal( smallStr, Formattable.Format( small, "g", null ) )
        Assert.Equal( smallStr, Formattable.Format( small, "R", null ) )
        Assert.Equal( smallStr, Formattable.Format( small, "r", null ) )

        Assert.Equal( mediumStr, Formattable.Format( medium, "G", null ) )
        Assert.Equal( mediumStr, Formattable.Format( medium, "g", null ) )
        Assert.Equal( mediumStr, Formattable.Format( medium, "R", null ) )
        Assert.Equal( mediumStr, Formattable.Format( medium, "r", null ) )

        Assert.Equal( largeStr, Formattable.Format( large, "G", null ) )
        Assert.Equal( largeStr, Formattable.Format( large, "g", null ) )
        Assert.Equal( largeStr, Formattable.Format( large, "R", null ) )
        Assert.Equal( largeStr, Formattable.Format( large, "r", null ) )

    [<Fact>]
    member public this.GeneralAndRoundTrip_IgnorePrecision () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        for i in 0 .. 20 do
            Assert.Equal( smallStr, Formattable.Format( small, $"G{i}", null ) )
            Assert.Equal( smallStr, Formattable.Format( small, $"g{i}", null ) )
            Assert.Equal( smallStr, Formattable.Format( small, $"R{i}", null ) )
            Assert.Equal( smallStr, Formattable.Format( small, $"r{i}", null ) )

            Assert.Equal( mediumStr, Formattable.Format( medium, $"G{i}", null ) )
            Assert.Equal( mediumStr, Formattable.Format( medium, $"g{i}", null ) )
            Assert.Equal( mediumStr, Formattable.Format( medium, $"R{i}", null ) )
            Assert.Equal( mediumStr, Formattable.Format( medium, $"r{i}", null ) )

            Assert.Equal( largeStr, Formattable.Format( large, $"G{i}", null ) )
            Assert.Equal( largeStr, Formattable.Format( large, $"g{i}", null ) )
            Assert.Equal( largeStr, Formattable.Format( large, $"R{i}", null ) )
            Assert.Equal( largeStr, Formattable.Format( large, $"r{i}", null ) )

    [<Fact>]
    member public this.GeneralAndRoundTrip_IgnoreFormatProvider () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        for culture in cultures do
            Assert.Equal( smallStr, Formattable.Format( small, "G", culture ) )
            Assert.Equal( smallStr, Formattable.Format( small, "g", culture ) )
            Assert.Equal( smallStr, Formattable.Format( small, "R", culture ) )
            Assert.Equal( smallStr, Formattable.Format( small, "r", culture ) )

            Assert.Equal( mediumStr, Formattable.Format( medium, "G", culture ) )
            Assert.Equal( mediumStr, Formattable.Format( medium, "g", culture ) )
            Assert.Equal( mediumStr, Formattable.Format( medium, "R", culture ) )
            Assert.Equal( mediumStr, Formattable.Format( medium, "r", culture ) )

            Assert.Equal( largeStr, Formattable.Format( large, "G", culture ) )
            Assert.Equal( largeStr, Formattable.Format( large, "g", culture ) )
            Assert.Equal( largeStr, Formattable.Format( large, "R", culture ) )
            Assert.Equal( largeStr, Formattable.Format( large, "r", culture ) )

    [<Fact>]
    member public this.Decimal () =
        Assert.Equal( smallStr, Formattable.Format( small, "D", null ) )
        Assert.Equal( smallStr, Formattable.Format( small, "d", null ) )

        Assert.Equal( mediumStr, Formattable.Format( medium, "D", null ) )
        Assert.Equal( mediumStr, Formattable.Format( medium, "d", null ) )

        Assert.Equal( largeStr, Formattable.Format( large, "D", null ) )
        Assert.Equal( largeStr, Formattable.Format( large, "d", null ) )

    [<Fact>]
    member public this.Decimal_Precision () =
        Assert.Equal(       smallStr, Formattable.Format( small,  "D0", null ) )
        Assert.Equal(       smallStr, Formattable.Format( small,  "D4", null ) )
        Assert.Equal(       smallStr, Formattable.Format( small,  "D8", null ) )
        Assert.Equal( "001234567890", Formattable.Format( small, "D12", null ) )

    [<Fact>]
    member public this.Decimal_IgnoreFormatProvider () =
        for culture in cultures do
            Assert.Equal( smallStr, Formattable.Format( small, "D", culture ) )
            Assert.Equal( smallStr, Formattable.Format( small, "d", culture ) )

            Assert.Equal( mediumStr, Formattable.Format( medium, "D", culture ) )
            Assert.Equal( mediumStr, Formattable.Format( medium, "d", culture ) )

            Assert.Equal( largeStr, Formattable.Format( large, "D", culture ) )
            Assert.Equal( largeStr, Formattable.Format( large, "d", culture ) )

    [<Fact>]
    member public this.Hexadecimal () =
        Assert.Equal( "499602D2", Formattable.Format( small, "X", null ) )
        Assert.Equal( "499602d2", Formattable.Format( small, "x", null ) )

        Assert.Equal( "AB54A98CEB1F0AD2", Formattable.Format( medium, "X", null ) )
        Assert.Equal( "ab54a98ceb1f0ad2", Formattable.Format( medium, "x", null ) )

        Assert.Equal( "18EE90FF6C373E0EE4E3F0AD2", Formattable.Format( large, "X", null ) )
        Assert.Equal( "18ee90ff6c373e0ee4e3f0ad2", Formattable.Format( large, "x", null ) )

    [<Fact>]
    member public this.Hexadecimal_Precision () =
        Assert.Equal(     "499602D2", Formattable.Format( small,  "X0", null ) )
        Assert.Equal(     "499602D2", Formattable.Format( small,  "X4", null ) )
        Assert.Equal(     "499602D2", Formattable.Format( small,  "X8", null ) )
        Assert.Equal( "0000499602D2", Formattable.Format( small, "X12", null ) )

    [<Fact>]
    member public this.Hexadecimal_IgnoreFormatProvider () =
        for culture in cultures do
            Assert.Equal( "499602D2", Formattable.Format( small, "X", culture ) )
            Assert.Equal( "499602d2", Formattable.Format( small, "x", culture ) )

            Assert.Equal( "AB54A98CEB1F0AD2", Formattable.Format( medium, "X", culture ) )
            Assert.Equal( "ab54a98ceb1f0ad2", Formattable.Format( medium, "x", culture ) )

            Assert.Equal( "18EE90FF6C373E0EE4E3F0AD2", Formattable.Format( large, "X", culture ) )
            Assert.Equal( "18ee90ff6c373e0ee4e3f0ad2", Formattable.Format( large, "x", culture ) )

    [<Fact( Skip = "Not Supported until dotnet8" )>]
    member public this.Binary () =
        Assert.Equal(
            "1001001100101100000001011010010",
            Formattable.Format( small, "B", null )
        )
        Assert.Equal(
            "1001001100101100000001011010010",
            Formattable.Format( small, "b", null )
        )

        Assert.Equal(
            "1010101101010100101010011000110011101011000111110000101011010010",
            Formattable.Format( medium, "B", null )
        )
        Assert.Equal(
            "1010101101010100101010011000110011101011000111110000101011010010",
            Formattable.Format( medium, "b", null )
        )

        Assert.Equal(
            "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
            Formattable.Format( large, "B", null )
        )
        Assert.Equal(
            "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
            Formattable.Format( large, "b", null )
        )

    [<Fact( Skip = "Not Supported until dotnet8" )>]
    member public this.Binary_Precision () =
        Assert.Equal(  "1001001100101100000001011010010", Formattable.Format( small,  "B0", null ) )
        Assert.Equal(  "1001001100101100000001011010010", Formattable.Format( small,  "B8", null ) )
        Assert.Equal(  "1001001100101100000001011010010", Formattable.Format( small, "B16", null ) )
        Assert.Equal( "01001001100101100000001011010010", Formattable.Format( small, "B32", null ) )

    [<Fact( Skip = "Not Supported until dotnet8" )>]
    member public this.Binary_IgnoreFormatProvider () =
        for culture in cultures do
            Assert.Equal(
                "1001001100101100000001011010010",
                Formattable.Format( small, "X", culture )
            )
            Assert.Equal(
                "1001001100101100000001011010010",
                Formattable.Format( small, "x", culture )
            )

            Assert.Equal(
                "1010101101010100101010011000110011101011000111110000101011010010",
                Formattable.Format( medium, "X", culture )
            )
            Assert.Equal(
                "1010101101010100101010011000110011101011000111110000101011010010",
                Formattable.Format( medium, "x", culture )
            )

            Assert.Equal(
                "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
                Formattable.Format( large, "X", culture )
            )
            Assert.Equal(
                "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
                Formattable.Format( large, "x", culture )
            )

    [<Fact>]
    member public this.Currency () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        Assert.Equal( $"$1,234,567,890{defaultDigits}", Formattable.Format( small, "C", null ) )
        Assert.Equal( $"$1,234,567,890{defaultDigits}", Formattable.Format( small, "c", null ) )

        Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", Formattable.Format( medium, "C", null ) )
        Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", Formattable.Format( medium, "c", null ) )

        Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", Formattable.Format( large, "C", null ) )
        Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", Formattable.Format( large, "c", null ) )

    [<Fact>]
    member public this.Currency_Precision () =
        Assert.Equal( "$1,234,567,890"             , Formattable.Format( small,  "C0", null ) )
        Assert.Equal( "$1,234,567,890.0000"        , Formattable.Format( small,  "C4", null ) )
        Assert.Equal( "$1,234,567,890.00000000"    , Formattable.Format( small,  "C8", null ) )
        Assert.Equal( "$1,234,567,890.000000000000", Formattable.Format( small, "C12", null ) )

    [<Fact>]
    member public this.Currency_FormatProvider () =
        Assert.Equal( "$1,234.00",  Formattable.Format( Natural( 1234u ), "C", usCulture ) )
        Assert.Equal( "£1,234.00",  Formattable.Format( Natural( 1234u ), "C", ukCulture ) )
        Assert.Equal( "1 234,00 €", Formattable.Format( Natural( 1234u ), "C", frCulture ) )
        Assert.Equal( "1.234,00 €", Formattable.Format( Natural( 1234u ), "C", luCulture ) )

    [<Fact>]
    member public this.Currency_Patterns () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.CurrencyPositivePattern <- 0
        Assert.Equal( "$1,234.00",  Formattable.Format( Natural( 1234u ), "C", custom ) )

        custom.CurrencyPositivePattern <- 1
        Assert.Equal( "1,234.00$",  Formattable.Format( Natural( 1234u ), "C", custom ) )

        custom.CurrencyPositivePattern <- 2
        Assert.Equal( "$ 1,234.00",  Formattable.Format( Natural( 1234u ), "C", custom ) )

        custom.CurrencyPositivePattern <- 3
        Assert.Equal( "1,234.00 $",  Formattable.Format( Natural( 1234u ), "C", custom ) )

    [<Fact>]
    member public this.Currency_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.CurrencyGroupSizes <- [| 5; 4; 3 |]
        Assert.Equal( "$12,345,678,901,2345,67890.00",  Formattable.Format( medium, "C", custom ) )

    [<Fact>]
    member public this.FixedPoint () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}"

        Assert.Equal( $"1234567890{defaultDigits}", Formattable.Format( small, "F", null ) )
        Assert.Equal( $"1234567890{defaultDigits}", Formattable.Format( small, "f", null ) )

        Assert.Equal( $"12345678901234567890{defaultDigits}", Formattable.Format( medium, "F", null ) )
        Assert.Equal( $"12345678901234567890{defaultDigits}", Formattable.Format( medium, "f", null ) )

        Assert.Equal( $"123456789012345678901234567890{defaultDigits}", Formattable.Format( large, "F", null ) )
        Assert.Equal( $"123456789012345678901234567890{defaultDigits}", Formattable.Format( large, "f", null ) )

    [<Fact>]
    member public this.FixedPoint_Precision () =
        Assert.Equal( "1234567890."            , Formattable.Format( small,  "F0", null ) )
        Assert.Equal( "1234567890.0000"        , Formattable.Format( small,  "F4", null ) )
        Assert.Equal( "1234567890.00000000"    , Formattable.Format( small,  "F8", null ) )
        Assert.Equal( "1234567890.000000000000", Formattable.Format( small, "F12", null ) )

    [<Fact>]
    member public this.FixedPoint_FormatProvider () =
        Assert.Equal( "1234.00",  Formattable.Format( Natural( 1234u ), "F", usCulture ) )
        Assert.Equal( "1234.000", Formattable.Format( Natural( 1234u ), "F", ukCulture ) )
        Assert.Equal( "1234,000", Formattable.Format( Natural( 1234u ), "F", frCulture ) )
        Assert.Equal( "1234,000", Formattable.Format( Natural( 1234u ), "F", luCulture ) )

    [<Fact>]
    member public this.Number () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        Assert.Equal( $"1,234,567,890{defaultDigits}", Formattable.Format( small, "N", null ) )
        Assert.Equal( $"1,234,567,890{defaultDigits}", Formattable.Format( small, "n", null ) )

        Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", Formattable.Format( medium, "N", null ) )
        Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", Formattable.Format( medium, "n", null ) )

        Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", Formattable.Format( large, "N", null ) )
        Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", Formattable.Format( large, "n", null ) )

    [<Fact>]
    member public this.Number_Precision () =
        Assert.Equal( "1,234,567,890"             , Formattable.Format( small,  "N0", null ) )
        Assert.Equal( "1,234,567,890.0000"        , Formattable.Format( small,  "N4", null ) )
        Assert.Equal( "1,234,567,890.00000000"    , Formattable.Format( small,  "N8", null ) )
        Assert.Equal( "1,234,567,890.000000000000", Formattable.Format( small, "N12", null ) )

    [<Fact>]
    member public this.Number_FormatProvider () =
        Assert.Equal( "1,234.00",  Formattable.Format( Natural( 1234u ), "N", usCulture ) )
        Assert.Equal( "1,234.000", Formattable.Format( Natural( 1234u ), "N", ukCulture ) )
        Assert.Equal( "1 234,000", Formattable.Format( Natural( 1234u ), "N", frCulture ) )
        Assert.Equal( "1.234,000", Formattable.Format( Natural( 1234u ), "N", luCulture ) )

    [<Fact>]
    member public this.Number_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.NumberGroupSizes <- [| 5; 4; 3 |]
        Assert.Equal( "12,345,678,901,2345,67890.00",  Formattable.Format( medium, "N", custom ) )

    [<Fact>]
    member public this.Percent () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}"

        Assert.Equal( $"123,456,789,000{defaultDigits}%%", Formattable.Format( small, "P", null ) )
        Assert.Equal( $"123,456,789,000{defaultDigits}%%", Formattable.Format( small, "p", null ) )

        Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%%", Formattable.Format( medium, "P", null ) )
        Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%%", Formattable.Format( medium, "p", null ) )

        Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", Formattable.Format( large, "P", null ) )
        Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", Formattable.Format( large, "p", null ) )

    [<Fact>]
    member public this.Percent_Precision () =
        Assert.Equal( "123,456,789,000%"             , Formattable.Format( small,  "P0", null ) )
        Assert.Equal( "123,456,789,000.0000%"        , Formattable.Format( small,  "P4", null ) )
        Assert.Equal( "123,456,789,000.00000000%"    , Formattable.Format( small,  "P8", null ) )
        Assert.Equal( "123,456,789,000.000000000000%", Formattable.Format( small, "P12", null ) )

    [<Fact>]
    member public this.Percent_FormatProvider () =
        Assert.Equal( "123,400.00%",   Formattable.Format( Natural( 1234u ), "P", usCulture ) )
        Assert.Equal( "123,400.000%",  Formattable.Format( Natural( 1234u ), "P", ukCulture ) )
        Assert.Equal( "123 400,000 %", Formattable.Format( Natural( 1234u ), "P", frCulture ) )
        Assert.Equal( "123.400,000 %", Formattable.Format( Natural( 1234u ), "P", luCulture ) )

    [<Fact>]
    member public this.Percent_Patterns () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.PercentPositivePattern <- 0
        Assert.Equal( "123,400.00 %",  Formattable.Format( Natural( 1234u ), "P", custom ) )

        custom.PercentPositivePattern <- 1
        Assert.Equal( "123,400.00%",  Formattable.Format( Natural( 1234u ), "P", custom ) )

        custom.PercentPositivePattern <- 2
        Assert.Equal( "%123,400.00",  Formattable.Format( Natural( 1234u ), "P", custom ) )

        custom.PercentPositivePattern <- 3
        Assert.Equal( "% 123,400.00",  Formattable.Format( Natural( 1234u ), "P", custom ) )

    [<Fact>]
    member public this.Percent_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.PercentGroupSizes <- [| 5; 4; 3 |]
        Assert.Equal( "1,234,567,890,123,4567,89000.00%",  Formattable.Format( medium, "P", custom ) )

    [<Fact>]
    member public this.Exponential () =
        Assert.Equal( "1.234568E+009", Formattable.Format( small, "E", null ) )
        Assert.Equal( "1.234568e+009", Formattable.Format( small, "e", null ) )

        Assert.Equal( "1.234568E+019", Formattable.Format( medium, "E", null ) )
        Assert.Equal( "1.234568e+019", Formattable.Format( medium, "e", null ) )

        Assert.Equal( "1.234568E+029", Formattable.Format( large, "E", null ) )
        Assert.Equal( "1.234568e+029", Formattable.Format( large, "e", null ) )

    [<Fact>]
    member public this.Exponential_Precision () =
        Assert.Equal( "1.E+009"            , Formattable.Format( small,  "E0", null ) )
        // Case E2 added to test no rounding
        Assert.Equal( "1.23E+009"          , Formattable.Format( small,  "E2", null ) )
        Assert.Equal( "1.2346E+009"        , Formattable.Format( small,  "E4", null ) )
        Assert.Equal( "1.23456789E+009"    , Formattable.Format( small,  "E8", null ) )
        // Case E9 added to test Exponent = Precision
        Assert.Equal( "1.234567890E+009"   , Formattable.Format( small,  "E9", null ) )
        Assert.Equal( "1.234567890000E+009", Formattable.Format( small, "E12", null ) )

    [<Fact>]
    member public this.Exponential_FormatProvider () =
        Assert.Equal( "1.234568E+009", Formattable.Format( small, "E", usCulture ) )
        Assert.Equal( "1.234568E+009", Formattable.Format( small, "E", ukCulture ) )
        Assert.Equal( "1,234568E+009", Formattable.Format( small, "E", frCulture ) )
        Assert.Equal( "1,234568E+009", Formattable.Format( small, "E", luCulture ) )

    [<Fact>]
    member public this.UnknownFormatSpecifier () =
        let exc = Record.Exception(
            fun () ->
                Formattable.Format( Natural.Unit, "Z", null )
                |> ignore
        )

        Assert.NotNull( exc )
        Assert.IsType<System.FormatException>( exc )