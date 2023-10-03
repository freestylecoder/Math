namespace Natural

open Xunit
open Freestylecoding.Math

type public MultiplicativeIdentity() =
    static member GetMultiplicativeIdentity( this:'T when 'T :> System.Numerics.IMultiplicativeIdentity<'T,'T> ) : 'T =
        'T.MultiplicativeIdentity

    [<Fact>]
    member public this.IsIdentityCorrect () =
        Assert.Equal(
            Natural.Unit,
            MultiplicativeIdentity.GetMultiplicativeIdentity( Natural.Unit )
        )