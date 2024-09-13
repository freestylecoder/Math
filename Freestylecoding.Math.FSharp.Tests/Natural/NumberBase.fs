namespace Natural

// This is to stop warnings on direct interface calls
#nowarn "3536"

open System
open Xunit
open Freestylecoding.Math
(* type public Consts() =
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
*)

// This type allows the tests to have direct access to the INumberBase tings
type public NBDirect() =
    // Properties
    static member private OneInternal<'T when 'T :> Numerics.INumberBase<'T>>() : 'T = 'T.One
    static member One with get () = NBDirect.OneInternal<Natural>()

    static member private RadixInternal<'T when 'T :> Numerics.INumberBase<'T>>() : int = 'T.Radix
    static member Radix with get () = NBDirect.RadixInternal<Natural>()

    static member private ZeroInternal<'T when 'T :> Numerics.INumberBase<'T>>() : 'T = 'T.Zero
    static member Zero with get () = NBDirect.ZeroInternal<Natural>()

    // Property Checkers
    static member private IsCanonicalInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsCanonical( value )
    static member IsCanonical( value:Natural ) = NBDirect.IsCanonicalInternal<Natural>( value )

    static member private IsComplexNumberInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsComplexNumber( value )
    static member IsComplexNumber( value:Natural ) = NBDirect.IsComplexNumberInternal<Natural>( value )

    static member private IsFiniteInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsFinite( value )
    static member IsFinite( value:Natural ) = NBDirect.IsFiniteInternal<Natural>( value )

    static member private IsImaginaryNumberInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsImaginaryNumber( value )
    static member IsImaginaryNumber( value:Natural ) = NBDirect.IsImaginaryNumberInternal<Natural>( value )

    static member private IsInfinityInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsInfinity( value )
    static member IsInfinity( value:Natural ) = NBDirect.IsInfinityInternal<Natural>( value )

    static member private IsIntegerInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsInteger( value )
    static member IsInteger( value:Natural ) = NBDirect.IsIntegerInternal<Natural>( value )

    static member private IsNaNInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsNaN( value )
    static member IsNaN( value:Natural ) = NBDirect.IsNaNInternal<Natural>( value )

    static member private IsNegativeInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsNegative( value )
    static member IsNegative( value:Natural ) = NBDirect.IsNegativeInternal<Natural>( value )

    static member private IsNegativeInfinityInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsNegativeInfinity( value )
    static member IsNegativeInfinity( value:Natural ) = NBDirect.IsNegativeInfinityInternal<Natural>( value )

    static member private IsNormalInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsNormal( value )
    static member IsNormal( value:Natural ) = NBDirect.IsNormalInternal<Natural>( value )

    static member private IsPositiveInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsPositive( value )
    static member IsPositive( value:Natural ) = NBDirect.IsPositiveInternal<Natural>( value )

    static member private IsPositiveInfinityInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsPositiveInfinity( value )
    static member IsPositiveInfinity( value:Natural ) = NBDirect.IsPositiveInfinityInternal<Natural>( value )

    static member private IsRealNumberInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsRealNumber( value )
    static member IsRealNumber( value:Natural ) = NBDirect.IsRealNumberInternal<Natural>( value )

    static member private IsSubnormalInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsSubnormal( value )
    static member IsSubnormal( value:Natural ) = NBDirect.IsSubnormalInternal<Natural>( value )

    // Unary operators/checkers
    static member private AbsInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : 'T = 'T.Abs( value )
    static member Abs( value:Natural ) = NBDirect.AbsInternal<Natural>( value )

    static member private IsEvenIntegerInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsEvenInteger( value )
    static member IsEvenInteger( value:Natural ) = NBDirect.IsEvenIntegerInternal<Natural>( value )

    static member private IsOddIntegerInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsOddInteger( value )
    static member IsOddInteger( value:Natural ) = NBDirect.IsOddIntegerInternal<Natural>( value )

    static member private IsZeroInternal<'T when 'T :> Numerics.INumberBase<'T>>( value:'T ) : bool = 'T.IsZero( value )
    static member IsZero( value:Natural ) = NBDirect.IsZeroInternal<Natural>( value )

    // Binary operators/checkers
    static member private MaxMagnitudeInternal<'T when 'T :> Numerics.INumberBase<'T>>( x:'T, y:'T ) : 'T = 'T.MaxMagnitude( x, y )
    static member MaxMagnitude( x:Natural, y:Natural ) = NBDirect.MaxMagnitudeInternal<Natural>( x, y )

    static member private MaxMagnitudeNumberInternal<'T when 'T :> Numerics.INumberBase<'T>>( x:'T, y:'T ) : 'T = 'T.MaxMagnitudeNumber( x, y )
    static member MaxMagnitudeNumber( x:Natural, y:Natural ) = NBDirect.MaxMagnitudeNumberInternal<Natural>( x, y )

    static member private MinMagnitudeInternal<'T when 'T :> Numerics.INumberBase<'T>>( x:'T, y:'T ) : 'T = 'T.MinMagnitude( x, y )
    static member MinMagnitude( x:Natural, y:Natural ) = NBDirect.MinMagnitudeInternal<Natural>( x, y )

    static member private MinMagnitudeNumberInternal<'T when 'T :> Numerics.INumberBase<'T>>( x:'T, y:'T ) : 'T = 'T.MinMagnitudeNumber( x, y )
    static member MinMagnitudeNumber( x:Natural, y:Natural ) = NBDirect.MinMagnitudeNumberInternal<Natural>( x, y )

    // Other stuff
    static member private CreateCheckedInternal<'T, 'TOther when 'T :> Numerics.INumberBase<'T> and 'TOther :> Numerics.INumberBase<'TOther>>( value:'TOther ) : 'T = 'T.CreateChecked( value )
    static member CreateChecked( value:'TOther when 'TOther :> Numerics.INumberBase<'TOther> ) = NBDirect.CreateCheckedInternal<Natural,'TOther>( value )

type public NumberBase() =
    // This is just an absurdly large value that will overflow anything that could overflow
    // If you're curious how I came up with it:
    //  2^10 is roughly equal to 10^30
    //  Double.MaxValue is between 1E308 and 2E308
    //  Take a number slightly bigger that 308 (I use 312)
    //  Divide by 3 (104)
    //  Multiply by 10 (1040)
    //  Double it, just ot be sure (2080)
    //  Smooth it to a power of 32, because I know how <<< works (2048)
    static member OverflowValue = Natural.Unit <<< 2_048

    // Properties
    [<Fact>] member public this.One() = Assert.Equal( Natural( 1UL ), NBDirect.One )
    [<Fact>] member public this.Radix() = Assert.Equal( 2, NBDirect.Radix )
    [<Fact>] member public this.Zero() = Assert.Equal( Natural( 0UL ), NBDirect.Zero )

    // These all return const values based on the nature of a Natural number
    [<Fact>] member public this.IsCanonical() = Assert.True( NBDirect.IsCanonical( Natural.Unit ) )
    [<Fact>] member public this.IsComplexNumber() = Assert.False( NBDirect.IsComplexNumber( Natural.Unit ) )
    [<Fact>] member public this.IsFinite() = Assert.True( NBDirect.IsFinite( Natural.Unit ) )
    [<Fact>] member public this.IsImaginaryNumber() = Assert.False( NBDirect.IsImaginaryNumber( Natural.Unit ) )
    [<Fact>] member public this.IsInfinity() = Assert.False( NBDirect.IsInfinity( Natural.Unit ) )
    [<Fact>] member public this.IsInteger() = Assert.True( NBDirect.IsInteger( Natural.Unit ) )
    [<Fact>] member public this.IsNaN() = Assert.False( NBDirect.IsNaN( Natural.Unit ) )
    [<Fact>] member public this.IsNegative() = Assert.False( NBDirect.IsNegative( Natural.Unit ) )
    [<Fact>] member public this.IsNegativeInfinity() = Assert.False( NBDirect.IsNegativeInfinity( Natural.Unit ) )
    [<Fact>] member public this.IsNormal() = Assert.True( NBDirect.IsNormal( Natural.Unit ) )
    [<Fact>] member public this.IsPositive() = Assert.True( NBDirect.IsPositive( Natural.Unit ) )
    [<Fact>] member public this.IsPositiveInfinity() = Assert.False( NBDirect.IsPositiveInfinity( Natural.Unit ) )
    [<Fact>] member public this.IsRealNumber() = Assert.True( NBDirect.IsRealNumber( Natural.Unit ) )
    [<Fact>] member public this.IsSubnormal() = Assert.False( NBDirect.IsSubnormal( Natural.Unit ) )

    [<Fact>] member public this.Abs() = Assert.Equal( Natural.Unit, NBDirect.Abs( Natural.Unit ) )
    [<Fact>] member public this.IsEven() = Assert.True( NBDirect.IsEvenInteger( Natural( 2UL ) ) )
    [<Fact>] member public this.IsEvenZero() = Assert.True( NBDirect.IsEvenInteger( Natural.Zero ) )
    [<Fact>] member public this.IsNotEven() = Assert.False( NBDirect.IsEvenInteger( Natural.Unit ) )
    [<Fact>] member public this.IsOdd() = Assert.True( NBDirect.IsOddInteger( Natural.Unit ) )
    [<Fact>] member public this.IsOddZero() = Assert.False( NBDirect.IsOddInteger( Natural.Zero ) )
    [<Fact>] member public this.IsNotOdd() = Assert.False( NBDirect.IsOddInteger( Natural( 2UL ) ) )
    [<Fact>] member public this.IsZero() = Assert.False( NBDirect.IsZero( Natural.Unit ) )
    [<Fact>] member public this.IsZeroZero() = Assert.True( NBDirect.IsZero( Natural.Zero ) )

    [<Theory>]
    [<InlineData( 1UL, 1UL, 1UL )>]
    [<InlineData( 0UL, 1UL, 1UL )>]
    [<InlineData( 1UL, 0UL, 1UL )>]
    [<InlineData( 0UL, 0UL, 0UL )>]
    member public this.MaxMagnitude( x:uint64, y:uint64, expected:uint64 ) =
        Assert.Equal( Natural( expected ), NBDirect.MaxMagnitude( Natural( x ), Natural( y ) ) )

    [<Theory>]
    [<InlineData( 1UL, 1UL, 1UL )>]
    [<InlineData( 0UL, 1UL, 1UL )>]
    [<InlineData( 1UL, 0UL, 1UL )>]
    [<InlineData( 0UL, 0UL, 0UL )>]
    member public this.MaxMagnitudeNumber( x:uint64, y:uint64, expected:uint64 ) =
        Assert.Equal( Natural( expected ), NBDirect.MaxMagnitudeNumber( Natural( x ), Natural( y ) ) )

    [<Theory>]
    [<InlineData( 1UL, 1UL, 1UL )>]
    [<InlineData( 0UL, 1UL, 0UL )>]
    [<InlineData( 1UL, 0UL, 0UL )>]
    [<InlineData( 0UL, 0UL, 0UL )>]
    member public this.MinMagnitude( x:uint64, y:uint64, expected:uint64 ) =
        Assert.Equal( Natural( expected ), NBDirect.MinMagnitude( Natural( x ), Natural( y ) ) )

    [<Theory>]
    [<InlineData( 1UL, 1UL, 1UL )>]
    [<InlineData( 0UL, 1UL, 0UL )>]
    [<InlineData( 1UL, 0UL, 0UL )>]
    [<InlineData( 0UL, 0UL, 0UL )>]
    member public this.MinMagnitudeNumber( x:uint64, y:uint64, expected:uint64 ) =
        Assert.Equal( Natural( expected ), NBDirect.MinMagnitudeNumber( Natural( x ), Natural( y ) ) )

(*
    This set of CreateChecked basically test the TryConvertFromX methods in Natural
*)

    [<Fact>]
    member public this.CreateChecked() =
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1uy ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1uy ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1us ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1u ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1UL ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( UInt128( 0UL, 1UL ) ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( Natural( 1u ) ) )

    [<Fact>]
    member public this.CreateCheckedNonNative() =
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1y ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1s ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1 ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1L ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( ( Int128( 0UL, 1UL ) ) ) )

        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1.0f ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1.0 ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1.0m ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( 1I ) )

        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( Numerics.Complex( 1, 0 ) ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( Numerics.BigInteger( 1 ) ) )

        // TODO: These are not INumberBase yet, so it's failing for that
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( ( Integer( 1 ) ) ) )
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( ( Rational( 1, 1u ) ) ) )
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateChecked( ( Real( 1, 0 ) ) ) )

    [<Fact>]
    member public this.CreateCheckedNonNativeNegative() =
        let AssertExc value = 
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () ->
                        Numerics.INumberBase<Natural>.CreateChecked( value )
                        |> ignore
                )
            )
            |> ignore

        AssertExc -1y
        AssertExc -1s
        AssertExc -1
        AssertExc -1L
        AssertExc ( -Int128( 0UL, 1UL ) )

        AssertExc -1.0f
        AssertExc -1.0
        AssertExc -1.0m
        AssertExc -1I

        AssertExc (Numerics.Complex( -1.0, 0 ))
        AssertExc (Numerics.BigInteger( -1 ))

        // TODO: These are not INumberBase yet, so it's failing for that
        //AssertExc ( Integer( -1 ) )
        //AssertExc ( Rational( -1, 1u ) )
        //AssertExc ( Real( -1, 0 ) )

    [<Fact>]
    member public this.CreateCheckedNonNativeDecimal() =
        let AssertExc value = 
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () ->
                         Numerics.INumberBase<Natural>.CreateChecked( value )
                         |> ignore
                )
            )
            |> ignore

        AssertExc 1.1f
        AssertExc 1.1
        AssertExc 1.1m
        AssertExc (Numerics.Complex( 1.1, 0 ))

        // TODO: These are not INumberBase yet, so it's failing for that
        //AssertExc ( Rational( 1, 2u ) )
        //AssertExc ( Real( 1, -1 ) )

    [<Fact>]
    member public this.CreateCheckedNonNativeWeird() =
        let AssertExc value = 
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () ->
                        Numerics.INumberBase<Natural>.CreateChecked( value ) |> ignore
                )
            )
            |> ignore

        AssertExc Double.PositiveInfinity
        AssertExc Double.NegativeInfinity
        AssertExc Double.NaN
        AssertExc (Numerics.Complex( 1, 1 ))
        AssertExc (Numerics.Complex( 0, 1 ))

    [<Fact>]
    member public this.CreateSaturating() =
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1uy ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1us ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1u ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1UL ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( ( UInt128( 0UL, 1UL ) ) ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( ( Natural( 1u ) ) ) )

    [<Fact>]
    member public this.CreateSaturatingNonNative() =
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1y ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1s ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1 ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1L ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( ( Int128( 0UL, 1UL ) ) ) )

        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1.0f ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1.0 ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1.0m ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( 1I ) )

        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( Numerics.Complex( 1, 0 ) ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( Numerics.BigInteger( 1 ) ) )

        // TODO: These are not INumberBase yet, so it's failing for that
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( ( Integer( 1 ) ) ) )
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( ( Rational( 1, 1u ) ) ) )
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateSaturating( ( Real( 1, 0 ) ) ) )

    [<Fact>]
    member public this.CreateSaturatingNonNativeNegative() =
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -1y ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -1s ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -1 ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -1L ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -Int128( 0UL, 1UL ) ) ) )

        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -1.0f ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -1.0 ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -1.0m ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( -1I ) ) )

        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( Numerics.Complex( -1.0, 0 ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( Numerics.BigInteger( -1 ) ) )

        // TODO: These are not INumberBase yet, so it's failing for that
        //AssertExc ( Integer( -1 ) )
        //AssertExc ( Rational( -1, 1u ) )
        //AssertExc ( Real( -1, 0 ) )

    [<Fact>]
    member public this.CreateSaturatingNonNativeDecimal() =
        let AssertExc value = 
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () ->
                        Numerics.INumberBase<Natural>.CreateSaturating( value ) |> ignore
                )
            )
            |> ignore

        AssertExc 1.1f
        AssertExc 1.1
        AssertExc 1.1m
        AssertExc (Numerics.Complex( 1.1, 0 ))

        // TODO: These are not INumberBase yet, so it's failing for that
        //AssertExc ( Rational( 1, 2u ) )
        //AssertExc ( Real( 1, -1 ) )

    [<Fact>]
    member public this.CreateSaturatingNonNativeNegativeDecimal() =
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( -1.1f ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( -1.1 ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( -1.1m ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( ( Numerics.Complex( -1.1, 0 ) ) ) )

        // TODO: These are not INumberBase yet, so it's failing for that
        //Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( Rational( -1, 2u ) ) )
        //Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( Real( -1, -1 ) ) )

    [<Fact>]
    member public this.CreateSaturatingNonNativeWeird() =
        let AssertExc value = 
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () ->
                        Numerics.INumberBase<Natural>.CreateSaturating( value ) |> ignore
                )
            )
            |> ignore

        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateSaturating( Double.NegativeInfinity ) )

        AssertExc Double.NaN
        AssertExc Double.PositiveInfinity
        AssertExc (Numerics.Complex( 1, 1 ))
        AssertExc (Numerics.Complex( 0, 1 ))

    [<Fact>]
    member public this.CreateTruncating() =
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1uy ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1us ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1u ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1UL ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( ( UInt128( 0UL, 1UL ) ) ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( ( Natural( 1u ) ) ) )

    [<Fact>]
    member public this.CreateTruncatingNonNative() =
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1y ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1s ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1 ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1L ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( ( Int128( 0UL, 1UL ) ) ) )

        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1.0f ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1.0 ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1.0m ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1I ) )

        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( Numerics.Complex( 1, 0 ) ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( Numerics.BigInteger( 1 ) ) )

        // TODO: These are not INumberBase yet, so it's failing for that
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( ( Integer( 1 ) ) ) )
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( ( Rational( 1, 1u ) ) ) )
        //Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( ( Real( 1, 0 ) ) ) )

    [<Fact>]
    member public this.CreateTruncatingNonNativeNegative() =
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -1y ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -1s ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -1 ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -1L ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -Int128( 0UL, 1UL ) ) ) )

        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -1.0f ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -1.0 ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -1.0m ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( -1I ) ) )

        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( Numerics.Complex( -1.0, 0 ) ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( Numerics.BigInteger( -1 ) ) )

        // TODO: These are not INumberBase yet, so it's failing for that
        //AssertExc ( Integer( -1 ) )
        //AssertExc ( Rational( -1, 1u ) )
        //AssertExc ( Real( -1, 0 ) )

    [<Fact>]
    member public this.CreateTruncatingNonNativeDecimal() =
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1.1f ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1.1 ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( 1.1m ) )
        Assert.Equal( Natural.Unit, Numerics.INumberBase<Natural>.CreateTruncating( Numerics.Complex( 1.1, 0 ) ) )

        // TODO: These are not INumberBase yet, so it's failing for that
        //Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( Rational( 3, 2u ) ) )
        //Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( Real( 1, -1 ) ) )

    [<Fact>]
    member public this.CreateTruncatingNonNativeNegativeDecimal() =
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( -1.1f ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( -1.1 ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( -1.1m ) )
        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( ( Numerics.Complex( -1.1, 0 ) ) ) )

        // TODO: These are not INumberBase yet, so it's failing for that
        //Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( Rational( -1, 2u ) ) )
        //Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( Real( -1, -1 ) ) )

    [<Fact>]
    member public this.CreateTruncatingNonNativeWeird() =
        let AssertExc value = 
            Assert.IsType<System.OverflowException>(
                Record.Exception(
                    fun () ->
                        Numerics.INumberBase<Natural>.CreateTruncating( value ) |> ignore
                )
            )
            |> ignore

        Assert.Equal( Natural.Zero, Numerics.INumberBase<Natural>.CreateTruncating( Double.NegativeInfinity ) )

        AssertExc Double.NaN
        AssertExc Double.PositiveInfinity
        AssertExc (Numerics.Complex( 1, 1 ))
        AssertExc (Numerics.Complex( 0, 1 ))

