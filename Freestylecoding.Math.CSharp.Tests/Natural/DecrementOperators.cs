using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class DecrementOperators {
	private static T Decrement<T>( T n ) where T : IDecrementOperators<T> =>
		--n;

	[Theory]
	[InlineData( 1u, 0u )]   // Sanity
	[InlineData( 2u, 1u )]   // Sanity
	[InlineData( 10u, 9u )]   // Sanity
	public void Sanity( uint value, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			Decrement( new Natural( value ) )
		);

	[Fact]
	public void Overflow() =>
		Assert.Equal(
			new Natural( 0xFFFF_FFFFu ),
			Decrement(
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void CascadingOverflow() =>
		Assert.Equal(
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ),
			Decrement(
				FListNatural( 1u, 0u, 0u )
			)
		);

	[Fact]
	public void SingleItemBadUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				Decrement(
					Natural.Zero
				)
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Fact( Skip = "Postfix won't work until Natural is a ValueType" )]
	public void Postfix() {
		Natural actual = Natural.One;
		Assert.Equal(
			Natural.One,
			actual--
		);
		Assert.Equal(
			Natural.Zero,
			actual
		);
	}
}

public class CheckedDecrementOperators {
	private static T CheckedDecrement<T>( T n ) where T : IDecrementOperators<T> =>
		checked(--n);

	[Theory]
	[InlineData( 1u, 0u )]   // Sanity
	[InlineData( 2u, 1u )]   // Sanity
	[InlineData( 10u, 9u )]   // Sanity
	public void Sanity( uint value, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			CheckedDecrement( new Natural( value ) )
		);

	[Fact]
	public void Overflow() =>
		Assert.Equal(
			new Natural( 0xFFFF_FFFFu ),
			CheckedDecrement(
				FListNatural( 1u, 0u )
			)
		);

	[Fact]
	public void CascadingOverflow() =>
		Assert.Equal(
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ),
			CheckedDecrement(
				FListNatural( 1u, 0u, 0u )
			)
		);

	[Fact]
	public void SingleItemBadUnderflow() {
		System.Exception exc = Record.Exception(
			() =>
				CheckedDecrement(
					Natural.Zero
				)
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Fact( Skip = "Postfix won't work until Natural is a ValueType" )]
	public void Postfix() {
		Natural actual = Natural.One;
		Assert.Equal(
			Natural.One,
			checked( actual-- )
		);
		Assert.Equal(
			Natural.Zero,
			actual
		);
	}
}