namespace Integer

open Xunit
open Freestylecoding.Math

type public ObjectGetType() =
    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            typeof<Integer>,
            Integer.Unit.GetType()
        )

type public ObjectEquals() =
    [<Theory>]
    [<InlineData(  0,  0, true)>]
    [<InlineData(  1,  0, false)>]
    [<InlineData(  0,  1, false)>]
    [<InlineData(  1,  1, true)>]
    [<InlineData( -1,  0, false)>]
    [<InlineData(  0, -1, false)>]
    [<InlineData(  1, -1, false)>]
    [<InlineData( -1,  1, false)>]
    [<InlineData( -1, -1, true)>]
    member public this.Sanity (left:int) (right:int) (expected:bool) =
        Assert.Equal(
            expected,
            Integer( left ).Equals( Integer( right ) )
        )

    [<Fact>]
    member public this.LargeIntegers () =
        Assert.True(
            Integer( [0xFu; 0x00000101u], false ).Equals( Integer( [0xFu; 0x00000101u], false ) )
        )

        Assert.True(
            Integer( [0xFu; 0x00000101u], true ).Equals( Integer( [0xFu; 0x00000101u], true ) )
        )

        Assert.False(
            Integer( [0xFu; 0x00000101u], true ).Equals( Integer( [0xFu; 0x00000101u], false ) )
        )

    [<Fact>]
    member public this.HardcodedTypes () =
        Assert.True( Integer.Unit.Equals( sbyte 1 ) )
        Assert.True( Integer.Unit.Equals( int16 1 ) )
        Assert.True( Integer.Unit.Equals( int32 1 ) )
        Assert.True( Integer.Unit.Equals( int64 1 ) )

        Assert.True( Integer.Unit.Equals( System.Int128.One ) )
        Assert.True( Integer.Unit.Equals( System.Numerics.BigInteger.One ) )
        Assert.True( Integer.Unit.Equals( Natural.Unit ) )

    [<Fact>]
    member public this.NegativeHardcodedTypes () =
        Assert.True( Integer.NegativeOne.Equals( sbyte -1 ) )
        Assert.True( Integer.NegativeOne.Equals( int16 -1 ) )
        Assert.True( Integer.NegativeOne.Equals( int32 -1 ) )
        Assert.True( Integer.NegativeOne.Equals( int64 -1 ) )

        Assert.True( Integer.NegativeOne.Equals( System.Int128.NegativeOne ) )
        Assert.True( Integer.NegativeOne.Equals( System.Numerics.BigInteger.MinusOne ) )

    [<Fact>]
    member public this.UnsignedTypes () =
        Assert.True( Integer.Unit.Equals( byte   1 ) )
        Assert.True( Integer.Unit.Equals( uint16 1 ) )
        Assert.True( Integer.Unit.Equals( uint32 1 ) )
        Assert.True( Integer.Unit.Equals( uint64 1 ) )

        Assert.True( Integer.Unit.Equals( System.UInt128( 0uL, 1uL ) ) )

    [<Fact>]
    member public this.FloatingTypes () =
        Assert.True( Integer.Unit.Equals( float32 1 ) )
        Assert.True( Integer.Unit.Equals( float   1 ) )
        Assert.True( Integer.Unit.Equals( decimal 1 ) )

        Assert.True( Integer.Unit.Equals( System.Numerics.Complex( 1, 0 ) ) )

    [<Fact>]
    member public this.NegativeFloatingTypes () =
        Assert.True( Integer.NegativeOne.Equals( float32 -1 ) )
        Assert.True( Integer.NegativeOne.Equals( float   -1 ) )
        Assert.True( Integer.NegativeOne.Equals( decimal -1 ) )

        Assert.True( Integer.NegativeOne.Equals( System.Numerics.Complex( -1, 0 ) ) )

    [<Fact>]
    member public this.NotNumber () =
        Assert.False( Integer.Unit.Equals( "1" ) )
        Assert.False( Integer.Unit.Equals( true ) )

    [<Fact>]
    member public this.FloatingTypesWithDecimals () =
        Assert.False( Integer.Unit.Equals( float32 1.1 ) )
        Assert.False( Integer.Unit.Equals( float   1.1 ) )
        Assert.False( Integer.Unit.Equals( decimal 1.1 ) )

        Assert.False( Integer.Unit.Equals( System.Numerics.Complex( 1.1, 0 ) ) )

    [<Fact>]
    member public this.Imaginary () =
        // I'm making an assumption that an imaginary number is not an integer
        // This explicitly checks that assumption
        Assert.False( Integer.Unit.Equals( System.Numerics.Complex( 1, 1 ) ) )

