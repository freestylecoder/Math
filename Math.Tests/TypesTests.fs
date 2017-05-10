namespace Freestylecoding.Math.Tests

open Xunit
open Freestylecoding.Math.Types

module Types =
    [<Theory>]
    [<InlineData( 1u, 1u, 2u )>]
    [<InlineData( 1u, 5u, 6u )>]
    [<InlineData( 9u, 2u, 11u )>]
    let NaturalAddition l r s =
        let l = Natural ([l])
        let r = Natural ([r])
        Assert.Equal( Natural ([s]), l + r )
    
    [<Fact>]
    let NaturalAdditionOverflow () =
        let l = Natural ([System.UInt32.MaxValue])
        let r = Natural ([2u])
        Assert.Equal( Natural ([1u; 1u]), l + r )
        
    [<Fact>]
    let NaturalAdditionLeftBiggerNoOverflow () =
        let l = Natural ([0xFu; 0xFu])
        let r = Natural ([0xFF00u])
        Assert.Equal( Natural ([0xFu; 0xFF0Fu]), l + r )
        
    [<Fact>]
    let NaturalAdditionRightBiggerNoOverflow () =
        let l = Natural ([0xFF00u])
        let r = Natural ([0xFu; 0xFu])
        Assert.Equal( Natural ([0xFu; 0xFF0Fu]), l + r )
        
    [<Fact>]
    let NaturalAdditionCascadingOverflow () =
        let l = Natural ([1u])
        let r = Natural ([System.UInt32.MaxValue; System.UInt32.MaxValue])
        Assert.Equal( Natural ([1u; 0u; 0u]), l + r )

    [<Fact>]
    let NaturalAdditionOverflowCausedOverflow () =
        let l = Natural ([1u; 1u])
        let r = Natural ([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue])
        Assert.Equal( Natural ([2u; 0u; 0u]), l + r )
        