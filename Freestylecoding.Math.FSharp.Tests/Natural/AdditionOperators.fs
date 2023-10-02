namespace Natural

open Xunit
open Freestylecoding.Math

type public AdditionOperators() =
    static member Add( this:'T when 'T :> System.Numerics.IAdditionOperators<'T,'T,'T>, that:'T ) : 'T =
        'T.op_Addition( this, that )

    [<Theory>]
    [<InlineData( 0u, 0u,  0u )>]        // Sanity
    [<InlineData( 1u, 1u,  2u )>]        // Sanity
    [<InlineData( 1u, 0u,  1u )>]        // Sanity
    [<InlineData( 0u, 1u,  1u )>]        // Sanity
    [<InlineData( 1u, 5u,  6u )>]        // l < r
    [<InlineData( 9u, 2u, 11u )>]        // l > r
    member public this.Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            AdditionOperators.Add( Natural( [left] ), Natural( [right] ) )
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
            AdditionOperators.Add(
                Natural( [0xFFFF_FFFFu] ),
                Natural( [3u] )
            )
        )
        
    [<Fact>]
    member public this.LeftBiggerNoOverflow () =
        Assert.Equal(
            Natural( [0xFu; 0xFF0Fu] ),
            AdditionOperators.Add(
                Natural( [0xFu; 0xFu] ),
                Natural( [0xFF00u] )
            )
        )
        
    [<Fact>]
    member public this.RightBiggerNoOverflow () =
        Assert.Equal(
            Natural( [0xFu; 0xFF0Fu] ),
            AdditionOperators.Add(
                Natural( [0xFF00u] ),
                Natural( [0xFu; 0xFu] )
            )
        )
        
    [<Fact>]
    member public this.CascadingOverflow () =
        Assert.Equal(
            Natural( [1u; 0u; 0u] ),
            AdditionOperators.Add(
                Natural( [1u] ),
                Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
            )
        )

    [<Fact>]
    member public this.OverflowCausedOverflow () =
        Assert.Equal(
            Natural( [2u; 0u; 0u] ),
            AdditionOperators.Add(
                Natural( [1u; 1u] ),
                Natural( [1u; 0xFFFF_FFFFu - 1u; 0xFFFF_FFFFu] )
            )
        )

    [<Fact>]
    member public this.EdgeOfOverflow () =
        Assert.Equal(
            Natural( [1u; 0xFFFF_FFFFu; 1u] ),
            AdditionOperators.Add(
                Natural( [1u; 1u] ),
                Natural( [1u; 0xFFFF_FFFFu - 1u; 0u] )
            )
        )

    // NOTE: The "Checked" tests are commentted out because they are not calling the correct operator
    //  I cannot figure out who to force F# to call the correct one
    //  We need to really make sure the C# side of the tests do call the right one

    //static member CheckedAdd( this:'T when 'T :> System.Numerics.IAdditionOperators<'T,'T,'T>, that:'T ) : 'T =
    //    'T.op_CheckedAddition( this, that )
    //
    //[<Theory>]
    //[<InlineData( 0u, 0u,  0u )>]        // Sanity
    //[<InlineData( 1u, 1u,  2u )>]        // Sanity
    //[<InlineData( 1u, 0u,  1u )>]        // Sanity
    //[<InlineData( 0u, 1u,  1u )>]        // Sanity
    //[<InlineData( 1u, 5u,  6u )>]        // l < r
    //[<InlineData( 9u, 2u, 11u )>]       // l > r
    //member public this.CheckedSanity left right expected =
    //    Assert.Equal(
    //        Natural( [expected] ),
    //        AdditionOperators.CheckedAdd( Natural( [left] ), Natural( [right] ) )
    //    )
    //    
    //[<Fact>]
    //member public this.CheckedSimple () =
    //    Assert.Equal(
    //        Natural.Unit,
    //        Natural.Zero + Natural.Unit
    //    )
    //
    //[<Fact>]
    //member public this.CheckedOverflow () =
    //    Assert.Equal(
    //        Natural( [1u; 2u] ),
    //        AdditionOperators.CheckedAdd(
    //            Natural( [0xFFFF_FFFFu] ),
    //            Natural( [3u] )
    //        )
    //    )
    //        
    //[<Fact>]
    //member public this.CheckedLeftBiggerNoOverflow () =
    //    Assert.Equal(
    //        Natural( [0xFu; 0xFF0Fu] ),
    //        AdditionOperators.CheckedAdd(
    //            Natural( [0xFu; 0xFu] ),
    //            Natural( [0xFF00u] )
    //        )
    //    )
    //        
    //[<Fact>]
    //member public this.CheckedRightBiggerNoOverflow () =
    //    Assert.Equal(
    //        Natural( [0xFu; 0xFF0Fu] ),
    //        AdditionOperators.CheckedAdd(
    //            Natural( [0xFF00u] ),
    //            Natural( [0xFu; 0xFu] )
    //        )
    //    )
    //       
    //[<Fact>]
    //member public this.CheckedCascadingOverflow () =
    //    Assert.Equal(
    //        Natural( [1u; 0u; 0u] ),
    //        AdditionOperators.CheckedAdd(
    //            Natural( [1u] ),
    //            Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
    //        )
    //    )
    //
    //[<Fact>]
    //member public this.CheckedOverflowCausedOverflow () =
    //    Assert.Equal(
    //        Natural( [2u; 0u; 0u] ),
    //        AdditionOperators.CheckedAdd(
    //            Natural( [1u; 1u] ),
    //            Natural( [1u; 0xFFFF_FFFFu - 1u; 0xFFFF_FFFFu] )
    //        )
    //    )
    //
    //[<Fact>]
    //member public this.CheckedEdgeOfOverflow () =
    //    Assert.Equal(
    //        Natural( [1u; 0xFFFF_FFFFu; 1u] ),
    //        AdditionOperators.CheckedAdd(
    //            Natural( [1u; 1u] ),
    //            Natural( [1u; 0xFFFF_FFFFu - 1u; 0u] )
    //        )
    //    )
