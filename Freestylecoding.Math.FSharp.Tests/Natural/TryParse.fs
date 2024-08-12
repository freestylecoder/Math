namespace Natural

open System
open Xunit
open Freestylecoding.Math

type public Consts() =
    // This is a default vaule used throughout this class.
    // It gives us a value to initialize the byref vault to
    // We can't use 0, because 0 is returned on a "fail"
    static member DeadBeef = Natural( [0xDeadBeeful] )

    static member Small = Natural( [0x4996_02D2u] )
    static member SmallStr = "1234567890"

    static member Medium = Natural( [0xAB54_A98Cu; 0xEB1F_0AD2u] )
    static member MediumStr = "12345678901234567890"

    static member Large  = Natural( [0x0000_0001u; 0x8EE9_0FF6u; 0xC373_E0EEu; 0x4E3F_0AD2u] )
    static member LargeStr = "123456789012345678901234567890"

    static member usCulture = System.Globalization.CultureInfo( "en-US" ) // US
    static member ukCulture = System.Globalization.CultureInfo( "en-GB" ) // UK
    static member frCulture = System.Globalization.CultureInfo( "fr-FR" ) // France
    static member luCulture = System.Globalization.CultureInfo( "fr-LU" ) // Luxembourg
    static member Cultures = [ Consts.usCulture; Consts.ukCulture; Consts.frCulture; Consts.luCulture ]

    static member CurrentCulture = System.Globalization.CultureInfo.CurrentCulture.NumberFormat

    static member Whitespace = [|
            [| " " |];
            [| "  " |];
            [| "\t" |];
            [| "\t " |];
            [| "\t\t" |];
            [| "\n" |];
            [| "\r" |];
            [| "\r\n" |];
            [| "\n\r" |];
        |]

type public Overload() =
    // From Natural
    static member TryParse( s:string, result:byref<Natural> ) : bool =
        Natural.TryParse( s, &result )
    static member TryParse( charSpan:System.ReadOnlySpan<char>, result:byref<Natural> ) : bool =
        Natural.TryParse( charSpan, &result )
    static member TryParse( byteSpan:System.ReadOnlySpan<byte>, result:byref<Natural> ) : bool =
        Natural.TryParse( byteSpan, &result )

    // From IParsable<Natural>
    static member private TryParse<'T when 'T :> System.IParsable<'T>>( s:string, provider:System.IFormatProvider, result:byref<'T> ) : bool =
        'T.TryParse( s, provider, &result )
    static member TryParse( s:string, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
        Overload.TryParse<Natural>( s, provider, &result )

    // From ISpanParsable<Natural>
    static member private TryParse<'T when 'T :> System.ISpanParsable<'T>>( charSpan:System.ReadOnlySpan<char>, provider:System.IFormatProvider, result:byref<'T> ) : bool =
        'T.TryParse( charSpan, provider, &result )
    static member TryParse( charSpan:System.ReadOnlySpan<char>, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
        Overload.TryParse<Natural>( charSpan, provider, &result )

    // From IUtf8SpanParsable<Natural>
    static member private TryParse<'T when 'T :> System.IUtf8SpanParsable<'T>>( byteSpan:System.ReadOnlySpan<byte>, provider:System.IFormatProvider, result:byref<'T> ) : bool =
        'T.TryParse( byteSpan, provider, &result )
    static member TryParse( byteSpan:System.ReadOnlySpan<byte>, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
        Overload.TryParse<Natural>( byteSpan, provider, &result )

    // From INumberBase<Natural>
    static member private TryParse<'T when 'T :> System.Numerics.INumberBase<'T>>( s:string, style:System.Globalization.NumberStyles, provider:System.IFormatProvider, result:byref<'T> ) : bool =
        'T.TryParse( s, style, provider, &result )
    static member private TryParse<'T when 'T :> System.Numerics.INumberBase<'T>>( charSpan:System.ReadOnlySpan<char>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider, result:byref<'T> ) : bool =
        'T.TryParse( charSpan, style, provider, &result )
    static member private TryParse<'T when 'T :> System.Numerics.INumberBase<'T>>( byteSpan:System.ReadOnlySpan<byte>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider, result:byref<'T> ) : bool =
        'T.TryParse( byteSpan, style, provider, &result )
    static member TryParse( s:string, style:System.Globalization.NumberStyles, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
        Overload.TryParse<Natural>( s, style, provider, &result )
    static member TryParse( charSpan:System.ReadOnlySpan<char>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
        Overload.TryParse<Natural>( charSpan, style, provider, &result )
    static member TryParse( byteSpan:System.ReadOnlySpan<byte>, style:System.Globalization.NumberStyles, provider:System.IFormatProvider, result:byref<Natural> ) : bool =
        Overload.TryParse<Natural>( byteSpan, style, provider, &result )

type public TryParseString() =
    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse(s, &result) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.SmallStr, &result) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.MediumStr, &result) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.LargeStr, &result) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{s}1", &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"1{s}", &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{s}1", &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse( $"1{s}", &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{s}1{s}", &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{Consts.CurrentCulture.PositiveSign}1", &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"{Consts.CurrentCulture.NegativeSign}1", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{Consts.CurrentCulture.NegativeSign}0", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        // I would like for this to be a theory with the two cases separate
        // However, I can't do string interpolation in an attribute
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"{Consts.CurrentCulture.PositiveSign}{Consts.CurrentCulture.NegativeSign}0", &result) )
        Assert.Equal( Natural.Zero, result )

        result <- Consts.DeadBeef
        Assert.False( Overload.TryParse( $"{Consts.CurrentCulture.NegativeSign}{Consts.CurrentCulture.PositiveSign}0", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.PositiveSign}", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.NegativeSign}", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"0{Consts.CurrentCulture.NegativeSign}", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParentheses () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( "(1)", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( "(0)", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.NumberDecimalSeparator}1", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.NumberDecimalSeparator}0", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"1{Consts.CurrentCulture.NumberGroupSeparator}234", &result) )
        Assert.Equal( Natural( 1234u ), result )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"10{exp}1", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"{Consts.CurrentCulture.CurrencySymbol}1", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.CurrencySymbol}", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowHex () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( "1A", &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( "11", &result) )
        Assert.Equal( Natural( 11u ), result )

