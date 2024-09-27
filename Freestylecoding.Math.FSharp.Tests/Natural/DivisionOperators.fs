namespace Natural

open Xunit
open Freestylecoding.Math

type public DivisionOperators() =
    static member Divide( this:'T when 'T :> System.Numerics.IDivisionOperators<'T,'T,'T>, that:'T ) : 'T =
        'T.op_Division( this, that )
    static member CheckedDivide( this:'T when 'T :> System.Numerics.IDivisionOperators<'T,'T,'T>, that:'T ) : 'T =
        'T.op_CheckedDivision( this, that )

    [<Theory>]
    [<InlineData(  1u,  1u,  1u )>] // Sanity
    [<InlineData(  0u,  1u,  0u )>] // Sanity
    [<InlineData( 42u,  7u,  6u )>] // multiple bits
    [<InlineData( 50u,  5u, 10u )>] // rev
    [<InlineData( 50u, 10u,  5u )>] // rev
    [<InlineData( 54u,  5u, 10u )>] // has remainder
    member public this.Sanity dividend divisor quotient =
        Assert.Equal(
            Natural( [quotient] ),
            DivisionOperators.Divide(
                Natural( [dividend] ),
                Natural( [divisor] )
            )
        )

    [<Fact>]
    member public this.Zero () =
        Assert.Equal(
            Natural.Zero,
            DivisionOperators.Divide(
                Natural( [5u] ),
                Natural( [10u] )
            )
        )

    [<Fact>]
    member public this.DivideByZero () =
        let exc = Record.Exception(
            fun () ->
                DivisionOperators.Divide(
                    Natural.Unit,
                    Natural.Zero
                )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.DivideByZeroException>( exc )

    [<Fact>]
    member public this.Big () =
        Assert.Equal(
            Natural( [0xFEDCBA98u] ),
            DivisionOperators.Divide(
                Natural( [0x75CD9046u; 0x541D5980u] ),
                Natural( [0x76543210u] )
            )
        )

    [<Theory>]
    [<InlineData(  1u,  1u,  1u )>] // Sanity
    [<InlineData(  0u,  1u,  0u )>] // Sanity
    [<InlineData( 42u,  7u,  6u )>] // multiple bits
    [<InlineData( 50u,  5u, 10u )>] // rev
    [<InlineData( 50u, 10u,  5u )>] // rev
    [<InlineData( 54u,  5u, 10u )>] // has remainder
    member public this.CheckedSanity dividend divisor quotient =
        Assert.Equal(
            Natural( [quotient] ),
            DivisionOperators.CheckedDivide(
                Natural( [dividend] ),
                Natural( [divisor] )
            )
        )

    [<Fact>]
    member public this.CheckedZero () =
        Assert.Equal(
            Natural.Zero,
            DivisionOperators.CheckedDivide(
                Natural( [5u] ),
                Natural( [10u] )
            )
        )

    [<Fact>]
    member public this.CheckedDivideByZero () =
        let exc = Record.Exception(
            fun () ->
                DivisionOperators.CheckedDivide(
                    Natural.Unit,
                    Natural.Zero
                )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.DivideByZeroException>( exc )

    [<Fact>]
    member public this.CheckedBig () =
        Assert.Equal(
            Natural( [0xFEDCBA98u] ),
            DivisionOperators.CheckedDivide(
                Natural( [0x75CD9046u; 0x541D5980u] ),
                Natural( [0x76543210u] )
            )
        )
