namespace Freestylecoding.Math

open System
open System.Globalization

type internal ParseBuddy( style:NumberStyles, formatInfo:NumberFormatInfo ) =
    let isUnicodeDecimalDigit (c:char) =
        UnicodeCategory.DecimalDigitNumber = CharUnicodeInfo.GetUnicodeCategory( c )

    let hasSymbol (symbol:ReadOnlySpan<'T>) (span:ReadOnlySpan<'T>) : bool =
        match span.Count( symbol ) with
        | 0 -> false
        | 1 -> true
        | _ -> raise (System.FormatException())

    let hasSymbol (symbol:string) (span:ReadOnlySpan<char>) : bool =
            hasSymbol (symbol.AsSpan()) span

    member internal this.HasFlag (flag:NumberStyles) : bool =
        flag = (this.Style &&& flag)

    member internal this.Style = style
    member internal this.FormatInfo = formatInfo

    member internal this.AllowLeadingWhite   = this.HasFlag( NumberStyles.AllowLeadingWhite )
    member internal this.AllowTrailingWhite  = this.HasFlag( NumberStyles.AllowTrailingWhite )
    member internal this.AllowLeadingSign    = this.HasFlag( NumberStyles.AllowLeadingSign )
    member internal this.AllowTrailingSign   = this.HasFlag( NumberStyles.AllowTrailingSign )
    member internal this.AllowParentheses    = this.HasFlag( NumberStyles.AllowParentheses )
    member internal this.AllowDecimalPoint   = this.HasFlag( NumberStyles.AllowDecimalPoint )
    member internal this.AllowThousands      = this.HasFlag( NumberStyles.AllowThousands )
    member internal this.AllowExponent       = this.HasFlag( NumberStyles.AllowExponent )
    member internal this.AllowCurrencySymbol = this.HasFlag( NumberStyles.AllowCurrencySymbol )

    member val IsCurrency = false with get, set
    member val IsNegative = false with get, set
    member val IsExpNegative = false with get, set

    member val WholeNumber = [] with get, set
    member val Decimal = [] with get, set
    member val Exponent = [] with get, set

    member private this.ParseWhitespace( span:ReadOnlySpan<char> ) : ReadOnlySpan<char> = 
        match (this.AllowLeadingWhite, this.AllowTrailingWhite) with
        | (true, true) -> span.Trim()
        | (true, false) -> span.TrimStart()
        | (false, true) -> span.TrimEnd()
        | (false, false) -> span

    member private this.ParseSigns( span:ReadOnlySpan<char> ) : ReadOnlySpan<char> = 
        let firstDigit = span.IndexOf ( fun c -> isUnicodeDecimalDigit c )
        let lastDigit = span.LastIndexOf ( fun c -> isUnicodeDecimalDigit c )

        let leading = span.Slice( 0, firstDigit )        
        if leading.ContainsAnyExcept( $"{formatInfo.PositiveSign}{formatInfo.NegativeSign}{formatInfo.CurrencySymbol}(".AsSpan() )
        then raise (System.FormatException())

        // That null character is there because of Utf8 (byte) encoding
        // If a character can't convert, we get extra null characters
        // this strips it off
        let trailing = span.Slice( lastDigit + 1 )
        if trailing.ContainsAnyExcept( $"{formatInfo.PositiveSign}{formatInfo.NegativeSign}{formatInfo.CurrencySymbol}\u0000)".AsSpan() )
        then raise (System.FormatException())

        let leadingPositive = hasSymbol formatInfo.PositiveSign leading
        let leadingNegative = hasSymbol formatInfo.NegativeSign leading
        let leadingCurrency = hasSymbol formatInfo.CurrencySymbol leading
        let leadingParentheses = hasSymbol "(" leading

        let trailingPositive = hasSymbol formatInfo.PositiveSign trailing
        let trailingNegative = hasSymbol formatInfo.NegativeSign trailing
        let trailingCurrency = hasSymbol formatInfo.CurrencySymbol trailing
        let trailingParentheses = hasSymbol ")" trailing

        let parenNeg =
            match ( this.AllowParentheses, leadingParentheses, trailingParentheses ) with
            | (true,true,true)
                -> true
            | (_,false,false)
                -> false
            | _ ->
                raise (FormatException())
        let leadNeg =
            match (this.AllowLeadingSign, leadingNegative) with
            | (true,true) -> true
            | (false,true) -> raise (FormatException())
            | _ -> false
        let trailNeg =
            match (this.AllowTrailingSign, trailingNegative) with
            | (true,true) -> true
            | (false,true) -> raise (FormatException())
            | _ -> false
        let leadPos =
            match (this.AllowLeadingSign, leadingPositive) with
            | (true,true) -> true
            | (false,true) -> raise (FormatException())
            | _ -> false
        let trailPos =
            match (this.AllowTrailingSign, trailingPositive) with
            | (true,true) -> true
            | (false,true) -> raise (FormatException())
            | _ -> false

        let isPositive =
            match (leadPos, trailPos) with
            | (true,true) -> raise (FormatException())
            | (false,false) -> false
            | _ -> true

        this.IsNegative <-
            match (leadNeg, trailNeg, parenNeg) with
            | (true,false,false)
            | (false,true,false)
            | (false,false,true)
                -> true
            | (false,false,false)
                -> false
            | _ ->
                raise (FormatException())

        if isPositive && this.IsNegative && true
        then raise (FormatException())
        
        this.IsCurrency <-
            match ( this.AllowCurrencySymbol, leadingCurrency, trailingCurrency ) with
            | (true,true,false)
            | (true,false,true)
                -> true
            | (_,false,false)
                -> false
            | _ ->
                raise (FormatException())

        // TODO: Make sure this is correct...
        span.Slice( firstDigit, lastDigit - firstDigit + 1)

    member private this.ParseExpSigns( span:ReadOnlySpan<char> ) : ReadOnlySpan<char> = 
        let foundPositive = hasSymbol formatInfo.PositiveSign span
        let foundNegative = hasSymbol formatInfo.NegativeSign span

        // Highlander check
        if foundPositive && foundNegative
        then raise (FormatException())

        // Make sure it's in the first position
        if foundPositive && (0 < (span.IndexOf this.FormatInfo.PositiveSign))
        then raise (FormatException())

        if foundNegative && (0 < (span.IndexOf formatInfo.NegativeSign))
        then raise (FormatException())

        this.IsExpNegative <- foundNegative
        span.Slice(
            if foundNegative
            then this.FormatInfo.NegativeSign.Length
            else
                if foundPositive
                then this.FormatInfo.PositiveSign.Length
                else 0
        )

    member private this.ParseExponent( span:ReadOnlySpan<char> ) : ReadOnlySpan<char> = 
        let hasExponent = 
            match (hasSymbol "E" span, hasSymbol "e" span) with
            | (true, true) -> raise (FormatException())
            | (false,false) -> false
            | _ -> true

        if hasExponent && not this.AllowExponent
        then raise (FormatException())

        if hasExponent
        then
            let splitPoint = span.IndexOf( "E", StringComparison.InvariantCultureIgnoreCase )
            let exp = this.ParseExpSigns( span.Slice( splitPoint + 1 ) )
            this.Exponent <- exp.ToList()

            if (List.exists (fun c -> not (isUnicodeDecimalDigit( c ))) this.Exponent)
            then raise (System.FormatException())

            span.Slice( 0, splitPoint )
        else
            span
            
    member private this.ParseDecimal( span:ReadOnlySpan<char> ) : ReadOnlySpan<char> = 
        let decimalSymbol = 
            if this.AllowCurrencySymbol && this.IsCurrency
            then this.FormatInfo.CurrencyDecimalSeparator
            else this.FormatInfo.NumberDecimalSeparator

        let hasDecimal = hasSymbol decimalSymbol span

        if hasDecimal && not this.AllowDecimalPoint
        then raise (FormatException())

        if hasDecimal
        then
            let splitPoint = span.IndexOf( decimalSymbol, StringComparison.InvariantCultureIgnoreCase )
            this.Decimal <- span.Slice( splitPoint + decimalSymbol.Length ).ToList()

            if (List.exists (fun c -> not (isUnicodeDecimalDigit( c ))) this.Decimal)
            then raise (System.FormatException())

            span.Slice( 0, splitPoint )
        else
            span

    member internal this.Parse( span:ReadOnlySpan<char> ) : unit =
        let trimmedSpan = this.ParseWhitespace( span )
        let numericSpan = this.ParseSigns( trimmedSpan )

        let coefficientSpan = this.ParseExponent( numericSpan )
        let wholeNumberSpan = this.ParseDecimal( coefficientSpan )

        let groupSeparator = 
            if this.AllowCurrencySymbol && this.IsCurrency
            then this.FormatInfo.CurrencyGroupSeparator
            else this.FormatInfo.NumberGroupSeparator

        let separatorCount = wholeNumberSpan.Count( groupSeparator )
        if (0 < separatorCount) && not this.AllowThousands
        then raise (System.FormatException())

        let ranges = Span<Range>( Array.zeroCreate ( separatorCount + 1 ) )
        wholeNumberSpan.Split( ranges, groupSeparator ) |> ignore
        this.WholeNumber <- wholeNumberSpan.ToList( ranges )

        if (List.exists (fun c -> not (isUnicodeDecimalDigit( c ))) this.WholeNumber)
        then raise (System.FormatException())
