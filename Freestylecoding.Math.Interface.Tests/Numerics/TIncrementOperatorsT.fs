namespace Freestylecoding.Math.Interface.Tests

open Xunit

[<AbstractClass>]
type public TIncrementOperatorsT<'T when 'T :> System.Numerics.IIncrementOperators<'T>>() =
    abstract member item1 : 'T
    abstract member item2 : 'T
    abstract member item3 : 'T

    abstract member minValue : Option<'T>
    abstract member maxValue : Option<'T>

    // These are to give easy access to the operator in subclasses
    member _.Increment( this:'T when 'T :> System.Numerics.IIncrementOperators<'T> ) : 'T =
        'T.op_Increment( this )

    member _.CheckedIncrement( this:'T when 'T :> System.Numerics.IIncrementOperators<'T> ) : 'T =
        'T.op_CheckedIncrement( this )

    [<Fact>]
    member public this.Sanity () =
        Assert.NotEqual( this.item2, this.item1 )
        Assert.Equal( this.item2, 'T.op_Increment( this.item1 ) )
        Assert.NotEqual( this.item2, this.item1 )

    [<Fact>]
    member public this.CheckedSanity () =
        Assert.NotEqual( this.item3, this.item2 )
        Assert.Equal( this.item3, 'T.op_CheckedIncrement( this.item2 ) )
        Assert.NotEqual( this.item3, this.item2 )

    [<Fact>]
    member public this.Overflow () =
        if this.minValue.IsSome && this.maxValue.IsSome then
            Assert.Equal( this.minValue.Value, 'T.op_Increment( this.maxValue.Value ) )

    [<Fact>]
    member public this.CheckedOverflow () =
        if this.maxValue.IsSome then
            let exc = Record.Exception(
                fun () -> 'T.op_CheckedIncrement( this.maxValue.Value ) |> ignore
            )

            Assert.NotNull( exc )
            Assert.IsType<System.OverflowException>( exc ) |> ignore
