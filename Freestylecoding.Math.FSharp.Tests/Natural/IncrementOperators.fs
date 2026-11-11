namespace Natural

open Xunit
open Freestylecoding.Math

// NOTE: F# doesn't really have increment/decrement operators
//  It would mean the value is mutable
//  I'll get these tested in the C# tests
//	Maybe, after i confirm everything is working, I'll loop back and play with mutable to try it

type public IncrementOperators() =
    inherit Freestylecoding.Math.Interface.Tests.TIncrementOperatorsT<Natural>()

    override _.item1 = Natural( [1u] )
    override _.item2 = Natural( [2u] )
    override _.item3 = Natural( [3u] )

    override _.minValue = Some( Natural.Zero )
    override _.maxValue = None


    [<Fact>]
    member public this.OverflowGrows () =
        Assert.Equal(
            Natural( [1u; 0u] ),
            this.Increment(
                Natural( [0xFFFF_FFFFu] )
            )
        )
        
    [<Fact>]
    member public this.CascadingOverflow () =
        Assert.Equal(
            Natural( [1u; 0u; 0u] ),
            this.Increment(
                Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
            )
        )

    [<Fact>]
    member public this.CheckedOverflowGrows () =
        Assert.Equal(
            Natural( [1u; 0u] ),
            this.CheckedIncrement(
                Natural( [0xFFFF_FFFFu] )
            )
        )
        
    [<Fact>]
    member public this.CascadingCheckedOverflow () =
        Assert.Equal(
            Natural( [1u; 0u; 0u] ),
            this.CheckedIncrement(
                Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
            )
        )
