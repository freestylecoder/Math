namespace Freestylecoding.Math

open System

type public Integer(data:uint list, negative:bool) =
    inherit Natural( data )
    member private Integer.Negative:bool = negative && ( data <> [0u] )

    new(data:uint32 list) = Integer( data, false )
    new(data:uint32 seq) = Integer( Seq.toList( data ) )
    new(data:uint32 seq, negative:bool) = Integer( Seq.toList( data ), negative )
    new(data:Natural) = Integer( data.Data, false )
    new(data:Natural, negative:bool) = Integer( data.Data, negative )

    with
        static member Zero = Integer([0u])
        static member Unit = Integer([1u])

        // Bitwise Operators
        static member (&&&) (left:Integer, right:Integer) : Integer =
            let (l,r) = Helpers.normalize left.Data right.Data
            Integer( List.map2 (fun x y -> x &&& y) l r |> Helpers.compress, left.Negative && right.Negative )

        static member (|||) (left:Integer, right:Integer) : Integer =
            let (l,r) = Helpers.normalize left.Data right.Data
            Integer( List.map2 (fun x y -> x ||| y) l r |> Helpers.compress, left.Negative || right.Negative )

        static member (^^^) (left:Integer, right:Integer) : Integer =
            let (l,r) = Helpers.normalize left.Data right.Data
            Integer( List.map2 (fun x y -> x ^^^ y) l r |> Helpers.compress, ( left.Negative || right.Negative ) && not ( left.Negative && right.Negative ) )

        static member (~~~) (right:Integer) : Integer =
            Integer( List.map (fun x -> ~~~ x) right.Data |> Helpers.compress, not right.Negative )

        static member (<<<) (left:Integer, (right:int)) : Integer =
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

            Integer( (shift left.Data rMod) @ (List.init rDiv (fun i -> 0u)), left.Negative )

        static member (>>>) (left:Integer, right:int) : Integer =
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
            Integer( shift l rMod, left.Negative )
 
        // Comparison Operators
        static member op_Equality (left:Integer, right:Integer) : bool =
            let (l,r) = Helpers.normalize left.Data right.Data
            ( left.Negative = right.Negative ) &&
                List.map2 (fun x y -> x = y) l r
                |> List.reduce (fun x y -> x && y)
 
        static member op_GreaterThan (left:Integer, right:Integer) : bool =
            match (left.Negative,right.Negative) with
            | (false,false) -> Natural.op_GreaterThan( Natural( left.Data ), Natural( right.Data ) )
            | (false,true) -> true
            | (true,false) -> false
            | (true,true) -> Natural.op_GreaterThan( Natural( right.Data ), Natural( left.Data ) )

        static member op_LessThan (left:Integer, right:Integer) : bool =
            match (left.Negative,right.Negative) with
            | (false,false) -> Natural.op_LessThan( Natural( left.Data ), Natural( right.Data ) )
            | (false,true) -> false
            | (true,false) -> true
            | (true,true) -> Natural.op_LessThan( Natural( right.Data ), Natural( left.Data ) )

        static member op_GreaterThanOrEqual (left:Integer, right:Integer) : bool =
            match (left.Negative,right.Negative) with
            | (false,false) -> Natural.op_GreaterThanOrEqual( Natural( left.Data ), Natural( right.Data ) )
            | (false,true) -> true
            | (true,false) -> false
            | (true,true) -> Natural.op_GreaterThanOrEqual( Natural( right.Data ), Natural( left.Data ) )

        static member op_LessThanOrEqual (left:Integer, right:Integer) : bool =
            match (left.Negative,right.Negative) with
            | (false,false) -> Natural.op_LessThanOrEqual( Natural( left.Data ), Natural( right.Data ) )
            | (false,true) -> false
            | (true,false) -> true
            | (true,true) -> Natural.op_LessThanOrEqual( Natural( right.Data ), Natural( left.Data ) )

        static member op_Inequality (left:Integer, right:Integer) : bool =
            Integer.op_Equality( left, right )
            |> not

        // Arithmetic Operators
        // Binary
        static member (+) (left:Integer, right:Integer) : Integer =
            match (left.Negative,right.Negative) with
            | (false,false) -> Integer( Natural( left.Data ) + Natural( right.Data ) )
            | (true,true) -> Integer( Natural( right.Data ) + Natural( left.Data ), true )
            | _ ->
                if Natural( left.Data ) < Natural( right.Data ) then
                    Integer( Natural( right.Data ) - Natural( left.Data ), right.Negative )
                else
                    Integer( Natural( left.Data ) - Natural( right.Data ), left.Negative )

        static member (-) (left:Integer, right:Integer) : Integer =
            match (left.Negative,right.Negative) with
            | (false,true) -> Integer( Natural( left.Data ) + Natural( right.Data ) )
            | (true,false) -> Integer( Natural( left.Data ) + Natural( right.Data ), true )
            | (false,false) ->
                if Natural( left.Data ) < Natural( right.Data ) then
                    Integer( Natural( right.Data ) - Natural( left.Data ), true )
                else
                    Integer( Natural( left.Data ) - Natural( right.Data ), false )
            | (true,true) ->
                if Natural( left.Data ) < Natural( right.Data ) then
                    Integer( Natural( right.Data ) - Natural( left.Data ), false )
                else
                    Integer( Natural( left.Data ) - Natural( right.Data ), true )

        static member (*) (left:Integer, right:Integer) : Integer =
            if( left.Negative = right.Negative ) then
                Integer( Natural( left.Data ) * Natural( right.Data ) )
            else
                Integer( Natural( left.Data ) * Natural( right.Data ), true )
            
        static member (/%) (left:Integer, right:Integer) : Integer*Integer =
            let (q,r) = Natural(left.Data) /% Natural(right.Data)
            (Integer( q, Helpers.Xor left.Negative right.Negative ), Integer( r, left.Negative ) )

        static member (/) (left:Integer, right:Integer) : Integer =
            let (q,_) = left /% right
            q

        static member (%) (left:Integer, right:Integer) : Integer =
            let (_,r) = left /% right
            r

        // Unary
        static member (~-) (input:Integer) : Integer =
            new Integer( input.Data, not input.Negative )

        // .NET Object Overrides
        override left.Equals( right ) =
            match right.GetType() with
            | t when t = typeof<Integer> -> Integer.op_Equality(left, right :?> Integer)
            | _ -> false

        override this.GetHashCode() =
            let n:Integer = this
            let v =
                List.rev n.Data
                |> List.head
            v.GetHashCode()

        override this.ToString() =
            let s = Natural( this.Data ).ToString()
            match this.Negative with
            | true -> "-" + s
            | false -> s

        // IComparable (for .NET) 
        interface IComparable with
            member left.CompareTo right = 
                match right.GetType() with
                | t when t = typeof<Integer> ->
                    match Integer.op_Equality(left, right :?> Integer) with
                    | true -> 0
                    | false ->
                        match Integer.op_GreaterThan(left, right :?> Integer) with
                        | true -> 1
                        | false -> -1
                | _ -> raise (new ArgumentException())

        // Other things we need that require previous operators
        static member Parse (s:string) =
            Integer( Natural.Parse( s.TrimStart( '-' ) ), s.StartsWith( "-" ) )
