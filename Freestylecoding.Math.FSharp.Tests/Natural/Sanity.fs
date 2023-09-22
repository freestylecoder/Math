namespace Freestylecoding.Math.FSharp.Natural.Tests

open Xunit
open Freestylecoding.Math

[<Trait( "Type", "Sanity" )>]
type public Sanity() =
    [<Fact>]
    member public this.LeftShift () =
        Assert.Equal( Natural([4u; 0x8000_0000u; 0u]), Natural([9u]) <<< 63 )

    [<Fact>]
    member public this.RightShift () =
        Assert.Equal( Natural([9u]), Natural([4u; 0x8000_0000u; 0u]) >>> 63 )

    [<Fact>]
    member public this.GreaterThanTrue () =
        Assert.True( Natural([0xDEADBEEFu; 0xBADu]) > Natural([0xBADu; 0xDEADBEEFu;]) )

    [<Fact>]
    member public this.GreaterThanFalseByLessThan () =
        Assert.False( Natural([0xBADu; 0xDEADBEEFu]) > Natural([0xDEADBEEFu; 0xBADu]) )

    [<Fact>]
    member public this.GreaterThanFalseByEquals () =
        Assert.False( Natural([0xBADu; 0xDEADBEEFu]) > Natural([0xBADu; 0xDEADBEEFu]) )

    [<Fact>]
    member public this.Addition () =
        Assert.Equal( Natural([2u; 0u; 0u]), Natural([1u; 1u]) + Natural([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue]) )

    [<Fact>]
    member public this.Subtraction () =
        Assert.Equal( Natural ([0xFFFF_FFFFu; 0xFFFF_FFFFu]), Natural([1u; 0u; 0u]) - Natural([1u]) )

    [<Fact>]
    member public this.Multiplication () =
        Assert.Equal( Natural([0x75CD9046u; 0x541D5980u]), Natural([0xFEDCBA98u]) * Natural([0x76543210u]) )

    [<Fact>]
    member public this.DivisionModulo () =
        Assert.Equal( (Natural([0xFEDCBA98u]),Natural([0x12345678u])), Natural([0x75CD9046u; 0x6651AFF8u]) /% Natural([0x76543210u]) )

    [<Fact>]
    member public this.ToString () =
        Assert.Equal( "1234567890123456789", Natural([0x112210F4u; 0x7DE98115u]).ToString() )

    [<Fact>]
    member public this.Parse () =
        Assert.Equal( Natural([0x112210F4u; 0x7DE98115u] ), Natural.Parse("1234567890123456789") )
