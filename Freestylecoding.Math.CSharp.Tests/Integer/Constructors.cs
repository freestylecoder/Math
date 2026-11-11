using System;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Integer = Freestylecoding.Math.Integer;

public class Constructors {
	// Yes, it's kinda hacky that we're using Parse here
	// There is no "good" way to put in a large Natural the default way
	//  and compare it against the expected value
	// This is why Parse is part of the sanity tests
	[Theory]
	[InlineData( "0", 0u, 0u, 0u, false )]
	[InlineData( "4294967297", 0u, 1u, 1u, false )]
	[InlineData( "18446744073709551616", 1u, 0u, 0u, false )]
	[InlineData( "18446744073709551617", 1u, 0u, 1u, false )]
	[InlineData( "-0", 0u, 0u, 0u, false )]
	[InlineData( "-4294967297", 0u, 1u, 1u, true )]
	[InlineData( "-18446744073709551616", 1u, 0u, 0u, true )]
	[InlineData( "-18446744073709551617", 1u, 0u, 1u, true )]
	public void DefaultCtor( string expected, uint a, uint b, uint c, bool neg ) =>
		Assert.Equal( Integer.Parse( expected ), FListInteger( neg, a, b, c ) );

	[Fact]
	public void EmptyCtor() =>
		Assert.Equal( Integer.Zero, new Integer() );

	[Theory]
	[InlineData( 0u, 0u, 0u, false )]
	[InlineData( 0u, 1u, 1u, false )]
	[InlineData( 1u, 0u, 0u, false )]
	[InlineData( 1u, 0u, 1u, false )]
	[InlineData( 0u, 1u, 1u, true )]
	[InlineData( 1u, 0u, 0u, true )]
	[InlineData( 1u, 0u, 1u, true )]
	public void CopyCtor( uint a, uint b, uint c, bool neg ) {
		Integer expected = FListInteger( neg, a, b, c );
		Assert.Equal( expected, new Integer( expected ) );
	}

	[Theory]
	[InlineData( 0u, 0u, 0u )]
	[InlineData( 0u, 1u, 1u )]
	[InlineData( 1u, 0u, 0u )]
	[InlineData( 1u, 0u, 1u )]
	public void Natural( uint a, uint b, uint c ) {
		Integer expected = FListInteger( false, a, b, c );
		Assert.Equal( expected, new Integer( FListNatural( a, b, c ) ) );
	}

	[Theory]
	[InlineData( 0u, 0u, 0u, false )]
	[InlineData( 0u, 1u, 1u, false )]
	[InlineData( 1u, 0u, 0u, false )]
	[InlineData( 1u, 0u, 1u, false )]
	[InlineData( 0u, 1u, 1u, true )]
	[InlineData( 1u, 0u, 0u, true )]
	[InlineData( 1u, 0u, 1u, true )]
	public void NaturalSign( uint a, uint b, uint c, bool neg ) {
		Integer expected = FListInteger( neg, a, b, c );
		Assert.Equal( expected, new Integer( FListNatural( a, b, c ), neg ) );
	}

	[Theory]
	[InlineData( 0 )]
	[InlineData( 1 )]
	[InlineData( 2 )]
	[InlineData( 5 )]
	[InlineData( 100 )]
	[InlineData( -1 )]
	[InlineData( -2 )]
	[InlineData( -5 )]
	[InlineData( -100 )]
	public void Int8Ctor( SByte value ) {
		uint magnitude = Convert.ToUInt32( SByte.Abs(value) );
		bool negative = SByte.IsNegative( value );
		Assert.Equal( FListInteger( negative, magnitude ), new Integer( value ) );
	}

	[Theory]
	[InlineData( 0 )]
	[InlineData( 1 )]
	[InlineData( 2 )]
	[InlineData( 5 )]
	[InlineData( 100 )]
	[InlineData( -1 )]
	[InlineData( -2 )]
	[InlineData( -5 )]
	[InlineData( -100 )]
	public void Int16Ctor( short value ) {
		uint magnitude = Convert.ToUInt32( Int16.Abs(value) );
		bool negative = Int16.IsNegative( value );
		Assert.Equal( FListInteger( negative, magnitude ), new Integer( value ) );
	}

