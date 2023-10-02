namespace Natural

open Xunit
open Freestylecoding.Math

type public Comparable() =
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
            test (Natural( [left] ).CompareTo( Natural( [right] )) )
        )

    [<Fact>]
    member public this.Equals () =
        let ui = System.Convert.ToUInt32( System.Random().Next() )
        let left = Natural( ui )
        let right = Natural( ui )
        Assert.True( eq (left.CompareTo( right )) )

    [<Fact>]
    member public this.GreaterThan () =
        let left = Natural( System.Convert.ToUInt32( System.Random().Next( 1000, 2000 ) ) )
        let right = Natural( System.Convert.ToUInt32( System.Random().Next( 1000 ) ) )
        Assert.True( gt (left.CompareTo( right )) )

    [<Fact>]
    member public this.LessThan () =
        let left = Natural( System.Convert.ToUInt32( System.Random().Next( 1000 ) ) )
        let right = Natural( System.Convert.ToUInt32( System.Random().Next( 1000, 2000 ) ) )
        Assert.True( lt (left.CompareTo( right )) )

    [<Fact>]
    member public this.EqualsUInt32 () =
        let ui = System.Convert.ToUInt32( System.Random().Next() )
        let left = Natural( ui )
        let right = ui
        Assert.True( eq (left.CompareTo( right )) )

    [<Fact>]
    member public this.GreaterThanUInt32 () =
        let left = Natural( System.Convert.ToUInt32( System.Random().Next( 1000, 2000 ) ) )
        let right = System.Convert.ToUInt32( System.Random().Next( 1000 ) )
        Assert.True( gt (left.CompareTo( right )) )

    [<Fact>]
    member public this.LessThanUInt32 () =
        let left = Natural( System.Convert.ToUInt32( System.Random().Next( 1000 ) ) )
        let right = System.Convert.ToUInt32( System.Random().Next( 1000, 2000 ) )
        Assert.True( lt (left.CompareTo( right )) )

    [<Fact>]
    member public this.EqualsUInt64 () =
        let ul = System.Convert.ToUInt64( System.Random().Next() )
        let left = Natural( ul )
        let right = ul
        Assert.True( eq (left.CompareTo( right )) )

    [<Fact>]
    member public this.GreaterThanUInt64 () =
        let left = Natural( System.Convert.ToUInt64( System.Random().Next( 1000, 2000 ) ) )
        let right = System.Convert.ToUInt64( System.Random().Next( 1000 ) )
        Assert.True( gt (left.CompareTo( right )) )

    [<Fact>]
    member public this.LessThanUInt64 () =
        let left = Natural( System.Convert.ToUInt64( System.Random().Next( 1000 ) ) )
        let right = System.Convert.ToUInt64( System.Random().Next( 1000, 2000 ) )
        Assert.True( lt (left.CompareTo( right )) )

    [<Theory>]
    [<InlineData( 1  )>]
    [<InlineData( 1L )>]
    [<InlineData( 1F )>]
    [<InlineData( 1.0 )>]
    member public this.Incompatible value =
        let exc = Record.Exception(
            fun () ->
                Natural.Unit.CompareTo( value )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.ArgumentException>( exc )
