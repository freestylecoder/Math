using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class UnaryNegationOperators {
	private static T Negation<T>( T n ) where T : IUnaryNegationOperators<T, T> =>
		-n;
	private static T CheckedNegation<T>( T n ) where T : IUnaryNegationOperators<T, T> =>
		checked(-n);

	[Theory]
	[InlineData( 0u )]   // Sanity
	[InlineData( 1u )]   // Sanity
	[InlineData( 42u )]   // Sanity
	public void Sanity( uint value ) {
		System.Exception exc = Record.Exception(
			() =>
				Negation( new Natural( value ) )
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Theory]
	[InlineData( 0u )]   // Sanity
	[InlineData( 1u )]   // Sanity
	[InlineData( 42u )]   // Sanity
	public void CheckedSanity( uint value ) {
		System.Exception exc = Record.Exception(
			() =>
				UnaryNegationOperators.CheckedNegation( new Natural( value ) )
		);
		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}
}