	[Theory]
	[InlineData( 0 )]
	[InlineData( 1 )]
	[InlineData( 2 )]
	[InlineData( 5 )]
	[InlineData( 100 )]
	[InlineData( -1 )]
	[InlineData( -2 )]
	[InlineData( -5 )]
	[InlineData( -100 )]
	public void Uint32Ctor( int value ) {
		uint magnitude = Convert.ToUInt32( Int32.Abs(value) );
		bool negative = Int32.IsNegative( value );
		Assert.Equal( FListInteger( negative, magnitude ), new Integer( value ) );
	}

	[Theory]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000L, false )]
	[InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001L, false )]
	[InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000L, false )]
	[InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001L, false )]
	[InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078L, false )]
	[InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0L, false )]
	[InlineData( 0x0001_0000u, 0x0000_0000u, unchecked( (long)0xFFFF_0000_0000_0000L ), true )]
	[InlineData( 0x0000_1111u, 0x2222_3334u, unchecked( (long)0xFFFF_EEEE_DDDD_CCCCL ), true )]
	public void Int64Ctor( uint expectedHigh, uint expectedLow, long value, bool negative ) =>
		Assert.Equal( FListInteger( negative, expectedHigh, expectedLow ), new Integer( value ) );

	[Theory]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0000uL, false )]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0001uL, false )]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0001_0000_0000uL, false )]
	[InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0001uL, 0x0000_0000_0000_0000uL, false )]
	[InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001_0000_0000uL, 0x0000_0000_0000_0000uL, false )]
	[InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL, 0x0000_0001_0000_0001uL, false )]
	[InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u, 0x1200_0340_0560_0078uL, 0x1234_5678_9ABC_DEF0uL, false )]
	[InlineData( 0x0000_1111u, 0x2222_3333u, 0xBBBB_CCCCu, 0xDDDD_EEEFu, 0xFFFF_EEEE_DDDD_CCCCuL, 0x4444_3333_2222_1111uL, true )]

	public void Int128Ctor( uint i0, uint i1, uint i2, uint i3, ulong l0, ulong l1, bool neg ) =>
		Assert.Equal( FListInteger( neg, i0, i1, i2, i3 ), new Integer( new Int128( l0, l1 ) ) );

	[Theory]
	[InlineData( "0", 0u, 0u, 0u )]
	[InlineData( "4294967297", 0u, 1u, 1u )]
	[InlineData( "18446744073709551616", 1u, 0u, 0u )]
	[InlineData( "18446744073709551617", 1u, 0u, 1u )]

	public void UInt32List( string expected, uint a, uint b, uint c ) =>
		Assert.Equal( Integer.Parse( expected ), FListInteger( a, b, c ) );

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
		Assert.Equal( FListInteger( a, b, c ), new Integer( [a, b, c] ) );

	[Theory]
	[InlineData( 0u, 0u, 0u, false )]
	[InlineData( 0u, 0u, 1u, false )]
	[InlineData( 0u, 1u, 0u, false )]
	[InlineData( 0u, 1u, 1u, false )]
	[InlineData( 1u, 0u, 0u, false )]
	[InlineData( 1u, 0u, 1u, false )]
	[InlineData( 1u, 1u, 0u, false )]
	[InlineData( 1u, 1u, 1u, false )]
	[InlineData( 0u, 0u, 1u, true )]
	[InlineData( 0u, 1u, 0u, true )]
	[InlineData( 0u, 1u, 1u, true )]
	[InlineData( 1u, 0u, 0u, true )]
	[InlineData( 1u, 0u, 1u, true )]
	[InlineData( 1u, 1u, 0u, true )]
	[InlineData( 1u, 1u, 1u, true )]
	public void UInt32SequenceNegative( uint a, uint b, uint c, bool n ) =>
		Assert.Equal( FListInteger( n, a, b, c ), new Integer( [a, b, c], n ) );
}