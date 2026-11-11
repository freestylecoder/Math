namespace Natural

open Xunit
open Freestylecoding.Math

type public EquatableNatural() =
    inherit Freestylecoding.Math.Interface.Tests.TEquatableT<Natural>()

    override this.item1 = Natural.Unit
    override this.item2 = Natural.Unit
    override this.item3 = Natural.Unit
    override this.different1 = Natural.Zero
    override this.different2 = Natural( [System.UInt32.MaxValue] )

    [<Fact>]
    member public this.LargeNaturalsTrue () =
        Assert.True(
            (Natural( [0xFu; 0x00000101u] ) :> System.IEquatable<Natural>).Equals( Natural( [0xFu; 0x00000101u] ) )
        )

    [<Fact>]
    member public this.LargeNaturalsFalse () =
        Assert.False(
            (Natural( [0x8u; 0x00000101u] ) :> System.IEquatable<Natural>).Equals( Natural( [0xFu; 0x00000101u] ) )
        )
