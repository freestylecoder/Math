namespace Freestylecoding.Math.Interface.Tests

open Xunit

[<AbstractClass>]
type public TAdditiveIdentitySelfResult<'TSelf,'TResult when 'TSelf :> System.Numerics.IAdditiveIdentity<'TSelf,'TResult>>() =
    abstract member resultIdentity : 'TResult

    [<Fact>]
    member public this.IsIdentityCorrect () =
        Assert.Equal(
            this.resultIdentity,
            'TSelf.AdditiveIdentity
        )

[<AbstractClass>]
type public TAdditiveIdentitySelf<'TSelf when 'TSelf :> System.Numerics.IAdditiveIdentity<'TSelf,'TSelf>>() =
    inherit TAdditiveIdentitySelfResult<'TSelf,'TSelf>()