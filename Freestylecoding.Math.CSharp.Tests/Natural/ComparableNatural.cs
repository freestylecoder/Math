using System;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class ComparableNatural {
	private static readonly Random random = new Random();

	private static Func<int,bool> eq = (int actual) => actual == 0;
	private static Func<int,bool> gt = (int actual) => actual > 0;
	private static Func<int,bool> lt = (int actual) => actual < 0;

	[Theory]
	[InlineData( 0u, 0u, 0 )]
	[InlineData( 1u, 1u, 0 )]
	[InlineData( 1u, 0u, 1 )]
	[InlineData( 0u, 1u, -1 )]
	public void Sanity( uint left, uint right, int expected ) {
		Func<int, bool> TestFunc = expected switch {
			0 => eq,
			_ when expected > 0 => gt,
			_ when expected < 0 => lt,
			_ => throw new Exception( "Not Possible" )
		};

		Assert.True(
			TestFunc( ( new Natural( left ) as IComparable<Natural> ).CompareTo( new Natural( right ) ) )
		);
	}

	[Fact( DisplayName = "Equals" )]
	public void EqualsTest() {
		uint ui = Convert.ToUInt32( random.Next() );
		IComparable<Natural> left = new Natural( ui ) as IComparable<Natural>;
		Natural right = new Natural( ui );
		Assert.True( eq( left.CompareTo( right ) ) );
	}

	[Fact]
	public void GreaterThan() {
		IComparable<Natural> left = new Natural( Convert.ToUInt32( random.Next( 1000, 2000 ) ) ) as IComparable<Natural>;
		Natural right = new Natural( Convert.ToUInt32( random.Next( 1000 ) ) );
		Assert.True( gt( left.CompareTo( right ) ) );
	}

	[Fact]
	public void LessThan() {
		IComparable<Natural> left = new Natural( Convert.ToUInt32( random.Next( 1000 ) ) ) as IComparable<Natural>;
		Natural right = new Natural( Convert.ToUInt32( random.Next( 1000, 2000 ) ) );
		Assert.True( lt( left.CompareTo( right ) ) );
	}
}