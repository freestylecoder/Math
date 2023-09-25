namespace Natural

open Xunit
open Freestylecoding.Math

/// <summary>Sanity tests for Natural type</summary>
/// <remarks>
///     <para>
///         The sanity tests are here to do low level tests of a few parts.
///         This lets me isolate those tests so I can use those parts in the rest of the tests.
///     </para>
///     <para>
///         In other words:
///         IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
///     </para>
/// </remarks>
[<Trait( "Type", "Sanity" )>]
type public Sanity() =
    [<Fact>]
    member public this.LeftShift () =
        Assert.Equal(
            Natural([4u; 0x8000_0000u; 0u]),
            Natural([9u]) <<< 63
        )

    [<Fact>]
    member public this.RightShift () =
        Assert.Equal(
            Natural([9u]),
            Natural([4u; 0x8000_0000u; 0u]) >>> 63
        )

    [<Fact>]
    member public this.GreaterThanTrue () =
        Assert.True(
            Natural([0xDEADBEEFu; 0xBADu]) > Natural([0xBADu; 0xDEADBEEFu;])
        )

    [<Fact>]
    member public this.GreaterThanFalseByLessThan () =
        Assert.False(
            Natural([0xBADu; 0xDEADBEEFu]) > Natural([0xDEADBEEFu; 0xBADu])
        )

    [<Fact>]
    member public this.GreaterThanFalseByEquals () =
        Assert.False(
            Natural([0xBADu; 0xDEADBEEFu]) > Natural([0xBADu; 0xDEADBEEFu])
        )

    [<Fact>]
    member public this.Addition () =
        let maxUInt = System.UInt32.MaxValue
        Assert.Equal(
            Natural([2u; 0u; 0u]),
            Natural([1u; 1u]) + Natural([1u; maxUInt - 1u; maxUInt])
        )

    [<Fact>]
    member public this.Subtraction () =
        let maxUInt = System.UInt32.MaxValue
        Assert.Equal(
            Natural ([maxUInt; maxUInt]),
            Natural([1u; 0u; 0u]) - Natural([1u])
        )

    [<Fact>]
    member public this.Multiplication () =
        Assert.Equal(
            Natural([0x75CD9046u; 0x541D5980u]),
            Natural([0xFEDCBA98u]) * Natural([0x76543210u])
        )

    [<Fact>]
    member public this.DivisionModulo () =
        Assert.Equal(
            (
                Natural([0xFEDCBA98u]),
                Natural([0x12345678u])
            ),
            Natural([0x75CD9046u; 0x6651AFF8u]) /% Natural([0x76543210u])
        )

    [<Fact>]
    member public this.ToString () =
        Assert.Equal(
            "1234567890123456789",
            Natural([0x112210F4u; 0x7DE98115u]).ToString()
        )

    [<Fact>]
    member public this.Parse () =
        Assert.Equal(
            Natural([0x112210F4u; 0x7DE98115u] ),
            Natural.Parse("1234567890123456789")
        )