type public TryParseStringFormat() =
    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse(s, Consts.CurrentCulture, &result) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.SmallStr, Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.MediumStr, Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.LargeStr, Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse($"{s}1", Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse($"1{s}", Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse($"{s}1", Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse($"1{s}", Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse($"{s}1{s}", Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse($"{culture.NumberFormat.PositiveSign}1", culture, &result) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"{culture.NumberFormat.NegativeSign}1", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse($"{culture.NumberFormat.NegativeSign}0", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0", culture, &result) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False( Overload.TryParse($"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"1{culture.NumberFormat.PositiveSign}", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"1{culture.NumberFormat.NegativeSign}", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"0{culture.NumberFormat.NegativeSign}", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParentheses () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse( "(1)", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse( "(0)", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse( $"1{culture.NumberFormat.NumberDecimalSeparator}1", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse( $"1{culture.NumberFormat.NumberDecimalSeparator}0", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse( $"1{culture.NumberFormat.NumberGroupSeparator}234", culture, &result) )
            Assert.Equal( Natural( 1234u ), result )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse( $"10{exp}1", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse( $"{culture.NumberFormat.CurrencySymbol}1", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse( $"1{culture.NumberFormat.CurrencySymbol}", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowHex () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse( "1A", culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse( "11", culture, &result) )
            Assert.Equal( Natural( 11u ), result )

type public TryParseStringStyleFormat() =
    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                s,
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                Consts.SmallStr,
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                Consts.MediumStr,
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                Consts.LargeStr,
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"{s}1",
                System.Globalization.NumberStyles.AllowLeadingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"1{s}",
                System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"{s}1",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"1{s}",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"{s}1{s}",
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"{culture.NumberFormat.PositiveSign}1",
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"{culture.NumberFormat.NegativeSign}1",
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"{culture.NumberFormat.NegativeSign}0",
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0",
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0",
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowTrailingPositive () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"1{culture.NumberFormat.PositiveSign}",
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.TrailingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NegativeSign}",
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowTrailingNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"0{culture.NumberFormat.NegativeSign}",
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothTrailing () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}",
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}",
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    "(1)",
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowParenthesesNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    "(0)",
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOnlyOpen () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    "(0",
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOnlyClose () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    "0)",
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOutOfOrder () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    ")0(",
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesMoreThanOne () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    "((0))",
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DecimalPointOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NumberDecimalSeparator}1",
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowDecimalPointZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NumberDecimalSeparator}0",
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.DecimalMoreThanOne () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NumberDecimalSeparator}0{culture.NumberFormat.NumberDecimalSeparator}0",
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890",
                    System.Globalization.NumberStyles.AllowThousands,
                    culture,
                    &result
                ) )
            Assert.Equal( Consts.Small, result )

    [<Fact>]
    member public this.AllowExponent () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1e1",
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 10u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "2E2",
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 200u ), result )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1.2e1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 12u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "2.01E2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 201u ), result )

    [<Fact>]
    member public this.AllowNegativeExponentWithDecimal () =
        // Yes, these look silly, but they are technically valid
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "10.0e-1",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "200.0E-2",
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 2u ), result )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1e+1",
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 10u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "2E+2",
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 200u ), result )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "10e-1",
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "200E-2",
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 2u ), result )

    [<Fact>]
    member public this.ExponentWithNegativeSignOverflow () =
        let mutable result = Consts.DeadBeef
        Assert.False(
            Overload.TryParse(
                "1e-1",
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Zero, result )

        result <- Consts.DeadBeef
        Assert.False(
            Overload.TryParse(
                "2E-2",
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowCurrencySymbolPrefix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"{culture.NumberFormat.CurrencySymbol}1",
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowCurrencySymbolPostfix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"1{culture.NumberFormat.CurrencySymbol}",
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowHex () =
        // If you're curious, that's equal to "1,311,768,467,294,899,695"
        // It was verified with the Windows Calculator app
        let value = Natural( [ 305419896u; 2427178479u ] )

        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1234567890ABCDEF",
                System.Globalization.NumberStyles.AllowHexSpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( value, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1234567890ABCDEF",
                System.Globalization.NumberStyles.AllowHexSpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( value, result )

    [<Fact>]
    member public this.AllowBinary () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "10101100",
                System.Globalization.NumberStyles.AllowBinarySpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 172u ), result )

type public TryParseSpan() =
    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse(s.AsSpan(), &result) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.SmallStr.AsSpan(), &result) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.MediumStr.AsSpan(), &result) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.LargeStr.AsSpan(), &result) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{s}1".AsSpan(), &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"1{s}".AsSpan(), &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{s}1".AsSpan(), &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse( $"1{s}".AsSpan(), &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{s}1{s}".AsSpan(), &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{Consts.CurrentCulture.PositiveSign}1".AsSpan(), &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"{Consts.CurrentCulture.NegativeSign}1".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"{Consts.CurrentCulture.NegativeSign}0".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        // I would like for this to be a theory with the two cases separate
        // However, I can't do string interpolation in an attribute
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"{Consts.CurrentCulture.PositiveSign}{Consts.CurrentCulture.NegativeSign}0".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

        result <- Consts.DeadBeef
        Assert.False( Overload.TryParse( $"{Consts.CurrentCulture.NegativeSign}{Consts.CurrentCulture.PositiveSign}0".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.PositiveSign}".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.NegativeSign}".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"0{Consts.CurrentCulture.NegativeSign}".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParentheses () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( "(1)".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( "(0)".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.NumberDecimalSeparator}1".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.NumberDecimalSeparator}0".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( $"1{Consts.CurrentCulture.NumberGroupSeparator}234".AsSpan(), &result) )
        Assert.Equal( Natural( 1234u ), result )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"10{exp}1".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"{Consts.CurrentCulture.CurrencySymbol}1".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( $"1{Consts.CurrentCulture.CurrencySymbol}".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowHex () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( "1A".AsSpan(), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( "11".AsSpan(), &result) )
        Assert.Equal( Natural( 11u ), result )

type public TryParseSpanFormat() =
    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse(s.AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.SmallStr.AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.MediumStr.AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse(Consts.LargeStr.AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse($"{s}1".AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse($"1{s}".AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse($"{s}1".AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse($"1{s}".AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse($"{s}1{s}".AsSpan(), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse($"{culture.NumberFormat.PositiveSign}1".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"{culture.NumberFormat.NegativeSign}1".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse($"{culture.NumberFormat.NegativeSign}0".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False( Overload.TryParse($"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"1{culture.NumberFormat.PositiveSign}".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"1{culture.NumberFormat.NegativeSign}".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"0{culture.NumberFormat.NegativeSign}".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParentheses () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse("(1)".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse("(0)".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"1{culture.NumberFormat.NumberDecimalSeparator}1".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse($"1{culture.NumberFormat.NumberGroupSeparator}234".AsSpan(), culture, &result) )
            Assert.Equal( Natural( 1234u ), result )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"10{exp}1".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"{culture.NumberFormat.CurrencySymbol}1".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse($"1{culture.NumberFormat.CurrencySymbol}".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowHex () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse("1A".AsSpan(), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse("11".AsSpan(), culture, &result) )
            Assert.Equal( Natural( 11u ), result )

type public TryParseSpanStyleFormat() =
    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                s.AsSpan(),
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                Consts.SmallStr.AsSpan(),
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                Consts.MediumStr.AsSpan(),
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                Consts.LargeStr.AsSpan(),
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"{s}1".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"1{s}".AsSpan(),
                System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"{s}1".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"1{s}".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                $"{s}1{s}".AsSpan(),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"{culture.NumberFormat.PositiveSign}1".AsSpan(),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"{culture.NumberFormat.NegativeSign}1".AsSpan(),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"{culture.NumberFormat.NegativeSign}0".AsSpan(),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0".AsSpan(),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0".AsSpan(),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowTrailingPositive () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"1{culture.NumberFormat.PositiveSign}".AsSpan(),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.TrailingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NegativeSign}".AsSpan(),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowTrailingNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"0{culture.NumberFormat.NegativeSign}".AsSpan(),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothTrailing () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}".AsSpan(),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}".AsSpan(),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    "(1)".AsSpan(),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowParenthesesNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    "(0)".AsSpan(),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOnlyOpen () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    "(0".AsSpan(),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOnlyClose () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    "0)".AsSpan(),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOutOfOrder () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    ")0(".AsSpan(),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesMoreThanOne () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    "((0))".AsSpan(),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DecimalPointOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NumberDecimalSeparator}1".AsSpan(),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowDecimalPointZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.DecimalMoreThanOne () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NumberDecimalSeparator}0{culture.NumberFormat.NumberDecimalSeparator}0".AsSpan(),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890".AsSpan(),
                    System.Globalization.NumberStyles.AllowThousands,
                    culture,
                    &result
                ) )
            Assert.Equal( Consts.Small, result )

    [<Fact>]
    member public this.AllowExponent () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1e1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 10u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "2E2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 200u ), result )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1.2e1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 12u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "2.01E2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 201u ), result )

    [<Fact>]
    member public this.AllowNegativeExponentWithDecimal () =
        // Yes, these look silly, but they are technically valid
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "10.0e-1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "200.0E-2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 2u ), result )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1e+1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 10u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "2E+2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 200u ), result )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "10e-1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "200E-2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 2u ), result )

    [<Fact>]
    member public this.ExponentWithNegativeSignOverflow () =
        let mutable result = Consts.DeadBeef
        Assert.False(
            Overload.TryParse(
                "1e-1".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Zero, result )

        result <- Consts.DeadBeef
        Assert.False(
            Overload.TryParse(
                "2E-2".AsSpan(),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowCurrencySymbolPrefix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"{culture.NumberFormat.CurrencySymbol}1".AsSpan(),
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowCurrencySymbolPostfix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    $"1{culture.NumberFormat.CurrencySymbol}".AsSpan(),
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowHex () =
        // If you're curious, that's equal to "1,311,768,467,294,899,695"
        // It was verified with the Windows Calculator app
        let value = Natural( [ 305419896u; 2427178479u ] )

        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1234567890ABCDEF".AsSpan(),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( value, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "1234567890ABCDEF".AsSpan(),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( value, result )

    [<Fact>]
    member public this.AllowBinary () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                "10101100".AsSpan(),
                System.Globalization.NumberStyles.AllowBinarySpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 172u ), result )

type public TryParseUtf8() =
    let toSpan (s:string) : ReadOnlySpan<byte> =
        ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes(s) )

    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan s), &result) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan Consts.SmallStr), &result) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan Consts.MediumStr), &result) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan Consts.LargeStr), &result) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan $"{s}1"), &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan $"1{s}"), &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan $"{s}1"), &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan $"1{s}"), &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan $"{s}1{s}"), &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan $"{Consts.CurrentCulture.PositiveSign}1"), &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"{Consts.CurrentCulture.NegativeSign}1"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan $"{Consts.CurrentCulture.NegativeSign}0"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        // I would like for this to be a theory with the two cases separate
        // However, I can't do string interpolation in an attribute
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"{Consts.CurrentCulture.PositiveSign}{Consts.CurrentCulture.NegativeSign}0"), &result) )
        Assert.Equal( Natural.Zero, result )

        result <- Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"{Consts.CurrentCulture.NegativeSign}{Consts.CurrentCulture.PositiveSign}0"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"1{Consts.CurrentCulture.PositiveSign}"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"1{Consts.CurrentCulture.NegativeSign}"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"0{Consts.CurrentCulture.NegativeSign}"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParentheses () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan "(1)"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan "(0)"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"1{Consts.CurrentCulture.NumberDecimalSeparator}1"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"1{Consts.CurrentCulture.NumberDecimalSeparator}0"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan $"1{Consts.CurrentCulture.NumberGroupSeparator}234"), &result) )
        Assert.Equal( Natural( 1234u ), result )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"10{exp}1"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"{Consts.CurrentCulture.CurrencySymbol}1"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan $"1{Consts.CurrentCulture.CurrencySymbol}"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowHex () =
        let mutable result = Consts.DeadBeef
        Assert.False( Overload.TryParse( (toSpan "1A"), &result) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse( (toSpan "11"), &result) )
        Assert.Equal( Natural( 11u ), result )

