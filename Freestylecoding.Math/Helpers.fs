namespace Freestylecoding.Math

open System

type internal Helpers = 
    // Stole this beauty from http://stackoverflow.com/questions/109023/how-to-count-the-number-of-set-bits-in-a-32-bit-integer
    static member internal NumberOfSetBits (i:uint32) : uint32 =
        let x = i - ((i >>> 1) &&& 0x55555555u);
        let y = (x &&& 0x33333333u) + ((x >>> 2) &&& 0x33333333u);
        (((y + (y >>> 4)) &&& 0x0F0F0F0Fu) * 0x01010101u) >>> 24;
    
    static member normalize (left:uint32 list) (right:uint32 list) =
        let rightpad (l:uint32 list) n =
            List.init (n - l.Length) (fun i -> 0u) @ l

        let len = Math.Max( left.Length, right.Length )
        (rightpad left len, rightpad right len)
        
    //static member rec internal compress l =
    static member internal compress l =
        match l with
        | [] -> [0u]
        | 0u :: t -> Helpers.compress t
        | _ -> l

    static member internal Xor (l:bool) (r:bool) : bool =
        ( l || r ) && not ( l && r )

