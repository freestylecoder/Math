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
    
    [<CustomEquality>]
    [<CustomComparison>]
    type public Natural = Natural of uint32 list
        with
            static member Zero
                with get() = Natural([0u])

            static member Unit
                with get() = Natural([1u])

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

                Natural( (shift left rMod) @ (List.init rDiv (fun i -> 0u)) )

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
 
            // Comparison Operators
            static member op_Equality ((Natural left), (Natural right)) : bool =
                let (l,r) = normalize left right
                List.map2 (fun x y -> x = y) l r
                |> List.reduce (fun x y -> x && y)
 
            static member op_GreaterThan ((Natural left), (Natural right)) : bool =
                let rec gt l (r:uint32 list) =
                    match l with
                    | [] -> false
                    | h::t ->
                        if h = r.Head then gt t r.Tail
                        else if h > r.Head then true
                        else false

                match left.Length - right.Length with
                | x when x > 0 -> true
                | x when x < 0 -> false
                | _ -> gt left right

            static member op_LessThan ((Natural left), (Natural right)) : bool =
                let rec lt l (r:uint32 list) =
                    match l with
                    | [] -> false
                    | h::t ->
                        if h = r.Head then lt t r.Tail
                        else if h < r.Head then true
                        else false

                match left.Length - right.Length with
                | x when x < 0 -> true
                | x when x > 0 -> false
                | _ -> lt left right

            static member op_GreaterThanOrEqual ((Natural left), (Natural right)) : bool =
                let rec gte l (r:uint32 list) =
                    match l with
                    | [] -> true
                    | h::t ->
                        if h = r.Head then gte t r.Tail
                        else if h > r.Head then true
                        else false

                match left.Length - right.Length with
                | 0 -> gte left right
                | _ -> false

            static member op_LessThanOrEqual ((Natural left), (Natural right)) : bool =
                let rec lte l (r:uint32 list) =
                    match l with
                    | [] -> true
                    | h::t ->
                        if h = r.Head then lte t r.Tail
                        else if h < r.Head then true
                        else false

                match left.Length - right.Length with
                | 0 -> lte left right
                | _ -> false

            static member op_Inequality (left:Natural, right:Natural) : bool =
                Natural.op_Equality( left, right )
                |> not
 
            // Arithmetic Operators
            // Binary
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

            static member (-) ((Natural left), (Natural right)) : Natural =
                let rightpad (l:uint32 list) n =
                    List.init (n - l.Length) (fun i -> 0u) @ l

                let leftpad l n =
                    l @ List.init (n - l.Length) (fun i -> 0u)

                let (l,r) = normalize left right
                let len = Math.Max( left.Length, right.Length )
                let d = rightpad (List.map2 (fun x y -> x - y) l r) (len+1)
                let o = leftpad (List.map2 (fun  x y -> if y > x then 1u else 0u) l r) (len+1)
                let o2 = leftpad (List.map2 (fun  x y -> if y > x then 1u else 0u) d.Tail o.Tail) (len+1)
                let n = List.map3 (fun x y z -> x - y - z ) d o o2

                Natural( compress n )

            static member (*) (left:Natural, right:Natural) : Natural =
                let rec magic value bit =
                    match value with
                    | x when Natural.Zero = x ->
                        []
                    | _ ->
                        match Natural.Unit &&& value with
                        | Natural([0u]) ->
                            magic (value>>>1) (bit+1)
                        | Natural([1u]) ->
                            (left<<<bit) :: (magic (value>>>1) (bit+1))
                        | _ -> failwith "not possible (bit has value other than 0 or 1)"

                magic right 0
                |> List.sum
            
            static member (/%) (left:Natural, right:Natural) : Natural*Natural =
                let rec op bit =
                    let factor = right <<< bit
                    if factor > left then
                        (Natural.Zero,left)
                    else
                        let (quotient,remainder) = op (bit + 1)

                        if factor > remainder then
                            (quotient,remainder)
                        else
                            (quotient+(Natural.Unit<<<bit),remainder-factor)

                match right with
                | Natural([0u]) -> raise (new DivideByZeroException())
                | Natural([1u]) -> (left,Natural.Zero)
                | r when r = left -> (Natural.Unit,Natural.Zero)
                | _ ->
                    op 0

            static member (/) (left:Natural, right:Natural) : Natural =
                let (q,_) = left /% right
                q

            static member (%) (left:Natural, right:Natural) : Natural =
                let (_,r) = left /% right
                r

            // Unary

            // .NET Object Overrides
            override left.Equals( right ) =
                match right.GetType() with
                | t when t = typeof<Natural> -> Natural.op_Equality(left, right :?> Natural)
                | _ -> false

            override this.GetHashCode() =
                let (Natural n) = this
                let v =
                    List.rev n
                    |> List.head
                v.GetHashCode()

            //override this.ToString() =
            //    raise ( new NotImplementedException( "Need a few more operators defined first" ) )

            // IComparable (for .NET) 
            interface IComparable with
                member left.CompareTo right = 
                    match right.GetType() with
                    | t when t = typeof<Natural> ->
                        match Natural.op_Equality(left, right :?> Natural) with
                        | true -> 0
                        | false ->
                            match Natural.op_GreaterThan(left, right :?> Natural) with
                            | true -> 1
                            | false -> -1
                    | _ -> raise (new ArgumentException())