(*
    This set of CreateX basically test the TryConvertToX methods in Natural
*)

    static member private AssertCheckedEqual<'TOther when 'TOther :> Numerics.INumberBase<'TOther>>( value:Natural ) =
        Assert.Equal( 'TOther.One, 'TOther.CreateChecked( value ) )

    static member private AssertCheckedOverflow<'TOther when 'TOther :> Numerics.INumberBase<'TOther>>( value:Natural ) : unit =
        Assert.IsType<System.OverflowException>(
            Record.Exception(
                fun () -> 'TOther.CreateChecked( value ) |> ignore
            )
        ) |> ignore

    [<Fact>]
    member public this.CreateCheckedFromOther() =
        NumberBase.AssertCheckedEqual<Byte>   ( Natural.Unit )
        NumberBase.AssertCheckedEqual<UInt16> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<UInt32> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<UInt64> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<UInt128>( Natural.Unit )

        NumberBase.AssertCheckedEqual<SByte> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<Int16> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<Int32> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<Int64> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<Int128>( Natural.Unit )

        NumberBase.AssertCheckedEqual<Single> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<Double> ( Natural.Unit )
        NumberBase.AssertCheckedEqual<Decimal>( Natural.Unit )

        NumberBase.AssertCheckedEqual<Numerics.Complex>   ( Natural.Unit )
        NumberBase.AssertCheckedEqual<Numerics.BigInteger>( Natural.Unit )

        NumberBase.AssertCheckedEqual<Natural> ( Natural.Unit )
        //NumberBase.AssertCheckedEqual<Integer> ( Natural.Unit )
        //NumberBase.AssertCheckedEqual<Rational>( Natural.Unit )
        //NumberBase.AssertCheckedEqual<Real>    ( Natural.Unit )

    [<Fact>]
    member public this.CreateCheckedFromOtherOverflow() =
        NumberBase.AssertCheckedOverflow<Byte>   ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<UInt16> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<UInt32> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<UInt64> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<UInt128>( NumberBase.OverflowValue )

        NumberBase.AssertCheckedOverflow<SByte> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<Int16> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<Int32> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<Int64> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<Int128>( NumberBase.OverflowValue )

        NumberBase.AssertCheckedOverflow<Single> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<Double> ( NumberBase.OverflowValue )
        NumberBase.AssertCheckedOverflow<Decimal>( NumberBase.OverflowValue )

        NumberBase.AssertCheckedOverflow<Numerics.Complex>( NumberBase.OverflowValue )

        // All of these can't overflow
        //  1) Numerics.BigInteger
        //  2) Natural
        //  3) Integer
        //  4) Rational
        //  5) Real

        // To be fair, I'd _like_ to test that BigInteger took the big number
        // However:
        //  1) It's in the magnitude of 600 digits long
        //  2) I'd need to know the value to compute the BigInteger

    static member private AssertSaturatingEqual<'TOther when 'TOther :> Numerics.INumberBase<'TOther>>( value:Natural, expected:'TOther ) =
        Assert.Equal( expected, 'TOther.CreateSaturating( value ) )

    static member private AssertSaturatingEqual<'TOther when 'TOther :> Numerics.INumberBase<'TOther>>( value:Natural ) =
        NumberBase.AssertSaturatingEqual( value, 'TOther.One )

    [<Fact>]
    member public this.CreateSaturatingFromOther() =
        NumberBase.AssertSaturatingEqual<Byte>   ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<UInt16> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<UInt32> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<UInt64> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<UInt128>( Natural.Unit )

        NumberBase.AssertSaturatingEqual<SByte> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<Int16> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<Int32> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<Int64> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<Int128>( Natural.Unit )

        NumberBase.AssertSaturatingEqual<Single> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<Double> ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<Decimal>( Natural.Unit )

        NumberBase.AssertSaturatingEqual<Numerics.Complex>   ( Natural.Unit )
        NumberBase.AssertSaturatingEqual<Numerics.BigInteger>( Natural.Unit )

        NumberBase.AssertSaturatingEqual<Natural> ( Natural.Unit )
        //NumberBase.AssertSaturatingEqual<Integer> ( Natural.Unit )
        //NumberBase.AssertSaturatingEqual<Rational>( Natural.Unit )
        //NumberBase.AssertSaturatingEqual<Real>    ( Natural.Unit )

    [<Fact>]
    member public this.CreateSaturatingFromOtherSaturating() =
        NumberBase.AssertSaturatingEqual<Byte>   ( NumberBase.OverflowValue, Byte.MaxValue )
        NumberBase.AssertSaturatingEqual<UInt16> ( NumberBase.OverflowValue, UInt16.MaxValue )
        NumberBase.AssertSaturatingEqual<UInt32> ( NumberBase.OverflowValue, UInt32.MaxValue )
        NumberBase.AssertSaturatingEqual<UInt64> ( NumberBase.OverflowValue, UInt64.MaxValue )
        NumberBase.AssertSaturatingEqual<UInt128>( NumberBase.OverflowValue, UInt128.MaxValue )

        NumberBase.AssertSaturatingEqual<SByte> ( NumberBase.OverflowValue, SByte.MaxValue )
        NumberBase.AssertSaturatingEqual<Int16> ( NumberBase.OverflowValue, Int16.MaxValue )
        NumberBase.AssertSaturatingEqual<Int32> ( NumberBase.OverflowValue, Int32.MaxValue )
        NumberBase.AssertSaturatingEqual<Int64> ( NumberBase.OverflowValue, Int64.MaxValue )
        NumberBase.AssertSaturatingEqual<Int128>( NumberBase.OverflowValue, Int128.MaxValue )

        NumberBase.AssertSaturatingEqual<Single> ( NumberBase.OverflowValue, Single.MaxValue )
        NumberBase.AssertSaturatingEqual<Double> ( NumberBase.OverflowValue, Double.MaxValue )
        NumberBase.AssertSaturatingEqual<Decimal>( NumberBase.OverflowValue, Decimal.MaxValue )

        NumberBase.AssertSaturatingEqual<Numerics.Complex>( NumberBase.OverflowValue, Numerics.Complex( Double.MaxValue, 0.0 ) )

        // All of these can't overflow
        //  1) Numerics.BigInteger
        //  2) Natural
        //  3) Integer
        //  4) Rational
        //  5) Real

        // To be fair, I'd _like_ to test that BigInteger took the big number
        // However:
        //  1) It's in the magnitude of 600 digits long
        //  2) I'd need to know the value to compute the BigInteger

    static member private AssertTruncatingEqual<'TOther when 'TOther :> Numerics.INumberBase<'TOther>>( value:Natural, expected:'TOther ) =
        Assert.Equal( expected, 'TOther.CreateTruncating( value ) )

    static member private AssertTruncatingEqual<'TOther when 'TOther :> Numerics.INumberBase<'TOther>>( value:Natural ) =
        NumberBase.AssertTruncatingEqual( value, 'TOther.One )

    [<Fact>]
    member public this.CreateTruncatingFromOther() =
        NumberBase.AssertTruncatingEqual<Byte>   ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<UInt16> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<UInt32> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<UInt64> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<UInt128>( Natural.Unit )

        NumberBase.AssertTruncatingEqual<SByte> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<Int16> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<Int32> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<Int64> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<Int128>( Natural.Unit )

        NumberBase.AssertTruncatingEqual<Single> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<Double> ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<Decimal>( Natural.Unit )

        NumberBase.AssertTruncatingEqual<Numerics.Complex>   ( Natural.Unit )
        NumberBase.AssertTruncatingEqual<Numerics.BigInteger>( Natural.Unit )

        NumberBase.AssertTruncatingEqual<Natural> ( Natural.Unit )
        //NumberBase.AssertTruncatingEqual<Integer> ( Natural.Unit )
        //NumberBase.AssertTruncatingEqual<Rational>( Natural.Unit )
        //NumberBase.AssertTruncatingEqual<Real>    ( Natural.Unit )

    [<Fact>]
    member public this.CreateTruncatingFromOtherTruncating() =
        // If you're curious, that's equal to:
        // 32317006071311007300714876688669951960444102669715484032130345427524655138867890893197201411522913463688717960921898019494119559150490921095088152386448283120630877367300996091750197750389652106796057638384067568276792218642619756161838094338476170470581645852036305042887575891541065808607552399123930385521914333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230657
        // Knowing that is how I calculated the "correct" values for the floating point numbers
        let OverflowPlusOne = NumberBase.OverflowValue + Natural.Unit
        Diagnostics.Trace.WriteLine( OverflowPlusOne )

        NumberBase.AssertTruncatingEqual<Byte>   ( OverflowPlusOne )
        NumberBase.AssertTruncatingEqual<UInt16> ( OverflowPlusOne )
        NumberBase.AssertTruncatingEqual<UInt32> ( OverflowPlusOne )
        NumberBase.AssertTruncatingEqual<UInt64> ( OverflowPlusOne )
        NumberBase.AssertTruncatingEqual<UInt128>( OverflowPlusOne )

        NumberBase.AssertTruncatingEqual<SByte> ( OverflowPlusOne )
        NumberBase.AssertTruncatingEqual<Int16> ( OverflowPlusOne )
        NumberBase.AssertTruncatingEqual<Int32> ( OverflowPlusOne )
        NumberBase.AssertTruncatingEqual<Int64> ( OverflowPlusOne )
        NumberBase.AssertTruncatingEqual<Int128>( OverflowPlusOne )

        NumberBase.AssertTruncatingEqual<Decimal>( OverflowPlusOne )

        // 318478604952148193555853611059596230657
        NumberBase.AssertTruncatingEqual<Single> ( OverflowPlusOne, Single.Parse( "318478604952148193555853611059596230657" ) )
        // 14333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230657
        NumberBase.AssertTruncatingEqual<Double> ( OverflowPlusOne, Double.Parse( "14333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230657" ) )
        NumberBase.AssertTruncatingEqual<Numerics.Complex>( OverflowPlusOne, Numerics.Complex( Double.Parse( "14333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230657" ), 0.0 ) )

        // All of these can't overflow
        //  1) Numerics.BigInteger
        //  2) Natural
        //  3) Integer
        //  4) Rational
        //  5) Real

        // To be fair, I'd _like_ to test that BigInteger took the big number
        // However:
        //  1) It's in the magnitude of 600 digits long
        //  2) I'd need to know the value to compute the BigInteger
