using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class SubtractionOperators {
	private static T Sub<T>( T left, T right ) where T : ISubtractionOperators<T, T, T> =>
		left - right;

	[Theory]
	[InlineData( 1u, 1u, 0u )]    // Sanity
	[InlineData( 1u, 0u, 1u )]    // Sanity
	[InlineData( 9u, 2u, 7u )]    // l > r
	public void Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			Sub(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void SingleItemBadUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				Sub(
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
			Sub(
				FListNatural( 3u, 4u ),
				FListNatural( 1u, 2u )
			)
		);

	[Fact]
	public void MultiItemSafeUnderflow() =>
		Assert.Equal(
			FListNatural( 0x2u, 0xFFFFFFFFu ),
			Sub(
				FListNatural( 4u, 2u ),
				FListNatural( 1u, 3u )
			)
		);

	[Fact]
	public void MultiItemSafeCascadingUnderflow() =>
		Assert.Equal(
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ),
			Sub(
				FListNatural( 1u, 0u, 0u ),
				Natural.Unit
			)
		);

	[Fact]
	public void MultiItemUnsafeUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				Sub(
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
			Sub(
				FListNatural( 3u, 2u, 1u ),
				FListNatural( 1u, 2u, 3u )
			)
		);
}

public class CheckedSubtractionOperators {
	private static T CheckedSub<T>( T left, T right ) where T : ISubtractionOperators<T, T, T> =>
		checked(left - right);

	[Theory]
	[InlineData( 1u, 1u, 0u )]    // Sanity
	[InlineData( 1u, 0u, 1u )]    // Sanity
	[InlineData( 9u, 2u, 7u )]    // l > r
	public void Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			CheckedSub(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void SingleItemBadUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				CheckedSub(
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
			CheckedSub(
				FListNatural( 3u, 4u ),
				FListNatural( 1u, 2u )
			)
		);

	[Fact]
	public void MultiItemSafeUnderflow() =>
		Assert.Equal(
			FListNatural( 0x2u, 0xFFFFFFFFu ),
			CheckedSub(
				FListNatural( 4u, 2u ),
				FListNatural( 1u, 3u )
			)
		);

	[Fact]
	public void MultiItemSafeCascadingUnderflow() =>
		Assert.Equal(
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ),
			CheckedSub(
				FListNatural( 1u, 0u, 0u ),
				Natural.Unit
			)
		);

	[Fact]
	public void MultiItemUnsafeUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				CheckedSub(
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
			CheckedSub(
				FListNatural( 3u, 2u, 1u ),
				FListNatural( 1u, 2u, 3u )
			)
		);
}