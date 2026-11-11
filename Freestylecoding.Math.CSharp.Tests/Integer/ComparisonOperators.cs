using System;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Integer = Freestylecoding.Math.Integer;

public class Equality {
	[Theory]
	[InlineData( 0, 0, true )]
	[InlineData( 0, 1, false )]
	[InlineData( 1, 0, false )]
	[InlineData( 1, 1, true )]
	[InlineData( 0, -1, false )]
	[InlineData( -1, 0, false )]
	[InlineData( -1, 1, false )]
	[InlineData( 1, -1, false )]
	[InlineData( -1, -1, true )]
	public void Sanity( int left, int right, bool expected ) =>
		Assert.Equal( expected, new Integer( left ) == new Integer( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.False( FListInteger( 0xBADu, 0xDEADBEEFu ) == FListInteger( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.False( FListInteger( 0xDEADBEEFu ) == FListInteger( 0xBADu, 0xDEADBEEFu ) );
}

public class GreaterThan {
	[Theory]
	[InlineData( 0, 1, false )]
	[InlineData( 1, 0, true )]
	[InlineData( 0, -1, true )]
	[InlineData( -1, 0, false )]
	[InlineData( 1, 1, false )]
	[InlineData( -1, 1, false )]
	[InlineData( 1, -1, true )]
	[InlineData( -1, -1, false )]
	public void Sanity( int left, int right, bool expected ) =>
		Assert.Equal( expected, new Integer( left ) > new Integer( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.True( FListInteger( 0xBADu, 0xDEADBEEFu ) > FListInteger( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.False( FListInteger( 0xDEADBEEFu ) > FListInteger( 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThan() =>
		Assert.True( FListInteger( 1u, 1u ) > FListInteger( 1u, 0u ) );

	[Fact]
	public void CascadeEqual() =>
		Assert.False( FListInteger( 1u, 0u ) > FListInteger( 1u, 0u ) );

	[Fact]
	public void CascadeLessThan() =>
		Assert.False( FListInteger( 1u, 0u ) > FListInteger( 1u, 1u ) );

	[Fact]
	public void BiggerLeftNegative() =>
		Assert.False( FListInteger( true, 0xBADu, 0xDEADBEEFu ) > new Integer( 0xDEADBEEFu, true ) );

	[Fact]
	public void BiggerRightNegative() =>
		Assert.True( new Integer( 0xDEADBEEFu, true ) > FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanNegative() =>
		Assert.False( FListInteger( true, 1u, 1u ) > FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeEqualNegative() =>
		Assert.False( FListInteger( true, 1u, 0u ) > FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanNegative() =>
		Assert.True( FListInteger( true, 1u, 0u ) > FListInteger( true, 1u, 1u ) );

	[Fact]
	public void BiggerLeftMixedNegativeLeft() =>
		Assert.False( FListInteger( true, 0xBADu, 0xDEADBEEFu ) > new Integer( 0xDEADBEEFu, false ) );

	[Fact]
	public void BiggerRightMixedNegativeLeft() =>
		Assert.False( new Integer( 0xDEADBEEFu, true ) > FListInteger( false, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanMixedNegativeLeft() =>
		Assert.False( FListInteger( true, 1u, 1u ) > FListInteger( false, 1u, 0u ) );

	[Fact]
	public void CascadeEqualMixedNegativeLeft() =>
		Assert.False( FListInteger( true, 1u, 0u ) > FListInteger( false, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanMixedNegativeLeft() =>
		Assert.False( FListInteger( true, 1u, 0u ) > FListInteger( false, 1u, 1u ) );

	[Fact]
	public void BiggerLeftMixedNegativeRight() =>
		Assert.True( FListInteger( false, 0xBADu, 0xDEADBEEFu ) > new Integer( 0xDEADBEEFu, true ) );

	[Fact]
	public void BiggerRightMixedNegativeRight() =>
		Assert.True( new Integer( 0xDEADBEEFu, false ) > FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanMixedNegativeRight() =>
		Assert.True( FListInteger( false, 1u, 1u ) > FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeEqualMixedNegativeRight() =>
		Assert.True( FListInteger( false, 1u, 0u ) > FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanMixedNegativeRight() =>
		Assert.True( FListInteger( false, 1u, 0u ) > FListInteger( true, 1u, 1u ) );
}

public class LessThan {
	[Theory]
	[InlineData( 0, 1, true )]
	[InlineData( 1, 0, false )]
	[InlineData( 0, -1, false )]
	[InlineData( -1, 0, true )]
	[InlineData( 1, 1, false )]
	[InlineData( -1, 1, true )]
	[InlineData( 1, -1, false )]
	[InlineData( -1, -1, false )]
	public void Sanity( int left, int right, bool expected ) =>
		Assert.Equal( expected, new Integer( left ) < new Integer( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.False( FListInteger( 0xBADu, 0xDEADBEEFu ) < FListInteger( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.True( FListInteger( 0xDEADBEEFu ) < FListInteger( 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThan() =>
		Assert.False( FListInteger( 1u, 1u ) < FListInteger( 1u, 0u ) );

	[Fact]
	public void CascadeEqual() =>
		Assert.False( FListInteger( 1u, 0u ) < FListInteger( 1u, 0u ) );

	[Fact]
	public void CascadeLessThan() =>
		Assert.True( FListInteger( 1u, 0u ) < FListInteger( 1u, 1u ) );

	[Fact]
	public void BiggerLeftNegative() =>
		Assert.True( FListInteger( true, 0xBADu, 0xDEADBEEFu ) < new Integer( 0xDEADBEEFu, true ) );

	[Fact]
	public void BiggerRightNegative() =>
		Assert.False( new Integer( 0xDEADBEEFu, true ) < FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanNegative() =>
		Assert.True( FListInteger( true, 1u, 1u ) < FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeEqualNegative() =>
		Assert.False( FListInteger( true, 1u, 0u ) < FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanNegative() =>
		Assert.False( FListInteger( true, 1u, 0u ) < FListInteger( true, 1u, 1u ) );

	[Fact]
	public void BiggerLeftMixedNegativeLeft() =>
		Assert.True( FListInteger( true, 0xBADu, 0xDEADBEEFu ) < new Integer( 0xDEADBEEFu, false ) );

	[Fact]
	public void BiggerRightMixedNegativeLeft() =>
		Assert.True( new Integer( 0xDEADBEEFu, true ) < FListInteger( false, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanMixedNegativeLeft() =>
		Assert.True( FListInteger( true, 1u, 1u ) < FListInteger( false, 1u, 0u ) );

	[Fact]
	public void CascadeEqualMixedNegativeLeft() =>
		Assert.True( FListInteger( true, 1u, 0u ) < FListInteger( false, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanMixedNegativeLeft() =>
		Assert.True( FListInteger( true, 1u, 0u ) < FListInteger( false, 1u, 1u ) );

	[Fact]
	public void BiggerLeftMixedNegativeRight() =>
		Assert.False( FListInteger( false, 0xBADu, 0xDEADBEEFu ) < new Integer( 0xDEADBEEFu, true ) );

	[Fact]
	public void BiggerRightMixedNegativeRight() =>
		Assert.False( new Integer( 0xDEADBEEFu, false ) < FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanMixedNegativeRight() =>
		Assert.False( FListInteger( false, 1u, 1u ) < FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeEqualMixedNegativeRight() =>
		Assert.False( FListInteger( false, 1u, 0u ) < FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanMixedNegativeRight() =>
		Assert.False( FListInteger( false, 1u, 0u ) < FListInteger( true, 1u, 1u ) );
}

public class GreaterThanOrEqual {
	[Theory]
	[InlineData( 0, 1, false )]
	[InlineData( 1, 0, true )]
	[InlineData( 0, -1, true )]
	[InlineData( -1, 0, false )]
	[InlineData( 1, 1, true )]
	[InlineData( -1, 1, false )]
	[InlineData( 1, -1, true )]
	[InlineData( -1, -1, true )]
	public void Sanity( int left, int right, bool expected ) =>
		Assert.Equal( expected, new Integer( left ) >= new Integer( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.True( FListInteger( 0xBADu, 0xDEADBEEFu ) >= FListInteger( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.False( FListInteger( 0xDEADBEEFu ) >= FListInteger( 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThan() =>
		Assert.True( FListInteger( 1u, 1u ) >= FListInteger( 1u, 0u ) );

	[Fact]
	public void CascadeEqual() =>
		Assert.True( FListInteger( 1u, 0u ) >= FListInteger( 1u, 0u ) );

	[Fact]
	public void CascadeLessThan() =>
		Assert.False( FListInteger( 1u, 0u ) >= FListInteger( 1u, 1u ) );

	[Fact]
	public void BiggerLeftNegative() =>
		Assert.False( FListInteger( true, 0xBADu, 0xDEADBEEFu ) >= new Integer( 0xDEADBEEFu, true ) );

	[Fact]
	public void BiggerRightNegative() =>
		Assert.True( new Integer( 0xDEADBEEFu, true ) >= FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanNegative() =>
		Assert.False( FListInteger( true, 1u, 1u ) >= FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeEqualNegative() =>
		Assert.True( FListInteger( true, 1u, 0u ) >= FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanNegative() =>
		Assert.True( FListInteger( true, 1u, 0u ) >= FListInteger( true, 1u, 1u ) );

	[Fact]
	public void BiggerLeftMixedNegativeLeft() =>
		Assert.False( FListInteger( true, 0xBADu, 0xDEADBEEFu ) >= new Integer( 0xDEADBEEFu, false ) );

	[Fact]
	public void BiggerRightMixedNegativeLeft() =>
		Assert.False( new Integer( 0xDEADBEEFu, true ) >= FListInteger( false, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanMixedNegativeLeft() =>
		Assert.False( FListInteger( true, 1u, 1u ) >= FListInteger( false, 1u, 0u ) );

	[Fact]
	public void CascadeEqualMixedNegativeLeft() =>
		Assert.False( FListInteger( true, 1u, 0u ) >= FListInteger( false, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanMixedNegativeLeft() =>
		Assert.False( FListInteger( true, 1u, 0u ) >= FListInteger( false, 1u, 1u ) );

	[Fact]
	public void BiggerLeftMixedNegativeRight() =>
		Assert.True( FListInteger( false, 0xBADu, 0xDEADBEEFu ) >= new Integer( 0xDEADBEEFu, true ) );

	[Fact]
	public void BiggerRightMixedNegativeRight() =>
		Assert.True( new Integer( 0xDEADBEEFu, false ) >= FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanMixedNegativeRight() =>
		Assert.True( FListInteger( false, 1u, 1u ) >= FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeEqualMixedNegativeRight() =>
		Assert.True( FListInteger( false, 1u, 0u ) >= FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanMixedNegativeRight() =>
		Assert.True( FListInteger( false, 1u, 0u ) >= FListInteger( true, 1u, 1u ) );
}

public class LessThanOrEqual {
	[Theory]
	[InlineData( 0, 1, true )]
	[InlineData( 1, 0, false )]
	[InlineData( 0, -1, false )]
	[InlineData( -1, 0, true )]
	[InlineData( 1, 1, true )]
	[InlineData( -1, 1, true )]
	[InlineData( 1, -1, false )]
	[InlineData( -1, -1, true )]
	public void Sanity( int left, int right, bool expected ) =>
		Assert.Equal( expected, new Integer( left ) <= new Integer( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.False( FListInteger( 0xBADu, 0xDEADBEEFu ) <= FListInteger( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.True( FListInteger( 0xDEADBEEFu ) <= FListInteger( 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThan() =>
		Assert.False( FListInteger( 1u, 1u ) <= FListInteger( 1u, 0u ) );

	[Fact]
	public void CascadeEqual() =>
		Assert.True( FListInteger( 1u, 0u ) <= FListInteger( 1u, 0u ) );

	[Fact]
	public void CascadeLessThan() =>
		Assert.True( FListInteger( 1u, 0u ) <= FListInteger( 1u, 1u ) );

	[Fact]
	public void BiggerLeftNegative() =>
		Assert.True( FListInteger( true, 0xBADu, 0xDEADBEEFu ) <= new Integer( 0xDEADBEEFu, true ) );

	[Fact]
	public void BiggerRightNegative() =>
		Assert.False( new Integer( 0xDEADBEEFu, true ) <= FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanNegative() =>
		Assert.True( FListInteger( true, 1u, 1u ) <= FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeEqualNegative() =>
		Assert.True( FListInteger( true, 1u, 0u ) <= FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanNegative() =>
		Assert.False( FListInteger( true, 1u, 0u ) <= FListInteger( true, 1u, 1u ) );

	[Fact]
	public void BiggerLeftMixedNegativeLeft() =>
		Assert.True( FListInteger( true, 0xBADu, 0xDEADBEEFu ) <= new Integer( 0xDEADBEEFu, false ) );

	[Fact]
	public void BiggerRightMixedNegativeLeft() =>
		Assert.True( new Integer( 0xDEADBEEFu, true ) <= FListInteger( false, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanMixedNegativeLeft() =>
		Assert.True( FListInteger( true, 1u, 1u ) <= FListInteger( false, 1u, 0u ) );

	[Fact]
	public void CascadeEqualMixedNegativeLeft() =>
		Assert.True( FListInteger( true, 1u, 0u ) <= FListInteger( false, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanMixedNegativeLeft() =>
		Assert.True( FListInteger( true, 1u, 0u ) <= FListInteger( false, 1u, 1u ) );

	[Fact]
	public void BiggerLeftMixedNegativeRight() =>
		Assert.False( FListInteger( false, 0xBADu, 0xDEADBEEFu ) <= new Integer( 0xDEADBEEFu, true ) );

	[Fact]
	public void BiggerRightMixedNegativeRight() =>
		Assert.False( new Integer( 0xDEADBEEFu, false ) <= FListInteger( true, 0xBADu, 0xDEADBEEFu ) );

	[Fact]
	public void CascadeGreaterThanMixedNegativeRight() =>
		Assert.False( FListInteger( false, 1u, 1u ) <= FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeEqualMixedNegativeRight() =>
		Assert.False( FListInteger( false, 1u, 0u ) <= FListInteger( true, 1u, 0u ) );

	[Fact]
	public void CascadeLessThanMixedNegativeRight() =>
		Assert.False( FListInteger( false, 1u, 0u ) <= FListInteger( true, 1u, 1u ) );
}

public class Inequality {
	[Theory]
	[InlineData( 0, 0, false )]
	[InlineData( 0, 1, true )]
	[InlineData( 1, 0, true )]
	[InlineData( 1, 1, false )]
	[InlineData( 0, -1, true )]
	[InlineData( -1, 0, true )]
	[InlineData( -1, 1, true )]
	[InlineData( 1, -1, true )]
	[InlineData( -1, -1, false )]
	public void Sanity( int left, int right, bool expected ) =>
		Assert.Equal( expected, new Integer( left ) != new Integer( right ) );

	[Fact]
	public void BiggerLeft() =>
		Assert.True( FListInteger( 0xBADu, 0xDEADBEEFu ) != FListInteger( 0xDEADBEEFu ) );

	[Fact]
	public void BiggerRight() =>
		Assert.True( FListInteger( 0xDEADBEEFu ) != FListInteger( 0xBADu, 0xDEADBEEFu ) );
}