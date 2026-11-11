namespace Integer

open Xunit
open Freestylecoding.Math

type public EquatableInteger() =
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
    member public this.Sanity (left:int) (right:int) (expected:bool) =
        Assert.Equal(
            expected,
            (Integer( left ) :> System.IEquatable<Integer>).Equals( Integer( right ) )
        )

    [<Fact>]
    member public this.LargeIntegersTrue () =
        Assert.True(
            (Integer( [0xFu; 0x00000101u] ) :> System.IEquatable<Integer>).Equals( Integer( [0xFu; 0x00000101u] ) )
        )

    [<Fact>]
    member public this.LargeIntegersFalse () =
        Assert.False(
            (Integer( [0x8u; 0x00000101u] ) :> System.IEquatable<Integer>).Equals( Integer( [0xFu; 0x00000101u] ) )
        )

    [<Fact>]
    member public this.LargeNegativeIntegersTrue () =
        Assert.True(
            (Integer( [0xFu; 0x00000101u], true ) :> System.IEquatable<Integer>).Equals( Integer( [0xFu; 0x00000101u], true ) )
        )

    [<Fact>]
    member public this.LargeNegativeIntegersFalse () =
        Assert.False(
            (Integer( [0x8u; 0x00000101u], true ) :> System.IEquatable<Integer>).Equals( Integer( [0xFu; 0x00000101u], true ) )
        )
    
    [<Fact>]
    member public this.LargeMixedIntegersTrue () =
        Assert.False(
            (Integer( [0xFu; 0x00000101u], false ) :> System.IEquatable<Integer>).Equals( Integer( [0xFu; 0x00000101u], true ) )
        )

        Assert.False(
            (Integer( [0xFu; 0x00000101u], true ) :> System.IEquatable<Integer>).Equals( Integer( [0xFu; 0x00000101u], false ) )
        )
