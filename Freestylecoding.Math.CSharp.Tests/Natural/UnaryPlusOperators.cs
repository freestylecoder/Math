using System.Numerics;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class UnaryPlusOperators {
	private static T Plus<T>( T n ) where T : IUnaryPlusOperators<T, T> =>
		+n;
	[Theory]
	[InlineData( 0u )]   // Sanity
	[InlineData( 1u )]   // Sanity
	[InlineData( 42u )]   // Sanity
	public void Sanity( uint value ) =>
		Assert.Equal(
			new Natural( value ),
			Plus( new Natural( value ) )
		);
}