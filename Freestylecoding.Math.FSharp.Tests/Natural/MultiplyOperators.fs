namespace Natural

open Xunit
open Freestylecoding.Math

type public MultiplyOperators() =
    static member Multiply( this:'T when 'T :> System.Numerics.IMultiplyOperators<'T,'T,'T>, that:'T ) : 'T =
        'T.op_Multiply( this, that )

    [<Theory>]
    [<InlineData( 1u, 1u,  1u )>]        // Sanity
    [<InlineData( 1u, 0u,  0u )>]        // Sanity
    [<InlineData( 0u, 1u,  0u )>]        // Sanity
    [<InlineData( 6u, 7u, 42u )>]        // multiple bits
    member public this.Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            MultiplyOperators.Multiply(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.Big () =
        Assert.Equal(
            Natural( [0x75CD9046u; 0x541D5980u] ),
            MultiplyOperators.Multiply(
                Natural( [0xFEDCBA98u] ),
                Natural( [0x76543210u] )
            )
        )

// NOTE: The "Checked" tests are commentted out because they are not calling the correct operator
//  I cannot figure out who to force F# to call the correct one
//  We need to really make sure the C# side of the tests do call the right one

//type public CheckedMultiplyOperators() =
//    static member CheckedMultiply( this:'T when 'T :> System.Numerics.IMultiplyOperators<'T,'T,'T>, that:'T ) : 'T =
//        'T.op_CheckedMultiply( this, that )
//
//    [<Theory>]
//    [<InlineData( 1u, 1u,  1u )>]        // Sanity
//    [<InlineData( 1u, 0u,  0u )>]        // Sanity
//    [<InlineData( 0u, 1u,  0u )>]        // Sanity
//    [<InlineData( 6u, 7u, 42u )>]        // multiple bits
//    member public this.Sanity left right expected =
//        Assert.Equal(
//            Natural( [expected] ),
//            CheckedMultiplyOperators.CheckedMultiply(
//                Natural( [left] ),
//                Natural( [right] )
//            )
//        )
//
//    [<Fact>]
//    member public this.Big () =
//        Assert.Equal(
//            Natural( [0x75CD9046u; 0x541D5980u] ),
//            CheckedMultiplyOperators.CheckedMultiply(
//                Natural( [0xFEDCBA98u] ),
//                Natural( [0x76543210u] )
//            )
//        )
