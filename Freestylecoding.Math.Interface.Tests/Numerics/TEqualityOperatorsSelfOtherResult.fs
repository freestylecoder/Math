namespace Freestylecoding.Math.Interface.Tests

open Xunit

[<AbstractClass>]
type public TEqualityOperatorsSelfOtherResult<'TSelf,'TOther,'TResult when 'TSelf :> System.Numerics.IEqualityOperators<'TSelf,'TOther,'TResult>>() =
    /// <summary>This value should be less than <see cref="self2">self2</see> and equal to <see cref="other1">other1</see></summary>
    abstract member self1 : 'TSelf
    /// <summary>This value should be greater than <see cref="self1">self1</see> and equal to <see cref="other2">other2</see></summary>
    abstract member self2 : 'TSelf

    /// <summary>This value should be less than <see cref="other2">other2</see> and equal to <see cref="self1">self1</see></summary>
    abstract member other1 : 'TOther
    /// <summary>This value should be greater than <see cref="other1">other1</see> and equal to <see cref="self2">self2</see></summary>
    abstract member other2 : 'TOther

    /// <summary>The expected result of <c>Equality( self1, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member equality11 : 'TResult
    /// <summary>The expected result of <c>Equality( self1, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member equality12 : 'TResult
    /// <summary>The expected result of <c>Equality( self2, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member equality21 : 'TResult
    /// <summary>The expected result of <c>Equality( self2, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member equality22 : 'TResult

    /// <summary>The expected result of <c>Inequality( self1, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member inequality11 : 'TResult
    /// <summary>The expected result of <c>Inequality( self1, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member inequality12 : 'TResult
    /// <summary>The expected result of <c>Inequality( self2, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member inequality21 : 'TResult
    /// <summary>The expected result of <c>Inequality( self2, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member inequality22 : 'TResult

    static member private Equality( this:'T when 'T :> System.Numerics.IEqualityOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_Equality( this, that )
    member _.Equality( this:'TSelf, that:'TOther ) : 'TResult =
        TEqualityOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.Equality( this, that )

    static member private Inequality( this:'T when 'T :> System.Numerics.IEqualityOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_Inequality( this, that )
    member _.Inequality( this:'TSelf, that:'TOther ) : 'TResult =
        TEqualityOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.Inequality( this, that )

    [<Fact>]
    member public this.EqualitySanity () =
        Assert.Equal( this.equality11, this.Equality( this.self1, this.other1 ) )
        Assert.Equal( this.equality12, this.Equality( this.self1, this.other2 ) )
        Assert.Equal( this.equality21, this.Equality( this.self2, this.other1 ) )
        Assert.Equal( this.equality22, this.Equality( this.self2, this.other2 ) )

    [<Fact>]
    member public this.InequalitySanity () =
        Assert.Equal( this.inequality11, this.Inequality( this.self1, this.other1 ) )
        Assert.Equal( this.inequality12, this.Inequality( this.self1, this.other2 ) )
        Assert.Equal( this.inequality21, this.Inequality( this.self2, this.other1 ) )
        Assert.Equal( this.inequality22, this.Inequality( this.self2, this.other2 ) )

[<AbstractClass>]
type public TEqualityOperatorsSelfOther<'TSelf,'TOther when 'TSelf :> System.Numerics.IEqualityOperators<'TSelf,'TOther,bool>>() =
    inherit TEqualityOperatorsSelfOtherResult<'TSelf,'TOther,bool>()

    default _.equality11 = true
    default _.equality12 = false
    default _.equality21 = false
    default _.equality22 = true

    default this.inequality11 = not this.equality11
    default this.inequality12 = not this.equality12
    default this.inequality21 = not this.equality21
    default this.inequality22 = not this.equality22

[<AbstractClass>]
type public TEqualityOperatorsSelf<'TSelf when 'TSelf :> System.Numerics.IEqualityOperators<'TSelf,'TSelf,bool>>() =
    inherit TEqualityOperatorsSelfOther<'TSelf,'TSelf>()

    default this.other1 = this.self1
    default this.other2 = this.self2
