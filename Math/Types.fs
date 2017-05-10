namespace Freestylecoding.Math

open System
open System.Collections

module Types = 
    type Natural = uint32 list

    let rec inline (+) (l:Natural) (r:Natural) : Natural =
        let rec add (l:Natural) (r:Natural) : (Natural * bool) =
            ([0u],false)

        let (s,o) =
            if l.Length < r.Length
            then add r l
            else add l r

        if o
        then (s.Head + 1u) :: s.Tail
        else s

    let (^^) l r =
        (l || r) && not (l && r)

    //let rec public (+) (l:Natural) (r:Natural) : Natural =
    //    match l.Length - r.Length with
    //    | n when n < 0 -> r + l
    //    | p when p > 0 -> l + ( false :: r )
    //    | _ ->
    //        match l.IsEmpty with
    //        | true -> []
    //        | false ->
    //            let v = l.Tail + r.Tail
    //            let o =
    //                match l.Length = v.Length with
    //                | true -> true
    //                | false -> false
    //            let s = (not l.Head) ^^ (r.Head ^^ o)

    //            match (r.Head && o) || (l.Head && (r.Head || o )) with
    //            | true -> true :: s :: v
    //            | false -> s :: v

    //let rec public (-) (l:Natural) (r:Natural) : Natural =
    //    []

    //let rec public (*) (l:Natural) (r:Natural) : Natural =
        //match r with
        ////| 0 -> []
        //| _ -> []

type Integer( x : int ) =
    let HighestBit = int ( Math.Floor( Math.Log( float x, 2. ) ) )

    // Astute reader will notice the BitArray is packed in a Little-Endian format
    // This is for two reasons
    // 1) The index of the bit is the power of 2 for the value
    // 2) When we do all the bitwise math, the indexes will match
    let data = new BitArray( HighestBit )

    do for i = 0 to HighestBit do
        data.[i] <- (x &&& (1 <<< i)) > 0

    // NOTE: This is only settable for the private constructor that uses ba
    member val private Data = data with get, set

    new() = Integer( 0 )
    private new( ba:BitArray ) as this = Integer( 0 ) then this.Data <- ba

    static member (+) ( l : Integer, r : Integer ) =
        let s = new BitArray( Math.Max( l.Data.Length, r.Data.Length ) )
        let mutable o = false;

        let (^^) l r =
            (l || r) && not (l && r)

        let getBit (a:Integer) i =
            if i >= a.Data.Length then false
            else a.Data.[i]

        for i in { 0 .. s.Length - 1 } do
            s.[i] <- (not (getBit l i)) ^^ ((getBit r i) ^^ o)
            o <- ((getBit r i) && o) || ((getBit l i) && ((getBit r i) || o ))

        if o then
            s.Length <- s.Length + 1
            s.[s.Length - 1] <- true

        Integer( s )

    override this.Equals( obj ) = this.ToString() = obj.ToString()
    override this.GetHashCode() = this.Data.GetHashCode()

    override this.ToString() =
        let rec pack i =
            if this.Data.Length = i then 0
            else
                let v = (pack (i+1)) <<< 1
                if this.Data.[i] then v ||| 0x1
                else v

        if 33 < this.Data.Length then (pack 0).ToString()
        else raise (new NotImplementedException())