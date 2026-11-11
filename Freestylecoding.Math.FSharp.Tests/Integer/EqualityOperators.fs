namespace Integer

open Xunit
open Freestylecoding.Math

type public EqualityOperators() =
    static member Equality( this:'T when 'T :> System.Numerics.IEqualityOperators<'T,'T,bool>, that:'T ) : bool =
        'T.op_Equality( this, that )

    static member Inequality( this:'T when 'T :> System.Numerics.IEqualityOperators<'T,'T,bool>, that:'T ) : bool =
        'T.op_Inequality( this, that )

    [<Theory>]
    [<InlineData(  0,  0, true)>]
    [<InlineData(  1,  0, false)>]
    [<InlineData(  0,  1, false)>]
    [<InlineData(  1,  1, true)>]
    [<InlineData( -1,  0, false)>]
    [<InlineData(  0, -1, false)>]
    [<InlineData(  1, -1, false)>]
    [<InlineData( -1,  1, false)>]
    [<InlineData( -1, -1, true)>]
    member public this.EqualitySanity (left:int) (right:int) (expected:bool) =
        Assert.Equal(
            expected,
            EqualityOperators.Equality(
                Integer( left ),
                Integer( right )
            )
        )

    [<Fact>]
    member public this.EqualityLargeIntegersTrue () =
        Assert.True(
            EqualityOperators.Equality(
                Integer( [0xFu; 0x00000101u] ),
                Integer( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.EqualityLargeIntegersFalse () =
        Assert.False(
            EqualityOperators.Equality(
                Integer( [0x8u; 0x00000101u] ),
                Integer( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.EqualityLargeNegativeIntegersTrue () =
        Assert.True(
            EqualityOperators.Equality(
                Integer( [0xFu; 0x00000101u], true ),
                Integer( [0xFu; 0x00000101u], true )
            )
        )

    [<Fact>]
    member public this.EqualityLargeNegativeIntegersFalse () =
        Assert.False(
            EqualityOperators.Equality(
                Integer( [0x8u; 0x00000101u], true ),
                Integer( [0xFu; 0x00000101u], true )
            )
        )

    [<Fact>]
    member public this.EqualityLargeMixedIntegersTrue () =
        Assert.False(
            EqualityOperators.Equality(
                Integer( [0xFu; 0x00000101u], false ),
                Integer( [0xFu; 0x00000101u], true )
            )
        )

        Assert.False(
            EqualityOperators.Equality(
                Integer( [0xFu; 0x00000101u], true ),
                Integer( [0xFu; 0x00000101u], false )
            )
        )

    [<Theory>]
    [<InlineData(  0,  0, false)>]
    [<InlineData(  1,  0, true)>]
    [<InlineData(  0,  1, true)>]
    [<InlineData(  1,  1, false)>]
    [<InlineData( -1,  0, true)>]
    [<InlineData(  0, -1, true)>]
    [<InlineData(  1, -1, true)>]
    [<InlineData( -1,  1, true)>]
    [<InlineData( -1, -1, false)>]
    member public this.InequalitySanity (left:int) (right:int) (expected:bool) =
        Assert.Equal(
            expected,
            EqualityOperators.Inequality(
                Integer( left ),
                Integer( right )
            )
        )

    [<Fact>]
    member public this.InequalityLargeIntegersFalse () =
        Assert.False(
            EqualityOperators.Inequality(
                Integer( [0xFu; 0x00000101u] ),
                Integer( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.InequalityLargeIntegersTrue () =
        Assert.True(
            EqualityOperators.Inequality(
                Integer( [0x8u; 0x00000101u] ),
                Integer( [0xFu; 0x00000101u] )
            )
        )

    [<Fact>]
    member public this.InequalityLargeNegativeIntegersFalse () =
        Assert.False(
            EqualityOperators.Inequality(
                Integer( [0xFu; 0x00000101u], true ),
                Integer( [0xFu; 0x00000101u], true )
            )
        )

    [<Fact>]
    member public this.InequalityLargeNegativeIntegersTrue () =
        Assert.True(
            EqualityOperators.Inequality(
                Integer( [0x8u; 0x00000101u], true ),
                Integer( [0xFu; 0x00000101u], true )
            )
        )

    [<Fact>]
    member public this.InequalityLargeMixedIntegersTrue () =
        Assert.True(
            EqualityOperators.Inequality(
                Integer( [0xFu; 0x00000101u], false ),
                Integer( [0xFu; 0x00000101u], true )
            )
        )

        Assert.True(
            EqualityOperators.Inequality(
                Integer( [0xFu; 0x00000101u], true ),
                Integer( [0xFu; 0x00000101u], false )
            )
        )
