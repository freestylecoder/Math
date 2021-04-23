namespace Freestylecoding.Math.FSharp.Tests

open Xunit
open Freestylecoding.Math

module Rational =
    // NOTE: The sanity tests are here to do low level tests of a few parts
    // This lets me isolate those tests so I can use those parts in the rest of the tests
    //
    // In other words:
    // IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
    [<Trait( "Type", "Sanity" )>]
    module Sanity =
        //[<Fact>]
        //let LeftShift () =
        //    Assert.Equal( Rational([4u; 0x8000_0000u; 0u], true), Rational([9u], true) <<< 63 )

        //[<Fact>]
        //let RightShift () =
        //    Assert.Equal( Rational([9u], true), Rational([4u; 0x8000_0000u; 0u], true) >>> 63 )

        [<Fact>]
        let GreaterThanTrue () =
            Assert.True(
                Rational(
                    Integer( [0xBADu; 0xDEADBEEFu;] ),
                    Natural( [2u] )
                ) > Rational(
                    Integer( [0xDEADBEEFu; 0xBADu], true ),
                    Natural( [2u] )
                )
            )

        [<Fact>]
        let GreaterThanFalseByLessThan () =
            Assert.False(
                Rational(
                    Integer( [0xBADu; 0xDEADBEEFu;] , true),
                    Natural( [2u] )
                ) > Rational(
                    Integer( [0xDEADBEEFu; 0xBADu] ),
                    Natural( [2u] )
                )
            )

        [<Fact>]
        let GreaterThanFalseByEquals () =
            Assert.False(
                Rational(
                    Integer( [0xBADu; 0xDEADBEEFu;] , true),
                    Natural( [2u] )
                ) > Rational(
                    Integer( [0xBADu; 0xDEADBEEFu;] , true),
                    Natural( [2u] )
                )

            )

        [<Fact>]
        let Addition () =
            Assert.Equal(
                Rational( Integer( 5 ), Natural( 4u ) ),
                Rational( Integer.Unit, Natural( 2u ) ) + Rational( Integer( 3 ), Natural( 4u ) )
            )

        [<Fact>]
        let Subtraction () =
            Assert.Equal(
                Rational( Integer.Unit, Natural( 4u ) ),
                Rational( Integer( 3 ), Natural( 4u ) ) - Rational( Integer.Unit, Natural( 2u ) )
            )

        [<Fact>]
        let Multiplication () =
            Assert.Equal(
                Rational( Integer( 3 ), Natural( 8u ) ),
                Rational( Integer.Unit, Natural( 2u ) ) * Rational( Integer( 3 ), Natural( 4u ) )
            )

        [<Fact>]
        let DivisionModulo () =
            Assert.Equal(
                (
                    Integer.Unit,
                    Rational( Integer.Unit, Natural( 2u ) )
                ),
                Rational( Integer( 3 ), Natural( 4u ) ) /% Rational( Integer.Unit, Natural( 2u ) )
            )

        [<Fact>]
        let ToString () =
            Assert.Equal( "-5 / 7", Rational( Integer( -5 ), Natural( 7u ) ).ToString() )

        [<Fact>]
        let Parse () =
            Assert.Equal(
                Rational( Integer( [0x112210F4u; 0x7DE98115u], true ), Natural( 41u ) ),
                Rational.Parse("-1234567890123456789 / 41")
            )

    //module And =
    //    [<Theory>]
    //    [<InlineData( "0", "0", "0")>]
    //    [<InlineData( "1", "0", "0")>]
    //    [<InlineData( "0", "1", "0")>]
    //    [<InlineData( "1", "1", "1")>]
    //    [<InlineData( "-1", "1", "1")>]
    //    [<InlineData( "1", "-1", "1")>]
    //    [<InlineData( "-1", "-1", "-1")>]
    //    let Sanity l r x =
    //        Assert.Equal( Rational.Parse(x), Rational.Parse(l) &&& Rational.Parse(r) )
    //
    //    [<Fact>]
    //    let BiggerLeft () =
    //        Assert.Equal( Rational.Unit, Rational([0xFu; 0x00000101u]) &&& Rational([ 0x00010001u]) )
    //
    //    [<Fact>]
    //    let BiggerRight () =
    //        Assert.Equal( Rational.Unit, Rational([0x00010001u]) &&& Rational([0xFu; 0x00000101u]) )
    //
    //module Or =
    //    [<Theory>]
    //    [<InlineData( "0", "0", "0")>]
    //    [<InlineData( "1", "0", "1")>]
    //    [<InlineData( "0", "1", "1")>]
    //    [<InlineData( "1", "1", "1")>]
    //    [<InlineData( "-1", "1", "-1")>]
    //    [<InlineData( "1", "-1", "-1")>]
    //    [<InlineData( "-1", "-1", "-1")>]
    //    let Sanity l r x =
    //        Assert.Equal( Rational.Parse(x), Rational.Parse(l) ||| Rational.Parse(r) )
    //
    //    [<Fact>]
    //    let BiggerLeft () =
    //        Assert.Equal( Rational([0xFu; 0x10101u]), Rational([0xFu; 0x00000101u]) ||| Rational([ 0x00010001u]) )
    //
    //    [<Fact>]
    //    let BiggerRight () =
    //        Assert.Equal( Rational([0xFu; 0x10101u]), Rational([0x00010001u]) ||| Rational([0xFu; 0x00000101u]) )
    //
    //module Xor =
    //    [<Theory>]
    //    [<InlineData( "0", "0", "0")>]
    //    [<InlineData( "1", "0", "1")>]
    //    [<InlineData( "0", "1", "1")>]
    //    [<InlineData( "1", "1", "0")>]
    //    [<InlineData( "-1", "0", "-1")>]
    //    [<InlineData( "0", "-1", "-1")>]
    //    [<InlineData( "-1", "-1", "0")>]
    //    [<InlineData( "-1", "1", "0")>]
    //    [<InlineData( "1", "-1", "0")>]
    //    [<InlineData( "1", "2", "3")>]
    //    [<InlineData( "-1", "2", "-3")>]
    //    [<InlineData( "1", "-2", "-3")>]
    //    [<InlineData( "-1", "-2", "3")>]
    //    let Sanity l r x =
    //        Assert.Equal( Rational.Parse(x), Rational.Parse(l) ^^^ Rational.Parse(r) )
    //
    //    [<Fact>]
    //    let BiggerLeft () =
    //        Assert.Equal( Rational([0xFu; 0x10100u]), Rational([0xFu; 0x00000101u]) ^^^ Rational([0x00010001u]) )
    //
    //    [<Fact>]
    //    let BiggerRight () =
    //        Assert.Equal( Rational([0xFu; 0x10100u]), Rational([0x00010001u]) ^^^ Rational([0xFu; 0x00000101u]) )
    //
    //module BitwiseNot =
    //    // NOTE: These are not going through Parse because it would be
    //    // more difficult to casually tell what is going on.
    //    [<Theory>]
    //    [<InlineData( 0xFFFFFFFEu, 1u)>]
    //    [<InlineData( 1u, 0xFFFFFFFEu)>]
    //    let Sanity r x =
    //        Assert.Equal( Rational([x], true), ~~~ Rational([r], false) )
    //
    //    [<Fact>]
    //    let Bigger () =
    //        Assert.Equal( Rational([0xF0123456u; 0x789ABCDEu], true), ~~~ Rational([0x0FEDCBA9u; 0x87654321u], false) )
    //
    //    [<Theory>]
    //    [<InlineData( true, false )>]
    //    [<InlineData( false, true )>]
    //    let Sign expected initial =
    //        Assert.Equal( Rational([0xF0123456u; 0x789ABCDEu], expected), ~~~ Rational([0x0FEDCBA9u; 0x87654321u], initial) )
    //
    //module LeftShift =
    //    [<Theory>]
    //    [<InlineData( "1", 1, "2" )>]
    //    [<InlineData( "-1", 1, "-2" )>]
    //    let Sanity l r s =
    //        Assert.Equal( Rational.Parse( s ), Rational.Parse( l ) <<< r )
    //  
    //    [<Fact>]
    //    let Multiple () =
    //        Assert.Equal( Rational([0x3Cu]), Rational([0xFu]) <<< 2 )
    //
    //    [<Fact>]
    //    let Overflow () =
    //        Assert.Equal( Rational([1u; 0xFFFFFFFEu]), Rational([0xFFFFFFFFu]) <<< 1 )
    //   
    //    [<Fact>]
    //    let MultipleOverflow () =
    //        Assert.Equal( Rational([0x5u; 0xFFFFFFF8u]), Rational([0xBFFFFFFFu]) <<< 3 )
    //    
    //    [<Fact>]
    //    let OverOneUInt () =
    //        Assert.Equal( Rational([8u; 0u; 0u]), Rational.Unit <<< 67 )
    //
    //    [<Fact>]
    //    let OverflowNegative () =
    //        Assert.Equal( Rational([1u; 0xFFFFFFFEu], true), Rational([0xFFFFFFFFu], true) <<< 1 )
    //        
    //    [<Fact>]
    //    let MultipleOverflowNegative () =
    //        Assert.Equal( Rational([0x5u; 0xFFFFFFF8u], true), Rational([0xBFFFFFFFu], true) <<< 3 )
    //       
    //    [<Fact>]
    //    let OverOneUIntNegative () =
    //        Assert.Equal( Rational([8u; 0u; 0u], true), Rational( Natural.Unit, true ) <<< 67 )
    //
    //module RightShift =
    //    [<Theory>]
    //    [<InlineData( "1", 1, "0" )>]
    //    [<InlineData( "15", 2, "3" )>]
    //    [<InlineData( "60", 2, "15" )>]
    //    [<InlineData( "60", 1, "30" )>]
    //    [<InlineData( "-1", 1, "0" )>]
    //    [<InlineData( "-2", 1, "-1" )>]
    //    [<InlineData( "-15", 2, "-3" )>]
    //    [<InlineData( "-60", 2, "-15" )>]
    //    [<InlineData( "-60", 1, "-30" )>]
    //    let Sanity l r s =
    //        Assert.Equal( Rational.Parse( s ), Rational.Parse( l ) >>> r )
    //
    //    [<Fact>]
    //    let Underflow () =
    //        Assert.Equal( Rational([0x7FFFFFFFu]), Rational([0xFFFFFFFFu]) >>> 1 )
    //   
    //    [<Fact>]
    //    let MultipleUnderflow () =
    //        Assert.Equal( Rational([0x1u; 0x5FFFFFFFu]), Rational([0xAu; 0xFFFFFFFFu]) >>> 3 )
    //    
    //    [<Fact>]
    //    let OverOneUInt () =
    //        Assert.Equal( Rational.Unit, Rational([0x10u; 0u; 0u]) >>> 68 )
    //
    //    [<Fact>]
    //    let ReduceToZero () =
    //        Assert.Equal( Rational.Zero, Rational([0x10u; 0u; 0u]) >>> 99 )
    //
    //    [<Fact>]
    //    let UnderflowNegative () =
    //        Assert.Equal( Rational([0x7FFFFFFFu], true), Rational([0xFFFFFFFFu], true) >>> 1 )
    //    
    //    [<Fact>]
    //    let MultipleUnderflowNegative () =
    //        Assert.Equal( Rational([0x1u; 0x5FFFFFFFu], true), Rational([0xAu; 0xFFFFFFFFu], true) >>> 3 )
    //    
    //    [<Fact>]
    //    let OverOneUIntNegative () =
    //        Assert.Equal( Rational( Natural.Unit, true ), Rational([0x10u; 0u; 0u], true) >>> 68 )
    //    
    //    [<Fact>]
    //    let RetainsNegative () =
    //        Assert.Equal( Rational([1u], true ), Rational([2u], true) >>> 1 )
    //
    //    [<Fact>]
    //    let NegativeReduceToZero () =
    //        Assert.Equal( Rational.Zero, Rational([0x10u; 0u; 0u], true) >>> 99 )
    
    module Equality =
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
        let Sanity (l:int32) (r:int32) (e:bool) =
            Assert.Equal( e, Rational( Integer( l ), Natural.Unit ) = Rational( Integer( r ), Natural.Unit ) )

        [<Fact>]
        let Big () =
            Assert.True(
                Rational([0x4u; 0x5u; 0x6u], [0x1u; 0x2u; 0x3u]) = Rational([0x4u; 0x5u; 0x6u], [0x1u; 0x2u; 0x3u])
            )
    
        [<Fact>]
        let BigNegative () =
            Assert.True(
                Rational( Integer( [0x4u; 0x5u; 0x6u], true ), Natural( [0x1u; 0x2u; 0x3u] ) ) = Rational( Integer( [0x4u; 0x5u; 0x6u], true ), Natural( [0x1u; 0x2u; 0x3u] ) )
            )
    
        [<Fact>]
        let BiggerLeftNumerator () =
            Assert.False( Rational([0xBADu; 0xDEADBEEFu], [20u] ) = Rational([0xDEADBEEFu], [20u] ) )
    
        [<Fact>]
        let BiggerRightNumerator () =
            Assert.False( Rational([0xDEADBEEFu], [20u]) = Rational([0xBADu; 0xDEADBEEFu], [20u]) )
        
        [<Fact>]
        let BiggerLeftDenominator () =
            Assert.False( Rational([20u], [0xBADu; 0xDEADBEEFu]) = Rational([20u], [0xDEADBEEFu]) )
        
        [<Fact>]
        let BiggerRightDenominator () =
            Assert.False( Rational([20u], [0xDEADBEEFu]) = Rational([20u], [0xBADu; 0xDEADBEEFu]) )
    
    module GreaterThan =
        [<Theory>]
        [<InlineData( " 0",    " 1",   false )>]
        [<InlineData( " 1",    " 0",   true )>]
        [<InlineData( " 0",    "-1",   true )>]
        [<InlineData( "-1",    " 0",   false )>]
        [<InlineData( " 1",    " 1",   false )>]
        [<InlineData( "-1",    " 1",   false )>]
        [<InlineData( " 1",    "-1",   true )>]
        [<InlineData( "-1",    "-1",   false )>]
        [<InlineData( " 1/2",  " 1/2", false )>]
        [<InlineData( "-1/2",  " 1/2", false )>]
        [<InlineData( " 1/2",  "-1/2", true )>]
        [<InlineData( "-1/2",  "-1/2", false )>]
        [<InlineData( " 1/3",  " 1/2", false )>]
        [<InlineData( "-1/3",  " 1/2", false )>]
        [<InlineData( " 1/3",  "-1/2", true )>]
        [<InlineData( "-1/3",  "-1/2", true )>]
        let Sanity l r x =
            Assert.Equal( x, Rational.Parse( l ) > Rational.Parse( r ) )
    
    module LessThan =
        [<Theory>]
        [<InlineData( " 0",    " 1",   true )>]
        [<InlineData( " 1",    " 0",   false )>]
        [<InlineData( " 0",    "-1",   false )>]
        [<InlineData( "-1",    " 0",   true )>]
        [<InlineData( " 1",    " 1",   false )>]
        [<InlineData( "-1",    " 1",   true )>]
        [<InlineData( " 1",    "-1",   false )>]
        [<InlineData( "-1",    "-1",   false )>]
        [<InlineData( " 1/2",  " 1/2", false )>]
        [<InlineData( "-1/2",  " 1/2", true )>]
        [<InlineData( " 1/2",  "-1/2", false )>]
        [<InlineData( "-1/2",  "-1/2", false )>]
        [<InlineData( " 1/3",  " 1/2", true )>]
        [<InlineData( "-1/3",  " 1/2", true )>]
        [<InlineData( " 1/3",  "-1/2", false )>]
        [<InlineData( "-1/3",  "-1/2", false )>]
        let Sanity l r x =
            Assert.Equal( x, Rational.Parse( l ) < Rational.Parse( r ) )

    module GreaterThanOrEqual =
        [<Theory>]
        [<InlineData( " 0",    " 1",   false )>]
        [<InlineData( " 1",    " 0",   true )>]
        [<InlineData( " 0",    "-1",   true )>]
        [<InlineData( "-1",    " 0",   false )>]
        [<InlineData( " 1",    " 1",   true )>]
        [<InlineData( "-1",    " 1",   false )>]
        [<InlineData( " 1",    "-1",   true )>]
        [<InlineData( "-1",    "-1",   true )>]
        [<InlineData( " 1/2",  " 1/2", true )>]
        [<InlineData( "-1/2",  " 1/2", false )>]
        [<InlineData( " 1/2",  "-1/2", true )>]
        [<InlineData( "-1/2",  "-1/2", true )>]
        [<InlineData( " 1/3",  " 1/2", false )>]
        [<InlineData( "-1/3",  " 1/2", false )>]
        [<InlineData( " 1/3",  "-1/2", true )>]
        [<InlineData( "-1/3",  "-1/2", true )>]
        let Sanity l r x =
            Assert.Equal( x, Rational.Parse( l ) >= Rational.Parse( r ) )

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
        [<InlineData( " 1/2",  " 1/2", true )>]
        [<InlineData( "-1/2",  " 1/2", true )>]
        [<InlineData( " 1/2",  "-1/2", false )>]
        [<InlineData( "-1/2",  "-1/2", true )>]
        [<InlineData( " 1/3",  " 1/2", true )>]
        [<InlineData( "-1/3",  " 1/2", true )>]
        [<InlineData( " 1/3",  "-1/2", false )>]
        [<InlineData( "-1/3",  "-1/2", false )>]
        let Sanity l r x =
            Assert.Equal( x, Rational.Parse( l ) <= Rational.Parse( r ) )

    module Inequality =
        [<Theory>]
        [<InlineData( " 0",    " 1",   true )>]
        [<InlineData( " 1",    " 0",   true )>]
        [<InlineData( " 0",    "-1",   true )>]
        [<InlineData( "-1",    " 0",   true )>]
        [<InlineData( " 1",    " 1",   false )>]
        [<InlineData( "-1",    " 1",   true )>]
        [<InlineData( " 1",    "-1",   true )>]
        [<InlineData( "-1",    "-1",   false )>]
        [<InlineData( " 1/2",  " 1/2", false )>]
        [<InlineData( "-1/2",  " 1/2", true )>]
        [<InlineData( " 1/2",  "-1/2", true )>]
        [<InlineData( "-1/2",  "-1/2", false )>]
        [<InlineData( " 1/3",  " 1/2", true )>]
        [<InlineData( "-1/3",  " 1/2", true )>]
        [<InlineData( " 1/3",  "-1/2", true )>]
        [<InlineData( "-1/3",  "-1/2", true )>]
        let Sanity l r x =
            Assert.Equal( x, Rational.Parse( l ) <> Rational.Parse( r ) )

    module Addition =
        [<Theory>]
        [<InlineData( " 1/2",   " 1/2",     " 1" )>]     // Sanity
        [<InlineData( "-1/2",   " 1/2",     " 0" )>]     // Sanity
        [<InlineData( " 1/2",   "-1/2",     " 0" )>]     // Sanity
        [<InlineData( "-1/2",   "-1/2",     "-1" )>]     // Sanity
        [<InlineData( " 1/2",   " 0",       " 1/2" )>]   // Sanity
        [<InlineData( "-1/2",   " 0",       "-1/2" )>]   // Sanity
        [<InlineData( " 0",     " 1/2",     " 1/2" )>]   // Sanity
        [<InlineData( " 0",     "-1/2",     "-1/2" )>]   // Sanity
        [<InlineData( " 1/6",   " 5/6",     " 1" )>]     // l < r
        [<InlineData( "-1/6",   " 5/6",     " 2/3" )>]   // l < r
        [<InlineData( " 1/6",   "-5/6",     "-2/3" )>]   // l < r
        [<InlineData( "-1/6",   "-5/6",     "-1" )>]     // l < r
        [<InlineData( " 9/11",  " 2/11",    " 1" )>]     // l > r
        [<InlineData( "-9/11",  " 2/11",    "-7/11" )>]  // l > r
        [<InlineData( " 9/11",  "-2/11",    " 7/11" )>]  // l > r
        [<InlineData( "-9/11",  "-2/11",    "-1" )>]     // l > r
        let Sanity l r s =
            Assert.Equal( Rational.Parse( s ), Rational.Parse( l ) + Rational.Parse( r ) )
    
        [<Fact>]
        let DifferentDenominator () =
            Assert.Equal(
                Rational.Parse( "5/6" ),
                Rational.Parse( "1/2" ) + Rational.Parse( "1/3" )
            )

    module Subtraction =
        [<Theory>]
        [<InlineData( " 1/2",   " 1/2",     " 0" )>]     // Sanity
        [<InlineData( "-1/2",   " 1/2",     "-1" )>]     // Sanity
        [<InlineData( " 1/2",   "-1/2",     " 1" )>]     // Sanity
        [<InlineData( "-1/2",   "-1/2",     " 0" )>]     // Sanity
        [<InlineData( " 1/2",   " 0",       " 1/2" )>]   // Sanity
        [<InlineData( "-1/2",   " 0",       "-1/2" )>]   // Sanity
        [<InlineData( " 0",     " 1/2",     "-1/2" )>]   // Sanity
        [<InlineData( " 0",     "-1/2",     " 1/2" )>]   // Sanity
        [<InlineData( " 1/6",   " 5/6",     "-2/3" )>]   // l < r
        [<InlineData( "-1/6",   " 5/6",     "-1" )>]     // l < r
        [<InlineData( " 1/6",   "-5/6",     " 1" )>]     // l < r
        [<InlineData( "-1/6",   "-5/6",     " 2/3" )>]   // l < r
        [<InlineData( " 9/11",  " 2/11",    " 7/11" )>]  // l > r
        [<InlineData( "-9/11",  " 2/11",    "-1" )>]     // l > r
        [<InlineData( " 9/11",  "-2/11",    " 1" )>]     // l > r
        [<InlineData( "-9/11",  "-2/11",    "-7/11" )>]  // l > r
        let Sanity l r s =
            Assert.Equal( Rational.Parse( s ), Rational.Parse( l ) - Rational.Parse( r ) )
    
        [<Fact>]
        let DifferentDenominator () =
            Assert.Equal(
                Rational.Parse( "1/6" ),
                Rational.Parse( "1/2" ) - Rational.Parse( "1/3" )
            )

    module Multiply =
        [<Theory>]
        [<InlineData( " 1/2",  " 1",    " 1/2" )>]      // Sanity
        [<InlineData( "-1/2",  " 1",    "-1/2" )>]      // Sanity
        [<InlineData( " 1/2",  "-1",    "-1/2" )>]      // Sanity
        [<InlineData( "-1/2",  "-1",    " 1/2" )>]      // Sanity
        [<InlineData( " 1  ",  " 1/2",  " 1/2" )>]      // Sanity
        [<InlineData( "-1  ",  " 1/2",  "-1/2" )>]      // Sanity
        [<InlineData( " 1  ",  "-1/2",  "-1/2" )>]      // Sanity
        [<InlineData( "-1  ",  "-1/2",  " 1/2" )>]      // Sanity
        [<InlineData( " 1/2",  " 0",    " 0" )>]        // Sanity
        [<InlineData( "-1/2",  " 0",    " 0" )>]        // Sanity
        [<InlineData( " 0",    " 1/2",  " 0" )>]        // Sanity
        [<InlineData( " 0",    "-1/2",  " 0" )>]        // Sanity
        [<InlineData( " 6/11", " 7/13", " 42/143" )>]   // multiple bits
        [<InlineData( "-6/11", " 7/13", "-42/143" )>]   // multiple bits
        [<InlineData( " 6/11", "-7/13", "-42/143" )>]   // multiple bits
        [<InlineData( "-6/11", "-7/13", " 42/143" )>]   // multiple bits
        let Sanity l r p =
            Assert.Equal( Rational.Parse( p ), Rational.Parse( l ) * Rational.Parse( r ) )

        [<Fact>]
        let Inverse () =
            Assert.Equal(
                Rational.Parse( "1" ),
                Rational.Parse( "1/2" ) * Rational.Parse( "6/3" )
            )

        [<Fact>]
        let Associtivity () =
            Assert.Equal(
                ( Rational.Parse( "1/2" ) * Rational.Parse( "3/4" ) ) * Rational.Parse( "5/6" ),
                Rational.Parse( "1/2" ) * ( Rational.Parse( "3/4" ) * Rational.Parse( "5/6" ) )
            )

        [<Fact>]
        let Communtivity () =
            Assert.Equal(
                Rational.Parse( "1/2" ) * Rational.Parse( "3/4" ),
                Rational.Parse( "3/4" ) * Rational.Parse( "1/2" )
            )

        [<Fact>]
        let Distributive () =
            Assert.Equal(
                ( Rational.Parse( "1/2" ) * Rational.Parse( "3/4" ) ) + ( Rational.Parse( "1/2" ) * Rational.Parse( "5/6" ) ),
                Rational.Parse( "1/2" ) * ( Rational.Parse( "3/4" ) + Rational.Parse( "5/6" ) )
            )

    module Division =
        [<Theory>]
        [<InlineData( " 1/2",  " 1",    " 1/2" )>]      // Sanity
        [<InlineData( "-1/2",  " 1",    "-1/2" )>]      // Sanity
        [<InlineData( " 1/2",  "-1",    "-1/2" )>]      // Sanity
        [<InlineData( "-1/2",  "-1",    " 1/2" )>]      // Sanity
        [<InlineData( " 1  ",  " 1/2",  " 2" )>]        // Sanity
        [<InlineData( "-1  ",  " 1/2",  "-2" )>]        // Sanity
        [<InlineData( " 1  ",  "-1/2",  "-2" )>]        // Sanity
        [<InlineData( "-1  ",  "-1/2",  " 2" )>]        // Sanity
        [<InlineData( " 0",    " 1/2",  " 0" )>]        // Sanity
        [<InlineData( " 0",    "-1/2",  " 0" )>]        // Sanity
        [<InlineData( " 6/11", " 7/13", " 78/77" )>]    // multiple bits
        [<InlineData( "-6/11", " 7/13", "-78/77" )>]    // multiple bits
        [<InlineData( " 6/11", "-7/13", "-78/77" )>]    // multiple bits
        [<InlineData( "-6/11", "-7/13", " 78/77" )>]    // multiple bits
        let Sanity dividend divisor quotient =
            Assert.Equal( Rational.Parse( quotient ), Rational.Parse( dividend ) / Rational.Parse( divisor ) )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Rational.Unit / Rational.Zero ) |> ignore )

        [<Fact>]
        let Inverse () =
            Assert.Equal(
                Rational.Parse( "1" ),
                Rational.Parse( "1/2" ) / Rational.Parse( "1/2" )
            )

    module Modulo =
        [<Theory>]
        [<InlineData( " 1/2",  " 1",    " 1/2" )>]      // Sanity
        [<InlineData( "-1/2",  " 1",    "-1/2" )>]      // Sanity
        [<InlineData( " 1/2",  "-1",    "-1/2" )>]      // Sanity
        [<InlineData( "-1/2",  "-1",    " 1/2" )>]      // Sanity
        [<InlineData( " 1  ",  " 1/2",  " 0" )>]        // Sanity
        [<InlineData( "-1  ",  " 1/2",  " 0" )>]        // Sanity
        [<InlineData( " 1  ",  "-1/2",  " 0" )>]        // Sanity
        [<InlineData( "-1  ",  "-1/2",  " 0" )>]        // Sanity
        [<InlineData( " 0",    " 1/2",  " 0" )>]        // Sanity
        [<InlineData( " 0",    "-1/2",  " 0" )>]        // Sanity
        [<InlineData( " 6/11", " 7/13", " 1/77" )>]     // multiple bits
        [<InlineData( "-6/11", " 7/13", "-1/77" )>]     // multiple bits
        [<InlineData( " 6/11", "-7/13", "-1/77" )>]     // multiple bits
        [<InlineData( "-6/11", "-7/13", " 1/77" )>]     // multiple bits
        let Sanity dividend divisor remainder =
            Assert.Equal(
                Rational.Parse( remainder ),
                Rational.Parse( dividend ) % Rational.Parse( divisor )
            )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Rational.Unit % Rational.Zero ) |> ignore )

        [<Fact>]
        let Inverse () =
            Assert.Equal(
                Rational.Parse( "0" ),
                Rational.Parse( "1/2" ) % Rational.Parse( "1/2" )
            )

    module DivisionModulo =
        [<Theory>]
        [<InlineData( " 1/2",  " 1",    " 0", " 1/2" )>]      // Sanity
        [<InlineData( "-1/2",  " 1",    " 0", "-1/2" )>]      // Sanity
        [<InlineData( " 1/2",  "-1",    " 0", "-1/2" )>]      // Sanity
        [<InlineData( "-1/2",  "-1",    " 0", " 1/2" )>]      // Sanity
        [<InlineData( " 1  ",  " 1/2",  " 2", " 0" )>]        // Sanity
        [<InlineData( "-1  ",  " 1/2",  "-2", " 0" )>]        // Sanity
        [<InlineData( " 1  ",  "-1/2",  "-2", " 0" )>]        // Sanity
        [<InlineData( "-1  ",  "-1/2",  " 2", " 0" )>]        // Sanity
        [<InlineData( " 0",    " 1/2",  " 0", " 0" )>]        // Sanity
        [<InlineData( " 0",    "-1/2",  " 0", " 0" )>]        // Sanity
        [<InlineData( " 6/11", " 7/13", " 1", " 1/77" )>]     // multiple bits
        [<InlineData( "-6/11", " 7/13", "-1", "-1/77" )>]     // multiple bits
        [<InlineData( " 6/11", "-7/13", "-1", "-1/77" )>]     // multiple bits
        [<InlineData( "-6/11", "-7/13", " 1", " 1/77" )>]     // multiple bits
        let Sanity dividend divisor quotient remainder =
            Assert.Equal(
                ( Integer.Parse( quotient ), Rational.Parse( remainder ) ),
                Rational.Parse( dividend ) /% Rational.Parse( divisor )
            )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Rational.Unit /% Rational.Zero ) |> ignore )

        [<Fact>]
        let Inverse () =
            Assert.Equal(
                ( Integer.Parse( "1" ), Rational.Parse( "0" ) ),
                Rational.Parse( "1/2" ) /% Rational.Parse( "1/2" )
            )

    module Negation =
        [<Theory>]
        [<InlineData(  1, 1u, -1, 1u )>] // Sanity
        [<InlineData( -1, 1u,  1, 1u )>] // Sanity
        [<InlineData(  0, 1u,  0, 1u )>] // Sanity
        let Sanity (n:int) (d:uint) (en:int) (ed:uint) =
            Assert.Equal(
                Rational( Integer( en ), Natural( ed ) ),
                -Rational( Integer( n ), Natural( d ) )
            )

        [<Fact>]
        let Big () =
            Assert.Equal(
                Rational( Integer( [0xFEDCBA9u; 0x76543210u], true ), Natural.Unit ),
                -Rational( Integer( [0xFEDCBA9u; 0x76543210u], false ), Natural.Unit )
            )

        [<Fact>]
        let BigNegative () =
            Assert.Equal(
                Rational( Integer( [0xFEDCBA9u; 0x76543210u], false ), Natural.Unit ),
                -Rational( Integer( [0xFEDCBA9u; 0x76543210u], true ), Natural.Unit )
            )

        [<Fact>]
        let Small () =
            Assert.Equal(
                Rational( -Integer.Unit, Natural( [0xFEDCBA9u; 0x76543210u] ) ),
                -Rational( Integer.Unit, Natural( [0xFEDCBA9u; 0x76543210u] ) )
            )

        [<Fact>]
        let SmallNegative () =
            Assert.Equal(
                Rational( Integer.Unit, Natural( [0xFEDCBA9u; 0x76543210u] ) ),
                -Rational( -Integer.Unit, Natural( [0xFEDCBA9u; 0x76543210u] ) )
            )

    module Equals =
        [<Theory>]
        [<InlineData(  1,  1, true )>]    // Sanity
        [<InlineData(  1, -1, false )>]   // Sanity
        [<InlineData( -1,  1, false )>]   // Sanity
        [<InlineData( -1, -1, true )>]    // Sanity
        let Sanity (l:int32) (r:int32) (e:bool) =
            Assert.Equal( e, Rational( Integer( l ), Natural.Unit ).Equals( Rational( Integer( r ), Natural.Unit ) ) )
    
        [<Fact>]
        let NaturalEquals () =
            Assert.True( Rational.Unit.Equals( Natural.Unit ) )
    
        [<Fact>]
        let NaturalNotEquals () =
            Assert.False( Rational.Unit.Equals( Natural.Zero ) )
    
        [<Fact>]
        let NaturalSignNotEquals () =
            Assert.False( (-Rational.Unit).Equals( Natural.Unit ) )

        [<Fact>]
        let IntegerEquals () =
            Assert.True( Rational.Unit.Equals( Integer.Unit ) )
    
        [<Fact>]
        let IntegerNotEquals () =
            Assert.False( Rational.Unit.Equals( Integer.Zero ) )
    
        [<Fact>]
        let IntegerSignNotEquals () =
            Assert.False( (-Rational.Unit).Equals( Integer.Unit ) )
    
    module ToString =
        [<Theory>]
        [<InlineData(  0, 1u, "0" )>]
        [<InlineData(  1, 1u, "1" )>]
        [<InlineData( -1, 1u, "-1" )>]
        [<InlineData(  1, 2u, "1 / 2" )>]
        [<InlineData( -1, 2u, "-1 / 2" )>]
        [<InlineData(  5, 2u, "5 / 2" )>]
        [<InlineData(  41, 15526u, "41 / 15526" )>]
        let Sanity (num:int) (denom:uint) expected =
            Assert.Equal( expected, Rational( Integer( num ), Natural( denom ) ).ToString() )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal(
                "1234567890123456789",
                Rational( Integer( [ 0x112210F4u; 0x7DE98115u ] ), Natural.Unit ).ToString()
            )
    
        [<Fact>]
        let BiggerNegative () =
            Assert.Equal(
                "-1234567890123456789",
                Rational( Integer( [ 0x112210F4u; 0x7DE98115u ], true ), Natural.Unit ).ToString()
            )

        [<Fact>]
        let Smaller () =
            Assert.Equal(
                "1 / 1234567890123456789",
                Rational( Integer.Unit, Natural( [ 0x112210F4u; 0x7DE98115u ] ) ).ToString()
            )
    
        [<Fact>]
        let SmallerNegative () =
            Assert.Equal(
                "-1 / 1234567890123456789",
                Rational( -Integer.Unit, Natural( [ 0x112210F4u; 0x7DE98115u ] ) ).ToString()
            )
    
    module Parse =
        [<Theory>]
        [<InlineData( "0",       0, 1u )>]        // Sanity
        [<InlineData( "1",       1, 1u )>]        // Sanity
        [<InlineData( "-1",     -1, 1u )>]        // Sanity
        [<InlineData( "1/2",     1, 2u )>]        // Sanity
        [<InlineData( "1 /2",    1, 2u )>]        // Sanity
        [<InlineData( "1/ 2",    1, 2u )>]        // Sanity
        [<InlineData( "1 / 2",   1, 2u )>]        // Sanity
        [<InlineData( "-1 / 2", -1, 2u )>]        // Sanity
        [<InlineData( "1 / -2", -1, 2u )>]        // Sanity
        [<InlineData( "-1 / -2", 1, 2u )>]        // Sanity
        [<InlineData( "0 / -2",  0, 2u )>]        // Sanity
        let Sanity (str:string) (n:int32) (d:uint32) =
            Assert.Equal( Rational(n, d), Rational.Parse(str) )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal(
                Rational(
                    Integer( [ 0x112210F4u; 0x7DE98115u ] ),
                    Natural( 41u )
                ),
                Rational.Parse("1234567890123456789/41")
            )
    
        [<Fact>]
        let BiggerNegative () =
            Assert.Equal(
                Rational(
                    Integer( [ 0x112210F4u; 0x7DE98115u ], true ),
                    Natural( 41u )
                ),
                Rational.Parse("-1234567890123456789/41")
            )

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
                Record.Exception( fun () -> Rational.Parse( str ) |> ignore )
            )

        [<Fact>]
        let FormatEx () =
            Assert.IsType<System.FormatException>(
                Record.Exception( fun () -> Rational.Parse( "1/2/3" ) |> ignore )
            )
