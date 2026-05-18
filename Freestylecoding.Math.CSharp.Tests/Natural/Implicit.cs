using System;
using Xunit;

namespace Natural.Tests;

using Natural = Freestylecoding.Math.Natural;
using static Freestylecoding.Math.CSharp.Tests.Helpers;

// NOTE: The point of these is that the implicit conversion takes place
// That's why I'm not wasting time using FListNatural
public class Implicit {
	[Theory]
	[InlineData( (byte)0 )]
	[InlineData( (byte)1 )]
	[InlineData( (byte)2 )]
	[InlineData( (byte)5 )]
	[InlineData( (byte)100 )]
	public void FromByte( byte actual ) =>
		Assert.Equal( new Natural( actual ), actual );

	[Theory]
	[InlineData( (ushort)0 )]
	[InlineData( (ushort)1 )]
	[InlineData( (ushort)2 )]
	[InlineData( (ushort)5 )]
	[InlineData( (ushort)100 )]
	public void FromUnsignedShort( ushort actual ) =>
		Assert.Equal( new Natural( actual ), actual );

	[Theory]
	[InlineData( 0u )]
	[InlineData( 1u )]
	[InlineData( 2u )]
	[InlineData( 5u )]
	[InlineData( 100u )]
	public void FromUnsigned( uint actual ) =>
		Assert.Equal( new Natural( actual ), actual );

	[Theory]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL )]
	[InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001uL )]
	[InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000uL )]
	[InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL )]
	[InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078uL )]
	[InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0uL )]
	[InlineData( 0xFFFF_0000u, 0x0000_0000u, 0xFFFF_0000_0000_0000uL )]
	[InlineData( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0xFFFF_EEEE_DDDD_CCCCuL )]
	public void FromUnsignedLong( uint expectedHigh, uint expectedLow, ulong actual ) =>
		Assert.Equal( FListNatural( expectedHigh, expectedLow ), actual );

	[Theory]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0000uL )]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0001uL )]
	[InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0001_0000_0000uL )]
	[InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0001uL, 0x0000_0000_0000_0000uL )]
	[InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001_0000_0000uL, 0x0000_0000_0000_0000uL )]
	[InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL, 0x0000_0001_0000_0001uL )]
	[InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u, 0x1200_0340_0560_0078uL, 0x1234_5678_9ABC_DEF0uL )]
	public void FromUInt128( uint i0, uint i1, uint i2, uint i3, ulong l0, ulong l1 ) =>
		Assert.Equal( FListNatural( i0, i1, i2, i3 ), new UInt128( l0, l1 ) );

	[Fact]
	public void ToBigInteger() {
		Assert.Equal(
			new System.Numerics.BigInteger(
				new byte[] {
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00
				},
				true,
				true
			),
			Natural.Zero
		);

		Assert.Equal(
			new System.Numerics.BigInteger(
				new byte[] {
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x01
				},
                true,
                true
            ),
            Natural.Unit
        );

        Assert.Equal(
			new System.Numerics.BigInteger(
				new byte[] {
					0x00, 0x00, 0x00, 0x00,
			        0x00, 0x00, 0x00, 0x00,
			        0x00, 0x00, 0x00, 0x01,
			        0x00, 0x00, 0x00, 0x00
                },
                true,
                true
            ),
            	FListNatural( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u )    
        );

		Assert.Equal(
			new System.Numerics.BigInteger(
				new byte[] {
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x01,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00
				},
				true,
				true
			),
			FListNatural( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u )
		);

        Assert.Equal(
			new System.Numerics.BigInteger(
				new byte[] {
					0x00, 0x00, 0x00, 0x01,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00
                },
                true,
                true
            ),
			FListNatural( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u )
        );

        Assert.Equal(
			new System.Numerics.BigInteger(
				new byte[] {
					0x00, 0x00, 0x00, 0x01,
					0x00, 0x00, 0x00, 0x01,
					0x00, 0x00, 0x00, 0x01,
					0x00, 0x00, 0x00, 0x01
                },
                true,
                true
            ),
			FListNatural( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u )
        );

        Assert.Equal(
			new System.Numerics.BigInteger(
				new byte[] {
					0x12, 0x00, 0x03, 0x40,
					0x05, 0x60, 0x00, 0x78,
					0x12, 0x34, 0x56, 0x78,
					0x9A, 0xBC, 0xDE, 0xF0
                },
                true,
                true
            ),
			FListNatural( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u )
        );
    }
}