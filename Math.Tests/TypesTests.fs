namespace Freestylecoding.Math.Tests.Natural

open Xunit
open Freestylecoding.Math.Types

module Addition =
    [<Theory>]
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
        Assert.Equal( Natural([8u; 0u; 0u]), Natural([1u]) <<< 67 )

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
        Assert.Equal( Natural([1u]), Natural([0x10u; 0u; 0u]) >>> 68 )
        
    [<Fact>]
    let ReduceToZero () =
        Assert.Equal( Natural([0u]), Natural([0x10u; 0u; 0u]) >>> 99 )

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
        Assert.Equal( Natural([1u]), Natural([0xFu; 0x00000101u]) &&& Natural([ 0x00010001u]) )

    [<Fact>]
    let BiggerRight () =
        Assert.Equal( Natural([1u]), Natural([0x00010001u]) &&& Natural([0xFu; 0x00000101u]) )

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
