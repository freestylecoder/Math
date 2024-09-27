namespace Natural

open Xunit
open Freestylecoding.Math

type public ComparisonOperatorsNaturalNaturalBool() =
    static member private op_GT<'T when 'T :> System.Numerics.IComparisonOperators<'T,'T,bool>>( left:'T, right:'T ) =
        'T.op_GreaterThan( left, right )
    static member private op_GTE<'T when 'T :> System.Numerics.IComparisonOperators<'T,'T,bool>>( left:'T, right:'T ) =
        'T.op_GreaterThanOrEqual( left, right )
    static member private op_LT<'T when 'T :> System.Numerics.IComparisonOperators<'T,'T,bool>>( left:'T, right:'T ) =
        'T.op_LessThan( left, right )
    static member private op_LTE<'T when 'T :> System.Numerics.IComparisonOperators<'T,'T,bool>>( left:'T, right:'T ) =
        'T.op_LessThanOrEqual( left, right )

    [<Theory>]
    [<InlineData( 0u, 1u, false )>]
    [<InlineData( 1u, 0u, true )>]
    [<InlineData( 1u, 1u, false )>]
    member public this.GT_Sanity left right expected =
        Assert.Equal(
            expected,
            ComparisonOperatorsNaturalNaturalBool.op_GT(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.GT_BiggerLeft () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_GT(
                Natural( [0xBADu; 0xDEADBEEFu] ),
                Natural( [0xDEADBEEFu] )
            )
        )

    [<Fact>]
    member public this.GT_BiggerRight () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_GT(
                Natural( [0xDEADBEEFu] ),
                Natural( [0xBADu; 0xDEADBEEFu] )
            )
        )
    
    [<Fact>]
    member public this.GT_CascadeGreaterThan () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_GT(
                Natural( [1u; 1u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.GT_CascadeEqual () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_GT(
                Natural( [1u; 0u] ),
                Natural( [1u; 0u] )
            )
        )    

    [<Fact>]
    member public this.GT_CascadeLessThan () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_GT(
                Natural( [1u; 0u] ),
                Natural( [1u; 1u] )
            )
        )

    [<Theory>]
    [<InlineData( 0u, 1u, true )>]
    [<InlineData( 1u, 0u, false )>]
    [<InlineData( 1u, 1u, false )>]
    member public this.LT_Sanity left right expected =
        Assert.Equal(
            expected,
            ComparisonOperatorsNaturalNaturalBool.op_LT(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.LT_BiggerLeft () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_LT(
                Natural( [0xBADu; 0xDEADBEEFu] ),
                Natural( [0xDEADBEEFu] )
            )
        )

    [<Fact>]
    member public this.LT_BiggerRight () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_LT(
                Natural( [0xDEADBEEFu] ),
                Natural( [0xBADu; 0xDEADBEEFu] )
            )
        )
    
    [<Fact>]
    member public this.LT_CascadeGreaterThan () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_LT(
                Natural( [1u; 1u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.LT_CascadeEqual () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_LT(
                Natural( [1u; 0u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.LT_CascadeLessThan () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_LT(
                Natural( [1u; 0u] ),
                Natural( [1u; 1u] )
            )
        )

    [<Theory>]
    [<InlineData( 0u, 1u, false )>]
    [<InlineData( 1u, 0u, true )>]
    [<InlineData( 1u, 1u, true )>]
    member public this.GTE_Sanity left right expected =
        Assert.Equal(
            expected,
            ComparisonOperatorsNaturalNaturalBool.op_GTE(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.GTE_BiggerLeft () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_GTE(
                Natural( [0xBADu; 0xDEADBEEFu] ),
                Natural( [0xDEADBEEFu] )
            )
        )

    [<Fact>]
    member public this.GTE_BiggerRight () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_GTE(
                Natural( [0xDEADBEEFu] ),
                Natural( [0xBADu; 0xDEADBEEFu] )
            )
        )
    
    [<Fact>]
    member public this.GTE_CascadeGreaterThan () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_GTE(
                Natural( [1u; 1u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.GTE_CascadeEqual () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_GTE(
                Natural( [1u; 0u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.GTE_CascadeLessThan () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_GTE(
                Natural( [1u; 0u] ),
                Natural( [1u; 1u] )
            )
        )

    [<Theory>]
    [<InlineData( 0u, 1u, true )>]
    [<InlineData( 1u, 0u, false )>]
    [<InlineData( 1u, 1u, true )>]
    member public this.LTE_Sanity left right expected =
        Assert.Equal(
            expected,
            ComparisonOperatorsNaturalNaturalBool.op_LTE(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.LTE_BiggerLeft () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_LTE(
                Natural( [0xBADu; 0xDEADBEEFu] ),
                Natural( [0xDEADBEEFu] )
            )
        )

    [<Fact>]
    member public this.LTE_BiggerRight () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_LTE(
                Natural( [0xDEADBEEFu] ),
                Natural( [0xBADu; 0xDEADBEEFu] )
            )
        )
    
    [<Fact>]
    member public this.LTE_CascadeGreaterThan () =
        Assert.False(
            ComparisonOperatorsNaturalNaturalBool.op_LTE(
                Natural( [1u; 1u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.LTE_CascadeEqual () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_LTE(
                Natural( [1u; 0u] ),
                Natural( [1u; 0u] )
            )
        )
    
    [<Fact>]
    member public this.LTE_CascadeLessThan () =
        Assert.True(
            ComparisonOperatorsNaturalNaturalBool.op_LTE(
                Natural( [1u; 0u] ),
                Natural( [1u; 1u] )
            )
        )
