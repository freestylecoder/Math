using System.Numerics;
using Microsoft.FSharp.Core;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class SubtractionOperators : Freestylecoding.Math.Interface.Tests.TSubtractionOperatorsSelf<Natural> {
	public override FSharpOption<Natural> selfIdentity => Natural.Zero;
	public override FSharpOption<Natural> selfDecrement => Natural.Unit;
	public override FSharpOption<Natural> selfMin => Natural.Zero;
	public override FSharpOption<Natural> selfMax => FSharpOption<Natural>.None;

	public override bool IsUnsigned => true;

	public override Natural ToSelf( byte b ) => new Natural( b );

	[Fact]
	public void SingleItemBadUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				Subtraction(
					Natural.Zero,
					Natural.Unit
				)
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Fact]
	public void MultiItemNoUnderflow() =>
		Assert.Equal(
			FListNatural( 2u, 2u ),
			Subtraction(
				FListNatural( 3u, 4u ),
				FListNatural( 1u, 2u )
			)
		);

	[Fact]
	public void MultiItemSafeUnderflow() =>
		Assert.Equal(
			FListNatural( 0x2u, 0xFFFFFFFFu ),
			Subtraction(
				FListNatural( 4u, 2u ),
				FListNatural( 1u, 3u )
			)
		);

	[Fact]
	public void MultiItemSafeCascadingUnderflow() =>
		Assert.Equal(
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ),
			Subtraction(
				FListNatural( 1u, 0u, 0u ),
				Natural.Unit
			)
		);

	[Fact]
	public void MultiItemUnsafeUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				Subtraction(
					FListNatural( 1u, 2u ),
					FListNatural( 1u, 3u )
				)
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Fact]
	public void LargeWithUnderflows() =>
		Assert.Equal(
			FListNatural( 0x1u, 0xFFFF_FFFFu, 0xFFFF_FFFEu ),
			Subtraction(
				FListNatural( 3u, 2u, 1u ),
				FListNatural( 1u, 2u, 3u )
			)
		);

	[Fact]
	public void SingleItemCheckedBadUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				CheckedSubtraction(
					Natural.Zero,
					Natural.Unit
				)
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Fact]
	public void MultiItemCheckedNoUnderflow() =>
		Assert.Equal(
			FListNatural( 2u, 2u ),
			CheckedSubtraction(
				FListNatural( 3u, 4u ),
				FListNatural( 1u, 2u )
			)
		);

	[Fact]
	public void MultiItemCheckedSafeUnderflow() =>
		Assert.Equal(
			FListNatural( 0x2u, 0xFFFFFFFFu ),
			CheckedSubtraction(
				FListNatural( 4u, 2u ),
				FListNatural( 1u, 3u )
			)
		);

	[Fact]
	public void MultiItemCheckedSafeCascadingUnderflow() =>
		Assert.Equal(
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ),
			CheckedSubtraction(
				FListNatural( 1u, 0u, 0u ),
				Natural.Unit
			)
		);

	[Fact]
	public void MultiItemCheckedUnsafeUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				CheckedSubtraction(
					FListNatural( 1u, 2u ),
					FListNatural( 1u, 3u )
				)
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Fact]
	public void LargeCheckedWithUnderflows() =>
		Assert.Equal(
			FListNatural( 0x1u, 0xFFFF_FFFFu, 0xFFFF_FFFEu ),
			CheckedSubtraction(
				FListNatural( 3u, 2u, 1u ),
				FListNatural( 1u, 2u, 3u )
			)
		);
}