namespace Integer

open Xunit
open Freestylecoding.Math

type public Equality() =
    [<Theory>]
    [<InlineData(  0,  0, true )>]
    [<InlineData(  0,  1, false )>]
    [<InlineData(  1,  0, false )>]
    [<InlineData(  1,  1, true )>]
    [<InlineData(  0, -1, false )>]
    [<InlineData( -1,  0, false )>]
    [<InlineData( -1,  1, false )>]
    [<InlineData(  1, -1, false )>]
    [<InlineData( -1, -1, true )>]
    member public this.Sanity (left:int) (right:int) (expected:bool) =
        Assert.Equal( expected, Integer.op_Equality( Integer( left ), Integer( right ) ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Integer.op_Equality( Integer( [0xBADu; 0xDEADBEEFu] ), Integer( [0xDEADBEEFu] ) ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Integer.op_Equality( Integer( [0xDEADBEEFu] ), Integer( [0xBADu; 0xDEADBEEFu] ) ) )

type public GreaterThan() =
    [<Theory>]
    [<InlineData(  0,  1, false )>]
    [<InlineData(  1,  0, true )>]
    [<InlineData(  0, -1, true )>]
    [<InlineData( -1,  0, false )>]
    [<InlineData(  1,  1, false )>]
    [<InlineData( -1,  1, false )>]
    [<InlineData(  1, -1, true )>]
    [<InlineData( -1, -1, false )>]
    member public this.Sanity (left:int) (right:int) (expected:bool) =
        Assert.Equal( expected, Integer.op_GreaterThan( Integer( left ), Integer( right ) ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Integer.op_GreaterThan( Integer( [0xBADu; 0xDEADBEEFu] ), Integer( [0xDEADBEEFu] ) ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Integer.op_GreaterThan( Integer( [0xDEADBEEFu] ), Integer( [0xBADu; 0xDEADBEEFu] ) ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.True( Integer.op_GreaterThan( Integer( [1u; 1u] ), Integer( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.False( Integer.op_GreaterThan( Integer( [1u; 0u] ), Integer( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.False( Integer.op_GreaterThan( Integer( [1u; 0u] ), Integer( [1u; 1u] ) ) )

    [<Fact>]
    member public this.BiggerLeftNegative () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) > Integer([0xDEADBEEFu], true) )

    [<Fact>]
    member public this.BiggerRightNegative () =
        Assert.True( Integer([0xDEADBEEFu], true) > Integer([0xBADu; 0xDEADBEEFu], true) )
    
    [<Fact>]
    member public this.CascadeGreaterThanNegative () =
        Assert.False( Integer([1u; 1u], true) > Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeEqualNegative () =
        Assert.False( Integer([1u; 0u], true) > Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeLessThanNegative () =
        Assert.True( Integer([1u; 0u], true) > Integer([1u; 1u],true) )

    [<Fact>]
    member public this.BiggerLeftMixedNegativeLeft () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) > Integer([0xDEADBEEFu], false) )

    [<Fact>]
    member public this.BiggerRightMixedNegativeLeft () =
        Assert.False( Integer([0xDEADBEEFu], true) > Integer([0xBADu; 0xDEADBEEFu], false) )
    
    [<Fact>]
    member public this.CascadeGreaterThanMixedNegativeLeft () =
        Assert.False( Integer([1u; 1u], true) > Integer([1u; 0u], false) )
    
    [<Fact>]
    member public this.CascadeEqualMixedNegativeLeft () =
        Assert.False( Integer([1u; 0u], true) > Integer([1u; 0u], false) )
    
    [<Fact>]
    member public this.CascadeLessThanMixedNegativeLeft () =
        Assert.False( Integer([1u; 0u], true) > Integer([1u; 1u], false) )

    [<Fact>]
    member public this.BiggerLeftMixedNegativeRight () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu], false) > Integer([0xDEADBEEFu], true) )

    [<Fact>]
    member public this.BiggerRightMixedNegativeRight () =
        Assert.True( Integer([0xDEADBEEFu], false) > Integer([0xBADu; 0xDEADBEEFu], true) )
    
    [<Fact>]
    member public this.CascadeGreaterThanMixedNegativeRight () =
        Assert.True( Integer([1u; 1u], false) > Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeEqualMixedNegativeRight () =
        Assert.True( Integer([1u; 0u], false) > Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeLessThanMixedNegativeRight () =
        Assert.True( Integer([1u; 0u], false) > Integer([1u; 1u], true) )

type public LessThan() =
    [<Theory>]
    [<InlineData(  0,  1, true )>]
    [<InlineData(  1,  0, false )>]
    [<InlineData(  0, -1, false )>]
    [<InlineData( -1,  0, true )>]
    [<InlineData(  1,  1, false )>]
    [<InlineData( -1,  1, true )>]
    [<InlineData(  1, -1, false )>]
    [<InlineData( -1, -1, false )>]
    member public this.Sanity (left:int) (right:int) (expected:bool) =
        Assert.Equal( expected, Integer( left ) < Integer( right ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu]) < Integer([0xDEADBEEFu]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Integer([0xDEADBEEFu]) < Integer([0xBADu; 0xDEADBEEFu]) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.False( Integer([1u; 1u]) < Integer([1u; 0u]) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.False( Integer([1u; 0u]) < Integer([1u; 0u]) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.True( Integer([1u; 0u]) < Integer([1u; 1u]) )

    [<Fact>]
    member public this.BiggerLeftNegative () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu], true) < Integer([0xDEADBEEFu], true) )

    [<Fact>]
    member public this.BiggerRightNegative () =
        Assert.False( Integer([0xDEADBEEFu], true) < Integer([0xBADu; 0xDEADBEEFu], true) )
    
    [<Fact>]
    member public this.CascadeGreaterThanNegative () =
        Assert.True( Integer([1u; 1u], true) < Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeEqualNegative () =
        Assert.False( Integer([1u; 0u], true) < Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeLessThanNegative () =
        Assert.False( Integer([1u; 0u], true) < Integer([1u; 1u],true) )

    [<Fact>]
    member public this.BiggerLeftMixedNegativeLeft () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu], true) < Integer([0xDEADBEEFu], false) )

    [<Fact>]
    member public this.BiggerRightMixedNegativeLeft () =
        Assert.True( Integer([0xDEADBEEFu], true) < Integer([0xBADu; 0xDEADBEEFu], false) )
    
    [<Fact>]
    member public this.CascadeGreaterThanMixedNegativeLeft () =
        Assert.True( Integer([1u; 1u], true) < Integer([1u; 0u], false) )
    
    [<Fact>]
    member public this.CascadeEqualMixedNegativeLeft () =
        Assert.True( Integer([1u; 0u], true) < Integer([1u; 0u], false) )
    
    [<Fact>]
    member public this.CascadeLessThanMixedNegativeLeft () =
        Assert.True( Integer([1u; 0u], true) < Integer([1u; 1u], false) )

    [<Fact>]
    member public this.BiggerLeftMixedNegativeRight () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu], false) < Integer([0xDEADBEEFu], true) )

    [<Fact>]
    member public this.BiggerRightMixedNegativeRight () =
        Assert.False( Integer([0xDEADBEEFu], false) < Integer([0xBADu; 0xDEADBEEFu], true) )
    
    [<Fact>]
    member public this.CascadeGreaterThanMixedNegativeRight () =
        Assert.False( Integer([1u; 1u], false) < Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeEqualMixedNegativeRight () =
        Assert.False( Integer([1u; 0u], false) < Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeLessThanMixedNegativeRight () =
        Assert.False( Integer([1u; 0u], false) < Integer([1u; 1u], true) )

type public GreaterThanOrEqual() =
    [<Theory>]
    [<InlineData(  0,  1, false )>]
    [<InlineData(  1,  0, true )>]
    [<InlineData(  0, -1, true )>]
    [<InlineData( -1,  0, false )>]
    [<InlineData(  1,  1, true )>]
    [<InlineData( -1,  1, false )>]
    [<InlineData(  1, -1, true )>]
    [<InlineData( -1, -1, true )>]
    member public this.Sanity (left:int) (right:int) (expected:bool) =
        Assert.Equal( expected, Integer( left ) >= Integer( right ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu]) >= Integer([0xDEADBEEFu]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Integer([0xDEADBEEFu]) >= Integer([0xBADu; 0xDEADBEEFu]) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.True( Integer([1u; 1u]) >= Integer([1u; 0u]) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.True( Integer([1u; 0u]) >= Integer([1u; 0u]) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.False( Integer([1u; 0u]) >= Integer([1u; 1u]) )

    [<Fact>]
    member public this.BiggerLeftNegative () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) >= Integer([0xDEADBEEFu], true) )

    [<Fact>]
    member public this.BiggerRightNegative () =
        Assert.True( Integer([0xDEADBEEFu], true) >= Integer([0xBADu; 0xDEADBEEFu], true) )
    
    [<Fact>]
    member public this.CascadeGreaterThanNegative () =
        Assert.False( Integer([1u; 1u], true) >= Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeEqualNegative () =
        Assert.True( Integer([1u; 0u], true) >= Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeLessThanNegative () =
        Assert.True( Integer([1u; 0u], true) >= Integer([1u; 1u],true) )

    [<Fact>]
    member public this.BiggerLeftMixedNegativeLeft () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu], true) >= Integer([0xDEADBEEFu], false) )

    [<Fact>]
    member public this.BiggerRightMixedNegativeLeft () =
        Assert.False( Integer([0xDEADBEEFu], true) >= Integer([0xBADu; 0xDEADBEEFu], false) )
    
    [<Fact>]
    member public this.CascadeGreaterThanMixedNegativeLeft () =
        Assert.False( Integer([1u; 1u], true) >= Integer([1u; 0u], false) )
    
    [<Fact>]
    member public this.CascadeEqualMixedNegativeLeft () =
        Assert.False( Integer([1u; 0u], true) >= Integer([1u; 0u], false) )
    
    [<Fact>]
    member public this.CascadeLessThanMixedNegativeLeft () =
        Assert.False( Integer([1u; 0u], true) >= Integer([1u; 1u], false) )

    [<Fact>]
    member public this.BiggerLeftMixedNegativeRight () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu], false) >= Integer([0xDEADBEEFu], true) )

    [<Fact>]
    member public this.BiggerRightMixedNegativeRight () =
        Assert.True( Integer([0xDEADBEEFu], false) >= Integer([0xBADu; 0xDEADBEEFu], true) )
    
    [<Fact>]
    member public this.CascadeGreaterThanMixedNegativeRight () =
        Assert.True( Integer([1u; 1u], false) >= Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeEqualMixedNegativeRight () =
        Assert.True( Integer([1u; 0u], false) >= Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeLessThanMixedNegativeRight () =
        Assert.True( Integer([1u; 0u], false) >= Integer([1u; 1u], true) )

type public LessThanOrEqual() =
    [<Theory>]
    [<InlineData(  0,  1, true )>]
    [<InlineData(  1,  0, false )>]
    [<InlineData(  0, -1, false )>]
    [<InlineData( -1,  0, true )>]
    [<InlineData(  1,  1, true )>]
    [<InlineData( -1,  1, true )>]
    [<InlineData(  1, -1, false )>]
    [<InlineData( -1, -1, true )>]
    member public this.Sanity (left:int) (right:int) (expected:bool) =
        Assert.Equal( expected, Integer( left ) <= Integer( right ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu]) <= Integer([0xDEADBEEFu]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Integer([0xDEADBEEFu]) <= Integer([0xBADu; 0xDEADBEEFu]) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.False( Integer([1u; 1u]) <= Integer([1u; 0u]) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.True( Integer([1u; 0u]) <= Integer([1u; 0u]) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.True( Integer([1u; 0u]) <= Integer([1u; 1u]) )

    [<Fact>]
    member public this.BiggerLeftNegative () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu], true) <= Integer([0xDEADBEEFu], true) )

    [<Fact>]
    member public this.BiggerRightNegative () =
        Assert.False( Integer([0xDEADBEEFu], true) <= Integer([0xBADu; 0xDEADBEEFu], true) )
    
    [<Fact>]
    member public this.CascadeGreaterThanNegative () =
        Assert.True( Integer([1u; 1u], true) <= Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeEqualNegative () =
        Assert.True( Integer([1u; 0u], true) <= Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeLessThanNegative () =
        Assert.False( Integer([1u; 0u], true) <= Integer([1u; 1u],true) )

    [<Fact>]
    member public this.BiggerLeftMixedNegativeLeft () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu], true) <= Integer([0xDEADBEEFu], false) )

    [<Fact>]
    member public this.BiggerRightMixedNegativeLeft () =
        Assert.True( Integer([0xDEADBEEFu], true) <= Integer([0xBADu; 0xDEADBEEFu], false) )
    
    [<Fact>]
    member public this.CascadeGreaterThanMixedNegativeLeft () =
        Assert.True( Integer([1u; 1u], true) <= Integer([1u; 0u], false) )
    
    [<Fact>]
    member public this.CascadeEqualMixedNegativeLeft () =
        Assert.True( Integer([1u; 0u], true) <= Integer([1u; 0u], false) )
    
    [<Fact>]
    member public this.CascadeLessThanMixedNegativeLeft () =
        Assert.True( Integer([1u; 0u], true) <= Integer([1u; 1u], false) )

    [<Fact>]
    member public this.BiggerLeftMixedNegativeRight () =
        Assert.False( Integer([0xBADu; 0xDEADBEEFu], false) <= Integer([0xDEADBEEFu], true) )

    [<Fact>]
    member public this.BiggerRightMixedNegativeRight () =
        Assert.False( Integer([0xDEADBEEFu], false) <= Integer([0xBADu; 0xDEADBEEFu], true) )
    
    [<Fact>]
    member public this.CascadeGreaterThanMixedNegativeRight () =
        Assert.False( Integer([1u; 1u], false) <= Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeEqualMixedNegativeRight () =
        Assert.False( Integer([1u; 0u], false) <= Integer([1u; 0u], true) )
    
    [<Fact>]
    member public this.CascadeLessThanMixedNegativeRight () =
        Assert.False( Integer([1u; 0u], false) <= Integer([1u; 1u], true) )

type public Inequality() =
    [<Theory>]
    [<InlineData(  0,  0, false )>]
    [<InlineData(  0,  1, true )>]
    [<InlineData(  1,  0, true )>]
    [<InlineData(  1,  1, false )>]
    [<InlineData(  0, -1, true )>]
    [<InlineData( -1,  0, true )>]
    [<InlineData( -1,  1, true )>]
    [<InlineData(  1, -1, true )>]
    [<InlineData( -1, -1, false )>]
    member public this.Sanity (left:int) (right:int) (expected:bool) =
        Assert.Equal( expected, Integer( left ) <> Integer( right ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Integer([0xBADu; 0xDEADBEEFu]) <> Integer([0xDEADBEEFu]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Integer([0xDEADBEEFu]) <> Integer([0xBADu; 0xDEADBEEFu]) )
