using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class EqualityOperators : Freestylecoding.Math.Interface.Tests.TEqualityOperatorsSelf<Natural> {
	public override Natural self1 => Natural.Zero;
	public override Natural self2 => Natural.Unit;

	[Fact]
	public void EqualityLargeNaturalsTrue() =>
		Assert.True(
			Equality(
				FListNatural( 0xFu, 0x00000101u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[Fact]
	public void EqualityLargeNaturalsFalse() =>
		Assert.False(
			Equality(
				FListNatural( 0x8u, 0x00000101u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[Fact]
	public void InequalityLargeNaturalsFalse() =>
		Assert.False(
			Inequality(
				FListNatural( 0xFu, 0x00000101u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[Fact]
	public void InequalityLargeNaturalsTrue() =>
		Assert.True(
			Inequality(
				FListNatural( 0x8u, 0x00000101u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);
}