namespace Integer

open Xunit
open Freestylecoding.Math

type public Constants() =
    [<Fact>]
    member public this.ZeroConstant () =
        Assert.Equal( Integer( [0u], false ), Integer.Zero )

    [<Fact>]
    member public this.UnitConstant () =
        Assert.Equal( Integer( [1u], false ), Integer.Unit )

    [<Fact>]
    member public this.NegativeOneConstant () =
        Assert.Equal( Integer( [1u], true ), Integer.NegativeOne )
