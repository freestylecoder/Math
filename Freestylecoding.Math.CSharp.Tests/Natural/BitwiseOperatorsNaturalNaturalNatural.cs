using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class BitwiseOperators {
	private static T AND<T>( T left, T right ) where T : IBitwiseOperators<T, T, T> =>
		left & right;
	private static T OR<T>( T left, T right ) where T : IBitwiseOperators<T, T, T> =>
		left | right;
	private static T XOR<T>( T left, T right ) where T : IBitwiseOperators<T, T, T> =>
		left ^ right;
	private static T NOT<T>( T value ) where T : IBitwiseOperators<T, T, T> =>
		~value;

	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 1u, 0u, 0u )]
	[InlineData( 0u, 1u, 0u )]
	[InlineData( 1u, 1u, 1u )]
	[InlineData( 12u, 10u, 8u )]
	public void AND_Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			AND(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void AND_BiggerLeft() =>
		Assert.Equal(
			Natural.Unit,
			AND(
				FListNatural( 0xFu, 0x00000101u ),
				new Natural( 0x00010001u )
			)
		);

	[Fact]
	public void AND_BiggerRight() =>
		Assert.Equal(
			Natural.Unit,
			AND(
				new Natural( 0x00010001u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[FactAttribute]
	public void AND_Large() =>
		Assert.Equal(
			FListNatural( 1u, 0u, 0u, 0u ),
			AND(
				FListNatural( 1u, 1u, 0u, 0u ),
				FListNatural( 1u, 0u, 1u, 0u )
			)
		);

	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 1u, 0u, 1u )]
	[InlineData( 0u, 1u, 1u )]
	[InlineData( 1u, 1u, 1u )]
	[InlineData( 12u, 10u, 14u )]
	public void OR_Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			OR(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void OR_BiggerLeft() =>
		Assert.Equal(
			FListNatural( 0xFu, 0x10101u ),
			OR(
				FListNatural( 0xFu, 0x00000101u ),
				new Natural( 0x00010001u )
			)
		);

	[Fact]
	public void OR_BiggerRight() =>
		Assert.Equal(
			FListNatural( 0xFu, 0x10101u ),
			OR(
				new Natural( 0x00010001u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[FactAttribute]
	public void OR_Large() =>
		Assert.Equal(
			FListNatural( 1u, 1u, 1u, 0u ),
			OR(
				FListNatural( 1u, 1u, 0u, 0u ),
				FListNatural( 1u, 0u, 1u, 0u )
			)
		);

	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 1u, 0u, 1u )]
	[InlineData( 0u, 1u, 1u )]
	[InlineData( 1u, 1u, 0u )]
	[InlineData( 12u, 10u, 6u )]
	public void XOR_Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			XOR(
				new Natural( left ),
				new Natural( right )
			)
		);

	[Fact]
	public void XOR_BiggerLeft() =>
		Assert.Equal(
			FListNatural( 0xFu, 0x10100u ),
			XOR(
				FListNatural( 0xFu, 0x00000101u ),
				new Natural( 0x00010001u )
			)
		);

	[Fact]
	public void XOR_BiggerRight() =>
		Assert.Equal(
			FListNatural( 0xFu, 0x10100u ),
			XOR(
				new Natural( 0x00010001u ),
				FListNatural( 0xFu, 0x00000101u )
			)
		);

	[Fact]
	public void XOR_Large() =>
		Assert.Equal(
			FListNatural( 0u, 1u, 1u, 0u ),
			XOR(
				FListNatural( 1u, 1u, 0u, 0u ),
				FListNatural( 1u, 0u, 1u, 0u )
			)
		);

	[Theory]
	[InlineData( 0xFFFF_FFFEu, 1u )]
	[InlineData( 1u, 0xFFFF_FFFEu )]
	public void NOT_Sanity( uint value, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			NOT(
				new Natural( value )
			)
		);

	[Fact]
	public void NOT_Bigger() =>
		Assert.Equal(
			FListNatural( 0xF012_3456u, 0x789A_BCDEu ),
			NOT(
				FListNatural( 0x0FED_CBA9u, 0x8765_4321u )
			)
		);
}