namespace Natural

open Xunit
open Freestylecoding.Math

type public Formattable() =
    let small  = Natural( [0x4996_02D2u] ) :> System.IFormattable
    let smallStr = "1234567890"

    let medium = Natural( [0xAB54_A98Cu; 0xEB1F_0AD2u] ) :> System.IFormattable
    let mediumStr = "12345678901234567890"

    let large  = Natural( [0x0000_0001u; 0x8EE9_0FF6u; 0xC373_E0EEu; 0x4E3F_0AD2u] ) :> System.IFormattable
    let largeStr = "123456789012345678901234567890"

    let usCulture = System.Globalization.CultureInfo( "en-US" ) // US
    let ukCulture = System.Globalization.CultureInfo( "en-GB" ) // UK
    let frCulture = System.Globalization.CultureInfo( "fr-FR" ) // France
    let luCulture = System.Globalization.CultureInfo( "fr-LU" ) // Luxembourg

    let cultures = [| usCulture; ukCulture; frCulture; luCulture|]

    let one = Natural.Unit :> System.IFormattable
    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            "1",
            one.ToString( null,  null )
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
    member public this.StandardFormatSpecifiers format =
        Assert.Equal(
            0x4996_02D2u.ToString( format,  null ),
            small.ToString( format,  null )
        )

    [<Fact>]
    member public this.GeneralAndRoundTrip () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        Assert.Equal( smallStr, small.ToString( "G",  null ) )
        Assert.Equal( smallStr, small.ToString( "g",  null ) )
        Assert.Equal( smallStr, small.ToString( "R",  null ) )
        Assert.Equal( smallStr, small.ToString( "r",  null ) )

        Assert.Equal( mediumStr, medium.ToString( "G",  null ) )
        Assert.Equal( mediumStr, medium.ToString( "g",  null ) )
        Assert.Equal( mediumStr, medium.ToString( "R",  null ) )
        Assert.Equal( mediumStr, medium.ToString( "r",  null ) )

        Assert.Equal( largeStr, large.ToString( "G",  null ) )
        Assert.Equal( largeStr, large.ToString( "g",  null ) )
        Assert.Equal( largeStr, large.ToString( "R",  null ) )
        Assert.Equal( largeStr, large.ToString( "r",  null ) )

    [<Fact>]
    member public this.GeneralAndRoundTrip_IgnorePrecision () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        for i in 0 .. 20 do
            Assert.Equal( smallStr, small.ToString( $"G{i}",  null ) )
            Assert.Equal( smallStr, small.ToString( $"g{i}",  null ) )
            Assert.Equal( smallStr, small.ToString( $"R{i}",  null ) )
            Assert.Equal( smallStr, small.ToString( $"r{i}",  null ) )

            Assert.Equal( mediumStr, medium.ToString( $"G{i}",  null ) )
            Assert.Equal( mediumStr, medium.ToString( $"g{i}",  null ) )
            Assert.Equal( mediumStr, medium.ToString( $"R{i}",  null ) )
            Assert.Equal( mediumStr, medium.ToString( $"r{i}",  null ) )

            Assert.Equal( largeStr, large.ToString( $"G{i}",  null ) )
            Assert.Equal( largeStr, large.ToString( $"g{i}",  null ) )
            Assert.Equal( largeStr, large.ToString( $"R{i}",  null ) )
            Assert.Equal( largeStr, large.ToString( $"r{i}",  null ) )

    [<Fact>]
    member public this.GeneralAndRoundTrip_IgnoreFormatProvider () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        for culture in cultures do
            Assert.Equal( smallStr, small.ToString( "G",  culture ) )
            Assert.Equal( smallStr, small.ToString( "g",  culture ) )
            Assert.Equal( smallStr, small.ToString( "R",  culture ) )
            Assert.Equal( smallStr, small.ToString( "r",  culture ) )

            Assert.Equal( mediumStr, medium.ToString( "G",  culture ) )
            Assert.Equal( mediumStr, medium.ToString( "g",  culture ) )
            Assert.Equal( mediumStr, medium.ToString( "R",  culture ) )
            Assert.Equal( mediumStr, medium.ToString( "r",  culture ) )

            Assert.Equal( largeStr, large.ToString( "G",  culture ) )
            Assert.Equal( largeStr, large.ToString( "g",  culture ) )
            Assert.Equal( largeStr, large.ToString( "R",  culture ) )
            Assert.Equal( largeStr, large.ToString( "r",  culture ) )

    [<Fact>]
    member public this.Decimal () =
        Assert.Equal( smallStr, small.ToString( "D",  null ) )
        Assert.Equal( smallStr, small.ToString( "d",  null ) )

        Assert.Equal( mediumStr, medium.ToString( "D",  null ) )
        Assert.Equal( mediumStr, medium.ToString( "d",  null ) )

        Assert.Equal( largeStr, large.ToString( "D",  null ) )
        Assert.Equal( largeStr, large.ToString( "d",  null ) )

    [<Fact>]
    member public this.Decimal_Precision () =
        Assert.Equal(       smallStr, small.ToString(  "D0",  null ) )
        Assert.Equal(       smallStr, small.ToString(  "D4",  null ) )
        Assert.Equal(       smallStr, small.ToString(  "D8",  null ) )
        Assert.Equal( "001234567890", small.ToString( "D12",  null ) )

    [<Fact>]
    member public this.Decimal_IgnoreFormatProvider () =
        for culture in cultures do
            Assert.Equal( smallStr, small.ToString( "D",  culture ) )
            Assert.Equal( smallStr, small.ToString( "d",  culture ) )

            Assert.Equal( mediumStr, medium.ToString( "D",  culture ) )
            Assert.Equal( mediumStr, medium.ToString( "d",  culture ) )

            Assert.Equal( largeStr, large.ToString( "D",  culture ) )
            Assert.Equal( largeStr, large.ToString( "d",  culture ) )

    [<Fact>]
    member public this.Hexadecimal () =
        Assert.Equal( "499602D2", small.ToString( "X",  null ) )
        Assert.Equal( "499602d2", small.ToString( "x",  null ) )

        Assert.Equal( "AB54A98CEB1F0AD2", medium.ToString( "X",  null ) )
        Assert.Equal( "ab54a98ceb1f0ad2", medium.ToString( "x",  null ) )

        Assert.Equal( "18EE90FF6C373E0EE4E3F0AD2", large.ToString( "X",  null ) )
        Assert.Equal( "18ee90ff6c373e0ee4e3f0ad2", large.ToString( "x",  null ) )

    [<Fact>]
    member public this.Hexadecimal_Precision () =
        Assert.Equal(     "499602D2", small.ToString(  "X0",  null ) )
        Assert.Equal(     "499602D2", small.ToString(  "X4",  null ) )
        Assert.Equal(     "499602D2", small.ToString(  "X8",  null ) )
        Assert.Equal( "0000499602D2", small.ToString( "X12",  null ) )

    [<Fact>]
    member public this.Hexadecimal_IgnoreFormatProvider () =
        for culture in cultures do
            Assert.Equal( "499602D2", small.ToString( "X",  culture ) )
            Assert.Equal( "499602d2", small.ToString( "x",  culture ) )

            Assert.Equal( "AB54A98CEB1F0AD2", medium.ToString( "X",  culture ) )
            Assert.Equal( "ab54a98ceb1f0ad2", medium.ToString( "x",  culture ) )

            Assert.Equal( "18EE90FF6C373E0EE4E3F0AD2", large.ToString( "X",  culture ) )
            Assert.Equal( "18ee90ff6c373e0ee4e3f0ad2", large.ToString( "x",  culture ) )

    [<Fact>]
    member public this.Binary () =
        Assert.Equal(
            "1001001100101100000001011010010",
            small.ToString( "B",  null )
        )
        Assert.Equal(
            "1001001100101100000001011010010",
            small.ToString( "b",  null )
        )

        Assert.Equal(
            "1010101101010100101010011000110011101011000111110000101011010010",
            medium.ToString( "B",  null )
        )
        Assert.Equal(
            "1010101101010100101010011000110011101011000111110000101011010010",
            medium.ToString( "b",  null )
        )

        Assert.Equal(
            "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
            large.ToString( "B",  null )
        )
        Assert.Equal(
            "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
            large.ToString( "b",  null )
        )

    [<Fact>]
    member public this.Binary_Precision () =
        Assert.Equal(  "1001001100101100000001011010010", small.ToString(  "B0",  null ) )
        Assert.Equal(  "1001001100101100000001011010010", small.ToString(  "B8",  null ) )
        Assert.Equal(  "1001001100101100000001011010010", small.ToString( "B16",  null ) )
        Assert.Equal( "01001001100101100000001011010010", small.ToString( "B32",  null ) )

    [<Fact>]
    member public this.Binary_IgnoreFormatProvider () =
        for culture in cultures do
            Assert.Equal(
                "1001001100101100000001011010010",
                small.ToString( "B",  culture )
            )
            Assert.Equal(
                "1001001100101100000001011010010",
                small.ToString( "b",  culture )
            )

            Assert.Equal(
                "1010101101010100101010011000110011101011000111110000101011010010",
                medium.ToString( "B",  culture )
            )
            Assert.Equal(
                "1010101101010100101010011000110011101011000111110000101011010010",
                medium.ToString( "b",  culture )
            )

            Assert.Equal(
                "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
                large.ToString( "B",  culture )
            )
            Assert.Equal(
                "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010",
                large.ToString( "b",  culture )
            )

    [<Fact>]
    member public this.Currency () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        Assert.Equal( $"$1,234,567,890{defaultDigits}", small.ToString( "C",  null ) )
        Assert.Equal( $"$1,234,567,890{defaultDigits}", small.ToString( "c",  null ) )

        Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "C",  null ) )
        Assert.Equal( $"$12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "c",  null ) )

        Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "C",  null ) )
        Assert.Equal( $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "c",  null ) )

    [<Fact>]
    member public this.Currency_Precision () =
        Assert.Equal( "$1,234,567,890"             , small.ToString(  "C0",  null ) )
        Assert.Equal( "$1,234,567,890.0000"        , small.ToString(  "C4",  null ) )
        Assert.Equal( "$1,234,567,890.00000000"    , small.ToString(  "C8",  null ) )
        Assert.Equal( "$1,234,567,890.000000000000", small.ToString( "C12",  null ) )

    [<Fact>]
    member public this.Currency_FormatProvider () =
        let testVal = Natural( 1234u ) :> System.IFormattable
        Assert.Equal( "$1,234.00",  testVal.ToString( "C",  usCulture ) )
        Assert.Equal( "£1,234.00",  testVal.ToString( "C",  ukCulture ) )
        Assert.Equal( "1 234,00 €", testVal.ToString( "C",  frCulture ) )
        Assert.Equal( "1.234,00 €", testVal.ToString( "C",  luCulture ) )

    [<Fact>]
    member public this.Currency_Patterns () =
        let testVal = Natural( 1234u ) :> System.IFormattable
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.CurrencyPositivePattern <- 0
        Assert.Equal( "$1,234.00",  testVal.ToString( "C",  custom ) )

        custom.CurrencyPositivePattern <- 1
        Assert.Equal( "1,234.00$",  testVal.ToString( "C",  custom ) )

        custom.CurrencyPositivePattern <- 2
        Assert.Equal( "$ 1,234.00",  testVal.ToString( "C",  custom ) )

        custom.CurrencyPositivePattern <- 3
        Assert.Equal( "1,234.00 $",  testVal.ToString( "C",  custom ) )

    [<Fact>]
    member public this.Currency_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.CurrencyGroupSizes <- [| 5; 4; 3 |]
        Assert.Equal( "$12,345,678,901,2345,67890.00",  medium.ToString( "C",  custom ) )

    [<Fact>]
    member public this.FixedPoint () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}"

        Assert.Equal( $"1234567890{defaultDigits}", small.ToString( "F",  null ) )
        Assert.Equal( $"1234567890{defaultDigits}", small.ToString( "f",  null ) )

        Assert.Equal( $"12345678901234567890{defaultDigits}", medium.ToString( "F",  null ) )
        Assert.Equal( $"12345678901234567890{defaultDigits}", medium.ToString( "f",  null ) )

        Assert.Equal( $"123456789012345678901234567890{defaultDigits}", large.ToString( "F",  null ) )
        Assert.Equal( $"123456789012345678901234567890{defaultDigits}", large.ToString( "f",  null ) )

    [<Fact>]
    member public this.FixedPoint_Precision () =
        Assert.Equal( "1234567890."            , small.ToString(  "F0",  null ) )
        Assert.Equal( "1234567890.0000"        , small.ToString(  "F4",  null ) )
        Assert.Equal( "1234567890.00000000"    , small.ToString(  "F8",  null ) )
        Assert.Equal( "1234567890.000000000000", small.ToString( "F12",  null ) )

    [<Fact>]
    member public this.FixedPoint_FormatProvider () =
        let testVal = Natural( 1234u ) :> System.IFormattable
        Assert.Equal( "1234.00",  testVal.ToString( "F",  usCulture ) )
        Assert.Equal( "1234.000", testVal.ToString( "F",  ukCulture ) )
        Assert.Equal( "1234,000", testVal.ToString( "F",  frCulture ) )
        Assert.Equal( "1234,000", testVal.ToString( "F",  luCulture ) )

    [<Fact>]
    member public this.Number () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        Assert.Equal( $"1,234,567,890{defaultDigits}", small.ToString( "N",  null ) )
        Assert.Equal( $"1,234,567,890{defaultDigits}", small.ToString( "n",  null ) )

        Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "N",  null ) )
        Assert.Equal( $"12,345,678,901,234,567,890{defaultDigits}", medium.ToString( "n",  null ) )

        Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "N",  null ) )
        Assert.Equal( $"123,456,789,012,345,678,901,234,567,890{defaultDigits}", large.ToString( "n",  null ) )

    [<Fact>]
    member public this.Number_Precision () =
        Assert.Equal( "1,234,567,890"             , small.ToString(  "N0",  null ) )
        Assert.Equal( "1,234,567,890.0000"        , small.ToString(  "N4",  null ) )
        Assert.Equal( "1,234,567,890.00000000"    , small.ToString(  "N8",  null ) )
        Assert.Equal( "1,234,567,890.000000000000", small.ToString( "N12",  null ) )

    [<Fact>]
    member public this.Number_FormatProvider () =
        let testVal = Natural( 1234u ) :> System.IFormattable
        Assert.Equal( "1,234.00",  testVal.ToString( "N",  usCulture ) )
        Assert.Equal( "1,234.000", testVal.ToString( "N",  ukCulture ) )
        Assert.Equal( "1 234,000", testVal.ToString( "N",  frCulture ) )
        Assert.Equal( "1.234,000", testVal.ToString( "N",  luCulture ) )

    [<Fact>]
    member public this.Number_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.NumberGroupSizes <- [| 5; 4; 3 |]
        Assert.Equal( "12,345,678,901,2345,67890.00",  medium.ToString( "N",  custom ) )

    [<Fact>]
    member public this.Number_GroupSizes_Empty () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.NumberGroupSizes <- [||]
        Assert.Equal( "12345678901234567890.00",  medium.ToString( "N",  custom ) )

    [<Fact>]
    member public this.Percent () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}"

        Assert.Equal( $"123,456,789,000{defaultDigits}%%", small.ToString( "P",  null ) )
        Assert.Equal( $"123,456,789,000{defaultDigits}%%", small.ToString( "p",  null ) )

        Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%%", medium.ToString( "P",  null ) )
        Assert.Equal( $"1,234,567,890,123,456,789,000{defaultDigits}%%", medium.ToString( "p",  null ) )

        Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", large.ToString( "P",  null ) )
        Assert.Equal( $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", large.ToString( "p",  null ) )

    [<Fact>]
    member public this.Percent_Precision () =
        Assert.Equal( "123,456,789,000%"             , small.ToString(  "P0",  null ) )
        Assert.Equal( "123,456,789,000.0000%"        , small.ToString(  "P4",  null ) )
        Assert.Equal( "123,456,789,000.00000000%"    , small.ToString(  "P8",  null ) )
        Assert.Equal( "123,456,789,000.000000000000%", small.ToString( "P12",  null ) )

    [<Fact>]
    member public this.Percent_FormatProvider () =
        let testVal = Natural( 1234u ) :> System.IFormattable
        Assert.Equal( "123,400.00%",   testVal.ToString( "P",  usCulture ) )
        Assert.Equal( "123,400.000%",  testVal.ToString( "P",  ukCulture ) )
        Assert.Equal( "123 400,000 %", testVal.ToString( "P",  frCulture ) )
        Assert.Equal( "123.400,000 %", testVal.ToString( "P",  luCulture ) )

    [<Fact>]
    member public this.Percent_Patterns () =
        let testVal = Natural( 1234u ) :> System.IFormattable
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.PercentPositivePattern <- 0
        Assert.Equal( "123,400.00 %",  testVal.ToString( "P",  custom ) )

        custom.PercentPositivePattern <- 1
        Assert.Equal( "123,400.00%",  testVal.ToString( "P",  custom ) )

        custom.PercentPositivePattern <- 2
        Assert.Equal( "%123,400.00",  testVal.ToString( "P",  custom ) )

        custom.PercentPositivePattern <- 3
        Assert.Equal( "% 123,400.00",  testVal.ToString( "P",  custom ) )

    [<Fact>]
    member public this.Percent_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.PercentGroupSizes <- [| 5; 4; 3 |]
        Assert.Equal( "1,234,567,890,123,4567,89000.00%",  medium.ToString( "P",  custom ) )

    [<Fact>]
    member public this.Exponential () =
        Assert.Equal( "1.234568E+009", small.ToString( "E",  null ) )
        Assert.Equal( "1.234568e+009", small.ToString( "e",  null ) )

        Assert.Equal( "1.234568E+019", medium.ToString( "E",  null ) )
        Assert.Equal( "1.234568e+019", medium.ToString( "e",  null ) )

        Assert.Equal( "1.234568E+029", large.ToString( "E",  null ) )
        Assert.Equal( "1.234568e+029", large.ToString( "e",  null ) )

    [<Fact>]
    member public this.Exponential_Precision () =
        Assert.Equal( "1.E+009"            , small.ToString(  "E0",  null ) )
        // Case E2 added to test no rounding
        Assert.Equal( "1.23E+009"          , small.ToString(  "E2",  null ) )
        Assert.Equal( "1.2346E+009"        , small.ToString(  "E4",  null ) )
        Assert.Equal( "1.23456789E+009"    , small.ToString(  "E8",  null ) )
        // Case E9 added to test Exponent = Precision
        Assert.Equal( "1.234567890E+009"   , small.ToString(  "E9",  null ) )
        Assert.Equal( "1.234567890000E+009", small.ToString( "E12",  null ) )

    [<Fact>]
    member public this.Exponential_FormatProvider () =
        Assert.Equal( "1.234568E+009", small.ToString( "E",  usCulture ) )
        Assert.Equal( "1.234568E+009", small.ToString( "E",  ukCulture ) )
        Assert.Equal( "1,234568E+009", small.ToString( "E",  frCulture ) )
        Assert.Equal( "1,234568E+009", small.ToString( "E",  luCulture ) )

    [<Fact>]
    member public this.UnknownFormatSpecifier () =
        let exc = Record.Exception(
            fun () ->
                one.ToString( "Z",  null )
                |> ignore
        )

        Assert.NotNull( exc )
        Assert.IsType<System.FormatException>( exc )