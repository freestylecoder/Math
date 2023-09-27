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
        Assert.Equal( expected, Natural( [left] ) = Natural( [right] ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Natural( [0xBADu; 0xDEADBEEFu] ) = Natural( [0xDEADBEEFu] ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Natural( [0xDEADBEEFu] ) = Natural( [0xBADu; 0xDEADBEEFu] ) )

type public GreaterThan() =
    [<Theory>]
    [<InlineData( 0u, 1u, false )>]
    [<InlineData( 1u, 0u, true )>]
    [<InlineData( 1u, 1u, false )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural( [left] ) > Natural( [right] ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Natural( [0xBADu; 0xDEADBEEFu] ) > Natural( [0xDEADBEEFu] ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Natural( [0xDEADBEEFu] ) > Natural( [0xBADu; 0xDEADBEEFu] ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.True( Natural( [1u; 1u] ) > Natural( [1u; 0u] ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.False( Natural( [1u; 0u] ) > Natural( [1u; 0u] ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.False( Natural( [1u; 0u] ) > Natural( [1u; 1u] ) )

type public LessThan() =
    [<Theory>]
    [<InlineData( 0u, 1u, true )>]
    [<InlineData( 1u, 0u, false )>]
    [<InlineData( 1u, 1u, false )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural( [left] ) < Natural( [right] ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Natural( [0xBADu; 0xDEADBEEFu] ) < Natural( [0xDEADBEEFu] ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Natural( [0xDEADBEEFu] ) < Natural( [0xBADu; 0xDEADBEEFu] ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.False( Natural( [1u; 1u] ) < Natural( [1u; 0u] ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.False( Natural( [1u; 0u] ) < Natural( [1u; 0u] ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.True( Natural( [1u; 0u] ) < Natural( [1u; 1u] ) )

type public GreaterThanOrEqual() =
    [<Theory>]
    [<InlineData( 0u, 1u, false )>]
    [<InlineData( 1u, 0u, true )>]
    [<InlineData( 1u, 1u, true )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural( [left] ) >= Natural( [right] ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Natural( [0xBADu; 0xDEADBEEFu] ) >= Natural( [0xDEADBEEFu] ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.False( Natural( [0xDEADBEEFu] ) >= Natural( [0xBADu; 0xDEADBEEFu] ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.True( Natural( [1u; 1u] ) >= Natural( [1u; 0u] ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.True( Natural( [1u; 0u] ) >= Natural( [1u; 0u] ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.False( Natural( [1u; 0u] ) >= Natural( [1u; 1u] ) )

type public LessThanOrEqual() =
    [<Theory>]
    [<InlineData( 0u, 1u, true )>]
    [<InlineData( 1u, 0u, false )>]
    [<InlineData( 1u, 1u, true )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural( [left] ) <= Natural( [right] ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.False( Natural( [0xBADu; 0xDEADBEEFu] ) <= Natural( [0xDEADBEEFu] ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Natural( [0xDEADBEEFu] ) <= Natural( [0xBADu; 0xDEADBEEFu] ) )
    
    [<Fact>]
    member public this.CascadeGreaterThan () =
        Assert.False( Natural( [1u; 1u] ) <= Natural( [1u; 0u] ) )
    
    [<Fact>]
    member public this.CascadeEqual () =
        Assert.True( Natural( [1u; 0u] ) <= Natural( [1u; 0u] ) )
    
    [<Fact>]
    member public this.CascadeLessThan () =
        Assert.True( Natural( [1u; 0u] ) <= Natural( [1u; 1u] ) )

type public Inequality() =
    [<Theory>]
    [<InlineData( 0u, 0u, false )>]
    [<InlineData( 0u, 1u, true )>]
    [<InlineData( 1u, 0u, true )>]
    [<InlineData( 1u, 1u, false )>]
    member public this.Sanity left right expected =
        Assert.Equal( expected, Natural( [left] ) <> Natural( [right] ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.True( Natural( [0xBADu; 0xDEADBEEFu] ) <> Natural( [0xDEADBEEFu] ) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.True( Natural( [0xDEADBEEFu] ) <> Natural( [0xBADu; 0xDEADBEEFu] ) )
