namespace Freestylecoding.Math

open System
    
type public Natural(data:uint32 list) =
    member internal Natural.Data = data

    new(data:uint32 seq) = Natural( Seq.toList( data ) )

    with
        static member Zero = Natural([0u])
        static member Unit = Natural([1u])
            
        // Bitwise Operators
        static member (&&&) (left:Natural, right:Natural) : Natural =
            let (l,r) = Helpers.normalize left.Data right.Data
            Natural( List.map2 (fun x y -> x &&& y) l r |> Helpers.compress )

        static member (|||) (left:Natural, right:Natural) : Natural =
            let (l,r) = Helpers.normalize left.Data right.Data
            Natural( List.map2 (fun x y -> x ||| y) l r |> Helpers.compress )

        static member (^^^) (left:Natural, right:Natural) : Natural =
            let (l,r) = Helpers.normalize left.Data right.Data
            Natural( List.map2 (fun x y -> x ^^^ y) l r |> Helpers.compress )

        static member (~~~) (right:Natural) : Natural =
            Natural( List.map (fun x -> ~~~ x) right.Data |> Helpers.compress )

        static member (<<<) (left:Natural, (right:int)) : Natural =
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

            Natural( (shift left.Data rMod) @ (List.init rDiv (fun i -> 0u)) )

        static member (>>>) (left:Natural, right:int) : Natural =
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

            let l = chomp left.Data rDiv
            Natural( shift l rMod )
 
        // Comparison Operators
        static member op_Equality (left:Natural, right:Natural) : bool =
            let (l,r) = Helpers.normalize left.Data right.Data
            List.map2 (fun x y -> x = y) l r
            |> List.reduce (fun x y -> x && y)
 
        static member op_GreaterThan (left:Natural, right:Natural) : bool =
            let rec gt l (r:uint32 list) =
                match l with
                | [] -> false
                | h::t ->
                    if h = r.Head then gt t r.Tail
                    else if h > r.Head then true
                    else false

            match left.Data.Length - right.Data.Length with
            | x when x > 0 -> true
            | x when x < 0 -> false
            | _ -> gt left.Data right.Data

        static member op_LessThan (left:Natural, right:Natural) : bool =
            let rec lt l (r:uint32 list) =
                match l with
                | [] -> false
                | h::t ->
                    if h = r.Head then lt t r.Tail
                    else if h < r.Head then true
                    else false

            match left.Data.Length - right.Data.Length with
            | x when x < 0 -> true
            | x when x > 0 -> false
            | _ -> lt left.Data right.Data

        static member op_GreaterThanOrEqual (left:Natural, right:Natural) : bool =
            let rec gte l (r:uint32 list) =
                match l with
                | [] -> true
                | h::t ->
                    if h = r.Head then gte t r.Tail
                    else if h > r.Head then true
                    else false

            match left.Data.Length - right.Data.Length with
            | x when x > 0 -> true
            | x when x < 0 -> false
            | 0 -> gte left.Data right.Data

        static member op_LessThanOrEqual (left:Natural, right:Natural) : bool =
            let rec lte l (r:uint32 list) =
                match l with
                | [] -> true
                | h::t ->
                    if h = r.Head then lte t r.Tail
                    else if h < r.Head then true
                    else false

            match left.Data.Length - right.Data.Length with
            | x when x > 0 -> false
            | x when x < 0 -> true
            | 0 -> lte left.Data right.Data

        static member op_Inequality (left:Natural, right:Natural) : bool =
            Natural.op_Equality( left, right )
            |> not
 
        // Arithmetic Operators
        // Binary
        static member (+) (left:Natural, right:Natural) : Natural =
            let rightpad (l:uint32 list) n =
                List.init (n - l.Length) (fun i -> 0u) @ l

            let leftpad l n =
                l @ List.init (n - l.Length) (fun i -> 0u)

            let (l,r) = Helpers.normalize left.Data right.Data
            let len = Math.Max( left.Data.Length, right.Data.Length )
            let s = rightpad (List.map2 (fun x y -> x + y) l r) (len+1)
            let o = leftpad (List.map2 (fun  x y -> if x >= ( System.UInt32.MaxValue - y ) then 1u else 0u) l r) (len+1)
            let n = List.map2 (fun x y -> x + y ) s o

            Natural( Helpers.compress n )

        static member (-) (left:Natural, right:Natural) : Natural =
            let rightpad (l:uint32 list) n =
                List.init (n - l.Length) (fun i -> 0u) @ l

            let leftpad l n =
                l @ List.init (n - l.Length) (fun i -> 0u)

            let (l,r) = Helpers.normalize left.Data right.Data
            let len = Math.Max( left.Data.Length, right.Data.Length )
            let d = rightpad (List.map2 (fun x y -> x - y) l r) (len+1)
            let o = leftpad (List.map2 (fun  x y -> if y > x then 1u else 0u) l r) (len+1)
            let o2 = leftpad (List.map2 (fun  x y -> if y > x then 1u else 0u) d.Tail o.Tail) (len+1)
            let n = List.map3 (fun x y z -> x - y - z ) d o o2

            Natural( Helpers.compress n )

        static member (*) (left:Natural, right:Natural) : Natural =
            let rec magic value bit =
                match value with
                | x when Natural.Zero = x ->
                    []
                | _ ->
                    match Natural.Unit &&& value with
                    | z when z = Natural.Zero ->
                        magic (value>>>1) (bit+1)
                    | u when u = Natural.Unit ->
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
            | z when z = Natural.Zero -> raise (new DivideByZeroException())
            | u when u = Natural.Unit -> (left,Natural.Zero)
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
            let n:Natural = this
            let v =
                List.rev n.Data
                |> List.head
            v.GetHashCode()

        override this.ToString() =
            let rec f n : char list =
                match n with
                | z when z = Natural.Zero -> []
                | _ ->
                    let (q,r) = n /% Natural([10u])
                    Convert.ToChar(r.Data.Head + 48u) :: (f q)

            if Natural.Zero = this then
                "0"
            else
                String.Concat(
                    f this
                    |> List.rev
                    |> List.toArray
                )

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

        // Other things we need that require previous operators
        static member Parse (s:string) =
            // Yes, I know this is super inefficient
            // That's why it's currently local to this function and not part of the type
            let rec pow n e =
                match e with
                | 0 -> Natural.Unit
                | _ -> n * (pow n (e-1))

            s.ToCharArray()
            |> Array.rev
            |> Array.map (fun c -> Convert.ToUInt32(c) - 0x30u )
            |> Array.map (fun u -> Natural([u]))
            |> Array.mapi (fun i n -> n * (pow (Natural([10u])) i))
            |> Array.sum
