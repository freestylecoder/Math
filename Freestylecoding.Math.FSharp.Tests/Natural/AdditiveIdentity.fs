namespace Natural

open Xunit
open Freestylecoding.Math

type public AdditiveIdentity() =
    inherit Freestylecoding.Math.Interface.Tests.TAdditiveIdentitySelf<Natural>()

    override _.resultIdentity = Natural.Zero