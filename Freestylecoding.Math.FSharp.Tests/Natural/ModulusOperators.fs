namespace Natural

open Xunit
open Freestylecoding.Math

type public ModulusOperators() =
    static member Mod( left:'T when 'T :> System.Numerics.IModulusOperators<'T,'T,'T>, right:'T ) : 'T =
        'T.op_Modulus( left, right )

    [<Theory>]
    [<InlineData(  1u,  1u, 0u )>]  // Sanity
    [<InlineData(  0u,  1u, 0u )>]  // Sanity
    [<InlineData( 44u,  7u, 2u )>]  // multiple bits
    [<InlineData( 52u,  5u, 2u )>]  // rev
    [<InlineData( 52u, 10u, 2u )>]  // rev
    member public this.Sanity dividend divisor remainder =
        Assert.Equal(
            Natural( [remainder] ),
            ModulusOperators.Mod(
                Natural( [dividend] ),
                Natural( [divisor] )
            )
        )

    [<Fact>]
    member public this.Zero () =
        Assert.Equal(
            Natural( [0u] ),
            ModulusOperators.Mod(
                Natural( [20u] ),
                Natural( [10u] )
            )
        )

    [<Fact>]
    member public this.DivideByZero () =
        let exc = Record.Exception(
            fun () ->
                ModulusOperators.Mod( Natural.Unit, Natural.Zero )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.DivideByZeroException>( exc )

    [<Fact>]
    member public this.Big () =
        Assert.Equal(
            Natural( [0x12345678u] ),
            ModulusOperators.Mod(
                Natural( [0x75CD9046u; 0x6651AFF8u] ),
                Natural( [0x76543210u] )
            )
        )