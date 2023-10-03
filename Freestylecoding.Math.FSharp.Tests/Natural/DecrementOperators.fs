namespace Natural

open Xunit
open Freestylecoding.Math

// NOTE: F# doesn't really have increment/decrement operators
//  It would mean the value is mutable
//  I'll get these tested in the C# tests
//	Maybe, after i confirm everything is working, I'll loop back and play with mutable to try it

//type public DecrementOperators() =
//    static member Decrement( n:'T when 'T :> System.Numerics.IDecrementOperators<'T> ) : 'T =
//        'T.op_Decrement( n )
//
//    [<Theory>]
//    [<InlineData(  1u, 0u )>]   // Sanity
//    [<InlineData(  2u, 1u )>]   // Sanity
//    [<InlineData( 10u, 9u )>]   // Sanity
//    member public this.Sanity value expected =
//        Assert.Equal(
//            Natural( [expected] ),
//            DecrementOperators.Decrement( Natural( [value] ) )
//        )
//    
//    [<Fact>]
//    member public this.Overflow () =
//        Assert.Equal(
//            Natural( [0xFFFF_FFFFu] ),
//            DecrementOperators.Decrement(
//                Natural( [1u; 0u] )
//            )
//        )
//        
//    [<Fact>]
//    member public this.CascadingOverflow () =
//        Assert.Equal(
//            Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] ),
//            DecrementOperators.Decrement(
//                Natural( [1u; 0u; 0u] )
//            )
//        )
//
//	[<Fact>]
//	member public this.SingleItemBadUnderflow () =
//	    let exc = Record.Exception(
//	        fun () ->
//	            DecrementOperators.Decrement(
//	                Natural.Zero
//	            )
//	            |> ignore
//	    )
//	    Assert.NotNull( exc )
//	    Assert.IsType<System.OverflowException>( exc )

// NOTE: The "Checked" tests are commentted out because they are not calling the correct operator
//  I cannot figure out who to force F# to call the correct one
//  We need to really make sure the C# side of the tests do call the right one

//type public CheckedDecrementOperators() =
//    static member CheckedDecrement( this:'T when 'T :> System.Numerics.IDecrementOperators<'T> ) : 'T =
//        'T.op_CheckedDecrement( this )
//
//    [<Theory>]
//    [<InlineData(  1u, 0u )>]   // Sanity
//    [<InlineData(  2u, 1u )>]   // Sanity
//    [<InlineData( 10u, 9u )>]   // Sanity
//    member public this.Sanity value expected =
//        Assert.Equal(
//            Natural( [expected] ),
//            CheckedDecrementOperators.CheckedDecrement( Natural( [value] ) )
//        )
//    
//    [<Fact>]
//    member public this.Overflow () =
//        Assert.Equal(
//            Natural( [0xFFFF_FFFFu] ),
//            CheckedDecrementOperators.CheckedDecrement(
//                Natural( [1u; 0u] )
//            )
//        )
//        
//    [<Fact>]
//    member public this.CascadingOverflow () =
//        Assert.Equal(
//            Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] ),
//            CheckedDecrementOperators.CheckedDecrement(
//                Natural( [1u; 0u; 0u] )
//            )
//        )
//
//	[<Fact>]
//	member public this.SingleItemBadUnderflow () =
//	    let exc = Record.Exception(
//	        fun () ->
//	            DecrementOperators.CheckedDecrement(
//	                Natural.Zero
//	            )
//	            |> ignore
//	    )
//	    Assert.NotNull( exc )
//	    Assert.IsType<System.OverflowException>( exc )
