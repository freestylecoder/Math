namespace Freestylecoding.Math

open System
    
type public Real(significand:Integer, exponent:Integer) =
    let ten = Integer( 10 )
    let rec normalize s e =
        match s % ten with
        | x when x = Integer.Zero -> normalize (s/ten) (e+Integer.Unit)
        | _ -> (s, e)

    let (s, e) =
        if Integer.Zero = significand
        then ( Integer.Zero, Integer.Zero )
        else normalize significand exponent

    let internalParse (s:string) : Real =
        if String.IsNullOrWhiteSpace s then raise (new System.ArgumentNullException())

        let expSplit = s.Split( 'e', 'E' )
        let (sigStr,exp) = 
            match expSplit.Length with
            | 1 -> ( expSplit.[0], Integer.Zero )
            | 2 -> ( expSplit.[0], Integer.Parse( expSplit.[1] ) )
            | _ -> raise (new FormatException())

        let parts = sigStr.Split '.'
        let signif =
            match parts.Length with
            | 1 ->
                Real( Integer.Parse( parts.[0] ), Integer.Zero )
            | 2 ->
                if Integer.Parse( parts.[1] ).Negative then raise (new FormatException())
                Real(
                    Integer.Parse( $"{parts.[0]}{parts.[1]}" ),
                    -Integer( parts.[1].Length )
                )
            | _ ->
                raise (new FormatException())

        signif * Real( Integer.Unit, exp )
            
    member internal Real.Significand = s
    member internal Real.Exponent = e

    new( value:Real ) = Real( value.Significand, value.Exponent )
    new( value:Real, exp:Natural ) = Real( value * Real( Integer.Unit, exp ) )
    new( value:Real, exp:Integer ) = Real( value * Real( Integer.Unit, exp ) )

    new( significand:Natural ) =                   Real( Integer( significand ), Integer.Zero )
    new( significand:Natural, exponent:Integer ) = Real( Integer( significand ), exponent )
    new( significand:Natural, exponent:Natural ) = Real( Integer( significand ), Integer( exponent ) )

    new( significand:Integer ) =                   Real( significand, Integer.Zero )
    new( significand:Integer, exponent:Natural ) = Real( significand, Integer( exponent ) )

    new( significand:int32, exponent:int32 ) =   Real( Integer( significand ), Integer( exponent ) )
    new( significand:int32, exponent:uint32 ) =  Real( Integer( significand ), Natural( exponent ) )
    new( significand:uint32, exponent:int32 ) =  Real( Natural( significand ), Integer( exponent ) )
    new( significand:uint32, exponent:uint32 ) = Real( Natural( significand ), Natural( exponent ) )

    new( significand:Rational ) = Real( Real( significand.Numerator ) / Real( significand.Denominator ) )
    new( significand:Rational, exponent:Integer ) = Real( Real( significand.Numerator ) / Real( significand.Denominator ), exponent )
    new( significand:Rational, exponent:Natural ) = Real( Real( significand.Numerator ) / Real( significand.Denominator ), exponent )

    new( float:float32 ) =
        let s = float.ToString( "R" )
        let parts = s.Split '.'

        match parts.Length with
        | 1 ->
            Real( Integer.Parse( parts.[0] ), Integer.Zero )
        | _ ->
            if Integer.Parse( parts.[1] ).Negative then raise (new FormatException())
            Real(
                Integer.Parse( $"{parts.[0]}{parts.[1]}" ),
                -Integer( parts.[1].Length )
            )

    new( double:float ) =
        let s = double.ToString( "R" )
        let parts = s.Split '.'

        match parts.Length with
        | 1 ->
            Real( Integer.Parse( parts.[0] ), Integer.Zero )
        | _ ->
            if Integer.Parse( parts.[1] ).Negative then raise (new FormatException())
            Real(
                Integer.Parse( $"{parts.[0]}{parts.[1]}" ),
                -Integer( parts.[1].Length )
            )


    with
        static member Zero = Real( Integer.Zero, Integer.Zero )
        static member Unit = Real( Integer.Unit, Integer.Zero )
        static member One = Real( Integer.Unit, Integer.Zero )

        static member E = Real( Natural( [34u; 1329465128u; 679922920u; 604288761u] ), Integer( -30 ) )
 
        static member internal MaxPrecision = 30

        static member op_Implicit(integer:Integer) : Real =
            Real( integer, Integer.Zero )

        // This has no base case if exp < 0
        static member private ExpandExp( exp: Integer ) : Integer =
            let rec f e =
                match e with
                | _ when e = Integer.Zero ->
                    Integer.Unit
                | _ when e = Integer.Unit ->
                    Integer( 10 )
                | _ ->
                    Integer( 10 ) * ( f ( e - Integer.Unit ) )

            f exp

        static member private Truncate( value: Real ) : Real =
            if value.Exponent < Integer.Zero
            then Real( value.Significand / Real.ExpandExp( -value.Exponent ), Integer.Zero )
            else value

        static member private PrecisionFactor = Real.ExpandExp( Integer( Real.MaxPrecision ) )

        static member private Abs( value:Real ) : Real =
            Real( Integer( value.Significand.Data, false ), value.Exponent )

        // Comparison Operators
        static member op_Equality (left:Real, right:Real) : bool =
            // We internally normalize the number on construction
            // Thus, the parts just have to match
            // That is to say 43.21 could never be stored as both
            // 4321E-2 and 43210E-3
            (left.Significand = right.Significand) && (left.Exponent = right.Exponent)

        static member op_GreaterThan (left:Real, right:Real) : bool =
            match left.Exponent - right.Exponent with
            | n when n < Integer.Zero ->
                left.Significand > ( right.Significand * Real.ExpandExp( right.Exponent - left.Exponent ) )
            | p when p > Integer.Zero ->
                ( left.Significand * Real.ExpandExp( left.Exponent - right.Exponent ) ) > right.Significand
            | _ ->
                left.Significand > right.Significand

        static member op_LessThan (left:Real, right:Real) : bool =
            match left.Exponent - right.Exponent with
            | n when n < Integer.Zero ->
                left.Significand < ( right.Significand * Real.ExpandExp( right.Exponent - left.Exponent ) )
            | p when p > Integer.Zero ->
                ( left.Significand * Real.ExpandExp( left.Exponent - right.Exponent ) ) < right.Significand
            | _ ->
                left.Significand < right.Significand

        static member op_GreaterThanOrEqual (left:Real, right:Real) : bool =
            left = right || left > right

        static member op_LessThanOrEqual (left:Real, right:Real) : bool =
            left = right || left < right

        static member op_Inequality (left:Real, right:Real) : bool =
            not ( left = right )

        // Arithmetic Operators
        // Binary
        static member (+) (left:Real, right:Real) : Real =
            match left.Exponent - right.Exponent with
            | positive when positive > Integer.Zero ->
                let newLeft = left.Significand * Real.ExpandExp( positive )
                Real( newLeft + right.Significand, right.Exponent )
            | negative when negative < Integer.Zero ->
                let newRight = right.Significand * Real.ExpandExp( -negative )
                Real( left.Significand + newRight, left.Exponent )
            | _ ->
                Real( left.Significand + right.Significand, left.Exponent )

        static member (-) (left:Real, right:Real) : Real =
            left + ( -right )

        static member (*) (left:Real, right:Real) : Real =
            Real(
                left.Significand * right.Significand,
                left.Exponent + right.Exponent
            )

        static member (/) (left:Real, right:Real) : Real =
            // Sorry, not sorry
            match left with
            | z when z = Real.Zero -> Real.Zero
            | _ ->
                // The fudgeFactor helps me get rid of decimals in the denominator
                // I multiply out the demon to a whole number before begining
                // Then, divide it back out at the end
                let fudgeFactor = 
                    if( right.Exponent < Integer.Zero )
                    then Real.ExpandExp( -right.Exponent )
                    else Integer.Unit

                let ( result, resultRemainder ) =
                    if( right.Exponent < Integer.Zero )
                    then ( left.Significand * fudgeFactor ) /% right.Significand
                    else left.Significand /% right.Significand

                let booya =
                    match resultRemainder with
                    | z when z = Integer.Zero -> Integer.Zero
                    | _ ->
                        let ( remainderResult, remainderRemainder ) =
                            ( resultRemainder * Real.PrecisionFactor )
                            /%
                            right.Significand
                        let rr = Integer( remainderRemainder.Data, result.Negative )
                        
                        if ( rr <<< 1 ) < Integer( right.Significand.Data, false )
                        then remainderResult
                        else
                            if result.Negative
                            then remainderResult - Integer.Unit
                            else remainderResult + Integer.Unit

                Real(
                    ( ( result * Real.PrecisionFactor ) + booya ) / fudgeFactor,
                    left.Exponent - right.Exponent - Integer( Real.MaxPrecision )
                )

        static member (/%) (left:Real, right:Real) : Real*Real =
            let q = left / right
            
            match q.Exponent with
            | e when e < Integer.Zero -> (q, left - ( right * Real.Truncate( q ) ))
            | _ -> (q, Real.Zero)

        static member (%) (left:Real, right:Real) : Real =
            let (_,r) = left /% right
            r

        // See https://youtu.be/cbGB__V8MNk for where I got this idea
        static member Pow (``base``:Real, exp:Natural) : Real =
            let rec SquareMultiply (innerBase:Real) (innerExp:Natural) =
                match innerExp with
                | z when z = Natural.Unit -> innerBase
                | _ ->
                    let previous = SquareMultiply innerBase (innerExp >>> 1)
                    if Natural.Zero = ( innerExp &&& Natural.Unit )
                    then previous * previous
                    else ( previous * previous ) * ``base``

            match exp with
            | z when z = Natural.Zero -> Real.Unit
            | u when u = Natural.Unit -> ``base``
            | _ -> SquareMultiply ``base`` exp
