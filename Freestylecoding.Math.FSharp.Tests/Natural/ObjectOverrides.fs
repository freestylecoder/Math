
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
    member public this.SupportsUInt32 () =
        Assert.True(
            Natural.Unit.Equals( 1u )
        )

    [<Fact>]
    member public this.SupportsUInt64 () =
        Assert.True(
            Natural.Unit.Equals( 1ul )
        )

    [<Theory>]
    [<InlineData(  1   )>]
    [<InlineData(  1l  )>]
    [<InlineData(  1f  )>]
    [<InlineData(  1.0 )>]
    member public this.OtherTypes (obj:System.Object) =
        Assert.False( Natural.Unit.Equals( obj ) )

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
