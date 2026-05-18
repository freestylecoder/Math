using System.Linq;
using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class BinaryNumber {
	private static bool P2<T>( T value ) where T : IBinaryNumber<T> =>
		T.IsPow2( value );
	private static T L2<T>( T value ) where T : IBinaryNumber<T> =>
		T.Log2( value );

	[Fact]
	public void IsPow2() {
		foreach( int value in Enumerable.Range( 0, 32 ) )
			Assert.True( P2( Natural.Unit << value ) );

		Assert.Equal(
			P2( 0u ),
			P2( new Natural( 0u ) )
		);
		Assert.Equal(
			P2( 1u ),
			P2( new Natural( 1u ) )
		);
		Assert.Equal(
			P2( 3u ),
			P2( new Natural( 3u ) )
		);

		Assert.True( P2( FListNatural( 1u, 0u ) ) );
		Assert.True( P2( FListNatural( 1u, 0u, 0u ) ) );
		Assert.True( P2( FListNatural( 1u, 0u, 0u, 0u ) ) );

		Assert.False( P2( FListNatural( 1u, 1u ) ) );
		Assert.False( P2( FListNatural( 3u, 0u ) ) );
		Assert.False( P2( FListNatural( 3u, 0u, 0u ) ) );
		Assert.False( P2( FListNatural( 1u, 1u, 0u ) ) );
		Assert.False( P2( FListNatural( 1u, 0u, 1u ) ) );
		Assert.False( P2( FListNatural( 1u, 0u, 0u, 1u ) ) );
		Assert.False( P2( FListNatural( 1u, 0u, 1u, 0u ) ) );
		Assert.False( P2( FListNatural( 1u, 1u, 0u, 0u ) ) );
		Assert.False( P2( FListNatural( 3u, 0u, 0u, 0u ) ) );
	}

	[Fact]
	public void Log2() {
		for( int value = 0; value < 32; value++ )
			Assert.Equal(
				new Natural( L2( 1u << value ) ),
				L2( Natural.Unit << value )
			);

		for( uint value = 0; value < 32; value++ )
			Assert.Equal(
				new Natural( L2( value ) ),
				L2( new Natural( value ) )
			);

		Assert.Equal( new Natural( 32u ), L2( FListNatural( 1u, 0u ) ) );
		Assert.Equal( new Natural( 64u ), L2( FListNatural( 1u, 0u, 0u ) ) );
		Assert.Equal( new Natural( 96u ), L2( FListNatural( 1u, 0u, 0u, 0u ) ) );

		Assert.Equal( new Natural( 32u ), L2( FListNatural( 1u, 1u ) ) );
		Assert.Equal( new Natural( 33u ), L2( FListNatural( 3u, 0u ) ) );
		Assert.Equal( new Natural( 64u ), L2( FListNatural( 1u, 1u, 0u ) ) );
		Assert.Equal( new Natural( 64u ), L2( FListNatural( 1u, 0u, 1u ) ) );
		Assert.Equal( new Natural( 65u ), L2( FListNatural( 3u, 0u, 0u ) ) );
		Assert.Equal( new Natural( 96u ), L2( FListNatural( 1u, 0u, 0u, 1u ) ) );
		Assert.Equal( new Natural( 96u ), L2( FListNatural( 1u, 0u, 1u, 0u ) ) );
		Assert.Equal( new Natural( 96u ), L2( FListNatural( 1u, 1u, 0u, 0u ) ) );
		Assert.Equal( new Natural( 97u ), L2( FListNatural( 3u, 0u, 0u, 0u ) ) );
	}
}