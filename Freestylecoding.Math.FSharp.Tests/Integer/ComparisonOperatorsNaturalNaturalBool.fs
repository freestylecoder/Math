namespace Integer

open Xunit
open Freestylecoding.Math

type public ComparisonOperatorsIntegerIntegerBool() =
    static member private op_GT<'T when 'T :> System.Numerics.IComparisonOperators<'T,'T,bool>>( left:'T, right:'T ) =
        'T.op_GreaterThan( left, right )
    static member private op_GTE<'T when 'T :> System.Numerics.IComparisonOperators<'T,'T,bool>>( left:'T, right:'T ) =
        'T.op_GreaterThanOrEqual( left, right )
    static member private op_LT<'T when 'T :> System.Numerics.IComparisonOperators<'T,'T,bool>>( left:'T, right:'T ) =
        'T.op_LessThan( left, right )
    static member private op_LTE<'T when 'T :> System.Numerics.IComparisonOperators<'T,'T,bool>>( left:'T, right:'T ) =
        'T.op_LessThanOrEqual( left, right )

    static member private BigIntegerToInteger( bi:bigint ) : Integer =
        let rec baToList (ba:byte list) =
            match ba.Length with
            | 0 -> Integer.Zero
            | _ -> ((baToList ba.Tail) <<< 2) + (ba.Head |> int32 |> Integer)
        
        let i =
            bigint.Abs( bi ).ToByteArray()
            |> Array.toList
            |> baToList

        match bi.Sign with
        | -1 -> -i
        | _ -> i

    static member public IntPatterns : obj[][] = [|
        [| -1; -1 |]
        [| -1;  0 |]
        [| -1;  1 |]
        [|  0; -1 |]
        [|  0;  0 |]
        [|  0;  1 |]
        [|  1; -1 |]
        [|  1;  0 |]
        [|  1;  1 |]
    |]

    static member public BigIntPatterns : obj[][] = [|
        [| [| 0xFFuy; 0xBEuy; 0xADuy; 0xDEuy; 0xADuy; 0x0Buy |]; [| 0xFFuy; 0xBEuy; 0xADuy; 0xDEuy; 0x00uy |] |]
        [| [| 0xFFuy; 0xBEuy; 0xADuy; 0xDEuy; 0x00uy |]; [| 0xFFuy; 0xBEuy; 0xADuy; 0xDEuy; 0xADuy; 0x0Buy |] |]
        [| [| 0x01uy; 0x00uy; 0x00uy; 0x00uy; 0x01uy |]; [| 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x01uy |] |]
        [| [| 0x01uy; 0x00uy; 0x00uy; 0x00uy; 0x01uy |]; [| 0x01uy; 0x00uy; 0x00uy; 0x00uy; 0x01uy |] |]
        [| [| 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x01uy |]; [| 0x01uy; 0x00uy; 0x00uy; 0x00uy; 0x01uy |] |]
        [| [| 0xFFuy; 0xBEuy; 0xADuy; 0xDEuy; 0x00uy |]; [| 0x11uy; 0x41uy; 0x52uy; 0x21uy; 0xFFuy |] |] // Negative deadbeef
        [| [| 0x11uy; 0x41uy; 0x52uy; 0x21uy; 0xFFuy |]; [| 0xFFuy; 0xBEuy; 0xADuy; 0xDEuy; 0x00uy |] |]
        [| [| 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0xFFuy |]; [| 0xFFuy; 0xFFuy; 0xFFuy; 0xFFuy; 0xFEuy |] |]
        [| [| 0xFFuy; 0xFFuy; 0xFFuy; 0xFFuy; 0xFEuy |]; [| 0xFFuy; 0xFFuy; 0xFFuy; 0xFFuy; 0xFEuy |] |]
        [| [| 0xFFuy; 0xFFuy; 0xFFuy; 0xFFuy; 0xFEuy |]; [| 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0xFFuy |] |]
    |]


    [<Theory>]
    [<MemberData( "IntPatterns" )>]
    member public this.GT_Sanity (left:int) (right:int) =
        Assert.Equal(
            ComparisonOperatorsIntegerIntegerBool.op_GT( left, right ),
            ComparisonOperatorsIntegerIntegerBool.op_GT(
                Integer( left ),
                Integer( right )
            )
        )

    [<Theory>]
    [<MemberData( "BigIntPatterns" )>]
    member public this.GT_BigSanity (left:byte array) (right:byte array) =
        let leftBI = bigint left
        let leftI = ComparisonOperatorsIntegerIntegerBool.BigIntegerToInteger leftBI

        let rightBI = bigint right
        let rightI = ComparisonOperatorsIntegerIntegerBool.BigIntegerToInteger rightBI

        Assert.Equal(
            ComparisonOperatorsIntegerIntegerBool.op_GT( leftBI, rightBI ),
            ComparisonOperatorsIntegerIntegerBool.op_GT( leftI,  rightI  )
        )


    [<Theory>]
    [<MemberData( "IntPatterns" )>]
    member public this.LT_Sanity (left:int) (right:int) =
        Assert.Equal(
            ComparisonOperatorsIntegerIntegerBool.op_LT( left, right ),
            ComparisonOperatorsIntegerIntegerBool.op_LT(
                Integer( left ),
                Integer( right )
            )
        )

    [<Theory>]
    [<MemberData( "BigIntPatterns" )>]
    member public this.LT_BigSanity (left:byte array) (right:byte array) =
        let leftBI = bigint left
        let leftI = ComparisonOperatorsIntegerIntegerBool.BigIntegerToInteger leftBI

        let rightBI = bigint right
        let rightI = ComparisonOperatorsIntegerIntegerBool.BigIntegerToInteger rightBI

        Assert.Equal(
            ComparisonOperatorsIntegerIntegerBool.op_LT( leftBI, rightBI ),
            ComparisonOperatorsIntegerIntegerBool.op_LT( leftI,  rightI  )
        )


    [<Theory>]
    [<MemberData( "IntPatterns" )>]
    member public this.GTE_Sanity (left:int) (right:int) =
        Assert.Equal(
            ComparisonOperatorsIntegerIntegerBool.op_GTE( left, right ),
            ComparisonOperatorsIntegerIntegerBool.op_GTE(
                Integer( left ),
                Integer( right )
            )
        )

    [<Theory>]
    [<MemberData( "BigIntPatterns" )>]
    member public this.GTE_BigSanity (left:byte array) (right:byte array) =
        let leftBI = bigint left
        let leftI = ComparisonOperatorsIntegerIntegerBool.BigIntegerToInteger leftBI

        let rightBI = bigint right
        let rightI = ComparisonOperatorsIntegerIntegerBool.BigIntegerToInteger rightBI

        Assert.Equal(
            ComparisonOperatorsIntegerIntegerBool.op_GTE( leftBI, rightBI ),
            ComparisonOperatorsIntegerIntegerBool.op_GTE( leftI,  rightI  )
        )


    [<Theory>]
    [<MemberData( "IntPatterns" )>]
    member public this.LTE_Sanity (left:int) (right:int) =
        Assert.Equal(
            ComparisonOperatorsIntegerIntegerBool.op_LTE( left, right ),
            ComparisonOperatorsIntegerIntegerBool.op_LTE(
                Integer( left ),
                Integer( right )
            )
        )

    [<Theory>]
    [<MemberData( "BigIntPatterns" )>]
    member public this.LTE_BigSanity (left:byte array) (right:byte array) =
        let leftBI = bigint left
        let leftI = ComparisonOperatorsIntegerIntegerBool.BigIntegerToInteger leftBI

        let rightBI = bigint right
        let rightI = ComparisonOperatorsIntegerIntegerBool.BigIntegerToInteger rightBI

        Assert.Equal(
            ComparisonOperatorsIntegerIntegerBool.op_LTE( leftBI, rightBI ),
            ComparisonOperatorsIntegerIntegerBool.op_LTE( leftI,  rightI  )
        )
