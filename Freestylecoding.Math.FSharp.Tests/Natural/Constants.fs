namespace Natural

open Xunit
open Freestylecoding.Math

type public Constants() =
    [<Fact>]
    member public this.ZeroConstant () =
        Assert.Equal( Natural( [0u] ), Natural.Zero )

    [<Fact>]
    member public this.UnitConstant () =
        Assert.Equal( Natural( [1u] ), Natural.Unit )
