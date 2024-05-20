namespace Natural

open Xunit
open Freestylecoding.Math

type public EquatableNatural() =
    [<Theory>]
    [<InlineData(  0u,  0u, true)>]
    [<InlineData(  1u,  0u, false)>]
    [<InlineData(  0u,  1u, false)>]
    [<InlineData(  1u,  1u, true)>]
    member public this.Sanity left right expected =
        Assert.Equal(
            expected,
            Natural( [left] ).Equals( Natural( [right] ) )
        )

    [<Fact>]
    member public this.LargeNaturalsTrue () =
        Assert.True(
            Natural( [0xFu; 0x00000101u] ).Equals( Natural( [0xFu; 0x00000101u] ) )
        )

    [<Fact>]
    member public this.LargeNaturalsFalse () =
        Assert.False(
            Natural( [0x8u; 0x00000101u] ).Equals( Natural( [0xFu; 0x00000101u] ) )
        )