type public ObjectGetHashCode() =
    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            Integer.Unit.GetHashCode(),
            Integer.Unit.GetHashCode()
        )

        Assert.NotEqual(
            Integer.Zero.GetHashCode(),
            Integer.Unit.GetHashCode()
        )

        Assert.NotEqual(
            Integer.NegativeOne.GetHashCode(),
            Integer.Unit.GetHashCode()
        )

type public ObjectToString() =
    [<Theory>]
    [<InlineData(      0,      "0" )>] // Sanity
    [<InlineData(      1,      "1" )>] // Sanity
    [<InlineData(    123,    "123" )>] // multiple bits
    [<InlineData(  45678,  "45678" )>] // rev
    [<InlineData(     -1,     "-1" )>] // Sanity
    [<InlineData(   -123,   "-123" )>] // multiple bits
    [<InlineData( -45678, "-45678" )>] // rev
    member public this.Sanity (actual:int) (expected:string) =
        Assert.Equal(
            expected,
            Integer( actual ).ToString()
        )

    [<Fact>]
    member public this.Bigger () =
        Assert.Equal(
            "1234567890123456789",
            Integer( [0x112210F4u; 0x7DE98115u] ).ToString()
        )

    [<Fact>]
    member public this.BiggerNegative () =
        Assert.Equal(
            "-1234567890123456789",
            Integer( [0x112210F4u; 0x7DE98115u], true ).ToString()
        )

