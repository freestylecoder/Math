namespace Freestylecoding.Math

open System
open System.Numerics
open System.Globalization

type public Natural(data:uint32 list) =
    static let rec _compress (l:uint32 list) : uint32 list =
        match l with
        | [] -> [0u]
        | 0u :: t -> _compress t
        | _ -> l

    static let _bitwiseOperation (f:(uint32 -> uint32 -> uint32)) (left:Natural) (right:Natural) : Natural =
        let (l,r) = Helpers.normalize left.Data right.Data
        Natural( List.map2 f l r )

    // NOTE: All the base operators are declared here
    // This allows us to have all the externally visible operators, interfaces, etc
    //   reference the same code for optimising/debugging purposes
    static let _bitwiseAnd (left:Natural) (right:Natural) : Natural =
        _bitwiseOperation (fun x y -> x &&& y) left right

    static let _bitwiseOr (left:Natural) (right:Natural) : Natural =
        _bitwiseOperation (fun x y -> x ||| y) left right

    static let _bitwiseXor (left:Natural) (right:Natural) : Natural =
        _bitwiseOperation (fun x y -> x ^^^ y) left right

    static let _bitwiseNot (operand:Natural) : Natural =
        Natural( List.map (fun x -> ~~~ x) operand.Data )

    static let _leftShift (totalBitsToShift:int32) (operand:Natural) : Natural =
        let bitsToShift = totalBitsToShift % 32
        let listElementsToShift = totalBitsToShift / 32
        let overflowBits = ~~~(System.UInt32.MaxValue >>> bitsToShift)

        let shiftedList = 0u :: (List.map (fun x -> x <<< bitsToShift) operand.Data)
        let overflowList = (List.map (fun x -> (overflowBits &&& x) >>> (32 - bitsToShift) ) operand.Data) @ [0u]
        let result = List.map2 (fun x y -> x ||| y) shiftedList overflowList

        Natural( result @ (List.init listElementsToShift (fun i -> 0u)) )

    static let _rightShift (totalBitsToShift:int32) (operand:Natural) : Natural =
        let rec chomp n l =
            match n with
            | x when x > (List.length l) -> [0u]
            | x when x = (List.length l) -> []
            | _ ->
                (List.head l) :: chomp n (List.tail l)

        let bitsToShift = totalBitsToShift % 32
        let listElementsToShift = totalBitsToShift / 32
        let underflowBits = ~~~(System.UInt32.MaxValue <<< bitsToShift)

        let trimmedList = 
            match listElementsToShift with
            | 0 -> operand.Data
            | _ -> chomp listElementsToShift operand.Data

        let shiftedList = (List.map (fun x -> x >>> bitsToShift) trimmedList) @ [0u]
        let underflowList = 0u :: (List.map (fun x -> (underflowBits &&& x) <<< (32 - bitsToShift) ) trimmedList)
        let result =
            List.map2 (fun x y -> x ||| y) shiftedList underflowList
            |> chomp 1

        Natural( result )

    static let _equality (left:Natural) (right:Natural) : bool =
        let (l,r) = Helpers.normalize left.Data right.Data
        List.map2 (fun x y -> x = y) l r
        |> List.reduce (fun x y -> x && y)

    static let _greaterThan (left:Natural) (right:Natural) : bool =
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

    static let _lessThan (left:Natural) (right:Natural) : bool =
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

    static let _add (left:Natural) (right:Natural) : Natural = 
        let rec operation (l:uint32 list, r:uint32 list) : uint32 list =
            let rawSums = 0u :: List.map2 (fun x y -> x + y) l r
            let overflows = (List.map2 (fun x y -> if x > ( System.UInt32.MaxValue - y ) then 1u else 0u) l r) @ [0u]
            match overflows with
            | _ when Natural.Zero = Natural( overflows ) -> rawSums
            | _ -> operation ( rawSums, overflows )

        let result = operation ( Helpers.normalize left.Data right.Data )
        Natural( result )

    static let _subtract (left:Natural) (right:Natural) : Natural =
        if( left < right ) then raise (new OverflowException())

        let (l,r) = Helpers.normalize left.Data right.Data
        let rawDifferences = 0u :: (List.map2 (fun x y -> x - y) l r)
        let underflows = (List.map2 (fun  x y -> if y > x then 1u else 0u) l r) @ [0u]
        let cascadeUnderflows = (List.map2 (fun  x y -> if y > x then 1u else 0u) rawDifferences.Tail underflows.Tail) @ [0u]
        let result = List.map3 (fun x y z -> x - y - z ) rawDifferences underflows cascadeUnderflows

        Natural( result )

    static let _multiply (left:Natural) (right:Natural) : Natural =
        let rec magic value bitsToShiftLeft =
            match value with
            | x when Natural.Zero = x ->
                []
            | _ ->
                match (_bitwiseAnd Natural.Unit value) with
                | z when (_equality z Natural.Zero) ->
                    magic (_rightShift 1 value) (bitsToShiftLeft+1)
                | u when u = Natural.Unit ->
                    (_leftShift bitsToShiftLeft left) :: (magic (_rightShift 1 value) (bitsToShiftLeft+1))
                | _ -> failwith "not possible (bit has value other than 0 or 1)"

        magic right 0
        |> List.sum
            
    static let _divideModulo (left:Natural) (right:Natural) : Natural*Natural =
        let rec op bit =
            let factor = _leftShift bit right
            if _greaterThan factor left then
                (Natural.Zero,left)
            else
                let (quotient,remainder) = op (bit + 1)

                if _greaterThan factor remainder then
                    (quotient,remainder)
                else
                    ((_add quotient (_leftShift bit Natural.Unit)),(_subtract remainder factor))

        match right with
        | z when _equality z Natural.Zero -> raise (new DivideByZeroException())
        | u when _equality u Natural.Unit -> (left,Natural.Zero)
        | r when _equality r left -> (Natural.Unit,Natural.Zero)
        | _ ->
            op 0

    member internal Natural.Data = _compress data

    new() = Natural( [0u] )
    new(data:Natural) = Natural( data.Data )
    new(data:uint32) = Natural( [data] )
    new(data:uint64) = Natural( [
        Convert.ToUInt32( data >>> 32 );
        Convert.ToUInt32( data &&& 0xFFFF_FFFFUL )
    ] )

    // This exists to be nice to C#
    // F# sequences are C# IEnumerables
    new(data:uint32 seq) = Natural( Seq.toList( data ) )

    with
        static member Zero = Natural( [0u] )
        static member Unit = Natural( [1u] )

        static member op_Implicit( i:uint32 ) : Natural =
            Natural( i )
        static member op_Implicit( l:uint64 ) : Natural =
            Natural( l )

        // Bitwise Operators
        static member (&&&) (left:Natural, right:Natural) : Natural =
            _bitwiseAnd left right

        static member (|||) (left:Natural, right:Natural) : Natural =
            _bitwiseOr left right

        static member (^^^) (left:Natural, right:Natural) : Natural =
            _bitwiseXor left right

        static member (~~~) (right:Natural) : Natural =
            _bitwiseNot right

        static member (<<<) (left:Natural, (right:int)) : Natural =
            _leftShift right left

        static member (>>>) (left:Natural, right:int) : Natural =
            _rightShift right left

        // Comparison Operators
        static member op_Equality (left:Natural, right:Natural) : bool =
            _equality left right
 
        static member op_GreaterThan (left:Natural, right:Natural) : bool =
            _greaterThan left right

        static member op_LessThan (left:Natural, right:Natural) : bool =
            _lessThan left right

        static member op_GreaterThanOrEqual (left:Natural, right:Natural) : bool =
            (_equality left right) || (_greaterThan left right)

        static member op_LessThanOrEqual (left:Natural, right:Natural) : bool =
            (_equality left right) || (_lessThan left right)

        static member op_Inequality (left:Natural, right:Natural) : bool =
            not (_equality left right)
 
        // Arithmetic Operators
        // Binary
        static member (+) (left:Natural, right:Natural) : Natural =
            _add left right
 
        static member (-) (left:Natural, right:Natural) : Natural =
            _subtract left right

        static member (*) (left:Natural, right:Natural) : Natural =
            _multiply left right
            
        static member (/%) (left:Natural, right:Natural) : Natural*Natural =
            _divideModulo left right

        static member (/) (left:Natural, right:Natural) : Natural =
            let (q,_) = _divideModulo left right
            q

        static member (%) (left:Natural, right:Natural) : Natural =
            let (_,r) = _divideModulo left right
            r

        // Unary

        // .NET Object Overrides
        override this.Equals( that:Object ) =
            match that with
            | :? Natural as n -> _equality this n
            | :? uint32 as ui -> _equality this (Natural( ui ))
            | :? uint64 as ul -> _equality this (Natural( ul ))
            | _ -> false

        override this.GetHashCode() =
            let v =
                this.Data
                |> List.fold ( fun acc i -> acc ^^^ i ) 0u

            v.GetHashCode()

        override this.ToString() =
            let rec f n : char list =
                match n with
                | z when z = Natural.Zero -> []
                | _ ->
                    let (q,r) = _divideModulo n (Natural([10u]))
                    Convert.ToChar(r.Data.Head + 48u) :: (f q)

            if _equality Natural.Zero this then
                "0"
            else
                String.Concat(
                    f this
                    |> List.rev
                    |> List.toArray
                )

        // IComparable (for .NET) 
        member this.CompareTo that = (this :> IComparable).CompareTo( that )
        interface IComparable with
            member this.CompareTo that =
                let doWork left right =
                    match _equality left right with
                    | true -> 0
                    | false ->
                        match _greaterThan left right with
                        | true -> 1
                        | false -> -1

                match that with
                | :? Natural as n -> doWork this n
                | :? UInt32 as ui -> doWork this (Natural( ui ))
                | :? UInt64 as ul -> doWork this (Natural( ul ))
                | _ -> raise (new ArgumentException( "obj is not the same type as this instance." ))

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

        //interface IUnsignedNumber<Natural> with
        interface IEquatable<Natural> with
            member this.Equals( that:Natural ) : bool = 
                _equality this that

        interface IEqualityOperators<Natural,Natural,bool> with
            static member op_Inequality( left, right ) =
                not (_equality left right)
            static member op_Equality( left, right ) =
                _equality left right

        interface IAdditionOperators<Natural,Natural,Natural> with
            static member (+) (left:Natural, right:Natural) : Natural = 
                _add left right
            static member op_CheckedAddition (left:Natural, right:Natural) : Natural = 
                // Naturals don't overflow, and addition can't underflow
                _add left right

        interface IAdditiveIdentity<Natural,Natural> with
            static member AdditiveIdentity
                with get () = Natural.Zero

        interface IIncrementOperators<Natural> with
            static member op_CheckedIncrement ( value:Natural ) : Natural = 
                IAdditionOperators.op_CheckedAddition( value, Natural.Unit )
            static member op_Increment( value:Natural ) : Natural = 
                IAdditionOperators.op_Addition( value, Natural.Unit )

        interface ISubtractionOperators<Natural,Natural,Natural> with
            // Both of these throw an OverflowException
            // After a bit of research, I found an example in the Decimal type
            // A Decimal does not have a valid internal state on an Overflow/Underflow
            // As such, it always throws on an Overflow/Underflow
            static member op_CheckedSubtraction(left:Natural, right:Natural) : Natural = 
                _subtract left right
            static member (-) (left:Natural, right:Natural) : Natural = 
                _subtract left right

        interface IDecrementOperators<Natural> with
            static member op_Decrement(value:Natural) : Natural =
                _subtract value Natural.Unit
            static member op_CheckedDecrement( value: Natural ): Natural = 
                _subtract value Natural.Unit
 
        interface IUnaryNegationOperators<Natural,Natural> with
            static member op_CheckedUnaryNegation( value:Natural ) : Natural = 
                raise ( System.OverflowException() )
            static member (~-)( value: Natural ) : Natural = 
                raise ( System.OverflowException() )
        
        interface IUnaryPlusOperators<Natural,Natural> with
            static member (~+)( value:Natural ) : Natural = 
                Natural( value )

        interface IMultiplyOperators<Natural,Natural,Natural> with
            static member op_CheckedMultiply( left: Natural, right: Natural ) : Natural = 
                _multiply left right
            static member (*)( left:Natural, right:Natural ) : Natural = 
                _multiply left right

        interface IMultiplicativeIdentity<Natural,Natural> with
            static member MultiplicativeIdentity
                with get () = Natural.Unit

        interface IDivisionOperators<Natural,Natural,Natural> with
            static member op_CheckedDivision( left: Natural, right: Natural ) : Natural =
                let (q,_) = _divideModulo left right
                q
            static member (/)( left:Natural, right:Natural ) : Natural = 
                let (q,_) = _divideModulo left right
                q

        interface IFormattable with
            member this.ToString( format:string, formatProvider:IFormatProvider ) : string =
                let trimStart (c:char) (s:string) : string = s.TrimStart( c )

                let parseFormatString (f:string) =
                    match f with
                    | null -> ('G', None)
                    | _ ->
                        match f.Trim() with
                        | x when x.Length = 0 -> ('G', None)
                        | x when x.Length = 1 -> (x[0], None)
                        | _ -> (f[0], Some( Int32.Parse(f.Substring(1)) ))

                let (specifier, precision) = parseFormatString format
                let numberFormatInfo =
                    if null = formatProvider
                    then CultureInfo.CurrentCulture.NumberFormat
                    else formatProvider.GetFormat( typeof<NumberFormatInfo> ) :?> NumberFormatInfo

                let processSeparators rawString (groupSizesArray:int array) groupSeparator decimalDigits decimalSeparator =
                    // Yes, this is nasty. It works, but that doesn't mean I like how it did it
                    let reverseString (s:string) = string( (s.ToCharArray()) |> Array.rev )
                    let separator = reverseString groupSeparator

                    let mutable groupSize =
                        if 0 = groupSizesArray.Length
                        then 0
                        else Array.head groupSizesArray
                    let mutable groupSizes =
                        if 0 = groupSizesArray.Length
                        then [|0|]
                        else Array.tail groupSizesArray
                    let mutable reversedString = reverseString rawString
                    let mutable i = groupSize

                    while ( i < reversedString.Length ) && ( 0 <> groupSize ) do
                        reversedString <- reversedString.Insert( i, separator )
                        
                        if 1 < groupSizes.Length
                        then
                            groupSize <- Array.head groupSizes
                            groupSizes <- Array.tail groupSizes
                            ()

                        i <- i + groupSize + separator.Length

                    let suffix =
                        match precision with
                        | None -> $"{decimalSeparator}{string( '0', decimalDigits )}"
                        | Some( 0 ) -> String.Empty
                        | Some( l ) -> $"{decimalSeparator}{string( '0', l )}"

                    $"{reverseString reversedString}{suffix}"

                match specifier with
                // General and Round-trip
                | 'G' | 'g' | 'R' | 'r' ->
                    this.ToString()

                // Binary
                | 'B' | 'b' ->
                    let binaryResult = 
                        this.Data
                        |> List.map ( fun ui -> ui.ToString( "B32" ) )
                        |> List.toArray
                        |> String.concat ""
                        |> trimStart '0'

                    match precision with
                    | None -> binaryResult
                    | Some( p ) ->
                        if binaryResult.Length < p
                        then $"{string( '0', p - binaryResult.Length )}{binaryResult}"
                        else binaryResult
                    
                // Currency
                | 'C' | 'c' ->
                    let s =
                        processSeparators
                            (this.ToString())
                            numberFormatInfo.CurrencyGroupSizes
                            numberFormatInfo.CurrencyGroupSeparator
                            numberFormatInfo.CurrencyDecimalDigits
                            numberFormatInfo.CurrencyDecimalSeparator

                    match numberFormatInfo.CurrencyPositivePattern with
                    | 0 -> $"{numberFormatInfo.CurrencySymbol}{s}"
                    | 1 -> $"{s}{numberFormatInfo.CurrencySymbol}"
                    | 2 -> $"{numberFormatInfo.CurrencySymbol} {s}"
                    | 3 -> $"{s} {numberFormatInfo.CurrencySymbol}"
                    | _ -> raise (System.FormatException())

                // Decimal
                | 'D' | 'd' ->
                    let result = this.ToString()
                    match precision with
                    | Some( p ) ->
                        if p > result.Length
                        then String('0', p - result.Length) + result
                        else result
                    | None -> result

                // Exponetial
                | 'E' | 'e' ->
                    let decimalPlaces =
                        match precision with
                        | Some( p ) -> p
                        | None -> 6

                    let rawString = this.ToString()
                    let exponent = rawString.Length - 1

                    let truncString = 
                        match decimalPlaces - exponent with
                        | p when p > 0 -> $"{rawString}{string('0', p)}" // Pad Zeros
                        | n when n < 0 ->
                            // Check for rounding
                            match rawString[decimalPlaces+1] with
                            | '5' | '6' | '7' | '8' | '9' ->
                                // Rounding
                                ( Natural.Parse( rawString.Substring( decimalPlaces ) ) + Natural.Unit ).ToString()
                            | _ ->
                                // Rounding not required
                                rawString.Substring( decimalPlaces )
                        | _ -> rawString // Nothing to do

                    $"{truncString.Insert( 1, numberFormatInfo.NumberDecimalSeparator )}{specifier}{numberFormatInfo.PositiveSign}{exponent:D3}"

                // Fixed-point
                | 'F' | 'f' ->
                    let decimalZeros =
                        match precision with
                        | Some( i ) -> i
                        | None -> numberFormatInfo.NumberDecimalDigits
                    $"{this}{numberFormatInfo.NumberDecimalSeparator}{String( '0', decimalZeros )}"

                // Number
                | 'N' | 'n' ->
                    processSeparators
                        (this.ToString())
                        numberFormatInfo.NumberGroupSizes
                        numberFormatInfo.NumberGroupSeparator
                        numberFormatInfo.NumberDecimalDigits
                        numberFormatInfo.NumberDecimalSeparator

                // Percent
                | 'P' | 'p' ->
                    let s =
                        processSeparators
                            (this.ToString())
                            numberFormatInfo.PercentGroupSizes
                            numberFormatInfo.PercentGroupSeparator
                            numberFormatInfo.PercentDecimalDigits
                            numberFormatInfo.PercentDecimalSeparator

                    match numberFormatInfo.PercentPositivePattern with
                    | 0 -> $"{s} {numberFormatInfo.PercentSymbol}"
                    | 1 -> $"{s}{numberFormatInfo.PercentSymbol}"
                    | 2 -> $"{numberFormatInfo.PercentSymbol}{s}"
                    | 3 -> $"{numberFormatInfo.PercentSymbol} {s}"
                    | _ -> raise (System.FormatException())

                // Hexadecimal
                | 'X' | 'x' ->
                    let hexResult =
                        this.Data
                        |> List.map ( fun ui -> ui.ToString( $"{specifier}8" ).Substring( 2 ) )
                        |> List.toArray
                        |> String.concat ""
                        |> trimStart '0'
 
                    let paddedHexResult =
                        match precision with
                        | None -> hexResult
                        | Some( p ) ->
                            if hexResult.Length < p
                            then $"{string( '0', p - hexResult.Length )}{hexResult}"
                            else hexResult

                    $"0{specifier}" + paddedHexResult

                | _ -> raise ( System.FormatException( $"{specifier} is not a valid format specifier" ) )
        
        interface ISpanFormattable with
            member this.TryFormat( destination: Span<char>, charsWritten: byref<int>, format: ReadOnlySpan<char>, provider: IFormatProvider ) : bool = 
                let formattedString = (this :> IFormattable).ToString( format.ToString(), provider )
 
                if destination.Length < formattedString.Length
                then 
                    charsWritten <- destination.Length
                    formattedString
                        .Substring( 0, destination.Length )
                        .AsSpan()
                        .CopyTo( destination )
                    false
                else
                    charsWritten <- formattedString.Length
                    formattedString
                        .AsSpan()
                        .CopyTo( destination )
                    true

        interface IParsable<Natural> with
            /// <inheritdoc/>
            /// <remarks>
            ///     This method behaves similar to <see cref="System.UInt64.Parse(string, IFormatProvider?)"/>.
            ///     The exception is this method also allows <see cref="System.Globalization.NumberFormatInfo.NumberGroupSeparator"/>
            /// </remarks>
            static member Parse( s:string, provider:IFormatProvider ) : Natural = 
                if String.IsNullOrWhiteSpace( s )
                then raise (System.ArgumentNullException( nameof( s ) ))

                let numberFormatInfo =
                    if null = provider
                    then CultureInfo.CurrentCulture.NumberFormat
                    else provider.GetFormat( typeof<NumberFormatInfo> ) :?> NumberFormatInfo

                let processedString =
                    s
                        .Replace( numberFormatInfo.NumberGroupSeparator, "" )
                        .Replace( numberFormatInfo.PositiveSign, "" )
                        .Trim()

                if processedString.StartsWith( numberFormatInfo.NegativeSign )
                then raise (System.OverflowException())

                if Array.exists ( fun c -> Char.IsAsciiDigit( c ) |> not ) (processedString.ToCharArray())
                then raise (System.FormatException())

                Natural.Parse( processedString )
            static member TryParse( s: string, provider: IFormatProvider, result: byref<Natural> ): bool = 
                try
                    result <- IParsable.Parse( s, provider )
                    true
                with _ ->
                    result <- Natural.Zero
                    false
        
        interface ISpanParsable<Natural> with
            static member Parse( s: ReadOnlySpan<char>, provider: IFormatProvider ) : Natural = 
                IParsable.Parse( s.ToString(), provider )
            static member TryParse( s: ReadOnlySpan<char>, provider: IFormatProvider, result: byref<Natural> ) : bool = 
                IParsable.TryParse( s.ToString(), provider, ref result )
        
        interface INumberBase<Natural> with
            static member One
                with get () = Natural.Unit
            static member Radix 
                with get () = 2
            static member Zero
                with get () = Natural.Zero

            static member Abs( value:Natural ) : Natural = 
                Natural( value.Data )

            static member CreateChecked( value:'TOther ) : Natural = 
                // By definition of the interface, we know 'TOther is an INumberBase<'TOther>
                let inline IsNegative x = (^x: (static member IsNegative: ^x -> bool)( x ))
                let inline IsInteger x = (^x: (static member IsInteger: ^x -> bool)( x ))

                // Being an INumberBase, we also know it's IFormattable
                let inline tostr x = (^x: (member ToString: string * IFormatProvider -> string)( x, "", null ))

                if IsNegative value
                then raise (System.OverflowException())

                if IsInteger value
                then Natural.Parse( value.ToString() )
                else raise (System.NotSupportedException())

                //match value with
                //| :? Natural as n -> Natural( n )
                //| :? uint32 as ui -> Natural( ui )
                //| :? uint64 as ul -> Natural( ul )
                //| :? INumberBase<'TOther> as nb ->
                //    if IsNegative nb
                //    then raise (System.OverflowException())
                //    raise (System.NotSupportedException())
                //| _ ->
                //    raise (System.NotSupportedException())
            static member CreateSaturating( value:'TOther ) : Natural = 
                // By definition of the interface, we know 'TOther is a INumberBase<'TOther>
                let inline IsNegative x = (^x: (static member IsNegative: ^x -> bool)( x ))
                let inline IsInteger x = (^x: (static member IsInteger: ^x -> bool)( x ))

                if IsInteger value |> not
                then raise (System.NotSupportedException())

                if IsNegative value
                then Natural.Zero
                else Natural.Parse( value.ToString() )
            static member CreateTruncating( value: 'TOther ) : Natural = 
                raise (System.NotImplementedException())

            static member IsCanonical( value:Natural ) : bool =
                true
            static member IsComplexNumber( value:Natural ) : bool =
                false
            static member IsEvenInteger( value: Natural ) : bool = 
                _bitwiseAnd Natural.Unit value
                |> _equality Natural.Zero
            static member IsFinite( value:Natural ) : bool =
                true
            static member IsImaginaryNumber( value:Natural ) : bool =
                _equality Natural.Zero value
            static member IsInfinity( value:Natural ) : bool =
                false
            static member IsInteger( value:Natural ) : bool =
                true
            static member IsNaN( value:Natural ) : bool =
                false
            static member IsNegative( value:Natural ) : bool =
                false
            static member IsNegativeInfinity( value:Natural ) : bool =
                false
            static member IsNormal( value: Natural ): bool = 
                true
            static member IsOddInteger( value: Natural ) : bool = 
                _bitwiseAnd Natural.Unit value
                |> _equality Natural.Unit
            static member IsPositive( value:Natural ) : bool =
                true
            static member IsPositiveInfinity( value:Natural ) : bool =
                false
            static member IsRealNumber( value:Natural ) : bool =
                true
            static member IsSubnormal( value: Natural ): bool = 
                false
            static member IsZero( value:Natural ) : bool =
                _equality Natural.Zero value

            static member MaxMagnitude( x: Natural, y: Natural ) : Natural = 
                if _greaterThan x y
                then x
                else y
            static member MaxMagnitudeNumber( x: Natural, y: Natural ) : Natural = 
                if _greaterThan x y
                then x
                else y
            static member MinMagnitude( x: Natural, y: Natural ) : Natural = 
                if _lessThan x y
                then x
                else y
            static member MinMagnitudeNumber( x: Natural, y: Natural ) : Natural = 
                if _lessThan x y
                then x
                else y

            static member Parse( s:ReadOnlySpan<char>, style:NumberStyles, provider:IFormatProvider ) : Natural = 
                let mutable span = Span<char>( s.ToArray() )

                span <-
                    if style.HasFlag(NumberStyles.AllowLeadingWhite)
                    then span.TrimStart()
                    else span

                span <-
                    if style.HasFlag(NumberStyles.AllowTrailingWhite)
                    then span.TrimEnd()
                    else span

                //style.HasFlag(NumberStyles.AllowLeadingSign)
                raise (System.NotImplementedException())
            static member Parse( s:string, style:NumberStyles, provider:IFormatProvider ) : Natural = 
                INumberBase.Parse( s.AsSpan(), style, provider )
            static member TryParse( s:ReadOnlySpan<char>, style:NumberStyles, provider:IFormatProvider, result:byref<Natural> ) : bool = 
                try
                    result <- INumberBase.Parse( s, style, provider )
                    true
                with _ ->
                    result <- Natural.Zero
                    false
            static member TryParse( s:string, style:NumberStyles, provider: IFormatProvider, result:byref<Natural> ) : bool = 
                INumberBase.TryParse( s.AsSpan(), style, provider, ref result )

            static member TryConvertFromChecked( value:'TOther, result:byref<Natural> ) : bool = 
                raise (System.NotImplementedException())
            static member TryConvertFromSaturating( value:'TOther, result:byref<Natural> ) : bool = 
                raise (System.NotImplementedException())
            static member TryConvertFromTruncating( value:'TOther, result:byref<Natural> ) : bool = 
                raise (System.NotImplementedException())
            static member TryConvertToChecked( value:Natural, result:byref<'TOther> ) : bool = 
                raise (System.NotImplementedException())
            static member TryConvertToSaturating( value:Natural, result:byref<'TOther> ) : bool = 
                raise (System.NotImplementedException())
            static member TryConvertToTruncating( value:Natural, result:byref<'TOther> ) : bool = 
                raise (System.NotImplementedException())

        interface IBitwiseOperators<Natural,Natural,Natural> with
            static member (&&&) ( left:Natural, right:Natural ) : Natural =
                _bitwiseAnd left right
            static member (|||) ( left:Natural, right:Natural ) : Natural =
                _bitwiseOr left right
            static member op_OnesComplement ( value:Natural ) : Natural =
                _bitwiseNot value
            static member (^^^) ( left:Natural, right:Natural ) : Natural =
                _bitwiseXor left right

        interface IShiftOperators<Natural,int,Natural> with
            static member (<<<) ( left:Natural, right:int ) : Natural =
                _leftShift right left
            static member (>>>) ( left:Natural, right:int ) : Natural =
                _rightShift right left
            static member op_UnsignedRightShift( left:Natural, right:int ) : Natural =
                _rightShift right left

        //interface IComparisonOperators<Natural,Natural,bool> with
        //    static member op_LessThan( left, right ) =
        //        left < right
        //    static member op_LessThanOrEqual( left, right ) =
        //        left <= right
        //    static member op_GreaterThan( left, right ) =
        //        left > right
        //    static member op_GreaterThanOrEqual( left, right ) =
        //        left >= right

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
        //
        //    member this.op_CheckedAddition(left: Natural, right: Natural): Natural = 
        //        left + right
        //
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