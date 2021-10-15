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

    member internal Real.Significand = s
    member internal Real.Exponent = e

    //new(significand:uint32 list, exponent:uint32 list) = Real( Integer( significand ), Natural( exponent ) )
    //new(significand:uint32 list, exponent:uint32 list, negative:bool) = Real( Integer( significand, negative ), Natural( exponent ) )

    //new(significand:uint32 seq, exponent:uint32 seq) = Real( Integer( significand ), Natural( exponent ) )
    //new(significand:uint32 seq, exponent:uint32 seq, negative:bool) = Real( Integer( significand, negative ), Natural( exponent ) )

    new(significand:int32, exponent:int32) = Real( Integer( significand ), Integer( exponent ) )

    with
        static member Zero = Real( Integer.Zero, Integer.Zero )
        static member Unit = Real( Integer.Unit, Integer.Zero )
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

        // Comparison Operators
        static member op_Equality (left:Real, right:Real) : bool =
            // We internally normalize the number on construction
            // Thus, the parts just have to match
            // That is to say 43.21 could never be stored as both
            // 4321E-2 and 43210E-3
            (left.Significand = right.Significand) && (left.Exponent = right.Exponent)

        static member op_GreaterThan (left:Real, right:Real) : bool =
            raise (new System.NotImplementedException())
            let l = left.Significand * Integer( right.Exponent )
            let r = right.Significand * Integer( left.Exponent )
            l > r

        static member op_LessThan (left:Real, right:Real) : bool =
            raise (new System.NotImplementedException())
            let l = left.Significand * Integer( right.Exponent )
            let r = right.Significand * Integer( left.Exponent )
            l < r

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
                raise (new System.NotImplementedException())
                Real.op_Equality(left, right :?> Real)
            | t when t = typeof<Integer> ->
                left.Exponent.Equals( Natural.Zero ) && left.Significand.Equals( right )
            | t when t = typeof<Natural> ->
                left.Exponent.Equals( Natural.Zero ) && left.Significand.Equals( right )
            | _ -> false

        override this.GetHashCode() =
            raise (new System.NotImplementedException())

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
                raise (new System.NotImplementedException())
                match right.GetType() with
                | t when t = typeof<Real> ->
                    let r = right :?> Real
                    match Real.op_Equality(left, r) with
                    | true -> 0
                    | false ->
                        match Real.op_GreaterThan(left, r) with
                        | true -> 1
                        | false -> -1
                //| t when t = typeof<Integer> ->
                //    let r = Real( right :?> Integer, Natural.Unit )
                //    match Real.op_Equality(left, r) with
                //    | true -> 0
                //    | false ->
                //        match Real.op_GreaterThan(left, r) with
                //        | true -> 1
                //        | false -> -1
                //| t when t = typeof<Integer> ->
                //    let r = Real( right :?> Integer, Natural.Unit )
                //    match Real.op_Equality(left, r) with
                //    | true -> 0
                //    | false ->
                //        match Real.op_GreaterThan(left, r) with
                //        | true -> 1
                //        | false -> -1
                //| t when t = typeof<Natural> ->
                //    let r = Real( Integer( right :?> Natural ), Natural.Unit )
                //    match Real.op_Equality(left, r) with
                //    | true -> 0
                //    | false ->
                //        match Real.op_GreaterThan(left, r) with
                //        | true -> 1
                //        | false -> -1
                | _ -> raise (new System.ArgumentException())

        // Other things we need that require previous operators
        static member Parse (s:string) =
            if String.IsNullOrWhiteSpace s then raise (new System.ArgumentNullException())

            // TODO: Need to handle e?? in string
            // example: 1.1e6
            let parts = s.Split [| '.' |]
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
