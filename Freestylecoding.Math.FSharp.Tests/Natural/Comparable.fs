namespace Natural

open Xunit
open Freestylecoding.Math

type public Comparable() =
    inherit Freestylecoding.Math.Interface.Tests.TComparable<Natural>()

    let eq actual = actual = 0
    let gt actual = actual > 0
    let lt actual = actual < 0

    override this.smallest = Natural.Zero
    override this.small    = Natural.Unit
    override this.medium   = Natural( [System.UInt32.MaxValue] )
    override this.large    = Natural( [1u; System.UInt32.MaxValue] )
    override this.largest  = Natural( [1u; 0u; 0u] )

    [<Fact>]
    member public this.Equals () =
        let b = System.Convert.ToByte( System.Random().Next( 0, 256 ) )
        let left = Natural( b ) :> System.IComparable

        Assert.True( eq (left.CompareTo( Natural( b ) )) )
        Assert.True( eq (left.CompareTo( b )) )
        Assert.True( eq (left.CompareTo( uint16 b )) )
        Assert.True( eq (left.CompareTo( uint32 b )) )
        Assert.True( eq (left.CompareTo( uint64 b )) )
        Assert.True( eq (left.CompareTo( System.UInt128( 0uL, uint64 b ) )) )
        Assert.True( eq (left.CompareTo( System.Numerics.BigInteger b )) )

    [<Fact>]
    member public this.GreaterThan () =
        let left = Natural( System.Convert.ToUInt32( System.Random().Next( 128, 256 ) ) ) :> System.IComparable
        let b = System.Convert.ToByte( System.Random().Next( 0, 127 ) )

        Assert.True( gt (left.CompareTo( Natural( b ) )) )
        Assert.True( gt (left.CompareTo( b )) )
        Assert.True( gt (left.CompareTo( uint16 b )) )
        Assert.True( gt (left.CompareTo( uint32 b )) )
        Assert.True( gt (left.CompareTo( uint64 b )) )
        Assert.True( gt (left.CompareTo( System.UInt128( 0uL, uint64 b ) )) )
        Assert.True( gt (left.CompareTo( System.Numerics.BigInteger b )) )

    [<Fact>]
    member public this.GreaterThan_NegativeBigInteger () =
        let left = Natural.Unit :> System.IComparable

        Assert.True( gt (left.CompareTo( System.Numerics.BigInteger( -1m ) )) )

    [<Fact>]
    member public this.LessThan () =
        let left = Natural( System.Convert.ToUInt32( System.Random().Next( 0, 127 ) ) ) :> System.IComparable
        let b = System.Convert.ToByte( System.Random().Next( 128, 256 ) )

        Assert.True( lt (left.CompareTo( Natural( b ) )) )
        Assert.True( lt (left.CompareTo( b )) )
        Assert.True( lt (left.CompareTo( uint16 b )) )
        Assert.True( lt (left.CompareTo( uint32 b )) )
        Assert.True( lt (left.CompareTo( uint64 b )) )
        Assert.True( lt (left.CompareTo( System.UInt128( 0uL, uint64 b ) )) )
        Assert.True( lt (left.CompareTo( System.Numerics.BigInteger b )) )

    [<Fact>]
    member public this.LessThan_NegativeBigInteger () =
        let left = Natural.Unit :> System.IComparable

        Assert.False( lt (left.CompareTo( System.Numerics.BigInteger( -1m ) )) )

    [<Theory>]
    [<InlineData( 1  )>]
    [<InlineData( 1L )>]
    [<InlineData( 1F )>]
    [<InlineData( 1.0 )>]
    [<InlineData( "1" )>]
    member public this.Incompatible value =
        let one = Natural.Unit :> System.IComparable
        let exc = Record.Exception(
            fun () ->
                one.CompareTo( value )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.ArgumentException>( exc )
