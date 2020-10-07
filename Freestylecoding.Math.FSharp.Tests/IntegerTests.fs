namespace Freestylecoding.Math.FSharp.Tests

open Xunit
open Freestylecoding.Math

module Integer =
    // NOTE: The sanity tests are here to do low level tests of a few parts
    // This lets me isolate those tests so I can use those parts in the rest of the tests
    //
    // In other words:
    // IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
    [<Trait( "Type", "Sanity" )>]
    module Sanity =
        [<Fact>]
        let LeftShift () =
            Assert.Equal( Integer([4u; 0x8000_0000u; 0u]), Integer([9u]) <<< 63 )

        [<Fact>]
        let RightShift () =
            Assert.Equal( Integer([9u]), Integer([4u; 0x8000_0000u; 0u]) >>> 63 )

        [<Fact>]
        let GreaterThanTrue () =
            Assert.True( Integer([0xDEADBEEFu; 0xBADu]) > Integer([0xBADu; 0xDEADBEEFu;]) )

        [<Fact>]
        let GreaterThanFalseByLessThan () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu]) > Integer([0xDEADBEEFu; 0xBADu]) )

        [<Fact>]
        let GreaterThanFalseByEquals () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu]) > Integer([0xBADu; 0xDEADBEEFu]) )

        [<Fact>]
        let Addition () =
            Assert.Equal( Integer([2u; 0u; 0u]), Integer([1u; 1u]) + Integer([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue]) )

        [<Fact>]
        let Subtraction () =
            Assert.Equal( Integer ([0xFFFF_FFFFu; 0xFFFF_FFFFu]), Integer([1u; 0u; 0u]) - Integer([1u]) )

        [<Fact>]
        let Multiplication () =
            Assert.Equal( Integer([0x75CD9046u; 0x541D5980u]), Integer([0xFEDCBA98u]) * Integer([0x76543210u]) )

        [<Fact>]
        let DivisionModulo () =
            Assert.Equal( (Integer([0xFEDCBA98u]),Integer([0x12345678u])), Integer([0x75CD9046u; 0x6651AFF8u]) /% Integer([0x76543210u]) )

        [<Fact>]
        let ToString () =
            Assert.Equal( "1234567890123456789", Integer([0x112210F4u; 0x7DE98115u]).ToString() )

        [<Fact>]
        let Parse () =
            Assert.Equal( Integer([0x112210F4u; 0x7DE98115u] ), Integer.Parse("1234567890123456789") )

    module And =
        [<Theory>]
        [<InlineData( "0", "0", "0")>]
        [<InlineData( "1", "0", "0")>]
        [<InlineData( "0", "1", "0")>]
        [<InlineData( "1", "1", "1")>]
        [<InlineData( "-1", "1", "1")>]
        [<InlineData( "1", "-1", "1")>]
        [<InlineData( "-1", "-1", "-1")>]
        let Sanity l r x =
            Assert.Equal( Integer.Parse(x), Integer.Parse(l) &&& Integer.Parse(r) )

        [<Fact>]
        let BiggerLeft () =
            Assert.Equal( Integer.Unit, Integer([0xFu; 0x00000101u]) &&& Integer([ 0x00010001u]) )

        [<Fact>]
        let BiggerRight () =
            Assert.Equal( Integer.Unit, Integer([0x00010001u]) &&& Integer([0xFu; 0x00000101u]) )

    module Or =
        [<Theory>]
        [<InlineData( "0", "0", "0")>]
        [<InlineData( "1", "0", "1")>]
        [<InlineData( "0", "1", "1")>]
        [<InlineData( "1", "1", "1")>]
        [<InlineData( "-1", "1", "-1")>]
        [<InlineData( "1", "-1", "-1")>]
        [<InlineData( "-1", "-1", "-1")>]
        let Sanity l r x =
            Assert.Equal( Integer.Parse(x), Integer.Parse(l) ||| Integer.Parse(r) )

        [<Fact>]
        let BiggerLeft () =
            Assert.Equal( Integer([0xFu; 0x10101u]), Integer([0xFu; 0x00000101u]) ||| Integer([ 0x00010001u]) )

        [<Fact>]
        let BiggerRight () =
            Assert.Equal( Integer([0xFu; 0x10101u]), Integer([0x00010001u]) ||| Integer([0xFu; 0x00000101u]) )

    module Xor =
        [<InlineData( "0", "0", "0")>]
        [<InlineData( "1", "0", "1")>]
        [<InlineData( "0", "1", "1")>]
        [<InlineData( "1", "1", "0")>]
        [<InlineData( "-1", "0", "-1")>]
        [<InlineData( "0", "-1", "-1")>]
        [<InlineData( "-1", "-1", "0")>]
        [<InlineData( "-1", "1", "0")>]
        [<InlineData( "1", "-1", "0")>]
        [<InlineData( "1", "2", "3")>]
        [<InlineData( "-1", "2", "-3")>]
        [<InlineData( "1", "-2", "-3")>]
        [<InlineData( "-1", "-2", "3")>]
        let Sanity l r x =
            Assert.Equal( Integer.Parse(x), Integer.Parse(l) ^^^ Integer.Parse(r) )

        [<Fact>]
        let BiggerLeft () =
            Assert.Equal( Integer([0xFu; 0x10100u]), Integer([0xFu; 0x00000101u]) ^^^ Integer([0x00010001u]) )

        [<Fact>]
        let BiggerRight () =
            Assert.Equal( Integer([0xFu; 0x10100u]), Integer([0x00010001u]) ^^^ Integer([0xFu; 0x00000101u]) )

    module BitwiseNot =
        // NOTE: These are not going through Parse because it would be
        // more difficult to casually tell what is going on.
        [<Theory>]
        [<InlineData( 0xFFFFFFFEu, 1u)>]
        [<InlineData( 1u, 0xFFFFFFFEu)>]
        let Sanity r x =
            Assert.Equal( Integer([x], true), ~~~ Integer([r], false) )

        [<Fact>]
        let Bigger () =
            Assert.Equal( Integer([0xF0123456u; 0x789ABCDEu], true), ~~~ Integer([0x0FEDCBA9u; 0x87654321u], false) )

        [<Theory>]
        [<InlineData( true, false )>]
        [<InlineData( false, true )>]
        let Sign expected initial =
            Assert.Equal( Integer([0xF0123456u; 0x789ABCDEu], expected), ~~~ Integer([0x0FEDCBA9u; 0x87654321u], initial) )

    module LeftShift =
        [<Theory>]
        [<InlineData( "1", 1, "2" )>]
        [<InlineData( "-1", 1, "-2" )>]
        let Sanity l r s =
            Assert.Equal( Integer.Parse( s ), Integer.Parse( l ) <<< r )
        
        [<Fact>]
        let Multiple () =
            Assert.Equal( Integer([0x3Cu]), Integer([0xFu]) <<< 2 )

        [<Fact>]
        let Overflow () =
            Assert.Equal( Integer([1u; 0xFFFFFFFEu]), Integer([0xFFFFFFFFu]) <<< 1 )
        
        [<Fact>]
        let MultipleOverflow () =
            Assert.Equal( Integer([0x5u; 0xFFFFFFF8u]), Integer([0xBFFFFFFFu]) <<< 3 )
        
        [<Fact>]
        let OverOneUInt () =
            Assert.Equal( Integer([8u; 0u; 0u]), Integer.Unit <<< 67 )

        [<Fact>]
        let OverflowNegative () =
            Assert.Equal( Integer([1u; 0xFFFFFFFEu], true), Integer([0xFFFFFFFFu], true) <<< 1 )
            
        [<Fact>]
        let MultipleOverflowNegative () =
            Assert.Equal( Integer([0x5u; 0xFFFFFFF8u], true), Integer([0xBFFFFFFFu], true) <<< 3 )
            
        [<Fact>]
        let OverOneUIntNegative () =
            Assert.Equal( Integer([8u; 0u; 0u], true), Integer( Natural.Unit, true ) <<< 67 )

    module RightShift =
        [<Theory>]
        [<InlineData( "1", 1, "0" )>]
        [<InlineData( "15", 2, "3" )>]
        [<InlineData( "60", 2, "15" )>]
        [<InlineData( "60", 1, "30" )>]
        [<InlineData( "-1", 1, "0" )>]
        [<InlineData( "-2", 1, "-1" )>]
        [<InlineData( "-15", 2, "-3" )>]
        [<InlineData( "-60", 2, "-15" )>]
        [<InlineData( "-60", 1, "-30" )>]
        let Sanity l r s =
            Assert.Equal( Integer.Parse( s ), Integer.Parse( l ) >>> r )

        [<Fact>]
        let Underflow () =
            Assert.Equal( Integer([0x7FFFFFFFu]), Integer([0xFFFFFFFFu]) >>> 1 )
        
        [<Fact>]
        let MultipleUnderflow () =
            Assert.Equal( Integer([0x1u; 0x5FFFFFFFu]), Integer([0xAu; 0xFFFFFFFFu]) >>> 3 )
        
        [<Fact>]
        let OverOneUInt () =
            Assert.Equal( Integer.Unit, Integer([0x10u; 0u; 0u]) >>> 68 )

        [<Fact>]
        let ReduceToZero () =
            Assert.Equal( Integer.Zero, Integer([0x10u; 0u; 0u]) >>> 99 )

        [<Fact>]
        let UnderflowNegative () =
            Assert.Equal( Integer([0x7FFFFFFFu], true), Integer([0xFFFFFFFFu], true) >>> 1 )
        
        [<Fact>]
        let MultipleUnderflowNegative () =
            Assert.Equal( Integer([0x1u; 0x5FFFFFFFu], true), Integer([0xAu; 0xFFFFFFFFu], true) >>> 3 )
        
        [<Fact>]
        let OverOneUIntNegative () =
            Assert.Equal( Integer( Natural.Unit, true ), Integer([0x10u; 0u; 0u], true) >>> 68 )
        
        [<Fact>]
        let RetainsNegative () =
            Assert.Equal( Integer([1u], true ), Integer([2u], true) >>> 1 )

        [<Fact>]
        let NegativeReduceToZero () =
            Assert.Equal( Integer.Zero, Integer([0x10u; 0u; 0u], true) >>> 99 )

    module Equality =
        [<Theory>]
        [<InlineData( "0", "0", true )>]
        [<InlineData( "0", "1", false )>]
        [<InlineData( "1", "0", false )>]
        [<InlineData( "1", "1", true )>]
        [<InlineData( "0", "-1", false )>]
        [<InlineData( "-1", "0", false )>]
        [<InlineData( "-1", "1", false )>]
        [<InlineData( "1", "-1", false )>]
        [<InlineData( "-1", "-1", true )>]
        let Sanity l r x =
            Assert.Equal( x, Integer.Parse( l ) = Integer.Parse( r ) )

        [<Fact>]
        let BiggerLeft () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu]) = Integer([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.False( Integer([0xDEADBEEFu]) = Integer([0xBADu; 0xDEADBEEFu]) )

    module GreaterThan =
        [<Theory>]
        [<InlineData( "0", "1", false )>]
        [<InlineData( "1", "0", true )>]
        [<InlineData( "0", "-1", true )>]
        [<InlineData( "-1", "0", false )>]
        [<InlineData( "1", "1", false )>]
        [<InlineData( "-1", "1", false )>]
        [<InlineData( "1", "-1", true )>]
        [<InlineData( "-1", "-1", false )>]
        let Sanity l r x =
            Assert.Equal( x, Integer.Parse( l ) > Integer.Parse( r ) )

        [<Fact>]
        let BiggerLeft () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu]) > Integer([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.False( Integer([0xDEADBEEFu]) > Integer([0xBADu; 0xDEADBEEFu]) )
    
        [<Fact>]
        let CascadeGreaterThan () =
            Assert.True( Integer([1u; 1u]) > Integer([1u; 0u]) )
    
        [<Fact>]
        let CascadeEqual () =
            Assert.False( Integer([1u; 0u]) > Integer([1u; 0u]) )
    
        [<Fact>]
        let CascadeLessThan () =
            Assert.False( Integer([1u; 0u]) > Integer([1u; 1u]) )

        [<Fact>]
        let BiggerLeftNegative () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) > Integer([0xDEADBEEFu], true) )

        [<Fact>]
        let BiggerRightNegative () =
            Assert.True( Integer([0xDEADBEEFu], true) > Integer([0xBADu; 0xDEADBEEFu], true) )
    
        [<Fact>]
        let CascadeGreaterThanNegative () =
            Assert.False( Integer([1u; 1u], true) > Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeEqualNegative () =
            Assert.False( Integer([1u; 0u], true) > Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeLessThanNegative () =
            Assert.True( Integer([1u; 0u], true) > Integer([1u; 1u],true) )

        [<Fact>]
        let BiggerLeftMixedNegativeLeft () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) > Integer([0xDEADBEEFu], false) )

        [<Fact>]
        let BiggerRightMixedNegativeLeft () =
            Assert.False( Integer([0xDEADBEEFu], true) > Integer([0xBADu; 0xDEADBEEFu], false) )
    
        [<Fact>]
        let CascadeGreaterThanMixedNegativeLeft () =
            Assert.False( Integer([1u; 1u], true) > Integer([1u; 0u], false) )
    
        [<Fact>]
        let CascadeEqualMixedNegativeLeft () =
            Assert.False( Integer([1u; 0u], true) > Integer([1u; 0u], false) )
    
        [<Fact>]
        let CascadeLessThanMixedNegativeLeft () =
            Assert.False( Integer([1u; 0u], true) > Integer([1u; 1u], false) )

        [<Fact>]
        let BiggerLeftMixedNegativeRight () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu], false) > Integer([0xDEADBEEFu], true) )

        [<Fact>]
        let BiggerRightMixedNegativeRight () =
            Assert.True( Integer([0xDEADBEEFu], false) > Integer([0xBADu; 0xDEADBEEFu], true) )
    
        [<Fact>]
        let CascadeGreaterThanMixedNegativeRight () =
            Assert.True( Integer([1u; 1u], false) > Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeEqualMixedNegativeRight () =
            Assert.True( Integer([1u; 0u], false) > Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeLessThanMixedNegativeRight () =
            Assert.True( Integer([1u; 0u], false) > Integer([1u; 1u], true) )

    module LessThan =
        [<Theory>]
        [<InlineData( "0", "1", true )>]
        [<InlineData( "1", "0", false )>]
        [<InlineData( "0", "-1", false )>]
        [<InlineData( "-1", "0", true )>]
        [<InlineData( "1", "1", false )>]
        [<InlineData( "-1", "1", true )>]
        [<InlineData( "1", "-1", false )>]
        [<InlineData( "-1", "-1", false )>]
        let Sanity l r x =
            Assert.Equal( x, Integer.Parse( l ) < Integer.Parse( r ) )

        [<Fact>]
        let BiggerLeft () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu]) < Integer([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.True( Integer([0xDEADBEEFu]) < Integer([0xBADu; 0xDEADBEEFu]) )
    
        [<Fact>]
        let CascadeGreaterThan () =
            Assert.False( Integer([1u; 1u]) < Integer([1u; 0u]) )
    
        [<Fact>]
        let CascadeEqual () =
            Assert.False( Integer([1u; 0u]) < Integer([1u; 0u]) )
    
        [<Fact>]
        let CascadeLessThan () =
            Assert.True( Integer([1u; 0u]) < Integer([1u; 1u]) )

        [<Fact>]
        let BiggerLeftNegative () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu], true) < Integer([0xDEADBEEFu], true) )

        [<Fact>]
        let BiggerRightNegative () =
            Assert.False( Integer([0xDEADBEEFu], true) < Integer([0xBADu; 0xDEADBEEFu], true) )
    
        [<Fact>]
        let CascadeGreaterThanNegative () =
            Assert.True( Integer([1u; 1u], true) < Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeEqualNegative () =
            Assert.False( Integer([1u; 0u], true) < Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeLessThanNegative () =
            Assert.False( Integer([1u; 0u], true) < Integer([1u; 1u],true) )

        [<Fact>]
        let BiggerLeftMixedNegativeLeft () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu], true) < Integer([0xDEADBEEFu], false) )

        [<Fact>]
        let BiggerRightMixedNegativeLeft () =
            Assert.True( Integer([0xDEADBEEFu], true) < Integer([0xBADu; 0xDEADBEEFu], false) )
    
        [<Fact>]
        let CascadeGreaterThanMixedNegativeLeft () =
            Assert.True( Integer([1u; 1u], true) < Integer([1u; 0u], false) )
    
        [<Fact>]
        let CascadeEqualMixedNegativeLeft () =
            Assert.True( Integer([1u; 0u], true) < Integer([1u; 0u], false) )
    
        [<Fact>]
        let CascadeLessThanMixedNegativeLeft () =
            Assert.True( Integer([1u; 0u], true) < Integer([1u; 1u], false) )

        [<Fact>]
        let BiggerLeftMixedNegativeRight () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu], false) < Integer([0xDEADBEEFu], true) )

        [<Fact>]
        let BiggerRightMixedNegativeRight () =
            Assert.False( Integer([0xDEADBEEFu], false) < Integer([0xBADu; 0xDEADBEEFu], true) )
    
        [<Fact>]
        let CascadeGreaterThanMixedNegativeRight () =
            Assert.False( Integer([1u; 1u], false) < Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeEqualMixedNegativeRight () =
            Assert.False( Integer([1u; 0u], false) < Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeLessThanMixedNegativeRight () =
            Assert.False( Integer([1u; 0u], false) < Integer([1u; 1u], true) )

    module GreaterThanOrEqual =
        [<Theory>]
        [<InlineData( "0", "1", false )>]
        [<InlineData( "1", "0", true )>]
        [<InlineData( "0", "-1", true )>]
        [<InlineData( "-1", "0", false )>]
        [<InlineData( "1", "1", true )>]
        [<InlineData( "-1", "1", false )>]
        [<InlineData( "1", "-1", true )>]
        [<InlineData( "-1", "-1", true )>]
        let Sanity l r x =
            Assert.Equal( x, Integer.Parse( l ) >= Integer.Parse( r ) )

        [<Fact>]
        let BiggerLeft () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu]) >= Integer([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.False( Integer([0xDEADBEEFu]) >= Integer([0xBADu; 0xDEADBEEFu]) )
    
        [<Fact>]
        let CascadeGreaterThan () =
            Assert.True( Integer([1u; 1u]) >= Integer([1u; 0u]) )
    
        [<Fact>]
        let CascadeEqual () =
            Assert.True( Integer([1u; 0u]) >= Integer([1u; 0u]) )
    
        [<Fact>]
        let CascadeLessThan () =
            Assert.False( Integer([1u; 0u]) >= Integer([1u; 1u]) )

        [<Fact>]
        let BiggerLeftNegative () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) >= Integer([0xDEADBEEFu], true) )

        [<Fact>]
        let BiggerRightNegative () =
            Assert.True( Integer([0xDEADBEEFu], true) >= Integer([0xBADu; 0xDEADBEEFu], true) )
    
        [<Fact>]
        let CascadeGreaterThanNegative () =
            Assert.False( Integer([1u; 1u], true) >= Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeEqualNegative () =
            Assert.True( Integer([1u; 0u], true) >= Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeLessThanNegative () =
            Assert.True( Integer([1u; 0u], true) >= Integer([1u; 1u],true) )

        [<Fact>]
        let BiggerLeftMixedNegativeLeft () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) >= Integer([0xDEADBEEFu], false) )

        [<Fact>]
        let BiggerRightMixedNegativeLeft () =
            Assert.False( Integer([0xDEADBEEFu], true) >= Integer([0xBADu; 0xDEADBEEFu], false) )
    
        [<Fact>]
        let CascadeGreaterThanMixedNegativeLeft () =
            Assert.False( Integer([1u; 1u], true) >= Integer([1u; 0u], false) )
    
        [<Fact>]
        let CascadeEqualMixedNegativeLeft () =
            Assert.False( Integer([1u; 0u], true) >= Integer([1u; 0u], false) )
    
        [<Fact>]
        let CascadeLessThanMixedNegativeLeft () =
            Assert.False( Integer([1u; 0u], true) >= Integer([1u; 1u], false) )

        [<Fact>]
        let BiggerLeftMixedNegativeRight () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu], false) >= Integer([0xDEADBEEFu], true) )

        [<Fact>]
        let BiggerRightMixedNegativeRight () =
            Assert.True( Integer([0xDEADBEEFu], false) >= Integer([0xBADu; 0xDEADBEEFu], true) )
    
        [<Fact>]
        let CascadeGreaterThanMixedNegativeRight () =
            Assert.True( Integer([1u; 1u], false) >= Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeEqualMixedNegativeRight () =
            Assert.True( Integer([1u; 0u], false) >= Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeLessThanMixedNegativeRight () =
            Assert.True( Integer([1u; 0u], false) >= Integer([1u; 1u], true) )

    module LessThanOrEqual =
        [<Theory>]
        [<InlineData( "0", "1", true )>]
        [<InlineData( "1", "0", false )>]
        [<InlineData( "0", "-1", false )>]
        [<InlineData( "-1", "0", true )>]
        [<InlineData( "1", "1", true )>]
        [<InlineData( "-1", "1", true )>]
        [<InlineData( "1", "-1", false )>]
        [<InlineData( "-1", "-1", true )>]
        let Sanity l r x =
            Assert.Equal( x, Integer.Parse( l ) <= Integer.Parse( r ) )

        [<Fact>]
        let BiggerLeft () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu]) <= Integer([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.True( Integer([0xDEADBEEFu]) <= Integer([0xBADu; 0xDEADBEEFu]) )
    
        [<Fact>]
        let CascadeGreaterThan () =
            Assert.False( Integer([1u; 1u]) <= Integer([1u; 0u]) )
    
        [<Fact>]
        let CascadeEqual () =
            Assert.True( Integer([1u; 0u]) <= Integer([1u; 0u]) )
    
        [<Fact>]
        let CascadeLessThan () =
            Assert.True( Integer([1u; 0u]) <= Integer([1u; 1u]) )

        [<Fact>]
        let BiggerLeftNegative () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu], true) <= Integer([0xDEADBEEFu], true) )

        [<Fact>]
        let BiggerRightNegative () =
            Assert.False( Integer([0xDEADBEEFu], true) <= Integer([0xBADu; 0xDEADBEEFu], true) )
    
        [<Fact>]
        let CascadeGreaterThanNegative () =
            Assert.True( Integer([1u; 1u], true) <= Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeEqualNegative () =
            Assert.True( Integer([1u; 0u], true) <= Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeLessThanNegative () =
            Assert.False( Integer([1u; 0u], true) <= Integer([1u; 1u],true) )

        [<Fact>]
        let BiggerLeftMixedNegativeLeft () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu], true) <= Integer([0xDEADBEEFu], false) )

        [<Fact>]
        let BiggerRightMixedNegativeLeft () =
            Assert.True( Integer([0xDEADBEEFu], true) <= Integer([0xBADu; 0xDEADBEEFu], false) )
    
        [<Fact>]
        let CascadeGreaterThanMixedNegativeLeft () =
            Assert.True( Integer([1u; 1u], true) <= Integer([1u; 0u], false) )
    
        [<Fact>]
        let CascadeEqualMixedNegativeLeft () =
            Assert.True( Integer([1u; 0u], true) <= Integer([1u; 0u], false) )
    
        [<Fact>]
        let CascadeLessThanMixedNegativeLeft () =
            Assert.True( Integer([1u; 0u], true) <= Integer([1u; 1u], false) )

        [<Fact>]
        let BiggerLeftMixedNegativeRight () =
            Assert.False( Integer([0xBADu; 0xDEADBEEFu], false) <= Integer([0xDEADBEEFu], true) )

        [<Fact>]
        let BiggerRightMixedNegativeRight () =
            Assert.False( Integer([0xDEADBEEFu], false) <= Integer([0xBADu; 0xDEADBEEFu], true) )
    
        [<Fact>]
        let CascadeGreaterThanMixedNegativeRight () =
            Assert.False( Integer([1u; 1u], false) <= Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeEqualMixedNegativeRight () =
            Assert.False( Integer([1u; 0u], false) <= Integer([1u; 0u], true) )
    
        [<Fact>]
        let CascadeLessThanMixedNegativeRight () =
            Assert.False( Integer([1u; 0u], false) <= Integer([1u; 1u], true) )

    module Inequality =
        [<Theory>]
        [<InlineData( "0", "0", false )>]
        [<InlineData( "0", "1", true )>]
        [<InlineData( "1", "0", true )>]
        [<InlineData( "1", "1", false )>]
        [<InlineData( "0", "-1", true )>]
        [<InlineData( "-1", "0", true )>]
        [<InlineData( "-1", "1", true )>]
        [<InlineData( "1", "-1", true )>]
        [<InlineData( "-1", "-1", false )>]
        let Sanity l r x =
            Assert.Equal( x, Integer.Parse( l ) <> Integer.Parse( r ) )

        [<Fact>]
        let BiggerLeft () =
            Assert.True( Integer([0xBADu; 0xDEADBEEFu]) <> Integer([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.True( Integer([0xDEADBEEFu]) <> Integer([0xBADu; 0xDEADBEEFu]) )

    module Addition =
        [<Theory>]
        [<InlineData( "1", "1", "2" )>]        // Sanity
        [<InlineData( "-1", "1", "0" )>]        // Sanity
        [<InlineData( "1", "-1", "0" )>]        // Sanity
        [<InlineData( "-1", "-1", "-2" )>]        // Sanity
        [<InlineData( "1", "0", "1" )>]        // Sanity
        [<InlineData( "-1", "0", "-1" )>]        // Sanity
        [<InlineData( "0", "1", "1" )>]        // Sanity
        [<InlineData( "0", "-1", "-1" )>]        // Sanity
        [<InlineData( "1", "5", "6" )>]        // l < r
        [<InlineData( "-1", "5", "4" )>]        // l < r
        [<InlineData( "1", "-5", "-4" )>]        // l < r
        [<InlineData( "-1", "-5", "-6" )>]        // l < r
        [<InlineData( "9", "2", "11" )>]       // l > r
        [<InlineData( "-9", "2", "-7" )>]       // l > r
        [<InlineData( "9", "-2", "7" )>]       // l > r
        [<InlineData( "-9", "-2", "-11" )>]       // l > r
        let Sanity l r s =
            Assert.Equal( Integer.Parse( s ), Integer.Parse( l ) + Integer.Parse( r ) )
    
        [<Fact>]
        let Overflow () =
            Assert.Equal( Integer([1u; 2u]), Integer([System.UInt32.MaxValue]) + Integer([3u]) )

        [<Fact>]
        let LeftBiggerNoOverflow () =
            Assert.Equal( Integer([0xFu; 0xFF0Fu]), Integer([0xFu; 0xFu]) + Integer([0xFF00u]) )
        
        [<Fact>]
        let RightBiggerNoOverflow () =
            Assert.Equal( Integer ([0xFu; 0xFF0Fu]), Integer([0xFF00u]) + Integer([0xFu; 0xFu]) )
        
        [<Fact>]
        let CascadingOverflow () =
            Assert.Equal( Integer([1u; 0u; 0u]), Integer([1u]) + Integer([System.UInt32.MaxValue; System.UInt32.MaxValue]) )

        [<Fact>]
        let OverflowCausedOverflow () =
            Assert.Equal( Integer([2u; 0u; 0u]), Integer([1u; 1u]) + Integer([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue]) )

        [<Fact>]
        let OverflowNegative () =
            Assert.Equal( Integer([1u; 2u], true), Integer([System.UInt32.MaxValue], true) + Integer([3u], true) )

        [<Fact>]
        let LeftBiggerNoOverflowNegative () =
            Assert.Equal( Integer([0xFu; 0xFF0Fu], true), Integer([0xFu; 0xFu], true) + Integer([0xFF00u], true) )

        [<Fact>]
        let RightBiggerNoOverflowNegative () =
            Assert.Equal( Integer ([0xFu; 0xFF0Fu], true), Integer([0xFF00u], true) + Integer([0xFu; 0xFu], true) )

        [<Fact>]
        let CascadingOverflowNegative () =
            Assert.Equal( Integer([1u; 0u; 0u], true), Integer([1u], true) + Integer([System.UInt32.MaxValue; System.UInt32.MaxValue], true) )

        [<Fact>]
        let OverflowCausedOverflowNegative () =
            Assert.Equal( Integer([2u; 0u; 0u], true), Integer([1u; 1u], true) + Integer([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue], true) )

    module Subtraction =
        [<Theory>]
        [<InlineData( "1", "1", "0" )>]                            // Sanity
        [<InlineData( "-1", "1", "-2" )>]                            // Sanity
        [<InlineData( "1", "-1", "2" )>]                            // Sanity
        [<InlineData( "-1", "-1", "0" )>]                            // Sanity
        [<InlineData( "1", "0", "1" )>]                            // Sanity
        [<InlineData( "-1", "0", "-1" )>]                            // Sanity
        [<InlineData( "0", "1", "-1" )>]                            // Sanity
        [<InlineData( "0", "-1", "1" )>]                            // Sanity
        [<InlineData( "9", "2", "7" )>]                            // l > r
        [<InlineData( "-9", "2", "-11" )>]                            // l > r
        [<InlineData( "9", "-2", "11" )>]                            // l > r
        [<InlineData( "-9", "-2", "-7" )>]                            // l > r
        [<InlineData( "1", "5", "-4" )>]                            // l < r
        [<InlineData( "-1", "5", "-6" )>]                            // l < r
        [<InlineData( "1", "-5", "6" )>]                            // l < r
        [<InlineData( "-1", "-5", "4" )>]                            // l < r
        let Sanity l r s =
            Assert.Equal( Integer.Parse( s ), Integer.Parse( l ) - Integer.Parse( r ) )
    
        [<Fact>]
        let MultiItemNoUnderflow () =
            let l = Integer([3u; 4u])
            let r = Integer([1u; 2u])
            Assert.Equal( Integer([2u; 2u]), l - r )

        [<Fact>]
        let MultiItemNegativeNoUnderflow () =
            let l = Integer([3u; 4u], true)
            let r = Integer([1u; 2u], true)
            Assert.Equal( Integer([2u; 2u], true), l - r )

        [<Fact>]
        let MultiItemMixedLeftNoUnderflow () =
            let l = Integer([3u; 4u], true)
            let r = Integer([1u; 2u])
            Assert.Equal( Integer([4u; 6u], true), l - r )

        [<Fact>]
        let MultiItemMixedRightNoUnderflow () =
            let l = Integer([3u; 4u])
            let r = Integer([1u; 2u], true)
            Assert.Equal( Integer([4u; 6u]), l - r )

        [<Fact>]
        let MultiItemUnderflow () =
            let l = Integer ([4u; 2u])
            let r = Integer ([1u; 3u])
            Assert.Equal( Integer ([0x2u; 0xFFFFFFFFu]), l - r )
        
        [<Fact>]
        let MultiItemCascadingUnderflow () =
            let l = Integer ([1u; 0u; 0u])
            let r = Integer ([1u])
            Assert.Equal( Integer ([0xFFFFFFFFu; 0xFFFFFFFFu]), l - r )

        [<Fact>]
        let MultiItemNegativeOverflow () =
            let l = Integer([0x2u; 0xFFFFFFFFu], true)
            let r = Integer([1u; 3u])
            Assert.Equal( Integer([4u; 2u], true), l - r )
        
        [<Fact>]
        let MultiItemCascadingNegativeOverflow () =
            let l = Integer ([0xFFFFFFFFu; 0xFFFFFFFFu], true)
            let r = Integer ([1u])
            Assert.Equal( Integer([1u; 0u; 0u], true), l - r )

    module Multiply =
        [<Theory>]
        [<InlineData( "1", "1", "1" )>]        // Sanity
        [<InlineData( "-1", "1", "-1" )>]        // Sanity
        [<InlineData( "1", "-1", "-1" )>]        // Sanity
        [<InlineData( "-1", "-1", "1" )>]        // Sanity
        [<InlineData( "1", "0", "0" )>]        // Sanity
        [<InlineData( "-1", "0", "0" )>]        // Sanity
        [<InlineData( "0", "1", "0" )>]        // Sanity
        [<InlineData( "0", "-1", "0" )>]        // Sanity
        [<InlineData( "6", "7", "42" )>]       // multiple bits
        [<InlineData( "-6", "7", "-42" )>]       // multiple bits
        [<InlineData( "6", "-7", "-42" )>]       // multiple bits
        [<InlineData( "-6", "-7", "42" )>]       // multiple bits
        let Sanity l r p =
            Assert.Equal( Integer.Parse( p ), Integer.Parse( l ) * Integer.Parse( r ) )

        [<Fact>]
        let Big () =
            Assert.Equal( Integer([0x75CD9046u; 0x541D5980u]), Integer([0xFEDCBA98u]) * Integer([0x76543210u]) )

        [<Fact>]
        let BigMixedLeft () =
            Assert.Equal( Integer([0x75CD9046u; 0x541D5980u], true), Integer([0xFEDCBA98u], true) * Integer([0x76543210u]) )

        [<Fact>]
        let BigMixedRight () =
            Assert.Equal( Integer([0x75CD9046u; 0x541D5980u], true), Integer([0xFEDCBA98u]) * Integer([0x76543210u], true) )

        [<Fact>]
        let BigNegative () =
            Assert.Equal( Integer([0x75CD9046u; 0x541D5980u]), Integer([0xFEDCBA98u], true) * Integer([0x76543210u], true) )

    module Division =
        [<Theory>]
        [<InlineData( "1", "1", "1" )>]        // Sanity
        [<InlineData( "-1", "1", "-1" )>]        // Sanity
        [<InlineData( "1", "-1", "-1" )>]        // Sanity
        [<InlineData( "-1", "-1", "1" )>]        // Sanity
        [<InlineData( "0", "1", "0" )>]        // Sanity
        [<InlineData( "0", "-1", "0" )>]        // Sanity
        [<InlineData( "44", "7", "6" )>]       // multiple bits
        [<InlineData( "-44", "7", "-6" )>]       // multiple bits
        [<InlineData( "44", "-7", "-6" )>]       // multiple bits
        [<InlineData( "-44", "-7", "6" )>]       // multiple bits
        [<InlineData( "52", "5", "10" )>]      // rev
        [<InlineData( "-52", "5", "-10" )>]      // rev
        [<InlineData( "52", "-5", "-10" )>]      // rev
        [<InlineData( "-52", "-5", "10" )>]      // rev
        [<InlineData( "52", "10", "5" )>]      // rev
        [<InlineData( "-52", "10", "-5" )>]      // rev
        [<InlineData( "52", "-10", "-5" )>]      // rev
        [<InlineData( "-52", "-10", "5" )>]      // rev
        let Sanity dividend divisor quotient =
            Assert.Equal( Integer.Parse( quotient ), Integer.Parse( dividend ) / Integer.Parse( divisor ) )

        [<Fact>]
        let Zero () =
            Assert.Equal( Integer([0u]), Integer([5u]) / Integer([10u]) )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Integer.Unit / Integer.Zero ) |> ignore )

        [<Fact>]
        let Big () =
            Assert.Equal( Integer([0xFEDCBA98u]), Integer([0x75CD9046u; 0x6651AFF8u]) / Integer([0x76543210u]) )

        [<Fact>]
        let BigMixedLeft () =
            Assert.Equal( Integer([0xFEDCBA98u], true), Integer([0x75CD9046u; 0x6651AFF8u], true) / Integer([0x76543210u]) )

        [<Fact>]
        let BigMixedRight () =
            Assert.Equal( Integer([0xFEDCBA98u], true), Integer([0x75CD9046u; 0x6651AFF8u]) / Integer([0x76543210u], true) )

        [<Fact>]
        let BigNegative () =
            Assert.Equal( Integer([0xFEDCBA98u]), Integer([0x75CD9046u; 0x6651AFF8u], true) / Integer([0x76543210u], true) )

    module Modulo =
        [<Theory>]
        [<InlineData( "1", "1", "0" )>]        // Sanity
        [<InlineData( "-1", "1", "0" )>]        // Sanity
        [<InlineData( "1", "-1", "0" )>]        // Sanity
        [<InlineData( "-1", "-1", "0" )>]        // Sanity
        [<InlineData( "0", "1", "0" )>]        // Sanity
        [<InlineData( "0", "-1", "0" )>]        // Sanity
        [<InlineData( "44", "7", "2" )>]       // multiple bits
        [<InlineData( "-44", "7", "-2" )>]       // multiple bits
        [<InlineData( "44", "-7", "2" )>]       // multiple bits
        [<InlineData( "-44", "-7", "-2" )>]       // multiple bits
        [<InlineData( "52", "5", "2" )>]      // rev
        [<InlineData( "-52", "5", "-2" )>]      // rev
        [<InlineData( "52", "-5", "2" )>]      // rev
        [<InlineData( "-52", "-5", "-2" )>]      // rev
        [<InlineData( "52", "10", "2" )>]      // rev
        [<InlineData( "-52", "10", "-2" )>]      // rev
        [<InlineData( "52", "-10", "2" )>]      // rev
        [<InlineData( "-52", "-10", "-2" )>]      // rev
        let Sanity dividend divisor remainder =
            Assert.Equal( Integer.Parse( remainder ), Integer.Parse( dividend ) % Integer.Parse( divisor ) )

        [<Fact>]
        let Zero () =
            Assert.Equal( Integer([0u]), Integer([20u]) % Integer([10u]) )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Integer.Unit % Integer.Zero ) |> ignore )

        [<Fact>]
        let Big () =
            Assert.Equal( Integer([0x12345678u]), Integer([0x75CD9046u; 0x6651AFF8u]) % Integer([0x76543210u]) )

        [<Fact>]
        let BigMixedLeft () =
            Assert.Equal( Integer([0x12345678u], true), Integer([0x75CD9046u; 0x6651AFF8u], true) % Integer([0x76543210u]) )

        [<Fact>]
        let BigMixedRight () =
            Assert.Equal( Integer([0x12345678u]), Integer([0x75CD9046u; 0x6651AFF8u]) % Integer([0x76543210u], true) )

        [<Fact>]
        let BigNegative () =
            Assert.Equal( Integer([0x12345678u], true), Integer([0x75CD9046u; 0x6651AFF8u], true) % Integer([0x76543210u], true) )

    module DivisionModulo =
        [<Theory>]
        [<InlineData( "1", "1", "1", "0" )>]        // Sanity
        [<InlineData( "-1", "1", "-1", "0" )>]        // Sanity
        [<InlineData( "1", "-1", "-1", "0" )>]        // Sanity
        [<InlineData( "-1", "-1", "1", "0" )>]        // Sanity
        [<InlineData( "0", "1", "0", "0" )>]        // Sanity
        [<InlineData( "0", "-1", "0", "0" )>]        // Sanity
        [<InlineData( "44", "7", "6", "2" )>]       // multiple bits
        [<InlineData( "-44", "7", "-6", "-2" )>]       // multiple bits
        [<InlineData( "44", "-7", "-6", "2" )>]       // multiple bits
        [<InlineData( "-44", "-7", "6", "-2" )>]       // multiple bits
        [<InlineData( "52", "5", "10", "2" )>]      // rev
        [<InlineData( "-52", "5", "-10", "-2" )>]      // rev
        [<InlineData( "52", "-5", "-10", "2" )>]      // rev
        [<InlineData( "-52", "-5", "10", "-2" )>]      // rev
        [<InlineData( "52", "10", "5", "2" )>]      // rev
        [<InlineData( "-52", "10", "-5", "-2" )>]      // rev
        [<InlineData( "52", "-10", "-5", "2" )>]      // rev
        [<InlineData( "-52", "-10", "5", "-2" )>]      // rev
        let Sanity dividend divisor quotient remainder =
            Assert.Equal( (Integer.Parse( quotient ),Integer.Parse( remainder )), Integer.Parse( dividend ) /% Integer.Parse( divisor ) )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Integer.Unit /% Integer.Zero ) |> ignore )

        [<Fact>]
        let Big () =
            Assert.Equal( (Integer([0xFEDCBA98u]),Integer([0x12345678u])), Integer([0x75CD9046u; 0x6651AFF8u]) /% Integer([0x76543210u]) )

        [<Fact>]
        let BigMixedLeft () =
            Assert.Equal( (Integer([0xFEDCBA98u], true),Integer([0x12345678u], true)), Integer([0x75CD9046u; 0x6651AFF8u], true) /% Integer([0x76543210u]) )

        [<Fact>]
        let BigMixedRight () =
            Assert.Equal( (Integer([0xFEDCBA98u], true),Integer([0x12345678u])), Integer([0x75CD9046u; 0x6651AFF8u]) /% Integer([0x76543210u], true) )

        [<Fact>]
        let BigNegative () =
            Assert.Equal( (Integer([0xFEDCBA98u]),Integer([0x12345678u], true)), Integer([0x75CD9046u; 0x6651AFF8u], true) /% Integer([0x76543210u], true) )

    module Negation =
        [<Theory>]
        [<InlineData( "1", "-1" )>]                            // Sanity
        [<InlineData( "-1", "1" )>]                            // Sanity
        [<InlineData( "0", "0" )>]                            // Sanity
        let Sanity a e =
            Assert.Equal( Integer.Parse( e ), -Integer.Parse( a ) )

        [<Fact>]
        let Big () =
            Assert.Equal( Integer( [0xFEDCBA9u; 0x76543210u], true ), -Integer( [0xFEDCBA9u; 0x76543210u] )  )

        [<Fact>]
        let BigNegative () =
            Assert.Equal( Integer( [0xFEDCBA9u; 0x76543210u] ), -Integer( [0xFEDCBA9u; 0x76543210u], true )  )

    module ToString =
        [<Theory>]
        [<InlineData( 0u, false, "0" )>]          // Sanity
        [<InlineData( 1u, false, "1" )>]          // Sanity
        [<InlineData( 123u, false, "123" )>]      // multiple bits
        [<InlineData( 45678u, false, "45678" )>]  // rev
        [<InlineData( 1u, true, "-1" )>]          // Sanity
        [<InlineData( 123u, true, "-123" )>]      // multiple bits
        [<InlineData( 45678u, true, "-45678" )>]  // rev
        let Sanity data negative expected =
            Assert.Equal( expected, Integer([data], negative).ToString() )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal( "1234567890123456789", Integer( [ 0x112210F4u; 0x7DE98115u ] ).ToString() )

        [<Fact>]
        let BiggerNegative () =
            Assert.Equal( "-1234567890123456789", Integer( [ 0x112210F4u; 0x7DE98115u ], true ).ToString() )

    module Parse =
        [<Theory>]
        [<InlineData( "0", 0u, false )>]          // Sanity
        [<InlineData( "1", 1u, false )>]          // Sanity
        [<InlineData( "123", 123u, false )>]      // multiple bits
        [<InlineData( "45678", 45678u, false )>]  // rev
        [<InlineData( "-0", 0u, false )>]          // Sanity
        [<InlineData( "-1", 1u, true )>]          // Sanity
        [<InlineData( "-123", 123u, true )>]      // multiple bits
        [<InlineData( "-45678", 45678u, true )>]  // rev
        let Sanity str data negative =
            Assert.Equal( Integer([data], negative), Integer.Parse(str) )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal( Integer( [ 0x112210F4u; 0x7DE98115u ] ), Integer.Parse("1234567890123456789") )

        [<Fact>]
        let BiggerNegative () =
            Assert.Equal( Integer( [ 0x112210F4u; 0x7DE98115u ], true ), Integer.Parse("-1234567890123456789") )
