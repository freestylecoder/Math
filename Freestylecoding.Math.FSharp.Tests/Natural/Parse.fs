namespace Natural

open Xunit
open Freestylecoding.Math

type public Parse() =
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
    member public this.DisallowGroupSeparator () =
        Assert.IsType<System.FormatException>(
            Record.Exception(
                fun () -> Natural.Parse( $"1{currentCulture.NumberGroupSeparator}234" ) |> ignore
            )
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

    // The following tests are for INumberBase<Natural>.Parse( string, NumberStyles, IFormatProvider )
    // Every other version of parse, except the base Natural.Parse, eventually hit here
    // This version, except in a few cases, cleans up the string before passing to the base
    // It only natively handles Binary and Hex strings
