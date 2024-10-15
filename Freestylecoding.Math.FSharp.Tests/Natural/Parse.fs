namespace Natural

open System
open Xunit
open Freestylecoding.Math

type public Overloads() =
    // From Natural
    static member Parse( s:string ) : Natural =
        Natural.Parse( s )
    static member Parse( s:string, style:System.Globalization.NumberStyles ) : Natural =
        Natural.Parse( s, style )

    // From IParsable<Natural>
    static member private Parse<'T when 'T :> System.IParsable<'T>>( s:string, provider:System.IFormatProvider ) : 'T =
        'T.Parse( s, provider )
    static member Parse( s:string, provider:System.IFormatProvider ) : Natural =
        Overloads.Parse<Natural>( s, provider )

    // From ISpanParsable<Natural>
    static member private Parse<'T when 'T :> System.ISpanParsable<'T>>( charSpan:System.ReadOnlySpan<char>, provider:System.IFormatProvider ) : 'T =
        'T.Parse( charSpan, provider )
    static member Parse( charSpan:System.ReadOnlySpan<char>, provider:System.IFormatProvider ) : Natural =
        Overloads.Parse<Natural>( charSpan, provider )

    // From ISpanParsable<Natural>
    static member private Parse<'T when 'T :> System.IUtf8SpanParsable<'T>>( byteSpan:System.ReadOnlySpan<byte>, provider:System.IFormatProvider ) : 'T =
        'T.Parse( byteSpan, provider )
    static member Parse( byteSpan:System.ReadOnlySpan<byte>, provider:System.IFormatProvider ) : Natural =
        Overloads.Parse<Natural>( byteSpan, provider )

    // From INumberBase<Natural>
    static member private Parse<'T when 'T :> System.Numerics.INumberBase<'T>>( s:string, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( s, style, provider )
    static member private Parse<'T when 'T :> System.Numerics.INumberBase<'T>>( charSpan:System.ReadOnlySpan<char>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( charSpan, style, provider )
    static member private Parse<'T when 'T :> System.Numerics.INumberBase<'T>>( byteSpan:System.ReadOnlySpan<byte>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : 'T =
        'T.Parse( byteSpan, style, provider )
    static member Parse( s:string, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : Natural =
        Overloads.Parse<Natural>( s, style, provider )
    static member Parse( charSpan:System.ReadOnlySpan<char>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : Natural =
        Overloads.Parse<Natural>( charSpan, style, provider )
    static member Parse( byteSpan:System.ReadOnlySpan<byte>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider ) : Natural =
        Overloads.Parse<Natural>( byteSpan, style, provider )

type public ParseString() =
    let currentCulture = System.Globalization.CultureInfo.CurrentCulture.NumberFormat

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n s =
        Assert.Equal( Natural([n]), Overloads.Parse(s) )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            Natural( [ 0x112210F4u; 0x7DE98115u ] ),
            Overloads.Parse( "1234567890123456789" )
        )

    [<Fact>]
    member public this.InsaneSanity () =
        // This test case is for a very specialized test case
        // That's 1E80, which is roughly the number of atoms in the universe
        Assert.Equal(
            Natural( [ 863u; 2649374239u; 1809837936u; 3453057829u; 4020508874u; 1671571300u; 3468754944u; 0u; 0u ] ),
            Overloads.Parse( "100000000000000000000000000000000000000000000000000000000000000000000000000000000" )
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
            Overloads.Parse( s )
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
            Overloads.Parse( s )
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
            Overloads.Parse( s )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse( $"{currentCulture.PositiveSign}1" )
        )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"{currentCulture.NegativeSign}1" ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        Assert.Equal(
            Natural.Zero,
            Overloads.Parse( $"{currentCulture.NegativeSign}0" )
        )

    [<Fact>]
    member public this.DisallowBothLeading () =
        // I would like for this to be a theory with the two cases separate
        // However, I can't do string interpolation in an attribute
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"{currentCulture.PositiveSign}{currentCulture.NegativeSign}0" ) |> ignore
            )
        ) |> ignore
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"{currentCulture.NegativeSign}{currentCulture.PositiveSign}0" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"1{currentCulture.PositiveSign}" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"1{currentCulture.NegativeSign}" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"0{currentCulture.NegativeSign}" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowParentheses () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( "(1)" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( "(0)" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( "1.0" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( "1.0" ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        Assert.Equal(
            Natural( 1234u ),
            Overloads.Parse( $"1{currentCulture.NumberGroupSeparator}234" )
        )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"10{exp}1" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"{currentCulture.CurrencySymbol}1" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( $"1{currentCulture.CurrencySymbol}" ) |> ignore
            )
        )

    [<Fact>]
    member public this.DisallowHex () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Overloads.Parse( "1A" ) |> ignore
            )
        )

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        Assert.Equal(
            Natural( 11u ),
            Overloads.Parse( "11" )
        )

