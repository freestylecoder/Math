using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class BitwiseAnd {
	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 1u, 0u, 0u )]
	[InlineData( 0u, 1u, 0u )]
	[InlineData( 1u, 1u, 1u )]
	[InlineData( 12u, 10u, 8u )]
	public void Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			new Natural( left ) & new Natural( right )
		);

	[Fact]
	public void BiggerLeft() =>
		Assert.Equal(
			Natural.Unit,
			FListNatural( 0xFu, 0x00000101u ) & new Natural( 0x00010001u )
		);

	[Fact]
	public void BiggerRight() =>
		Assert.Equal(
			Natural.Unit,
			new Natural( 0x00010001u ) & FListNatural( 0xFu, 0x00000101u )
		);

	[Fact]
	public void Large() =>
		Assert.Equal(
			FListNatural( 1u, 0u, 0u, 0u ),
			FListNatural( 1u, 1u, 0u, 0u ) & FListNatural( 1u, 0u, 1u, 0u )
		);
}

public class BitwiseOr {
	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 1u, 0u, 1u )]
	[InlineData( 0u, 1u, 1u )]
	[InlineData( 1u, 1u, 1u )]
	[InlineData( 12u, 10u, 14u )]
	public void Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			new Natural( left ) | new Natural( right )
		);

	[Fact]

	public void BiggerLeft() =>
		Assert.Equal(
			FListNatural( 0xFu, 0x10101u ),
			FListNatural( 0xFu, 0x00000101u ) | new Natural( 0x00010001u )
		);

	[Fact]
	public void BiggerRight() =>
		Assert.Equal(
			FListNatural( 0xFu, 0x10101u ),
			new Natural( 0x00010001u ) | FListNatural( 0xFu, 0x00000101u )
		);

	[Fact]
	public void Large() =>
		Assert.Equal(
			FListNatural( 1u, 1u, 1u, 0u ),
			FListNatural( 1u, 1u, 0u, 0u ) | FListNatural( 1u, 0u, 1u, 0u )
		);
}

public class BitwiseXor {
	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 1u, 0u, 1u )]
	[InlineData( 0u, 1u, 1u )]
	[InlineData( 1u, 1u, 0u )]
	[InlineData( 12u, 10u, 6u )]
	public void Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			new Natural( left ) ^ new Natural( right )
		);

	[Fact]
	public void BiggerLeft() =>
		Assert.Equal(
			FListNatural( 0xFu, 0x10100u ),
			FListNatural( 0xFu, 0x00000101u ) ^ new Natural( 0x00010001u )
		);

	[Fact]
	public void BiggerRight() =>
		Assert.Equal(
			FListNatural( 0xFu, 0x10100u ),
			new Natural( 0x00010001u ) ^ FListNatural( 0xFu, 0x00000101u )
		);

	[Fact]
	public void Large() =>
		Assert.Equal(
			FListNatural( 0u, 1u, 1u, 0u ),
			FListNatural( 1u, 1u, 0u, 0u ) ^ FListNatural( 1u, 0u, 1u, 0u )
		);
}

public class BitwiseNot {
	[Theory]
	[InlineData( 0xFFFF_FFFEu, 1u )]
	[InlineData( 1u, 0xFFFF_FFFEu )]
	public void Sanity( uint value, uint expected ) =>
		Assert.Equal( new Natural( expected ), ~new Natural( value ) );

	[Fact]
	public void Bigger() =>
		Assert.Equal(
			FListNatural( 0xF012_3456u, 0x789A_BCDEu ),
			~FListNatural( 0x0FED_CBA9u, 0x8765_4321u )
		);
}

public class LeftShift {
	[Theory]
	[InlineData( 1u, 1, 2u )]   // Sanity
	[InlineData( 0xFu, 2, 0x3Cu )]  // multiple bits
	public void Sanity( uint left, int right, uint expected ) =>
		Assert.Equal( new Natural( expected ), new Natural( left ) << right );

	[Fact]
	public void Overflow() =>
		Assert.Equal( FListNatural( 1u, 0xFFFFFFFEu ), new Natural( 0xFFFFFFFFu ) << 1 );

	[Fact]
	public void MultipleOverflow() =>
		Assert.Equal( FListNatural( 0x5u, 0xFFFFFFF8u ), new Natural( 0xBFFFFFFFu ) << 3 );

	[Fact]
	public void OverOneUInt() =>
		Assert.Equal( FListNatural( 8u, 0u, 0u ), Natural.Unit << 67 );
}

public class RightShift {
	[Theory]
	[InlineData( 1u, 1, 0u )]   // Sanity
	[InlineData( 0xFu, 2, 0x3u )]   // multiple bits
	[InlineData( 0x3Cu, 2, 0xFu )]  // multiple bits
	public void Sanity( uint left, int right, uint expected ) =>
		Assert.Equal( new Natural( expected ), new Natural( left ) >> right );

	[Fact]
	public void Underflow() =>
		Assert.Equal( new Natural( 0x7FFFFFFFu ), new Natural( 0xFFFFFFFFu ) >> 1 );

	[Fact]
	public void MultipleUnderflow() =>
		Assert.Equal( FListNatural( 0x1u, 0x5FFFFFFFu ), FListNatural( 0xAu, 0xFFFFFFFFu ) >> 3 );

	[Fact]
	public void OverOneUInt() =>
		Assert.Equal( Natural.Unit, FListNatural( 0x10u, 0u, 0u ) >> 68 );

	[Fact]
	public void ReduceToZero() =>
		Assert.Equal( Natural.Zero, FListNatural( 0x10u, 0u, 0u ) >> 99 );
}