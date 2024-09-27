namespace Natural

open Xunit
open Freestylecoding.Math

type public BitwiseOperators() =
    static member AND( left:'T when 'T :> System.Numerics.IBitwiseOperators<'T,'T,'T>, right:'T ) : 'T =
        'T.op_BitwiseAnd( left, right )
    static member OR ( left:'T when 'T :> System.Numerics.IBitwiseOperators<'T,'T,'T>, right:'T ) : 'T =
        'T.op_BitwiseOr( left, right )
    static member XOR( left:'T when 'T :> System.Numerics.IBitwiseOperators<'T,'T,'T>, right:'T ) : 'T =
        'T.op_ExclusiveOr( left, right )
    static member NOT( value:'T when 'T :> System.Numerics.IBitwiseOperators<'T,'T,'T> ) : 'T =
        'T.op_OnesComplement( value )

    [<Theory>]
    [<InlineData(  0u,  0u, 0u)>]
    [<InlineData(  1u,  0u, 0u)>]
    [<InlineData(  0u,  1u, 0u)>]
    [<InlineData(  1u,  1u, 1u)>]
    [<InlineData( 12u, 10u, 8u)>]
    member public this.AND_Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            BitwiseOperators.AND(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.AND_BiggerLeft () =
        Assert.Equal(
            Natural.Unit,
            BitwiseOperators.AND(
                Natural( [0xFu; 0x00000101u] ),
                Natural( [ 0x00010001u] )
            )
        )

    [<Fact>]
    member public this.AND_BiggerRight () =
        Assert.Equal(
            Natural.Unit,
            BitwiseOperators.AND(
                Natural( [0x00010001u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<FactAttribute>]
    member public this.AND_Large () =
        Assert.Equal(
            Natural( [1u; 0u; 0u; 0u] ),
            BitwiseOperators.AND(
                Natural( [1u; 1u; 0u; 0u] ),
                Natural( [1u; 0u; 1u; 0u] )
            )
        )

    [<Theory>]
    [<InlineData(  0u,  0u,  0u)>]
    [<InlineData(  1u,  0u,  1u)>]
    [<InlineData(  0u,  1u,  1u)>]
    [<InlineData(  1u,  1u,  1u)>]
    [<InlineData( 12u, 10u, 14u)>]
    member public this.OR_Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            BitwiseOperators.OR(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.OR_BiggerLeft () =
        Assert.Equal(
            Natural( [0xFu; 0x10101u] ),
            BitwiseOperators.OR(
                Natural( [0xFu; 0x00000101u] ),
                Natural( [ 0x00010001u] )
            )
        )

    [<Fact>]
    member public this.OR_BiggerRight () =
        Assert.Equal(
            Natural( [0xFu; 0x10101u] ),
            BitwiseOperators.OR(
                Natural( [0x00010001u] ),
                Natural( [0xFu; 0x00000101u] ) 
            ) 
        )

    [<FactAttribute>]
    member public this.OR_Large () =
        Assert.Equal(
            Natural( [1u; 1u; 1u; 0u] ),
            BitwiseOperators.OR(
                Natural( [1u; 1u; 0u; 0u] ),
                Natural( [1u; 0u; 1u; 0u] )
            )
        )

    [<Theory>]
    [<InlineData(  0u,  0u, 0u)>]
    [<InlineData(  1u,  0u, 1u)>]
    [<InlineData(  0u,  1u, 1u)>]
    [<InlineData(  1u,  1u, 0u)>]
    [<InlineData( 12u, 10u, 6u)>]
    member public this.XOR_Sanity left right expected =
        Assert.Equal(
            Natural( [expected] ),
            BitwiseOperators.XOR(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.XOR_BiggerLeft () =
        Assert.Equal(
            Natural( [0xFu; 0x10100u] ),
            BitwiseOperators.XOR(
                Natural( [0xFu; 0x00000101u] ),
                Natural( [0x00010001u] )
            )
        )

    [<Fact>]
    member public this.XOR_BiggerRight () =
        Assert.Equal(
            Natural( [0xFu; 0x10100u] ),
            BitwiseOperators.XOR(
                Natural( [0x00010001u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<FactAttribute>]
    member public this.XOR_Large () =
        Assert.Equal(
            Natural( [0u; 1u; 1u; 0u] ),
            BitwiseOperators.XOR(
                Natural( [1u; 1u; 0u; 0u] ),
                Natural( [1u; 0u; 1u; 0u] )
            )
        )

    [<Theory>]
    [<InlineData( 0xFFFF_FFFEu, 1u)>]
    [<InlineData( 1u, 0xFFFF_FFFEu)>]
    member public this.NOT_Sanity value expected =
        Assert.Equal(
            Natural( [expected] ),
            BitwiseOperators.NOT(
                Natural( [value] )
            )
        )

    [<Fact>]
    member public this.NOT_Bigger () =
        Assert.Equal(
            Natural( [0xF012_3456u; 0x789A_BCDEu] ),
            BitwiseOperators.NOT(
                Natural( [0x0FED_CBA9u; 0x8765_4321u] )
            )
        )
