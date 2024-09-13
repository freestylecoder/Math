namespace Freestylecoding.Math

open System
open System.Linq
open System.Numerics
open System.Globalization

type public Natural(data:uint32 list) =
    static let _defaultNumberStyle = NumberStyles.Integer ||| NumberStyles.AllowThousands
    static let _defaultFormatProvider = CultureInfo.CurrentCulture.NumberFormat

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

    static let _parse (s:ReadOnlySpan<char>) (style:NumberStyles) (provider:IFormatProvider) : Natural = 
        let powersOf2 = Seq.unfold (fun state -> Some( state, _leftShift 1 state )) Natural.Unit
        let powersOf10 = Seq.unfold (fun state -> Some( state, _add (_leftShift 3 state) (_leftShift 1 state) )) Natural.Unit
        let powersOf16 = Seq.unfold (fun state -> Some( state, _leftShift 4 state )) Natural.Unit

        let pow10n e =
            let (quot,rem) = _divideModulo e (Natural( UInt32.MaxValue >>> 1 ))
            let maxPowerOf10 =
                if _greaterThan quot Natural.Zero
                then _multiply quot (powersOf10.ElementAt( Int32.MaxValue ))
                else Natural.Unit

            _multiply maxPowerOf10 (powersOf10.ElementAt( Convert.ToInt32( rem.Data.Head ) ))

        let hasFlag (flag:NumberStyles) : bool =
            flag = (style &&& flag)

        match ((hasFlag NumberStyles.AllowBinarySpecifier),(hasFlag NumberStyles.AllowHexSpecifier)) with
        | (true,true) ->
            raise (System.ArgumentException( "With the AllowHexSpecifier or AllowBinarySpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be AllowLeadingWhite and AllowTrailingWhite.", nameof( style )))
        | (true,false) ->
            if s.ContainsAnyExcept( '0', '1' )
            then raise (System.FormatException())
                    
            s.ToSeq()
            |> Seq.rev
            |> Seq.map (fun c -> if '0' = c then Natural.Zero else Natural.Unit )
            |> Seq.map2 (fun n1 n2 -> _multiply n1 n2 ) powersOf2
            |> Seq.sum
        | (false,true) ->
            if s.ContainsAnyExcept( "0123456789abcdefABCDEF".AsSpan() )
            then raise (System.FormatException())
                    
            s.ToSeq()
            |> Seq.rev
            |> Seq.map (fun c -> Natural(Convert.ToUInt32(c.ToString(), 16)))
            |> Seq.map2 (fun n1 n2 -> _multiply n1 n2 ) powersOf16
            |> Seq.sum
        | _ ->
            let listToNatural (l:char list) : Natural =
                l
                |> List.rev
                |> List.map (fun c -> Natural([Convert.ToUInt32(CharUnicodeInfo.GetNumericValue(c))]))
                |> List.toSeq
                |> Seq.map2 (fun n1 n2 -> _multiply n1 n2 ) powersOf10
                |> Seq.sum

            let numberFormatInfo =
                if null = provider
                then _defaultFormatProvider
                else provider.GetFormat( typeof<NumberFormatInfo> ) :?> NumberFormatInfo

            let parseBuddy = ParseBuddy( style, numberFormatInfo )
            parseBuddy.Parse( s )

            let decFactor = powersOf10.ElementAt( parseBuddy.Decimal.Length )
            let natWhole =
                seq { parseBuddy.WholeNumber; parseBuddy.Decimal }
                |> List.concat
                |> listToNatural

            let natExp = listToNatural parseBuddy.Exponent
            let expFactor = pow10n natExp

            let (q,r) =
                if parseBuddy.IsExpNegative
                then _divideModulo natWhole ( _multiply expFactor decFactor )
                else _divideModulo ( _multiply natWhole expFactor ) decFactor
                
            if _greaterThan r Natural.Zero
            then raise (System.OverflowException())
                
            if parseBuddy.IsNegative && ( q > Natural.Zero )
            then raise (System.OverflowException())

            q

    static let _tryParse (s:ReadOnlySpan<Char>) (style:NumberStyles) (provider:IFormatProvider) (result:byref<Natural>) : bool =
        try
            result <- _parse s style provider
            true
        with _ ->
            result <- Natural.Zero
            false

    member internal Natural.Data = _compress data

    new() = Natural( [0u] )
    new(data:Natural) = Natural( data.Data )
    new(data:uint8 ) = Natural( [uint32 data] )
    new(data:uint16) = Natural( [uint32 data] )
    new(data:uint32) = Natural( [data] )
    new(data:uint64) = Natural( [
        Convert.ToUInt32( data >>> 32 );
        Convert.ToUInt32( data &&& 0xFFFF_FFFFUL )
    ] )
    new(data:UInt128) =
        let down x : uint32 = UInt128.op_Explicit( ( data >>> x ) &&& 0xFFFF_FFFFUL )
        Natural( [
            down 96;
            down 64;
            down 32;
            down  0
        ] )

    // This exists to be nice to C#
    // F# sequences are C# IEnumerables
    new(data:uint32 seq) = Natural( Seq.toList( data ) )

    with
        static member Zero = Natural( [0u] )
        static member Unit = Natural( [1u] )

        // In bound implicit up casts
        static member op_Implicit( y:uint8 ) : Natural =
            Natural( y )
        static member op_Implicit( s:uint16 ) : Natural =
            Natural( s )
        static member op_Implicit( i:uint32 ) : Natural =
            Natural( i )
        static member op_Implicit( L:uint64 ) : Natural =
            Natural( L )
        static member op_Implicit( LL:UInt128 ) : Natural =
            Natural( LL )

        // Out bound implicit up cast
        static member op_Implicit( n:Natural ) : BigInteger =
            BigInteger(
                ReadOnlySpan<Byte>(
                    n.Data
                        |> List.map (fun ui -> List.map (fun b -> b &&& 0xFFu) [ ui >>> 24; ui >>> 16; ui >>> 8; ui])
                        |> List.collect (fun i -> i)
                        |> List.map (fun ui -> Convert.ToByte(ui))
                        |> List.toArray
                ),
                true,
                true
            )

        // In bound explicit checked side cast
        static member op_Explicit( bi:BigInteger ) : Natural =
            if BigInteger.IsNegative( bi )
            then raise (System.OverflowException())

            let bitMask = BigInteger( UInt32.MaxValue )
            let rec ChunkBigInteger (x:BigInteger) =
                if x > bitMask
                then
                    (uint32 (x &&& bitMask)) :: (ChunkBigInteger (x >>> 32))
                else
                    [ uint32 x ]

            Natural( (List.rev (ChunkBigInteger bi)) )

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
        static member private Equals( this:Natural, that:obj ) =
            let thatType = that.GetType()

            let IsNumberBase (t:Type) =
                t
                    .GetInterfaces()
                    .Select( fun t -> t.Name )
                    .Any( fun s -> s.Contains( "INumberBase" ) )

            let IsPositive (t:Type) =
                let method = 
                    t
                        .GetMethods(
                            Reflection.BindingFlags.Public |||
                            // If it's an unsigned type, IsPositive is private
                            Reflection.BindingFlags.NonPublic |||
                            Reflection.BindingFlags.Static |||
                            Reflection.BindingFlags.FlattenHierarchy
                        )
                        .First( fun m -> m.Name.EndsWith( "IsPositive" ) )

                match method with
                | null -> false
                | _ -> 
                    match method.Invoke( null, [| that |] ) with
                    | :? bool as b -> b
                    | _ -> false

            let IsInteger (t:Type) =
                let methods = 
                    t
                        .GetMethods(
                            Reflection.BindingFlags.Public |||
                            // If it's an integer type, IsInteger is private
                            Reflection.BindingFlags.NonPublic |||
                            Reflection.BindingFlags.Static |||
                            Reflection.BindingFlags.FlattenHierarchy
                        )
                let method = 
                    methods
                        .First( fun m -> m.Name.Contains( "IsInteger" ) )

                match method with
                | null -> false
                | _ -> 
                    match method.Invoke( null, [| that |] ) with
                    | :? bool as b -> b
                    | _ -> false

            let CreateChecked (t:Type) =
                let genericMethod = 
                    typeof<INumberBase<Natural>>
                        .GetMethods()
                        .First( fun m -> m.Name.EndsWith( "CreateChecked" ) )

                let method =
                    genericMethod.MakeGenericMethod( [| t.UnderlyingSystemType |])

                match method with
                | null -> Natural.Zero
                | _ -> 
                    match method.Invoke( null, [| that |] ) with
                    | :? Natural as n -> n
                    | _ -> Natural.Zero

            IsNumberBase thatType
            &&
            IsPositive thatType
            &&
            IsInteger thatType
            &&
            _equality this (CreateChecked thatType)

        override this.Equals( that:Object ) =
            match that with
            | :? Natural    as  n  -> _equality this n
            | :? byte       as  b  -> _equality this (Natural(  b  ))
            | :? uint16     as us  -> _equality this (Natural( us  ))
            | :? uint32     as ui  -> _equality this (Natural( ui  ))
            | :? uint64     as uL  -> _equality this (Natural( uL  ))
            | :? UInt128    as uLL -> _equality this (Natural( uLL ))
            | :? BigInteger as bi  ->
                if BigInteger.IsNegative bi
                then false
                else _equality this (Natural.op_Explicit bi)
            | _ ->
                // We only worry about using reflection after we get the easy ones
                Natural.Equals( this, that )

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
        interface IComparable with
            member this.CompareTo that =
                let doWork left right =
                    match _equality left right with
                    | true -> 0
                    | false ->
                        match _greaterThan left right with
                        | true -> 1
                        | false -> -1

                // This is only supposted to handle the exact same type
                // As such, I'm only handling the types I support implicit conversion from
                // This is already more than it should handle
                match that with
                | :? Natural as n   -> doWork this n
                | :? Byte    as uy  -> doWork this (Natural uy )
                | :? UInt16  as us  -> doWork this (Natural us )
                | :? UInt32  as ui  -> doWork this (Natural ui )
                | :? UInt64  as ul  -> doWork this (Natural ul )
                | :? UInt128 as uLL -> doWork this (Natural uLL)
                | _ -> raise (new ArgumentException( "obj is not the same type as this instance." ))

        static member Parse (s:string) : Natural =
            _parse (s.AsSpan()) _defaultNumberStyle _defaultFormatProvider
        static member Parse (s:string, style:System.Globalization.NumberStyles) : Natural =
            _parse (s.AsSpan()) style _defaultFormatProvider

        static member TryParse( s:string, result:byref<Natural>) : bool =
            _tryParse (s.AsSpan()) _defaultNumberStyle _defaultFormatProvider &result
        static member TryParse( s:ReadOnlySpan<Char>, result:byref<Natural>) : bool =
            _tryParse s _defaultNumberStyle _defaultFormatProvider &result
        static member TryParse( s:ReadOnlySpan<Byte>, result:byref<Natural>) : bool =
            let utf16text = Span<Char>( ( Array.create s.Length '\u0000' ) )
            let mutable x = 0
            System.Text.Unicode.Utf8.ToUtf16( s, utf16text, &x, &x, true, true ) |> ignore
            _tryParse utf16text _defaultNumberStyle _defaultFormatProvider &result

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
                IAdditionOperators<Natural,Natural,Natural>.op_CheckedAddition( value, Natural.Unit )
            static member op_Increment( value:Natural ) : Natural =
                _add value Natural.Unit

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
                    then _defaultFormatProvider
                    else formatProvider.GetFormat( typeof<NumberFormatInfo> ) :?> NumberFormatInfo

                let processSeparators rawString (groupSizesArray:int array) groupSeparator decimalDigits decimalSeparator =
                    // Yes, this is nasty. It works, but that doesn't mean I like how it did it
                    let reverseString (s:string) = String( (s.ToCharArray()) |> Array.rev )
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
                        
                        if 0 < groupSizes.Length
                        then
                            groupSize <- Array.head groupSizes
                            groupSizes <- Array.tail groupSizes
                            ()

                        i <- i + groupSize + separator.Length

                    let suffix =
                        match precision with
                        | None -> $"{decimalSeparator}{String( '0', decimalDigits )}"
                        | Some( 0 ) -> String.Empty
                        | Some( l ) -> $"{decimalSeparator}{String( '0', l )}"

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
                        then $"{String( '0', p - binaryResult.Length )}{binaryResult}"
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

                // Exponential
                | 'E' | 'e' ->
                    let decimalPlaces =
                        match precision with
                        | Some( p ) -> p
                        | None -> 6

                    let rawString = this.ToString()
                    let exponent = rawString.Length - 1

                    let truncString = 
                        match decimalPlaces - exponent with
                        | p when p > 0 -> rawString.PadRight( decimalPlaces + 1, '0' )
                        | n when n < 0 ->
                            // At this point, we know we're getting a substring
                            // So, just make sure we have plenty of zeros to deal with everything
                            let paddedString = rawString.PadRight( exponent + decimalPlaces, '0' )

                            // Check for rounding
                            match paddedString[decimalPlaces+1] with
                            | '5' | '6' | '7' | '8' | '9' ->
                                // Rounding
                                ( Natural.Parse( paddedString.Substring( 0, decimalPlaces+1 ) ) + Natural.Unit ).ToString()
                            | _ ->
                                // Rounding not required
                                paddedString.Substring( 0, decimalPlaces+1 )
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
                            $"{this}00"
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
                        |> List.map ( fun ui -> ui.ToString( $"{specifier}8" ) )
                        |> List.toArray
                        |> String.concat ""
                        |> trimStart '0'
 
                    match precision with
                    | None -> hexResult
                    | Some( p ) ->
                        if hexResult.Length < p
                        then $"{String( '0', p - hexResult.Length )}{hexResult}"
                        else hexResult

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
                _parse (s.AsSpan()) _defaultNumberStyle provider
            static member TryParse( s: string, provider: IFormatProvider, result: byref<Natural> ): bool = 
                _tryParse (s.AsSpan()) _defaultNumberStyle provider &result
        
        interface ISpanParsable<Natural> with
            static member Parse( s: ReadOnlySpan<char>, provider: IFormatProvider ) : Natural = 
                _parse s _defaultNumberStyle provider
            static member TryParse( s: ReadOnlySpan<char>, provider: IFormatProvider, result: byref<Natural> ) : bool = 
                _tryParse s _defaultNumberStyle provider &result
        
        interface IUtf8SpanParsable<Natural> with
            // INumberBase<T> handles ALL the ReadOnlySpan<byte> cases
            // Unfortunately, F# has no way to access them
            static member Parse( utf8text: ReadOnlySpan<byte>, provider: IFormatProvider ) : Natural = 
                let utf16text = Span<Char>( ( Array.create utf8text.Length '\u0000' ) )
                let mutable x = 0
                System.Text.Unicode.Utf8.ToUtf16( utf8text, utf16text, &x, &x, true, true ) |> ignore
                _parse utf16text _defaultNumberStyle provider
            static member TryParse( s: ReadOnlySpan<byte>, provider: IFormatProvider, result: byref<Natural> ) : bool = 
                let utf16text = Span<Char>( ( Array.create s.Length '\u0000' ) )
                let mutable x = 0
                System.Text.Unicode.Utf8.ToUtf16( s, utf16text, &x, &x, true, true ) |> ignore
                _tryParse utf16text _defaultNumberStyle provider &result
        
        interface INumberBase<Natural> with
            static member One
                with get () = Natural.Unit
            static member Radix 
                with get () = 2
            static member Zero
                with get () = Natural.Zero

            static member Abs( value:Natural ) : Natural = 
                Natural( value.Data )

            // These are not implemented because they are virtual in the interface
            // The default versions are just fine, as they use the "Try" versions
            //static member CreateChecked<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value:'TOther ) : Natural = 
            //    raise (System.NotImplementedException( "CreateChecked" ))
            //static member CreateSaturating<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value:'TOther ) : Natural = 
            //    raise (System.NotImplementedException( "CreateSaturating" ))
            //static member CreateTruncating<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value: 'TOther ) : Natural = 
            //    raise (System.NotImplementedException( "CreateTruncating" ))

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
                // Technically, 0 is an imaginary number
                // Keeping this a const false to match UInt64
                false
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
                _parse s style provider
            static member Parse( s:string, style:NumberStyles, provider:IFormatProvider ) : Natural = 
                _parse (s.AsSpan()) style provider

            static member TryParse( s:ReadOnlySpan<char>, style:NumberStyles, provider:IFormatProvider, result:byref<Natural> ) : bool = 
                _tryParse s style provider &result
            static member TryParse( s:string, style:NumberStyles, provider: IFormatProvider, result:byref<Natural> ) : bool = 
                _tryParse (s.AsSpan()) style provider &result

            static member TryConvertFromChecked<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value:'TOther, result:byref<Natural> ) : bool = 
                // HACK: The F# type coersion system wouldn't pattern match on the generic type
                // It generates a FS0008 at compile time
                let quickCheck (v:obj) : Option<Natural> =
                    match v with
                    | :? Natural  as n  -> Some( Natural( n ) )
                    | :? uint8   as uy  -> Some( Natural( uy ) )
                    | :? uint16  as us  -> Some( Natural( us ) )
                    | :? uint32  as ui  -> Some( Natural( ui ) )
                    | :? uint64  as uL  -> Some( Natural( uL ) )
                    | :? UInt128 as uLL -> Some( Natural( uLL ) )
                    | :? Complex as c ->
                        // NOTE: I know "R" (aka, Round-trip) is not recommended for doubles
                        // The reason it's not recommended is because it messes up decimals
                        // We know this has no decimals, so I can get ALL the whole numbers
                        if Complex.IsRealNumber( c ) && Double.IsPositive( c.Real ) && Double.IsInteger( c.Real )
                        then Some( Natural.Parse( c.Real.ToString( "R" ), NumberStyles.AllowDecimalPoint ) )
                        else raise (System.OverflowException())
                    | _ -> None

                match quickCheck value with
                | Some( n ) ->
                    result <- n
                    true
                | None ->
                    match true with
                    | _ when 'TOther.IsComplexNumber( value ) -> raise (System.OverflowException())
                    | _ when 'TOther.IsImaginaryNumber( value ) -> raise (System.OverflowException())
                    | _ when 'TOther.IsInfinity( value ) -> raise (System.OverflowException())
                    | _ when 'TOther.IsNaN( value ) -> raise (System.OverflowException())
                    | _ when 'TOther.IsNegative( value ) -> raise (System.OverflowException())
                    | _ when 'TOther.IsZero( value ) ->
                        result <- Natural.Zero
                        true
                    | _ when 'TOther.IsInteger( value ) ->
                        result <- Natural.Parse( value.ToString( "R", null ), NumberStyles.Any )
                        true
                    | _ when 'TOther.IsRealNumber( value ) -> raise (System.OverflowException())
                    | _ -> false

            static member TryConvertFromSaturating<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value:'TOther, result:byref<Natural> ) : bool = 
                match true with
                // SURPRISE! NaN is Negative!
                // It's actually a "bug" in the implementation for the native types
                // Either way, we need to catch it first
                | _ when 'TOther.IsNaN( value ) ->
                    raise (System.OverflowException())
                | _ when 'TOther.IsNegative( value ) ->
                    result <- Natural.Zero
                    true
                | _ ->
                    result <- INumberBase<Natural>.CreateChecked<'TOther>( value )
                    true

            static member TryConvertFromTruncating<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value:'TOther, result:byref<Natural> ) : bool = 
                match value :> obj with
                | :? float32 as f ->
                    result <- INumberBase<Natural>.CreateSaturating<float>(
                        Math.Truncate( float f )
                    )
                    true
                | :? float as d -> 
                    result <- INumberBase<Natural>.CreateSaturating<float>( Math.Truncate( d ) )
                    true
                | :? decimal as m -> 
                    result <- INumberBase<Natural>.CreateSaturating<decimal>( Math.Truncate( m ) )
                    true
                | :? Complex as c ->
                    result <- INumberBase<Natural>.CreateSaturating<Complex>(
                        Complex( Math.Truncate( c.Real ), c.Imaginary )
                    )
                    true
                | _ ->
                    // Due to the nature of Natural, I see no difference between Saturating and Truncating
                    result <- INumberBase<Natural>.CreateSaturating<'TOther>( value )
                    true

            static member TryConvertToChecked<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value:Natural, result:byref<'TOther> ) : bool = 
                result <- INumberBase<'TOther>.CreateChecked<BigInteger>(
                    Natural.op_Implicit( value )
                )

                // I don't agree with how BigInteger returns
                //  PositiveInfinity for floating point numbers
                if 'TOther.IsInfinity( result )
                then raise (System.OverflowException())

                true

            static member TryConvertToSaturating<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value:Natural, result:byref<'TOther> ) : bool = 
                result <- INumberBase<'TOther>.CreateSaturating<BigInteger>(
                    Natural.op_Implicit( value )
                )

                // I don't agree with how BigInteger returns
                //  PositiveInfinity for floating point numbers
                if 'TOther.IsInfinity( result )
                then
                    match 'TOther.Zero :> obj with
                    | :? Single ->
                        result <- 'TOther.CreateChecked( Single.MaxValue )
                    | :? Double 
                    | :? Complex ->
                        result <- 'TOther.CreateChecked( Double.MaxValue )
                    | _ -> raise (System.OverflowException())

                true

            static member TryConvertToTruncating<'TOther when 'TOther :> System.Numerics.INumberBase<'TOther>>( value:Natural, result:byref<'TOther> ) : bool = 
                // Manually fix Decimal, because BigInteger acts weird
                match 'TOther.Zero :> obj with
                | :? Decimal ->
                    result <- INumberBase<'TOther>.CreateTruncating<BigInteger>(
                        Natural.op_Implicit( value &&& Natural( [UInt32.MaxValue;UInt32.MaxValue;UInt32.MaxValue;] ) )
                    )
                | _ ->
                    result <- INumberBase<'TOther>.CreateTruncating<BigInteger>(
                        Natural.op_Implicit( value )
                    )

                // I don't agree with how BigInteger returns
                //  PositiveInfinity for floating point numbers
                if 'TOther.IsInfinity( result )
                then
                    let strValue = value.ToString()
                    match 'TOther.Zero :> obj with
                    | :? Single ->
                        // Single.MaxValue is 3.4028235E+38 (-ish)
                        // Thus, it's a 39 digit number
                        let strTrimmed = String( strValue.TakeLast( 39 ).ToArray() )
                        let singleMaxValue = "340282346638528859811704183484516925440"

                        // If it's still too big, get rid of the most significant digit
                        if 0 < strTrimmed.CompareTo( singleMaxValue )
                        then 
                            result <-
                                'TOther.CreateTruncating(
                                    Single.Parse( strTrimmed.Substring( 1, 38 ) )
                                )
                        else
                            result <-
                                'TOther.CreateTruncating(
                                    Single.Parse( strTrimmed )
                                )
                    | :? Double 
                    | :? Complex ->
                        // Double.MaxValue is 1.7E308 (-ish)
                        // Thus, it's a 309 digit number
                        let strTrimmed = String( strValue.TakeLast( 309 ).ToArray() )
                        let doubleMaxValue = "179769313486231570814527423731704356798070567525844996598917476803157260780028538760589558632766878171540458953514382464234321326889464182768467546703537516986049910576551282076245490090389328944075868508455133942304583236903222948165808559332123348274797826204144723168738177180919299881250404026184124858368"

                        // If it's still too big, get rid of the most significant digit
                        if 0 < strTrimmed.CompareTo( doubleMaxValue )
                        then 
                            result <-
                                'TOther.CreateTruncating(
                                    Double.Parse( strTrimmed.Substring( 1, 308 ) )
                                )
                        else
                            result <-
                                'TOther.CreateTruncating(
                                    Double.Parse( strTrimmed )
                                )
                    | _ -> raise (System.OverflowException())

                true

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