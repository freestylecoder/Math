using Xunit;

namespace Natural.Tests;

using Natural = Freestylecoding.Math.Natural;
using static Freestylecoding.Math.CSharp.Tests.Helpers;

public class Constants {
	[Fact]
	public void ZeroConstant() =>
		Assert.Equal( FListNatural( 0u ), Natural.Zero );

	[Fact]
	public void UnitConstant() =>
		Assert.Equal( FListNatural( 1u ), Natural.Unit );

	// One is a constant that comes from INumberBase
	// I use the math-y terms
	[Fact]
	public void OneConstant() =>
		Assert.Equal( FListNatural( 1u ), Natural.One );
}