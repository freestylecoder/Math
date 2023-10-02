namespace Natural

open Xunit
open Freestylecoding.Math

type public EquatableNatural() =
    static member TestMethod( this:Natural, that:Natural ) : bool =
        (this :> System.IEquatable<Natural>).Equals( that )

    [<Theory>]
    [<InlineData(  0u,  0u, true)>]
    [<InlineData(  1u,  0u, false)>]
    [<InlineData(  0u,  1u, false)>]
    [<InlineData(  1u,  1u, true)>]
    member public this.Sanity left right expected =
        Assert.Equal(
            expected,
            EquatableNatural.TestMethod( Natural( [left] ), Natural( [right] ) )
        )

    [<Fact>]
    member public this.LargeNaturalsTrue () =
        Assert.True(
            EquatableNatural.TestMethod(
                Natural( [0xFu; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.LargeNaturalsFalse () =
        Assert.False(
            EquatableNatural.TestMethod(
                Natural( [0x8u; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )
