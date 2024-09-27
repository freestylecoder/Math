
namespace Natural

open Xunit
open Freestylecoding.Math

type public ObjectGetType() =
    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            typeof<Natural>,
            Natural.Unit.GetType()
        )

type public ObjectEquals() =
    [<Theory>]
    [<InlineData(  0u,  0u, true)>]
    [<InlineData(  1u,  0u, false)>]
    [<InlineData(  0u,  1u, false)>]
    [<InlineData(  1u,  1u, true)>]
    member public this.Sanity left right expected =
        Assert.Equal(
            expected,
            Natural( [left] ).Equals( Natural( [right] ) )
        )

    [<Fact>]
    member public this.LargeNaturals () =
        Assert.True(
            Natural( [0xFu; 0x00000101u] ).Equals( Natural( [0xFu; 0x00000101u] ) )
        )

    [<Fact>]
    member public this.HardcodedTypes () =
        Assert.True( Natural.Unit.Equals( byte   1 ) )
        Assert.True( Natural.Unit.Equals( uint16 1 ) )
        Assert.True( Natural.Unit.Equals( uint32 1 ) )
        Assert.True( Natural.Unit.Equals( uint64 1 ) )

        Assert.True( Natural.Unit.Equals( System.UInt128( 0uL, 1uL ) ) )
        Assert.True( Natural.Unit.Equals( System.Numerics.BigInteger 1 ) )

    [<Fact>]
    member public this.IntegralTypes () =
        Assert.True( Natural.Unit.Equals( sbyte 1 ) )
        Assert.True( Natural.Unit.Equals( int16 1 ) )
        Assert.True( Natural.Unit.Equals( int32 1 ) )
        Assert.True( Natural.Unit.Equals( int64 1 ) )

        Assert.True( Natural.Unit.Equals( System.Int128( 0uL, 1uL ) ) )

    [<Fact>]
    member public this.FloatingTypes () =
        Assert.True( Natural.Unit.Equals( float32 1 ) )
        Assert.True( Natural.Unit.Equals( float   1 ) )
        Assert.True( Natural.Unit.Equals( decimal 1 ) )

        Assert.True( Natural.Unit.Equals( System.Numerics.Complex( 1, 0 ) ) )

    [<Fact>]
    member public this.NotNumber () =
        Assert.False( Natural.Unit.Equals( "1" ) )
        Assert.False( Natural.Unit.Equals( true ) )

    [<Fact>]
    member public this.NegativeIntegralTypes () =
        Assert.False( Natural.Unit.Equals( sbyte -1 ) )
        Assert.False( Natural.Unit.Equals( int16 -1 ) )
        Assert.False( Natural.Unit.Equals( int32 -1 ) )
        Assert.False( Natural.Unit.Equals( int64 -1 ) )

        Assert.False( Natural.Unit.Equals( -System.Int128( 0uL, 1uL ) ) )
        Assert.False( Natural.Unit.Equals( -System.Numerics.BigInteger( 1m ) ) )

    [<Fact>]
    member public this.FloatingTypesWithDecimals () =
        Assert.False( Natural.Unit.Equals( float32 1.1 ) )
        Assert.False( Natural.Unit.Equals( float   1.1 ) )
        Assert.False( Natural.Unit.Equals( decimal 1.1 ) )

        Assert.False( Natural.Unit.Equals( System.Numerics.Complex( 1.1, 0 ) ) )

    [<Fact>]
    member public this.Imaginary () =
        // I'm making an assumption that an imaginary number is not an integer
        // This explicitly checks that assumption
        Assert.False( Natural.Unit.Equals( System.Numerics.Complex( 1, 1 ) ) )

type public ObjectGetHashCode() =
    [<Fact>]
    member public this.Sanity () =
        Assert.Equal(
            Natural.Unit.GetHashCode(),
            Natural.Unit.GetHashCode()
        )

        Assert.NotEqual(
            Natural.Zero.GetHashCode(),
            Natural.Unit.GetHashCode()
        )

type public ObjectToString() =
    [<Theory>]
    [<InlineData( 0u, "0" )>]          // Sanity
    [<InlineData( 1u, "1" )>]          // Sanity
    [<InlineData( 123u, "123" )>]      // multiple bits
    [<InlineData( 45678u, "45678" )>]  // rev
    member public this.Sanity actual expected =
        Assert.Equal(
            expected,
            Natural( [actual] ).ToString()
        )

    [<Fact>]
    member public this.Bigger () =
        Assert.Equal(
            "1234567890123456789",
            Natural( [0x112210F4u; 0x7DE98115u] ).ToString()
        )
