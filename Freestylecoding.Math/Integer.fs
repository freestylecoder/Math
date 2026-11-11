namespace Freestylecoding.Math

open System
open System.Linq
open System.Numerics
open System.Globalization

// This gets rid of warnings for explicit interface calls
// Mostly a necessary evil because of the static interface calls
#nowarn "3536"

[<Diagnostics.DebuggerDisplay( "{SingleThreadedToString()}" )>]
type public Integer(data:uint list, negative:bool) =
    inherit Natural( data )

    static let _defaultNumberStyle = NumberStyles.Integer ||| NumberStyles.AllowThousands
    static let _defaultFormatProvider = CultureInfo.CurrentCulture.NumberFormat

    // NOTE: All the base operators are declared here
    // This allows us to have all the externally visible operators, interfaces, etc
    //   reference the same code for optimising/debugging purposes
    static let _bitwiseAnd (left:Integer) (right:Integer) : Integer =
        Integer(
            Natural( left.Data ) &&& Natural( right.Data ),
            left.Negative && right.Negative
        )

    static let _bitwiseOr (left:Integer) (right:Integer) : Integer =
        Integer(
            Natural( left.Data ) ||| Natural( right.Data ),
            left.Negative || right.Negative
        )

    static let _bitwiseXor (left:Integer) (right:Integer) : Integer =
        Integer(
            Natural( left.Data ) ^^^ Natural( right.Data ),
            ( left.Negative || right.Negative ) && not ( left.Negative && right.Negative )
        )

    static let _bitwiseNot (operand:Integer) : Integer =
        Integer(
            ~~~ Natural( operand.Data ),
            not operand.Negative
        )

    static let _leftShift (totalBitsToShift:int32) (operand:Integer) : Integer =
        Integer(
            Natural( operand.Data ) <<< totalBitsToShift,
            operand.Negative
        )

    static let _rightShift (totalBitsToShift:int32) (operand:Integer) : Integer =
        Integer(
            Natural( operand.Data ) >>> totalBitsToShift,
            operand.Negative
        )

    static let _equality (left:Integer) (right:Integer) : bool =
        left.Negative = right.Negative && Natural( left.Data ) = Natural( right.Data )

    static let _greaterThan (left:Integer) (right:Integer) : bool =
        match (left.Negative,right.Negative) with
        | (false,false) -> Natural( left.Data ) > Natural( right.Data )
        | (false,true) -> true
        | (true,false) -> false
        | (true,true) -> Natural( right.Data ) > Natural( left.Data )

    static let _lessThan (left:Integer) (right:Integer) : bool =
        match (left.Negative,right.Negative) with
        | (false,false) -> Natural( left.Data ) < Natural( right.Data )
        | (false,true) -> false
        | (true,false) -> true
        | (true,true) -> Natural( right.Data ) < Natural( left.Data )

    static let _add (left:Integer) (right:Integer) : Integer = 
        let naturalLeft = Natural( left.Data )
        let naturalRight = Natural( right.Data )
        // TODO: Refactor into one match
        match left.Negative = right.Negative with
        | true ->
            Integer( naturalLeft + naturalRight, left.Negative )
        | false ->
            match naturalLeft < naturalRight with
            | true -> Integer( naturalRight - naturalLeft, right.Negative )
            | false -> Integer( naturalLeft - naturalRight, left.Negative )

    static let _subtract (left:Integer) (right:Integer) : Integer =
        let naturalLeft = Natural( left.Data )
        let naturalRight = Natural( right.Data )
        // TODO: Refactor into one match
        match left.Negative = right.Negative with
        | false ->
            Integer( naturalLeft + naturalRight, left.Negative )
        | true ->
            if naturalLeft < naturalRight then
                Integer( naturalRight - naturalLeft, not left.Negative )
            else
                Integer( naturalLeft - naturalRight, left.Negative )

    static let _multiply (left:Integer) (right:Integer) : Integer =
        Integer(
            Natural( left.Data ) * Natural( right.Data ),
            Helpers.Xor left.Negative right.Negative
        )

    static let _divideModulo (left:Integer) (right:Integer) : Integer*Integer =
        let (q,r) = Natural(left.Data) /% Natural(right.Data)
        (
            Integer( q, Helpers.Xor left.Negative right.Negative ),
            Integer( r, left.Negative )
        )

    static let _parse (s:string) (style:NumberStyles) (provider:IFormatProvider) : Integer = 
        Integer( Natural.Parse( s.Trim().TrimStart( '-' ) ), s.Trim().StartsWith( "-" ) )

    static let _tryParse (s:string) (style:NumberStyles) (provider:IFormatProvider) (result:byref<Integer>) : bool =
        try
            result <- _parse s style provider
            true
        with _ ->
            result <- Integer.Zero
            false

    member internal Integer.Negative:bool = negative && ( data <> [0u] )

    new() = Integer( [0u], false )
    new(data:Integer) = Integer( data.Data, data.Negative )

    new(data:Natural) = Integer( data.Data, false )
    new(data:Natural, negative:bool) = Integer( data.Data, negative )

    new(data:int8 ) = Integer( [uint32 (SByte.Abs(data))], SByte.IsNegative( data ) )
    new(data:int16) = Integer( [uint32 (Int16.Abs(data))], Int16.IsNegative( data ) )
    new(data:int32) = Integer( [uint32 (Int32.Abs(data))], Int32.IsNegative( data ) )
    new(data:int64) =
        let magnitude = Convert.ToUInt64( Int64.Abs(data) )
        let split (l:UInt64) = [
            Convert.ToUInt32( l >>> 32 );
            Convert.ToUInt32( l &&& 0xFFFF_FFFFUL )
        ]
        Integer( (split magnitude), Int64.IsNegative(data) )
    new(data:Int128) =
        let magnitude = Int128.Abs( data )
        let neg = Int128.IsNegative( data )
        let mask = Int128( 0UL, 0xFFFF_FFFFUL )
        let down x : uint32 = Int128.op_Explicit( ( magnitude >>> x ) &&& mask )
        Integer(
            [
                down 96;
                down 64;
                down 32;
                down  0
            ],
            neg
        )

    new(data:uint32 list) = Integer( data, false )
    new(data:uint32 seq) = Integer( Seq.toList( data ) )
    new(data:uint32 seq, negative:bool) = Integer( Seq.toList( data ), negative )

    with
        static member Zero = Integer([0u])
        static member Unit = Integer([1u])
        static member NegativeOne = Integer([1u], true)

        // In bound implicit up casts
        static member op_Implicit( y:int8 ) : Integer =
            Integer( y )
        static member op_Implicit( s:int16 ) : Integer =
            Integer( s )
        static member op_Implicit( i:int32 ) : Integer =
            Integer( i )
        static member op_Implicit( L:int64 ) : Integer =
            Integer( L )
        static member op_Implicit( LL:Int128 ) : Integer =
            Integer( LL )
        static member op_Implicit( natural:Natural ) : Integer =
            Integer( natural )

        // Implicit side casts
        static member op_Implicit( bi:BigInteger ) : Integer =
            let magnitude = BigInteger.Abs( bi )
            let negative = BigInteger.IsNegative( bi )
            let bitMask = BigInteger( UInt32.MaxValue )

            let rec ChunkBigInteger (x:BigInteger) =
                if x > bitMask
                then
                    (uint32 (x &&& bitMask)) :: (ChunkBigInteger (x >>> 32))
                else
                    [ uint32 x ]

            Integer( (List.rev (ChunkBigInteger magnitude)), negative )
        static member op_Implicit( i:Integer ) : BigInteger =
            let bi = BigInteger(
                ReadOnlySpan<Byte>(
                    i.Data
                        |> List.map (fun ui -> List.map (fun b -> b &&& 0xFFu) [ ui >>> 24; ui >>> 16; ui >>> 8; ui])
                        |> List.collect (fun i -> i)
                        |> List.map (fun ui -> Convert.ToByte(ui))
                        |> List.toArray
                ),
                true,
                true
            )

            match i.Negative with
            | false -> bi
            | true -> -bi

        // Bitwise Operators
        static member (&&&) (left:Integer, right:Integer) : Integer =
            _bitwiseAnd left right

        static member (|||) (left:Integer, right:Integer) : Integer =
            _bitwiseOr left right

        static member (^^^) (left:Integer, right:Integer) : Integer =
            _bitwiseXor left right

        static member (~~~) (right:Integer) : Integer =
            _bitwiseNot right

        // This is here for C#
        static member op_OnesComplement(right:Integer) : Integer =
            _bitwiseNot right

        static member (<<<) (left:Integer, right:int) : Integer =
            _leftShift right left

        static member (>>>) (left:Integer, right:int) : Integer =
            _rightShift right left
 
        // Comparison Operators
        static member op_Equality (left:Integer, right:Integer) : bool =
            _equality left right
            
        static member op_GreaterThan (left:Integer, right:Integer) : bool =
            _greaterThan left right

        static member op_LessThan (left:Integer, right:Integer) : bool =
            _lessThan left right

        static member op_GreaterThanOrEqual (left:Integer, right:Integer) : bool =
            left = right || left > right

        static member op_LessThanOrEqual (left:Integer, right:Integer) : bool =
            left = right || left < right

        static member op_Inequality (left:Integer, right:Integer) : bool =
            _equality left right
            |> not

        // Arithmetic Operators
        // Binary
        static member (+) (left:Integer, right:Integer) : Integer =
            _add left right

        static member (-) (left:Integer, right:Integer) : Integer =
            _subtract left right

        static member (*) (left:Integer, right:Integer) : Integer =
            _multiply left right
            
        static member (/%) (left:Integer, right:Integer) : Integer*Integer =
            _divideModulo left right

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
        static member private Equals( this:Integer, that:obj ) =
            //// This method contains a few gaurd checks for if a method can't be found through reflection
            //// Nothing I send though here should catch those
            //// As such, if you do a Code Coverage, they will show as unreached
            //let thatType = that.GetType()

            //let IsNumberBase (t:Type) =
            //    t
            //        .GetInterfaces()
            //        .Select( fun t -> t.Name )
            //        .Any( fun s -> s.Contains( "INumberBase" ) )

            //let IsPositive (t:Type) =
            //    let method = 
            //        t
            //            .GetMethods(
            //                Reflection.BindingFlags.Public |||
            //                // If it's an unsigned type, IsPositive is private
            //                Reflection.BindingFlags.NonPublic |||
            //                Reflection.BindingFlags.Static |||
            //                Reflection.BindingFlags.FlattenHierarchy
            //            )
            //            .FirstOrDefault( fun m -> m.Name.EndsWith( "IsPositive" ) )

            //    match method with
            //    | null -> false
            //    | _ -> 
            //        match method.Invoke( null, [| that |] ) with
            //        | :? bool as b -> b
            //        | _ -> false

            //let IsInteger (t:Type) =
            //    let methods = 
            //        t
            //            .GetMethods(
            //                Reflection.BindingFlags.Public |||
            //                // If it's an integer type, IsInteger is private
            //                Reflection.BindingFlags.NonPublic |||
            //                Reflection.BindingFlags.Static |||
            //                Reflection.BindingFlags.FlattenHierarchy
            //            )
            //    let method = 
            //        methods
            //            .FirstOrDefault( fun m -> m.Name.Contains( "IsInteger" ) )

            //    match method with
            //    | null -> false
            //    | _ -> 
            //        match method.Invoke( null, [| that |] ) with
            //        | :? bool as b -> b
            //        | _ -> false

            //let CreateChecked (t:Type) =
            //    let genericMethod = 
            //        typeof<INumberBase<Integer>>
            //            .GetMethods()
            //            .FirstOrDefault( fun m -> m.Name.EndsWith( "CreateChecked" ) )

            //    let method =
            //        genericMethod.MakeGenericMethod( [| t.UnderlyingSystemType |])

            //    match method with
            //    | null -> Integer.Zero
            //    | _ -> 
            //        match method.Invoke( null, [| that |] ) with
            //        | :? Integer as n -> n
            //        | _ -> Integer.Zero

            //IsNumberBase thatType
            //&&
            //IsPositive thatType
            //&&
            //IsInteger thatType
            //&&
            //_equality this (CreateChecked thatType)

            // TODO: Fix this when I get INumberBase<Integer> added
            false

        override this.Equals( that:Object ) =
            // This required F#10
            // I still haven't completed the F#8 changes, so I'll loop back
            //#nowarn "3536"
            match that with
            | :? Integer    as  I  -> _equality this I
            | :? Natural    as  N  -> _equality this N
            | :? byte       as  b  -> _equality this (Natural(  b  ))
            | :? uint16     as us  -> _equality this (Natural( us  ))
            | :? uint32     as ui  -> _equality this (Natural( ui  ))
            | :? uint64     as uL  -> _equality this (Natural( uL  ))
            | :? UInt128    as uLL -> _equality this (Natural( uLL ))
            | :? sbyte      as sb  -> _equality this (Integer( sb  ))
            | :? int16      as  s  -> _equality this (Integer(  s  ))
            | :? int32      as  i  -> _equality this (Integer(  i  ))
            | :? int64      as  L  -> _equality this (Integer(  L  ))
            | :? Int128     as  LL -> _equality this (Integer(  LL ))
            | :? BigInteger as bi  -> _equality this bi
            | _ ->
                // We only worry about using reflection after we get the easy ones
                Integer.Equals( this, that )
            //#warnon "3536"

        override this.GetHashCode() =
            let v =
                List.rev this.Data
                |> List.head
            let x =
                match this.Negative with
                | false -> v
                | true -> ~~~v
            x.GetHashCode()

        override this.ToString() =
            let s = Natural( this.Data ).ToString()
            match this.Negative with
            | true -> "-" + s
            | false -> s

        member internal this.SingleThreadedToString() =
            let s = Natural( this.Data ).SingleThreadedToString()
            match this.Negative with
            | true -> "-" + s
            | false -> s

        // NOTE: This mostly exists here so I can use it in DebuggerDisplay
        member this.ToString( format:string ) : string =
            //(this :> IFormattable).ToString( format, _defaultFormatProvider )
            raise (NotImplementedException())

        // IComparable (for .NET) 
        interface IComparable with
            member this.CompareTo that = 
                let doWork right =
                    match _equality this right with
                    | true -> 0
                    | false ->
                        match _greaterThan this right with
                        | true -> 1
                        | false -> -1

                // This is only supposted to handle the exact same type
                // As such, I'm only handling the types I support implicit conversion from
                // This is already more than it should handle

                // This required F#10
                // I still haven't completed the F#8 changes, so I'll loop back
                //#nowarn "3536"
                match that with
                | :? Integer as I   -> doWork I
                | :? Natural as n   -> doWork n
                | :? Byte    as uy  -> doWork (Natural uy )
                | :? UInt16  as us  -> doWork (Natural us )
                | :? UInt32  as ui  -> doWork (Natural ui )
                | :? UInt64  as ul  -> doWork (Natural ul )
                | :? UInt128 as uLL -> doWork (Natural uLL)
                | :? SByte   as y   -> doWork (Integer y  )
                | :? Int16   as s   -> doWork (Integer s  )
                | :? Int32   as i   -> doWork (Integer i  )
                | :? Int64   as l   -> doWork (Integer l  )
                | :? Int128  as LL  -> doWork (Integer LL )
                | :? BigInteger as bi ->
                    doWork bi
                | _ -> raise (new ArgumentException( "obj is not the same type as this instance." ))
                //#warnon "3536"

        // Other things we need that require previous operators
        static member Parse (s:string) : Integer =
            _parse s _defaultNumberStyle _defaultFormatProvider
        static member Parse (s:string, style:System.Globalization.NumberStyles) : Integer =
            _parse s style _defaultFormatProvider

        static member TryParse( s:string, result:byref<Integer>) : bool =
            _tryParse s _defaultNumberStyle _defaultFormatProvider &result
        static member TryParse( s:ReadOnlySpan<Char>, result:byref<Integer>) : bool =
            _tryParse (s.ToString()) _defaultNumberStyle _defaultFormatProvider &result
        static member TryParse( s:ReadOnlySpan<Byte>, result:byref<Integer>) : bool =
            _tryParse (System.Text.Encoding.UTF8.GetString( s )) _defaultNumberStyle _defaultFormatProvider &result

        //interface IUnsignedNumber<Integer> with
        interface IEquatable<Integer> with
            member this.Equals( that:Integer ) : bool = 
                _equality this that

        interface IEqualityOperators<Integer,Integer,bool> with
            static member op_Inequality( left, right ) =
                not (_equality left right)
            static member op_Equality( left, right ) =
                _equality left right

        // IComparisonOperators implements IEqualityOperators
        // So, even though INumberBase doesn't need IComparisonOperators
        // It helps the flow to do this early
        interface IComparisonOperators<Integer,Integer,bool> with
            static member op_GreaterThan( left, right ) =
                _greaterThan left right
            static member op_GreaterThanOrEqual( left, right ) =
                _equality left right || _greaterThan left right
            static member op_LessThan( left, right ) =
                _lessThan left right
            static member op_LessThanOrEqual( left, right ) =
                _equality left right || _lessThan left right

