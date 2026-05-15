using System.Numerics;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class ComparisonOperatorsNaturalNaturalBool {
	private static bool op_GT<T>( T left, T right ) where T : IComparisonOperators<T, T, bool> =>
		left > right;
	private static bool op_GTE<T>( T left, T right ) where T : IComparisonOperators<T, T, bool> =>
		left >= right;
	private static bool op_LT<T>( T left, T right ) where T : IComparisonOperators<T, T, bool> =>
		left < right;
	private static bool op_LTE<T>( T left, T right ) where T : IComparisonOperators<T, T, bool> =>
		left <= right;

	[Theory]
	[InlineData( 0u, 1u, false )]
	[InlineData( 1u, 0u, true )]
	[InlineData( 1u, 1u, false )]
	public void GT_Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal(
			expected,
			op_GT(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void GT_BiggerLeft() =>
		Assert.True(
			op_GT(
				FListNatural( 0xBADu, 0xDEADBEEFu ),
				new Natural( 0xDEADBEEFu )
			)
		);

	[Fact]
	public void GT_BiggerRight() =>
		Assert.False(
			op_GT(
				new Natural( 0xDEADBEEFu ),
				FListNatural( 0xBADu, 0xDEADBEEFu )
			)
		);

	[Fact]
	public void GT_CascadeGreaterThan() =>
		Assert.True(
			op_GT(
				FListNatural( 1u, 1u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void GT_CascadeEqual() =>
		Assert.False(
			op_GT(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void GT_CascadeLessThan() =>
		Assert.False(
			op_GT(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 1u )
			)
		);

	[Theory]
	[InlineData( 0u, 1u, true )]
	[InlineData( 1u, 0u, false )]
	[InlineData( 1u, 1u, false )]
	public void LT_Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal(
			expected,
			op_LT(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void LT_BiggerLeft() =>
		Assert.False(
			op_LT(
				FListNatural( 0xBADu, 0xDEADBEEFu ),
				new Natural( 0xDEADBEEFu )
			)
		);

	[Fact]
	public void LT_BiggerRight() =>
		Assert.True(
			op_LT(
				new Natural( 0xDEADBEEFu ),
				FListNatural( 0xBADu, 0xDEADBEEFu )
			)
		);

	[Fact]
	public void LT_CascadeGreaterThan() =>
		Assert.False(
			op_LT(
				FListNatural( 1u, 1u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void LT_CascadeEqual() =>
		Assert.False(
			op_LT(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void LT_CascadeLessThan() =>
		Assert.True(
			op_LT(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 1u )
			)
		);

	[Theory]
	[InlineData( 0u, 1u, false )]
	[InlineData( 1u, 0u, true )]
	[InlineData( 1u, 1u, true )]
	public void GTE_Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal(
			expected,
			op_GTE(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void GTE_BiggerLeft() =>
		Assert.True(
			op_GTE(
				FListNatural( 0xBADu, 0xDEADBEEFu ),
				new Natural( 0xDEADBEEFu )
			)
		);

	[Fact]
	public void GTE_BiggerRight() =>
		Assert.False(
			op_GTE(
				new Natural( 0xDEADBEEFu ),
				FListNatural( 0xBADu, 0xDEADBEEFu )
			)
		);

	[Fact]
	public void GTE_CascadeGreaterThan() =>
		Assert.True(
			op_GTE(
				FListNatural( 1u, 1u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void GTE_CascadeEqual() =>
		Assert.True(
			op_GTE(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void GTE_CascadeLessThan() =>
		Assert.False(
			op_GTE(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 1u )
			)
		);

	[Theory]
	[InlineData( 0u, 1u, true )]
	[InlineData( 1u, 0u, false )]
	[InlineData( 1u, 1u, true )]
	public void LTE_Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal(
			expected,
			op_LTE(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void LTE_BiggerLeft() =>
		Assert.False(
			op_LTE(
				FListNatural( 0xBADu, 0xDEADBEEFu ),
				new Natural( 0xDEADBEEFu )
			)
		);

	[Fact]
	public void LTE_BiggerRight() =>
		Assert.True(
			op_LTE(
				new Natural( 0xDEADBEEFu ),
				FListNatural( 0xBADu, 0xDEADBEEFu )
			)
		);

	[Fact]
	public void LTE_CascadeGreaterThan() =>
		Assert.False(
			op_LTE(
				FListNatural( 1u, 1u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void LTE_CascadeEqual() =>
		Assert.True(
			op_LTE(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void LTE_CascadeLessThan() =>
		Assert.True(
			op_LTE(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 1u )
			)
		);
}