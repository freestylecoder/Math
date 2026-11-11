namespace Integer

open Xunit
open Freestylecoding.Math

type public Addition () =
    [<Theory>]
    [<InlineData(  1,  1,   2 )>]        // Sanity
    [<InlineData( -1,  1,   0 )>]        // Sanity
    [<InlineData(  1, -1,   0 )>]        // Sanity
    [<InlineData( -1, -1,  -2 )>]        // Sanity
    [<InlineData(  1,  0,   1 )>]        // Sanity
    [<InlineData( -1,  0,  -1 )>]        // Sanity
    [<InlineData(  0,  1,   1 )>]        // Sanity
    [<InlineData(  0, -1,  -1 )>]        // Sanity
    [<InlineData(  1,  5,   6 )>]        // l < r
    [<InlineData( -1,  5,   4 )>]        // l < r
    [<InlineData(  1, -5,  -4 )>]        // l < r
    [<InlineData( -1, -5,  -6 )>]        // l < r
    [<InlineData(  9,  2,  11 )>]        // l > r
    [<InlineData( -9,  2,  -7 )>]        // l > r
    [<InlineData(  9, -2,   7 )>]        // l > r
    [<InlineData( -9, -2, -11 )>]        // l > r
    member public this.Sanity (l:int) (r:int) (s:int) =
        Assert.Equal( Integer( s ), Integer( l ) + Integer( r ) )

    [<Fact>]
    member public this.Overflow () =
        Assert.Equal( Integer([1u; 2u]), Integer([System.UInt32.MaxValue]) + Integer([3u]) )

    [<Fact>]
    member public this.leftBiggerNoOverflow () =
        Assert.Equal( Integer([0xFu; 0xFF0Fu]), Integer([0xFu; 0xFu]) + Integer([0xFF00u]) )
    
    [<Fact>]
    member public this.rightBiggerNoOverflow () =
        Assert.Equal( Integer ([0xFu; 0xFF0Fu]), Integer([0xFF00u]) + Integer([0xFu; 0xFu]) )
    
    [<Fact>]
    member public this.CascadingOverflow () =
        Assert.Equal( Integer([1u; 0u; 0u]), Integer([1u]) + Integer([System.UInt32.MaxValue; System.UInt32.MaxValue]) )

    [<Fact>]
    member public this.OverflowCausedOverflow () =
        Assert.Equal( Integer([2u; 0u; 0u]), Integer([1u; 1u]) + Integer([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue]) )

    [<Fact>]
    member public this.OverflowNegative () =
        Assert.Equal( Integer([1u; 2u], true), Integer([System.UInt32.MaxValue], true) + Integer([3u], true) )

    [<Fact>]
    member public this.leftBiggerNoOverflowNegative () =
        Assert.Equal( Integer([0xFu; 0xFF0Fu], true), Integer([0xFu; 0xFu], true) + Integer([0xFF00u], true) )

    [<Fact>]
    member public this.rightBiggerNoOverflowNegative () =
        Assert.Equal( Integer ([0xFu; 0xFF0Fu], true), Integer([0xFF00u], true) + Integer([0xFu; 0xFu], true) )

    [<Fact>]
    member public this.CascadingOverflowNegative () =
        Assert.Equal( Integer([1u; 0u; 0u], true), Integer([1u], true) + Integer([System.UInt32.MaxValue; System.UInt32.MaxValue], true) )

    [<Fact>]
    member public this.OverflowCausedOverflowNegative () =
        Assert.Equal( Integer([2u; 0u; 0u], true), Integer([1u; 1u], true) + Integer([1u; System.UInt32.MaxValue - 1u; System.UInt32.MaxValue], true) )

