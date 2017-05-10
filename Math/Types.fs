namespace Freestylecoding.Math

open System

module Types = 
    type public Natural = Natural of uint32 list

    let (+) (left:Natural) (right:Natural) : Natural =
        let getList n =
            let (Natural l) = n
            l

        let len =
            Math.Max( (getList left).Length, (getList right).Length )

        let l = 
            List.init (len - (getList left).Length) (fun i -> 0u)
            |> List.append
            <| (getList left)
            |> List.rev

        let r = 
            List.init (len - (getList right).Length) (fun i -> 0u)
            |> List.append
            <| (getList right)
            |> List.rev

        let f i x y =
            match i with
            | 0 -> x + y
            | i ->
                match l.[i-1] >= ( System.UInt32.MaxValue - r.[i-1] ) with
                | true -> x + y + 1u
                | false -> x + y

        let s = 
            List.mapi2 f l r
            |> List.rev

        if s.Head = (List.min [ List.last l; List.last r; s.Head ]) then Natural ( 1u :: s )
        else Natural ( s )