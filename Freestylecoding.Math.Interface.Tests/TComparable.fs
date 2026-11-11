namespace Freestylecoding.Math.Interface.Tests

open Xunit

[<AbstractClass>]
type public TComparable<'T when 'T :> System.IComparable>() =
    /// <summary>The value that should be the smallest of every other member</summary>
    /// <remarks>When the 5 members are sorted in ascending order, this should be the first value</remarks>
    abstract member smallest : 'T
    /// <summary>The value that should be the bigger than smallest and smaller than every other member</summary>
    /// <remarks>When the 5 members are sorted in ascending order, this should be the second value</remarks>
    abstract member small    : 'T
    /// <summary>The value that should be the middle of every other member</summary>
    /// <remarks>When the 5 members are sorted in ascending order, this should be the third value</remarks>
    abstract member medium   : 'T
    /// <summary>The value that should be the smaller than largest and larger than every other member</summary>
    /// <remarks>When the 5 members are sorted in ascending order, this should be the fourth value</remarks>
    abstract member large    : 'T
    /// <summary>The value that should be the largest of every other member</summary>
    /// <remarks>When the 5 members are sorted in ascending order, this should be the last value</remarks>
    abstract member largest  : 'T

    [<Fact>]
    member public this.Sanity () =
        // EQUALS
        Assert.Equal( 0, this.smallest.CompareTo( this.smallest ) )
        Assert.Equal( 0, this.small.CompareTo( this.small ) )
        Assert.Equal( 0, this.medium.CompareTo( this.medium ) )
        Assert.Equal( 0, this.large.CompareTo( this.large ) )
        Assert.Equal( 0, this.largest.CompareTo( this.largest ) )

        // LESS THAN
        Assert.True( 0 > this.smallest.CompareTo( this.small ) )
        Assert.True( 0 > this.smallest.CompareTo( this.medium ) )
        Assert.True( 0 > this.smallest.CompareTo( this.large ) )
        Assert.True( 0 > this.smallest.CompareTo( this.largest ) )

        Assert.True( 0 > this.small.CompareTo( this.medium ) )
        Assert.True( 0 > this.small.CompareTo( this.large ) )
        Assert.True( 0 > this.small.CompareTo( this.largest ) )

        Assert.True( 0 > this.medium.CompareTo( this.large ) )
        Assert.True( 0 > this.medium.CompareTo( this.largest ) )

        Assert.True( 0 > this.large.CompareTo( this.largest ) )

        // GREATOR THAN
        Assert.True( 0 < this.largest.CompareTo( this.smallest ) )
        Assert.True( 0 < this.largest.CompareTo( this.small ) )
        Assert.True( 0 < this.largest.CompareTo( this.medium ) )
        Assert.True( 0 < this.largest.CompareTo( this.large ) )

        Assert.True( 0 < this.large.CompareTo( this.smallest ) )
        Assert.True( 0 < this.large.CompareTo( this.small ) )
        Assert.True( 0 < this.large.CompareTo( this.medium ) )

        Assert.True( 0 < this.medium.CompareTo( this.smallest ) )
        Assert.True( 0 < this.medium.CompareTo( this.small ) )

        Assert.True( 0 < this.small.CompareTo( this.smallest ) )

    [<Fact>]
    member public this.Ordering () =
        let r = System.Random().Next( 1, 5 );
        let arr =
            [|0..4|]
            |> Array.map ( fun i -> ( i * r ) % 5 )
            |> Array.map ( fun i -> 
                    match i with
                    | 0 -> this.smallest
                    | 1 -> this.small
                    | 2 -> this.medium
                    | 3 -> this.large
                    | 4 -> this.largest
                    | _ -> raise (System.IndexOutOfRangeException( "Shouldn't be possible because of the %" ) )
                )

        let expected = [| this.smallest; this.small; this.medium; this.large; this.largest |]
        System.Array.Sort( arr )

        Assert.Equal<'T>( expected, arr )
