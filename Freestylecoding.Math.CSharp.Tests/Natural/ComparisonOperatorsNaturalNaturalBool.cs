using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class ComparisonOperatorsNaturalNaturalBool : Freestylecoding.Math.Interface.Tests.TComparisonOperatorsSelf<Natural> {
	public override Natural self0 => Natural.Zero;
	public override Natural self1 => Natural.Unit;
	public override Natural self2 => new Natural( int.MaxValue );
	public override Natural self3 => new Natural( uint.MaxValue );

	[Fact]
	public void GreaterThan_BiggerLeft() =>
		Assert.True(
			GreaterThan(
				FListNatural( 0xBADu, 0xDEADBEEFu ),
				new Natural( 0xDEADBEEFu )
			)
		);

	[Fact]
	public void GreaterThan_BiggerRight() =>
		Assert.False(
			GreaterThan(
				new Natural( 0xDEADBEEFu ),
				FListNatural( 0xBADu, 0xDEADBEEFu )
			)
		);

	[Fact]
	public void GreaterThan_CascadeGreaterThan() =>
		Assert.True(
			GreaterThan(
				FListNatural( 1u, 1u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void GreaterThan_CascadeEqual() =>
		Assert.False(
			GreaterThan(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void GreaterThan_CascadeLessThan() =>
		Assert.False(
			GreaterThan(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 1u )
			)
		);

	[Fact]
	public void LessThan_BiggerLeft() =>
		Assert.False(
			LessThan(
				FListNatural( 0xBADu, 0xDEADBEEFu ),
				new Natural( 0xDEADBEEFu )
			)
		);

	[Fact]
	public void LessThan_BiggerRight() =>
		Assert.True(
			LessThan(
				new Natural( 0xDEADBEEFu ),
				FListNatural( 0xBADu, 0xDEADBEEFu )
			)
		);

	[Fact]
	public void LessThan_CascadeGreaterThan() =>
		Assert.False(
			LessThan(
				FListNatural( 1u, 1u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void LessThan_CascadeEqual() =>
		Assert.False(
			LessThan(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void LessThan_CascadeLessThan() =>
		Assert.True(
			LessThan(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 1u )
			)
		);

	[Fact]
	public void GreaterThanOrEqual_BiggerLeft() =>
		Assert.True(
			GreaterThanOrEqual(
				FListNatural( 0xBADu, 0xDEADBEEFu ),
				new Natural( 0xDEADBEEFu )
			)
		);

	[Fact]
	public void GreaterThanOrEqual_BiggerRight() =>
		Assert.False(
			GreaterThanOrEqual(
				new Natural( 0xDEADBEEFu ),
				FListNatural( 0xBADu, 0xDEADBEEFu )
			)
		);

	[Fact]
	public void GreaterThanOrEqual_CascadeGreaterThan() =>
		Assert.True(
			GreaterThanOrEqual(
				FListNatural( 1u, 1u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void GreaterThanOrEqual_CascadeEqual() =>
		Assert.True(
			GreaterThanOrEqual(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void GreaterThanOrEqual_CascadeLessThan() =>
		Assert.False(
			GreaterThanOrEqual(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 1u )
			)
		);

	[Fact]
	public void LessThanOrEqual_BiggerLeft() =>
		Assert.False(
			LessThanOrEqual(
				FListNatural( 0xBADu, 0xDEADBEEFu ),
				new Natural( 0xDEADBEEFu )
			)
		);

	[Fact]
	public void LessThanOrEqual_BiggerRight() =>
		Assert.True(
			LessThanOrEqual(
				new Natural( 0xDEADBEEFu ),
				FListNatural( 0xBADu, 0xDEADBEEFu )
			)
		);

	[Fact]
	public void LessThanOrEqual_CascadeGreaterThan() =>
		Assert.False(
			LessThanOrEqual(
				FListNatural( 1u, 1u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void LessThanOrEqual_CascadeEqual() =>
		Assert.True(
			LessThanOrEqual(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void LessThanOrEqual_CascadeLessThan() =>
		Assert.True(
			LessThanOrEqual(
				FListNatural( 1u, 0u ),
				FListNatural( 1u, 1u )
			)
		);
}