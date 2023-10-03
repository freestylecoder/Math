namespace Natural

open Xunit
open Freestylecoding.Math

type public UnaryNegationOperators() =
    static member Negation( n:'T when 'T :> System.Numerics.IUnaryNegationOperators<'T,'T> ) : 'T =
        'T.op_UnaryNegation( n )

    [<Theory>]
    [<InlineData(  0u )>]   // Sanity
    [<InlineData(  1u )>]   // Sanity
    [<InlineData( 42u )>]   // Sanity
    member public this.Sanity value =
        let exc = Record.Exception(
            fun () ->
                UnaryNegationOperators.Negation( Natural( [value] ) )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

// NOTE: The "Checked" tests are commentted out because they are not calling the correct operator
//  I cannot figure out who to force F# to call the correct one
//  We need to really make sure the C# side of the tests do call the right one

//type public CheckedUnaryNegationOperators() =
//    static member Negation( n:'T when 'T :> System.Numerics.IUnaryNegationOperators<'T,'T> ) : 'T =
//        'T.op_CheckedUnaryNegation( n )
//
//    [<Theory>]
//    [<InlineData(  0u )>]   // Sanity
//    [<InlineData(  1u )>]   // Sanity
//    [<InlineData( 42u )>]   // Sanity
//    member public this.Sanity value =
//        let exc = Record.Exception(
//            fun () ->
//                CheckedUnaryNegationOperators.Negation( Natural( [value] ) )
//                |> ignore
//        )
//        Assert.NotNull( exc )
//        Assert.IsType<System.OverflowException>( exc )

