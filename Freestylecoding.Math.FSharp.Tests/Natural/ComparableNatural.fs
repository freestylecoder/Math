namespace Natural

open Xunit
open Freestylecoding.Math

type public ComparableNatural() =
    let eq actual = actual = 0
    let gt actual = actual > 0
    let lt actual = actual < 0

    [<Theory>]
    [<InlineData( 0u, 0u,  0 )>]
    [<InlineData( 1u, 1u,  0 )>]
    [<InlineData( 1u, 0u,  1 )>]
    [<InlineData( 0u, 1u, -1 )>]
    member public this.Sanity left right expected =
        let test =
            match expected with
            | 0 -> eq
            | p when p > 0 -> gt
            | n when n < 0 -> lt
            | _ -> raise (System.Exception( "Not Possible" ))

        Assert.True(
            test ((Natural( [left] ) :> System.IComparable<Natural>).CompareTo( Natural( [right] )) )
        )

    [<Fact>]
    member public this.Equals () =
        let ui = System.Convert.ToUInt32( System.Random().Next() )
        let left = Natural( ui ) :> System.IComparable<Natural>
        let right = Natural( ui )
        Assert.True( eq (left.CompareTo( right )) )

    [<Fact>]
    member public this.GreaterThan () =
        let left = Natural( System.Convert.ToUInt32( System.Random().Next( 1000, 2000 ) ) ) :> System.IComparable<Natural>
        let right = Natural( System.Convert.ToUInt32( System.Random().Next( 1000 ) ) )
        Assert.True( gt (left.CompareTo( right )) )

    [<Fact>]
    member public this.LessThan () =
        let left = Natural( System.Convert.ToUInt32( System.Random().Next( 1000 ) ) ) :> System.IComparable<Natural>
        let right = Natural( System.Convert.ToUInt32( System.Random().Next( 1000, 2000 ) ) )
        Assert.True( lt (left.CompareTo( right )) )
