namespace Natural

open Xunit
open Freestylecoding.Math

type public BinaryNumber() =
    static member P2( value:'T when 'T :> System.Numerics.IBinaryNumber<'T> ) : bool =
        'T.IsPow2( value )
    static member L2( value:'T when 'T :> System.Numerics.IBinaryNumber<'T> ) : 'T =
        'T.Log2( value )

    [<Fact>]
    member public this.IsPow2 () =
        for value = 0 to 32 do
            Assert.True( BinaryNumber.P2( Natural.Unit <<< value ) )

        Assert.Equal(
            BinaryNumber.P2( 0u ),
            BinaryNumber.P2( Natural( [0u] ) )
        )
        Assert.Equal(
            BinaryNumber.P2( 1u ),
            BinaryNumber.P2( Natural( [1u] ) )
        )
        Assert.Equal(
            BinaryNumber.P2( 3u ),
            BinaryNumber.P2( Natural( [3u] ) )
        )

        Assert.True( BinaryNumber.P2( Natural( [1u; 0u] ) ) )
        Assert.True( BinaryNumber.P2( Natural( [1u; 0u; 0u] ) ) )
        Assert.True( BinaryNumber.P2( Natural( [1u; 0u; 0u; 0u] ) ) )

        Assert.False( BinaryNumber.P2( Natural( [1u; 1u] ) ) )
        Assert.False( BinaryNumber.P2( Natural( [3u; 0u] ) ) )
        Assert.False( BinaryNumber.P2( Natural( [3u; 0u; 0u] ) ) )
        Assert.False( BinaryNumber.P2( Natural( [1u; 1u; 0u] ) ) )
        Assert.False( BinaryNumber.P2( Natural( [1u; 0u; 1u] ) ) )
        Assert.False( BinaryNumber.P2( Natural( [1u; 0u; 0u; 1u] ) ) )
        Assert.False( BinaryNumber.P2( Natural( [1u; 0u; 1u; 0u] ) ) )
        Assert.False( BinaryNumber.P2( Natural( [1u; 1u; 0u; 0u] ) ) )
        Assert.False( BinaryNumber.P2( Natural( [3u; 0u; 0u; 0u] ) ) )

    [<Fact>]
    member public this.Log2 () =
        for value in 0 .. 31 do
            Assert.Equal(
                Natural( BinaryNumber.L2( 1u <<< value ) ),
                BinaryNumber.L2( Natural.Unit <<< value )
            )

        for value in 0u .. 31u do
            Assert.Equal(
                Natural( BinaryNumber.L2( value ) ),
                BinaryNumber.L2( Natural( [value] ) )
            )

        Assert.Equal( Natural( [ 32u ] ), BinaryNumber.L2( Natural( [1u; 0u] ) ) )
        Assert.Equal( Natural( [ 64u ] ), BinaryNumber.L2( Natural( [1u; 0u; 0u] ) ) )
        Assert.Equal( Natural( [ 96u ] ), BinaryNumber.L2( Natural( [1u; 0u; 0u; 0u] ) ) )

        Assert.Equal( Natural( [ 32u ] ), BinaryNumber.L2( Natural( [1u; 1u] ) ) )
        Assert.Equal( Natural( [ 33u ] ), BinaryNumber.L2( Natural( [3u; 0u] ) ) )
        Assert.Equal( Natural( [ 64u ] ), BinaryNumber.L2( Natural( [1u; 1u; 0u] ) ) )
        Assert.Equal( Natural( [ 64u ] ), BinaryNumber.L2( Natural( [1u; 0u; 1u] ) ) )
        Assert.Equal( Natural( [ 65u ] ), BinaryNumber.L2( Natural( [3u; 0u; 0u] ) ) )
        Assert.Equal( Natural( [ 96u ] ), BinaryNumber.L2( Natural( [1u; 0u; 0u; 1u] ) ) )
        Assert.Equal( Natural( [ 96u ] ), BinaryNumber.L2( Natural( [1u; 0u; 1u; 0u] ) ) )
        Assert.Equal( Natural( [ 96u ] ), BinaryNumber.L2( Natural( [1u; 1u; 0u; 0u] ) ) )
        Assert.Equal( Natural( [ 97u ] ), BinaryNumber.L2( Natural( [3u; 0u; 0u; 0u] ) ) )
