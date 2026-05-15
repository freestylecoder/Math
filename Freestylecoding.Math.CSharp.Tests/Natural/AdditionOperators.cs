using System.Numerics;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class AdditionOperators {
    private static T Add<T>( T left, T right ) where T : IAdditionOperators<T, T, T> =>
        left + right;

    [Theory]
    [InlineData( 0u, 0u, 0u )]        // Sanity
    [InlineData( 1u, 1u, 2u )]        // Sanity
    [InlineData( 1u, 0u, 1u )]        // Sanity
    [InlineData( 0u, 1u, 1u )]        // Sanity
    [InlineData( 1u, 5u, 6u )]        // l < r
    [InlineData( 9u, 2u, 11u )]        // l > r
    public void Sanity( uint left, uint right, uint expected ) =>
        Assert.Equal(
            new Natural( expected ),
            Add( new Natural( left ), new Natural( right ) )
        );

    [Fact]
    public void Simple() =>
        Assert.Equal(
            Natural.Unit,
            Natural.Zero + Natural.Unit
        );

    [Fact]
    public void Overflow() =>
        Assert.Equal(
            FListNatural( 1u, 2u ),
            Add(
                new Natural( 0xFFFF_FFFFu ),
                new Natural( 3u )
            )
        );

    [Fact]
    public void LeftBiggerNoOverflow() =>
        Assert.Equal(
            FListNatural( 0xFu, 0xFF0Fu ),
            Add(
                FListNatural( 0xFu, 0xFu ),
                new Natural( 0xFF00u )
            )
        );

    [Fact]
    public void RightBiggerNoOverflow() =>
        Assert.Equal(
            FListNatural( 0xFu, 0xFF0Fu ),
            Add(
                new Natural( 0xFF00u ),
                FListNatural( 0xFu, 0xFu )
            )
        );

    [Fact]
    public void CascadingOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 0u, 0u ),
            Add(
                new Natural( 1u ),
                FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu )
            )
        );

    [Fact]
    public void OverflowCausedOverflow() =>
        Assert.Equal(
            FListNatural( 2u, 0u, 0u ),
            Add(
                FListNatural( 1u, 1u ),
                FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0xFFFF_FFFFu )
            )
        );

    [Fact]
    public void EdgeOfOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 0xFFFF_FFFFu, 1u ),
            Add(
                FListNatural( 1u, 1u ),
                FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0u )
            )
        );

    private static T CheckedAdd<T>( T left, T right ) where T : IAdditionOperators<T, T, T> {
        checked {
            return left + right;
        }
    }

    [Theory]
    [InlineData( 0u, 0u, 0u )]        // Sanity
    [InlineData( 1u, 1u, 2u )]        // Sanity
    [InlineData( 1u, 0u, 1u )]        // Sanity
    [InlineData( 0u, 1u, 1u )]        // Sanity
    [InlineData( 1u, 5u, 6u )]        // l < r
    [InlineData( 9u, 2u, 11u )]       // l > r
    public void CheckedSanity( uint left, uint right, uint expected ) =>
        Assert.Equal(
            new Natural( expected ),
            CheckedAdd( new Natural( left ), new Natural( right ) )
        );

    [Fact]
    public void CheckedSimple() =>
        Assert.Equal(
            Natural.Unit,
            Natural.Zero + Natural.Unit
        );

    [Fact]
    public void CheckedOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 2u ),
            CheckedAdd(
                new Natural( 0xFFFF_FFFFu ),
                new Natural( 3u )
            )
        );

    [Fact]
    public void CheckedLeftBiggerNoOverflow() =>
        Assert.Equal(
            FListNatural( 0xFu, 0xFF0Fu ),
            CheckedAdd(
                FListNatural( 0xFu, 0xFu ),
                new Natural( 0xFF00u )
            )
        );

    [Fact]
    public void CheckedRightBiggerNoOverflow() =>
        Assert.Equal(
            FListNatural( 0xFu, 0xFF0Fu ),
            CheckedAdd(
                new Natural( 0xFF00u ),
                FListNatural( 0xFu, 0xFu )
            )
        );

    [Fact]
    public void CheckedCascadingOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 0u, 0u ),
            CheckedAdd(
                new Natural( 1u ),
                FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu )
            )
        );

    [Fact]
    public void CheckedOverflowCausedOverflow() =>
        Assert.Equal(
            FListNatural( 2u, 0u, 0u ),
            CheckedAdd(
                FListNatural( 1u, 1u ),
                FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0xFFFF_FFFFu )
            )
        );

    [Fact]
    public void CheckedEdgeOfOverflow() =>
        Assert.Equal(
            FListNatural( 1u, 0xFFFF_FFFFu, 1u ),
            CheckedAdd(
                FListNatural( 1u, 1u ),
                FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0u )
            )
        );
}