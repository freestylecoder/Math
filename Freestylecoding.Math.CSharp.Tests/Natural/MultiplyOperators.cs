using System.Numerics;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class MultiplyOperators {
    private static T Multiply<T>( T left, T right ) where T : IMultiplyOperators<T, T, T> =>
        left * right;
    private static T CheckedMultiply<T>( T left, T right ) where T : IMultiplyOperators<T, T, T> =>
        checked(left * right);

    [Theory]
    [InlineData( 1u, 1u, 1u )]        // Sanity
    [InlineData( 1u, 0u, 0u )]        // Sanity
    [InlineData( 0u, 1u, 0u )]        // Sanity
    [InlineData( 6u, 7u, 42u )]        // multiple bits
    public void Sanity( uint left, uint right, uint expected ) =>
        Assert.Equal(
            new Natural( expected ),
            Multiply(
                new Natural( left ),
                new Natural( right )
            )
        );

    [Fact]
    public void Big() =>
        Assert.Equal(
            FListNatural( 0x75CD9046u, 0x541D5980u ),
            Multiply(
                new Natural( 0xFEDCBA98u ),
                new Natural( 0x76543210u )
            )
        );

    [Theory]
    [InlineData( 1u, 1u, 1u )]        // Sanity
    [InlineData( 1u, 0u, 0u )]        // Sanity
    [InlineData( 0u, 1u, 0u )]        // Sanity
    [InlineData( 6u, 7u, 42u )]        // multiple bits
    public void CheckedSanity( uint left, uint right, uint expected ) =>
        Assert.Equal(
            new Natural( expected ),
            CheckedMultiply(
                new Natural( left ),
                new Natural( right )
            )
        );

    [Fact]
    public void ChcekedBig() =>
        Assert.Equal(
            FListNatural( 0x75CD9046u, 0x541D5980u ),
            CheckedMultiply(
                new Natural( 0xFEDCBA98u ),
                new Natural( 0x76543210u )
            )
        );
}