type public ToStringFormat() =
    let small  = Integer( [0x4996_02D2u] )
    let smallStr = "1234567890"

    let medium = Integer( [0xAB54_A98Cu; 0xEB1F_0AD2u] )
    let mediumStr = "12345678901234567890"

    let large  = Integer( [0x0000_0001u; 0x8EE9_0FF6u; 0xC373_E0EEu; 0x4E3F_0AD2u] )
    let largeStr = "123456789012345678901234567890"

    let smallNeg  = Integer( [0x4996_02D2u], true )
    let smallNegStr = "-1234567890"

    let mediumNeg = Integer( [0xAB54_A98Cu; 0xEB1F_0AD2u], true )
    let mediumNegStr = "-12345678901234567890"

    let largeNeg  = Integer( [0x0000_0001u; 0x8EE9_0FF6u; 0xC373_E0EEu; 0x4E3F_0AD2u], true )
    let largeNegStr = "-123456789012345678901234567890"

    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            "1",
            Integer.Unit.ToString( null )
        )

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
    [<InlineData( "X" )>]
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
        Assert.Equal(
            0x4996_02D2.ToString( format ),
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
    member public this.NegativeGeneralAndRoundTrip () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        Assert.Equal( smallNegStr, smallNeg.ToString( "G" ) )
        Assert.Equal( smallNegStr, smallNeg.ToString( "g" ) )
        Assert.Equal( smallNegStr, smallNeg.ToString( "R" ) )
        Assert.Equal( smallNegStr, smallNeg.ToString( "r" ) )

        Assert.Equal( mediumNegStr, mediumNeg.ToString( "G" ) )
        Assert.Equal( mediumNegStr, mediumNeg.ToString( "g" ) )
        Assert.Equal( mediumNegStr, mediumNeg.ToString( "R" ) )
        Assert.Equal( mediumNegStr, mediumNeg.ToString( "r" ) )

        Assert.Equal( largeNegStr, largeNeg.ToString( "G" ) )
        Assert.Equal( largeNegStr, largeNeg.ToString( "g" ) )
        Assert.Equal( largeNegStr, largeNeg.ToString( "R" ) )
        Assert.Equal( largeNegStr, largeNeg.ToString( "r" ) )

    [<Fact>]
    member public this.NegativeGeneralAndRoundTrip_IgnorePrecision () =
        // These are all the same as Object.ToString
        // Thus, I'm lumping the tests together
        for i in 0 .. 20 do
            Assert.Equal( smallNegStr, smallNeg.ToString( $"G{i}" ) )
            Assert.Equal( smallNegStr, smallNeg.ToString( $"g{i}" ) )
            Assert.Equal( smallNegStr, smallNeg.ToString( $"R{i}" ) )
            Assert.Equal( smallNegStr, smallNeg.ToString( $"r{i}" ) )

            Assert.Equal( mediumNegStr, mediumNeg.ToString( $"G{i}" ) )
            Assert.Equal( mediumNegStr, mediumNeg.ToString( $"g{i}" ) )
            Assert.Equal( mediumNegStr, mediumNeg.ToString( $"R{i}" ) )
            Assert.Equal( mediumNegStr, mediumNeg.ToString( $"r{i}" ) )

            Assert.Equal( largeNegStr, largeNeg.ToString( $"G{i}" ) )
            Assert.Equal( largeNegStr, largeNeg.ToString( $"g{i}" ) )
            Assert.Equal( largeNegStr, largeNeg.ToString( $"R{i}" ) )
            Assert.Equal( largeNegStr, largeNeg.ToString( $"r{i}" ) )

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
    member public this.NegativeDecimal () =
        Assert.Equal( smallNegStr, smallNeg.ToString( "D" ) )
        Assert.Equal( smallNegStr, smallNeg.ToString( "d" ) )

        Assert.Equal( mediumNegStr, mediumNeg.ToString( "D" ) )
        Assert.Equal( mediumNegStr, mediumNeg.ToString( "d" ) )

        Assert.Equal( largeNegStr, largeNeg.ToString( "D" ) )
        Assert.Equal( largeNegStr, largeNeg.ToString( "d" ) )

    [<Fact>]
    member public this.NegativeDecimal_Precision () =
        Assert.Equal(       smallNegStr, smallNeg.ToString(  "D0" ) )
        Assert.Equal(       smallNegStr, smallNeg.ToString(  "D4" ) )
        Assert.Equal(       smallNegStr, smallNeg.ToString(  "D8" ) )
        Assert.Equal( "001234567890", smallNeg.ToString( "D12" ) )

    [<Fact>]
    member public this.Hexadecimal () =
        let smallBI:System.Numerics.BigInteger = small
        Assert.Equal( smallBI.ToString( "X" ), small.ToString( "X" ) )
        Assert.Equal( smallBI.ToString( "x" ), small.ToString( "x" ) )

        let mediumBI:System.Numerics.BigInteger = medium
        Assert.Equal( mediumBI.ToString( "X" ), medium.ToString( "X" ) )
        Assert.Equal( mediumBI.ToString( "x" ), medium.ToString( "x" ) )

        let largeBI:System.Numerics.BigInteger = large
        Assert.Equal( largeBI.ToString( "X" ), large.ToString( "X" ) )
        Assert.Equal( largeBI.ToString( "x" ), large.ToString( "x" ) )

    [<Fact>]
    member public this.Hexadecimal_Precision () =
        let smallBI:System.Numerics.BigInteger = small
        Assert.Equal( smallBI.ToString(  "X0" ), small.ToString(  "X0" ) )
        Assert.Equal( smallBI.ToString(  "X4" ), small.ToString(  "X4" ) )
        Assert.Equal( smallBI.ToString(  "X8" ), small.ToString(  "X8" ) )
        Assert.Equal( smallBI.ToString( "X12" ), small.ToString( "X12" ) )
        Assert.Equal( smallBI.ToString( "X16" ), small.ToString( "X16" ) )

    [<Fact>]
    member public this.NegativeHexadecimal () =
        let smallBI:System.Numerics.BigInteger = smallNeg
        Assert.Equal( smallBI.ToString( "X" ), smallNeg.ToString( "X" ) )
        Assert.Equal( smallBI.ToString( "x" ), smallNeg.ToString( "x" ) )

        let mediumBI:System.Numerics.BigInteger = mediumNeg
        Assert.Equal( mediumBI.ToString( "X" ), mediumNeg.ToString( "X" ) )
        Assert.Equal( mediumBI.ToString( "x" ), mediumNeg.ToString( "x" ) )

        let largeBI:System.Numerics.BigInteger = largeNeg
        Assert.Equal( largeBI.ToString( "X" ), largeNeg.ToString( "X" ) )
        Assert.Equal( largeBI.ToString( "x" ), largeNeg.ToString( "x" ) )

    [<Fact>]
    member public this.NegativeHexadecimal_Precision () =
        let smallBI:System.Numerics.BigInteger = smallNeg
        Assert.Equal( smallBI.ToString(  "X0" ), smallNeg.ToString(  "X0" ) )
        Assert.Equal( smallBI.ToString(  "X4" ), smallNeg.ToString(  "X4" ) )
        Assert.Equal( smallBI.ToString(  "X8" ), smallNeg.ToString(  "X8" ) )
        Assert.Equal( smallBI.ToString( "X12" ), smallNeg.ToString( "X12" ) )
        Assert.Equal( smallBI.ToString( "X16" ), smallNeg.ToString( "X16" ) )

    [<Fact>]
    member public this.Binary () =
        let smallBI:System.Numerics.BigInteger = small
        Assert.Equal( smallBI.ToString( "B" ), small.ToString( "B" ) )
        Assert.Equal( smallBI.ToString( "b" ), small.ToString( "b" ) )

        let mediumBI:System.Numerics.BigInteger = medium
        Assert.Equal( mediumBI.ToString( "B" ), medium.ToString( "B" ) )
        Assert.Equal( mediumBI.ToString( "b" ), medium.ToString( "b" ) )

        let largeBI:System.Numerics.BigInteger = large
        Assert.Equal( largeBI.ToString( "B" ), large.ToString( "B" ) )
        Assert.Equal( largeBI.ToString( "b" ), large.ToString( "b" ) )

    [<Fact>]
    member public this.Binary_Precision () =
        let smallBI:System.Numerics.BigInteger = small
        Assert.Equal( smallBI.ToString(  "B0" ), small.ToString(  "B0" ) )
        Assert.Equal( smallBI.ToString(  "B8" ), small.ToString(  "B8" ) )
        Assert.Equal( smallBI.ToString( "B16" ), small.ToString( "B16" ) )
        Assert.Equal( smallBI.ToString( "B32" ), small.ToString( "B32" ) )
        Assert.Equal( smallBI.ToString( "B64" ), small.ToString( "B64" ) )

    [<Fact>]
    member public this.NegativeBinary () =
        let smallBI:System.Numerics.BigInteger = smallNeg
        Assert.Equal( smallBI.ToString( "B" ), smallNeg.ToString( "B" ) )
        Assert.Equal( smallBI.ToString( "b" ), smallNeg.ToString( "b" ) )

        let mediumBI:System.Numerics.BigInteger = mediumNeg
        Assert.Equal( mediumBI.ToString( "B" ), mediumNeg.ToString( "B" ) )
        Assert.Equal( mediumBI.ToString( "b" ), mediumNeg.ToString( "b" ) )

        let largeBI:System.Numerics.BigInteger = largeNeg
        Assert.Equal( largeBI.ToString( "B" ), largeNeg.ToString( "B" ) )
        Assert.Equal( largeBI.ToString( "b" ), largeNeg.ToString( "b" ) )

    [<Fact>]
    member public this.NegativeBinary_Precision () =
        let smallBI:System.Numerics.BigInteger = smallNeg
        Assert.Equal( smallBI.ToString(  "B0" ), smallNeg.ToString(  "B0" ) )
        Assert.Equal( smallBI.ToString(  "B8" ), smallNeg.ToString(  "B8" ) )
        Assert.Equal( smallBI.ToString( "B16" ), smallNeg.ToString( "B16" ) )
        Assert.Equal( smallBI.ToString( "B32" ), smallNeg.ToString( "B32" ) )
        Assert.Equal( smallBI.ToString( "B64" ), smallNeg.ToString( "B64" ) )

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
    member public this.NegativeCurrency () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        Assert.Equal( $"$-1,234,567,890{defaultDigits}", smallNeg.ToString( "C" ) )
        Assert.Equal( $"$-1,234,567,890{defaultDigits}", smallNeg.ToString( "c" ) )

        Assert.Equal( $"$-12,345,678,901,234,567,890{defaultDigits}", mediumNeg.ToString( "C" ) )
        Assert.Equal( $"$-12,345,678,901,234,567,890{defaultDigits}", mediumNeg.ToString( "c" ) )

        Assert.Equal( $"$-123,456,789,012,345,678,901,234,567,890{defaultDigits}", largeNeg.ToString( "C" ) )
        Assert.Equal( $"$-123,456,789,012,345,678,901,234,567,890{defaultDigits}", largeNeg.ToString( "c" ) )

    [<Fact>]
    member public this.NegativeCurrency_Precision () =
        Assert.Equal( "$-1,234,567,890"             , smallNeg.ToString(  "C0" ) )
        Assert.Equal( "$-1,234,567,890.0000"        , smallNeg.ToString(  "C4" ) )
        Assert.Equal( "$-1,234,567,890.00000000"    , smallNeg.ToString(  "C8" ) )
        Assert.Equal( "$-1,234,567,890.000000000000", smallNeg.ToString( "C12" ) )

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
    member public this.NegativeFixedPoint () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits)}"

        Assert.Equal( $"-1234567890{defaultDigits}", smallNeg.ToString( "F" ) )
        Assert.Equal( $"-1234567890{defaultDigits}", smallNeg.ToString( "f" ) )

        Assert.Equal( $"-12345678901234567890{defaultDigits}", mediumNeg.ToString( "F" ) )
        Assert.Equal( $"-12345678901234567890{defaultDigits}", mediumNeg.ToString( "f" ) )

        Assert.Equal( $"-123456789012345678901234567890{defaultDigits}", largeNeg.ToString( "F" ) )
        Assert.Equal( $"-123456789012345678901234567890{defaultDigits}", largeNeg.ToString( "f" ) )

    [<Fact>]
    member public this.NegativeFixedPoint_Precision () =
        Assert.Equal( "-1234567890."            , smallNeg.ToString(  "F0" ) )
        Assert.Equal( "-1234567890.0000"        , smallNeg.ToString(  "F4" ) )
        Assert.Equal( "-1234567890.00000000"    , smallNeg.ToString(  "F8" ) )
        Assert.Equal( "-1234567890.000000000000", smallNeg.ToString( "F12" ) )

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
    member public this.NegativeNumber () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits)}"

        Assert.Equal( $"-1,234,567,890{defaultDigits}", smallNeg.ToString( "N" ) )
        Assert.Equal( $"-1,234,567,890{defaultDigits}", smallNeg.ToString( "n" ) )

        Assert.Equal( $"-12,345,678,901,234,567,890{defaultDigits}", mediumNeg.ToString( "N" ) )
        Assert.Equal( $"-12,345,678,901,234,567,890{defaultDigits}", mediumNeg.ToString( "n" ) )

        Assert.Equal( $"-123,456,789,012,345,678,901,234,567,890{defaultDigits}", largeNeg.ToString( "N" ) )
        Assert.Equal( $"-123,456,789,012,345,678,901,234,567,890{defaultDigits}", largeNeg.ToString( "n" ) )

    [<Fact>]
    member public this.NegativeNumber_Precision () =
        Assert.Equal( "-1,234,567,890"             , smallNeg.ToString(  "N0" ) )
        Assert.Equal( "-1,234,567,890.0000"        , smallNeg.ToString(  "N4" ) )
        Assert.Equal( "-1,234,567,890.00000000"    , smallNeg.ToString(  "N8" ) )
        Assert.Equal( "-1,234,567,890.000000000000", smallNeg.ToString( "N12" ) )

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
    member public this.NegalivePercent () =
        let defaultDigits = $".{System.String( '0', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits)}"

        Assert.Equal( $"-123,456,789,000{defaultDigits}%%", smallNeg.ToString( "P" ) )
        Assert.Equal( $"-123,456,789,000{defaultDigits}%%", smallNeg.ToString( "p" ) )

        Assert.Equal( $"-1,234,567,890,123,456,789,000{defaultDigits}%%", mediumNeg.ToString( "P" ) )
        Assert.Equal( $"-1,234,567,890,123,456,789,000{defaultDigits}%%", mediumNeg.ToString( "p" ) )

        Assert.Equal( $"-12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", largeNeg.ToString( "P" ) )
        Assert.Equal( $"-12,345,678,901,234,567,890,123,456,789,000{defaultDigits}%%", largeNeg.ToString( "p" ) )

    [<Fact>]
    member public this.NegativePercent_Precision () =
        Assert.Equal( "-123,456,789,000%"             , smallNeg.ToString(  "P0" ) )
        Assert.Equal( "-123,456,789,000.0000%"        , smallNeg.ToString(  "P4" ) )
        Assert.Equal( "-123,456,789,000.00000000%"    , smallNeg.ToString(  "P8" ) )
        Assert.Equal( "-123,456,789,000.000000000000%", smallNeg.ToString( "P12" ) )

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
    member public this.NegativeExponential () =
        Assert.Equal( "-1.234568E+009", smallNeg.ToString( "E" ) )
        Assert.Equal( "-1.234568e+009", smallNeg.ToString( "e" ) )

        Assert.Equal( "-1.234568E+019", mediumNeg.ToString( "E" ) )
        Assert.Equal( "-1.234568e+019", mediumNeg.ToString( "e" ) )

        Assert.Equal( "-1.234568E+029", largeNeg.ToString( "E" ) )
        Assert.Equal( "-1.234568e+029", largeNeg.ToString( "e" ) )

    [<Fact>]
    member public this.NegativeExponential_Precision () =
        Assert.Equal( "-1.E+009"            , smallNeg.ToString(  "E0" ) )
        // Case E2 added to test no rounding
        Assert.Equal( "-1.23E+009"          , smallNeg.ToString(  "E2" ) )
        Assert.Equal( "-1.2346E+009"        , smallNeg.ToString(  "E4" ) )
        Assert.Equal( "-1.23456789E+009"    , smallNeg.ToString(  "E8" ) )
        // Case E9 added to test Exponent = Precision
        Assert.Equal( "-1.234567890E+009"   , smallNeg.ToString(  "E9" ) )
        Assert.Equal( "-1.234567890000E+009", smallNeg.ToString( "E12" ) )

    [<Fact>]
    member public this.UnknownFormatSpecifier () =
        let exc = Record.Exception(
            fun () ->
                Integer.Unit.ToString( "Z" )
                |> ignore
        )

        Assert.NotNull( exc )
        Assert.IsType<System.FormatException>( exc )