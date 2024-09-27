namespace Natural

open Xunit
open Freestylecoding.Math

type public UnaryNegationOperators() =
    static member Negation( n:'T when 'T :> System.Numerics.IUnaryNegationOperators<'T,'T> ) : 'T =
        'T.op_UnaryNegation( n )

    static member CheckedNegation( n:'T when 'T :> System.Numerics.IUnaryNegationOperators<'T,'T> ) : 'T =
        'T.op_CheckedUnaryNegation( n )

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

    [<Theory>]
    [<InlineData(  0u )>]   // Sanity
    [<InlineData(  1u )>]   // Sanity
    [<InlineData( 42u )>]   // Sanity
    member public this.CheckedSanity value =
        let exc = Record.Exception(
            fun () ->
                UnaryNegationOperators.CheckedNegation( Natural( [value] ) )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

