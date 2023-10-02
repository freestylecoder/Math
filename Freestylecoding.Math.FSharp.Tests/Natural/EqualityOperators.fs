namespace Natural

open Xunit
open Freestylecoding.Math

type public EqualityOperators() =
    static member Equality( this:'T when 'T :> System.Numerics.IEqualityOperators<'T,'T,bool>, that:'T ) : bool =
        'T.op_Equality( this, that )

    static member Inequality( this:'T when 'T :> System.Numerics.IEqualityOperators<'T,'T,bool>, that:'T ) : bool =
        'T.op_Inequality( this, that )

    [<Theory>]
    [<InlineData(  0u,  0u, true)>]
    [<InlineData(  1u,  0u, false)>]
    [<InlineData(  0u,  1u, false)>]
    [<InlineData(  1u,  1u, true)>]
    member public this.EqualitySanity left right (expected:bool) =
        Assert.Equal(
            expected,
            EqualityOperators.Equality(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.EqualityLargeNaturalsTrue () =
        Assert.True(
            EqualityOperators.Equality(
                Natural( [0xFu; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.EqualityLargeNaturalsFalse () =
        Assert.False(
            EqualityOperators.Equality(
                Natural( [0x8u; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<Theory>]
    [<InlineData(  0u,  0u, false)>]
    [<InlineData(  1u,  0u, true)>]
    [<InlineData(  0u,  1u, true)>]
    [<InlineData(  1u,  1u, false)>]
    member public this.InequalitySanity left right (expected:bool) =
        Assert.Equal(
            expected,
            EqualityOperators.Inequality(
                Natural( [left] ),
                Natural( [right] )
            )
        )

    [<Fact>]
    member public this.InequalityLargeNaturalsFalse () =
        Assert.False(
            EqualityOperators.Inequality(
                Natural( [0xFu; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.InequalityLargeNaturalsTrue () =
        Assert.True(
            EqualityOperators.Inequality(
                Natural( [0x8u; 0x00000101u] ),
                Natural( [0xFu; 0x00000101u] )
            )
        )
