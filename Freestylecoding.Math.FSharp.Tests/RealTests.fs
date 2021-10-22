namespace Freestylecoding.Math.FSharp.Tests

open Xunit
open Freestylecoding.Math

module Real =
    // NOTE: The sanity tests are here to do low level tests of a few parts
    // This lets me isolate those tests so I can use those parts in the rest of the tests
    //
    // In other words:
    // IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!

    //[<Trait( "Type", "Sanity" )>]
    //module Sanity =
    //    [<Fact>]
    //    let GreaterThanTrue () =
    //        Assert.True(
    //            Rational(
    //                Integer( [0xBADu; 0xDEADBEEFu;] ),
    //                Natural( [2u] )
    //            ) > Rational(
    //                Integer( [0xDEADBEEFu; 0xBADu], true ),
    //                Natural( [2u] )
    //            )
    //        )
    //
    //    [<Fact>]
    //    let GreaterThanFalseByLessThan () =
    //        Assert.False(
    //            Rational(
    //                Integer( [0xBADu; 0xDEADBEEFu;] , true),
    //                Natural( [2u] )
    //            ) > Rational(
    //                Integer( [0xDEADBEEFu; 0xBADu] ),
    //                Natural( [2u] )
    //            )
    //        )
    //
    //    [<Fact>]
    //    let GreaterThanFalseByEquals () =
    //        Assert.False(
    //            Rational(
    //                Integer( [0xBADu; 0xDEADBEEFu;] , true),
    //                Natural( [2u] )
    //            ) > Rational(
    //                Integer( [0xBADu; 0xDEADBEEFu;] , true),
    //                Natural( [2u] )
    //            )
    //        )
    //
    //    [<Fact>]
    //    let Addition () =
    //        Assert.Equal(
    //            Rational( Integer( 5 ), Natural( 4u ) ),
    //            Rational( Integer.Unit, Natural( 2u ) ) + Rational( Integer( 3 ), Natural( 4u ) )
    //        )
    //
    //    [<Fact>]
    //    let Subtraction () =
    //        Assert.Equal(
    //            Rational( Integer.Unit, Natural( 4u ) ),
    //            Rational( Integer( 3 ), Natural( 4u ) ) - Rational( Integer.Unit, Natural( 2u ) )
    //        )
    //
    //    [<Fact>]
    //    let Multiplication () =
    //        Assert.Equal(
    //            Rational( Integer( 3 ), Natural( 8u ) ),
    //            Rational( Integer.Unit, Natural( 2u ) ) * Rational( Integer( 3 ), Natural( 4u ) )
    //        )
    //
    //    [<Fact>]
    //    let DivisionModulo () =
    //        Assert.Equal(
    //            (
    //                Integer.Unit,
    //                Rational( Integer.Unit, Natural( 2u ) )
    //            ),
    //            Rational( Integer( 3 ), Natural( 4u ) ) /% Rational( Integer.Unit, Natural( 2u ) )
    //        )
    //
    //    [<Fact>]
    //    let ToString () =
    //        Assert.Equal( "-5 / 7", Rational( Integer( -5 ), Natural( 7u ) ).ToString() )
    //
    //    [<Fact>]
    //    let Parse () =
    //        Assert.Equal(
    //            Rational( Integer( [0x112210F4u; 0x7DE98115u], true ), Natural( 41u ) ),
    //            Rational.Parse("-1234567890123456789 / 41")
    //        )

    //module Ctor =
    //    [<Theory>]
    //    [<InlineData(  " 0", "1",  " 0" )>]
    //    [<InlineData(  " 1", "1",  " 1" )>]
    //    [<InlineData(  "-1", "1",  "-1" )>]
    //    [<InlineData(  " 1", "2",  " 1/2" )>]
    //    [<InlineData(  "-1", "2",  "-1/2" )>]
    //    [<InlineData(  " 2", "4",  " 1/2" )>]
    //    [<InlineData(  "-2", "4",  "-1/2" )>]
    //    [<InlineData(  " 6", "15", " 2/5" )>]
    //    [<InlineData(  "-6", "15", "-2/5" )>]
    //    let Sanity n d r =
    //        Assert.Equal(
    //            Rational.Parse( r ),
    //            Rational( Integer.Parse( n ), Natural.Parse( d ) )
    //        )
    //
    //    [<Fact>]
    //    let DivideByZero =
    //        Assert.IsType<System.DivideByZeroException>(
    //            Record.Exception( fun () -> Rational( Integer.Unit, Natural.Zero ) |> ignore )
    //        )

    module Equality =
        [<Theory>]
        [<InlineData( " 0", " 0", true )>]
        [<InlineData( " 0", " 1", false )>]
        [<InlineData( " 1", " 0", false )>]
        [<InlineData( " 1", " 1", true )>]
        [<InlineData( " 0", "-1", false )>]
        [<InlineData( "-1", " 0", false )>]
        [<InlineData( "-1", " 1", false )>]
        [<InlineData( " 1", "-1", false )>]
        [<InlineData( "-1", "-1", true )>]
        [<InlineData( " 1", "10", false )>]
        [<InlineData( " 1", "0.1", false )>]
        [<InlineData( "10", "0.1", false )>]
        [<InlineData( "10", "100", false )>]
        let Sanity (l:string) (r:string) (e:bool) =
            Assert.Equal( e, Real.Parse( l ) = Real.Parse( r ) )

        [<Theory>]
        [<InlineData(  0,  0, true )>]
        [<InlineData(  0,  1, false )>]
        [<InlineData(  1,  0, false )>]
        [<InlineData(  1,  1, true )>]
        [<InlineData(  0, -1, false )>]
        [<InlineData( -1,  0, false )>]
        [<InlineData( -1,  1, false )>]
        [<InlineData(  1, -1, false )>]
        [<InlineData( -1, -1, true )>]
        let CheckSignificand l r (e:bool) =
            Assert.Equal( e, Real( l, 0 ) = Real( r, 0 ) )

        [<Theory>]
        [<InlineData(  0,  0, true )>]
        [<InlineData(  0,  1, false )>]
        [<InlineData(  1,  0, false )>]
        [<InlineData(  1,  1, true )>]
        [<InlineData(  0, -1, false )>]
        [<InlineData( -1,  0, false )>]
        [<InlineData( -1,  1, false )>]
        [<InlineData(  1, -1, false )>]
        [<InlineData( -1, -1, true )>]
        let CheckExponent l r (e:bool) =
            Assert.Equal( e, Real( 1, l ) = Real( 1, r ) )
    
    module GreaterThan =
        [<Theory>]
        [<InlineData( " 0",     " 1",   false )>]
        [<InlineData( " 1",     " 0",   true )>]
        [<InlineData( " 0",     "-1",   true )>]
        [<InlineData( "-1",     " 0",   false )>]
        [<InlineData( " 1",     " 1",   false )>]
        [<InlineData( "-1",     " 1",   false )>]
        [<InlineData( " 1",     "-1",   true )>]
        [<InlineData( "-1",     "-1",   false )>]
        [<InlineData( " 0.5",   " 0.5", false )>]
        [<InlineData( "-0.5",   " 0.5", false )>]
        [<InlineData( " 0.5",   "-0.5", true )>]
        [<InlineData( "-0.5",   "-0.5", false )>]
        [<InlineData( " 0.33",  " 0.5", false )>]
        [<InlineData( "-0.33",  " 0.5", false )>]
        [<InlineData( " 0.33",  "-0.5", true )>]
        [<InlineData( "-0.33",  "-0.5", true )>]
        let Sanity l r x =
            Assert.Equal( x, Real.op_GreaterThan( Real.Parse( l ), Real.Parse( r ) ) )

        [<Theory>]
        [<InlineData( "0.01", "0.011", false )>]
        [<InlineData( "0.011", "0.01", true )>]
        [<InlineData( "1111", "100", true)>]
        let Normalization l r x =
            Assert.Equal( x, Real.op_GreaterThan( Real.Parse( l ), Real.Parse( r ) ) )
    
    //module LessThan =
    //    [<Theory>]
    //    [<InlineData( " 0",    " 1",   true )>]
    //    [<InlineData( " 1",    " 0",   false )>]
    //    [<InlineData( " 0",    "-1",   false )>]
    //    [<InlineData( "-1",    " 0",   true )>]
    //    [<InlineData( " 1",    " 1",   false )>]
    //    [<InlineData( "-1",    " 1",   true )>]
    //    [<InlineData( " 1",    "-1",   false )>]
    //    [<InlineData( "-1",    "-1",   false )>]
    //    [<InlineData( " 1/2",  " 1/2", false )>]
    //    [<InlineData( "-1/2",  " 1/2", true )>]
    //    [<InlineData( " 1/2",  "-1/2", false )>]
    //    [<InlineData( "-1/2",  "-1/2", false )>]
    //    [<InlineData( " 1/3",  " 1/2", true )>]
    //    [<InlineData( "-1/3",  " 1/2", true )>]
    //    [<InlineData( " 1/3",  "-1/2", false )>]
    //    [<InlineData( "-1/3",  "-1/2", false )>]
    //    let Sanity l r x =
    //        Assert.Equal( x, Rational.Parse( l ) < Rational.Parse( r ) )

    module GreaterThanOrEqual =
        [<Theory>]
        [<InlineData( " 0",     " 1",   false )>]
        [<InlineData( " 1",     " 0",   true )>]
        [<InlineData( " 0",     "-1",   true )>]
        [<InlineData( "-1",     " 0",   false )>]
        [<InlineData( " 1",     " 1",   true )>]
        [<InlineData( "-1",     " 1",   false )>]
        [<InlineData( " 1",     "-1",   true )>]
        [<InlineData( "-1",     "-1",   true )>]
        [<InlineData( " 0.5",   " 0.5", true )>]
        [<InlineData( "-0.5",   " 0.5", false )>]
        [<InlineData( " 0.5",   "-0.5", true )>]
        [<InlineData( "-0.5",   "-0.5", true )>]
        [<InlineData( " 0.33",  " 0.5", false )>]
        [<InlineData( "-0.33",  " 0.5", false )>]
        [<InlineData( " 0.33",  "-0.5", true )>]
        [<InlineData( "-0.33",  "-0.5", true )>]
        let Sanity l r x =
            Assert.Equal( x, Real.op_GreaterThanOrEqual( Real.Parse( l ), Real.Parse( r ) ) )

    //module LessThanOrEqual =
    //    [<Theory>]
    //    [<InlineData( " 0",    " 1",   true )>]
    //    [<InlineData( " 1",    " 0",   false )>]
    //    [<InlineData( " 0",    "-1",   false )>]
    //    [<InlineData( "-1",    " 0",   true )>]
    //    [<InlineData( " 1",    " 1",   true )>]
    //    [<InlineData( "-1",    " 1",   true )>]
    //    [<InlineData( " 1",    "-1",   false )>]
    //    [<InlineData( "-1",    "-1",   true )>]
    //    [<InlineData( " 1/2",  " 1/2", true )>]
    //    [<InlineData( "-1/2",  " 1/2", true )>]
    //    [<InlineData( " 1/2",  "-1/2", false )>]
    //    [<InlineData( "-1/2",  "-1/2", true )>]
    //    [<InlineData( " 1/3",  " 1/2", true )>]
    //    [<InlineData( "-1/3",  " 1/2", true )>]
    //    [<InlineData( " 1/3",  "-1/2", false )>]
    //    [<InlineData( "-1/3",  "-1/2", false )>]
    //    let Sanity l r x =
    //        Assert.Equal( x, Rational.Parse( l ) <= Rational.Parse( r ) )

    module Inequality =
        [<Theory>]
        [<InlineData( " 0",     " 1",   true )>]
        [<InlineData( " 1",     " 0",   true )>]
        [<InlineData( " 0",     "-1",   true )>]
        [<InlineData( "-1",     " 0",   true )>]
        [<InlineData( " 1",     " 1",   false )>]
        [<InlineData( "-1",     " 1",   true )>]
        [<InlineData( " 1",     "-1",   true )>]
        [<InlineData( "-1",     "-1",   false )>]
        [<InlineData( " 0.5",   " 0.5", false )>]
        [<InlineData( "-0.5",   " 0.5", true )>]
        [<InlineData( " 0.5",   "-0.5", true )>]
        [<InlineData( "-0.5",   "-0.5", false )>]
        [<InlineData( " 0.33",  " 0.5", true )>]
        [<InlineData( "-0.33",  " 0.5", true )>]
        [<InlineData( " 0.33",  "-0.5", true )>]
        [<InlineData( "-0.33",  "-0.5", true )>]
        let Sanity l r x =
            Assert.Equal( x, Real.Parse( l ) <> Real.Parse( r ) )

        [<Theory>]
        [<InlineData(  0,  0, false )>]
        [<InlineData(  0,  1, true )>]
        [<InlineData(  1,  0, true )>]
        [<InlineData(  1,  1, false )>]
        [<InlineData(  0, -1, true )>]
        [<InlineData( -1,  0, true )>]
        [<InlineData( -1,  1, true )>]
        [<InlineData(  1, -1, true )>]
        [<InlineData( -1, -1, false )>]
        let CheckSignificand l r (e:bool) =
            Assert.Equal( e, Real( l, 0 ) <> Real( r, 0 ) )

        [<Theory>]
        [<InlineData(  0,  0, false )>]
        [<InlineData(  0,  1, true )>]
        [<InlineData(  1,  0, true )>]
        [<InlineData(  1,  1, false )>]
        [<InlineData(  0, -1, true )>]
        [<InlineData( -1,  0, true )>]
        [<InlineData( -1,  1, true )>]
        [<InlineData(  1, -1, true )>]
        [<InlineData( -1, -1, false )>]
        let CheckExponent l r (e:bool) =
            Assert.Equal( e, Real( 1, l ) <> Real( 1, r ) )

    module Addition =
        [<Theory>]
        [<InlineData( "0",        "0",         "0" )>]             // Sanity
        [<InlineData( "1",        "0",         "1" )>]             // Sanity
        [<InlineData( "0",        "1",         "1" )>]             // Sanity
        [<InlineData( "1",        "1",         "2" )>]             // Sanity
        [<InlineData( "10",       "10",        "20" )>]            // Sanity
        [<InlineData( ".1",       ".1",        ".2" )>]            // Sanity
        [<InlineData( "12300",    "45000",     "57300" )>]         // Sanity
        [<InlineData( "0.00123",  "0.000045",  "0.001275" )>]      // Sanity
        [<InlineData( "12300",    "0.000045",  "12300.000045" )>]  // Sanity
        [<InlineData( "0.00123",  "45000",     "45000.00123" )>]   // Sanity
        [<InlineData( "-1",       "0",         "-1" )>]            // Sanity
        [<InlineData( "0",        "-1",        "-1" )>]            // Sanity
        [<InlineData( "-1",       "-1",         "-2" )>]           // Sanity
        [<InlineData( "-10",      "-10",       "-20" )>]           // Sanity
        [<InlineData( "-0.1",     "-0.1",      "-0.2" )>]          // Sanity
        [<InlineData( "-12300",   "-45000",    "-57300" )>]        // Sanity
        [<InlineData( "-0.00123", "-0.000045", "-0.001275" )>]     // Sanity
        [<InlineData( "-12300",   "-0.000045", "-12300.000045" )>] // Sanity
        [<InlineData( "-0.00123", "-45000",    "-45000.00123" )>]  // Sanity
        let Sanity l r s =
            Assert.Equal( Real.Parse( s ), Real.Parse( l ) + Real.Parse( r ) )

    module Subtraction =
        [<Theory>]
        [<InlineData( "0",        "0",         "0" )>]             // Sanity
        [<InlineData( "1",        "0",         "1" )>]             // Sanity
        [<InlineData( "0",        "1",         "-1" )>]            // Sanity
        [<InlineData( "1",        "1",         "0" )>]             // Sanity
        [<InlineData( "10",       "10",        "0" )>]             // Sanity
        [<InlineData( ".1",       ".1",        "0" )>]             // Sanity
        [<InlineData( "12300",    "45000",     "-32700" )>]        // Sanity
        [<InlineData( "0.00123",  "0.000045",  "0.001185" )>]      // Sanity
        [<InlineData( "12300",    "0.000045",  "12299.999955" )>]  // Sanity
        [<InlineData( "0.00123",  "45000",     "-44999.99877" )>]  // Sanity
        [<InlineData( "-1",       "0",         "-1" )>]            // Sanity
        [<InlineData( "0",        "-1",        "1" )>]             // Sanity
        [<InlineData( "-1",       "-1",        "0" )>]             // Sanity
        [<InlineData( "-10",      "-10",       "0" )>]             // Sanity
        [<InlineData( "-0.1",     "-0.1",      "0" )>]             // Sanity
        [<InlineData( "-12300",   "-45000",    "32700" )>]         // Sanity
        [<InlineData( "-0.00123", "-0.000045", "-0.001185" )>]     // Sanity
        [<InlineData( "-12300",   "-0.000045", "-12299.999955" )>] // Sanity
        [<InlineData( "-0.00123", "-45000",    "44999.99877" )>]   // Sanity
        let Sanity l r s =
            Assert.Equal( Real.Parse( s ), Real.Parse( l ) - Real.Parse( r ) )

    module Multiply =
        [<SkippableTheory>]
        [<InlineData( "0",  "0",    "0" )>]      // Sanity
        [<InlineData( "0",  "1",    "0" )>]      // Sanity
        [<InlineData( "1",  "0",    "0" )>]      // Sanity
        [<InlineData( "1",  "1",    "1" )>]      // Sanity
        [<InlineData( "-1", "1",    "-1" )>]      // Sanity
        [<InlineData( "1",  "-1",    "-1" )>]      // Sanity
        [<InlineData( "-1", "-1",    "1" )>]      // Sanity
        [<InlineData( "1",  "42",    "42" )>]        // Sanity
        [<InlineData( "1",  "3.14",  "3.14" )>]        // Sanity
        [<InlineData( "1",  "0.056",  "0.056" )>]        // Sanity
        [<InlineData( "42",    "1",  "42" )>]        // Sanity
        [<InlineData( "3.14",  "1",  "3.14" )>]        // Sanity
        [<InlineData( "0.056", "1",   "0.056" )>]        // Sanity
        [<InlineData( "3.14159265", "2.7182818", "8.53973412350877" )>]
        [<InlineData( "1234567890", "987654321", "1219326311126352690" )>]
        [<InlineData( ".123456789", ".0987654321", "0.0121932631112635269" )>]
        [<InlineData( "1234567890", ".0987654321", "121932631.112635269" )>]
        [<InlineData( "1234567890", ".0987654321", "121932631.112635269" )>]
        [<InlineData( "2e100", "1e-100", "2", Skip = "Parse doesn't support E yet" )>]
        [<InlineData( "2e101", "1e-100", "20", Skip = "Parse doesn't support E yet" )>]
        [<InlineData( "2e100", "1e-101", "0.2", Skip = "Parse doesn't support E yet" )>]
        let Sanity l r p =
            Assert.Equal( Real.Parse( p ), Real.Parse( l ) * Real.Parse( r ) )
    
        [<Fact>]
        let Inverse () =
            Assert.Equal(
                Real.Parse( "1" ),
                Real.Parse( "0.5" ) * Real.Parse( "2" )
            )
    
        [<Fact>]
        let Associtivity () =
            Assert.Equal(
                ( Real.Parse( "0.5" ) * Real.Parse( "0.25" ) ) * Real.Parse( "0.125" ),
                Real.Parse( "0.5" ) * ( Real.Parse( "0.25" ) * Real.Parse( "0.125" ) )
            )
    
        [<Fact>]
        let Communtivity () =
            Assert.Equal(
                Real.Parse( "0.5" ) * Real.Parse( "0.25" ),
                Real.Parse( "0.25" ) * Real.Parse( "0.5" )
            )
    
        [<Fact>]
        let Distributive () =
            Assert.Equal(
                ( Real.Parse( "0.5" ) * Real.Parse( "0.25" ) ) + ( Real.Parse( "0.5" ) * Real.Parse( "0.125" ) ),
                Real.Parse( "0.5" ) * ( Real.Parse( "0.25" ) + Real.Parse( "0.125" ) )
            )

    module Division =
        [<Theory>]
        [<InlineData( " 0.5",  " 1",    " 0.5" )>]      // Sanity
        [<InlineData( "-0.5",  " 1",    "-0.5" )>]      // Sanity
        [<InlineData( " 0.5",  "-1",    "-0.5" )>]      // Sanity
        [<InlineData( "-0.5",  "-1",    " 0.5" )>]      // Sanity
        [<InlineData( " 1  ",  " 0.5",  " 2" )>]        // Sanity
        [<InlineData( "-1  ",  " 0.5",  "-2" )>]        // Sanity
        [<InlineData( " 1  ",  "-0.5",  "-2" )>]        // Sanity
        [<InlineData( "-1  ",  "-0.5",  " 2" )>]        // Sanity
        [<InlineData( " 0",    " 0.5",  " 0" )>]        // Sanity
        [<InlineData( " 0",    "-0.5",  " 0" )>]        // Sanity
        [<InlineData( " 0.5",  " 0.5",  " 1" )>]        // Sanity
        [<InlineData( "-0.5",  " 0.5",  "-1" )>]        // Sanity
        [<InlineData( " 0.5",  "-0.5",  "-1" )>]        // Sanity
        [<InlineData( "-0.5",  "-0.5",  " 1" )>]        // Sanity
        [<InlineData( " 1",    " 2",    " 0.5" )>]      // Sanity
        [<InlineData( "-1",    " 2",    "-0.5" )>]      // Sanity
        [<InlineData( " 1",    "-2",    "-0.5" )>]      // Sanity
        [<InlineData( "-1",    "-2",    " 0.5" )>]      // Sanity
        let Sanity dividend divisor quotient =
            Assert.Equal( Real.Parse( quotient ), Real.Parse( dividend ) / Real.Parse( divisor ) )
    
        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Real.Unit / Real.Zero ) |> ignore )

        [<Theory>]
        [<InlineData( " 1",   "3",  "0.333333333333333333333333333333" )>]
        [<InlineData( "10",   "3",  "3.33333333333333333333333333333" )>]
        [<InlineData( " 0.1", "3",  "0.0333333333333333333333333333333" )>]
        let DefaultPreceision dividend divisor quotient =
            Assert.Equal( Real.Parse( quotient ), Real.Parse( dividend ) / Real.Parse( divisor ) )

        [<Theory>]
        [<InlineData( " 2",   "3",  "0.666666666666666666666666666667" )>]
        [<InlineData( "20",   "3",  "6.66666666666666666666666666667" )>]
        [<InlineData( " 0.2", "3",  "0.0666666666666666666666666666667" )>]
        [<InlineData( "10",   "11", "0.90909090909090909090909090909" )>]
        let Rounding dividend divisor quotient =
            Assert.Equal( Real.Parse( quotient ), Real.Parse( dividend ) / Real.Parse( divisor ) )

    module Modulo =
        [<Theory>]
        [<InlineData( " 0.5", " 1",   " 0.5" )>]      // Sanity
        [<InlineData( "-0.5", " 1",   "-0.5" )>]      // Sanity
        [<InlineData( " 0.5", "-1",   " 0.5" )>]      // Sanity
        [<InlineData( "-0.5", "-1",   "-0.5" )>]      // Sanity
        [<InlineData( " 1  ", " 0.5", " 0" )>]        // Sanity
        [<InlineData( "-1  ", " 0.5", " 0" )>]        // Sanity
        [<InlineData( " 1  ", "-0.5", " 0" )>]        // Sanity
        [<InlineData( "-1  ", "-0.5", " 0" )>]        // Sanity
        [<InlineData( " 0",   " 0.5", " 0" )>]        // Sanity
        [<InlineData( " 0",   "-0.5", " 0" )>]        // Sanity
        [<InlineData( " 3.8", " 1.3", " 1.2" )>]     // multiple bits
        [<InlineData( "-3.8", " 1.3", "-1.2" )>]     // multiple bits
        [<InlineData( " 3.8", "-1.3", " 1.2" )>]     // multiple bits
        [<InlineData( "-3.8", "-1.3", "-1.2" )>]     // multiple bits
        let Sanity dividend divisor remainder =
            Assert.Equal(
                Real.Parse( remainder ),
                Real.Parse( dividend ) % Real.Parse( divisor )
            )
    
        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Real.Unit % Real.Zero ) |> ignore )
    
        [<Fact>]
        let Inverse () =
            Assert.Equal(
                Real.Parse( "0" ),
                Real.Parse( "0.5" ) % Real.Parse( "0.5" )
            )

    module DivisionModulo =
        [<Theory>]
        [<InlineData( " 0.5",  " 1",    " 0.5", " 0.5" )>]      // Sanity
        [<InlineData( "-0.5",  " 1",    "-0.5", "-0.5" )>]      // Sanity
        [<InlineData( " 0.5",  "-1",    "-0.5", " 0.5" )>]      // Sanity
        [<InlineData( "-0.5",  "-1",    " 0.5", "-0.5" )>]      // Sanity
        [<InlineData( " 1  ",  " 0.5",  " 2",   " 0" )>]        // Sanity
        [<InlineData( "-1  ",  " 0.5",  "-2",   " 0" )>]        // Sanity
        [<InlineData( " 1  ",  "-0.5",  "-2",   " 0" )>]        // Sanity
        [<InlineData( "-1  ",  "-0.5",  " 2",   " 0" )>]        // Sanity
        [<InlineData( " 0",    " 0.5",  " 0",   " 0" )>]        // Sanity
        [<InlineData( " 0",    "-0.5",  " 0",   " 0" )>]        // Sanity
        [<InlineData( " 3.8", " 1.3", " 2.923076923076923076923076923076", " 1.2" )>]     // multiple bits
        [<InlineData( "-3.8", " 1.3", "-2.923076923076923076923076923076", "-1.2" )>]     // multiple bits
        [<InlineData( " 3.8", "-1.3", "-2.923076923076923076923076923076", " 1.2" )>]     // multiple bits
        [<InlineData( "-3.8", "-1.3", " 2.923076923076923076923076923076", "-1.2" )>]     // multiple bits
        let Sanity dividend divisor quotient remainder =
            Assert.Equal(
                ( Real.Parse( quotient ), Real.Parse( remainder ) ),
                Real.Parse( dividend ) /% Real.Parse( divisor )
            )
    
        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Real.Unit /% Real.Zero ) |> ignore )
    
        [<Fact>]
        let Inverse () =
            Assert.Equal(
                ( Real.Parse( "1" ), Real.Parse( "0" ) ),
                Real.Parse( "0.5" ) /% Real.Parse( "0.5" )
            )

    //module Negation =
    //    [<Theory>]
    //    [<InlineData(  1, 1u, -1, 1u )>] // Sanity
    //    [<InlineData( -1, 1u,  1, 1u )>] // Sanity
    //    [<InlineData(  0, 1u,  0, 1u )>] // Sanity
    //    let Sanity (n:int) (d:uint) (en:int) (ed:uint) =
    //        Assert.Equal(
    //            Rational( Integer( en ), Natural( ed ) ),
    //            -Rational( Integer( n ), Natural( d ) )
    //        )
    //
    //    [<Fact>]
    //    let Big () =
    //        Assert.Equal(
    //            Rational( Integer( [0xFEDCBA9u; 0x76543210u], true ), Natural.Unit ),
    //            -Rational( Integer( [0xFEDCBA9u; 0x76543210u], false ), Natural.Unit )
    //        )
    //
    //    [<Fact>]
    //    let BigNegative () =
    //        Assert.Equal(
    //            Rational( Integer( [0xFEDCBA9u; 0x76543210u], false ), Natural.Unit ),
    //            -Rational( Integer( [0xFEDCBA9u; 0x76543210u], true ), Natural.Unit )
    //        )
    //
    //    [<Fact>]
    //    let Small () =
    //        Assert.Equal(
    //            Rational( -Integer.Unit, Natural( [0xFEDCBA9u; 0x76543210u] ) ),
    //            -Rational( Integer.Unit, Natural( [0xFEDCBA9u; 0x76543210u] ) )
    //        )
    //
    //    [<Fact>]
    //    let SmallNegative () =
    //        Assert.Equal(
    //            Rational( Integer.Unit, Natural( [0xFEDCBA9u; 0x76543210u] ) ),
    //            -Rational( -Integer.Unit, Natural( [0xFEDCBA9u; 0x76543210u] ) )
    //        )

    //module Equals =
    //    [<Theory>]
    //    [<InlineData(  1,  1, true )>]    // Sanity
    //    [<InlineData(  1, -1, false )>]   // Sanity
    //    [<InlineData( -1,  1, false )>]   // Sanity
    //    [<InlineData( -1, -1, true )>]    // Sanity
    //    let Sanity (l:int32) (r:int32) (e:bool) =
    //        Assert.Equal( e, Rational( Integer( l ), Natural.Unit ).Equals( Rational( Integer( r ), Natural.Unit ) ) )
    //    
    //    [<Fact>]
    //    let NaturalEquals () =
    //        Assert.True( Rational.Unit.Equals( Natural.Unit ) )
    //    
    //    [<Fact>]
    //    let NaturalNotEquals () =
    //        Assert.False( Rational.Unit.Equals( Natural.Zero ) )
    //    
    //    [<Fact>]
    //    let NaturalSignNotEquals () =
    //        Assert.False( (-Rational.Unit).Equals( Natural.Unit ) )
    //
    //    [<Fact>]
    //    let IntegerEquals () =
    //        Assert.True( Rational.Unit.Equals( Integer.Unit ) )
    //    
    //    [<Fact>]
    //    let IntegerNotEquals () =
    //        Assert.False( Rational.Unit.Equals( Integer.Zero ) )
    //    
    //    [<Fact>]
    //    let IntegerSignNotEquals () =
    //        Assert.False( (-Rational.Unit).Equals( Integer.Unit ) )
    
    module ToString =
        [<Theory>]
        [<InlineData(  0,      0, "0" )>]
        [<InlineData(  1,      0, "1" )>]
        [<InlineData( -1,      0, "-1" )>]
        [<InlineData(  1,      1, "10" )>]
        [<InlineData( -1,      1, "-10" )>]
        [<InlineData(  1,     -1, "0.1" )>]
        [<InlineData( -1,     -1, "-0.1" )>]
        [<InlineData(  12345,  0, "12345" )>]
        [<InlineData( -12345,  0, "-12345" )>]
        [<InlineData(  12345, -3, "12.345" )>]
        [<InlineData( -12345, -3, "-12.345" )>]
        [<InlineData(  12000,  0, "12000" )>]
        [<InlineData( -12000,  0, "-12000" )>]
        let Sanity (s:int) (e:int) expected =
            Assert.Equal( expected, Real( s, e ).ToString() )

        [<Fact>]
        let SimplePositiveLarge () =
            Assert.Equal( "11111111111111111111111111", Real.Parse( "11111111111111111111111111" ).ToString() )

        [<Fact>]
        let SimpleNegativeLarge () =
            Assert.Equal( "-11111111111111111111111111", Real.Parse( "-11111111111111111111111111" ).ToString() )

        [<Fact>]
        let SimplePositiveLargeRounded () =
            Assert.Equal( "77777777777777777777777777", Real.Parse( "77777777777777777777777777" ).ToString() )

        [<Fact>]
        let SimpleNegativeLargeRounded () =
            Assert.Equal( "-77777777777777777777777777", Real.Parse( "-77777777777777777777777777" ).ToString() )

        [<Fact>]
        let SimplePositiveSmall () =
            Assert.Equal( "0.00000000000000000000000011111111111111111111111111", Real.Parse( "0.00000000000000000000000011111111111111111111111111" ).ToString() )

        [<Fact>]
        let SimpleNegativeSmall () =
            Assert.Equal( "-0.00000000000000000000000011111111111111111111111111", Real.Parse( "-0.00000000000000000000000011111111111111111111111111" ).ToString() )


    module Parse =
        [<Theory>]
        [<InlineData( "4321",      4321,  0 )>]        // Sanity
        [<InlineData( "43.21",     4321, -2 )>]        // Sanity
        [<InlineData( "432100",    4321,  2 )>]        // Sanity
        [<InlineData( "0.04321",   4321, -5 )>]        // Sanity
        [<InlineData( ".04321",    4321, -5 )>]        // Sanity
        [<InlineData( "-4321",    -4321,  0 )>]        // Sanity
        [<InlineData( "-43.21",   -4321, -2 )>]        // Sanity
        [<InlineData( "-432100",  -4321,  2 )>]        // Sanity
        [<InlineData( "-0.04321", -4321, -5 )>]        // Sanity
        [<InlineData( "-.04321",  -4321, -5 )>]        // Sanity
        let Sanity (str:string) (n:int32) (d:int32) =
            Assert.Equal( Real(n, d), Real.Parse(str) )
    
        [<Theory>]
        [<InlineData( "" )>]
        [<InlineData( " " )>]
        [<InlineData( "\t" )>]
        [<InlineData( "\n" )>]
        [<InlineData( "\r" )>]
        [<InlineData( null )>]
        [<InlineData( "\r\n" )>]
        let ArgEmpty str =
            Assert.IsType<System.ArgumentNullException>(
                Record.Exception( fun () -> Real.Parse( str ) |> ignore )
            )

        [<Theory>]
        [<InlineData( "1.-2" )>]
        [<InlineData( "1.2.3" )>]
        let FormatEx str =
            Assert.IsType<System.FormatException>(
                Record.Exception( fun () -> Real.Parse( str ) |> ignore )
            )