type public ParseStringStyle() =
    let small  = Natural( [0x4996_02D2u] )
    let smallStr = "1234567890"

    let medium = Natural( [0xAB54_A98Cu; 0xEB1F_0AD2u] )
    let mediumStr = "12345678901234567890"

    let large  = Natural( [0x0000_0001u; 0x8EE9_0FF6u; 0xC373_E0EEu; 0x4E3F_0AD2u] )
    let largeStr = "123456789012345678901234567890"

    let currentCulture = System.Globalization.CultureInfo.CurrentCulture.NumberFormat

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            Overloads.Parse(
                s,
                System.Globalization.NumberStyles.Integer
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            Natural( [ 0x112210F4u; 0x7DE98115u ] ),
            Overloads.Parse(
                "1234567890123456789",
                System.Globalization.NumberStyles.Integer
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
            Overloads.Parse(
                $"{s}1",
                System.Globalization.NumberStyles.AllowLeadingWhite
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
            Overloads.Parse(
                $"1{s}",
                System.Globalization.NumberStyles.AllowTrailingWhite
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
            Overloads.Parse(
                $"{s}1",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"1{s}",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"{s}1{s}",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite
            )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"{currentCulture.PositiveSign}1",
                System.Globalization.NumberStyles.AllowLeadingSign
            )
        )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () -> 
                    Overloads.Parse(
                        $"{currentCulture.NegativeSign}1",
                        System.Globalization.NumberStyles.AllowLeadingSign
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        Assert.Equal(
            Natural.Zero,
            Overloads.Parse(
                $"{currentCulture.NegativeSign}0",
                System.Globalization.NumberStyles.AllowLeadingSign
            )
        )

    [<Fact>]
    member public this.DisallowBothLeading () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        $"{currentCulture.PositiveSign}{currentCulture.NegativeSign}0",
                        System.Globalization.NumberStyles.AllowLeadingSign
                    ) |> ignore
            )
        ) |> ignore
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        $"{currentCulture.NegativeSign}{currentCulture.PositiveSign}0",
                        System.Globalization.NumberStyles.AllowLeadingSign
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.AllowTrailingPositive () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"1{currentCulture.PositiveSign}",
                System.Globalization.NumberStyles.AllowTrailingSign
            )
        )

    [<Fact>]
    member public this.TrailingNegativeOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () -> 
                    Overloads.Parse(
                        $"1{currentCulture.NegativeSign}",
                        System.Globalization.NumberStyles.AllowTrailingSign
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.AllowTrailingNegativeZero () =
        Assert.Equal(
            Natural.Zero,
            Overloads.Parse(
                $"0{currentCulture.NegativeSign}",
                System.Globalization.NumberStyles.AllowTrailingSign
            )
        )

    [<Fact>]
    member public this.DisallowBothTrailing () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        $"0{currentCulture.PositiveSign}{currentCulture.NegativeSign}",
                        System.Globalization.NumberStyles.AllowTrailingSign
                    ) |> ignore
            )
        ) |> ignore
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        $"0{currentCulture.NegativeSign}{currentCulture.PositiveSign}",
                        System.Globalization.NumberStyles.AllowTrailingSign
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.ParenthesesNegativeOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () -> 
                    Overloads.Parse(
                        "(1)",
                        System.Globalization.NumberStyles.AllowParentheses
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.AllowParenthesesNegativeZero () =
            Assert.Equal(
                Natural.Zero,
                Overloads.Parse(
                    "(0)",
                    System.Globalization.NumberStyles.AllowParentheses
                )
            )

    [<Fact>]
    member public this.ParenthesesOnlyOpen () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> 
                    Overloads.Parse(
                        "(0",
                        System.Globalization.NumberStyles.AllowParentheses
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.ParenthesesOnlyClose () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> 
                    Overloads.Parse(
                        "0)",
                        System.Globalization.NumberStyles.AllowParentheses
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.ParenthesesOutOfOrder () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> 
                    Overloads.Parse(
                        ")0(",
                        System.Globalization.NumberStyles.AllowParentheses
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.ParenthesesMoreThanOne () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> 
                    Overloads.Parse(
                        "((0))",
                        System.Globalization.NumberStyles.AllowParentheses
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.DecimalPointOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        $"1{currentCulture.NumberDecimalSeparator}1",
                        System.Globalization.NumberStyles.AllowDecimalPoint
                    ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowDecimalPointZero () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"1{currentCulture.NumberDecimalSeparator}0",
                System.Globalization.NumberStyles.AllowDecimalPoint
            )
        )

    [<Fact>]
    member public this.DecimalMoreThanOne () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        $"1{currentCulture.NumberDecimalSeparator}0{currentCulture.NumberDecimalSeparator}0",
                        System.Globalization.NumberStyles.AllowDecimalPoint
                    ) |> ignore
            )
        )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        Assert.Equal(
            small,
            Overloads.Parse(
                $"1{currentCulture.NumberGroupSeparator}234{currentCulture.NumberGroupSeparator}567{currentCulture.NumberGroupSeparator}890",
                System.Globalization.NumberStyles.AllowThousands
            )
        )

    [<Fact>]
    member public this.AllowExponent () =
        Assert.Equal(
            Natural( 10u ),
            Overloads.Parse(
                "1e1",
                System.Globalization.NumberStyles.AllowExponent
            )
        )

        Assert.Equal(
            Natural( 200u ),
            Overloads.Parse(
                "2E2",
                System.Globalization.NumberStyles.AllowExponent
            )
        )

    [<Fact>]
    member public this.AllowLargeExponent () =
        // This test is mainly to hit a few areas in the base pasre code
        Assert.Equal(
            Natural( [ 0x2u; 0x540B_E400u ] ),
            Overloads.Parse(
                "1e10",
                System.Globalization.NumberStyles.AllowExponent
            )
        )

        // 1E80 is roughly the number of atoms in the universe
        Assert.Equal(
            Natural( [ 863u; 2649374239u; 1809837936u; 3453057829u; 4020508874u; 1671571300u; 3468754944u; 0u; 0u ] ),
            Overloads.Parse(
                "1e80",
                System.Globalization.NumberStyles.AllowExponent
            )
        )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        Assert.Equal(
            Natural( 12u ),
            Overloads.Parse(
                "1.2e1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint
            )
        )

        Assert.Equal(
            Natural( 201u ),
            Overloads.Parse(
                "2.01E2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint
            )
        )

    [<Fact>]
    member public this.AllowNegativeExponentWithDecimal () =
        // Yes, these look silly, but they are technically valid
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                "10.0e-1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint
            )
        )

        Assert.Equal(
            Natural( 2u ),
            Overloads.Parse(
                "200.0E-2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint
            )
        )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        Assert.Equal(
            Natural( 10u ),
            Overloads.Parse(
                "1e+1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign
            )
        )

        Assert.Equal(
            Natural( 200u ),
            Overloads.Parse(
                "2E+2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign
            )
        )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                "10e-1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign
            )
        )

        Assert.Equal(
            Natural( 2u ),
            Overloads.Parse(
                "200E-2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign
            )
        )

    [<Fact>]
    member public this.ExponentWithNegativeSignOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        "1e-1",
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign
                    ) |> ignore
            )
        ) |> ignore

        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        "2E-2",
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.AllowCurrencySymbolPrefix () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"{currentCulture.CurrencySymbol}1",
                System.Globalization.NumberStyles.AllowCurrencySymbol
            )
        )

    [<Fact>]
    member public this.AllowCurrencySymbolPostfix () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"1{currentCulture.CurrencySymbol}",
                System.Globalization.NumberStyles.AllowCurrencySymbol
            )
        )

    [<Fact>]
    member public this.AllowHex () =
        // If you're curious, that's equal to "1,311,768,467,294,899,695"
        // It was verified with the Windows Calculator app
        let value = Natural( [ 305419896u; 2427178479u ] )

        Assert.Equal(
            value,
            Overloads.Parse(
                "01234567890ABCDEF",
                System.Globalization.NumberStyles.AllowHexSpecifier
            )
        )

        Assert.Equal(
            value,
            Overloads.Parse(
                "01234567890abcdef",
                System.Globalization.NumberStyles.AllowHexSpecifier
            )
        )

    [<Fact>]
    member public this.AllowHex_BadInput () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        "1234567890ABCDEFG",
                        System.Globalization.NumberStyles.AllowHexSpecifier
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.AllowBinary () =
        Assert.Equal(
            Natural( 172u ),
            Overloads.Parse(
                "10101100",
                System.Globalization.NumberStyles.AllowBinarySpecifier
            )
        )

    [<Fact>]
    member public this.AllowBinary_BadInput () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        "012",
                        System.Globalization.NumberStyles.AllowBinarySpecifier
                    ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.DisallowBothBinaryAndHex () =
        Assert.IsType<System.ArgumentException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        "0",
                        System.Globalization.NumberStyles.AllowBinarySpecifier ||| System.Globalization.NumberStyles.AllowHexSpecifier
                    ) |> ignore
            )
        ) |> ignore

