namespace Integer

open Xunit
open Freestylecoding.Math

type public BitwiseAnd () =
    [<Theory>]
    [<InlineData( 0, 0, 0)>]
    [<InlineData( 1, 0, 0)>]
    [<InlineData( 0, 1, 0)>]
    [<InlineData( 1, 1, 1)>]
    [<InlineData( -1, 1, 1)>]
    [<InlineData( 1, -1, 1)>]
    [<InlineData( -1, -1, -1)>]
    member public this.Sanity (l:int) (r:int) (x:int) =
        Assert.Equal( Integer(x), Integer(l) &&& Integer(r) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.Equal( Integer.Unit, Integer([0xFu; 0x00000101u]) &&& Integer([ 0x00010001u]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.Equal( Integer.Unit, Integer([0x00010001u]) &&& Integer([0xFu; 0x00000101u]) )

    [<Fact>]
    member public this.Large () =
        Assert.Equal( Integer( [1u; 0u; 0u; 0u] ), Integer( [1u; 1u; 0u; 0u] ) &&& Integer( [1u; 0u; 1u; 0u] ) )

type public BitwiseOr () =
    [<Theory>]
    [<InlineData( 0, 0, 0)>]
    [<InlineData( 1, 0, 1)>]
    [<InlineData( 0, 1, 1)>]
    [<InlineData( 1, 1, 1)>]
    [<InlineData( -1, 1, -1)>]
    [<InlineData( 1, -1, -1)>]
    [<InlineData( -1, -1, -1)>]
    member public this.Sanity (l:int) (r:int) (x:int) =
        Assert.Equal( Integer(x), Integer(l) ||| Integer(r) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.Equal( Integer([0xFu; 0x10101u]), Integer([0xFu; 0x00000101u]) ||| Integer([ 0x00010001u]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.Equal( Integer([0xFu; 0x10101u]), Integer([0x00010001u]) ||| Integer([0xFu; 0x00000101u]) )

    [<Fact>]
    member public this.Large () =
        Assert.Equal( Integer( [1u; 1u; 1u; 0u] ), Integer( [1u; 1u; 0u; 0u] ) ||| Integer( [1u; 0u; 1u; 0u] ) )

type public BitwiseXor () =
    [<Theory>]
    [<InlineData( 0, 0, 0)>]
    [<InlineData( 1, 0, 1)>]
    [<InlineData( 0, 1, 1)>]
    [<InlineData( 1, 1, 0)>]
    [<InlineData( -1, 0, -1)>]
    [<InlineData( 0, -1, -1)>]
    [<InlineData( -1, -1, 0)>]
    [<InlineData( -1, 1, 0)>]
    [<InlineData( 1, -1, 0)>]
    [<InlineData( 1, 2, 3)>]
    [<InlineData( -1, 2, -3)>]
    [<InlineData( 1, -2, -3)>]
    [<InlineData( -1, -2, 3)>]
    member public this.Sanity (l:int) (r:int) (x:int) =
        Assert.Equal( Integer(x), Integer(l) ^^^ Integer(r) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.Equal( Integer([0xFu; 0x10100u]), Integer([0xFu; 0x00000101u]) ^^^ Integer([0x00010001u]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.Equal( Integer([0xFu; 0x10100u]), Integer([0x00010001u]) ^^^ Integer([0xFu; 0x00000101u]) )

    [<Fact>]
    member public this.Large () =
        Assert.Equal( Integer( [0u; 1u; 1u; 0u] ), Integer( [1u; 1u; 0u; 0u] ) ^^^ Integer( [1u; 0u; 1u; 0u] ) )

type public BitwiseNot () =
    [<Theory>]
    [<InlineData( 0xFFFFFFFEu, 1u)>]
    [<InlineData( 1u, 0xFFFFFFFEu)>]
    member public this.Sanity r x =
        Assert.Equal( Integer([x], true), ~~~ Integer([r], false) )

    [<Fact>]
    member public this.Bigger () =
        Assert.Equal( Integer([0xF0123456u; 0x789ABCDEu], true), ~~~ Integer([0x0FEDCBA9u; 0x87654321u], false) )

    [<Theory>]
    [<InlineData( true, false )>]
    [<InlineData( false, true )>]
    member public this.Sign expected initial =
        Assert.Equal( Integer([0xF0123456u; 0x789ABCDEu], expected), ~~~ Integer([0x0FEDCBA9u; 0x87654321u], initial) )

type public LeftShift () =
    [<Theory>]
    [<InlineData( 1, 1, 2 )>]
    [<InlineData( -1, 1, -2 )>]
    member public this.Sanity (l:int) (r:int) (s:int) =
        Assert.Equal( Integer( s ), Integer( l ) <<< r )
    
    [<Fact>]
    member public this.Multiple () =
        Assert.Equal( Integer([0x3Cu]), Integer([0xFu]) <<< 2 )

    [<Fact>]
    member public this.Overflow () =
        Assert.Equal( Integer([1u; 0xFFFFFFFEu]), Integer([0xFFFFFFFFu]) <<< 1 )
    
    [<Fact>]
    member public this.MultipleOverflow () =
        Assert.Equal( Integer([0x5u; 0xFFFFFFF8u]), Integer([0xBFFFFFFFu]) <<< 3 )
    
    [<Fact>]
    member public this.OverOneUInt () =
        Assert.Equal( Integer([8u; 0u; 0u]), Integer.Unit <<< 67 )

    [<Fact>]
    member public this.OverflowNegative () =
        Assert.Equal( Integer([1u; 0xFFFFFFFEu], true), Integer([0xFFFFFFFFu], true) <<< 1 )
        
    [<Fact>]
    member public this.MultipleOverflowNegative () =
        Assert.Equal( Integer([0x5u; 0xFFFFFFF8u], true), Integer([0xBFFFFFFFu], true) <<< 3 )
        
    [<Fact>]
    member public this.OverOneUIntNegative () =
        Assert.Equal( Integer([8u; 0u; 0u], true), Integer( Natural.Unit, true ) <<< 67 )

type public RightShift () =
    [<Theory>]
    [<InlineData( 1, 1, 0 )>]
    [<InlineData( 15, 2, 3 )>]
    [<InlineData( 60, 2, 15 )>]
    [<InlineData( 60, 1, 30 )>]
    [<InlineData( -1, 1, 0 )>]
    [<InlineData( -2, 1, -1 )>]
    [<InlineData( -15, 2, -3 )>]
    [<InlineData( -60, 2, -15 )>]
    [<InlineData( -60, 1, -30 )>]
    member public this.Sanity (l:int) (r:int) (s:int) =
        Assert.Equal( Integer( s ), Integer( l ) >>> r )

    [<Fact>]
    member public this.Underflow () =
        Assert.Equal( Integer([0x7FFFFFFFu]), Integer([0xFFFFFFFFu]) >>> 1 )
    
    [<Fact>]
    member public this.MultipleUnderflow () =
        Assert.Equal( Integer([0x1u; 0x5FFFFFFFu]), Integer([0xAu; 0xFFFFFFFFu]) >>> 3 )
    
    [<Fact>]
    member public this.OverOneUInt () =
        Assert.Equal( Integer.Unit, Integer([0x10u; 0u; 0u]) >>> 68 )

    [<Fact>]
    member public this.ReduceToZero () =
        Assert.Equal( Integer.Zero, Integer([0x10u; 0u; 0u]) >>> 99 )

    [<Fact>]
    member public this.UnderflowNegative () =
        Assert.Equal( Integer([0x7FFFFFFFu], true), Integer([0xFFFFFFFFu], true) >>> 1 )
    
    [<Fact>]
    member public this.MultipleUnderflowNegative () =
        Assert.Equal( Integer([0x1u; 0x5FFFFFFFu], true), Integer([0xAu; 0xFFFFFFFFu], true) >>> 3 )
    
    [<Fact>]
    member public this.OverOneUIntNegative () =
        Assert.Equal( Integer( Natural.Unit, true ), Integer([0x10u; 0u; 0u], true) >>> 68 )
    
    [<Fact>]
    member public this.RetainsNegative () =
        Assert.Equal( Integer([1u], true ), Integer([2u], true) >>> 1 )

    [<Fact>]
    member public this.NegativeReduceToZero () =
        Assert.Equal( Integer.Zero, Integer([0x10u; 0u; 0u], true) >>> 99 )
