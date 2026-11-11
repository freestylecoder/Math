namespace Natural

open Xunit
open Freestylecoding.Math

type public AdditionOperators() =
    inherit Freestylecoding.Math.Interface.Tests.TAdditionOperatorsSelf<Natural>()

    override _.selfIdentity = Some Natural.Zero
    override _.selfIncrement = Some Natural.Unit
    override _.self1 = Natural( 18u )
    override _.self2 = Natural( 42u )
    override _.selfMin = Some Natural.Zero
    override _.selfMax = None

    override _.sum11 = Natural( 36u )
    override _.sum12 = Natural( 60u )
    override _.sum21 = Natural( 60u )
    override _.sum22 = Natural( 84u )

    [<Fact>]
    member public this.Overflow () =
        Assert.Equal(
            Natural( [1u; 2u] ),
            this.Addition(
                Natural( [0xFFFF_FFFFu] ),
                Natural( [3u] )
            )
        )
        
    [<Fact>]
    member public this.LeftBiggerNoOverflow () =
        Assert.Equal(
            Natural( [0xFu; 0xFF0Fu] ),
            this.Addition(
                Natural( [0xFu; 0xFu] ),
                Natural( [0xFF00u] )
            )
        )
        
    [<Fact>]
    member public this.RightBiggerNoOverflow () =
        Assert.Equal(
            Natural( [0xFu; 0xFF0Fu] ),
            this.Addition(
                Natural( [0xFF00u] ),
                Natural( [0xFu; 0xFu] )
            )
        )
        
    [<Fact>]
    member public this.CascadingOverflow () =
        Assert.Equal(
            Natural( [1u; 0u; 0u] ),
            this.Addition(
                Natural( [1u] ),
                Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
            )
        )

    [<Fact>]
    member public this.OverflowCausedOverflow () =
        Assert.Equal(
            Natural( [2u; 0u; 0u] ),
            this.Addition(
                Natural( [1u; 1u] ),
                Natural( [1u; 0xFFFF_FFFFu - 1u; 0xFFFF_FFFFu] )
            )
        )

    [<Fact>]
    member public this.EdgeOfOverflow () =
        Assert.Equal(
            Natural( [1u; 0xFFFF_FFFFu; 1u] ),
            this.Addition(
                Natural( [1u; 1u] ),
                Natural( [1u; 0xFFFF_FFFFu - 1u; 0u] )
            )
        )

    [<Fact>]
    member public this.CheckedOverflow () =
        Assert.Equal(
            Natural( [1u; 2u] ),
            this.CheckedAddition(
                Natural( [0xFFFF_FFFFu] ),
                Natural( [3u] )
            )
        )
            
    [<Fact>]
    member public this.CheckedLeftBiggerNoOverflow () =
        Assert.Equal(
            Natural( [0xFu; 0xFF0Fu] ),
            this.CheckedAddition(
                Natural( [0xFu; 0xFu] ),
                Natural( [0xFF00u] )
            )
        )
            
    [<Fact>]
    member public this.CheckedRightBiggerNoOverflow () =
        Assert.Equal(
            Natural( [0xFu; 0xFF0Fu] ),
            this.CheckedAddition(
                Natural( [0xFF00u] ),
                Natural( [0xFu; 0xFu] )
            )
        )
           
    [<Fact>]
    member public this.CheckedCascadingOverflow () =
        Assert.Equal(
            Natural( [1u; 0u; 0u] ),
            this.CheckedAddition(
                Natural( [1u] ),
                Natural( [0xFFFF_FFFFu; 0xFFFF_FFFFu] )
            )
        )
    
    [<Fact>]
    member public this.CheckedOverflowCausedOverflow () =
        Assert.Equal(
            Natural( [2u; 0u; 0u] ),
            this.CheckedAddition(
                Natural( [1u; 1u] ),
                Natural( [1u; 0xFFFF_FFFFu - 1u; 0xFFFF_FFFFu] )
            )
        )
    
    [<Fact>]
    member public this.CheckedEdgeOfOverflow () =
        Assert.Equal(
            Natural( [1u; 0xFFFF_FFFFu; 1u] ),
            this.CheckedAddition(
                Natural( [1u; 1u] ),
                Natural( [1u; 0xFFFF_FFFFu - 1u; 0u] )
            )
        )
