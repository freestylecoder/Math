namespace Natural

open Xunit
open Freestylecoding.Math

// NOTE: F# doesn't really have increment/decrement operators
//  It would mean the value is mutable
//  I'll get these tested in the C# tests
//	Maybe, after i confirm everything is working, I'll loop back and play with mutable to try it

//type public IncrementOperators() =
//    static member Increment( n:'T when 'T :> System.Numerics.IIncrementOperators<'T> ) : 'T =
//        'T.op_Increment( n )
//
//    [<Theory>]
//    [<InlineData( 0u,  1u )>]   // Sanity
//    [<InlineData( 1u,  2u )>]   // Sanity
//    [<InlineData( 9u, 10u )>]   // Sanity
//    member public this.Sanity value expected =
//        Assert.Equal(
//            Natural( [expected] ),
//            IncrementOperators.Increment( Natural( [value] ) )
//        )
//    
//    [<Fact>]
//    member public this.Overflow () =
//        Assert.Equal(
//            Natural( [1u; 0u] ),
//            IncrementOperators.Increment(
//                Natural( [0xFFFF_FFFFu] )
//            )
//        )
//        
//    [<Fact>]
//    member public this.CascadingOverflow () =
//        Assert.Equal(
//            Natural( [1u; 0u; 0u] ),
//            IncrementOperators.Increment(
//                Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
//            )
//        )

// NOTE: The "Checked" tests are commentted out because they are not calling the correct operator
//  I cannot figure out who to force F# to call the correct one
//  We need to really make sure the C# side of the tests do call the right one

//type public CheckedIncrementOperators() =
//    static member CheckedIncrement( this:'T when 'T :> System.Numerics.IIncrementOperators<'T> ) : 'T =
//        'T.op_CheckedIncrement( this )
//
//    [<Theory>]
//    [<InlineData( 0u,  1u )>]   // Sanity
//    [<InlineData( 1u,  2u )>]   // Sanity
//    [<InlineData( 9u, 10u )>]   // Sanity
//    member public this.Sanity value expected =
//        Assert.Equal(
//            Natural( [expected] ),
//            CheckedIncrementOperators.CheckedIncrement( Natural( [value] ) )
//        )
//    
//    [<Fact>]
//    member public this.Overflow () =
//        Assert.Equal(
//            Natural( [1u; 0u] ),
//            CheckedIncrementOperators.CheckedIncrement(
//                Natural( [0xFFFF_FFFFu] )
//            )
//        )
//        
//    [<Fact>]
//    member public this.CascadingOverflow () =
//        Assert.Equal(
//            Natural( [1u; 0u; 0u] ),
//            CheckedIncrementOperators.CheckedIncrement(
//                Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
//            )
//        )
