namespace Freestylecoding.Math.Interface.Tests

open Xunit

[<AbstractClass>]
type public TEquatableT<'T when 'T :> System.IEquatable<'T> and 'T : equality>() =
    abstract member item1 : 'T;
    abstract member item2 : 'T;
    abstract member item3 : 'T;

    abstract member different1 : 'T;
    abstract member different2 : 'T;

    [<Fact>]
    member public this.Sanity () =
        Assert.True( this.item1.Equals( this.item1 ) )
        Assert.True( this.item1.Equals( this.item2 ) )
        Assert.True( this.item1.Equals( this.item3 ) )
        Assert.False( this.item1.Equals( this.different1 ) )
        Assert.False( this.item1.Equals( this.different2 ) )

        Assert.True( this.item2.Equals( this.item1 ) )
        Assert.True( this.item2.Equals( this.item2 ) )
        Assert.True( this.item2.Equals( this.item3 ) )
        Assert.False( this.item2.Equals( this.different1 ) )
        Assert.False( this.item2.Equals( this.different2 ) )

        Assert.True( this.item3.Equals( this.item1 ) )
        Assert.True( this.item3.Equals( this.item2 ) )
        Assert.True( this.item3.Equals( this.item3 ) )
        Assert.False( this.item3.Equals( this.different1 ) )
        Assert.False( this.item3.Equals( this.different2 ) )

        Assert.True( this.different1.Equals( this.different1 ) )
        Assert.False( this.different1.Equals( this.different2 ) )
        Assert.False( this.different1.Equals( this.item1 ) )
        Assert.False( this.different1.Equals( this.item2 ) )
        Assert.False( this.different1.Equals( this.item3 ) )

        Assert.True( this.different2.Equals( this.different2 ) )
        Assert.False( this.different2.Equals( this.different1 ) )
        Assert.False( this.different2.Equals( this.item1 ) )
        Assert.False( this.different2.Equals( this.item2 ) )
        Assert.False( this.different2.Equals( this.item3 ) )

    [<Fact>]
    member public this.ShouldMatchObjectEquals () =
        let allItems = [| this.item1; this.item2; this.item3; this.different1; this.different2 |]

        for left in allItems do
            for right in allItems do
                Assert.Equal(
                    left.Equals( right ),
                    (left :> obj).Equals( right )
                )

    [<Fact>]
    member public this.ShouldMatchObjectGetHashCode () =
        let allItems = [| this.item1; this.item2; this.item3; this.different1; this.different2 |]

        for left in allItems do
            for right in allItems do
                Assert.Equal(
                    left.Equals( right ),
                    (left :> obj).GetHashCode() = (right :> obj).GetHashCode()
                )

    [<Fact>]
    member public this.ShouldMatchOpEquality () =
        let allItems = [| this.item1; this.item2; this.item3; this.different1; this.different2 |]

        for left in allItems do
            for right in allItems do
                Assert.Equal(
                    left.Equals( right ),
                    (left = right)
                )

    [<Fact>]
    member public this.ShouldMatchOpInequality () =
        let allItems = [| this.item1; this.item2; this.item3; this.different1; this.different2 |]

        for left in allItems do
            for right in allItems do
                Assert.NotEqual(
                    left.Equals( right ),
                    (left <> right)
                )

