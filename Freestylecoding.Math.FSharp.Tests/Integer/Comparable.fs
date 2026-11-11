namespace Integer

open Xunit
open Freestylecoding.Math

type public Comparable() =
    let eq actual = actual = 0
    let gt actual = actual > 0
    let lt actual = actual < 0

    [<Theory>]
    [<InlineData(  0,  0,  0 )>]
    [<InlineData(  1,  1,  0 )>]
    [<InlineData(  1,  0,  1 )>]
    [<InlineData(  0,  1, -1 )>]
    [<InlineData( -1,  1, -1 )>]
    [<InlineData(  1, -1,  1 )>]
    [<InlineData( -1, -1,  0 )>]
    member public this.Sanity (left:int) (right:int) (expected:int) =
        let test =
            match expected with
            | 0 -> eq
            | p when p > 0 -> gt
            | n when n < 0 -> lt
            | _ -> raise (System.Exception( "Not Possible" ))

        Assert.True(
            test ((Integer( left ) :> System.IComparable).CompareTo( Integer( right )) )
        )

    [<Fact>]
    member public this.Equals () =
        // NOTE: This only goes to 127 so that I know the cast to signed byte will work
        let b = System.Convert.ToByte( System.Random().Next( 0, 127 ) )
        let left = Integer( int16 b ) :> System.IComparable

        Assert.True( eq (left.CompareTo( Natural( b ) )) )
        Assert.True( eq (left.CompareTo( Integer( int16 b ) )) )

        Assert.True( eq (left.CompareTo( b )) )
        Assert.True( eq (left.CompareTo( uint16 b )) )
        Assert.True( eq (left.CompareTo( uint32 b )) )
        Assert.True( eq (left.CompareTo( uint64 b )) )
        Assert.True( eq (left.CompareTo( System.UInt128( 0uL, uint64 b ) )) )
        Assert.True( eq (left.CompareTo( System.Numerics.BigInteger b )) )

        Assert.True( eq (left.CompareTo( int8  b )) )
        Assert.True( eq (left.CompareTo( int16 b )) )
        Assert.True( eq (left.CompareTo( int32 b )) )
        Assert.True( eq (left.CompareTo( int64 b )) )
        Assert.True( eq (left.CompareTo( System.Int128( 0uL, uint64 b ) )) )

    [<Fact>]
    member public this.NegativeEquals () =
        // NOTE: This only goes to 127 so that I know the cast to signed byte will work
        let b = System.Convert.ToSByte( System.Random().Next( -127, 0 ) )
        let left = Integer( b ) :> System.IComparable

        Assert.True( eq (left.CompareTo( Integer( b ) )) )
        Assert.True( eq (left.CompareTo( System.Numerics.BigInteger b )) )

        Assert.True( eq (left.CompareTo( int8  b )) )
        Assert.True( eq (left.CompareTo( int16 b )) )
        Assert.True( eq (left.CompareTo( int32 b )) )
        Assert.True( eq (left.CompareTo( int64 b )) )
        Assert.True( eq (left.CompareTo( System.Int128.CreateChecked( b ) )) )

    [<Fact>]
    member public this.GreaterThan () =
        let left = Integer( System.Convert.ToInt32( System.Random().Next( 128, 256 ) ) ) :> System.IComparable
        let b = System.Convert.ToByte( System.Random().Next( 0, 127 ) )

        Assert.True( gt (left.CompareTo( Natural( b ) )) )
        Assert.True( gt (left.CompareTo( Integer( int16 b ) )) )

        Assert.True( gt (left.CompareTo( b )) )
        Assert.True( gt (left.CompareTo( uint16 b )) )
        Assert.True( gt (left.CompareTo( uint32 b )) )
        Assert.True( gt (left.CompareTo( uint64 b )) )
        Assert.True( gt (left.CompareTo( System.UInt128( 0uL, uint64 b ) )) )
        Assert.True( gt (left.CompareTo( System.Numerics.BigInteger b )) )

        Assert.True( gt (left.CompareTo( int8 b )) )
        Assert.True( gt (left.CompareTo( int16 b )) )
        Assert.True( gt (left.CompareTo( int32 b )) )
        Assert.True( gt (left.CompareTo( int64 b )) )
        Assert.True( gt (left.CompareTo( System.Int128( 0uL, uint64 b ) )) )

    [<Fact>]
    member public this.NegativeGreaterThan () =
        let left = Integer( System.Convert.ToInt32( System.Random().Next( -64, 0 ) ) ) :> System.IComparable
        let b = System.Convert.ToSByte( System.Random().Next( -127, -65 ) )

        Assert.True( gt (left.CompareTo( Integer( b ) )) )

        Assert.True( gt (left.CompareTo( int8  b )) )
        Assert.True( gt (left.CompareTo( int16 b )) )
        Assert.True( gt (left.CompareTo( int32 b )) )
        Assert.True( gt (left.CompareTo( int64 b )) )
        Assert.True( gt (left.CompareTo( System.Int128.CreateChecked( b ) )) )
        Assert.True( gt (left.CompareTo( System.Numerics.BigInteger b )) )

    [<Fact>]
    member public this.LessThan () =
        let left = Integer( System.Convert.ToInt32( System.Random().Next( 0, 63 ) ) ) :> System.IComparable
        let b = System.Convert.ToByte( System.Random().Next( 64, 128 ) )

        Assert.True( lt (left.CompareTo( Natural( b ) )) )
        Assert.True( lt (left.CompareTo( Integer( int16 b ) )) )

        Assert.True( lt (left.CompareTo( b )) )
        Assert.True( lt (left.CompareTo( uint16 b )) )
        Assert.True( lt (left.CompareTo( uint32 b )) )
        Assert.True( lt (left.CompareTo( uint64 b )) )
        Assert.True( lt (left.CompareTo( System.UInt128( 0uL, uint64 b ) )) )
        Assert.True( lt (left.CompareTo( System.Numerics.BigInteger b )) )

        Assert.True( lt (left.CompareTo( int8  b )) )
        Assert.True( lt (left.CompareTo( int16 b )) )
        Assert.True( lt (left.CompareTo( int32 b )) )
        Assert.True( lt (left.CompareTo( int64 b )) )
        Assert.True( lt (left.CompareTo( System.Int128( 0uL, uint64 b ) )) )

    [<Fact>]
    member public this.NegativeLessThan () =
        let left = Integer( System.Convert.ToInt32( System.Random().Next( -127, -65 ) ) ) :> System.IComparable
        let b = System.Convert.ToSByte( System.Random().Next( -64, 0 ) )

        Assert.True( lt (left.CompareTo( Integer( b ) )) )

        Assert.True( lt (left.CompareTo( int8  b )) )
        Assert.True( lt (left.CompareTo( int16 b )) )
        Assert.True( lt (left.CompareTo( int32 b )) )
        Assert.True( lt (left.CompareTo( int64 b )) )
        Assert.True( lt (left.CompareTo( System.Int128.CreateChecked( b ) )) )
        Assert.True( lt (left.CompareTo( System.Numerics.BigInteger b )) )

    [<Theory>]
    [<InlineData( 1F )>]
    [<InlineData( 1.0 )>]
    [<InlineData( "1" )>]
    member public this.Incompatible value =
        let one = Integer.Unit :> System.IComparable
        let exc = Record.Exception(
            fun () ->
                one.CompareTo( value )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.ArgumentException>( exc )
