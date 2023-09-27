namespace Natural

open Xunit
open Freestylecoding.Math

type public Addition() =
    [<Theory>]
    [<InlineData( 0u, 0u,  0u )>]        // Sanity
    [<InlineData( 1u, 1u,  2u )>]        // Sanity
    [<InlineData( 1u, 0u,  1u )>]        // Sanity
    [<InlineData( 0u, 1u,  1u )>]        // Sanity
    [<InlineData( 1u, 5u,  6u )>]        // l < r
    [<InlineData( 9u, 2u, 11u )>]       // l > r
    member public this.Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            Natural( [left] ) + Natural( [right] )
        )
    
    [<Fact>]
    member public this.Simple () =
        Assert.Equal(
            Natural.Unit,
            Natural.Zero + Natural.Unit
        )

    [<Fact>]
    member public this.Overflow () =
        Assert.Equal(
            Natural( [1u; 2u] ),
            Natural( [0xFFFF_FFFFu] ) + Natural( [3u] )
        )
        
    [<Fact>]
    member public this.LeftBiggerNoOverflow () =
        Assert.Equal(
            Natural( [0xFu; 0xFF0Fu] ),
            Natural( [0xFu; 0xFu] ) + Natural( [0xFF00u] )
        )
        
    [<Fact>]
    member public this.RightBiggerNoOverflow () =
        Assert.Equal(
            Natural( [0xFu; 0xFF0Fu] ),
            Natural( [0xFF00u] ) + Natural( [0xFu; 0xFu] )
        )
        
    [<Fact>]
    member public this.CascadingOverflow () =
        Assert.Equal(
            Natural( [1u; 0u; 0u] ),
            Natural( [1u] ) + Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
        )

    [<Fact>]
    member public this.OverflowCausedOverflow () =
        Assert.Equal(
            Natural( [2u; 0u; 0u] ),
            Natural( [1u; 1u] ) + Natural( [1u; 0xFFFF_FFFFu - 1u; 0xFFFF_FFFFu] )
        )

    [<Fact>]
    member public this.EdgeOfOverflow () =
        Assert.Equal(
            Natural( [1u; 0xFFFF_FFFFu; 1u] ),
            Natural( [1u; 1u] ) + Natural( [1u; 0xFFFF_FFFFu - 1u; 0u] )
        )

type public Subtraction() =
    [<Theory>]
    [<InlineData( 1u, 1u, 0u )>]    // Sanity
    [<InlineData( 1u, 0u, 1u )>]    // Sanity
    [<InlineData( 9u, 2u, 7u )>]    // l > r
    member public this.Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            Natural( [left] ) - Natural( [right] )
        )
    
    [<Fact>]
    member public this.SingleItemBadUnderflow () =
        let exc = Record.Exception(
            fun () ->
                Natural.Zero - Natural.Unit
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

    [<Fact>]
    member public this.MultiItemNoUnderflow () =
        Assert.Equal(
            Natural( [2u; 2u] ),
            Natural( [3u; 4u] ) - Natural( [1u; 2u] )
        )
        
    [<Fact>]
    member public this.MultiItemSafeUnderflow () =
        Assert.Equal(
            Natural( [0x2u; 0xFFFFFFFFu] ),
            Natural( [4u; 2u] ) - Natural( [1u; 3u] )
        )
        
    [<Fact>]
    member public this.MultiItemSafeCascadingUnderflow () =
        Assert.Equal(
            Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] ),
            Natural( [1u; 0u; 0u] ) - Natural.Unit
        )
        
    [<Fact>]
    member public this.MultiItemUnsafeUnderflow () =
        let exc = Record.Exception(
            fun () ->
                Natural( [1u; 2u] ) - Natural( [1u; 3u] )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

    [<Fact>]
    member public this.LargeWithUnderflows () =
        Assert.Equal(
            Natural( [0x1u; 0xFFFF_FFFFu; 0xFFFF_FFFEu] ),
            Natural( [3u; 2u; 1u] ) - Natural( [1u; 2u; 3u] )
        )

type public Multiply() =
    [<Theory>]
    [<InlineData( 1u, 1u,  1u )>]        // Sanity
    [<InlineData( 1u, 0u,  0u )>]        // Sanity
    [<InlineData( 0u, 1u,  0u )>]        // Sanity
    [<InlineData( 6u, 7u, 42u )>]       // multiple bits
    member public this.Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            Natural( [left] ) * Natural( [right] )
        )

    [<Fact>]
    member public this.Big () =
        Assert.Equal(
            Natural( [0x75CD9046u; 0x541D5980u] ),
            Natural( [0xFEDCBA98u] ) * Natural( [0x76543210u] )
        )

