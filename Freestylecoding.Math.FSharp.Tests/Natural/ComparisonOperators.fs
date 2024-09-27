namespace Natural

open Xunit
open Freestylecoding.Math

type public Equality() =
    [<Theory>]
    [<InlineData( 0u, 0u, true )>]
    [<InlineData( 0u, 1u, false )>]
    [<InlineData( 1u, 0u, false )>]
    [<InlineData( 1u, 1u, true )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural.op_Equality( Natural( [left] ), Natural( [right] ) ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Natural.op_Equality( Natural( [0xBADu; 0xDEADBEEFu] ), Natural( [0xDEADBEEFu] ) ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Natural.op_Equality( Natural( [0xDEADBEEFu] ), Natural( [0xBADu; 0xDEADBEEFu] ) ) )

type public GreaterThan() =
    [<Theory>]
    [<InlineData( 0u, 1u, false )>]
    [<InlineData( 1u, 0u, true )>]
    [<InlineData( 1u, 1u, false )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural.op_GreaterThan( Natural( [left] ), Natural( [right] ) ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Natural.op_GreaterThan( Natural( [0xBADu; 0xDEADBEEFu] ), Natural( [0xDEADBEEFu] ) ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Natural.op_GreaterThan( Natural( [0xDEADBEEFu] ), Natural( [0xBADu; 0xDEADBEEFu] ) ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.True( Natural.op_GreaterThan( Natural( [1u; 1u] ), Natural( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.False( Natural.op_GreaterThan( Natural( [1u; 0u] ), Natural( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.False( Natural.op_GreaterThan( Natural( [1u; 0u] ), Natural( [1u; 1u] ) ) )

type public LessThan() =
    [<Theory>]
    [<InlineData( 0u, 1u, true )>]
    [<InlineData( 1u, 0u, false )>]
    [<InlineData( 1u, 1u, false )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural.op_LessThan( Natural( [left] ), Natural( [right] ) ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Natural.op_LessThan( Natural( [0xBADu; 0xDEADBEEFu] ), Natural( [0xDEADBEEFu] ) ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Natural.op_LessThan( Natural( [0xDEADBEEFu] ), Natural( [0xBADu; 0xDEADBEEFu] ) ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.False( Natural.op_LessThan( Natural( [1u; 1u] ), Natural( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.False( Natural.op_LessThan( Natural( [1u; 0u] ), Natural( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.True( Natural.op_LessThan( Natural( [1u; 0u] ), Natural( [1u; 1u] ) ) )

type public GreaterThanOrEqual() =
    [<Theory>]
    [<InlineData( 0u, 1u, false )>]
    [<InlineData( 1u, 0u, true )>]
    [<InlineData( 1u, 1u, true )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural.op_GreaterThanOrEqual( Natural( [left] ), Natural( [right] ) ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Natural.op_GreaterThanOrEqual( Natural( [0xBADu; 0xDEADBEEFu] ), Natural( [0xDEADBEEFu] ) ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Natural.op_GreaterThanOrEqual( Natural( [0xDEADBEEFu] ), Natural( [0xBADu; 0xDEADBEEFu] ) ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.True( Natural.op_GreaterThanOrEqual( Natural( [1u; 1u] ), Natural( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.True( Natural.op_GreaterThanOrEqual( Natural( [1u; 0u] ), Natural( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.False( Natural.op_GreaterThanOrEqual( Natural( [1u; 0u] ), Natural( [1u; 1u] ) ) )

type public LessThanOrEqual() =
    [<Theory>]
    [<InlineData( 0u, 1u, true )>]
    [<InlineData( 1u, 0u, false )>]
    [<InlineData( 1u, 1u, true )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural.op_LessThanOrEqual( Natural( [left] ), Natural( [right] ) ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Natural.op_LessThanOrEqual( Natural( [0xBADu; 0xDEADBEEFu] ), Natural( [0xDEADBEEFu] ) ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Natural.op_LessThanOrEqual( Natural( [0xDEADBEEFu] ), Natural( [0xBADu; 0xDEADBEEFu] ) ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.False( Natural.op_LessThanOrEqual( Natural( [1u; 1u] ), Natural( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.True( Natural.op_LessThanOrEqual( Natural( [1u; 0u] ), Natural( [1u; 0u] ) ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.True( Natural.op_LessThanOrEqual( Natural( [1u; 0u] ), Natural( [1u; 1u] ) ) )

type public Inequality() =
    [<Theory>]
    [<InlineData( 0u, 0u, false )>]
    [<InlineData( 0u, 1u, true )>]
    [<InlineData( 1u, 0u, true )>]
    [<InlineData( 1u, 1u, false )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural.op_Inequality( Natural( [left] ), Natural( [right] ) ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Natural.op_Inequality( Natural( [0xBADu; 0xDEADBEEFu] ), Natural( [0xDEADBEEFu] ) ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Natural.op_Inequality( Natural( [0xDEADBEEFu] ), Natural( [0xBADu; 0xDEADBEEFu] ) ) )