type public ParseStringFormat() =
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

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            Overloads.Parse(
                s,
                currentCulture
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            Natural( [ 0x112210F4u; 0x7DE98115u ] ),
            Overloads.Parse(
                "1234567890123456789",
                currentCulture
            )
        )

    [<Fact>]
    member public this.NullFormatIsCurrentCulture () =
        Assert.Equal(
            Overloads.Parse( smallStr, currentCulture ),
            Overloads.Parse( smallStr, null )
        )
        Assert.Equal(
            Overloads.Parse( mediumStr, currentCulture ),
            Overloads.Parse( mediumStr, null )
        )
        Assert.Equal(
            Overloads.Parse( largeStr, currentCulture ),
            Overloads.Parse( largeStr, null )
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
            Overloads.Parse(
                $"{s}1",
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
            Overloads.Parse(
                $"1{s}",
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
            Overloads.Parse(
                $"{s}1",
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"1{s}",
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"{s}1{s}",
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                Overloads.Parse(
                    $"{culture.NumberFormat.PositiveSign}1",
                    culture
                )
            )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        Overloads.Parse(
                            $"{culture.NumberFormat.NegativeSign}1",
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                Overloads.Parse(
                    $"{culture.NumberFormat.NegativeSign}0",
                    culture
                )
            )

    [<Fact>]
    member public this.DisallowBothLeading () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
                            $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0",
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
                            $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0",
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{culture.NumberFormat.PositiveSign}", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{culture.NumberFormat.NegativeSign}", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"0{culture.NumberFormat.NegativeSign}", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowParentheses () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( "(1)", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( "(0)", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{culture.NumberFormat.NumberDecimalSeparator}0", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{culture.NumberFormat.NumberDecimalSeparator}0", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowGroupSeparator () =
        for culture in cultures do
            Assert.Equal(
                Natural( 1234u ),
                Overloads.Parse( $"1{culture.NumberFormat.NumberGroupSeparator}234", culture )
            )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"10{exp}1", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"{culture.NumberFormat.CurrencySymbol}1", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{currentCulture.CurrencySymbol}", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowHex () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( "1A", culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        for culture in cultures do
            Assert.Equal(
                Natural( 11u ),
                Overloads.Parse( "11", culture )
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

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            Overloads.Parse(
                s,
                System.Globalization.NumberStyles.Integer,
                currentCulture
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            Natural( [ 0x112210F4u; 0x7DE98115u ] ),
            Overloads.Parse(
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
            Overloads.Parse(
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
            Overloads.Parse(
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
            Overloads.Parse(
                $"{s}1",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"1{s}",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
                            $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0",
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
                            $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}",
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
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
                        Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
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
                        Overloads.Parse(
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
                        Overloads.Parse(
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
                        Overloads.Parse(
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
                    Overloads.Parse(
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
                Overloads.Parse(
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
                    Overloads.Parse(
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
                Overloads.Parse(
                    $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890",
                    System.Globalization.NumberStyles.AllowThousands,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowExponent () =
        Assert.Equal(
            Natural( 10u ),
            Overloads.Parse(
                "1e1",
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            Overloads.Parse(
                "2E2",
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        Assert.Equal(
            Natural( 12u ),
            Overloads.Parse(
                "1.2e1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 201u ),
            Overloads.Parse(
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
            Overloads.Parse(
                "10.0e-1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            Overloads.Parse(
                "200.0E-2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        Assert.Equal(
            Natural( 10u ),
            Overloads.Parse(
                "1e+1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            Overloads.Parse(
                "2E+2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                "10e-1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            Overloads.Parse(
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
                    Overloads.Parse(
                        "1e-1",
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                        currentCulture
                    ) |> ignore
            )
        ) |> ignore

        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
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
                Overloads.Parse(
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
                Overloads.Parse(
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
            Overloads.Parse(
                "1234567890ABCDEF",
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

        Assert.Equal(
            value,
            Overloads.Parse(
                "1234567890abcdef",
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowBinary () =
        Assert.Equal(
            Natural( 172u ),
            Overloads.Parse(
                "10101100",
                System.Globalization.NumberStyles.AllowBinarySpecifier,
                currentCulture
            )
        )

type public ParseSpanFormat() =
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

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            Overloads.Parse(
                s.AsSpan(),
                currentCulture
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            Natural( [ 0x112210F4u; 0x7DE98115u ] ),
            Overloads.Parse(
                "1234567890123456789".AsSpan(),
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
            Overloads.Parse(
                $"{s}1".AsSpan(),
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
            Overloads.Parse(
                $"1{s}".AsSpan(),
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
            Overloads.Parse(
                $"{s}1".AsSpan(),
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"1{s}".AsSpan(),
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"{s}1{s}".AsSpan(),
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                Overloads.Parse(
                    $"{culture.NumberFormat.PositiveSign}1".AsSpan(),
                    culture
                )
            )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        Overloads.Parse(
                            $"{culture.NumberFormat.NegativeSign}1".AsSpan(),
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                Overloads.Parse(
                    $"{culture.NumberFormat.NegativeSign}0".AsSpan(),
                    culture
                )
            )

    [<Fact>]
    member public this.DisallowBothLeading () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
                            $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(),
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
                            $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0".AsSpan(),
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{culture.NumberFormat.PositiveSign}".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{culture.NumberFormat.NegativeSign}".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"0{culture.NumberFormat.NegativeSign}".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowParentheses () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( "(1)".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( "(0)".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowGroupSeparator () =
        for culture in cultures do
            Assert.Equal(
                Natural( 1234u ),
                Overloads.Parse( $"1{culture.NumberFormat.NumberGroupSeparator}234".AsSpan(), culture )
            )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"10{exp}1".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"{culture.NumberFormat.CurrencySymbol}1".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( $"1{currentCulture.CurrencySymbol}".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowHex () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( "1A".AsSpan(), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        for culture in cultures do
            Assert.Equal(
                Natural( 11u ),
                Overloads.Parse( "11".AsSpan(), culture )
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

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            Overloads.Parse(
                s.AsSpan(),
                System.Globalization.NumberStyles.Integer,
                currentCulture
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            medium,
            Overloads.Parse(
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
            Overloads.Parse(
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
            Overloads.Parse(
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
            Overloads.Parse(
                $"{s}1".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                $"1{s}".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
                            $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(),
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
                            $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}".AsSpan(),
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
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
                        Overloads.Parse(
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
                Overloads.Parse(
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
                        Overloads.Parse(
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
                        Overloads.Parse(
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
                        Overloads.Parse(
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
                        Overloads.Parse(
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
                    Overloads.Parse(
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
                Overloads.Parse(
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
                    Overloads.Parse(
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
                Overloads.Parse(
                    $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890".AsSpan(),
                    System.Globalization.NumberStyles.AllowThousands,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowExponent () =
        Assert.Equal(
            Natural( 10u ),
            Overloads.Parse(
                "1e1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            Overloads.Parse(
                "2E2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        Assert.Equal(
            Natural( 12u ),
            Overloads.Parse(
                "1.2e1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 201u ),
            Overloads.Parse(
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
            Overloads.Parse(
                "10.0e-1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            Overloads.Parse(
                "200.0E-2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        Assert.Equal(
            Natural( 10u ),
            Overloads.Parse(
                "1e+1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            Overloads.Parse(
                "2E+2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                "10e-1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            Overloads.Parse(
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
                    Overloads.Parse(
                        "1e-1".AsSpan(),
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                        currentCulture
                    ) |> ignore
            )
        ) |> ignore

        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
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
                Overloads.Parse(
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
                Overloads.Parse(
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
            Overloads.Parse(
                "1234567890ABCDEF".AsSpan(),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

        Assert.Equal(
            value,
            Overloads.Parse(
                "1234567890abcdef".AsSpan(),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowBinary () =
        Assert.Equal(
            Natural( 172u ),
            Overloads.Parse(
                "10101100".AsSpan(),
                System.Globalization.NumberStyles.AllowBinarySpecifier,
                currentCulture
            )
        )

type public ParseUtf8Format() =
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

    let toSpan (s:string) : ReadOnlySpan<byte> =
        ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes(s) )

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            Overloads.Parse(
                (toSpan s),
                currentCulture
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            Natural( [ 0x112210F4u; 0x7DE98115u ] ),
            Overloads.Parse(
                (toSpan "1234567890123456789"),
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
            Overloads.Parse(
                (toSpan $"{s}1"),
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
            Overloads.Parse(
                (toSpan $"1{s}"),
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
            Overloads.Parse(
                (toSpan $"{s}1"),
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                (toSpan $"1{s}"),
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                (toSpan $"{s}1{s}"),
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                Overloads.Parse(
                    (toSpan $"{culture.NumberFormat.PositiveSign}1"),
                    culture
                )
            )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        for culture in cultures do
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () -> 
                        Overloads.Parse(
                            (toSpan $"{culture.NumberFormat.NegativeSign}1"),
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        for culture in cultures do
            Assert.Equal(
                Natural.Zero,
                Overloads.Parse(
                    (toSpan $"{culture.NumberFormat.NegativeSign}0"),
                    culture
                )
            )

    [<Fact>]
    member public this.DisallowBothLeading () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
                            (toSpan $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0"),
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
                            (toSpan $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0"),
                            culture
                        ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan $"1{culture.NumberFormat.PositiveSign}"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan $"1{culture.NumberFormat.NegativeSign}"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan $"0{culture.NumberFormat.NegativeSign}"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowParentheses () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan "(1)"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan "(0)"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan $"1{culture.NumberFormat.NumberDecimalSeparator}0"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan $"1{culture.NumberFormat.NumberDecimalSeparator}0"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.AllowGroupSeparator () =
        for culture in cultures do
            Assert.Equal(
                Natural( 1234u ),
                Overloads.Parse( (toSpan $"1{culture.NumberFormat.NumberGroupSeparator}234"), culture )
            )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan $"10{exp}1"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan $"{culture.NumberFormat.CurrencySymbol}1"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan $"1{currentCulture.CurrencySymbol}"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.DisallowHex () =
        for culture in cultures do
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () -> Overloads.Parse( (toSpan "1A"), culture ) |> ignore
                )
            ) |> ignore

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        for culture in cultures do
            Assert.Equal(
                Natural( 11u ),
                Overloads.Parse( (toSpan "11"), culture )
            )

type public ParseUtf8StyleFormat() =
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

    let toSpan (s:string) : ReadOnlySpan<byte> =
        ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes(s) )

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        Assert.Equal(
            Natural([n]),
            Overloads.Parse(
                (toSpan s),
                System.Globalization.NumberStyles.Integer,
                currentCulture
            )
        )
    
    [<Fact>]
    member public this.BiggerSanity () =
        Assert.Equal(
            medium,
            Overloads.Parse(
                (toSpan mediumStr),
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
            Overloads.Parse(
                (toSpan $"{s}1"),
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
            Overloads.Parse(
                (toSpan $"1{s}"),
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
            Overloads.Parse(
                (toSpan $"{s}1"),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                (toSpan $"1{s}"),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                (toSpan $"{s}1{s}"),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                Overloads.Parse(
                    (toSpan $"{culture.NumberFormat.PositiveSign}1"),
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
                        Overloads.Parse(
                            (toSpan $"{culture.NumberFormat.NegativeSign}1"),
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
                Overloads.Parse(
                    (toSpan $"{culture.NumberFormat.NegativeSign}0"),
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
                        Overloads.Parse(
                            (toSpan $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0"),
                            System.Globalization.NumberStyles.AllowLeadingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
                            (toSpan $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0"),
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
                Overloads.Parse(
                    (toSpan $"1{culture.NumberFormat.PositiveSign}"),
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
                        Overloads.Parse(
                            (toSpan $"1{culture.NumberFormat.NegativeSign}"),
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
                Overloads.Parse(
                    (toSpan $"0{culture.NumberFormat.NegativeSign}"),
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
                        Overloads.Parse(
                            (toSpan $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}"),
                            System.Globalization.NumberStyles.AllowTrailingSign,
                            culture
                        ) |> ignore
               )
            ) |> ignore
            Assert.IsType<System.FormatException>(
                Record.Exception(
                    fun () ->
                        Overloads.Parse(
                            (toSpan $"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}"),
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
                        Overloads.Parse(
                            (toSpan "(1)"),
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
                Overloads.Parse(
                    (toSpan "(0)"),
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
                        Overloads.Parse(
                            (toSpan "(0"),
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
                        Overloads.Parse(
                            (toSpan "0)"),
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
                        Overloads.Parse(
                            (toSpan ")0("),
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
                        Overloads.Parse(
                            (toSpan "((0))"),
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
                    Overloads.Parse(
                        (toSpan $"1{currentCulture.NumberDecimalSeparator}1"),
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
                Overloads.Parse(
                    (toSpan $"1{culture.NumberFormat.NumberDecimalSeparator}0"),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture
                )
            )

    [<Fact>]
    member public this.DecimalMoreThanOne () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        (toSpan $"1{currentCulture.NumberDecimalSeparator}0{currentCulture.NumberDecimalSeparator}0"),
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
                Overloads.Parse(
                    (toSpan $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890"),
                    System.Globalization.NumberStyles.AllowThousands,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowExponent () =
        Assert.Equal(
            Natural( 10u ),
            Overloads.Parse(
                (toSpan "1e1"),
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            Overloads.Parse(
                (toSpan "2E2"),
                System.Globalization.NumberStyles.AllowExponent,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        Assert.Equal(
            Natural( 12u ),
            Overloads.Parse(
                (toSpan "1.2e1"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 201u ),
            Overloads.Parse(
                (toSpan "2.01E2"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowNegativeExponentWithDecimal () =
        // Yes, these look silly, but they are technically valid
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                (toSpan "10.0e-1"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            Overloads.Parse(
                (toSpan "200.0E-2"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        Assert.Equal(
            Natural( 10u ),
            Overloads.Parse(
                (toSpan "1e+1"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 200u ),
            Overloads.Parse(
                (toSpan "2E+2"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        Assert.Equal(
            Natural.Unit,
            Overloads.Parse(
                (toSpan "10e-1"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

        Assert.Equal(
            Natural( 2u ),
            Overloads.Parse(
                (toSpan "200E-2"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                currentCulture
            )
        )

    [<Fact>]
    member public this.ExponentWithNegativeSignOverflow () =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        (toSpan "1e-1"),
                        System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowLeadingSign,
                        currentCulture
                    ) |> ignore
            )
        ) |> ignore

        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () ->
                    Overloads.Parse(
                        (toSpan "2E-2"),
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
                Overloads.Parse(
                    (toSpan $"{culture.NumberFormat.CurrencySymbol}1"),
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture
                )
            )

    [<Fact>]
    member public this.AllowCurrencySymbolPostfix () =
        for culture in cultures do
            Assert.Equal(
                Natural.Unit,
                Overloads.Parse(
                    (toSpan $"1{culture.NumberFormat.CurrencySymbol}"),
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
            Overloads.Parse(
                (toSpan "1234567890ABCDEF"),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

        Assert.Equal(
            value,
            Overloads.Parse(
                (toSpan "1234567890abcdef"),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                currentCulture
            )
        )

    [<Fact>]
    member public this.AllowBinary () =
        Assert.Equal(
            Natural( 172u ),
            Overloads.Parse(
                (toSpan "10101100"),
                System.Globalization.NumberStyles.AllowBinarySpecifier,
                currentCulture
            )
        )
