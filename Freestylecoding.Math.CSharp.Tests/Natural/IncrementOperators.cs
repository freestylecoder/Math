using System.Numerics;
using Microsoft.FSharp.Core;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class IncrementOperators : Freestylecoding.Math.Interface.Tests.TIncrementOperatorsT<Natural> {
	public override Natural item1 => new Natural( 1u );
	public override Natural item2 => new Natural( 2u );
	public override Natural item3 => new Natural( 3u );

	public override FSharpOption<Natural> minValue => new FSharpOption<Natural>( Natural.Zero );
	public override FSharpOption<Natural> maxValue => FSharpOption<Natural>.None;

	[Fact]
	public void OverflowGrows() =>
		Assert.Equal(
			FListNatural( 1u, 0u ),
			Increment( new Natural( 0xFFFF_FFFFu ) )
		);

	[Fact]
	public void CascadingOverflow() =>
		Assert.Equal(
			FListNatural( 1u, 0u, 0u ),
			Increment( FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ) )
		);

	[Fact]
	public void CheckedOverflowGrows() =>
		Assert.Equal(
			FListNatural( 1u, 0u ),
			CheckedIncrement( new Natural( 0xFFFF_FFFFu ) )
		);

	[Fact]
	public void CascadingCheckedOverflow() =>
		Assert.Equal(
			FListNatural( 1u, 0u, 0u ),
			CheckedIncrement( FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ) )
		);
}