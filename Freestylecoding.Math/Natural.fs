namespace Freestylecoding.Math

open System
open System.Numerics

type public Natural(data:uint32 list) =
    let rec Compress (l:uint32 list) : uint32 list =
        match l with
        | [] -> [0u]
        | 0u :: t -> Compress t
        | _ -> l

    member internal Natural.Data = Compress data

    new() = Natural( [0u] )
    new(data:uint32) = Natural( [data] )
    new(data:uint64) = Natural( [
        Convert.ToUInt32( data >>> 32 );
        Convert.ToUInt32( data &&& 0xFFFF_FFFFUL )
    ] )

    new(data:uint32 seq) = Natural( Seq.toList( data ) )

    with
        static member Zero = Natural( [0u] )
        static member Unit = Natural( [1u] )

        static member op_Implicit( i:uint32 ) : Natural =
            Natural( i )
        static member op_Implicit( l:uint64 ) : Natural =
            Natural( l )

        static member private BitwiseOperation (f:(uint32 -> uint32 -> uint32)) (left:Natural) (right:Natural) : Natural =
            let (l,r) = Helpers.normalize left.Data right.Data
            Natural( List.map2 f l r )

        // Bitwise Operators
        static member (&&&) (left:Natural, right:Natural) : Natural =
            Natural.BitwiseOperation (fun x y -> x &&& y) left right

        static member (|||) (left:Natural, right:Natural) : Natural =
            Natural.BitwiseOperation (fun x y -> x ||| y) left right

        static member (^^^) (left:Natural, right:Natural) : Natural =
            Natural.BitwiseOperation (fun x y -> x ^^^ y) left right

        static member (~~~) (right:Natural) : Natural =
            Natural( List.map (fun x -> ~~~ x) right.Data )

        static member (<<<) (left:Natural, (right:int)) : Natural =
            let bitsToShift = right % 32
            let listElementsToShift = right / 32
            let overflowBits = ~~~(System.UInt32.MaxValue >>> bitsToShift)

            let shiftedList = 0u :: (List.map (fun x -> x <<< bitsToShift) left.Data)
            let overflowList = (List.map (fun x -> (overflowBits &&& x) >>> (32 - bitsToShift) ) left.Data) @ [0u]
            let result = List.map2 (fun x y -> x ||| y) shiftedList overflowList

            Natural( result @ (List.init listElementsToShift (fun i -> 0u)) )

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
            not (left = right)
 
        // Arithmetic Operators
        // Binary
        static let Add (left:Natural) (right:Natural) : Natural = 
            let rec operation (l:uint32 list, r:uint32 list) : uint32 list =
                let rawSums = 0u :: List.map2 (fun x y -> x + y) l r
                let overflows = (List.map2 (fun x y -> if x > ( System.UInt32.MaxValue - y ) then 1u else 0u) l r) @ [0u]
                match overflows with
                | _ when Natural.Zero = Natural( overflows ) -> rawSums
                | _ -> operation ( rawSums, overflows )

            let result = operation ( Helpers.normalize left.Data right.Data )
            Natural( result )

        static member (+) (left:Natural, right:Natural) : Natural =
            Add left right

        static member (-) (left:Natural, right:Natural) : Natural =
            if( left < right ) then raise (new OverflowException())

            let (l,r) = Helpers.normalize left.Data right.Data
            let rawDifferences = 0u :: (List.map2 (fun x y -> x - y) l r)
            let underflows = (List.map2 (fun  x y -> if y > x then 1u else 0u) l r) @ [0u]
            let cascadeUnderflows = (List.map2 (fun  x y -> if y > x then 1u else 0u) rawDifferences.Tail underflows.Tail) @ [0u]
            let result = List.map3 (fun x y z -> x - y - z ) rawDifferences underflows cascadeUnderflows

            Natural( result )

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
            | t when t = typeof<uint32> -> Natural.op_Equality(left, Natural( right :?> uint32 ) )
            | t when t = typeof<uint64> -> Natural.op_Equality(left, Natural( right :?> uint64 ) )
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

        interface IUnsignedNumber<Natural> with
            // IEquatable<Natural>
            member this.Equals(other: Natural): bool = 
                this = other

            // IFormatable
            member this.ToString(format: string, formatProvider: IFormatProvider): string = 
                raise (System.NotImplementedException())

            // IParsable<Natural>
            member this.Parse(s: string, provider: IFormatProvider): Natural = 
                raise (System.NotImplementedException())
            member this.TryParse(s: string, provider: IFormatProvider, result: byref<Natural>): bool = 
                raise (System.NotImplementedException())

            // ISpanFormatable : IFormatable
            member this.TryFormat(destination: Span<char>, charsWritten: byref<int>, format: ReadOnlySpan<char>, provider: IFormatProvider): bool = 
                raise (System.NotImplementedException())

            // ISpanParsable<Natural>
            member this.Parse(s: ReadOnlySpan<char>, provider: IFormatProvider): Natural = 
                raise (System.NotImplementedException())
            member this.TryParse(s: ReadOnlySpan<char>, provider: IFormatProvider, result: byref<Natural>): bool = 
                raise (System.NotImplementedException())

            // IAdditionOperators<Natural,Natural,Natural>
            static member (+)(left:Natural, right:Natural) : Natural = 
                Add left right
            member this.op_CheckedAddition(left: Natural, right: Natural): Natural = 
                Add left right

            // IAdditiveIdentity<Natural,Natural>
            member this.AdditiveIdentity
                with get () = Natural.Zero

            // IDecrementOperators<Natural>
            member this.op_CheckedDecrement(value: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.op_Decrement( value:Natural ) : Natural =
                raise (System.NotImplementedException())
            //member this.(~--)(value: Natural): Natural = 
            //    raise (System.NotImplementedException())

            // IDivisionOperators<Natural,Natural,Natural>
            member this.op_CheckedDivision(left: Natural, right: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.(/)(left:Natural, right:Natural) : Natural = 
                raise (System.NotImplementedException())

            // IEqualityOperators<Natural,Natural,bool>
            member this.(=)(left: Natural, right: Natural): bool = 
                raise (System.NotImplementedException())
            member this.(<>)(left: Natural, right: Natural): bool = 
                raise (System.NotImplementedException())

            // IIncrementOperators<Natural>
            member this.op_CheckedIncrement(value: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.op_Increment(value: Natural): Natural = 
                raise (System.NotImplementedException())
            //member this.(~++)(value: Natural): Natural = 
            //    raise (System.NotImplementedException())

            // IMultiplicativeIdentity<Natural,Natural>
            member this.MultiplicativeIdentity
                with get () = raise (System.NotImplementedException())
                
            // IMultiplyOperators<Natural,Natural,Natural>
            member this.op_CheckedMultiply(left: Natural, right: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.(*)(left:Natural, right:Natural) : Natural = 
                left * right

            // INumberBase<Natural>
            member this.One
                with get () = Natural.Unit
            member this.Radix 
                with get () = 2
            member this.Zero
                with get () = Natural.Zero

            member this.Abs(value: Natural): Natural = 
                Natural( value.Data )
            member this.CreateChecked(value: 'TOther): Natural = 
                raise (System.NotImplementedException())
            member this.CreateSaturating(value: 'TOther): Natural = 
                raise (System.NotImplementedException())
            member this.CreateTruncating(value: 'TOther): Natural = 
                raise (System.NotImplementedException())
            member this.IsCanonical(value:Natural) : bool = true
            member this.IsComplexNumber(value:Natural) : bool = false
            member this.IsEvenInteger(value: Natural): bool = 
                raise (System.NotImplementedException())
            member this.IsFinite(value:Natural) : bool = true
            member this.IsImaginaryNumber(value:Natural) : bool = false
            member this.IsInfinity(value:Natural) : bool = false
            member this.IsInteger(value:Natural) : bool = 
                raise (System.NotImplementedException())
            member this.IsNaN(value:Natural) : bool = false
            member this.IsNegative(value:Natural) : bool = false
            member this.IsNegativeInfinity(value:Natural) : bool = false
            member this.IsNormal(value: Natural): bool = 
                raise (System.NotImplementedException())
            member this.IsOddInteger(value: Natural): bool = 
                raise (System.NotImplementedException())
            member this.IsPositive(value:Natural) : bool = true
            member this.IsPositiveInfinity(value:Natural) : bool = false
            member this.IsRealNumber(value:Natural) : bool = true
            member this.IsSubnormal(value: Natural): bool = 
                raise (System.NotImplementedException())
            member this.IsZero(value:Natural) : bool = value = Natural.Zero
            member this.MaxMagnitude(x: Natural, y: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.MaxMagnitudeNumber(x: Natural, y: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.MinMagnitude(x: Natural, y: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.MinMagnitudeNumber(x: Natural, y: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.Parse(s: ReadOnlySpan<char>, style: Globalization.NumberStyles, provider: IFormatProvider): Natural = 
                raise (System.NotImplementedException())
            member this.Parse(s: string, style: Globalization.NumberStyles, provider: IFormatProvider): Natural = 
                raise (System.NotImplementedException())
            member this.TryConvertFromChecked(value: 'TOther, result: byref<Natural>): bool = 
                raise (System.NotImplementedException())
            member this.TryConvertFromSaturating(value: 'TOther, result: byref<Natural>): bool = 
                raise (System.NotImplementedException())
            member this.TryConvertFromTruncating(value: 'TOther, result: byref<Natural>): bool = 
                raise (System.NotImplementedException())
            member this.TryConvertToChecked(value: Natural, result: byref<'TOther>): bool = 
                raise (System.NotImplementedException())
            member this.TryConvertToSaturating(value: Natural, result: byref<'TOther>): bool = 
                raise (System.NotImplementedException())
            member this.TryConvertToTruncating(value: Natural, result: byref<'TOther>): bool = 
                raise (System.NotImplementedException())
            member this.TryParse(s: ReadOnlySpan<char>, style: Globalization.NumberStyles, provider: IFormatProvider, result: byref<Natural>): bool = 
                raise (System.NotImplementedException())
            member this.TryParse(s: string, style: Globalization.NumberStyles, provider: IFormatProvider, result: byref<Natural>): bool = 
                raise (System.NotImplementedException())

            // ISubtractionOperators<Natural,Natural,Natural>
            member this.op_CheckedSubtraction(left: Natural, right: Natural): Natural = 
                raise (System.NotImplementedException())
            member this.(-)(left:Natural, right:Natural) : Natural = 
                left - right

            // IUnaryNegationOperators<Natural,Natural>
            member this.op_CheckedUnaryNegation( value:Natural ) : Natural = 
                raise ( System.OverflowException() )
            member this.(~-)(value: Natural): Natural = 
                raise ( System.OverflowException() )

            // IUnaryPlusOperators<Natural,Natural>
            member this.(~+)(value:Natural) : Natural = 
                value

        interface IBitwiseOperators<Natural,Natural,Natural> with
            static member (&&&)( left, right ) =
                left &&& right
            static member (|||)( left, right ) =
                left ||| right
            static member op_OnesComplement( left ) =
                ~~~left
            static member (^^^)( left, right ) =
                left ^^^ right

        //interface IShiftOperators<Natural,int,Natural> with
        //    static member (<<<)( left, right ) =
        //        left <<< right
        //    static member (>>>)( left, right ) =
        //        left >>> right
        //    static member op_UnsignedRightShift( left, right ) =
        //        left >>> right

        //interface IEqualityOperators<Natural,Natural,bool> with
        //    static member op_Inequality( left, right ) =
        //        left <> right
        //    static member op_Equality( left, right ) =
        //        left = right

        //interface IComparisonOperators<Natural,Natural,bool> with
        //    static member op_LessThan( left, right ) =
        //        left < right
        //    static member op_LessThanOrEqual( left, right ) =
        //        left <= right
        //    static member op_GreaterThan( left, right ) =
        //        left > right
        //    static member op_GreaterThanOrEqual( left, right ) =
        //        left >= right

        //interface IAdditionOperators<Natural,Natural,Natural> with
        //    static member op_Addition(left, right) =
        //        left + right
        //    static member op_CheckedAddition(left, right) = 
        //        left + right

        //interface IAdditiveIdentity<Natural,Natural> with
        //    static member AdditiveIdentity = Natural.Zero

        //interface ISubtractionOperators<Natural,Natural,Natural> with
        //    static member op_Subtraction(left, right) =
        //        if
        //            left < right
        //        then
        //            Natural( Array.create right.Data.Length UInt32.MaxValue |> Array.toList ) - ( right - left )
        //        else
        //            left - right
        //    static member op_CheckedSubtraction(left, right) = 
        //        left - right

        //interface IDecrementOperators<Natural> with
        //    static member op_Decrement(value:Natural) : Natural =
        //        Natural.op_Subtraction( value, Natural.Unit )
        //    static member op_CheckedDecrement( value: Natural ): Natural = 
        //        ISubtractionOperators<Natural,Natural,Natural>.op_CheckedSubtraction( value, Natural.Unit )

        //interface IUnsignedNumber<Natural> with
        //    // IAdditionOperators<Natural,Natural,Natural>
        //    member this.(+)(left: Natural, right: Natural): Natural = 
        //        let rec operation (l:uint32 list, r:uint32 list) : uint32 list =
        //            let rawSums = 0u :: List.map2 (fun x y -> x + y) l r
        //            let overflows = (List.map2 (fun x y -> if x > ( System.UInt32.MaxValue - y ) then 1u else 0u) l r) @ [0u]
        //            match overflows with
        //            | _ when Natural.Zero = Natural( overflows ) -> rawSums
        //            | _ -> operation ( rawSums, overflows )
        //
        //        let result = operation ( Helpers.normalize left.Data right.Data )
        //        Natural( result )

        //    member this.op_CheckedAddition(left: Natural, right: Natural): Natural = 
        //        left + right

        //    member this.(*)(left: Natural, right: Natural): Natural = 
        //        Natural.op_Multiply( left, right )
        //    member this.(-)(left: Natural, right: Natural): Natural = 
        //        Natural.op_Subtraction( left, right )
        //    member this.(/)(left: Natural, right: Natural): Natural = 
        //        Natural.op_Division( left, right )
        //    member this.(<>)(left: Natural, right: Natural): bool = 
        //        Natural.op_Inequality( left, right )
        //    member this.(=)(left: Natural, right: Natural): bool = 
        //        Natural.op_Equality( left, right )
        //    member this.(~+)(value: Natural): Natural = 
        //        value
        //    member this.(~++)(value: Natural): Natural = 
        //        Natural.op_Addition( value, Natural.Unit )
        //    member this.(~-)(value: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.(~--)(value: Natural): Natural = 
        //        Natural.op_Subtraction( value, Natural.Unit )
        //    member this.Abs(value: Natural): Natural = 
        //        value
        //    member this.get_AdditiveIdentity: Natural = 
        //        Natural.Zero
        //    member this.CreateChecked(value: 'TOther): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.CreateSaturating(value: 'TOther): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.CreateTruncating(value: 'TOther): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.Equals(other: Natural): bool = 
        //        Natural.op_Equality( this, other )
        //    member this.IsCanonical(value: Natural): bool = 
        //        raise (System.NotImplementedException())
        //    member this.IsComplexNumber(value: Natural): bool = 
        //        false
        //    member this.IsEvenInteger(value: Natural): bool = 
        //        raise (System.NotImplementedException())
        //    member this.IsFinite(value: Natural): bool = 
        //        true
        //    member this.IsImaginaryNumber(value: Natural): bool = 
        //        false
        //    member this.IsInfinity(value: Natural): bool = 
        //        false
        //    member this.IsInteger(value: Natural): bool = 
        //        true
        //    member this.IsNaN(value: Natural): bool = 
        //        false
        //    member this.IsNegative(value: Natural): bool = 
        //        false
        //    member this.IsNegativeInfinity(value: Natural): bool = 
        //        false
        //    member this.IsNormal(value: Natural): bool = 
        //        raise (System.NotImplementedException())
        //    member this.IsOddInteger(value: Natural): bool = 
        //        raise (System.NotImplementedException())
        //    member this.IsPositive(value: Natural): bool = 
        //        true
        //    member this.IsPositiveInfinity(value: Natural): bool = 
        //        false
        //    member this.IsRealNumber(value: Natural): bool = 
        //        true
        //    member this.IsSubnormal(value: Natural): bool = 
        //        raise (System.NotImplementedException())
        //    member this.IsZero(value: Natural): bool = 
        //        Natural.op_Equality( value, Natural.Zero )
        //    member this.MaxMagnitude(x: Natural, y: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.MaxMagnitudeNumber(x: Natural, y: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.MinMagnitude(x: Natural, y: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.MinMagnitudeNumber(x: Natural, y: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.get_MultiplicativeIdentity: Natural = 
        //        Natural.Unit
        //    member this.get_One: Natural = 
        //        raise (System.NotImplementedException())
        //    member this.Parse(s: ReadOnlySpan<char>, style: Globalization.NumberStyles, provider: IFormatProvider): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.Parse(s: string, style: Globalization.NumberStyles, provider: IFormatProvider): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.Parse(s: ReadOnlySpan<char>, provider: IFormatProvider): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.Parse(s: string, provider: IFormatProvider): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.get_Radix: int = 
        //        raise (System.NotImplementedException())
        //    member this.ToString(format: string, formatProvider: IFormatProvider): string = 
        //        raise (System.NotImplementedException())
        //    member this.TryConvertFromChecked(value: 'TOther, result: byref<Natural>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryConvertFromSaturating(value: 'TOther, result: byref<Natural>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryConvertFromTruncating(value: 'TOther, result: byref<Natural>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryConvertToChecked(value: Natural, result: byref<'TOther>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryConvertToSaturating(value: Natural, result: byref<'TOther>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryConvertToTruncating(value: Natural, result: byref<'TOther>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryFormat(destination: Span<char>, charsWritten: byref<int>, format: ReadOnlySpan<char>, provider: IFormatProvider): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryParse(s: ReadOnlySpan<char>, style: Globalization.NumberStyles, provider: IFormatProvider, result: byref<Natural>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryParse(s: string, style: Globalization.NumberStyles, provider: IFormatProvider, result: byref<Natural>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryParse(s: ReadOnlySpan<char>, provider: IFormatProvider, result: byref<Natural>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.TryParse(s: string, provider: IFormatProvider, result: byref<Natural>): bool = 
        //        raise (System.NotImplementedException())
        //    member this.get_Zero: Natural = 
        //        raise (System.NotImplementedException())
        //    member this.op_CheckedDivision(left: Natural, right: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.op_CheckedIncrement(value: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.op_CheckedMultiply(left: Natural, right: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.op_CheckedSubtraction(left: Natural, right: Natural): Natural = 
        //        raise (System.NotImplementedException())
        //    member this.op_CheckedUnaryNegation(value: Natural): Natural = 
        //        raise (System.NotImplementedException())