using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class Equality {
	[Theory]
	[InlineData( 0u, 0u, true )]
	[InlineData( 0u, 1u, false )]
	[InlineData( 1u, 0u, false )]
	[InlineData( 1u, 1u, true )]
	public void Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal(
			expected,
			new Natural( left ) == new Natural( right )
		);

	[Fact]
	public void BiggerLeft() =>
		Assert.False( FListNatural( 0xBADu, 0xDEADBEEFu ) == new Natural( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.False( new Natural( 0xDEADBEEFu ) == FListNatural( 0xBADu, 0xDEADBEEFu ) );
}

public class GreaterThan {
	[Theory]
	[InlineData( 0u, 1u, false )]
	[InlineData( 1u, 0u, true )]
	[InlineData( 1u, 1u, false )]
	public void Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal( expected, new Natural( left ) > new Natural( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.True( FListNatural( 0xBADu, 0xDEADBEEFu ) > new Natural( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.False( new Natural( 0xDEADBEEFu ) > FListNatural( 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThan() =>
		Assert.True( FListNatural( 1u, 1u ) > FListNatural( 1u, 0u ) );

	[Fact]
	public void CascadeEqual() =>
		Assert.False( FListNatural( 1u, 0u ) > FListNatural( 1u, 0u ) );

	[Fact]
	public void CascadeLessThan() =>
		Assert.False( FListNatural( 1u, 0u ) > FListNatural( 1u, 1u ) );
}

public class LessThan {
	[Theory]
	[InlineData( 0u, 1u, true )]
	[InlineData( 1u, 0u, false )]
	[InlineData( 1u, 1u, false )]
	public void Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal( expected, new Natural( left ) < new Natural( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.False( FListNatural( 0xBADu, 0xDEADBEEFu ) < new Natural( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.True( new Natural( 0xDEADBEEFu ) < FListNatural( 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThan() =>
		Assert.False( FListNatural( 1u, 1u ) < FListNatural( 1u, 0u ) );

	[Fact]
	public void CascadeEqual() =>
		Assert.False( FListNatural( 1u, 0u ) < FListNatural( 1u, 0u ) );

	[Fact]
	public void CascadeLessThan() =>
		Assert.True( FListNatural( 1u, 0u ) < FListNatural( 1u, 1u ) );
}

public class GreaterThanOrEqual {
	[Theory]
	[InlineData( 0u, 1u, false )]
	[InlineData( 1u, 0u, true )]
	[InlineData( 1u, 1u, true )]
	public void Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal( expected, new Natural( left ) >= new Natural( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.True( FListNatural( 0xBADu, 0xDEADBEEFu ) >= new Natural( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.False( new Natural( 0xDEADBEEFu ) >= FListNatural( 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThan() =>
		Assert.True( FListNatural( 1u, 1u ) >= FListNatural( 1u, 0u ) );

	[Fact]
	public void CascadeEqual() =>
		Assert.True( FListNatural( 1u, 0u ) >= FListNatural( 1u, 0u ) );

	[Fact]
	public void CascadeLessThan() =>
		Assert.False( FListNatural( 1u, 0u ) >= FListNatural( 1u, 1u ) );
}

public class LessThanOrEqual {
	[Theory]
	[InlineData( 0u, 1u, true )]
	[InlineData( 1u, 0u, false )]
	[InlineData( 1u, 1u, true )]
	public void Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal( expected, new Natural( left ) <= new Natural( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.False( FListNatural( 0xBADu, 0xDEADBEEFu ) <= new Natural( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.True( new Natural( 0xDEADBEEFu ) <= FListNatural( 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThan() =>
		Assert.False( FListNatural( 1u, 1u ) <= FListNatural( 1u, 0u ) );

	[Fact]
	public void CascadeEqual() =>
		Assert.True( FListNatural( 1u, 0u ) <= FListNatural( 1u, 0u ) );

	[Fact]
	public void CascadeLessThan() =>
		Assert.True( FListNatural( 1u, 0u ) <= FListNatural( 1u, 1u ) );
}

public class Inequality {
	[Theory]
	[InlineData( 0u, 0u, false )]
	[InlineData( 0u, 1u, true )]
	[InlineData( 1u, 0u, true )]
	[InlineData( 1u, 1u, false )]
	public void Sanity( uint left, uint right, bool expected ) =>
		Assert.Equal( expected, new Natural( left ) != new Natural( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.True( FListNatural( 0xBADu, 0xDEADBEEFu ) != new Natural( 0xDEADBEEFu ) );

    [Fact]
	public void BiggerRight() =>
		Assert.True( new Natural( 0xDEADBEEFu ) != FListNatural( 0xBADu, 0xDEADBEEFu ) );
}