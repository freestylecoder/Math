namespace Natural

open Xunit
open Freestylecoding.Math

type public EqualityOperators() =
    inherit Freestylecoding.Math.Interface.Tests.TEqualityOperatorsSelf<Natural>()

    override _.self1  = Natural.Zero
    override _.self2  = Natural.Unit

    [<Fact>]
    member public this.EqualityLargeNaturalsTrue () =
        Assert.True(
            this.Equality(
                Natural( [0xFu; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.EqualityLargeNaturalsFalse () =
        Assert.False(
            this.Equality(
                Natural( [0x8u; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.InequalityLargeNaturalsFalse () =
        Assert.False(
            this.Inequality(
                Natural( [0xFu; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.InequalityLargeNaturalsTrue () =
        Assert.True(
            this.Inequality(
                Natural( [0x8u; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )
