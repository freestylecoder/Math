namespace Natural

open Xunit
open Freestylecoding.Math

type public Utf8SpanFormattable() =
    let smallUI = 0x4996_02D2u
    let small   = Natural( smallUI )
    let smallStr = "1234567890"

    let medium = Natural( [0xAB54_A98Cu; 0xEB1F_0AD2u] )
    let mediumStr = "12345678901234567890"

    let large  = Natural( [0x0000_0001u; 0x8EE9_0FF6u; 0xC373_E0EEu; 0x4E3F_0AD2u] )
    let largeStr = "123456789012345678901234567890"

    let usCulture = System.Globalization.CultureInfo( "en-US" ) // US
    let ukCulture = System.Globalization.CultureInfo( "en-GB" ) // UK
    let frCulture = System.Globalization.CultureInfo( "fr-FR" ) // France
    let luCulture = System.Globalization.CultureInfo( "fr-LU" ) // Luxembourg

    let getUtf8Span expectedLength : System.Span<byte> =
        let destBuffer = [| for _ in 1 .. expectedLength -> 32uy |]
        System.Span<byte>( destBuffer )

    let assertEqual (expected:string) (value:'T when 'T :> System.IUtf8SpanFormattable) (format:string) (formatProvider:System.IFormatProvider) =
        let destination = getUtf8Span expected.Length
        let mutable charsWritten = 0
        Assert.True( Utf8SpanFormattable.TryFormat( value, destination, &charsWritten, format, formatProvider ) )
        Assert.Equal( expected.Length, charsWritten )
        // The TrimEnd is because I filled the original buffer with spaces
        Assert.Equal( expected, System.Text.Encoding.UTF8.GetString( destination ).TrimEnd() )

    let assertEqualAgainstUnsignedInt (value:uint32) (format:string) (culture:System.Globalization.CultureInfo) =
        let expectedSpan:System.Span<byte> = getUtf8Span 50
        let mutable expectedBytesWritten = 0
        let expectedResult = value.TryFormat( expectedSpan, &expectedBytesWritten, format, culture)

        let actualSpan:System.Span<byte> = getUtf8Span 50
        let mutable actualBytesWritten = 0
        let actualResult =
            (Natural( value ) :> System.IUtf8SpanFormattable)
                .TryFormat( actualSpan, &actualBytesWritten, format, culture)

        Assert.Equal( expectedResult, actualResult )
        Assert.Equal( expectedBytesWritten, actualBytesWritten )
        Assert.Equal<byte>( expectedSpan, actualSpan )

    let cultures = [| usCulture; ukCulture; frCulture; luCulture|]

    static member TryFormat( this:'T when 'T :> System.IUtf8SpanFormattable, destination:System.Span<byte>, charsWritten:byref<int>, format:System.ReadOnlySpan<char>, formatProvider:System.IFormatProvider ) : bool =
        this.TryFormat( destination, &charsWritten, format, formatProvider )

    [<Fact>]
    member public this.Sanity () =
        // Yes, it does the test twice.
        // It also double checks the shortcut test method
        assertEqual "1" Natural.Unit "" null

        let destBuffer = [| 32uy; 32uy; 32uy; 32uy; 32uy |]
        let destination = System.Span<byte>( destBuffer )
        let mutable charsWritten = 0
        Assert.True( Utf8SpanFormattable.TryFormat( Natural.Unit, destination, &charsWritten, "", null ) )
        Assert.Equal( 1, charsWritten )
        // The TrimEnd is because I filled the original buffer with spaces
        Assert.Equal( "1", System.Text.Encoding.UTF8.GetString( destination ).TrimEnd() )

    [<Fact>]
    member public this.FillToAvailable () =
        let destBuffer = [| 32uy; 32uy; 32uy; 32uy; 32uy |]
        let destination = System.Span<byte>( destBuffer )
        let mutable charsWritten = 0
        Assert.False( Utf8SpanFormattable.TryFormat( small, destination, &charsWritten, "", null ) )
        Assert.Equal( 5, charsWritten )
        // The TrimEnd is because I filled the original buffer with spaces
        Assert.Equal( smallStr.Substring( 0, 5 ), System.Text.Encoding.UTF8.GetString( destination ).TrimEnd() )

    [<Theory>]
    [<InlineData( "B" )>]
    [<InlineData( "C" )>]
    [<InlineData( "D" )>]
    [<InlineData( "E" )>]
    [<InlineData( "F" )>]
    [<InlineData( "G" )>]
    [<InlineData( "N" )>]
    [<InlineData( "P" )>]
    [<InlineData( "R" )>]
    [<InlineData( "D" )>]
    [<InlineData( "b" )>]
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
        let actualBuffer = [| for _ in 1 .. 32 -> 32uy |]
        let actual = System.Span<byte>( actualBuffer )
        let mutable actualChars = 0

        let expectedBuffer = [| for _ in 1 .. 32 -> 32uy |]
        let expected = System.Span<byte>( expectedBuffer )
        let mutable expectedChars = 0

        Assert.True( Utf8SpanFormattable.TryFormat( 0x4996_02D2u, expected, &expectedChars, format, null ) )
        Assert.True( Utf8SpanFormattable.TryFormat( small, actual, &actualChars, format, null ) )

        Assert.Equal( expected.ToString(), actual.ToString() )

    [<Fact>]
    member public this.GeneralAndRoundTrip () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        assertEqual smallStr small "G" null
        assertEqual smallStr small "g" null
        assertEqual smallStr small "R" null
        assertEqual smallStr small "r" null

        assertEqual mediumStr medium "G" null
        assertEqual mediumStr medium "g" null
        assertEqual mediumStr medium "R" null
        assertEqual mediumStr medium "r" null

        assertEqual largeStr large "G" null
        assertEqual largeStr large "g" null
        assertEqual largeStr large "R" null
        assertEqual largeStr large "r" null

    [<Fact>]
    member public this.GeneralAndRoundTrip_IgnorePrecision () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        for i in 0 .. 20 do
            assertEqual smallStr small $"G{i}" null
            assertEqual smallStr small $"g{i}" null
            assertEqual smallStr small $"R{i}" null
            assertEqual smallStr small $"r{i}" null

            assertEqual mediumStr medium $"G{i}" null
            assertEqual mediumStr medium $"g{i}" null
            assertEqual mediumStr medium $"R{i}" null
            assertEqual mediumStr medium $"r{i}" null

            assertEqual largeStr large $"G{i}" null
            assertEqual largeStr large $"g{i}" null
            assertEqual largeStr large $"R{i}" null
            assertEqual largeStr large $"r{i}" null

    [<Fact>]
    member public this.GeneralAndRoundTrip_IgnoreFormatProvider () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        for culture in cultures do
            assertEqual smallStr small "G" culture
            assertEqual smallStr small "g" culture
            assertEqual smallStr small "R" culture
            assertEqual smallStr small "r" culture

            assertEqual mediumStr medium "G" culture
            assertEqual mediumStr medium "g" culture
            assertEqual mediumStr medium "R" culture
            assertEqual mediumStr medium "r" culture

            assertEqual largeStr large "G" culture
            assertEqual largeStr large "g" culture
            assertEqual largeStr large "R" culture
            assertEqual largeStr large "r" culture

    [<Fact>]
    member public this.Decimal () =
        assertEqual smallStr small "D" null
        assertEqual smallStr small "d" null

        assertEqual mediumStr medium "D" null
        assertEqual mediumStr medium "d" null

        assertEqual largeStr large "D" null
        assertEqual largeStr large "d" null

    [<Fact>]
    member public this.Decimal_Precision () =
        assertEqual       smallStr small  "D0" null
        assertEqual       smallStr small  "D4" null
        assertEqual       smallStr small  "D8" null
        assertEqual "001234567890" small "D12" null

    [<Fact>]
    member public this.Decimal_IgnoreFormatProvider () =
        for culture in cultures do
            assertEqual smallStr small "D" culture
            assertEqual smallStr small "d" culture

            assertEqual mediumStr medium "D" culture
            assertEqual mediumStr medium "d" culture

            assertEqual largeStr large "D" culture
            assertEqual largeStr large "d" culture

    [<Fact>]
    member public this.Hexadecimal () =
        assertEqual "499602D2" small "X" null
        assertEqual "499602d2" small "x" null

        assertEqual "AB54A98CEB1F0AD2" medium "X" null
        assertEqual "ab54a98ceb1f0ad2" medium "x" null

        assertEqual "18EE90FF6C373E0EE4E3F0AD2" large "X" null
        assertEqual "18ee90ff6c373e0ee4e3f0ad2" large "x" null

    [<Fact>]
    member public this.Hexadecimal_Precision () =
        assertEqual     "499602D2" small  "X0" null
        assertEqual     "499602D2" small  "X4" null
        assertEqual     "499602D2" small  "X8" null
        assertEqual "0000499602D2" small "X12" null

    [<Fact>]
    member public this.Hexadecimal_IgnoreFormatProvider () =
        for culture in cultures do
            assertEqual "499602D2" small "X" culture
            assertEqual "499602d2" small "x" culture

            assertEqual "AB54A98CEB1F0AD2" medium "X" culture
            assertEqual "ab54a98ceb1f0ad2" medium "x" culture

            assertEqual "18EE90FF6C373E0EE4E3F0AD2" large "X" culture
            assertEqual "18ee90ff6c373e0ee4e3f0ad2" large "x" culture

    [<Fact>]
    member public this.Binary () =
        assertEqual "1001001100101100000001011010010" small "B" null
        assertEqual "1001001100101100000001011010010" small "b" null

        assertEqual "1010101101010100101010011000110011101011000111110000101011010010" medium "B" null
        assertEqual "1010101101010100101010011000110011101011000111110000101011010010" medium "b" null

        assertEqual "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010" large "B" null
        assertEqual "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010" large "b" null

    [<Fact>]
    member public this.Binary_Precision () =
        assertEqual  "1001001100101100000001011010010" small  "B0" null
        assertEqual  "1001001100101100000001011010010" small  "B8" null
        assertEqual  "1001001100101100000001011010010" small "B16" null
        assertEqual "01001001100101100000001011010010" small "B32" null

    [<Fact>]
    member public this.Binary_IgnoreFormatProvider () =
        for culture in cultures do
            assertEqual "1001001100101100000001011010010" small "B" culture
            assertEqual "1001001100101100000001011010010" small "b" culture

            assertEqual "1010101101010100101010011000110011101011000111110000101011010010" medium "B" culture
            assertEqual "1010101101010100101010011000110011101011000111110000101011010010" medium "b" culture

            assertEqual "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010" large "B" culture
            assertEqual "1100011101110100100001111111101101100001101110011111000001110111001001110001111110000101011010010" large "b" culture

    [<Fact>]
    member public this.Currency () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        assertEqual $"$1,234,567,890{defaultDigits}" small "C" null
        assertEqual $"$1,234,567,890{defaultDigits}" small "c" null

        assertEqual $"$12,345,678,901,234,567,890{defaultDigits}" medium "C" null
        assertEqual $"$12,345,678,901,234,567,890{defaultDigits}" medium "c" null

        assertEqual $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}" large "C" null
        assertEqual $"$123,456,789,012,345,678,901,234,567,890{defaultDigits}" large "c" null

    [<Fact>]
    member public this.Currency_Precision () =
        assertEqual "$1,234,567,890"              small  "C0" null
        assertEqual "$1,234,567,890.0000"         small  "C4" null
        assertEqual "$1,234,567,890.00000000"     small  "C8" null
        assertEqual "$1,234,567,890.000000000000" small "C12" null

    [<Fact>]
    member public this.Currency_FormatProvider () =
        assertEqualAgainstUnsignedInt 1234u "C" usCulture
        assertEqualAgainstUnsignedInt 1234u "C" ukCulture
        assertEqualAgainstUnsignedInt 1234u "C" frCulture
        assertEqualAgainstUnsignedInt 1234u "C" luCulture

    [<Fact>]
    member public this.Currency_Patterns () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.CurrencyPositivePattern <- 0
        assertEqual "$1,234.00" (Natural( 1234u )) "C" custom

        custom.CurrencyPositivePattern <- 1
        assertEqual "1,234.00$" (Natural( 1234u )) "C" custom

        custom.CurrencyPositivePattern <- 2
        assertEqual "$ 1,234.00" (Natural( 1234u )) "C" custom

        custom.CurrencyPositivePattern <- 3
        assertEqual "1,234.00 $" (Natural( 1234u )) "C" custom

    [<Fact>]
    member public this.Currency_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.CurrencyGroupSizes <- [| 5; 4; 3 |]
        assertEqual "$12,345,678,901,2345,67890.00" medium "C" custom

    [<Fact>]
    member public this.FixedPoint () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}"

        assertEqual $"1234567890{defaultDigits}" small "F" null
        assertEqual $"1234567890{defaultDigits}" small "f" null

        assertEqual $"12345678901234567890{defaultDigits}" medium "F" null
        assertEqual $"12345678901234567890{defaultDigits}" medium "f" null

        assertEqual $"123456789012345678901234567890{defaultDigits}" large "F" null
        assertEqual $"123456789012345678901234567890{defaultDigits}" large "f" null

    [<Fact>]
    member public this.FixedPoint_Precision () =
        assertEqual "1234567890."             small  "F0" null
        assertEqual "1234567890.0000"         small  "F4" null
        assertEqual "1234567890.00000000"     small  "F8" null
        assertEqual "1234567890.000000000000" small "F12" null

    [<Fact>]
    member public this.FixedPoint_FormatProvider () =
        assertEqualAgainstUnsignedInt 1234u "F" usCulture
        assertEqualAgainstUnsignedInt 1234u "F" ukCulture
        assertEqualAgainstUnsignedInt 1234u "F" frCulture
        assertEqualAgainstUnsignedInt 1234u "F" luCulture

    [<Fact>]
    member public this.Number () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        assertEqual $"1,234,567,890{defaultDigits}" small "N" null
        assertEqual $"1,234,567,890{defaultDigits}" small "n" null

        assertEqual $"12,345,678,901,234,567,890{defaultDigits}" medium "N" null
        assertEqual $"12,345,678,901,234,567,890{defaultDigits}" medium "n" null

        assertEqual $"123,456,789,012,345,678,901,234,567,890{defaultDigits}" large "N" null
        assertEqual $"123,456,789,012,345,678,901,234,567,890{defaultDigits}" large "n" null

    [<Fact>]
    member public this.Number_Precision () =
        assertEqual "1,234,567,890"              small  "N0" null
        assertEqual "1,234,567,890.0000"         small  "N4" null
        assertEqual "1,234,567,890.00000000"     small  "N8" null
        assertEqual "1,234,567,890.000000000000" small "N12" null

    [<Fact>]
    member public this.Number_FormatProvider () =
        assertEqualAgainstUnsignedInt 1234u "N" usCulture
        assertEqualAgainstUnsignedInt 1234u "N" ukCulture
        assertEqualAgainstUnsignedInt 1234u "N" frCulture
        assertEqualAgainstUnsignedInt 1234u "N" luCulture

    [<Fact>]
    member public this.Number_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.NumberGroupSizes <- [| 5; 4; 3 |]
        assertEqual "12,345,678,901,2345,67890.00" medium "N" custom

    [<Fact>]
    member public this.Percent () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}"

        assertEqual $"123,456,789,000{defaultDigits}%%" small "P" null
        assertEqual $"123,456,789,000{defaultDigits}%%" small "p" null

        assertEqual $"1,234,567,890,123,456,789,000{defaultDigits}%%" medium "P" null
        assertEqual $"1,234,567,890,123,456,789,000{defaultDigits}%%" medium "p" null

        assertEqual $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%" large "P" null
        assertEqual $"12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%" large "p" null

    [<Fact>]
    member public this.Percent_Precision () =
        assertEqual "123,456,789,000%"              small  "P0" null
        assertEqual "123,456,789,000.0000%"         small  "P4" null
        assertEqual "123,456,789,000.00000000%"     small  "P8" null
        assertEqual "123,456,789,000.000000000000%" small "P12" null

    [<Fact>]
    member public this.Percent_FormatProvider () =
        assertEqualAgainstUnsignedInt 1234u "P" usCulture
        assertEqualAgainstUnsignedInt 1234u "P" ukCulture
        assertEqualAgainstUnsignedInt 1234u "P" frCulture
        assertEqualAgainstUnsignedInt 1234u "P" luCulture

    [<Fact>]
    member public this.Percent_Patterns () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.PercentPositivePattern <- 0
        assertEqual "123,400.00 %" (Natural( 1234u )) "P" custom

        custom.PercentPositivePattern <- 1
        assertEqual "123,400.00%"  (Natural( 1234u )) "P" custom

        custom.PercentPositivePattern <- 2
        assertEqual "%123,400.00"  (Natural( 1234u )) "P" custom

        custom.PercentPositivePattern <- 3
        assertEqual "% 123,400.00" (Natural( 1234u )) "P" custom

    [<Fact>]
    member public this.Percent_GroupSizes () =
        let custom = System.Globalization.CultureInfo( "en-US" ).NumberFormat
        custom.PercentGroupSizes <- [| 5; 4; 3 |]
        assertEqual "1,234,567,890,123,4567,89000.00%" medium "P" custom

    [<Fact>]
    member public this.Exponential () =
        assertEqual "1.234568E+009" small "E" null
        assertEqual "1.234568e+009" small "e" null

        assertEqual "1.234568E+019" medium "E" null
        assertEqual "1.234568e+019" medium "e" null

        assertEqual "1.234568E+029" large "E" null
        assertEqual "1.234568e+029" large "e" null

    [<Fact>]
    member public this.Exponential_Precision () =
        assertEqual "1.E+009"             small  "E0" null
        // Case E2 added to test no rounding
        assertEqual "1.23E+009"           small  "E2" null
        assertEqual "1.2346E+009"         small  "E4" null
        assertEqual "1.23456789E+009"     small  "E8" null
        // Case E9 added to test Exponent = Precision
        assertEqual "1.234567890E+009"    small  "E9" null
        assertEqual "1.234567890000E+009" small "E12" null

    [<Fact>]
    member public this.Exponential_FormatProvider () =
        assertEqualAgainstUnsignedInt smallUI "E" usCulture
        assertEqualAgainstUnsignedInt smallUI "E" ukCulture
        assertEqualAgainstUnsignedInt smallUI "E" frCulture
        assertEqualAgainstUnsignedInt smallUI "E" luCulture

    [<Fact>]
    member public this.UnknownFormatSpecifier () =
        let destBuffer = [| 32uy; 32uy; 32uy; 32uy; 32uy |]
        let destination = System.Span<byte>( destBuffer )
        let mutable charsWritten = 0

        try 
            Utf8SpanFormattable.TryFormat( Natural.Unit, destination, &charsWritten, "Z", null )
            |> ignore

            Assert.Fail( "Expected System.FormatException" )
        with
            | :? System.FormatException -> ()
            | exp -> Assert.Fail( $"Expected System.FormatException, Actual {exp.GetType().Name}" )
