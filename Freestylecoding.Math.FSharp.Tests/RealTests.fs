namespace Freestylecoding.Math.FSharp.Tests

open Xunit
open Freestylecoding.Math

module Real =
    module Ctor =
        [<Theory>]
        [<InlineData(  " 0", " 1",  "     0" )>]
        [<InlineData(  " 1", " 1",  "    10" )>]
        [<InlineData(  "-1", " 1",  "   -10" )>]
        [<InlineData(  " 1", " 2",  "   100" )>]
        [<InlineData(  "-1", " 2",  "  -100" )>]
        [<InlineData(  " 2", " 4",  " 20000" )>]
        [<InlineData(  "-2", " 4",  "-20000" )>]
        [<InlineData(  " 0", "-1",  "     0" )>]
        [<InlineData(  " 1", "-1",  "     0.1" )>]
        [<InlineData(  "-1", "-1",  "    -0.1" )>]
        [<InlineData(  " 1", "-2",  "     0.01" )>]
        [<InlineData(  "-1", "-2",  "    -0.01" )>]
        [<InlineData(  " 2", "-4",  "     0.0002" )>]
        [<InlineData(  "-2", "-4",  "    -0.0002" )>]
        [<InlineData(  "12", "-1",  "     1.2" )>]
        let Sanity n d r =
            Assert.Equal(
                Real.Parse( r ),
                Real( Integer.Parse( n ), Integer.Parse( d ) )
            )

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
        let CheckSignificand (l:int32) (r:int32) (e:bool) =
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
        let CheckExponent (l:int32) (r:int32) (e:bool) =
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
    
    module LessThan =
        [<Theory>]
        [<InlineData( " 0",     " 1",   true )>]
        [<InlineData( " 1",     " 0",   false )>]
        [<InlineData( " 0",     "-1",   false )>]
        [<InlineData( "-1",     " 0",   true )>]
        [<InlineData( " 1",     " 1",   false )>]
        [<InlineData( "-1",     " 1",   true )>]
        [<InlineData( " 1",     "-1",   false )>]
        [<InlineData( "-1",     "-1",   false )>]
        [<InlineData( " 0.5",   " 0.5", false )>]
        [<InlineData( "-0.5",   " 0.5", true )>]
        [<InlineData( " 0.5",   "-0.5", false )>]
        [<InlineData( "-0.5",   "-0.5", false )>]
        [<InlineData( " 0.33",  " 0.5", true )>]
        [<InlineData( "-0.33",  " 0.5", true )>]
        [<InlineData( " 0.33",  "-0.5", false )>]
        [<InlineData( "-0.33",  "-0.5", false )>]
        let Sanity l r x =
            Assert.Equal( x, Real.op_LessThan( Real.Parse( l ), Real.Parse( r ) ) )

        [<Theory>]
        [<InlineData( "0.01", "0.011", true )>]
        [<InlineData( "0.011", "0.01", false )>]
        [<InlineData( "1111", "100", false )>]
        let Normalization l r x =
            Assert.Equal( x, Real.op_LessThan( Real.Parse( l ), Real.Parse( r ) ) )

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

    module LessThanOrEqual =
        [<Theory>]
        [<InlineData( " 0",    " 1",   true )>]
        [<InlineData( " 1",    " 0",   false )>]
        [<InlineData( " 0",    "-1",   false )>]
        [<InlineData( "-1",    " 0",   true )>]
        [<InlineData( " 1",    " 1",   true )>]
        [<InlineData( "-1",    " 1",   true )>]
        [<InlineData( " 1",    "-1",   false )>]
        [<InlineData( "-1",    "-1",   true )>]
        [<InlineData( " 0.5",  " 0.5", true )>]
        [<InlineData( "-0.5",  " 0.5", true )>]
        [<InlineData( " 0.5",  "-0.5", false )>]
        [<InlineData( "-0.5",  "-0.5", true )>]
        [<InlineData( " 0.33", " 0.5", true )>]
        [<InlineData( "-0.33", " 0.5", true )>]
        [<InlineData( " 0.33", "-0.5", false )>]
        [<InlineData( "-0.33", "-0.5", false )>]
        let Sanity l r x =
            Assert.Equal( x, Real.Parse( l ) <= Real.Parse( r ) )

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
        let CheckSignificand (l:int32) (r:int32) (e:bool) =
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
        let CheckExponent (l:int32) (r:int32) (e:bool) =
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

    module Negation =
        [<Theory>]
        [<InlineData(  "1", "-1" )>] // Sanity
        [<InlineData( "-1", " 1" )>] // Sanity
        [<InlineData(  "0", " 0" )>] // Sanity
        let Sanity l r =
            Assert.Equal(
                Real.Parse( l ),
                -Real.Parse( r )
            )

    module Equals =
        [<Theory>]
        [<InlineData(  1,  1, true )>]    // Sanity
        [<InlineData(  1, -1, false )>]   // Sanity
        [<InlineData( -1,  1, false )>]   // Sanity
        [<InlineData( -1, -1, true )>]    // Sanity
        let Sanity (l:int32) (r:int32) (e:bool) =
            Assert.Equal( e, Real( Integer( l ), Integer.Unit ).Equals( Real( Integer( r ), Integer.Unit ) ) )
        
        [<Fact>]
        let NaturalEquals () =
            Assert.True( Real.Unit.Equals( Natural.Unit ) )
        
        [<Fact>]
        let NaturalNotEquals () =
            Assert.False( Real.Unit.Equals( Natural.Zero ) )
        
        [<Fact>]
        let NaturalSignNotEquals () =
            Assert.False( (-Real.Unit).Equals( Natural.Unit ) )
    
        [<Fact>]
        let IntegerEquals () =
            Assert.True( Real.Unit.Equals( Integer.Unit ) )
        
        [<Fact>]
        let IntegerNotEquals () =
            Assert.False( Real.Unit.Equals( Integer.Zero ) )
        
        [<Fact>]
        let IntegerSignNotEquals () =
            Assert.False( (-Real.Unit).Equals( Integer.Unit ) )

        [<Fact>]
        let RationalEquals () =
            Assert.True( Real.Unit.Equals( Rational.Unit ) )
        
        [<Fact>]
        let RationalNotEquals () =
            Assert.False( Real.Unit.Equals( Rational.Zero ) )
        
        [<Fact>]
        let RationalSignNotEquals () =
            Assert.False( (-Real.Unit).Equals( Rational.Unit ) )

        [<Fact>]
        let RationalConversionEquals () =
            Assert.True( Real.Parse( "0.5" ).Equals( Rational( 1, 2u ) ) )

        [<Fact>]
        let RationalConversionEqualsWithRepeating () =
            Assert.True( Real.Parse( "0.333333333333333333333333333333" ).Equals( Rational( 1, 3u ) ) )
    
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
        [<InlineData( "4.321e3",   4321,  0 )>]        // Sanity
        [<InlineData( "4.32e3",    432,   1 )>]        // Sanity
        [<InlineData( "43.21e-3",  4321, -5 )>]        // Sanity
        [<InlineData( "43.2e-1",   432,  -2 )>]        // Sanity
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
