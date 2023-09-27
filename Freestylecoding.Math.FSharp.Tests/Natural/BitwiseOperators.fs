namespace Natural

open Xunit
open Freestylecoding.Math

type public BitwiseAnd() =
    [<Theory>]
    [<InlineData(  0u,  0u, 0u)>]
    [<InlineData(  1u,  0u, 0u)>]
    [<InlineData(  0u,  1u, 0u)>]
    [<InlineData(  1u,  1u, 1u)>]
    [<InlineData( 12u, 10u, 8u)>]
    member public this.Sanity left right expected =
        Assert.Equal( Natural( [expected] ), Natural( [left] ) &&& Natural( [right] ) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.Equal( Natural.Unit, Natural([0xFu; 0x00000101u]) &&& Natural([ 0x00010001u]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.Equal( Natural.Unit, Natural([0x00010001u]) &&& Natural([0xFu; 0x00000101u]) )

    [<FactAttribute>]
    member public this.Large () =
        Assert.Equal( Natural( [1u; 0u; 0u; 0u] ), Natural( [1u; 1u; 0u; 0u] ) &&& Natural( [1u; 0u; 1u; 0u] ) )

type public BitwiseOr() =
    [<Theory>]
    [<InlineData(  0u,  0u,  0u)>]
    [<InlineData(  1u,  0u,  1u)>]
    [<InlineData(  0u,  1u,  1u)>]
    [<InlineData(  1u,  1u,  1u)>]
    [<InlineData( 12u, 10u, 14u)>]
    member public this.Sanity left right expected =
        Assert.Equal( Natural([expected]), Natural([left]) ||| Natural([right]) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.Equal( Natural([0xFu; 0x10101u]), Natural([0xFu; 0x00000101u]) ||| Natural([ 0x00010001u]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.Equal( Natural([0xFu; 0x10101u]), Natural([0x00010001u]) ||| Natural([0xFu; 0x00000101u]) )

    [<FactAttribute>]
    member public this.Large () =
        Assert.Equal( Natural( [1u; 1u; 1u; 0u] ), Natural( [1u; 1u; 0u; 0u] ) ||| Natural( [1u; 0u; 1u; 0u] ) )

type public BitwiseXor() =
    [<Theory>]
    [<InlineData(  0u,  0u, 0u)>]
    [<InlineData(  1u,  0u, 1u)>]
    [<InlineData(  0u,  1u, 1u)>]
    [<InlineData(  1u,  1u, 0u)>]
    [<InlineData( 12u, 10u, 6u)>]
    member public this.Sanity left right expected =
        Assert.Equal( Natural([expected]), Natural([left]) ^^^ Natural([right]) )

    [<Fact>]
    member public this.BiggerLeft () =
        Assert.Equal( Natural([0xFu; 0x10100u]), Natural([0xFu; 0x00000101u]) ^^^ Natural([0x00010001u]) )

    [<Fact>]
    member public this.BiggerRight () =
        Assert.Equal( Natural([0xFu; 0x10100u]), Natural([0x00010001u]) ^^^ Natural([0xFu; 0x00000101u]) )

    [<FactAttribute>]
    member public this.Large () =
        Assert.Equal( Natural( [0u; 1u; 1u; 0u] ), Natural( [1u; 1u; 0u; 0u] ) ^^^ Natural( [1u; 0u; 1u; 0u] ) )

type public BitwiseNot() =
    [<Theory>]
    [<InlineData( 0xFFFF_FFFEu, 1u)>]
    [<InlineData( 1u, 0xFFFF_FFFEu)>]
    member public this.Sanity value expected =
        Assert.Equal( Natural([expected]), ~~~ Natural([value]) )

    [<Fact>]
    member public this.Bigger () =
        Assert.Equal( Natural([0xF012_3456u; 0x789A_BCDEu]), ~~~ Natural([0x0FED_CBA9u; 0x8765_4321u]) )

type public LeftShift() =
    [<Theory>]
    [<InlineData(   1u, 1,    2u )>]        // Sanity
    [<InlineData( 0xFu, 2, 0x3Cu )>]   // multiple bits
    member public this.Sanity left right expected =
        Assert.Equal( Natural([expected]), Natural([left]) <<< right )
    
    [<Fact>]
    member public this.Overflow () =
        Assert.Equal( Natural([1u; 0xFFFFFFFEu]), Natural([0xFFFFFFFFu]) <<< 1 )
    
    [<Fact>]
    member public this.MultipleOverflow () =
        Assert.Equal( Natural([0x5u; 0xFFFFFFF8u]), Natural([0xBFFFFFFFu]) <<< 3 )
    
    [<Fact>]
    member public this.OverOneUInt () =
        Assert.Equal( Natural([8u; 0u; 0u]), Natural.Unit <<< 67 )

type public RightShift() =
    [<Theory>]
    [<InlineData(    1u, 1,   0u )>]        // Sanity
    [<InlineData(  0xFu, 2, 0x3u )>]    // multiple bits
    [<InlineData( 0x3Cu, 2, 0xFu )>]   // multiple bits
    member public this.Sanity left right expected =
        Assert.Equal( Natural([expected]), Natural([left]) >>> right )

    [<Fact>]
    member public this.Underflow () =
        Assert.Equal( Natural([0x7FFFFFFFu]), Natural([0xFFFFFFFFu]) >>> 1 )
    
    [<Fact>]
    member public this.MultipleUnderflow () =
        Assert.Equal( Natural([0x1u; 0x5FFFFFFFu]), Natural([0xAu; 0xFFFFFFFFu]) >>> 3 )
    
    [<Fact>]
    member public this.OverOneUInt () =
        Assert.Equal( Natural.Unit, Natural([0x10u; 0u; 0u]) >>> 68 )
    
    [<Fact>]
    member public this.ReduceToZero () =
        Assert.Equal( Natural.Zero, Natural([0x10u; 0u; 0u]) >>> 99 )
