namespace Freestylecoding.Math

open System
open System.Globalization
open System.Text.RegularExpressions

// Warning for "experimental Parallel feature"
#nowarn "57"

type internal ParseBuddy( style:NumberStyles, formatInfo:NumberFormatInfo ) =
    let isUnicodeDecimalDigit (c:char) =
        UnicodeCategory.DecimalDigitNumber = CharUnicodeInfo.GetUnicodeCategory( c )

    let hasSymbol (symbol:string) (value:string) : bool =
        let count = (value.Length - value.Replace( symbol, String.Empty ).Length) / symbol.Length
        match count with
        | 0 -> false
        | 1 -> true
        | _ -> raise (System.FormatException())

    let verifyStringIsClean (str:string) : string =
        if Array.Parallel.exists (fun c -> not (isUnicodeDecimalDigit( c ))) (str.ToCharArray())
        then raise (System.FormatException())

        str

    let hasFlag (flag:NumberStyles) : bool =
        flag = (style &&& flag)

    let allowLeadingWhite   = hasFlag( NumberStyles.AllowLeadingWhite )
    let allowTrailingWhite  = hasFlag( NumberStyles.AllowTrailingWhite )
    let allowLeadingSign    = hasFlag( NumberStyles.AllowLeadingSign )
    let allowTrailingSign   = hasFlag( NumberStyles.AllowTrailingSign )
    let allowParentheses    = hasFlag( NumberStyles.AllowParentheses )
    let allowDecimalPoint   = hasFlag( NumberStyles.AllowDecimalPoint )
    let allowThousands      = hasFlag( NumberStyles.AllowThousands )
    let allowExponent       = hasFlag( NumberStyles.AllowExponent )
    let allowCurrencySymbol = hasFlag( NumberStyles.AllowCurrencySymbol )

    let getRegexString groupSeparator decimalSeparator =
        let group = Regex.Escape( groupSeparator )
        let decimal = Regex.Escape( decimalSeparator )

        let positive = Regex.Escape( formatInfo.PositiveSign )
        let negative = Regex.Escape( formatInfo.NegativeSign ).Replace( "-", "\-" )
        let currency = Regex.Escape( formatInfo.CurrencySymbol )

        let group1 = String.Format(
                "([{0}{1}{2}\a]*)",
                (if allowParentheses then "(" else String.Empty),
                (if allowCurrencySymbol then currency else String.Empty),
                (if allowLeadingSign then $"{positive}{negative}" else String.Empty)
            )

        let group2 = String.Format(
                "([\d{0}]+)",
                (if allowThousands then group else String.Empty)
            )

        let group3 =
            if allowDecimalPoint
            then $"(?:{decimal})?([\d]*)"
            else "([\a]*)"

        let group4 =
            if allowExponent
            then "[eE]?([+\-]?)"
            else "([\a]*)"

        let group5 =
            if allowExponent
            then "([\d]*)"
            else "([\a]*)"

        let group6 = String.Format(
                "([{0}{1}{2}\a]*)",
                (if allowParentheses then ")" else String.Empty),
                (if allowCurrencySymbol then currency else String.Empty),
                (if allowTrailingSign then $"{positive}{negative}" else String.Empty)
            )

        String
            .Format(
                "^{0}{1}{2}{3}{4}{5}{6}{7}\0*$",
                (if allowLeadingWhite then "\s*" else String.Empty),
                group1,
                group2,
                group3,
                group4,
                group5,
                group6,
                (if allowTrailingWhite then "\s*" else String.Empty)
            )

    let parseSigns signs = 
        let parenNeg =
            match (hasSymbol "(" signs, hasSymbol ")" signs) with
            | (true, true ) -> true
            | (false,false) -> false
            | _ -> raise (FormatException())

        let isNegative =
            match (hasSymbol formatInfo.NegativeSign signs, parenNeg) with
            | (true,false)
            | (false,true)
                -> true
            | (false,false)
                -> false
            | _ ->
                raise (FormatException())

        if isNegative && hasSymbol formatInfo.PositiveSign signs
        then raise (FormatException())

        isNegative

    member val IsCurrency = false with get, set
    member val IsNegative = false with get, set
    member val IsExpNegative = false with get, set

    member val WholeNumber = String.Empty with get, set
    member val Decimal = String.Empty with get, set
    member val Exponent = String.Empty with get, set

    member internal this.Parse( str:string ) : unit =
        if allowCurrencySymbol && (hasSymbol formatInfo.CurrencySymbol str)
        then this.IsCurrency <- true

        let groupSeparator =
            if this.IsCurrency
            then formatInfo.CurrencyGroupSeparator
            else formatInfo.NumberGroupSeparator

        let decimalSeparator =
            if this.IsCurrency
            then formatInfo.CurrencyDecimalSeparator
            else formatInfo.NumberDecimalSeparator

        let groups = Regex( getRegexString groupSeparator decimalSeparator ).Match( str ).Groups
        if( 7 <> groups.Count ) then raise (System.FormatException())

        this.IsNegative <-
            String.Concat( groups.[1].Value, groups.[6].Value )
            |> parseSigns
        this.IsExpNegative <- "-" = groups.[4].Value

        this.Exponent <- verifyStringIsClean groups.[5].Value
        this.Decimal <- verifyStringIsClean groups.[3].Value
        this.WholeNumber <-
            groups.[2].Value.Replace( groupSeparator, String.Empty )
            |> verifyStringIsClean
