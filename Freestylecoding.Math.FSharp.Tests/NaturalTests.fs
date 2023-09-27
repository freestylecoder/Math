namespace Freestylecoding.Math.FSharp.Tests

open Xunit
open Freestylecoding.Math

module Natural =
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
        let Simple () =
            let l = Natural.Zero
            let r = Natural.Unit
            Assert.Equal( Natural.Unit, l+r)

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
