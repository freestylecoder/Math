namespace Freestylecoding.Math.Interface.Tests

open Xunit

[<AbstractClass>]
type public TComparisonOperatorsSelfOtherResult<'TSelf,'TOther,'TResult when 'TSelf :> System.Numerics.IComparisonOperators<'TSelf,'TOther,'TResult>>() =
    inherit TEqualityOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>()

    /// <summary>This value should be less than <see cref="self1">self1</see> and equal to <see cref="other0">other0</see></summary>
    abstract member self0 : 'TSelf
    /// <summary>This value should be greater than <see cref="self2">self2</see> and equal to <see cref="other3">other3</see></summary>
    abstract member self3 : 'TSelf

    /// <summary>This value should be less than <see cref="other1">other1</see> and equal to <see cref="self0">self0</see></summary>
    abstract member other0 : 'TOther
    /// <summary>This value should be greater than <see cref="other2">other2</see> and equal to <see cref="self3">self3</see></summary>
    abstract member other3 : 'TOther

    /// <summary>The expected result of <c>GreaterThan( self0, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt00 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self0, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt01 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self0, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt02 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self0, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt03 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self1, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gt10 : 'TResult  // true
    /// <summary>The expected result of <c>GreaterThan( self1, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt11 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self1, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt12 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self1, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt13 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self2, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gt20 : 'TResult  // true
    /// <summary>The expected result of <c>GreaterThan( self2, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gt21 : 'TResult  // true
    /// <summary>The expected result of <c>GreaterThan( self2, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt22 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self2, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt23 : 'TResult  // false
    /// <summary>The expected result of <c>GreaterThan( self3, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gt30 : 'TResult  // true
    /// <summary>The expected result of <c>GreaterThan( self3, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gt31 : 'TResult  // true
    /// <summary>The expected result of <c>GreaterThan( self3, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gt32 : 'TResult  // true
    /// <summary>The expected result of <c>GreaterThan( self3, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gt33 : 'TResult  // false

    /// <summary>The expected result of <c>GreaterThanOrEqual( self0, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte00 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self0, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gte01 : 'TResult // false
    /// <summary>The expected result of <c>GreaterThanOrEqual( self0, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gte02 : 'TResult // false
    /// <summary>The expected result of <c>GreaterThanOrEqual( self0, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gte03 : 'TResult // false
    /// <summary>The expected result of <c>GreaterThanOrEqual( self1, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte10 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self1, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte11 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self1, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gte12 : 'TResult // false
    /// <summary>The expected result of <c>GreaterThanOrEqual( self1, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gte13 : 'TResult // false
    /// <summary>The expected result of <c>GreaterThanOrEqual( self2, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte20 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self2, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte21 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self2, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte22 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self2, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member gte23 : 'TResult // false
    /// <summary>The expected result of <c>GreaterThanOrEqual( self3, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte30 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self3, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte31 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self3, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte32 : 'TResult // true
    /// <summary>The expected result of <c>GreaterThanOrEqual( self3, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member gte33 : 'TResult // true
                                      
    /// <summary>The expected result of <c>LessThan( self0, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt00 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self0, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lt01 : 'TResult  // true
    /// <summary>The expected result of <c>LessThan( self0, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lt02 : 'TResult  // true
    /// <summary>The expected result of <c>LessThan( self0, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lt03 : 'TResult  // true
    /// <summary>The expected result of <c>LessThan( self1, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt10 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self1, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt11 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self1, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lt12 : 'TResult  // true
    /// <summary>The expected result of <c>LessThan( self1, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lt13 : 'TResult  // true
    /// <summary>The expected result of <c>LessThan( self2, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt20 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self2, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt21 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self2, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt22 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self2, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lt23 : 'TResult  // true
    /// <summary>The expected result of <c>LessThan( self3, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt30 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self3, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt31 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self3, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt32 : 'TResult  // false
    /// <summary>The expected result of <c>LessThan( self3, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lt33 : 'TResult  // false
                                      
    /// <summary>The expected result of <c>LessThanOrEqual( self0, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte00 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self0, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte01 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self0, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte02 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self0, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte03 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self1, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lte10 : 'TResult // false
    /// <summary>The expected result of <c>LessThanOrEqual( self1, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte11 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self1, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte12 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self1, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte13 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self2, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lte20 : 'TResult // false
    /// <summary>The expected result of <c>LessThanOrEqual( self2, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lte21 : 'TResult // false
    /// <summary>The expected result of <c>LessThanOrEqual( self2, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte22 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self2, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte23 : 'TResult // true
    /// <summary>The expected result of <c>LessThanOrEqual( self3, other0 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lte30 : 'TResult // false
    /// <summary>The expected result of <c>LessThanOrEqual( self3, other1 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lte31 : 'TResult // false
    /// <summary>The expected result of <c>LessThanOrEqual( self3, other2 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>false</c></remarks>
    abstract member lte32 : 'TResult // false
    /// <summary>The expected result of <c>LessThanOrEqual( self3, other3 )</c></summary>
    /// <remarks>This should be symantically equivalent to <c>true</c></remarks>
    abstract member lte33 : 'TResult // true

    static member private GreaterThan( this:'T when 'T :> System.Numerics.IComparisonOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_GreaterThan( this, that )
    member _.GreaterThan( this:'TSelf, that:'TOther ) : 'TResult =
        TComparisonOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.GreaterThan( this, that )

    static member private GreaterThanOrEqual( this:'T when 'T :> System.Numerics.IComparisonOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_GreaterThanOrEqual( this, that )
    member _.GreaterThanOrEqual( this:'TSelf, that:'TOther ) : 'TResult =
        TComparisonOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.GreaterThanOrEqual( this, that )

    static member private LessThan( this:'T when 'T :> System.Numerics.IComparisonOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_LessThan( this, that )
    member _.LessThan( this:'TSelf, that:'TOther ) : 'TResult =
        TComparisonOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.LessThan( this, that )

    static member private LessThanOrEqual( this:'T when 'T :> System.Numerics.IComparisonOperators<'T,'U,'V>, that:'U ) : 'V =
        'T.op_LessThanOrEqual( this, that )
    member _.LessThanOrEqual( this:'TSelf, that:'TOther ) : 'TResult =
        TComparisonOperatorsSelfOtherResult<'TSelf,'TOther,'TResult>.LessThanOrEqual( this, that )

    [<Fact>]
    member public this.GreaterThanSanity () =
        Assert.Equal( this.gt00, this.GreaterThan( this.self0, this.other0 ) )
        Assert.Equal( this.gt01, this.GreaterThan( this.self0, this.other1 ) )
        Assert.Equal( this.gt02, this.GreaterThan( this.self0, this.other2 ) )
        Assert.Equal( this.gt03, this.GreaterThan( this.self0, this.other3 ) )

        Assert.Equal( this.gt10, this.GreaterThan( this.self1, this.other0 ) )
        Assert.Equal( this.gt11, this.GreaterThan( this.self1, this.other1 ) )
        Assert.Equal( this.gt12, this.GreaterThan( this.self1, this.other2 ) )
        Assert.Equal( this.gt13, this.GreaterThan( this.self1, this.other3 ) )

        Assert.Equal( this.gt20, this.GreaterThan( this.self2, this.other0 ) )
        Assert.Equal( this.gt21, this.GreaterThan( this.self2, this.other1 ) )
        Assert.Equal( this.gt22, this.GreaterThan( this.self2, this.other2 ) )
        Assert.Equal( this.gt23, this.GreaterThan( this.self2, this.other3 ) )

        Assert.Equal( this.gt30, this.GreaterThan( this.self3, this.other0 ) )
        Assert.Equal( this.gt31, this.GreaterThan( this.self3, this.other1 ) )
        Assert.Equal( this.gt32, this.GreaterThan( this.self3, this.other2 ) )
        Assert.Equal( this.gt33, this.GreaterThan( this.self3, this.other3 ) )

    [<Fact>]
    member public this.GreaterThanOrEqualSanity () =
        Assert.Equal( this.gte00, this.GreaterThanOrEqual( this.self0, this.other0 ) )
        Assert.Equal( this.gte01, this.GreaterThanOrEqual( this.self0, this.other1 ) )
        Assert.Equal( this.gte02, this.GreaterThanOrEqual( this.self0, this.other2 ) )
        Assert.Equal( this.gte03, this.GreaterThanOrEqual( this.self0, this.other3 ) )
                             
        Assert.Equal( this.gte10, this.GreaterThanOrEqual( this.self1, this.other0 ) )
        Assert.Equal( this.gte11, this.GreaterThanOrEqual( this.self1, this.other1 ) )
        Assert.Equal( this.gte12, this.GreaterThanOrEqual( this.self1, this.other2 ) )
        Assert.Equal( this.gte13, this.GreaterThanOrEqual( this.self1, this.other3 ) )
                             
        Assert.Equal( this.gte20, this.GreaterThanOrEqual( this.self2, this.other0 ) )
        Assert.Equal( this.gte21, this.GreaterThanOrEqual( this.self2, this.other1 ) )
        Assert.Equal( this.gte22, this.GreaterThanOrEqual( this.self2, this.other2 ) )
        Assert.Equal( this.gte23, this.GreaterThanOrEqual( this.self2, this.other3 ) )
                             
        Assert.Equal( this.gte30, this.GreaterThanOrEqual( this.self3, this.other0 ) )
        Assert.Equal( this.gte31, this.GreaterThanOrEqual( this.self3, this.other1 ) )
        Assert.Equal( this.gte32, this.GreaterThanOrEqual( this.self3, this.other2 ) )
        Assert.Equal( this.gte33, this.GreaterThanOrEqual( this.self3, this.other3 ) )

    [<Fact>]
    member public this.LessThanSanity () =
        Assert.Equal( this.lt00, this.LessThan( this.self0, this.other0 ) )
        Assert.Equal( this.lt01, this.LessThan( this.self0, this.other1 ) )
        Assert.Equal( this.lt02, this.LessThan( this.self0, this.other2 ) )
        Assert.Equal( this.lt03, this.LessThan( this.self0, this.other3 ) )

        Assert.Equal( this.lt10, this.LessThan( this.self1, this.other0 ) )
        Assert.Equal( this.lt11, this.LessThan( this.self1, this.other1 ) )
        Assert.Equal( this.lt12, this.LessThan( this.self1, this.other2 ) )
        Assert.Equal( this.lt13, this.LessThan( this.self1, this.other3 ) )

        Assert.Equal( this.lt20, this.LessThan( this.self2, this.other0 ) )
        Assert.Equal( this.lt21, this.LessThan( this.self2, this.other1 ) )
        Assert.Equal( this.lt22, this.LessThan( this.self2, this.other2 ) )
        Assert.Equal( this.lt23, this.LessThan( this.self2, this.other3 ) )

        Assert.Equal( this.lt30, this.LessThan( this.self3, this.other0 ) )
        Assert.Equal( this.lt31, this.LessThan( this.self3, this.other1 ) )
        Assert.Equal( this.lt32, this.LessThan( this.self3, this.other2 ) )
        Assert.Equal( this.lt33, this.LessThan( this.self3, this.other3 ) )

    [<Fact>]
    member public this.LessThanOrEqualSanity () =
        Assert.Equal( this.lte00, this.LessThanOrEqual( this.self0, this.other0 ) )
        Assert.Equal( this.lte01, this.LessThanOrEqual( this.self0, this.other1 ) )
        Assert.Equal( this.lte02, this.LessThanOrEqual( this.self0, this.other2 ) )
        Assert.Equal( this.lte03, this.LessThanOrEqual( this.self0, this.other3 ) )
                             
        Assert.Equal( this.lte10, this.LessThanOrEqual( this.self1, this.other0 ) )
        Assert.Equal( this.lte11, this.LessThanOrEqual( this.self1, this.other1 ) )
        Assert.Equal( this.lte12, this.LessThanOrEqual( this.self1, this.other2 ) )
        Assert.Equal( this.lte13, this.LessThanOrEqual( this.self1, this.other3 ) )
                             
        Assert.Equal( this.lte20, this.LessThanOrEqual( this.self2, this.other0 ) )
        Assert.Equal( this.lte21, this.LessThanOrEqual( this.self2, this.other1 ) )
        Assert.Equal( this.lte22, this.LessThanOrEqual( this.self2, this.other2 ) )
        Assert.Equal( this.lte23, this.LessThanOrEqual( this.self2, this.other3 ) )
                             
        Assert.Equal( this.lte30, this.LessThanOrEqual( this.self3, this.other0 ) )
        Assert.Equal( this.lte31, this.LessThanOrEqual( this.self3, this.other1 ) )
        Assert.Equal( this.lte32, this.LessThanOrEqual( this.self3, this.other2 ) )
        Assert.Equal( this.lte33, this.LessThanOrEqual( this.self3, this.other3 ) )

[<AbstractClass>]
type public TComparisonOperatorsSelfOther<'TSelf,'TOther when 'TSelf :> System.Numerics.IComparisonOperators<'TSelf,'TOther,bool>>() =
    inherit TComparisonOperatorsSelfOtherResult<'TSelf,'TOther,bool>()

    default _.equality11 = true
    default _.equality12 = false
    default _.equality21 = false
    default _.equality22 = true

    default this.inequality11 = not this.equality11
    default this.inequality12 = not this.equality12
    default this.inequality21 = not this.equality21
    default this.inequality22 = not this.equality22

    default _.gt00 = false
    default _.gt01 = false
    default _.gt02 = false
    default _.gt03 = false
    default _.gt10 = true
    default _.gt11 = false
    default _.gt12 = false
    default _.gt13 = false
    default _.gt20 = true
    default _.gt21 = true
    default _.gt22 = false
    default _.gt23 = false
    default _.gt30 = true
    default _.gt31 = true
    default _.gt32 = true
    default _.gt33 = false
    
    default _.gte00 = true
    default _.gte01 = false
    default _.gte02 = false
    default _.gte03 = false
    default _.gte10 = true
    default _.gte11 = true
    default _.gte12 = false
    default _.gte13 = false
    default _.gte20 = true
    default _.gte21 = true
    default _.gte22 = true
    default _.gte23 = false
    default _.gte30 = true
    default _.gte31 = true
    default _.gte32 = true
    default _.gte33 = true

    default _.lt00 = false
    default _.lt01 = true
    default _.lt02 = true
    default _.lt03 = true
    default _.lt10 = false
    default _.lt11 = false
    default _.lt12 = true
    default _.lt13 = true
    default _.lt20 = false
    default _.lt21 = false
    default _.lt22 = false
    default _.lt23 = true
    default _.lt30 = false
    default _.lt31 = false
    default _.lt33 = false
    default _.lt32 = false

    default _.lte00 = true
    default _.lte01 = true
    default _.lte02 = true
    default _.lte03 = true
    default _.lte10 = false
    default _.lte11 = true
    default _.lte12 = true
    default _.lte13 = true
    default _.lte20 = false
    default _.lte21 = false
    default _.lte22 = true
    default _.lte23 = true
    default _.lte30 = false
    default _.lte31 = false
    default _.lte32 = false
    default _.lte33 = true

[<AbstractClass>]
type public TComparisonOperatorsSelf<'TSelf when 'TSelf :> System.Numerics.IComparisonOperators<'TSelf,'TSelf,bool>>() =
    inherit TComparisonOperatorsSelfOther<'TSelf,'TSelf>()
    
    default this.other0 = this.self0
    default this.other1 = this.self1
    default this.other2 = this.self2
    default this.other3 = this.self3
    