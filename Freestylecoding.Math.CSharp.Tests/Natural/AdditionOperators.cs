using System.Numerics;
using Microsoft.FSharp.Core;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class AdditionOperators : Freestylecoding.Math.Interface.Tests.TAdditionOperatorsSelf<Natural> {
    public override FSharpOption<Natural> selfIdentity => new FSharpOption<Natural>( Natural.Zero );
    public override FSharpOption<Natural> selfIncrement => new FSharpOption<Natural>( Natural.One );
	public override Natural self1 => new Natural( 18u );
	public override Natural self2 => new Natural( 42u );
	public override FSharpOption<Natural> selfMin => new FSharpOption<Natural>( Natural.Zero );
	public override FSharpOption<Natural> selfMax => FSharpOption<Natural>.None;

	public override Natural sum11 => new Natural( 36u );
	public override Natural sum12 => new Natural( 60u );
	public override Natural sum21 => new Natural( 60u );
	public override Natural sum22 => new Natural( 84u );

    [Fact]
    public void Overflow() =>
        Assert.Equal(
            FListNatural( 1u, 2u ),
            Addition(
                new Natural( 0xFFFF_FFFFu ),
                new Natural( 3u )
            )
        );

    [Fact]
    public void LeftBiggerNoOverflow() =>
        Assert.Equal(
            FListNatural( 0xFu, 0xFF0Fu ),
            Addition(
                FListNatural( 0xFu, 0xFu ),
                new Natural( 0xFF00u )
            )
        );

    [Fact]
    public void RightBiggerNoOverflow() =>
        Assert.Equal(
            FListNatural( 0xFu, 0xFF0Fu ),
            Addition(
                new Natural( 0xFF00u ),
                FListNatural( 0xFu, 0xFu )
            )
        );

    [Fact]
    public void CascadingOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 0u, 0u ),
            Addition(
                new Natural( 1u ),
                FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu )
            )
        );

    [Fact]
    public void OverflowCausedOverflow() =>
        Assert.Equal(
            FListNatural( 2u, 0u, 0u ),
            Addition(
                FListNatural( 1u, 1u ),
                FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0xFFFF_FFFFu )
            )
        );

    [Fact]
    public void EdgeOfOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 0xFFFF_FFFFu, 1u ),
            Addition(
                FListNatural( 1u, 1u ),
                FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0u )
            )
        );

    [Fact]
    public void CheckedOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 2u ),
            CheckedAddition(
                new Natural( 0xFFFF_FFFFu ),
                new Natural( 3u )
            )
        );

    [Fact]
    public void CheckedLeftBiggerNoOverflow() =>
        Assert.Equal(
            FListNatural( 0xFu, 0xFF0Fu ),
            CheckedAddition(
                FListNatural( 0xFu, 0xFu ),
                new Natural( 0xFF00u )
            )
        );

    [Fact]
    public void CheckedRightBiggerNoOverflow() =>
        Assert.Equal(
            FListNatural( 0xFu, 0xFF0Fu ),
            CheckedAddition(
                new Natural( 0xFF00u ),
                FListNatural( 0xFu, 0xFu )
            )
        );

    [Fact]
    public void CheckedCascadingOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 0u, 0u ),
            CheckedAddition(
                new Natural( 1u ),
                FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu )
            )
        );

    [Fact]
    public void CheckedOverflowCausedOverflow() =>
        Assert.Equal(
            FListNatural( 2u, 0u, 0u ),
            CheckedAddition(
                FListNatural( 1u, 1u ),
                FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0xFFFF_FFFFu )
            )
        );

    [Fact]
    public void CheckedEdgeOfOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 0xFFFF_FFFFu, 1u ),
            CheckedAddition(
                FListNatural( 1u, 1u ),
                FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0u )
            )
        );
}