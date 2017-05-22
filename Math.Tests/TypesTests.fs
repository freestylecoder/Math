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