type public Division() =
    [<Theory>]
    [<InlineData(  1u,  1u,  1u )>] // Sanity
    [<InlineData(  0u,  1u,  0u )>] // Sanity
    [<InlineData( 42u,  7u,  6u )>] // multiple bits
    [<InlineData( 50u,  5u, 10u )>] // rev
    [<InlineData( 50u, 10u,  5u )>] // rev
    [<InlineData( 54u,  5u, 10u )>] // has remainder
    member public this.Sanity dividend divisor quotient =
        Assert.Equal(
            Natural( [quotient] ),
            Natural( [dividend] ) / Natural( [divisor] )
        )

    [<Fact>]
    member public this.Zero () =
        Assert.Equal(
            Natural.Zero,
            Natural( [5u] ) / Natural( [10u] )
        )

    [<Fact>]
    member public this.DivideByZero () =
        let exc = Record.Exception(
            fun () ->
                Natural.Unit / Natural.Zero
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.DivideByZeroException>( exc )

    [<Fact>]
    member public this.Big () =
        Assert.Equal(
            Natural( [0xFEDCBA98u] ),
            Natural( [0x75CD9046u; 0x541D5980u] ) / Natural( [0x76543210u] )
        )

type public Modulo() =
    [<Theory>]
    [<InlineData(  1u,  1u, 0u )>]  // Sanity
    [<InlineData(  0u,  1u, 0u )>]  // Sanity
    [<InlineData( 44u,  7u, 2u )>]  // multiple bits
    [<InlineData( 52u,  5u, 2u )>]  // rev
    [<InlineData( 52u, 10u, 2u )>]  // rev
    member public this.Sanity dividend divisor remainder =
        Assert.Equal(
            Natural( [remainder] ),
            Natural( [dividend] ) % Natural( [divisor] )
        )

    [<Fact>]
    member public this.Zero () =
        Assert.Equal(
            Natural( [0u] ),
            Natural( [20u] ) % Natural( [10u] )
        )

    [<Fact>]
    member public this.DivideByZero () =
        let exc = Record.Exception(
            fun () ->
                Natural.Unit % Natural.Zero
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.DivideByZeroException>( exc )

    [<Fact>]
    member public this.Big () =
        Assert.Equal(
            Natural( [0x12345678u] ),
            Natural( [0x75CD9046u; 0x6651AFF8u] ) % Natural( [0x76543210u] ) )

type public DivisionModulo() =
    [<Theory>]
    [<InlineData(  1u,  1u,  1u, 0u )>] // Sanity
    [<InlineData(  0u,  1u,  0u, 0u )>] // Sanity
    [<InlineData( 44u,  7u,  6u, 2u )>] // multiple bits
    [<InlineData( 52u,  5u, 10u, 2u )>] // rev
    [<InlineData( 52u, 10u,  5u, 2u )>] // rev
    member public this.Sanity dividend divisor quotient remainder =
        Assert.Equal(
            ( Natural( [quotient] ), Natural( [remainder] ) ),
            Natural( [dividend] ) /% Natural( [divisor] )
        )

    [<Fact>]
    member public this.DivideByZero () =
        let exc = Record.Exception(
            fun () ->
                Natural.Unit /% Natural.Zero
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.DivideByZeroException>( exc )

    [<Fact>]
    member public this.Big () =
        Assert.Equal(
            ( Natural( [0xFEDCBA98u] ), Natural( [0x12345678u] ) ),
            Natural( [0x75CD9046u; 0x6651AFF8u] ) /% Natural( [0x76543210u] )
        )
