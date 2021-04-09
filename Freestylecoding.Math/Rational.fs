namespace Freestylecoding.Math

open System
    
type public Rational(numerator:Integer, denominator:Natural) =
    let rec GCD (a:Natural) (b:Natural) =
        match b with
        | x when x = Natural.Zero -> a
        | x when x > a -> GCD b a
        | x -> GCD x (a%b)

    let commonDivisor = GCD ( Natural( numerator.Data ) ) denominator
    let n = numerator / Integer( commonDivisor )
    let d = denominator / commonDivisor

    do
        if Natural.Zero = denominator then raise ( System.DivideByZeroException() )

    member internal Rational.Numerator = n
    member internal Rational.Denominator = d

    new(numerator:uint32 list, denominator:uint32 list) = Rational( Integer( numerator ), Natural( denominator ) )
    new(numerator:uint32 list, denominator:uint32 list, negative:bool) = Rational( Integer( numerator, negative ), Natural( denominator ) )

    new(numerator:uint32 seq, denominator:uint32 seq) = Rational( Integer( numerator ), Natural( denominator ) )
    new(numerator:uint32 seq, denominator:uint32 seq, negative:bool) = Rational( Integer( numerator, negative ), Natural( denominator ) )

    new(numerator:int32, denominator:uint32) = Rational( Integer( numerator ), Natural( denominator ) )

    with
        static member Zero = Rational( Integer.Zero, Natural.Unit )
        static member Unit = Rational( Integer.Unit, Natural.Unit )

        static member private Inverse( rational: Rational ) : Rational =
            Rational(
                Integer( rational.Denominator, rational.Numerator.Negative ),
                Natural( rational.Numerator.Data )
            )

        static member op_Implicit(integer:Integer) : Rational =
            Rational( integer, Natural.Unit )

        // Bitwise Operators
        static member (&&&) (left:Rational, right:Rational) : Rational =
            raise (new System.NotImplementedException())

        static member (|||) (left:Rational, right:Rational) : Rational =
            raise (new System.NotImplementedException())

        static member (^^^) (left:Rational, right:Rational) : Rational =
            raise (new System.NotImplementedException())

        static member (~~~) (right:Rational) : Rational =
            raise (new System.NotImplementedException())

        static member (<<<) (left:Rational, (right:int)) : Rational =
            raise (new System.NotImplementedException())

        static member (>>>) (left:Rational, right:int) : Rational =
            raise (new System.NotImplementedException())
 
        // Comparison Operators
        static member op_Equality (left:Rational, right:Rational) : bool =
            ( left.Denominator = right.Denominator ) && ( left.Numerator = right.Numerator )

        static member op_GreaterThan (left:Rational, right:Rational) : bool =
            raise (new System.NotImplementedException())

        static member op_LessThan (left:Rational, right:Rational) : bool =
            raise (new System.NotImplementedException())

        static member op_GreaterThanOrEqual (left:Rational, right:Rational) : bool =
            raise (new System.NotImplementedException())

        static member op_LessThanOrEqual (left:Rational, right:Rational) : bool =
            raise (new System.NotImplementedException())

        static member op_Inequality (left:Rational, right:Rational) : bool =
            raise (new System.NotImplementedException())

        // Arithmetic Operators
        // Binary
        static member (+) (left:Rational, right:Rational) : Rational =
            Rational(
                (left.Numerator * Integer( right.Denominator )) + (right.Numerator * Integer( left.Denominator )),
                left.Denominator * right.Denominator
            )

        static member (-) (left:Rational, right:Rational) : Rational =
            left + ( -right )

        static member (*) (left:Rational, right:Rational) : Rational =
            Rational(
                left.Numerator * right.Numerator,
                left.Denominator * right.Denominator
            )
           
        static member (/) (left:Rational, right:Rational) : Rational =
            left * Rational.Inverse( right )

        static member (/%) (left:Rational, right:Rational) : Integer*Rational =
            let p = left / right
            let (q,r) = p.Numerator /% Integer( p.Denominator )
            (q, Rational( r, p.Denominator ) )

        static member (%) (left:Rational, right:Rational) : Rational =
            let (_,r) = left /% right
            r

        // Unary
        static member (~-) (input:Rational) : Rational =
            Rational(
                -input.Numerator,
                input.Denominator
            )

        // .NET Object Overrides
        override left.Equals( right ) =
            match right.GetType() with
            | t when t = typeof<Rational> ->
                Rational.op_Equality(left, right :?> Rational)
            | t when t = typeof<Integer> ->
                left.Denominator.Equals( Natural.Unit ) && left.Numerator.Equals( right )
            | t when t = typeof<Natural> ->
                left.Denominator.Equals( Natural.Unit ) && left.Numerator.Equals( right )
            | _ -> false

        override this.GetHashCode() =
            raise (new System.NotImplementedException())

        override this.ToString() =
            match this.Denominator with
            | x when x = Natural.Unit -> $"{this.Numerator}"
            | _ -> $"{this.Numerator} / {this.Denominator}"

        // IComparable (for .NET) 
        interface IComparable with
            member left.CompareTo right = 
                raise (new System.NotImplementedException())

        // Other things we need that require previous operators
        static member Parse (s:string) =
            if String.IsNullOrWhiteSpace s then raise (new System.ArgumentNullException())

            let parts = s.Split [| '/' |]
            let split =
                match parts.Length with
                | 1 -> [| parts.[0]; "1" |]
                | 2 -> parts
                | _ -> raise (new FormatException())

            let numerator = Integer.Parse( split.[0].Trim() )
            let denominator = Integer.Parse( split.[1].Trim() )

            Rational(
                Integer(
                    numerator.Data,
                    Helpers.Xor numerator.Negative denominator.Negative
                ),
                Natural( denominator.Data )
            )
