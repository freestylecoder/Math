namespace Natural

open Xunit
open Freestylecoding.Math

type public ComparisonOperatorsNaturalNaturalBool() =
    inherit Freestylecoding.Math.Interface.Tests.TComparisonOperatorsSelf<Natural>()

    override _.self0 = Natural.Zero
    override _.self1 = Natural.Unit
    override _.self2 = Natural( uint System.Int32.MaxValue )
    override _.self3 = Natural( System.UInt32.MaxValue )

    [<Fact>]
    member public this.GT_BiggerLeft () =
        Assert.True(
            this.GreaterThan(
                Natural( [0xBADu; 0xDEADBEEFu] ),
                Natural( [0xDEADBEEFu] )
            )
        )

    [<Fact>]
    member public this.GT_BiggerRight () =
        Assert.False(
            this.GreaterThan(
                Natural( [0xDEADBEEFu] ),
                Natural( [0xBADu; 0xDEADBEEFu] )
            )
        )
    
    [<Fact>]
    member public this.GT_CascadeGreaterThan () =
        Assert.True(
            this.GreaterThan(
                Natural( [1u; 1u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.GT_CascadeEqual () =
        Assert.False(
            this.GreaterThan(
                Natural( [1u; 0u] ),
                Natural( [1u; 0u] )
            )
        )    

    [<Fact>]
    member public this.GT_CascadeLessThan () =
        Assert.False(
            this.GreaterThan(
                Natural( [1u; 0u] ),
                Natural( [1u; 1u] )
            )
        )

    [<Fact>]
    member public this.LT_BiggerLeft () =
        Assert.False(
            this.LessThan(
                Natural( [0xBADu; 0xDEADBEEFu] ),
                Natural( [0xDEADBEEFu] )
            )
        )

    [<Fact>]
    member public this.LT_BiggerRight () =
        Assert.True(
            this.LessThan(
                Natural( [0xDEADBEEFu] ),
                Natural( [0xBADu; 0xDEADBEEFu] )
            )
        )
    
    [<Fact>]
    member public this.LT_CascadeGreaterThan () =
        Assert.False(
            this.LessThan(
                Natural( [1u; 1u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.LT_CascadeEqual () =
        Assert.False(
            this.LessThan(
                Natural( [1u; 0u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.LT_CascadeLessThan () =
        Assert.True(
            this.LessThan(
                Natural( [1u; 0u] ),
                Natural( [1u; 1u] )
            )
        )

    [<Fact>]
    member public this.GTE_BiggerLeft () =
        Assert.True(
            this.GreaterThanOrEqual(
                Natural( [0xBADu; 0xDEADBEEFu] ),
                Natural( [0xDEADBEEFu] )
            )
        )

    [<Fact>]
    member public this.GTE_BiggerRight () =
        Assert.False(
            this.GreaterThanOrEqual(
                Natural( [0xDEADBEEFu] ),
                Natural( [0xBADu; 0xDEADBEEFu] )
            )
        )
    
    [<Fact>]
    member public this.GTE_CascadeGreaterThan () =
        Assert.True(
            this.GreaterThanOrEqual(
                Natural( [1u; 1u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.GTE_CascadeEqual () =
        Assert.True(
            this.GreaterThanOrEqual(
                Natural( [1u; 0u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.GTE_CascadeLessThan () =
        Assert.False(
            this.GreaterThanOrEqual(
                Natural( [1u; 0u] ),
                Natural( [1u; 1u] )
            )
        )

    [<Fact>]
    member public this.LTE_BiggerLeft () =
        Assert.False(
            this.LessThanOrEqual(
                Natural( [0xBADu; 0xDEADBEEFu] ),
                Natural( [0xDEADBEEFu] )
            )
        )

    [<Fact>]
    member public this.LTE_BiggerRight () =
        Assert.True(
            this.LessThanOrEqual(
                Natural( [0xDEADBEEFu] ),
                Natural( [0xBADu; 0xDEADBEEFu] )
            )
        )
    
    [<Fact>]
    member public this.LTE_CascadeGreaterThan () =
        Assert.False(
            this.LessThanOrEqual(
                Natural( [1u; 1u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.LTE_CascadeEqual () =
        Assert.True(
            this.LessThanOrEqual(
                Natural( [1u; 0u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.LTE_CascadeLessThan () =
        Assert.True(
            this.LessThanOrEqual(
                Natural( [1u; 0u] ),
                Natural( [1u; 1u] )
            )
        )
