namespace Freestylecoding.Math

open System
    
type public Natural(data:uint32 list) =
    member internal Natural.Data = data

    new(data:uint32) = Natural( [data] )
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
            let bitsToShift = right % 32
            let listElementsToShift = right / 32
            let overflowBits = ~~~(System.UInt32.MaxValue >>> bitsToShift)

            let shiftedList = 0u :: (List.map (fun x -> x <<< bitsToShift) left.Data)
            let overflowList = (List.map (fun x -> (overflowBits &&& x) >>> (32 - bitsToShift) ) left.Data) @ [0u]
            let result = List.map2 (fun x y -> x ||| y) shiftedList overflowList

            Natural( Helpers.compress( result ) @ (List.init listElementsToShift (fun i -> 0u)) )

        static member (>>>) (left:Natural, right:int) : Natural =
            let rec chomp n l =
                match n with
                | x when x > (List.length l) -> [0u]
                | x when x = (List.length l) -> []
                | _ ->
                    (List.head l) :: chomp n (List.tail l)

            let bitsToShift = right % 32
            let listElementsToShift = right / 32
            let underflowBits = ~~~(System.UInt32.MaxValue <<< bitsToShift)

            let trimmedList = 
                match listElementsToShift with
                | 0 -> left.Data
                | _ -> chomp listElementsToShift left.Data

            let shiftedList = (List.map (fun x -> x >>> bitsToShift) trimmedList) @ [0u]
            let underflowList = 0u :: (List.map (fun x -> (underflowBits &&& x) <<< (32 - bitsToShift) ) trimmedList)
            let result =
                List.map2 (fun x y -> x ||| y) shiftedList underflowList
                |> chomp 1
                |> Helpers.compress

            Natural( result )

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
                    else h > r.Head

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
                    else h < r.Head

            match left.Data.Length - right.Data.Length with
            | x when x < 0 -> true
            | x when x > 0 -> false
            | _ -> lt left.Data right.Data

        static member op_GreaterThanOrEqual (left:Natural, right:Natural) : bool =
            (left = right) || (left > right)

        static member op_LessThanOrEqual (left:Natural, right:Natural) : bool =
            (left = right) || (left < right)

        static member op_Inequality (left:Natural, right:Natural) : bool =
            (left = right) |> not
 
        // Arithmetic Operators
        // Binary
        static member (+) (left:Natural, right:Natural) : Natural =
            let rec operation (l:uint32 list, r:uint32 list) : uint32 list =
                let rawSums = 0u :: List.map2 (fun x y -> x + y) l r
                let overflows = (List.map2 (fun x y -> if x > ( System.UInt32.MaxValue - y ) then 1u else 0u) l r) @ [0u]
                match overflows with
                | _ when [0u] = Helpers.compress overflows -> rawSums
                | _ -> operation ( rawSums, overflows )

            let result = operation ( Helpers.normalize left.Data right.Data )
            Natural( Helpers.compress result )

        static member (-) (left:Natural, right:Natural) : Natural =
            if( left < right ) then raise (new OverflowException())

            let (l,r) = Helpers.normalize left.Data right.Data
            let rawDifferences = 0u :: (List.map2 (fun x y -> x - y) l r)
            let underflows = (List.map2 (fun  x y -> if y > x then 1u else 0u) l r) @ [0u]
            let cascadeUnderflows = (List.map2 (fun  x y -> if y > x then 1u else 0u) rawDifferences.Tail underflows.Tail) @ [0u]
            let result = List.map3 (fun x y z -> x - y - z ) rawDifferences underflows cascadeUnderflows

            Natural( Helpers.compress result )

        static member (*) (left:Natural, right:Natural) : Natural =
            let rec magic value bitsToShiftLeft =
                match value with
                | x when Natural.Zero = x ->
                    []
                | _ ->
                    match Natural.Unit &&& value with
                    | z when z = Natural.Zero ->
                        magic (value>>>1) (bitsToShiftLeft+1)
                    | u when u = Natural.Unit ->
                        (left<<<bitsToShiftLeft) :: (magic (value>>>1) (bitsToShiftLeft+1))
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
            |> Array.map (fun c -> Convert.ToUInt32(c) - 48u )
            |> Array.map (fun u -> Natural([u]))
            |> Array.mapi (fun i n -> n * (pow (Natural([10u])) i))
            |> Array.sum