type public Subtraction () =
    [<Theory>]
    [<InlineData(  1,  1,   0 )>]             // Sanity
    [<InlineData( -1,  1,  -2 )>]             // Sanity
    [<InlineData(  1, -1,   2 )>]             // Sanity
    [<InlineData( -1, -1,   0 )>]             // Sanity
    [<InlineData(  1,  0,   1 )>]             // Sanity
    [<InlineData( -1,  0,  -1 )>]             // Sanity
    [<InlineData(  0,  1,  -1 )>]             // Sanity
    [<InlineData(  0, -1,   1 )>]             // Sanity
    [<InlineData(  9,  2,   7 )>]             // l > r
    [<InlineData( -9,  2, -11 )>]             // l > r
    [<InlineData(  9, -2,  11 )>]             // l > r
    [<InlineData( -9, -2,  -7 )>]             // l > r
    [<InlineData(  1,  5,  -4 )>]             // l < r
    [<InlineData( -1,  5,  -6 )>]             // l < r
    [<InlineData(  1, -5,   6 )>]             // l < r
    [<InlineData( -1, -5,   4 )>]             // l < r
    member public this.Sanity (l:int) (r:int) (s:int) =
        Assert.Equal( Integer( s ), Integer( l ) - Integer( r ) )

    [<Fact>]
    member public this.MultiItemNoUnderflow () =
        let l = Integer([3u; 4u])
        let r = Integer([1u; 2u])
        Assert.Equal( Integer([2u; 2u]), l - r )

    [<Fact>]
    member public this.MultiItemNegativeNoUnderflow () =
        let l = Integer([3u; 4u], true)
        let r = Integer([1u; 2u], true)
        Assert.Equal( Integer([2u; 2u], true), l - r )

    [<Fact>]
    member public this.MultiItemMixedLeftNoUnderflow () =
        let l = Integer([3u; 4u], true)
        let r = Integer([1u; 2u])
        Assert.Equal( Integer([4u; 6u], true), l - r )

    [<Fact>]
    member public this.MultiItemMixedRightNoUnderflow () =
        let l = Integer([3u; 4u])
        let r = Integer([1u; 2u], true)
        Assert.Equal( Integer([4u; 6u]), l - r )

    [<Fact>]
    member public this.MultiItemUnderflow () =
        let l = Integer ([4u; 2u])
        let r = Integer ([1u; 3u])
        Assert.Equal( Integer ([0x2u; 0xFFFFFFFFu]), l - r )
    
    [<Fact>]
    member public this.MultiItemCascadingUnderflow () =
        let l = Integer ([1u; 0u; 0u])
        let r = Integer ([1u])
        Assert.Equal( Integer ([0xFFFFFFFFu; 0xFFFFFFFFu]), l - r )

    [<Fact>]
    member public this.MultiItemNegativeOverflow () =
        let l = Integer([0x2u; 0xFFFFFFFFu], true)
        let r = Integer([1u; 3u])
        Assert.Equal( Integer([4u; 2u], true), l - r )
    
    [<Fact>]
    member public this.MultiItemCascadingNegativeOverflow () =
        let l = Integer ([0xFFFFFFFFu; 0xFFFFFFFFu], true)
        let r = Integer ([1u])
        Assert.Equal( Integer([1u; 0u; 0u], true), l - r )

type public Multiply () =
    [<Theory>]
    [<InlineData(  1,  1,   1 )>]       // Sanity
    [<InlineData( -1,  1,  -1 )>]       // Sanity
    [<InlineData(  1, -1,  -1 )>]       // Sanity
    [<InlineData( -1, -1,   1 )>]       // Sanity
    [<InlineData(  1,  0,   0 )>]       // Sanity
    [<InlineData( -1,  0,   0 )>]       // Sanity
    [<InlineData(  0,  1,   0 )>]       // Sanity
    [<InlineData(  0, -1,   0 )>]       // Sanity
    [<InlineData(  6,  7,  42 )>]       // multiple bits
    [<InlineData( -6,  7, -42 )>]       // multiple bits
    [<InlineData(  6, -7, -42 )>]       // multiple bits
    [<InlineData( -6, -7,  42 )>]       // multiple bits
    member public this.Sanity (l:int) (r:int) (p:int) =
        Assert.Equal( Integer( p ), Integer( l ) * Integer( r ) )

    [<Fact>]
    member public this.Big () =
        Assert.Equal( Integer([0x75CD9046u; 0x541D5980u]), Integer([0xFEDCBA98u]) * Integer([0x76543210u]) )

    [<Fact>]
    member public this.BigMixedLeft () =
        Assert.Equal( Integer([0x75CD9046u; 0x541D5980u], true), Integer([0xFEDCBA98u], true) * Integer([0x76543210u]) )

    [<Fact>]
    member public this.BigMixedRight () =
        Assert.Equal( Integer([0x75CD9046u; 0x541D5980u], true), Integer([0xFEDCBA98u]) * Integer([0x76543210u], true) )

    [<Fact>]
    member public this.BigNegative () =
        Assert.Equal( Integer([0x75CD9046u; 0x541D5980u]), Integer([0xFEDCBA98u], true) * Integer([0x76543210u], true) )

