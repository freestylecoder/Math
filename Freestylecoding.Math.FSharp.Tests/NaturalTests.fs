namespace Freestylecoding.Math.FSharp.Tests

open Xunit
open Freestylecoding.Math

module Natural =
    // NOTE: The sanity tests are here to do low level tests of a few parts
    // This lets me isolate those tests so I can use those parts in the rest of the tests
    //
    // In other words:
    // IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
    [<Trait( "Type", "Sanity" )>]
    module Sanity =
        [<Fact>]
        let LeftShift () =
            Assert.Equal( Natural([4u; 0x8000_0000u; 0u]), Natural([9u]) <<< 63 )

        [<Fact>]
        let RightShift () =
            Assert.Equal( Natural([9u]), Natural([4u; 0x8000_0000u; 0u]) >>> 63 )

        [<Fact>]
        let GreaterThanTrue () =
            Assert.True( Natural([0xDEADBEEFu; 0xBADu]) > Natural([0xBADu; 0xDEADBEEFu;]) )

        [<Fact>]
        let GreaterThanFalseByLessThan () =
            Assert.False( Natural([0xBADu; 0xDEADBEEFu]) > Natural([0xDEADBEEFu; 0xBADu]) )

        [<Fact>]
        let GreaterThanFalseByEquals () =
            Assert.False( Natural([0xBADu; 0xDEADBEEFu]) > Natural([0xBADu; 0xDEADBEEFu]) )

        [<Fact>]
        let Addition () =
            Assert.Equal( Natural([2u; 0u; 0u]), Natural([1u; 1u]) + Natural([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue]) )

        [<Fact>]
        let Subtraction () =
            Assert.Equal( Natural ([0xFFFF_FFFFu; 0xFFFF_FFFFu]), Natural([1u; 0u; 0u]) - Natural([1u]) )

        [<Fact>]
        let Multiplication () =
            Assert.Equal( Natural([0x75CD9046u; 0x541D5980u]), Natural([0xFEDCBA98u]) * Natural([0x76543210u]) )

        [<Fact>]
        let DivisionModulo () =
            Assert.Equal( (Natural([0xFEDCBA98u]),Natural([0x12345678u])), Natural([0x75CD9046u; 0x6651AFF8u]) /% Natural([0x76543210u]) )

        [<Fact>]
        let ToString () =
            Assert.Equal( "1234567890123456789", Natural([0x112210F4u; 0x7DE98115u]).ToString() )

        [<Fact>]
        let Parse () =
            Assert.Equal( Natural([0x112210F4u; 0x7DE98115u] ), Natural.Parse("1234567890123456789") )

    module Ctor =
        [<Theory>]
        [<InlineData(                    "0", 0u, 0u, 0u )>]
        [<InlineData(           "4294967297", 0u, 1u, 1u )>]
        [<InlineData( "18446744073709551616", 1u, 0u, 0u )>]
        [<InlineData( "18446744073709551617", 1u, 0u, 1u )>]
        let Default expected a b c =
            Assert.Equal( Natural.Parse( expected ), Natural( [a; b; c] ) )

        [<Theory>]
        [<InlineData(   0u )>]
        [<InlineData(   1u )>]
        [<InlineData(   2u )>]
        [<InlineData(   5u )>]
        [<InlineData( 100u )>]
        let Uint32 (actual:uint32) =
            Assert.Equal( Natural( [actual] ), Natural( actual ) )

        [<Theory>]
        [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL )>]
        [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001uL )>]
        [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000uL )>]
        [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL )>]
        [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078uL )>]
        [<InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0uL )>]
        [<InlineData( 0xFFFF_0000u, 0x0000_0000u, 0xFFFF_0000_0000_0000uL )>]
        [<InlineData( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0xFFFF_EEEE_DDDD_CCCCuL )>]
        let Uint64 (expectedHigh:uint32) (expectedLow:uint32) (actual:uint64) =
            Assert.Equal( Natural( [expectedHigh; expectedLow] ), Natural( actual ) )

        [<Theory>]
        [<InlineData( 0u, 0u, 0u )>]
        [<InlineData( 0u, 0u, 1u )>]
        [<InlineData( 0u, 1u, 0u )>]
        [<InlineData( 0u, 1u, 1u )>]
        [<InlineData( 1u, 0u, 0u )>]
        [<InlineData( 1u, 0u, 1u )>]
        [<InlineData( 1u, 1u, 0u )>]
        [<InlineData( 1u, 1u, 1u )>]
        let Uint32Sequence (a:uint32) (b:uint32) (c:uint32) =
            Assert.Equal( Natural( [ a; b; c ] ), Natural( seq { a; b; c } ) )

    module Implicit =
        [<Theory>]
        [<InlineData(   0u )>]
        [<InlineData(   1u )>]
        [<InlineData(   2u )>]
        [<InlineData(   5u )>]
        [<InlineData( 100u )>]
        let Uint32 (actual:uint32) =
            Assert.Equal( Natural( [actual] ), Natural.op_Implicit( actual ) )

        [<Theory>]
        [<InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL )>]
        [<InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001uL )>]
        [<InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000uL )>]
        [<InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL )>]
        [<InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078uL )>]
        [<InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0uL )>]
        [<InlineData( 0xFFFF_0000u, 0x0000_0000u, 0xFFFF_0000_0000_0000uL )>]
        [<InlineData( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0xFFFF_EEEE_DDDD_CCCCuL )>]
        let Uint64 (expectedHigh:uint32) (expectedLow:uint32) (actual:uint64) =
            Assert.Equal( Natural( [expectedHigh; expectedLow] ), Natural.op_Implicit( actual ) )

    module And =
        [<Theory>]
        [<InlineData( 0u, 0u, 0u)>]
        [<InlineData( 1u, 0u, 0u)>]
        [<InlineData( 0u, 1u, 0u)>]
        [<InlineData( 1u, 1u, 1u)>]
        let Sanity l r x =
            Assert.Equal( Natural([x]), Natural([l]) &&& Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.Equal( Natural.Unit, Natural([0xFu; 0x00000101u]) &&& Natural([ 0x00010001u]) )

        [<Fact>]
        let BiggerRight () =
            Assert.Equal( Natural.Unit, Natural([0x00010001u]) &&& Natural([0xFu; 0x00000101u]) )

    module Or =
        [<Theory>]
        [<InlineData( 0u, 0u, 0u)>]
        [<InlineData( 1u, 0u, 1u)>]
        [<InlineData( 0u, 1u, 1u)>]
        [<InlineData( 1u, 1u, 1u)>]
        let Sanity l r x =
            Assert.Equal( Natural([x]), Natural([l]) ||| Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.Equal( Natural([0xFu; 0x10101u]), Natural([0xFu; 0x00000101u]) ||| Natural([ 0x00010001u]) )

        [<Fact>]
        let BiggerRight () =
            Assert.Equal( Natural([0xFu; 0x10101u]), Natural([0x00010001u]) ||| Natural([0xFu; 0x00000101u]) )

    module Xor =
        [<Theory>]
        [<InlineData( 0u, 0u, 0u)>]
        [<InlineData( 1u, 0u, 1u)>]
        [<InlineData( 0u, 1u, 1u)>]
        [<InlineData( 1u, 1u, 0u)>]
        let Sanity l r x =
            Assert.Equal( Natural([x]), Natural([l]) ^^^ Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.Equal( Natural([0xFu; 0x10100u]), Natural([0xFu; 0x00000101u]) ^^^ Natural([0x00010001u]) )

        [<Fact>]
        let BiggerRight () =
            Assert.Equal( Natural([0xFu; 0x10100u]), Natural([0x00010001u]) ^^^ Natural([0xFu; 0x00000101u]) )

    module BitwiseNot =
        [<Theory>]
        [<InlineData( 0xFFFFFFFEu, 1u)>]
        [<InlineData( 1u, 0xFFFFFFFEu)>]
        let Sanity r x =
            Assert.Equal( Natural([x]), ~~~ Natural([r]) )

        [<Fact>]
        let Bigger () =
            Assert.Equal( Natural([0xF0123456u; 0x789ABCDEu]), ~~~ Natural([0x0FEDCBA9u; 0x87654321u]) )

    module LeftShift =
        [<Theory>]
        [<InlineData( 1u, 1, 2u )>]        // Sanity
        [<InlineData( 0xFu, 2, 0x3Cu )>]   // multiple bits
        let Sanity l r s =
            Assert.Equal( Natural([s]), Natural([l]) <<< r )
        
        [<Fact>]
        let Overflow () =
            Assert.Equal( Natural([1u; 0xFFFFFFFEu]), Natural([0xFFFFFFFFu]) <<< 1 )
        
        [<Fact>]
        let MultipleOverflow () =
            Assert.Equal( Natural([0x5u; 0xFFFFFFF8u]), Natural([0xBFFFFFFFu]) <<< 3 )
        
        [<Fact>]
        let OverOneUInt () =
            Assert.Equal( Natural([8u; 0u; 0u]), Natural.Unit <<< 67 )

    module RightShift =
        [<Theory>]
        [<InlineData( 1u, 1, 0u )>]        // Sanity
        [<InlineData( 0xFu, 2, 0x3u )>]    // multiple bits
        [<InlineData( 0x3Cu, 2, 0xFu )>]   // multiple bits
        let Sanity l r s =
            Assert.Equal( Natural([s]), Natural([l]) >>> r )

        [<Fact>]
        let Underflow () =
            Assert.Equal( Natural([0x7FFFFFFFu]), Natural([0xFFFFFFFFu]) >>> 1 )
        
        [<Fact>]
        let MultipleUnderflow () =
            Assert.Equal( Natural([0x1u; 0x5FFFFFFFu]), Natural([0xAu; 0xFFFFFFFFu]) >>> 3 )
        
        [<Fact>]
        let OverOneUInt () =
            Assert.Equal( Natural.Unit, Natural([0x10u; 0u; 0u]) >>> 68 )
        
        [<Fact>]
        let ReduceToZero () =
            Assert.Equal( Natural([0u]), Natural([0x10u; 0u; 0u]) >>> 99 )

    module Equality =
        [<Theory>]
        [<InlineData( 0u, 0u, true )>]
        [<InlineData( 0u, 1u, false )>]
        [<InlineData( 1u, 0u, false )>]
        [<InlineData( 1u, 1u, true )>]
        let Sanity l r x =
            Assert.Equal( x, Natural([l]) = Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.False( Natural([0xBADu; 0xDEADBEEFu]) = Natural([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.False( Natural([0xDEADBEEFu]) = Natural([0xBADu; 0xDEADBEEFu]) )

    module GreaterThan =
        [<Theory>]
        [<InlineData( 0u, 1u, false )>]
        [<InlineData( 1u, 0u, true )>]
        [<InlineData( 1u, 1u, false )>]
        let Sanity l r x =
            Assert.Equal( x, Natural([l]) > Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.True( Natural([0xBADu; 0xDEADBEEFu]) > Natural([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.False( Natural([0xDEADBEEFu]) > Natural([0xBADu; 0xDEADBEEFu]) )
    
        [<Fact>]
        let CascadeGreaterThan () =
            Assert.True( Natural([1u; 1u]) > Natural([1u; 0u]) )
    
        [<Fact>]
        let CascadeEqual () =
            Assert.False( Natural([1u; 0u]) > Natural([1u; 0u]) )
    
        [<Fact>]
        let CascadeLessThan () =
            Assert.False( Natural([1u; 0u]) > Natural([1u; 1u]) )

    module LessThan =
        [<Theory>]
        [<InlineData( 0u, 1u, true )>]
        [<InlineData( 1u, 0u, false )>]
        [<InlineData( 1u, 1u, false )>]
        let Sanity l r x =
            Assert.Equal( x, Natural([l]) < Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.False( Natural([0xBADu; 0xDEADBEEFu]) < Natural([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.True( Natural([0xDEADBEEFu]) < Natural([0xBADu; 0xDEADBEEFu]) )
    
        [<Fact>]
        let CascadeGreaterThan () =
            Assert.False( Natural([1u; 1u]) < Natural([1u; 0u]) )
    
        [<Fact>]
        let CascadeEqual () =
            Assert.False( Natural([1u; 0u]) < Natural([1u; 0u]) )
    
        [<Fact>]
        let CascadeLessThan () =
            Assert.True( Natural([1u; 0u]) < Natural([1u; 1u]) )

    module GreaterThanOrEqual =
        [<Theory>]
        [<InlineData( 0u, 1u, false )>]
        [<InlineData( 1u, 0u, true )>]
        [<InlineData( 1u, 1u, true )>]
        let Sanity l r x =
            Assert.Equal( x, Natural([l]) >= Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.True( Natural([0xBADu; 0xDEADBEEFu]) >= Natural([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.False( Natural([0xDEADBEEFu]) >= Natural([0xBADu; 0xDEADBEEFu]) )
    
        [<Fact>]
        let CascadeGreaterThan () =
            Assert.True( Natural([1u; 1u]) >= Natural([1u; 0u]) )
    
        [<Fact>]
        let CascadeEqual () =
            Assert.True( Natural([1u; 0u]) >= Natural([1u; 0u]) )
    
        [<Fact>]
        let CascadeLessThan () =
            Assert.False( Natural([1u; 0u]) >= Natural([1u; 1u]) )

    module LessThanOrEqual =
        [<Theory>]
        [<InlineData( 0u, 1u, true )>]
        [<InlineData( 1u, 0u, false )>]
        [<InlineData( 1u, 1u, true )>]
        let Sanity l r x =
            Assert.Equal( x, Natural([l]) <= Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.False( Natural([0xBADu; 0xDEADBEEFu]) <= Natural([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.True( Natural([0xDEADBEEFu]) <= Natural([0xBADu; 0xDEADBEEFu]) )
    
        [<Fact>]
        let CascadeGreaterThan () =
            Assert.False( Natural([1u; 1u]) <= Natural([1u; 0u]) )
    
        [<Fact>]
        let CascadeEqual () =
            Assert.True( Natural([1u; 0u]) <= Natural([1u; 0u]) )
    
        [<Fact>]
        let CascadeLessThan () =
            Assert.True( Natural([1u; 0u]) <= Natural([1u; 1u]) )

    module Inequality =
        [<Theory>]
        [<InlineData( 0u, 0u, false )>]
        [<InlineData( 0u, 1u, true )>]
        [<InlineData( 1u, 0u, true )>]
        [<InlineData( 1u, 1u, false )>]
        let Sanity l r x =
            Assert.Equal( x, Natural([l]) <> Natural([r]) )

        [<Fact>]
        let BiggerLeft () =
            Assert.True( Natural([0xBADu; 0xDEADBEEFu]) <> Natural([0xDEADBEEFu]) )

        [<Fact>]
        let BiggerRight () =
            Assert.True( Natural([0xDEADBEEFu]) <> Natural([0xBADu; 0xDEADBEEFu]) )

    module Addition =
        [<Theory>]
        [<InlineData( 0u, 0u, 0u )>]        // Sanity
        [<InlineData( 1u, 1u, 2u )>]        // Sanity
        [<InlineData( 1u, 0u, 1u )>]        // Sanity
        [<InlineData( 0u, 1u, 1u )>]        // Sanity
        [<InlineData( 1u, 5u, 6u )>]        // l < r
        [<InlineData( 9u, 2u, 11u )>]       // l > r
        let Sanity l r s =
            let l = Natural ([l])
            let r = Natural ([r])
            Assert.Equal( Natural ([s]), l + r )
    
        [<Fact>]
        let Overflow () =
            let l = Natural ([System.UInt32.MaxValue])
            let r = Natural ([3u])
            Assert.Equal( Natural ([1u; 2u]), l + r )
        
        [<Fact>]
        let LeftBiggerNoOverflow () =
            let l = Natural ([0xFu; 0xFu])
            let r = Natural ([0xFF00u])
            Assert.Equal( Natural ([0xFu; 0xFF0Fu]), l + r )
        
        [<Fact>]
        let RightBiggerNoOverflow () =
            let l = Natural ([0xFF00u])
            let r = Natural ([0xFu; 0xFu])
            Assert.Equal( Natural ([0xFu; 0xFF0Fu]), l + r )
        
        [<Fact>]
        let CascadingOverflow () =
            let l = Natural ([1u])
            let r = Natural ([System.UInt32.MaxValue; System.UInt32.MaxValue])
            Assert.Equal( Natural ([1u; 0u; 0u]), l + r )

        [<Fact>]
        let OverflowCausedOverflow () =
            let l = Natural ([1u; 1u])
            let r = Natural ([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue])
            Assert.Equal( Natural ([2u; 0u; 0u]), l + r )

        [<Fact>]
        let EdgeOfOverflow () =
            let l = Natural ([1u; 1u])
            let r = Natural ([1u; System.UInt32.MaxValue - 1u; 0u])
            Assert.Equal( Natural ([1u; System.UInt32.MaxValue; 1u]), l + r )

    module Subtraction =
        [<Theory>]
        [<InlineData( 1u, 1u, 0u )>]                            // Sanity
        [<InlineData( 1u, 0u, 1u )>]                            // Sanity
        [<InlineData( 9u, 2u, 7u )>]                            // l > r
        let Sanity l r s =
            let l = Natural ([l])
            let r = Natural ([r])
            Assert.Equal( Natural ([s]), l - r )
    
        [<Fact>]
        let SingleItemBadUnderflow () =
            let l = Natural ([0u])
            let r = Natural ([1u])
            Assert.True(
                try
                    l - r |> ignore
                    false
                with
                | :? System.OverflowException -> true
            )

        [<Fact>]
        let MultiItemNoUnderflow () =
            let l = Natural ([3u; 4u])
            let r = Natural ([1u; 2u])
            Assert.Equal( Natural ([2u; 2u]), l - r )
        
        [<Fact>]
        let MultiItemSafeUnderflow () =
            let l = Natural ([4u; 2u])
            let r = Natural ([1u; 3u])
            Assert.Equal( Natural ([0x2u; 0xFFFFFFFFu]), l - r )
        
        [<Fact>]
        let MultiItemSafeCascadingUnderflow () =
            let l = Natural ([1u; 0u; 0u])
            let r = Natural ([1u])
            Assert.Equal( Natural ([0xFFFFFFFFu; 0xFFFFFFFFu]), l - r )
        
        [<Fact>]
        let MultiItemUnsafeUnderflow () =
            let l = Natural ([1u; 2u])
            let r = Natural ([1u; 3u])
            Assert.True(
                try
                    l - r |> ignore
                    false
                with
                | :? System.OverflowException -> true
            )

        [<Fact>]
        let LargeWithUnderflows () =
            let l = Natural ([3u; 2u; 1u])
            let r = Natural ([1u; 2u; 3u])
            Assert.Equal( Natural ([0x1u; 0xFFFFFFFFu; 0xFFFFFFFEu]), l - r )

    module Multiply =
        [<Theory>]
        [<InlineData( 1u, 1u, 1u )>]        // Sanity
        [<InlineData( 1u, 0u, 0u )>]        // Sanity
        [<InlineData( 0u, 1u, 0u )>]        // Sanity
        [<InlineData( 6u, 7u, 42u )>]       // multiple bits
        let Sanity l r p =
            Assert.Equal( Natural([p]), Natural([l]) * Natural([r]) )

        [<Fact>]
        let Big () =
            Assert.Equal( Natural([ 0x75CD9046u; 0x541D5980u]), Natural( [0xFEDCBA98u]) * Natural([0x76543210u]) )

    module Division =
        [<Theory>]
        [<InlineData( 1u, 1u, 1u )>]        // Sanity
        [<InlineData( 0u, 1u, 0u )>]        // Sanity
        [<InlineData( 42u, 7u, 6u )>]       // multiple bits
        [<InlineData( 50u, 5u, 10u )>]      // rev
        [<InlineData( 50u, 10u, 5u )>]      // rev
        [<InlineData( 54u, 5u, 10u )>]      // has remainder
        let Sanity dividend divisor quotient =
            Assert.Equal( Natural([quotient]), Natural([dividend]) / Natural([divisor]) )

        [<Fact>]
        let Zero () =
            Assert.Equal( Natural( [0u] ), Natural([5u]) / Natural([10u]) )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Natural.Unit / Natural.Zero ) |> ignore )

        [<Fact>]
        let Big () =
            Assert.Equal( Natural([0xFEDCBA98u]), Natural([0x75CD9046u; 0x541D5980u]) / Natural([0x76543210u]) )

    module Modulo =
        [<Theory>]
        [<InlineData( 1u, 1u, 0u )>]        // Sanity
        [<InlineData( 0u, 1u, 0u )>]        // Sanity
        [<InlineData( 44u, 7u, 2u )>]       // multiple bits
        [<InlineData( 52u, 5u, 2u )>]       // rev
        [<InlineData( 52u, 10u, 2u )>]      // rev
        let Sanity dividend divisor remainder =
            Assert.Equal( Natural([remainder]), Natural([dividend]) % Natural([divisor]) )

        [<Fact>]
        let Zero () =
            Assert.Equal( Natural([0u]), Natural([20u]) % Natural([10u]) )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Natural.Unit % Natural.Zero ) |> ignore )

        [<Fact>]
        let Big () =
            Assert.Equal( Natural([0x12345678u]), Natural([0x75CD9046u; 0x6651AFF8u]) % Natural([0x76543210u]) )

    module DivisionModulo =
        [<Theory>]
        [<InlineData( 1u, 1u, 1u, 0u )>]        // Sanity
        [<InlineData( 0u, 1u, 0u, 0u )>]        // Sanity
        [<InlineData( 44u, 7u, 6u, 2u )>]       // multiple bits
        [<InlineData( 52u, 5u, 10u, 2u )>]      // rev
        [<InlineData( 52u, 10u, 5u, 2u )>]      // rev
        let Sanity dividend divisor quotient remainder =
            Assert.Equal( (Natural([quotient]),Natural([remainder])), Natural([dividend]) /% Natural([divisor]) )

        [<Fact>]
        let DivideByZero () =
            Assert.Throws<System.DivideByZeroException>( fun () -> ( Natural.Unit /% Natural.Zero ) |> ignore )

        [<Fact>]
        let Big () =
            Assert.Equal( (Natural([0xFEDCBA98u]),Natural([0x12345678u])), Natural([0x75CD9046u; 0x6651AFF8u]) /% Natural([0x76543210u]) )

    module ToString =
        [<Theory>]
        [<InlineData( 0u, "0" )>]          // Sanity
        [<InlineData( 1u, "1" )>]          // Sanity
        [<InlineData( 123u, "123" )>]      // multiple bits
        [<InlineData( 45678u, "45678" )>]  // rev
        let Sanity n s =
            Assert.Equal( s, Natural([n]).ToString() )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal( "1234567890123456789", Natural( [ 0x112210F4u; 0x7DE98115u ] ).ToString() )

    module Parse =
        [<Theory>]
        [<InlineData( 0u, "0" )>]          // Sanity
        [<InlineData( 1u, "1" )>]          // Sanity
        [<InlineData( 123u, "123" )>]      // multiple bits
        [<InlineData( 45678u, "45678" )>]  // rev
        let Sanity n s =
            Assert.Equal( Natural([n]), Natural.Parse(s) )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal( Natural( [ 0x112210F4u; 0x7DE98115u ] ), Natural.Parse("1234567890123456789") )
