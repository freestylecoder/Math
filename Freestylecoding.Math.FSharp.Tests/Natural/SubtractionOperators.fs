namespace Natural

open Xunit
open Freestylecoding.Math

type public SubtractionOperators() =
    inherit Freestylecoding.Math.Interface.Tests.TSubtractionOperatorsSelf<Natural>()

    override _.selfIdentity = Some Natural.Zero
    override _.selfDecrement = Some Natural.Unit
    override _.selfMin = Some Natural.Zero
    override _.selfMax = None
    override _.IsUnsigned = true

    override _.ToSelf( b:byte ) : Natural =
        Natural( b )

    [<Fact>]
    member public this.SingleItemBadUnderflow () =
        let exc = Record.Exception(
            fun () ->
                this.Subtraction(
                    Natural.Zero,
                    Natural.Unit
                )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

    [<Fact>]
    member public this.MultiItemNoUnderflow () =
        Assert.Equal(
            Natural( [2u; 2u] ),
            this.Subtraction(
                Natural( [3u; 4u] ),
                Natural( [1u; 2u] )
            )
        )
        
    [<Fact>]
    member public this.MultiItemSafeUnderflow () =
        Assert.Equal(
            Natural( [0x2u; 0xFFFFFFFFu] ),
            this.Subtraction(
                Natural( [4u; 2u] ),
                Natural( [1u; 3u] )
            )
        )
        
    [<Fact>]
    member public this.MultiItemSafeCascadingUnderflow () =
        Assert.Equal(
            Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] ),
            this.Subtraction(
                Natural( [1u; 0u; 0u] ),
                Natural.Unit
            )
        )
        
    [<Fact>]
    member public this.MultiItemUnsafeUnderflow () =
        let exc = Record.Exception(
            fun () ->
                this.Subtraction(
                    Natural( [1u; 2u] ),
                    Natural( [1u; 3u] )
                )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

    [<Fact>]
    member public this.LargeWithUnderflows () =
        Assert.Equal(
            Natural( [0x1u; 0xFFFF_FFFFu; 0xFFFF_FFFEu] ),
            this.Subtraction(
                Natural( [3u; 2u; 1u] ),
                Natural( [1u; 2u; 3u] )
            )
        )

    [<Fact>]
    member public this.SingleItemCheckedBadUnderflow () =
        let exc = Record.Exception(
            fun () ->
                this.CheckedSubtraction(
                    Natural.Zero,
                    Natural.Unit
                )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

    [<Fact>]
    member public this.MultiItemCheckedNoUnderflow () =
        Assert.Equal(
            Natural( [2u; 2u] ),
            this.CheckedSubtraction(
                Natural( [3u; 4u] ),
                Natural( [1u; 2u] )
            )
        )
        
    [<Fact>]
    member public this.MultiItemCheckedSafeUnderflow () =
        Assert.Equal(
            Natural( [0x2u; 0xFFFFFFFFu] ),
            this.CheckedSubtraction(
                Natural( [4u; 2u] ),
                Natural( [1u; 3u] )
            )
        )
        
    [<Fact>]
    member public this.MultiItemCheckedSafeCascadingUnderflow () =
        Assert.Equal(
            Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] ),
            this.CheckedSubtraction(
                Natural( [1u; 0u; 0u] ),
                Natural.Unit
            )
        )
        
    [<Fact>]
    member public this.MultiItemCheckedUnsafeUnderflow () =
        let exc = Record.Exception(
            fun () ->
                this.CheckedSubtraction(
                    Natural( [1u; 2u] ),
                    Natural( [1u; 3u] )
                )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )

    [<Fact>]
    member public this.LargeCheckedWithUnderflows () =
        Assert.Equal(
            Natural( [0x1u; 0xFFFF_FFFFu; 0xFFFF_FFFEu] ),
            this.CheckedSubtraction(
                Natural( [3u; 2u; 1u] ),
                Natural( [1u; 2u; 3u] )
            )
        )
