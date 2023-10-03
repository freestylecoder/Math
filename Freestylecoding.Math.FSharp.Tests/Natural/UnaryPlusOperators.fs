namespace Natural

open Xunit
open Freestylecoding.Math

type public UnaryPlusOperators() =
    static member Plus( n:'T when 'T :> System.Numerics.IUnaryPlusOperators<'T,'T> ) : 'T =
        'T.op_UnaryPlus( n )

    [<Theory>]
    [<InlineData(  0u )>]   // Sanity
    [<InlineData(  1u )>]   // Sanity
    [<InlineData( 42u )>]   // Sanity
    member public this.Sanity value =
        Assert.Equal(
            Natural( [value] ),
            UnaryPlusOperators.Plus( Natural( [value] ) )
        )
