namespace Freestylecoding.Math

open System

type public Integer(data:uint list, negative:bool) =
    inherit Natural( data )
    member internal Integer.Negative:bool = negative && ( data <> [0u] )

    new(data:uint32 list) = Integer( data, false )
    new(data:uint32 seq) = Integer( Seq.toList( data ) )
    new(data:uint32 seq, negative:bool) = Integer( Seq.toList( data ), negative )
    new(data:Natural) = Integer( data.Data, false )
    new(data:Natural, negative:bool) = Integer( data.Data, negative )
    new(data:int32) = Integer( [Convert.ToUInt32(Math.Abs(data))], -1 = Math.Sign(data) )

    with
        static member Zero = Integer([0u])
        static member Unit = Integer([1u])

        static member op_Implicit(natural:Natural) : Integer =
            Integer( natural )

        // Bitwise Operators
        static member (&&&) (left:Integer, right:Integer) : Integer =
            Integer(
                Natural( left.Data ) &&& Natural( right.Data ),
                left.Negative && right.Negative
            )

        static member (|||) (left:Integer, right:Integer) : Integer =
            Integer(
                Natural( left.Data ) ||| Natural( right.Data ),
                left.Negative || right.Negative
            )

        static member (^^^) (left:Integer, right:Integer) : Integer =
            Integer(
                Natural( left.Data ) ^^^ Natural( right.Data ),
                ( left.Negative || right.Negative ) && not ( left.Negative && right.Negative )
            )

        static member (~~~) (right:Integer) : Integer =
            Integer(
                ~~~ Natural( right.Data ),
                not right.Negative
            )

        static member (<<<) (left:Integer, (right:int)) : Integer =
            Integer(
                Natural( left.Data ) <<< right,
                left.Negative
            )

        static member (>>>) (left:Integer, right:int) : Integer =
            Integer(
                Natural( left.Data ) >>> right,
                left.Negative
            )
 
        // Comparison Operators
        static member op_Equality (left:Integer, right:Integer) : bool =
            left.Negative = right.Negative && Natural( left.Data ) = Natural( right.Data )
 
        static member op_GreaterThan (left:Integer, right:Integer) : bool =
            match (left.Negative,right.Negative) with
            | (false,false) -> Natural( left.Data ) > Natural( right.Data )
            | (false,true) -> true
            | (true,false) -> false
            | (true,true) -> Natural( right.Data ) > Natural( left.Data )

        static member op_LessThan (left:Integer, right:Integer) : bool =
            match (left.Negative,right.Negative) with
            | (false,false) -> Natural( left.Data ) < Natural( right.Data )
            | (false,true) -> false
            | (true,false) -> true
            | (true,true) -> Natural( right.Data ) < Natural( left.Data )

        static member op_GreaterThanOrEqual (left:Integer, right:Integer) : bool =
            left = right || left > right

        static member op_LessThanOrEqual (left:Integer, right:Integer) : bool =
            left = right || left < right

        static member op_Inequality (left:Integer, right:Integer) : bool =
            not ( left = right )

        // Arithmetic Operators
        // Binary
        static member (+) (left:Integer, right:Integer) : Integer =
            let naturalLeft = Natural( left.Data )
            let naturalRight = Natural( right.Data )
            match left.Negative = right.Negative with
            | true ->
                Integer( naturalLeft + naturalRight, left.Negative )
            | false ->
                match naturalLeft < naturalRight with
                | true -> Integer( naturalRight - naturalLeft, right.Negative )
                | false -> Integer( naturalLeft - naturalRight, left.Negative )

        static member (-) (left:Integer, right:Integer) : Integer =
            let naturalLeft = Natural( left.Data )
            let naturalRight = Natural( right.Data )
            match left.Negative = right.Negative with
            | false ->
                Integer( naturalLeft + naturalRight, left.Negative )
            | true ->
                if naturalLeft < naturalRight then
                    Integer( naturalRight - naturalLeft, not left.Negative )
                else
                    Integer( naturalLeft - naturalRight, left.Negative )

        static member (*) (left:Integer, right:Integer) : Integer =
            Integer(
                Natural( left.Data ) * Natural( right.Data ),
                Helpers.Xor left.Negative right.Negative
            )
            
        static member (/%) (left:Integer, right:Integer) : Integer*Integer =
            let (q,r) = Natural(left.Data) /% Natural(right.Data)
            (
                Integer( q, Helpers.Xor left.Negative right.Negative ),
                Integer( r, left.Negative )
            )

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
            | t when t = typeof<Integer> ->
                Integer.op_Equality(left, right :?> Integer)
            | t when t = typeof<Natural> ->
                ( not left.Negative ) && Natural.op_Equality( Natural( left.Data ), right :?> Natural )
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
                | t when t = typeof<Natural> ->
                    match left.Negative with
                    | true -> -1
                    | false ->
                        match Natural.op_Equality( Natural( left.Data ), right :?> Natural) with
                        | true -> 0
                        | false ->
                            match Natural.op_GreaterThan( Natural( left.Data ), right :?> Natural) with
                            | true -> 1
                            | false -> -1
                | _ -> raise (new ArgumentException())

        // Other things we need that require previous operators
        static member Parse (s:string) =
             Integer( Natural.Parse( s.Trim().TrimStart( '-' ) ), s.Trim().StartsWith( "-" ) )
