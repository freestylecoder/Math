namespace Freestylecoding.Math.Interface.Tests

open Xunit

[<AbstractClass>]
type public TSubtractionOperatorsSelfOtherResult<'TSelf,'TOther,'TResult when 'TSelf :> System.Numerics.ISubtractionOperators<'TSelf,'TOther,'TResult>>() =
    let _random = System.Random()

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
    ///         If selfMin and selfMax are not None, this value ensures underflow from selfMix results in selfMax
    ///     </para>
    ///     <para>
    ///         If this type implements System.Numerics.INumberBase, it is appropriate to use INumberBase.One
    ///     </para>
    /// </remarks>
    abstract member selfDecrement  : Option<'TSelf>
    /// <summary>If this type has a minimum value, this should be that value, else None</summary>
    abstract member selfMin  : Option<'TSelf>
    /// <summary>If this type has a maximum value, this should be that value, else None</summary>
    abstract member selfMax  : Option<'TSelf>

    abstract member otherIdentity : Option<'TOther>
    abstract member otherDecrement : Option<'TOther>
    abstract member otherMin : Option<'TOther>
    abstract member otherMax : Option<'TOther>

    abstract member resultIdentity : Option<'TResult>
    abstract member resultDecrement : Option<'TResult>
    abstract member resultMin : Option<'TResult>
    abstract member resultMax : Option<'TResult>

    abstract member IsUnsigned : bool

    /// <summary>Converts a <see cref="System.Byte">byte</see> to <typeparamref name="'TSelf"/></summary>
    /// <param name="b">A <see cref="System.Byte">byte</see> to convert</param>
    /// <returns>A <typeparamref name="'TSelf"/> that is symantically the same as <paramref name="b"/></returns>
    /// <exception cref="System.NotImplementedException"><see cref="IsUnsigned">IsUnsigned</see> is <c>true</c> and this member is not overloaded</excpetion>
    /// <remarks>
    ///     <para>
    ///         This method is used internally when <see cref="IsUnsigned">IsUnsigned</see> is <c>true</c>.
    ///     </para>
    ///     <para>
    ///         The class uses random bytes to create data to test various aspects of Subtraction.
    ///         This method uses an unsigned <see cref="System.Byte">byte</see> to convert to a value of <typeparamref name="'TSelf"/>.
    ///     </para>
    ///     <para>
    ///         The conversion from <see cref="System.Byte">byte</see> to <typeparamref name="'TSelf"/> should be one-to-one.
    ///         It is acceptable, and encouraged, for the user to scale the byte by a constant value to better fill <typeparamref name="'TSelf"/>.
    ///         However, the assumption is that if <c>b1 - b2 = b3</c>, then <c>ToSelf( b1 ) - ToSelf( b2 ) = ToSelf( b3 )</c>.
    ///     </para>
    /// </remarks>
    abstract member ToSelf : byte -> 'TSelf
    default _.ToSelf( b:byte ) : 'TSelf =
        raise (new System.NotImplementedException())

    abstract member ToSelf : System.SByte -> 'TSelf
    default _.ToSelf( b:System.SByte ) : 'TSelf =
        raise (new System.NotImplementedException())

    abstract member ToOther : byte -> 'TOther
    default _.ToOther( b:byte ) : 'TOther =
        raise (new System.NotImplementedException())

    abstract member ToOther : System.SByte -> 'TOther
    default _.ToOther( b:System.SByte ) : 'TOther =
        raise (new System.NotImplementedException())

    abstract member ToResult : byte -> 'TResult
    default _.ToResult( b:byte ) : 'TResult =
        raise (new System.NotImplementedException())

    abstract member ToResult : System.SByte -> 'TResult
    default _.ToResult( b:System.SByte ) : 'TResult =
        raise (new System.NotImplementedException())

    member private this.ToSByte( b:byte ) : sbyte =
      // Yes, this is much slower than doing an unchecked cast
      // However, I want to maintain the strict ordering
      // ie: [0,255] maps exactly to [-128,127]
      // the cast would map [0,255] to [0,127]U[-128,-1]
      let i16 = System.Convert.ToInt16( b ) - 128s
      let u8 = System.Convert.ToSByte( i16 )
      sbyte b

    member private this.GetSelf( b:byte ) : 'TSelf =
        match this.IsUnsigned with
        | true -> this.ToSelf( b )
        | false -> this.ToSelf( this.ToSByte b )

    member private this.GetOther( b:byte ) : 'TOther =
        match this.IsUnsigned with
        | true -> this.ToOther( b )
        | false -> this.ToOther( this.ToSByte b )

    member private this.GetResult( b:byte ) : 'TResult =
        match this.IsUnsigned with
        | true -> this.ToResult( b )
        | false -> this.ToResult( this.ToSByte b )

    static member private Subtraction( this:'T when 'T :> System.Numerics.ISubtractionOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_Subtraction( this, that )
    member _.Subtraction( this:'TSelf, that:'TOther ) : 'TResult =
        TSubtractionOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.Subtraction( this, that )

    static member private CheckedSubtraction( this:'T when 'T :> System.Numerics.ISubtractionOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_CheckedSubtraction( this, that )
    member _.CheckedSubtraction( this:'TSelf, that:'TOther ) : 'TResult =
        TSubtractionOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.CheckedSubtraction( this, that )

    [<Fact>]
    member public this.SubtractionSanity () =
        Assert.Equal( this.GetResult( 1uy ), this.Subtraction( this.GetSelf( 1uy ), this.GetOther( 0uy ) ) )
        Assert.Equal( this.GetResult( 0uy ), this.Subtraction( this.GetSelf( 1uy ), this.GetOther( 1uy ) ) )
        Assert.Equal( this.GetResult( 1uy ), this.Subtraction( this.GetSelf( 2uy ), this.GetOther( 1uy ) ) )
        Assert.Equal( this.GetResult( 2uy ), this.Subtraction( this.GetSelf( 5uy ), this.GetOther( 3uy ) ) )
        Assert.Equal( this.GetResult( 5uy ), this.Subtraction( this.GetSelf( 12uy ), this.GetOther( 7uy ) ) )

        let (rbytes:byte array) = [| 0uy; 0uy |]
        for _ in 1 .. 5 do
            _random.NextBytes( rbytes )
            let left = this.GetSelf( Array.max rbytes )
            let right = this.GetOther( Array.min rbytes )
            let difference = this.GetResult( (Array.max rbytes) - (Array.min rbytes) )

            Assert.Equal( difference, this.Subtraction( left, right ) )

    [<Fact>]
    member public this.CheckedSubtractionSanity () =
        Assert.Equal( this.GetResult( 1uy ), this.CheckedSubtraction( this.GetSelf( 1uy ), this.GetOther( 0uy ) ) )
        Assert.Equal( this.GetResult( 0uy ), this.CheckedSubtraction( this.GetSelf( 1uy ), this.GetOther( 1uy ) ) )
        Assert.Equal( this.GetResult( 1uy ), this.CheckedSubtraction( this.GetSelf( 2uy ), this.GetOther( 1uy ) ) )
        Assert.Equal( this.GetResult( 2uy ), this.CheckedSubtraction( this.GetSelf( 5uy ), this.GetOther( 3uy ) ) )
        Assert.Equal( this.GetResult( 5uy ), this.CheckedSubtraction( this.GetSelf( 12uy ), this.GetOther( 7uy ) ) )

        let (rbytes:byte array) = [| 0uy; 0uy |]

        for _ in 1 .. 5 do
            _random.NextBytes( rbytes )
            let left = this.GetSelf( Array.max rbytes )
            let right = this.GetOther( Array.min rbytes )
            let difference = this.GetResult( (Array.max rbytes) - (Array.min rbytes) )

            Assert.Equal( difference, this.CheckedSubtraction( left, right ) )

    [<Fact>]
    member public this.CheckedSubtractionMinMinusDecrementThrowsOverflowException () =
        if this.selfMin.IsSome && this.otherDecrement.IsSome then
            let exc = Record.Exception(
                fun () -> this.CheckedSubtraction( this.selfMin.Value, this.otherDecrement.Value ) |> ignore
            )

            Assert.NotNull( exc )
            Assert.IsType<System.OverflowException>( exc ) |> ignore

    [<Fact>]
    member public this.SubtractionMinMinusDecrementProducesMax () =
        if this.selfMin.IsSome && this.otherDecrement.IsSome && this.resultMax.IsSome then
            Assert.Equal<'TResult>( this.resultMax.Value, this.Subtraction( this.selfMin.Value, this.otherDecrement.Value ) )

    [<Fact>]
    member public this.SubtractionOtherIdentity () =
        if this.otherIdentity.IsSome then
            let (rbytes:byte array) = [| 0uy; 0uy; 0uy; 0uy; 0uy |]
            _random.NextBytes( rbytes )
            for i in rbytes do
                Assert.Equal( this.GetResult( i ), this.Subtraction( this.GetSelf( i ), this.otherIdentity.Value ) )

            if this.selfIdentity.IsSome && this.resultIdentity.IsSome then
                Assert.Equal( this.resultIdentity.Value, this.Subtraction( this.selfIdentity.Value, this.otherIdentity.Value ) )

            if this.selfDecrement.IsSome && this.resultDecrement.IsSome then
                Assert.Equal( this.resultDecrement.Value, this.Subtraction( this.selfDecrement.Value, this.otherIdentity.Value ) )

[<AbstractClass>]
type public TSubtractionOperatorsSelfOtherSelf<'TSelf,'TOther when 'TSelf :> System.Numerics.ISubtractionOperators<'TSelf,'TOther,'TSelf>>() =
    inherit TSubtractionOperatorsSelfOtherResult<'TSelf,'TOther,'TSelf>()

    default this.resultIdentity = this.selfIdentity
    default this.resultDecrement = this.selfDecrement
    default this.resultMin = this.selfMin
    default this.resultMax = this.selfMax

    default this.ToResult( b:byte ) = this.ToSelf( b )
    default this.ToResult( b:sbyte ) = this.ToSelf( b )

[<AbstractClass>]
type public TSubtractionOperatorsSelfOtherOther<'TSelf,'TOther when 'TSelf :> System.Numerics.ISubtractionOperators<'TSelf,'TOther,'TOther>>() =
    inherit TSubtractionOperatorsSelfOtherResult<'TSelf,'TOther,'TOther>()

    default this.resultIdentity = this.otherIdentity
    default this.resultDecrement = this.otherDecrement
    default this.resultMin = this.otherMin
    default this.resultMax = this.otherMax

    default this.ToResult( b:byte ) = this.ToOther( b )
    default this.ToResult( b:sbyte ) = this.ToOther( b )

[<AbstractClass>]
type public TSubtractionOperatorsSelf<'TSelf when 'TSelf :> System.Numerics.ISubtractionOperators<'TSelf,'TSelf,'TSelf>>() =
    inherit TSubtractionOperatorsSelfOtherResult<'TSelf,'TSelf,'TSelf>()

    default this.otherIdentity = this.selfIdentity
    default this.otherDecrement = this.selfDecrement
    default this.otherMin = this.selfMin
    default this.otherMax = this.selfMax

    default this.resultIdentity = this.selfIdentity
    default this.resultDecrement = this.selfDecrement
    default this.resultMin = this.selfMin
    default this.resultMax = this.selfMax

    default this.ToOther( b:byte ) = this.ToSelf( b )
    default this.ToOther( b:sbyte ) = this.ToSelf( b )

    default this.ToResult( b:byte ) = this.ToSelf( b )
    default this.ToResult( b:sbyte ) = this.ToSelf( b )
