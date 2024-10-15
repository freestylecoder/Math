namespace Freestylecoding.Math

open System
open System.Linq
open System.Numerics
open System.Globalization

// This gets rid of warnings for explicit interface calls
// Mostly a necessary evil because of the static interface calls
#nowarn "3536"

[<Diagnostics.DebuggerDisplay( "{SingleThreadedToString()}" )>]
type public Natural(data:uint32 list) =
    static let _defaultNumberStyle = NumberStyles.Integer ||| NumberStyles.AllowThousands
    static let _defaultFormatProvider = CultureInfo.CurrentCulture.NumberFormat

    static let rec _compress (l:uint32 list) : uint32 list =
        match l with
        | [] -> [0u]
        | 0u :: t -> _compress t
        | _ -> l

    // NOTE: All the base operators are declared here
    // This allows us to have all the externally visible operators, interfaces, etc
    //   reference the same code for optimising/debugging purposes
    static let _bitwiseAnd (left:Natural) (right:Natural) : Natural =
        let (l,r) =
            // NOTE: This let binding is a bit of a hack
            // for some reason, the compiler can't figure out Natural.Data is a "uint32 list"
            // Probably because
            //  1) Data isn't defined until later and
            //  2) nothing exists before this to give it a hint
            //      (_bitwiseOperation used to give it that hint)
            // However, I can't move the formal defination of Data before this
            // because let bindings have to come before members
            let (lData:uint32 list) = left.Data
            if lData.Length < right.Data.Length
            then (right.Data, left.Data)
            else (left.Data, right.Data)

        Natural(
            List.splitAt (l.Length - r.Length) l
            |> snd
            |> List.map2 (fun x y -> x &&& y) r
        )

    static let _bitwiseOr (left:Natural) (right:Natural) : Natural =
        let (l,r) =
            if left.Data.Length < right.Data.Length
            then (right.Data, left.Data)
            else (left.Data, right.Data)

        let (l0, l1) = List.splitAt (l.Length - r.Length) l
        Natural(
            l1
            |> List.map2 (fun x y -> x ||| y) r
            |> List.append l0
        )

    static let _bitwiseXor (left:Natural) (right:Natural) : Natural =
        let (l,r) =
            if left.Data.Length < right.Data.Length
            then (right.Data, left.Data)
            else (left.Data, right.Data)

        let (l0, l1) = List.splitAt (l.Length - r.Length) l
        Natural(
            l1
            |> List.map2 (fun x y -> x ^^^ y) r
            |> List.append l0
        )

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
        left.Data.Length = right.Data.Length
        &&
        List.map2 (fun x y -> x = y) left.Data right.Data
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

    static let rec _add (left:Natural) (right:Natural) : Natural = 
        let rec operation (l:uint32 list) (r:uint32 list) : Natural =
            let rawSums = 0u :: List.map2 (fun x y -> x + y) l r
            let overflows = (List.map2 (fun x y -> if x > ( System.UInt32.MaxValue - y ) then 1u else 0u) l r) @ [0u]
            match overflows with
            | _ when Natural.Zero = Natural( overflows ) -> Natural( rawSums )
            | _ -> operation rawSums overflows

        // This does a trivial version of checking for possible overflow
        let rec findSplit (x:uint32 list) (y:uint32 list) (i:int32) : uint32 list * uint32 list * uint32 list =
            if x.Length = y.Length
            then
                // Base case. If the lists are the same length, just return them
                ([], x, y)
            else
                let (x0, x1) = List.splitAt i x
                // We previously checked if X > Y
                // As such, we only need to see if X is in overflow danger
                // FYI: What this does is see if there is a 1 highest order bit
                // Basically: x.Head &&& 0x8000_0000 = 0x8000_0000
                if x.Head > 0x7FFF_FFFFu
                then findSplit x (0u :: y) (i-1)
                else (x0,x1,y)

        // Addition is communitive
        // For ease later, I'm figuring out with is the bigger number
        let (big,little) =
            if _lessThan left right
            then (right, left)
            else (left, right)

        // Spilt the big number to only do the parts that will change
        // Pad the little number to the same length as the part that will change
        let (b0, b1, l) = findSplit big.Data little.Data (big.Data.Length - little.Data.Length)

        let result = operation b1 l
        Natural(
            // See if we ended up with a bigger number than we hoped for
            if result.Data.Length > l.Length
            then
                // Add the overflow to the unchanged part before prepending to the result
                List.append
                    ((_add (Natural( b0 )) (Natural(result.Data.Head))).Data)
                    result.Data.Tail
            else
                // Prepend the unchanged part to the result
                List.append b0 result.Data
        )

    static let _subtract (left:Natural) (right:Natural) : Natural =
        if( _lessThan left right ) then raise (new OverflowException())

        // Find the smallest part of left that is greater than right
        let rec findSplit (x:uint32 list) (y:uint32 list) (i:int32) : uint32 list * uint32 list * uint32 list =
            let (x0, x1) = List.splitAt i x
            if _lessThan (Natural( x1 )) (Natural( y ))
            then findSplit x (0u :: y) (i-1)
            else (x0,x1,y)

        let (l0, l1, r) = findSplit left.Data right.Data (left.Data.Length - right.Data.Length)

        let rawDifferences = 0u :: (List.map2 (fun x y -> x - y) l1 r)
        let underflows = (List.map2 (fun  x y -> if y > x then 1u else 0u) l1 r) @ [0u]
        let cascadeUnderflows = (List.map2 (fun  x y -> if y > x then 1u else 0u) rawDifferences.Tail underflows.Tail) @ [0u]
        let result = List.map3 (fun x y z -> x - y - z ) rawDifferences underflows cascadeUnderflows

        Natural( List.append l0 (_compress result) )

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
        let multiplyBy10 x = _add (_leftShift 3 x) (_leftShift 1 x)

        let rec pow10 (e:Natural) : Natural =
            let isEven (x:'T when 'T :> INumberBase<'T>) =
                'T.IsEvenInteger( x )

            // Code Coverage: Cases 3u and 4u aren't hit.
            // These are trivial case, so I'm ok
            match e with
            | _ when e = Natural( [0u] ) -> Natural( [1u] )
            | _ when e = Natural( [1u] ) -> Natural( [10u] )
            | _ when e = Natural( [2u] ) -> Natural( [100u] )
            | _ when e = Natural( [3u] ) -> Natural( [1000u] )
            | _ when e = Natural( [4u] ) -> Natural( [10000u] )
            | _ when (isEven e) ->
                // do even code
                let half = _rightShift 1 e
                let result = pow10 half
                _multiply result result
            | _     ->
                // do odd code
                let half = _rightShift 1 e
                let result = pow10 half
                multiplyBy10 (_multiply result result)

        let powersOf2 = Seq.unfold (fun state -> Some( state, _leftShift 1 state )) Natural.Unit
        let powersOf10 = Seq.unfold (fun state -> Some( state, multiplyBy10 state )) Natural.Unit
        let powersOf16 = Seq.unfold (fun state -> Some( state, _leftShift 4 state )) Natural.Unit

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

            let decFactor = pow10 (Natural( [uint32 parseBuddy.Decimal.Length] ))
            let natWhole =
                parseBuddy.Decimal
                |> List.append parseBuddy.WholeNumber
                |> listToNatural

            let natExp = listToNatural parseBuddy.Exponent
            let expFactor = pow10 natExp

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
        let mask = UInt128( 0UL, 0xFFFF_FFFFUL )
        let down x : uint32 = UInt128.op_Explicit( ( data >>> x ) &&& mask )
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
            // This method contains a few gaurd checks for if a method can't be found through reflection
            // Nothing I send though here should catch those
            // As such, if you do a Code Coverage, they will show as unreached
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
                        .FirstOrDefault( fun m -> m.Name.EndsWith( "IsPositive" ) )

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
                        .FirstOrDefault( fun m -> m.Name.Contains( "IsInteger" ) )

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
                        .FirstOrDefault( fun m -> m.Name.EndsWith( "CreateChecked" ) )

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

        member private this.BaseToString( singleThreaded ) =
            let rec chunkListToArray n i : uint32 array =
                match n with
                | z when z = Natural.Zero ->
                    // We now know how big of any array we need, so just create it
                    Array.zeroCreate (i-1)
                | _ ->
                    // 1,000,000,000 is the largest power of 10 that fits in an uint32
                    let (q,r) = _divideModulo n (Natural([1_000_000_000u]))
                    let arr = chunkListToArray q (i+1)
                    arr.[arr.Length - i] <- r.Data.Head
                    arr

            if _equality Natural.Zero this
            then
                // Bailing out the degenerate case because it would
                // end up Trimming out to an empty string
                "0"
            else
                let unsighedArray = chunkListToArray this 1
                // Specifying the type here so it's not ambigious in String.Join
                let (stringArray:string array) =
                    if singleThreaded
                    then Array.map (fun (u:uint32) -> u.ToString( "D9" )) unsighedArray
                    else Array.Parallel.map (fun (u:uint32) -> u.ToString( "D9" )) unsighedArray
                String.Join( "", stringArray ).TrimStart( '0' )

        override this.ToString() =
            this.BaseToString( false )

        // NOTE: This mostly exists here so I can use it in DebuggerDisplay
        member internal this.SingleThreadedToString() : string =
            this.BaseToString( true )

        // NOTE: This mostly exists here so I can use it in DebuggerDisplay
        member this.ToString( format:string ) : string =
            (this :> IFormattable).ToString( format, _defaultFormatProvider )

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
                | :? BigInteger as bi ->
                    if bi < BigInteger.Zero
                    then 1
                    else doWork this (Natural.op_Explicit( bi ))
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

        // IComparisonOperators implements IEqualityOperators
        // So, even though INumberBase doesn't need IComparisonOperators
        // It helps the flow to do this early
        interface IComparisonOperators<Natural,Natural,bool> with
            static member op_GreaterThan( left, right ) =
                _greaterThan left right
            static member op_GreaterThanOrEqual( left, right ) =
                _equality left right || _greaterThan left right
            static member op_LessThan( left, right ) =
                _lessThan left right
            static member op_LessThanOrEqual( left, right ) =
                _equality left right || _lessThan left right

        interface IAdditionOperators<Natural,Natural,Natural> with
            static member (+) (left:Natural, right:Natural) : Natural = 
                _add left right
            static member op_CheckedAddition (left:Natural, right:Natural) : Natural = 
                IAdditionOperators<Natural,Natural,Natural>.op_CheckedAddition( left, right )

        interface IAdditiveIdentity<Natural,Natural> with
            static member AdditiveIdentity
                with get () = Natural.Zero

        interface IIncrementOperators<Natural> with
            static member op_CheckedIncrement ( value:Natural ) : Natural = 
                IIncrementOperators<Natural>.op_CheckedIncrement( value )
            static member op_Increment( value:Natural ) : Natural =
                _add value Natural.Unit

        interface ISubtractionOperators<Natural,Natural,Natural> with
            // Both of these throw an OverflowException
            // After a bit of research, I found an example in the Decimal type
            // A Decimal does not have a valid internal state on an Overflow/Underflow
            // As such, it always throws on an Overflow/Underflow
            static member op_CheckedSubtraction(left:Natural, right:Natural) : Natural = 
                ISubtractionOperators<Natural,Natural,Natural>.op_CheckedSubtraction( left, right )
            static member (-) (left:Natural, right:Natural) : Natural = 
                _subtract left right

        interface IDecrementOperators<Natural> with
            static member op_Decrement(value:Natural) : Natural =
                _subtract value Natural.Unit
            static member op_CheckedDecrement( value: Natural ): Natural = 
                IDecrementOperators<Natural>.op_CheckedDecrement( value )
 
        interface IUnaryNegationOperators<Natural,Natural> with
            static member op_CheckedUnaryNegation( value:Natural ) : Natural = 
                IUnaryNegationOperators<Natural,Natural>.op_CheckedUnaryNegation( value )
            static member (~-)( value: Natural ) : Natural = 
                raise ( System.OverflowException() )
        
        interface IUnaryPlusOperators<Natural,Natural> with
            static member (~+)( value:Natural ) : Natural = 
                Natural( value )

        interface IMultiplyOperators<Natural,Natural,Natural> with
            static member op_CheckedMultiply( left: Natural, right: Natural ) : Natural = 
                IMultiplyOperators<Natural,Natural,Natural>.op_CheckedMultiply( left, right )
            static member (*)( left:Natural, right:Natural ) : Natural = 
                _multiply left right

        interface IMultiplicativeIdentity<Natural,Natural> with
            static member MultiplicativeIdentity
                with get () = Natural.Unit

        interface IDivisionOperators<Natural,Natural,Natural> with
            static member op_CheckedDivision( left: Natural, right: Natural ) : Natural =
                IDivisionOperators<Natural,Natural,Natural>.op_CheckedDivision( left, right )
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
                    // NumberFormatInfo does a bounds check
                    // You can't set it outside the range
                    // However, F# needs a catch all
                    // As such, this code block is not covered because it is unreachable
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
                    // NumberFormatInfo does a bounds check
                    // You can't set it outside the range
                    // However, F# needs a catch all
                    // As such, this code block is not covered because it is unreachable
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
        
        interface IUtf8SpanFormattable with
            member this.TryFormat(utf8Destination: Span<byte>, bytesWritten: byref<int>, format: ReadOnlySpan<char>, provider: IFormatProvider): bool = 
                let mutable x = 0
                let formattedString = (this :> IFormattable).ToString( format.ToString(), provider )
 
                if utf8Destination.Length < formattedString.Length
                then 
                    bytesWritten <- utf8Destination.Length
                    System.Text.Unicode.Utf8.FromUtf16(
                        formattedString.Substring( 0, utf8Destination.Length ),
                        utf8Destination,
                        &x,
                        &bytesWritten,
                        true,
                        true
                    )
                    |> ignore

                    false
                else
                    bytesWritten <- formattedString.Length
                    System.Text.Unicode.Utf8.FromUtf16(
                        formattedString,
                        utf8Destination,
                        &x,
                        &bytesWritten,
                        true,
                        true
                    )
                    |> ignore

                    true

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
                    // This first one will show as negative in code coverage
                    // The base CreateChecked looks for matching types as returns the value
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
                        else None
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
                        // NOTE: This would be hit by something like "Integer"
                        // There is a test for it, which is currently skipped (because Integer isn't a INumberBase yet)
                        result <- Natural.Zero
                        true
                    | _ when 'TOther.IsInteger( value ) ->
                        result <- Natural.Parse( value.ToString( "R", null ), NumberStyles.Any )
                        true
                    | _ when 'TOther.IsRealNumber( value ) -> raise (System.OverflowException())
                    | _ ->
                        // I cannot think of a way to trigger this.
                        // However, if we ever did hit it, we definitely don't support it
                        false

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
                    | _ ->
                        // This is a catch-all
                        // I can't think of a type that will return Infinity that we can't handle
                        raise (System.OverflowException())

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
                // NOTE: If you do a test code coverage, a few paths are not hit
                // We're talking egde cases on edge cases.
                // I got lucky that what I set up hit the two opposing branches within float/double 
                // I'm not going to go through the mental gymnastics to hit them on both
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

        interface IUnsignedNumber<Natural>

        interface IComparable<Natural> with
            member this.CompareTo(other: Natural): int = 
                (this :> IComparable).CompareTo( other )

        interface IModulusOperators<Natural,Natural,Natural> with
            static member (%)(left: Natural, right: Natural): Natural = 
                let (_,r) = _divideModulo left right
                r

        interface INumber<Natural>

        interface IBitwiseOperators<Natural,Natural,Natural> with
            static member (&&&) ( left:Natural, right:Natural ) : Natural =
                _bitwiseAnd left right
            static member (|||) ( left:Natural, right:Natural ) : Natural =
                _bitwiseOr left right
            static member op_OnesComplement ( value:Natural ) : Natural =
                _bitwiseNot value
            static member (^^^) ( left:Natural, right:Natural ) : Natural =
                _bitwiseXor left right

        interface IBinaryNumber<Natural> with
            static member IsPow2 ( value:Natural ) : bool =
                0ul = (List.sum value.Data.Tail)
                &&
                1ul = (Helpers.NumberOfSetBits value.Data.Head)
            static member Log2 ( value:Natural ) : Natural =
                Natural( UInt32.Log2( value.Data.Head ) )
                +
                ( Natural( 32ul ) * Natural( uint32 value.Data.Tail.Length ) )
            
        interface IShiftOperators<Natural,int,Natural> with
            static member (<<<) ( left:Natural, right:int ) : Natural =
                _leftShift right left
            static member (>>>) ( left:Natural, right:int ) : Natural =
                _rightShift right left
            static member op_UnsignedRightShift( left:Natural, right:int ) : Natural =
                _rightShift right left
