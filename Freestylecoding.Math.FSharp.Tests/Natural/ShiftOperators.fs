namespace Natural

open Xunit
open Freestylecoding.Math

type public ShiftOperators() =
    static member LS( left:'T when 'T :> System.Numerics.IShiftOperators<'T,'I,'T>, right:'I ) : 'T =
        'T.op_LeftShift( left, right )
    static member RS( left:'T when 'T :> System.Numerics.IShiftOperators<'T,'I,'T>, right:'I ) : 'T =
        'T.op_RightShift( left, right )
    static member URS( left:'T when 'T :> System.Numerics.IShiftOperators<'T,'I,'T>, right:'I ) : 'T =
        'T.op_UnsignedRightShift( left, right )

    // LEFT SHIFT

    [<Theory>]
    [<InlineData(   1u, 1,    2u )>]        // Sanity
    [<InlineData( 0xFu, 2, 0x3Cu )>]   // multiple bits
    member public this.LS_Sanity left right expected =
        Assert.Equal( Natural([expected]), ShiftOperators.LS( Natural([left]), right ) )
    
    [<Fact>]
    member public this.LS_Overflow () =
        Assert.Equal( Natural([1u; 0xFFFFFFFEu]), ShiftOperators.LS( Natural([0xFFFFFFFFu]), 1 ) )
    
    [<Fact>]
    member public this.LS_MultipleOverflow () =
        Assert.Equal( Natural([0x5u; 0xFFFFFFF8u]), ShiftOperators.LS( Natural([0xBFFFFFFFu]), 3 ) )
    
    [<Fact>]
    member public this.LS_OverOneUInt () =
        Assert.Equal( Natural([8u; 0u; 0u]), ShiftOperators.LS( Natural.Unit, 67 ) )

    // RIGHT SHIFT

    [<Theory>]
    [<InlineData(    1u, 1,   0u )>]        // Sanity
    [<InlineData(  0xFu, 2, 0x3u )>]    // multiple bits
    [<InlineData( 0x3Cu, 2, 0xFu )>]   // multiple bits
    member public this.RS_Sanity left right expected =
        Assert.Equal( Natural([expected]), ShiftOperators.RS( Natural([left]), right ) )

    [<Fact>]
    member public this.RS_Underflow () =
        Assert.Equal( Natural([0x7FFFFFFFu]), ShiftOperators.RS( Natural([0xFFFFFFFFu]), 1 ) )
    
    [<Fact>]
    member public this.RS_MultipleUnderflow () =
        Assert.Equal( Natural([0x1u; 0x5FFFFFFFu]), ShiftOperators.RS( Natural([0xAu; 0xFFFFFFFFu]), 3 ) )
    
    [<Fact>]
    member public this.RS_OverOneUInt () =
        Assert.Equal( Natural.Unit, ShiftOperators.RS( Natural([0x10u; 0u; 0u]), 68 ) )
    
    [<Fact>]
    member public this.RS_ReduceToZero () =
        Assert.Equal( Natural.Zero, ShiftOperators.RS( Natural([0x10u; 0u; 0u]), 99 ) )

    // UNSIGNED RIGHT SHIFT
    // (Since the type is unsigned, this is the same as RightShift

    [<Theory>]
    [<InlineData(    1u, 1,   0u )>]        // Sanity
    [<InlineData(  0xFu, 2, 0x3u )>]    // multiple bits
    [<InlineData( 0x3Cu, 2, 0xFu )>]   // multiple bits
    member public this.URS_Sanity left right expected =
        Assert.Equal( Natural([expected]), ShiftOperators.URS( Natural([left]), right ) )

    [<Fact>]
    member public this.URS_Underflow () =
        Assert.Equal( Natural([0x7FFFFFFFu]), ShiftOperators.URS( Natural([0xFFFFFFFFu]), 1 ) )
    
    [<Fact>]
    member public this.URS_MultipleUnderflow () =
        Assert.Equal( Natural([0x1u; 0x5FFFFFFFu]), ShiftOperators.URS( Natural([0xAu; 0xFFFFFFFFu]), 3 ) )
    
    [<Fact>]
    member public this.URS_OverOneUInt () =
        Assert.Equal( Natural.Unit, ShiftOperators.URS( Natural([0x10u; 0u; 0u]), 68 ) )
    
    [<Fact>]
    member public this.URS_ReduceToZero () =
        Assert.Equal( Natural.Zero, ShiftOperators.URS( Natural([0x10u; 0u; 0u]), 99 ) )

    [<Fact>]
    member public this.URS_ExtremeReduceToZero () =
        // NOTE: This is an extreme reductionary case to hit one specific edge case in the base operator
        // Specifically, this part:
        //              let rec chomp n l =
        //                  match n with
        //                  | x when x > (List.length l) -> [0u]
        // It's only tested here, so that this test suite really has a reason to be

        Assert.Equal( Natural.Zero, ShiftOperators.URS( Natural([0x10u; 0u; 0u]), 999 ) )
