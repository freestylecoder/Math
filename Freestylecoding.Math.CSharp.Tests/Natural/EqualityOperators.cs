using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class EqualityOperators {
	private static bool InterfaceEq<T>( T left, T right ) where T : IEqualityOperators<T, T, bool> =>
		left == right;

	private static bool InterfaceNe<T>( T left, T right ) where T : IEqualityOperators<T, T, bool> =>
		left != right;

	[Theory]
	[InlineData( 0u, 0u, true )]
	[InlineData( 1u, 0u, false )]
	[InlineData( 0u, 1u, false )]
	[InlineData( 1u, 1u, true )]
	public void EqualitySanity( uint left, uint right, bool expected ) =>
		Assert.Equal(
			expected,
			InterfaceEq(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void EqualityLargeNaturalsTrue() =>
		Assert.True(
			InterfaceEq(
				FListNatural( 0xFu, 0x00000101u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[Fact]
	public void EqualityLargeNaturalsFalse() =>
		Assert.False(
			InterfaceEq(
				FListNatural( 0x8u, 0x00000101u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[Theory]
	[InlineData( 0u, 0u, false )]
	[InlineData( 1u, 0u, true )]
	[InlineData( 0u, 1u, true )]
	[InlineData( 1u, 1u, false )]
	public void InequalitySanity( uint left, uint right, bool expected ) =>
		Assert.Equal(
			expected,
			InterfaceNe(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void InequalityLargeNaturalsFalse() =>
		Assert.False(
			InterfaceNe(
				FListNatural( 0xFu, 0x00000101u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[Fact]
	public void InequalityLargeNaturalsTrue() =>
		Assert.True(
			InterfaceNe(
				FListNatural( 0x8u, 0x00000101u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);
}