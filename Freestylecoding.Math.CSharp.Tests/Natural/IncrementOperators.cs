using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class IncrementOperators {
	private static void AssertIncrement<T>( T expected, T actual ) where T : IIncrementOperators<T> {
		// This one checks the return of the Increment expression
		Assert.Equal<T>( expected, ++actual );
		// This one ensures the original was mutated
		Assert.Equal<T>( expected, actual );
	}

	[Theory]
	[InlineData( 0u, 1u )]   // Sanity
	[InlineData( 1u, 2u )]   // Sanity
	[InlineData( 9u, 10u )]   // Sanity
	public void Sanity( uint value, uint expected ) =>
		AssertIncrement(
			new Natural( expected ),
			value
		);

	[Fact]
	public void Overflow() =>
		AssertIncrement(
			FListNatural( 1u, 0u ),
			new Natural( 0xFFFF_FFFFu )
		);

	[Fact]
	public void CascadingOverflow() =>
		AssertIncrement(
			FListNatural( 1u, 0u, 0u ),
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu )
		);

	[Fact( Skip = "Postfix won't work until Natural is a ValueType" )]
	public void Postfix() {
		Natural actual = Natural.Zero;
		Assert.Equal(
			Natural.Zero,
			actual++
		);
		Assert.Equal(
			Natural.One,
			actual
		);
	}
}

public class CheckedIncrementOperators {
	private static void AssertCheckedIncrement<T>( T expected, T actual ) where T : IIncrementOperators<T> {
		checked {
			// This one checks the return of the Increment expression
			Assert.Equal<T>( expected, ++actual );
			// This one ensures the original was mutated
			Assert.Equal<T>( expected, actual );
		}
	}

	[Theory]
	[InlineData( 0u, 1u )]   // Sanity
	[InlineData( 1u, 2u )]   // Sanity
	[InlineData( 9u, 10u )]   // Sanity
	public void Sanity( uint value, uint expected ) =>
		AssertCheckedIncrement(
			new Natural( expected ),
			value
		);

	[Fact]
	public void Overflow() =>
		AssertCheckedIncrement(
			FListNatural( 1u, 0u ),
			new Natural( 0xFFFF_FFFFu )
		);

	[Fact]
	public void CascadingOverflow() =>
		AssertCheckedIncrement(
			FListNatural( 1u, 0u, 0u ),
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu )
		);

	[Fact( Skip = "Postfix won't work until Natural is a ValueType" )]
	public void Postfix() {
		Natural actual = Natural.Zero;
		Assert.Equal(
			Natural.Zero,
			checked( actual++ )
		);
		Assert.Equal(
			Natural.One,
			actual
		);
	}
}