type public Division () =
    [<Theory>]
    [<InlineData(   1,   1,   1 )>]      // Sanity
    [<InlineData(  -1,   1,  -1 )>]      // Sanity
    [<InlineData(   1,  -1,  -1 )>]      // Sanity
    [<InlineData(  -1,  -1,   1 )>]      // Sanity
    [<InlineData(   0,   1,   0 )>]      // Sanity
    [<InlineData(   0,  -1,   0 )>]      // Sanity
    [<InlineData(  44,   7,   6 )>]      // multiple bits
    [<InlineData( -44,   7,  -6 )>]      // multiple bits
    [<InlineData(  44,  -7,  -6 )>]      // multiple bits
    [<InlineData( -44,  -7,   6 )>]      // multiple bits
    [<InlineData(  52,   5,  10 )>]      // rev
    [<InlineData( -52,   5, -10 )>]      // rev
    [<InlineData(  52,  -5, -10 )>]      // rev
    [<InlineData( -52,  -5,  10 )>]      // rev
    [<InlineData(  52,  10,   5 )>]      // rev
    [<InlineData( -52,  10,  -5 )>]      // rev
    [<InlineData(  52, -10,  -5 )>]      // rev
    [<InlineData( -52, -10,   5 )>]      // rev
    member public this.Sanity (dividend:int) (divisor:int) (quotient:int) =
        Assert.Equal( Integer( quotient ), Integer( dividend ) / Integer( divisor ) )

    [<Fact>]
    member public this.Zero () =
        Assert.Equal( Integer([0u]), Integer([5u]) / Integer([10u]) )

    [<Fact>]
    member public this.DivideByZero () =
        Assert.Throws<System.DivideByZeroException>( fun () -> ( Integer.Unit / Integer.Zero ) |> ignore )

    [<Fact>]
    member public this.Big () =
        Assert.Equal( Integer([0xFEDCBA98u]), Integer([0x75CD9046u; 0x6651AFF8u]) / Integer([0x76543210u]) )

    [<Fact>]
    member public this.BigMixedLeft () =
        Assert.Equal( Integer([0xFEDCBA98u], true), Integer([0x75CD9046u; 0x6651AFF8u], true) / Integer([0x76543210u]) )

    [<Fact>]
    member public this.BigMixedRight () =
        Assert.Equal( Integer([0xFEDCBA98u], true), Integer([0x75CD9046u; 0x6651AFF8u]) / Integer([0x76543210u], true) )

    [<Fact>]
    member public this.BigNegative () =
        Assert.Equal( Integer([0xFEDCBA98u]), Integer([0x75CD9046u; 0x6651AFF8u], true) / Integer([0x76543210u], true) )

type public Modulo () =
    [<Theory>]
    [<InlineData(   1,   1,  0 )>]      // Sanity
    [<InlineData(  -1,   1,  0 )>]      // Sanity
    [<InlineData(   1,  -1,  0 )>]      // Sanity
    [<InlineData(  -1,  -1,  0 )>]      // Sanity
    [<InlineData(   0,   1,  0 )>]      // Sanity
    [<InlineData(   0,  -1,  0 )>]      // Sanity
    [<InlineData(  44,   7,  2 )>]      // multiple bits
    [<InlineData( -44,   7, -2 )>]      // multiple bits
    [<InlineData(  44,  -7,  2 )>]      // multiple bits
    [<InlineData( -44,  -7, -2 )>]      // multiple bits
    [<InlineData(  52,   5,  2 )>]      // rev
    [<InlineData( -52,   5, -2 )>]      // rev
    [<InlineData(  52,  -5,  2 )>]      // rev
    [<InlineData( -52,  -5, -2 )>]      // rev
    [<InlineData(  52,  10,  2 )>]      // rev
    [<InlineData( -52,  10, -2 )>]      // rev
    [<InlineData(  52, -10,  2 )>]      // rev
    [<InlineData( -52, -10, -2 )>]      // rev
    member public this.Sanity (dividend:int) (divisor:int) (remainder:int) =
        Assert.Equal( Integer( remainder ), Integer( dividend ) % Integer( divisor ) )

    [<Fact>]
    member public this.Zero () =
        Assert.Equal( Integer([0u]), Integer([20u]) % Integer([10u]) )

    [<Fact>]
    member public this.DivideByZero () =
        Assert.Throws<System.DivideByZeroException>( fun () -> ( Integer.Unit % Integer.Zero ) |> ignore )

    [<Fact>]
    member public this.Big () =
        Assert.Equal( Integer([0x12345678u]), Integer([0x75CD9046u; 0x6651AFF8u]) % Integer([0x76543210u]) )

    [<Fact>]
    member public this.BigMixedLeft () =
        Assert.Equal( Integer([0x12345678u], true), Integer([0x75CD9046u; 0x6651AFF8u], true) % Integer([0x76543210u]) )

    [<Fact>]
    member public this.BigMixedRight () =
        Assert.Equal( Integer([0x12345678u]), Integer([0x75CD9046u; 0x6651AFF8u]) % Integer([0x76543210u], true) )

    [<Fact>]
    member public this.BigNegative () =
        Assert.Equal( Integer([0x12345678u], true), Integer([0x75CD9046u; 0x6651AFF8u], true) % Integer([0x76543210u], true) )

