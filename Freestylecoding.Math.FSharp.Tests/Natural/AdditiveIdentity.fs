namespace Natural

open Xunit
open Freestylecoding.Math

type public AdditiveIdentity() =
    static member GetAdditiveIdentity( this:'T when 'T :> System.Numerics.IAdditiveIdentity<'T,'T> ) : 'T =
        'T.AdditiveIdentity

    [<Fact>]
    member public this.IsIdentityCorrect () =
        Assert.Equal(
            Natural.Zero,
            AdditiveIdentity.GetAdditiveIdentity( Natural.Unit )
        )