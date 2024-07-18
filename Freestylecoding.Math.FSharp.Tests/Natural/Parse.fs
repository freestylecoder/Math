namespace Natural

open System
open Xunit
open Freestylecoding.Math

type public ParseString() =
    let currentCulture = System.Globalization.CultureInfo.CurrentCulture.NumberFormat

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n s =
        Assert.Equal( Natural([n]), Natural.Parse(s) )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            Natural( [ 0x112210F4u; 0x7DE98115u ] ),
            Natural.Parse( "1234567890123456789" )
        )

    [<Theory>]
    [<InlineData( " 1" )>]
    [<InlineData( "  1" )>]
    [<InlineData( "\t1" )>]
    [<InlineData( "\t 1" )>]
    [<InlineData( "\t\t1" )>]
    [<InlineData( "\n1" )>]
    [<InlineData( "\r1" )>]
    [<InlineData( "\r\n1" )>]
    [<InlineData( "\n\r1" )>]
    member public this.AllowLeadingWhiteSpace s =
        Assert.Equal(
            Natural.Unit,
            Natural.Parse( s )
        )

    [<Theory>]
    [<InlineData( "1 " )>]
    [<InlineData( "1  " )>]
    [<InlineData( "1\t" )>]
    [<InlineData( "1\t " )>]
    [<InlineData( "1\t\t" )>]
    [<InlineData( "1\n" )>]
    [<InlineData( "1\r" )>]
    [<InlineData( "1\r\n" )>]
    [<InlineData( "1\n\r" )>]
    member public this.AllowTrailingWhiteSpace s =
        Assert.Equal(
            Natural.Unit,
            Natural.Parse( s )
        )

    [<Theory>]
    [<InlineData( " 1 " )>]
    [<InlineData( "  1 " )>]
    [<InlineData( "\t1 " )>]
    [<InlineData( "\t 1\t" )>]
    [<InlineData( "\t\t1  " )>]
    [<InlineData( " 1\n" )>]
    [<InlineData( "\t1\r" )>]
    [<InlineData( "  1\r\n" )>]
    [<InlineData( "\t 1\n\r" )>]
    member public this.AllowWhiteSpace (s:string) =
        Assert.Equal(
            Natural.Unit,
            Natural.Parse( s )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        Assert.Equal(
            Natural.Unit,
            Natural.Parse( $"{currentCulture.PositiveSign}1" )
        )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () -> Natural.Parse( $"{currentCulture.NegativeSign}1" ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        Assert.Equal(
            Natural.Zero,
            Natural.Parse( $"{currentCulture.NegativeSign}0" )
        )

    [<Fact>]
    member public this.DisallowBothLeading () =
        // I would like for this to be a theory with the two cases separate
        // However, I can't do string interpolation in an attribute
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"{currentCulture.PositiveSign}{currentCulture.NegativeSign}0" ) |> ignore
            )
        ) |> ignore
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"{currentCulture.NegativeSign}{currentCulture.PositiveSign}0" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"1{currentCulture.PositiveSign}" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"1{currentCulture.NegativeSign}" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"0{currentCulture.NegativeSign}" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowParentheses () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( "(1)" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( "(0)" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( "1.0" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( "1.0" ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        Assert.Equal(
            Natural( 1234u ),
            Natural.Parse( $"1{currentCulture.NumberGroupSeparator}234" )
        )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"10{exp}1" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"{currentCulture.CurrencySymbol}1" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"1{currentCulture.CurrencySymbol}0" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowHex () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( "1A" ) |> ignore
            )
        )

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        Assert.Equal(
            Natural( 11u ),
            Natural.Parse( "11" )
        )

type public ParseStringStyleFormat() =
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
    let cultures = [ usCulture; ukCulture; frCulture; luCulture ]

    let currentCulture = System.Globalization.CultureInfo.CurrentCulture.NumberFormat

    static member NumberBase<'T when 'T :> System.Numerics.INumberBase<'T>>( s:string, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( s, style, provider )
    static member NumberBase<'T when 'T :> System.Numerics.INumberBase<'T>>( charSpan:System.ReadOnlySpan<char>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( charSpan, style, provider )
    static member NumberBase<'T when 'T :> System.Numerics.INumberBase<'T>>( byteSpan:System.ReadOnlySpan<byte>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( byteSpan, style, provider )

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            ParseStringStyleFormat.NumberBase<Natural>(
                s,
                System.Globalization.NumberStyles.Integer,
                currentCulture
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            Natural( [ 0x112210F4u; 0x7DE98115u ] ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "1234567890123456789",
                System.Globalization.NumberStyles.Integer,
                currentCulture
            )
        )

    [<Theory>]
    [<InlineData( " " )>]
    [<InlineData( "  " )>]
    [<InlineData( "\t" )>]
    [<InlineData( "\t " )>]
    [<InlineData( "\t\t" )>]
    [<InlineData( "\n" )>]
    [<InlineData( "\r" )>]
    [<InlineData( "\r\n" )>]
    [<InlineData( "\n\r" )>]
    [<InlineData( "\n\t\r " )>]
    member public this.AllowLeadingWhiteSpace s =
        Assert.Equal(
            Natural.Unit,
            ParseStringStyleFormat.NumberBase<Natural>(
                $"{s}1",
                System.Globalization.NumberStyles.AllowLeadingWhite,
                currentCulture
            )
        )

    [<Theory>]
    [<InlineData( " " )>]
    [<InlineData( "  " )>]
    [<InlineData( "\t" )>]
    [<InlineData( "\t " )>]
    [<InlineData( "\t\t" )>]
    [<InlineData( "\n" )>]
    [<InlineData( "\r" )>]
    [<InlineData( "\r\n" )>]
    [<InlineData( "\n\r" )>]
    [<InlineData( "\n\t\r " )>]
    member public this.AllowTrailingWhiteSpace s =
        Assert.Equal(
            Natural.Unit,
            ParseStringStyleFormat.NumberBase<Natural>(
                $"1{s}",
                System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )

    [<Theory>]
    [<InlineData( " " )>]
    [<InlineData( "  " )>]
    [<InlineData( "\t" )>]
    [<InlineData( "\t " )>]
    [<InlineData( "\t\t" )>]
    [<InlineData( "\n" )>]
    [<InlineData( "\r" )>]
    [<InlineData( "\r\n" )>]
    [<InlineData( "\n\r" )>]
    [<InlineData( "\n\t\r " )>]
    member public this.AllowWhiteSpace s =
        Assert.Equal(
            Natural.Unit,
            ParseStringStyleFormat.NumberBase<Natural>(
                $"{s}1",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            ParseStringStyleFormat.NumberBase<Natural>(
                $"1{s}",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            ParseStringStyleFormat.NumberBase<Natural>(
                $"{s}1{s}",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseStringStyleFormat.NumberBase<Natural>(
                    $"{culture.NumberFormat.PositiveSign}1",
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture
                )
            )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        ParseStringStyleFormat.NumberBase<Natural>(
                            $"{culture.NumberFormat.NegativeSign}1",
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                ParseStringStyleFormat.NumberBase<Natural>(
                    $"{culture.NumberFormat.NegativeSign}0",
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture
                )
            )

    [<Fact>]
    member public this.DisallowBothLeading () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        ParseStringStyleFormat.NumberBase<Natural>(
                            $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0",
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        ParseStringStyleFormat.NumberBase<Natural>(
                            $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0",
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowTrailingPositive () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseStringStyleFormat.NumberBase<Natural>(
                    $"1{culture.NumberFormat.PositiveSign}",
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture
                )
            )

    [<Fact>]
    member public this.TrailingNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        ParseStringStyleFormat.NumberBase<Natural>(
                            $"1{culture.NumberFormat.NegativeSign}",
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowTrailingNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                ParseStringStyleFormat.NumberBase<Natural>(
                    $"0{culture.NumberFormat.NegativeSign}",
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture
                )
            )

    [<Fact>]
    member public this.DisallowBothTrailing () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        ParseStringStyleFormat.NumberBase<Natural>(
                            $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}",
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        ParseStringStyleFormat.NumberBase<Natural>(
                            $"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}",
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ParenthesesNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        ParseStringStyleFormat.NumberBase<Natural>(
                            "(1)",
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowParenthesesNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                ParseStringStyleFormat.NumberBase<Natural>(
                    "(0)",
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture
                )
            )

    [<Fact>]
    member public this.ParenthesesOnlyOpen () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> 
                        ParseStringStyleFormat.NumberBase<Natural>(
                            "(0",
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ParenthesesOnlyClose () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> 
                        ParseStringStyleFormat.NumberBase<Natural>(
                            "0)",
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ParenthesesOutOfOrder () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> 
                        ParseStringStyleFormat.NumberBase<Natural>(
                            ")0(",
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ParenthesesMoreThanOne () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> 
                        ParseStringStyleFormat.NumberBase<Natural>(
                            "((0))",
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DecimalPointOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    ParseStringStyleFormat.NumberBase<Natural>(
                        $"1{currentCulture.NumberDecimalSeparator}1",
                        System.Globalization.NumberStyles.AllowDecimalPoint,
                        currentCulture
                    ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowDecimalPointZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseStringStyleFormat.NumberBase<Natural>(
                    $"1{culture.NumberFormat.NumberDecimalSeparator}0",
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture
                )
            )

    [<Fact>]
    member public this.DecimalMoreThanOne () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    ParseStringStyleFormat.NumberBase<Natural>(
                        $"1{currentCulture.NumberDecimalSeparator}0{currentCulture.NumberDecimalSeparator}0",
                        System.Globalization.NumberStyles.AllowDecimalPoint,
                        currentCulture
                    ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        for culture in cultures do
            Assert.Equal(
                small,
                ParseStringStyleFormat.NumberBase<Natural>(
                    $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890",
                    System.Globalization.NumberStyles.AllowThousands,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowExponent () =
        Assert.Equal(
            Natural( 10u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "1e1",
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "2E2",
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        Assert.Equal(
            Natural( 12u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "1.2e1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 201u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "2.01E2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowNegativeExponentWithDecimal () =
        // Yes, these look silly, but they are technically valid
        Assert.Equal(
            Natural.Unit,
            ParseStringStyleFormat.NumberBase<Natural>(
                "10.0e-1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "200.0E-2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        Assert.Equal(
            Natural( 10u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "1e+1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "2E+2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        Assert.Equal(
            Natural.Unit,
            ParseStringStyleFormat.NumberBase<Natural>(
                "10e-1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "200E-2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

    [<Fact>]
    member public this.ExponentWithNegativeSignOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    ParseStringStyleFormat.NumberBase<Natural>(
                        "1e-1",
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                        currentCulture
                    ) |> ignore
            )
        ) |> ignore

        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    ParseStringStyleFormat.NumberBase<Natural>(
                        "2E-2",
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                        currentCulture
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.AllowCurrencySymbolPrefix () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseStringStyleFormat.NumberBase<Natural>(
                    $"{culture.NumberFormat.CurrencySymbol}1",
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowCurrencySymbolPostfix () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseStringStyleFormat.NumberBase<Natural>(
                    $"1{culture.NumberFormat.CurrencySymbol}",
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowHex () =
        // If you're curious, that's equal to "1,311,768,467,294,899,695"
        // It was verified with the Windows Calculator app
        let value = Natural( [ 305419896u; 2427178479u ] )

        Assert.Equal(
            value,
            ParseStringStyleFormat.NumberBase<Natural>(
                "1234567890ABCDEF",
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

        Assert.Equal(
            value,
            ParseStringStyleFormat.NumberBase<Natural>(
                "1234567890abcdef",
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowBinary () =
        Assert.Equal(
            Natural( 172u ),
            ParseStringStyleFormat.NumberBase<Natural>(
                "10101100",
                System.Globalization.NumberStyles.AllowBinarySpecifier,
                currentCulture
            )
        )

type public ParseSpanStyleFormat() =
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
    let cultures = [ usCulture; ukCulture; frCulture; luCulture ]

    let currentCulture = System.Globalization.CultureInfo.CurrentCulture.NumberFormat

    static member NumberBase<'T when 'T :> System.Numerics.INumberBase<'T>>( s:string, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( s, style, provider )
    static member NumberBase<'T when 'T :> System.Numerics.INumberBase<'T>>( charSpan:System.ReadOnlySpan<char>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( charSpan, style, provider )
    static member NumberBase<'T when 'T :> System.Numerics.INumberBase<'T>>( byteSpan:System.ReadOnlySpan<byte>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( byteSpan, style, provider )

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            ParseSpanStyleFormat.NumberBase<Natural>(
                s.AsSpan(),
                System.Globalization.NumberStyles.Integer,
                currentCulture
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            medium,
            ParseSpanStyleFormat.NumberBase<Natural>(
                mediumStr.AsSpan(),
                System.Globalization.NumberStyles.Integer,
                currentCulture
            )
        )

    [<Theory>]
    [<InlineData( " " )>]
    [<InlineData( "  " )>]
    [<InlineData( "\t" )>]
    [<InlineData( "\t " )>]
    [<InlineData( "\t\t" )>]
    [<InlineData( "\n" )>]
    [<InlineData( "\r" )>]
    [<InlineData( "\r\n" )>]
    [<InlineData( "\n\r" )>]
    [<InlineData( "\n\t\r " )>]
    member public this.AllowLeadingWhiteSpace s =
        Assert.Equal(
            Natural.Unit,
            ParseSpanStyleFormat.NumberBase<Natural>(
                $"{s}1".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite,
                currentCulture
            )
        )

    [<Theory>]
    [<InlineData( " " )>]
    [<InlineData( "  " )>]
    [<InlineData( "\t" )>]
    [<InlineData( "\t " )>]
    [<InlineData( "\t\t" )>]
    [<InlineData( "\n" )>]
    [<InlineData( "\r" )>]
    [<InlineData( "\r\n" )>]
    [<InlineData( "\n\r" )>]
    [<InlineData( "\n\t\r " )>]
    member public this.AllowTrailingWhiteSpace s =
        Assert.Equal(
            Natural.Unit,
            ParseSpanStyleFormat.NumberBase<Natural>(
                $"1{s}".AsSpan(),
                System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )

    [<Theory>]
    [<InlineData( " " )>]
    [<InlineData( "  " )>]
    [<InlineData( "\t" )>]
    [<InlineData( "\t " )>]
    [<InlineData( "\t\t" )>]
    [<InlineData( "\n" )>]
    [<InlineData( "\r" )>]
    [<InlineData( "\r\n" )>]
    [<InlineData( "\n\r" )>]
    [<InlineData( "\n\t\r " )>]
    member public this.AllowWhiteSpace s =
        Assert.Equal(
            Natural.Unit,
            ParseSpanStyleFormat.NumberBase<Natural>(
                $"{s}1".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            ParseSpanStyleFormat.NumberBase<Natural>(
                $"1{s}".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            ParseSpanStyleFormat.NumberBase<Natural>(
                $"{s}1{s}".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    $"{culture.NumberFormat.PositiveSign}1".AsSpan(),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture
                )
            )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            $"{culture.NumberFormat.NegativeSign}1".AsSpan(),
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    $"{culture.NumberFormat.NegativeSign}0".AsSpan(),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture
                )
            )

    [<Fact>]
    member public this.DisallowBothLeading () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(),
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0".AsSpan(),
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowTrailingPositive () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    $"1{culture.NumberFormat.PositiveSign}".AsSpan(),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture
                )
            )

    [<Fact>]
    member public this.TrailingNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            $"1{culture.NumberFormat.NegativeSign}".AsSpan(),
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowTrailingNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    $"0{culture.NumberFormat.NegativeSign}".AsSpan(),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture
                )
            )

    [<Fact>]
    member public this.DisallowBothTrailing () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}".AsSpan(),
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            $"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}".AsSpan(),
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ParenthesesNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            "(1)".AsSpan(),
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowParenthesesNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    "(0)".AsSpan(),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture
                )
            )

    [<Fact>]
    member public this.ParenthesesOnlyOpen () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> 
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            "(0".AsSpan(),
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ParenthesesOnlyClose () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> 
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            "0)".AsSpan(),
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ParenthesesOutOfOrder () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> 
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            ")0(".AsSpan(),
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ParenthesesMoreThanOne () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> 
                        ParseSpanStyleFormat.NumberBase<Natural>(
                            "((0))".AsSpan(),
                            System.Globalization.NumberStyles.AllowParentheses,
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DecimalPointOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    ParseSpanStyleFormat.NumberBase<Natural>(
                        $"1{currentCulture.NumberDecimalSeparator}1".AsSpan(),
                        System.Globalization.NumberStyles.AllowDecimalPoint,
                        currentCulture
                    ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowDecimalPointZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    $"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture
                )
            )

    [<Fact>]
    member public this.DecimalMoreThanOne () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    ParseSpanStyleFormat.NumberBase<Natural>(
                        $"1{currentCulture.NumberDecimalSeparator}0{currentCulture.NumberDecimalSeparator}0".AsSpan(),
                        System.Globalization.NumberStyles.AllowDecimalPoint,
                        currentCulture
                    ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        for culture in cultures do
            Assert.Equal(
                small,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890".AsSpan(),
                    System.Globalization.NumberStyles.AllowThousands,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowExponent () =
        Assert.Equal(
            Natural( 10u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "1e1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "2E2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        Assert.Equal(
            Natural( 12u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "1.2e1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 201u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "2.01E2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowNegativeExponentWithDecimal () =
        // Yes, these look silly, but they are technically valid
        Assert.Equal(
            Natural.Unit,
            ParseSpanStyleFormat.NumberBase<Natural>(
                "10.0e-1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "200.0E-2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        Assert.Equal(
            Natural( 10u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "1e+1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "2E+2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        Assert.Equal(
            Natural.Unit,
            ParseSpanStyleFormat.NumberBase<Natural>(
                "10e-1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "200E-2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

    [<Fact>]
    member public this.ExponentWithNegativeSignOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    ParseSpanStyleFormat.NumberBase<Natural>(
                        "1e-1".AsSpan(),
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                        currentCulture
                    ) |> ignore
            )
        ) |> ignore

        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    ParseSpanStyleFormat.NumberBase<Natural>(
                        "2E-2".AsSpan(),
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                        currentCulture
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.AllowCurrencySymbolPrefix () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    $"{culture.NumberFormat.CurrencySymbol}1".AsSpan(),
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowCurrencySymbolPostfix () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                ParseSpanStyleFormat.NumberBase<Natural>(
                    $"1{culture.NumberFormat.CurrencySymbol}".AsSpan(),
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowHex () =
        // If you're curious, that's equal to "1,311,768,467,294,899,695"
        // It was verified with the Windows Calculator app
        let value = Natural( [ 305419896u; 2427178479u ] )

        Assert.Equal(
            value,
            ParseSpanStyleFormat.NumberBase<Natural>(
                "1234567890ABCDEF".AsSpan(),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

        Assert.Equal(
            value,
            ParseSpanStyleFormat.NumberBase<Natural>(
                "1234567890abcdef".AsSpan(),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowBinary () =
        Assert.Equal(
            Natural( 172u ),
            ParseSpanStyleFormat.NumberBase<Natural>(
                "10101100".AsSpan(),
                System.Globalization.NumberStyles.AllowBinarySpecifier,
                currentCulture
            )
        )