type public DivisionModulo () =
    [<Theory>]
    [<InlineData(   1,   1,   1,  0 )>]      // Sanity
    [<InlineData(  -1,   1,  -1,  0 )>]      // Sanity
    [<InlineData(   1,  -1,  -1,  0 )>]      // Sanity
    [<InlineData(  -1,  -1,   1,  0 )>]      // Sanity
    [<InlineData(   0,   1,   0,  0 )>]      // Sanity
    [<InlineData(   0,  -1,   0,  0 )>]      // Sanity
    [<InlineData(  44,   7,   6,  2 )>]      // multiple bits
    [<InlineData( -44,   7,  -6, -2 )>]      // multiple bits
    [<InlineData(  44,  -7,  -6,  2 )>]      // multiple bits
    [<InlineData( -44,  -7,   6, -2 )>]      // multiple bits
    [<InlineData(  52,   5,  10,  2 )>]      // rev
    [<InlineData( -52,   5, -10, -2 )>]      // rev
    [<InlineData(  52,  -5, -10,  2 )>]      // rev
    [<InlineData( -52,  -5,  10, -2 )>]      // rev
    [<InlineData(  52,  10,   5,  2 )>]      // rev
    [<InlineData( -52,  10,  -5, -2 )>]      // rev
    [<InlineData(  52, -10,  -5,  2 )>]      // rev
    [<InlineData( -52, -10,   5, -2 )>]      // rev
    member public this.Sanity (dividend:int) (divisor:int) (quotient:int) (remainder:int) =
        Assert.Equal( (Integer( quotient ),Integer( remainder )), Integer( dividend ) /% Integer( divisor ) )

    [<Fact>]
    member public this.DivideByZero () =
        Assert.Throws<System.DivideByZeroException>( fun () -> ( Integer.Unit /% Integer.Zero ) |> ignore )

    [<Fact>]
    member public this.Big () =
        Assert.Equal( (Integer([0xFEDCBA98u]),Integer([0x12345678u])), Integer([0x75CD9046u; 0x6651AFF8u]) /% Integer([0x76543210u]) )

    [<Fact>]
    member public this.BigMixedLeft () =
        Assert.Equal( (Integer([0xFEDCBA98u], true),Integer([0x12345678u], true)), Integer([0x75CD9046u; 0x6651AFF8u], true) /% Integer([0x76543210u]) )

    [<Fact>]
    member public this.BigMixedRight () =
        Assert.Equal( (Integer([0xFEDCBA98u], true),Integer([0x12345678u])), Integer([0x75CD9046u; 0x6651AFF8u]) /% Integer([0x76543210u], true) )

    [<Fact>]
    member public this.BigNegative () =
        Assert.Equal( (Integer([0xFEDCBA98u]),Integer([0x12345678u], true)), Integer([0x75CD9046u; 0x6651AFF8u], true) /% Integer([0x76543210u], true) )

type public Negation () =
    [<Theory>]
    [<InlineData(  1, -1 )>]           // Sanity
    [<InlineData( -1,  1 )>]           // Sanity
    [<InlineData(  0,  0 )>]           // Sanity
    member public this.Sanity (a:int) (e:int) =
        Assert.Equal( Integer( e ), -Integer( a ) )

    [<Fact>]
    member public this.Big () =
        Assert.Equal( Integer( [0xFEDCBA9u; 0x76543210u], true ), -Integer( [0xFEDCBA9u; 0x76543210u] )  )

    [<Fact>]
    member public this.BigNegative () =
        Assert.Equal( Integer( [0xFEDCBA9u; 0x76543210u] ), -Integer( [0xFEDCBA9u; 0x76543210u], true )  )