type public TryParseUtf8Format() =
    let toSpan (s:string) : ReadOnlySpan<byte> =
        ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes(s) )

    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan s), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan Consts.SmallStr), Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan Consts.MediumStr), Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan Consts.LargeStr), Consts.CurrentCulture, &result) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan $"{s}1"), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan $"1{s}"), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan $"{s}1"), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan $"1{s}"), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True( Overload.TryParse((toSpan $"{s}1{s}"), Consts.CurrentCulture, &result) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse((toSpan $"{culture.NumberFormat.PositiveSign}1"), culture, &result) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"{culture.NumberFormat.NegativeSign}1"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse((toSpan $"{culture.NumberFormat.NegativeSign}0"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingPositive () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"1{culture.NumberFormat.PositiveSign}"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegative () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"1{culture.NumberFormat.NegativeSign}"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowTrailingNegativeZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"0{culture.NumberFormat.NegativeSign}"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParentheses () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan "(1)"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowParenthesesZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan "(0)"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPoint () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"1{culture.NumberFormat.NumberDecimalSeparator}1"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowDecimalPointZero () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"1{culture.NumberFormat.NumberDecimalSeparator}0"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse((toSpan $"1{culture.NumberFormat.NumberGroupSeparator}234"), culture, &result) )
            Assert.Equal( Natural( 1234u ), result )

    [<Theory>]
    [<InlineData( "e" )>]
    [<InlineData( "E" )>]
    [<InlineData( "e-" )>]
    [<InlineData( "E-" )>]
    member public this.DisallowExponent (exp:string) =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"10{exp}1"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPrefix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"{culture.NumberFormat.CurrencySymbol}1"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowCurrencySymbolPostfix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan $"1{culture.NumberFormat.CurrencySymbol}"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowHex () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.False( Overload.TryParse((toSpan "1A"), culture, &result) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ReadBinaryAsDecimal () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True( Overload.TryParse((toSpan "11"), culture, &result) )
            Assert.Equal( Natural( 11u ), result )

 type public TryParseUtf8StyleFormat() =
    let toSpan (s:string) : ReadOnlySpan<byte> =
        ReadOnlySpan<byte>( System.Text.Encoding.UTF8.GetBytes(s) )

    static member Whitespace = Consts.Whitespace

    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity n (s:string) =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan s),
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural([n]), result )
    
    [<Fact>]
    member public this.BiggerSanity () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan Consts.SmallStr),
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Small, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan Consts.MediumStr),
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Medium, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan Consts.LargeStr),
                System.Globalization.NumberStyles.Integer,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Consts.Large, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowLeadingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan $"{s}1"),
                System.Globalization.NumberStyles.AllowLeadingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowTrailingWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan $"1{s}"),
                System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Theory>]
    [<MemberData( "Whitespace" )>]
    member public this.AllowWhiteSpace s =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan $"{s}1"),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan $"1{s}"),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan $"{s}1{s}"),
                System.Globalization.NumberStyles.AllowLeadingWhite ||| System.Globalization.NumberStyles.AllowTrailingWhite,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowLeadingPositive () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan $"{culture.NumberFormat.PositiveSign}1"),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.LeadingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan $"{culture.NumberFormat.NegativeSign}1"),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowLeadingNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan $"{culture.NumberFormat.NegativeSign}0"),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothLeading () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan $"{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}0"),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan $"{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}0"),
                    System.Globalization.NumberStyles.AllowLeadingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowTrailingPositive () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan $"1{culture.NumberFormat.PositiveSign}"),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.TrailingNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan $"1{culture.NumberFormat.NegativeSign}"),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowTrailingNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan $"0{culture.NumberFormat.NegativeSign}"),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DisallowBothTrailing () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan $"0{culture.NumberFormat.PositiveSign}{culture.NumberFormat.NegativeSign}"),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan $"0{culture.NumberFormat.NegativeSign}{culture.NumberFormat.PositiveSign}"),
                    System.Globalization.NumberStyles.AllowTrailingSign,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesNegativeOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan "(1)"),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowParenthesesNegativeZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan "(0)"),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOnlyOpen () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan "(0"),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOnlyClose () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan "0)"),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesOutOfOrder () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan ")0("),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.ParenthesesMoreThanOne () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan "((0))"),
                    System.Globalization.NumberStyles.AllowParentheses,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.DecimalPointOverflow () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan $"1{culture.NumberFormat.NumberDecimalSeparator}1"),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowDecimalPointZero () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan $"1{culture.NumberFormat.NumberDecimalSeparator}0"),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.DecimalMoreThanOne () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.False(
                Overload.TryParse(
                    (toSpan $"1{culture.NumberFormat.NumberDecimalSeparator}0{culture.NumberFormat.NumberDecimalSeparator}0"),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowGroupSeparator () =
        let mutable result = Consts.DeadBeef
        for culture in Consts.Cultures do
            result <- Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan $"1{culture.NumberFormat.NumberGroupSeparator}234{culture.NumberFormat.NumberGroupSeparator}567{culture.NumberFormat.NumberGroupSeparator}890"),
                    System.Globalization.NumberStyles.AllowThousands,
                    culture,
                    &result
                ) )
            Assert.Equal( Consts.Small, result )

    [<Fact>]
    member public this.AllowExponent () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "1e1"),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 10u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "2E2"),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 200u ), result )

    [<Fact>]
    member public this.AllowExponentWithDecimal () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "1.2e1"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 12u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "2.01E2"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 201u ), result )

    [<Fact>]
    member public this.AllowNegativeExponentWithDecimal () =
        // Yes, these look silly, but they are technically valid
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "10.0e-1"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "200.0E-2"),
                System.Globalization.NumberStyles.AllowExponent ||| System.Globalization.NumberStyles.AllowDecimalPoint,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 2u ), result )

    [<Fact>]
    member public this.AllowExponentWithPositiveSign () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "1e+1"),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 10u ), result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "2E+2"),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 200u ), result )

    [<Fact>]
    member public this.AllowExponentWithNegativeSign () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "10e-1"),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Unit, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "200E-2"),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 2u ), result )

    [<Fact>]
    member public this.ExponentWithNegativeSignOverflow () =
        let mutable result = Consts.DeadBeef
        Assert.False(
            Overload.TryParse(
                (toSpan "1e-1"),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Zero, result )

        result <- Consts.DeadBeef
        Assert.False(
            Overload.TryParse(
                (toSpan "2E-2"),
                System.Globalization.NumberStyles.AllowExponent,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural.Zero, result )

    [<Fact>]
    member public this.AllowCurrencySymbolPrefix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan $"{culture.NumberFormat.CurrencySymbol}1"),
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowCurrencySymbolPostfix () =
        for culture in Consts.Cultures do
            let mutable result = Consts.DeadBeef
            Assert.True(
                Overload.TryParse(
                    (toSpan $"1{culture.NumberFormat.CurrencySymbol}"),
                    System.Globalization.NumberStyles.AllowCurrencySymbol,
                    culture,
                    &result
                ) )
            Assert.Equal( Natural.Unit, result )

    [<Fact>]
    member public this.AllowHex () =
        // If you're curious, that's equal to "1,311,768,467,294,899,695"
        // It was verified with the Windows Calculator app
        let value = Natural( [ 305419896u; 2427178479u ] )

        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "1234567890ABCDEF"),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( value, result )

        result <- Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "1234567890ABCDEF"),
                System.Globalization.NumberStyles.AllowHexSpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( value, result )

    [<Fact>]
    member public this.AllowBinary () =
        let mutable result = Consts.DeadBeef
        Assert.True(
            Overload.TryParse(
                (toSpan "10101100"),
                System.Globalization.NumberStyles.AllowBinarySpecifier,
                Consts.CurrentCulture,
                &result
            ) )
        Assert.Equal( Natural( 172u ), result )