namespace Integer

open Xunit
open Freestylecoding.Math

// NOTE: The sanity tests are here to do low level tests of a few parts
// This lets me isolate those tests so I can use those parts in the rest of the tests
//
// In other words:
// IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
[<Trait( "Type", "Sanity" )>]
type public Sanity() =
    [<Fact>]
    member public this.LeftShift () =
        Assert.Equal( Integer([4u; 0x8000_0000u; 0u], true), Integer([9u], true) <<< 63 )

    [<Fact>]
    member public this.RightShift () =
        Assert.Equal( Integer([9u], true), Integer([4u; 0x8000_0000u; 0u], true) >>> 63 )

    [<Fact>]
    member public this.GreaterThanTrue () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu]) > Integer([0xDEADBEEFu; 0xBADu], true) )

    [<Fact>]
    member public this.GreaterThanFalseByLessThan () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) > Integer([0xDEADBEEFu; 0xBADu]) )

    [<Fact>]
    member public this.GreaterThanFalseByEquals () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) > Integer([0xBADu; 0xDEADBEEFu], true) )

    [<Fact>]
    member public this.Addition () =
        Assert.Equal( Integer ([0xFFFF_FFFFu; 0xFFFF_FFFFu]), Integer([1u; 0u; 0u]) + Integer([1u], true) )

    [<Fact>]
    member public this.Subtraction () =
        Assert.Equal( Integer([2u; 0u; 0u]), Integer([1u; 1u]) - Integer([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue], true) )

    [<Fact>]
    member public this.Multiplication () =
        Assert.Equal( Integer([0x75CD9046u; 0x541D5980u], true), Integer([0xFEDCBA98u], true) * Integer([0x76543210u]) )

    [<Fact>]
    member public this.DivisionModulo () =
        Assert.Equal( (Integer([0xFEDCBA98u]),Integer([0x12345678u], true)), Integer([0x75CD9046u; 0x6651AFF8u], true) /% Integer([0x76543210u], true) )

    [<Fact>]
    member public this.ToString () =
        Assert.Equal( "-1234567890123456789", Integer([0x112210F4u; 0x7DE98115u], true).ToString() )

    [<Fact>]
    member public this.Parse () =
        Assert.Equal( Integer([0x112210F4u; 0x7DE98115u], true ), Integer.Parse("-1234567890123456789") )