(*
        static member ECalc ( precision:Integer ) : Real =
            let precisionFactor = Real( Real.ExpandExp( precision ) )
            let rec series (index:Natural) (factorial:Natural) (guess:Real) =
                let f = index * factorial
                let iter = guess + ( Real.Unit / Real( f ) )
                if Real( Integer.Unit, ( -precision ) ) > Real.Abs( iter - guess )
                    then Real.Truncate( iter * precisionFactor ) / precisionFactor
                    else series (index + Natural.Unit) f iter

            series (Natural( 2u )) Natural.Unit (Real( 2, 0 ))

        static member Exp (x:Real) : Real =
            let rec series (index:Natural) (factorial:Natural) (power:Real) (guess:Real) =
                let f = index * factorial
                let p = power * x
                let iter = guess + ( p / Real( f ) )
                if Real( Integer.Unit, Integer( -Real.MaxPrecision ) ) > Real.Abs( iter - guess )
                    then Real.Truncate( iter * Real( Real.PrecisionFactor ) ) / Real( Real.PrecisionFactor )
                    else series (index + Natural.Unit) f p iter

            series (Natural( 2u )) Natural.Unit x (Real.Unit + x)

        static member LogN (x:Real) : Real =
            let rec NewtonsMethod (guess:Real) =
                let e = Real.Exp( guess )
                let iter = guess + ( Real( 2, 0 ) * ( ( x - e ) / ( x + e ) ) )
                if Real( Integer.Unit, Integer( -Real.MaxPrecision ) ) > Real.Abs( iter - guess )
                    then Real.Truncate( iter * Real( Real.PrecisionFactor ) ) / Real( Real.PrecisionFactor )
                    else NewtonsMethod iter

            NewtonsMethod Real.Zero

        static member Root(index:Natural, radicand:Real) : Real =
            let a = Real( index - Natural.Unit ) / Real( index )
            let b = radicand / Real( index )
            
            let rec NewtonsMethod (guess:Real) =
                let iter = ( a * guess ) + ( b / Real.Pow( guess, index - Natural.Unit ) )
                if Real( Integer.Unit, Integer( -Real.MaxPrecision ) ) > Real.Abs( iter - guess )
                    then Real.Truncate( iter * Real( Real.PrecisionFactor ) ) / Real( Real.PrecisionFactor )
                    else NewtonsMethod iter

            //let guess = Real.Unit
            //let iter1 = ( a * guess ) + ( b / Real.Pow( guess, index - Natural.Unit ) )
            //let iter2 = ( a * iter1 ) + ( b / Real.Pow( iter1, index - Natural.Unit ) )
            //let iter3 = ( a * iter2 ) + ( b / Real.Pow( iter2, index - Natural.Unit ) )
            //let iter4 = ( a * iter3 ) + ( b / Real.Pow( iter3, index - Natural.Unit ) )
            //let iter5 = ( a * iter4 ) + ( b / Real.Pow( iter4, index - Natural.Unit ) )
            //iter5
            NewtonsMethod Real.Unit

        static member Pow (``base``:Real, exp:Real) : Real =
            if Real.Zero > ``base``
            then raise (System.ArgumentOutOfRangeException( "base", "Base must be positive" ))

            let partial = Real.Pow( ``base``, Natural( exp.Significand.Data ) )
            partial * Real.Root( Real.ExpandExp( -exp.Exponent ), ``base`` )
*)
        // Unary
        static member (~-) (input:Real) : Real =
            Real(
                -input.Significand,
                input.Exponent
            )

        // .NET Object Overrides
        override left.Equals( right ) =
            match right.GetType() with
            | t when t = typeof<Real> ->
                Real.op_Equality(left, right :?> Real)
            | t when t = typeof<Rational> ->
                let c = right :?> Rational
                let r = Real( c.Numerator, Integer.Zero ) / Real( Integer( c.Denominator ), Integer.Zero )
                Real.op_Equality(left, r)
            | t when t = typeof<Integer> ->
                left.Exponent.Equals( Integer.Zero ) && left.Significand.Equals( right )
            | t when t = typeof<Natural> ->
                left.Exponent.Equals( Integer.Zero ) && left.Significand.Equals( right )
            | _ -> false

        override this.GetHashCode() =
            this.Significand.GetHashCode() ^^^ this.Exponent.GetHashCode()

        override this.ToString() =
            match this.Exponent with
            | p when p > Integer.Zero ->
                let zeros = Real.ExpandExp( this.Exponent ).ToString().Substring( 1 )
                $"{this.Significand}{zeros}"
            | n when n < Integer.Zero ->
                let digits =
                    if this.Significand.Negative
                    then Integer( this.Significand.Data, false ).ToString()
                    else this.Significand.ToString()
                
                let sign = if this.Significand.Negative then "-" else String.Empty
                
                match Integer( digits.Length ) + n with
                | a when a > Integer.Zero ->
                    let whole = Real.Truncate( this )
                    let decimal = ( this - whole ).ToString()
                    $"{whole}.{decimal.Substring( decimal.IndexOf( '.' ) + 1 )}"
                | a when a < Integer.Zero ->
                    $"{sign}0.{Real.ExpandExp( -a ).ToString().Substring( 1 )}{digits}"
                | _ ->
                    $"{sign}0.{digits}"
            | _ -> // z when z = Integer.Zero
                this.Significand.ToString()

        // IComparable (for .NET) 
        interface IComparable with
            // TODO: Add support for the other types
            member left.CompareTo right = 
                let result r =
                    match Real.op_Equality(left, r) with
                    | true -> 0
                    | false ->
                        match Real.op_GreaterThan(left, r) with
                        | true -> 1
                        | false -> -1

                match right.GetType() with
                | t when t = typeof<Real> ->
                    result ( right :?> Real )
                | t when t = typeof<Rational> ->
                    let r = right :?> Rational
                    result ( Real( r.Numerator, Integer.Zero ) / Real( Integer( r.Denominator ), Integer.Zero ) )
                | t when t = typeof<Integer> ->
                    result ( Real( right :?> Integer, Integer.Zero) )
                | t when t = typeof<Natural> ->
                    result ( Real( Integer( right :?> Natural ), Integer.Zero) )
                | t when t = typeof<int> ->
                    result ( Real( Integer( right :?> int ), Integer.Zero) )
                | _ -> raise (new System.InvalidCastException())

        // Other things we need that require previous operators
        static member Parse (s:string) =
            if String.IsNullOrWhiteSpace s then raise (new System.ArgumentNullException())

            let expSplit = s.Split( 'e', 'E' )
            let (sigStr,exp) = 
                match expSplit.Length with
                | 1 -> ( expSplit.[0], Integer.Zero )
                | 2 -> ( expSplit.[0], Integer.Parse( expSplit.[1] ) )
                | _ -> raise (new FormatException())

            let parts = sigStr.Split '.'
            let signif =
                match parts.Length with
                | 1 ->
                    Real( Integer.Parse( parts.[0] ), Integer.Zero )
                | 2 ->
                    if Integer.Parse( parts.[1] ).Negative then raise (new FormatException())
                    Real(
                        Integer.Parse( $"{parts.[0]}{parts.[1]}" ),
                        -Integer( parts.[1].Length )
                    )
                | _ ->
                    raise (new FormatException())

            signif * Real( Integer.Unit, exp )