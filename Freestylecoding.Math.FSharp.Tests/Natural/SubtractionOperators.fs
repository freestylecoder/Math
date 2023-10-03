namespace Natural

open Xunit
open Freestylecoding.Math

type public SubtractionOperators() =
    static member Sub( this:'T when 'T :> System.Numerics.ISubtractionOperators<'T,'T,'T>, that:'T ) : 'T =
        'T.op_Subtraction( this, that )

    [<Theory>]
    [<InlineData( 1u, 1u, 0u )>]    // Sanity
    [<InlineData( 1u, 0u, 1u )>]    // Sanity
    [<InlineData( 9u, 2u, 7u )>]    // l > r
    member public this.Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            SubtractionOperators.Sub(
                Natural( [left] ),
                Natural( [right] )
            )
        )
    
    [<Fact>]
    member public this.SingleItemBadUnderflow () =
        let exc = Record.Exception(
            fun () ->
                SubtractionOperators.Sub(
                    Natural.Zero,
                    Natural.Unit
                )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

    [<Fact>]
    member public this.MultiItemNoUnderflow () =
        Assert.Equal(
            Natural( [2u; 2u] ),
            SubtractionOperators.Sub(
                Natural( [3u; 4u] ),
                Natural( [1u; 2u] )
            )
        )
        
    [<Fact>]
    member public this.MultiItemSafeUnderflow () =
        Assert.Equal(
            Natural( [0x2u; 0xFFFFFFFFu] ),
            SubtractionOperators.Sub(
                Natural( [4u; 2u] ),
                Natural( [1u; 3u] )
            )
        )
        
    [<Fact>]
    member public this.MultiItemSafeCascadingUnderflow () =
        Assert.Equal(
            Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] ),
            SubtractionOperators.Sub(
                Natural( [1u; 0u; 0u] ),
                Natural.Unit
            )
        )
        
    [<Fact>]
    member public this.MultiItemUnsafeUnderflow () =
        let exc = Record.Exception(
            fun () ->
                SubtractionOperators.Sub(
                    Natural( [1u; 2u] ),
                    Natural( [1u; 3u] )
                )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

    [<Fact>]
    member public this.LargeWithUnderflows () =
        Assert.Equal(
            Natural( [0x1u; 0xFFFF_FFFFu; 0xFFFF_FFFEu] ),
            SubtractionOperators.Sub(
                Natural( [3u; 2u; 1u] ),
                Natural( [1u; 2u; 3u] )
            )
        )

// NOTE: The "Checked" tests are commentted out because they are not calling the correct operator
//  I cannot figure out who to force F# to call the correct one
//  We need to really make sure the C# side of the tests do call the right one

//type public CheckedSubtractionOperators() =
//    static member CheckedSub( this:'T when 'T :> System.Numerics.ISubtractionOperators<'T,'T,'T>, that:'T ) : 'T =
//        'T.op_CheckedSubtraction( this, that )
//
//    [<Theory>]
//    [<InlineData( 1u, 1u, 0u )>]    // Sanity
//    [<InlineData( 1u, 0u, 1u )>]    // Sanity
//    [<InlineData( 9u, 2u, 7u )>]    // l > r
//    member public this.Sanity left right expected =
//        Assert.Equal(
//            Natural( [expected] ),
//            CheckedSubtractionOperators.CheckedSub(
//                Natural( [left] ),
//                Natural( [right] )
//            )
//        )
//    
//    [<Fact>]
//    member public this.SingleItemBadUnderflow () =
//        let exc = Record.Exception(
//            fun () ->
//                CheckedSubtractionOperators.CheckedSub(
//                    Natural.Zero,
//                    Natural.Unit
//                )
//                |> ignore
//        )
//        Assert.NotNull( exc )
//        Assert.IsType<System.OverflowException>( exc )
//
//    [<Fact>]
//    member public this.MultiItemNoUnderflow () =
//        Assert.Equal(
//            Natural( [2u; 2u] ),
//            CheckedSubtractionOperators.CheckedSub(
//                Natural( [3u; 4u] ),
//                Natural( [1u; 2u] )
//            )
//        )
//        
//    [<Fact>]
//    member public this.MultiItemSafeUnderflow () =
//        Assert.Equal(
//            Natural( [0x2u; 0xFFFFFFFFu] ),
//            CheckedSubtractionOperators.CheckedSub(
//                Natural( [4u; 2u] ),
//                Natural( [1u; 3u] )
//            )
//        )
//        
//    [<Fact>]
//    member public this.MultiItemSafeCascadingUnderflow () =
//        Assert.Equal(
//            Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] ),
//            CheckedSubtractionOperators.CheckedSub(
//                Natural( [1u; 0u; 0u] ),
//                Natural.Unit
//            )
//        )
//        
//    [<Fact>]
//    member public this.MultiItemUnsafeUnderflow () =
//        let exc = Record.Exception(
//            fun () ->
//                CheckedSubtractionOperators.CheckedSub(
//                    Natural( [1u; 2u] ),
//                    Natural( [1u; 3u] )
//                )
//                |> ignore
//        )
//        Assert.NotNull( exc )
//        Assert.IsType<System.OverflowException>( exc )
//
//    [<Fact>]
//    member public this.LargeWithUnderflows () =
//        Assert.Equal(
//            Natural( [0x1u; 0xFFFF_FFFFu; 0xFFFF_FFFEu] ),
//            CheckedSubtractionOperators.CheckedSub(
//                Natural( [3u; 2u; 1u] ),
//                Natural( [1u; 2u; 3u] )
//            )
//        )
