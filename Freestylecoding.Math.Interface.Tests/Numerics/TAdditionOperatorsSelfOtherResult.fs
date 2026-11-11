namespace Freestylecoding.Math.Interface.Tests

open Xunit

[<AbstractClass>]
type public TAdditionOperatorsSelfOtherResult<'TSelf,'TOther,'TResult when 'TSelf :> System.Numerics.IAdditionOperators<'TSelf,'TOther,'TResult>>() =
    /// <summary>This value should by symantically equal to Zero</summary>
    /// <remarks>
    ///     <para>
    ///         If this type implements System.Numerics.IAdditiveIdentity, it is appropriate to use IAdditiveIdentity.AdditiveIdentity
    ///     </para>
    ///     <para>
    ///         If this type implements System.Numerics.INumberBase, it is appropriate to use INumberBase.Zero
    ///     </para>
    /// </remarks>
    abstract member selfIdentity : Option<'TSelf>
    /// <summary>This value should by symantically equal to One</summary>
    /// <remarks>
    ///     <para>
    ///         If selfMin and selfMax are not None, this value ensures overflow from selfMax results in selfMin
    ///     </para>
    ///     <para>
    ///         If this type implements System.Numerics.INumberBase, it is appropriate to use INumberBase.One
    ///     </para>
    /// </remarks>
    abstract member selfIncrement  : Option<'TSelf>
    /// <summary>This can be any vaule in which self1 + other1 and self1 + other2 has a valid sum in TResult</summary>
    abstract member self1    : 'TSelf
    /// <summary>This can be any vaule in which self2 + other1 and self2 + other2 has a valid sum in TResult</summary>
    abstract member self2    : 'TSelf
    /// <summary>If this type has a minimum value, this should be that value, else None</summary>
    abstract member selfMin  : Option<'TSelf>
    /// <summary>If this type has a maximum value, this should be that value, else None</summary>
    abstract member selfMax  : Option<'TSelf>

    abstract member otherIdentity : Option<'TOther>
    abstract member otherIncrement : Option<'TOther>
    abstract member other1 : 'TOther
    abstract member other2 : 'TOther
    abstract member otherMin : Option<'TOther>
    abstract member otherMax : Option<'TOther>

    abstract member resultIdentity : Option<'TResult>
    abstract member resultIncrement : Option<'TResult>
    abstract member result1 : 'TResult
    abstract member result2 : 'TResult
    abstract member resultMin : Option<'TResult>
    abstract member resultMax : Option<'TResult>

    abstract member sum11 : 'TResult
    abstract member sum12 : 'TResult
    abstract member sum21 : 'TResult
    abstract member sum22 : 'TResult

    static member private Addition( this:'T when 'T :> System.Numerics.IAdditionOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_Addition( this, that )
    member _.Addition( this:'TSelf, that:'TOther ) : 'TResult =
        TAdditionOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.Addition( this, that )

    static member private CheckedAddition( this:'T when 'T :> System.Numerics.IAdditionOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_CheckedAddition( this, that )
    member _.CheckedAddition( this:'TSelf, that:'TOther ) : 'TResult =
        TAdditionOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.CheckedAddition( this, that )

    [<Fact>]
    member public this.AdditionSanity () =
        Assert.Equal( this.sum11, this.Addition( this.self1, this.other1 ) )
        Assert.Equal( this.sum12, this.Addition( this.self1, this.other2 ) )
        Assert.Equal( this.sum21, this.Addition( this.self2, this.other1 ) )
        Assert.Equal( this.sum22, this.Addition( this.self2, this.other2 ) )

    [<Fact>]
    member public this.CheckedAdditionSanity () =
        Assert.Equal( this.sum11, this.CheckedAddition( this.self1, this.other1 ) )
        Assert.Equal( this.sum12, this.CheckedAddition( this.self1, this.other2 ) )
        Assert.Equal( this.sum21, this.CheckedAddition( this.self2, this.other1 ) )
        Assert.Equal( this.sum22, this.CheckedAddition( this.self2, this.other2 ) )

    [<Fact>]
    member public this.CheckedAdditionMaxPlusIncrementThrowsOverflowException () =
        if this.selfMax.IsSome && this.otherIdentity.IsSome then
            let exc = Record.Exception(
                fun () -> this.CheckedAddition( this.selfMax.Value, this.otherIdentity.Value ) |> ignore
            )

            Assert.NotNull( exc )
            Assert.IsType<System.OverflowException>( exc ) |> ignore

    [<Fact>]
    member public this.AdditionMaxPlusIncrementThrowsOverflowException () =
        if this.selfMax.IsSome && this.otherIdentity.IsSome && this.resultMin.IsSome then
            Assert.Equal<'TResult>( this.resultMin.Value, this.Addition( this.selfMax.Value, this.otherIncrement.Value ) )

    [<Fact>]
    member public this.AdditionSelfIdentity () =
        if this.selfIdentity.IsSome then
            Assert.Equal( this.result1, this.Addition( this.selfIdentity.Value, this.other1 ) )
            Assert.Equal( this.result2, this.Addition( this.selfIdentity.Value, this.other2 ) )

            if this.otherIdentity.IsSome && this.resultIdentity.IsSome then
                Assert.Equal( this.resultIdentity.Value, this.Addition( this.selfIdentity.Value, this.otherIdentity.Value ) )

            if this.otherIncrement.IsSome && this.resultIncrement.IsSome then
                Assert.Equal( this.resultIncrement.Value, this.Addition( this.selfIdentity.Value, this.otherIncrement.Value ) )

    [<Fact>]
    member public this.AdditionOtherIdentity () =
        if this.otherIdentity.IsSome then
            Assert.Equal( this.result1, this.Addition( this.self1, this.otherIdentity.Value ) )
            Assert.Equal( this.result2, this.Addition( this.self2, this.otherIdentity.Value ) )

            if this.selfIdentity.IsSome && this.resultIdentity.IsSome then
                Assert.Equal( this.resultIdentity.Value, this.Addition( this.selfIdentity.Value, this.otherIdentity.Value ) )

            if this.selfIncrement.IsSome && this.resultIncrement.IsSome then
                Assert.Equal( this.resultIncrement.Value, this.Addition( this.selfIncrement.Value, this.otherIdentity.Value ) )

    [<Fact>]
    member public this.AdditionCommutivity () =
        // By definition Addition should have the property of a + b = b + a
        Assert.Equal( this.sum21, this.Addition( this.self1, this.other2 ) )
        Assert.Equal( this.sum12, this.Addition( this.self2, this.other1 ) )

[<AbstractClass>]
type public TAdditionOperatorsSelfOtherSelf<'TSelf,'TOther when 'TSelf :> System.Numerics.IAdditionOperators<'TSelf,'TOther,'TSelf>>() =
    inherit TAdditionOperatorsSelfOtherResult<'TSelf,'TOther,'TSelf>()

    default this.resultIdentity = this.selfIdentity
    default this.resultIncrement = this.selfIncrement
    default this.result1 = this.self1
    default this.result2 = this.self2
    default this.resultMin = this.selfMin
    default this.resultMax = this.selfMax

[<AbstractClass>]
type public TAdditionOperatorsSelfOtherOther<'TSelf,'TOther when 'TSelf :> System.Numerics.IAdditionOperators<'TSelf,'TOther,'TOther>>() =
    inherit TAdditionOperatorsSelfOtherResult<'TSelf,'TOther,'TOther>()

    default this.resultIdentity = this.otherIdentity
    default this.resultIncrement = this.otherIncrement
    default this.result1 = this.other1
    default this.result2 = this.other2
    default this.resultMin = this.otherMin
    default this.resultMax = this.otherMax

[<AbstractClass>]
type public TAdditionOperatorsSelf<'TSelf when 'TSelf :> System.Numerics.IAdditionOperators<'TSelf,'TSelf,'TSelf>>() =
    inherit TAdditionOperatorsSelfOtherResult<'TSelf,'TSelf,'TSelf>()

    default this.otherIdentity = this.selfIdentity
    default this.otherIncrement = this.selfIncrement
    default this.other1 = this.self1
    default this.other2 = this.self2
    default this.otherMin = this.selfMin
    default this.otherMax = this.selfMax

    default this.resultIdentity = this.selfIdentity
    default this.resultIncrement = this.selfIncrement
    default this.result1 = this.self1
    default this.result2 = this.self2
    default this.resultMin = this.selfMin
    default this.resultMax = this.selfMax

