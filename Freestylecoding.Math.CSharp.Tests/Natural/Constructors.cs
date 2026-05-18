using System;
using Xunit;

namespace Natural.Tests;

using Natural = Freestylecoding.Math.Natural;
using static Freestylecoding.Math.CSharp.Tests.Helpers;

public class Constructors() {
	// Yes, it's kinda hacky that we're using Parse here
	// There is no "good" way to put in a large Natural the default way
	//  and compare it against the expected value
	// This is why Parse is part of the sanity tests
	[Theory]
	[InlineData( "0", 0u, 0u, 0u )]
	[InlineData( "4294967297", 0u, 1u, 1u )]
	[InlineData( "18446744073709551616", 1u, 0u, 0u )]
	[InlineData( "18446744073709551617", 1u, 0u, 1u )]
	public void DefaultCtor( string expected, uint a, uint b, uint c ) =>
		Assert.Equal( Natural.Parse( expected ), FListNatural( a, b, c ) );

	[Fact]
	public void EmptyCtor() =>
		Assert.Equal( Natural.Zero, new Natural() );

	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 0u, 1u, 1u )]
	[InlineData( 1u, 0u, 0u )]
	[InlineData( 1u, 0u, 1u )]
	public void CopyCtor( uint a, uint b, uint c ) {
		Natural expected = FListNatural( a, b, c );
		Assert.Equal( expected, new Natural( expected ) );
	}

	[Theory]
	[InlineData( (byte)0 )]
	[InlineData( (byte)1 )]
	[InlineData( (byte)2 )]
	[InlineData( (byte)5 )]
	[InlineData( (byte)100 )]
	public void UInt8Ctro( byte actual ) =>
		Assert.Equal( FListNatural( (uint)actual ), new Natural( actual ) );

	[Theory]
	[InlineData( (ushort)0 )]
	[InlineData( (ushort)1 )]
	[InlineData( (ushort)2 )]
	[InlineData( (ushort)5 )]
	[InlineData( (ushort)100 )]
	public void UInt16Ctro( ushort actual ) =>
		Assert.Equal( FListNatural( (uint)actual ), new Natural( actual ) );

	[Theory]
	[InlineData( 0u )]
	[InlineData( 1u )]
	[InlineData( 2u )]
	[InlineData( 5u )]
	[InlineData( 100u )]
	public void UInt32Ctro( uint actual ) =>
		Assert.Equal( FListNatural( actual ), new Natural( actual ) );

	[Theory]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL )]
	[InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001uL )]
	[InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000uL )]
	[InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL )]
	[InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078uL )]
	[InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0uL )]
	[InlineData( 0xFFFF_0000u, 0x0000_0000u, 0xFFFF_0000_0000_0000uL )]
	[InlineData( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0xFFFF_EEEE_DDDD_CCCCuL )]
	public void UInt64Ctor( uint expectedHigh, uint expectedLow, ulong actual ) =>
		Assert.Equal( FListNatural( expectedHigh, expectedLow ), new Natural( actual ) );

	[Theory]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0000uL )]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0001uL )]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0001_0000_0000uL )]
	[InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0001uL, 0x0000_0000_0000_0000uL )]
	[InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001_0000_0000uL, 0x0000_0000_0000_0000uL )]
	[InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL, 0x0000_0001_0000_0001uL )]
	[InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u, 0x1200_0340_0560_0078uL, 0x1234_5678_9ABC_DEF0uL )]
	public void UInt128Ctor( uint i0, uint i1, uint i2, uint i3, ulong l0, ulong l1 ) =>
		Assert.Equal( FListNatural( i0, i1, i2, i3 ), new Natural( new UInt128( l0, l1 ) ) );

	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 0u, 0u, 1u )]
	[InlineData( 0u, 1u, 0u )]
	[InlineData( 0u, 1u, 1u )]
	[InlineData( 1u, 0u, 0u )]
	[InlineData( 1u, 0u, 1u )]
	[InlineData( 1u, 1u, 0u )]
	[InlineData( 1u, 1u, 1u )]
	public void UInt32Sequence( uint a, uint b, uint c ) =>
		Assert.Equal(
			FListNatural( a, b, c ),
			new Natural( [a, b, c] )
		);
}