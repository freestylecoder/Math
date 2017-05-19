namespace Freestylecoding.Math

open System

module Types = 
    type public Natural = Natural of uint32 list

    let inline private value (Natural n) = n

    // Stole this beauty from http://stackoverflow.com/questions/109023/how-to-count-the-number-of-set-bits-in-a-32-bit-integer
    let private NumberOfSetBits (i:uint32) : uint32 =
        let x = i - ((i >>> 1) &&& 0x55555555u);
        let y = (x &&& 0x33333333u) + ((x >>> 2) &&& 0x33333333u);
        (((y + (y >>> 4)) &&& 0x0F0F0F0Fu) * 0x01010101u) >>> 24;

    let (+) (Natural left) (Natural right) : Natural =
        let rightpad (l:uint32 list) n =
            List.init (n - l.Length) (fun i -> 0u) @ l

        let leftpad l n =
            l @ List.init (n - l.Length) (fun i -> 0u)

        let len = Math.Max( left.Length, right.Length )
        let l = rightpad left len
        let r = rightpad right len
        let s = rightpad (List.map2 (fun x y -> x + y) l r) (len+1)
        let o = leftpad (List.map2 (fun  x y -> if x >= ( System.UInt32.MaxValue - y ) then 1u else 0u) l r) (len+1)
        let n = List.map2 (fun x y -> x + y ) s o

        Natural( if 0u = n.Head then n.Tail else n )
        
    let (<<<) (Natural left) (right:uint32) : Natural =
        let msb = 0x80000000u

        let shift1 (l:uint32 list) =
            let s = 0u :: (List.map (fun x -> x <<< 1) l)
            let o = (List.map (fun x -> if (msb &&& x) > 0u then 1u else 0u) l) @ [0u]
            let n = List.map2 (fun x y -> x ||| y) s o
            if 0u = n.Head then n.Tail else n

        let rec shift (l:uint32 list) n =
            match n with
            | 0u -> l
            | x -> shift1 (shift l (n-1u))

        let rDiv = right / 32u
        let rMod = right % 32u

        let l = left @ (List.init (int(rDiv)) (fun i -> 0u))
        Natural( shift l rMod )

    let (>>>) (Natural left) (right:uint32) : Natural =
        let msb = 0x80000000u

        let rec chomp l n =
            match l with
            | [] -> [0u]
            | _ ->
                match n with
                | 0u -> l
                | x -> chomp (List.rev l |> List.tail |> List.rev) (n-1u)

        let shift1 (l:uint32 list) =
            let s = (List.map (fun x -> x >>> 1) l) @ [0u]
            let o = 0u :: (List.map (fun x -> if (1u &&& x) > 0u then msb else 0u) l)
            let n = List.map2 (fun x y -> x ||| y) s o
            chomp (if 0u = n.Head then n.Tail else n) 1u

        let rec shift (l:uint32 list) n =
            match n with
            | 0u -> l
            | x -> shift1 (shift l (n-1u))

        let rDiv = right / 32u
        let rMod = right % 32u

        let l = chomp left rDiv
        Natural( shift l rMod )

    let (*) (Natural left) (Natural right) : Natural =
        let leftbitcount = 
            List.map NumberOfSetBits left
            |> List.sum

        let rightbitcount = 
            List.map NumberOfSetBits right
            |> List.sum

        let l = if leftbitcount > rightbitcount then left else right
        let r = if leftbitcount > rightbitcount then right else left

        Natural([])