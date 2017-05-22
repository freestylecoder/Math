namespace Freestylecoding.Math

open System

module Types = 
    // Stole this beauty from http://stackoverflow.com/questions/109023/how-to-count-the-number-of-set-bits-in-a-32-bit-integer
    let private NumberOfSetBits (i:uint32) : uint32 =
        let x = i - ((i >>> 1) &&& 0x55555555u);
        let y = (x &&& 0x33333333u) + ((x >>> 2) &&& 0x33333333u);
        (((y + (y >>> 4)) &&& 0x0F0F0F0Fu) * 0x01010101u) >>> 24;
    
    let private normalize (left:uint32 list) (right:uint32 list) =
        let rightpad (l:uint32 list) n =
            List.init (n - l.Length) (fun i -> 0u) @ l

        let len = Math.Max( left.Length, right.Length )
        (rightpad left len, rightpad right len)
        
    let rec private compress l =
        match l with
        | [] -> [0u]
        | 0u :: t -> compress t
        | _ -> l

    type public Natural = Natural of uint32 list
        with
            static member Zero
                with get() = Natural([0u])

            // Bitwise Operators
            static member (&&&) ((Natural left), (Natural right)) : Natural =
                let (l,r) = normalize left right
                Natural( List.map2 (fun x y -> x &&& y) l r |> compress )

            static member (|||) ((Natural left), (Natural right)) : Natural =
                let (l,r) = normalize left right
                Natural( List.map2 (fun x y -> x ||| y) l r |> compress )

            static member (^^^) ((Natural left), (Natural right)) : Natural =
                let (l,r) = normalize left right
                Natural( List.map2 (fun x y -> x ^^^ y) l r |> compress )

            static member (~~~) ((Natural right)) : Natural =
                Natural( List.map (fun x -> ~~~ x) right |> compress )

            static member (<<<) ((Natural left), (right:int)) : Natural =
                let msb = 0x80000000u

                let shift1 (l:uint32 list) =
                    let s = 0u :: (List.map (fun x -> x <<< 1) l)
                    let o = (List.map (fun x -> if (msb &&& x) > 0u then 1u else 0u) l) @ [0u]
                    let n = List.map2 (fun x y -> x ||| y) s o
                    if 0u = n.Head then n.Tail else n

                let rec shift (l:uint32 list) n =
                    match n with
                    | 0 -> l
                    | x -> shift1 (shift l (n-1))

                let rDiv = right / 32
                let rMod = right % 32

                let l = left @ (List.init rDiv (fun i -> 0u))
                Natural( shift l rMod )

            static member (>>>) ((Natural left), (right:int)) : Natural =
                let msb = 0x80000000u

                let rec chomp l n =
                    match l with
                    | [] -> [0u]
                    | _ ->
                        match n with
                        | 0 -> l
                        | x -> chomp (List.rev l |> List.tail |> List.rev) (n-1)

                let shift1 (l:uint32 list) =
                    let s = (List.map (fun x -> x >>> 1) l) @ [0u]
                    let o = 0u :: (List.map (fun x -> if (1u &&& x) > 0u then msb else 0u) l)
                    let n = List.map2 (fun x y -> x ||| y) s o
                    chomp (if 0u = n.Head then n.Tail else n) 1

                let rec shift (l:uint32 list) n =
                    match n with
                    | 0 -> l
                    | x -> shift1 (shift l (n-1))

                let rDiv = right / 32
                let rMod = right % 32

                let l = chomp left rDiv
                Natural( shift l rMod )
 
            // Boolean Operators
 
            // Arithmetic Operators
            static member (+) ((Natural left), (Natural right)) : Natural =
                let rightpad (l:uint32 list) n =
                    List.init (n - l.Length) (fun i -> 0u) @ l

                let leftpad l n =
                    l @ List.init (n - l.Length) (fun i -> 0u)

                let (l,r) = normalize left right
                let len = Math.Max( left.Length, right.Length )
                let s = rightpad (List.map2 (fun x y -> x + y) l r) (len+1)
                let o = leftpad (List.map2 (fun  x y -> if x >= ( System.UInt32.MaxValue - y ) then 1u else 0u) l r) (len+1)
                let n = List.map2 (fun x y -> x + y ) s o

                Natural( compress n )

            static member (*) ((Natural left), (Natural right)) : Natural =
                let leftbitcount = 
                    List.map NumberOfSetBits left
                    |> List.sum

                let rightbitcount = 
                    List.map NumberOfSetBits right
                    |> List.sum

                let l = Natural(if leftbitcount > rightbitcount then left else right)
                let r = (if leftbitcount > rightbitcount then right else left) |> List.rev

                let rec f (i:int) (b:int) (x:uint32) : Natural list =
                    match x >>> b with
                    | 0u -> []
                    | n ->
                        match n &&& 1u with
                        | 0u -> f i (b+1) x
                        | 1u -> (l <<< ((i * 32) + b)) :: (f i (b+1) x)
                        | _ -> failwith "not possible"

                List.mapi (fun i x -> f i 0 x) r
                |> List.concat
                |> List.sum
