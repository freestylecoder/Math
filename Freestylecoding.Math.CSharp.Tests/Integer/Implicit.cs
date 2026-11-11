using System;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Integer = Freestylecoding.Math.Integer;

public class Implicit {
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
    public void Int8( SByte actual ) =>
        Assert.Equal( new Integer( actual ), actual );

    [Theory]
    [InlineData(    0 )]
    [InlineData(    1 )]
    [InlineData(    2 )]
    [InlineData(    5 )]
    [InlineData(  100 )]
    [InlineData(   -1 )]
    [InlineData(   -2 )]
    [InlineData(   -5 )]
    [InlineData( -100 )]
    public void Int16( short actual ) =>
        Assert.Equal( new Integer( actual ), actual );

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
    public void Int32( int actual ) =>
        Assert.Equal( new Integer( actual ), actual );

    [Theory]
    [InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000L, false )]
    [InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0001L, false )]
    [InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0001_0000_0000L, false )]
    [InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001L, false )]
    [InlineData( 0x1200_0340u, 0x0560_0078u, 0x1200_0340_0560_0078L, false )]
    [InlineData( 0x1234_5678u, 0x9ABC_DEF0u, 0x1234_5678_9ABC_DEF0L, false )]
    [InlineData( 0x0001_0000u, 0x0000_0000u, unchecked( (long)0xFFFF_0000_0000_0000L ), true )]
    [InlineData( 0x0000_1111u, 0x2222_3334u, unchecked( (long)0xFFFF_EEEE_DDDD_CCCCL ), true )]
    public void Int64( uint expectedHigh, uint expectedLow, long actual, bool neg ) =>
        Assert.Equal( new Integer( [expectedHigh, expectedLow], neg ), actual );

    [Theory]
    [InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0000uL, false )]
    [InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000_0000_0000uL, 0x0000_0000_0000_0001uL, false )]
    [InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000_0000_0000uL, 0x0000_0001_0000_0000uL, false )]
    [InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000_0000_0001uL, 0x0000_0000_0000_0000uL, false )]
    [InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001_0000_0000uL, 0x0000_0000_0000_0000uL, false )]
    [InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001_0000_0001uL, 0x0000_0001_0000_0001uL, false )]
    [InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u, 0x1200_0340_0560_0078uL, 0x1234_5678_9ABC_DEF0uL, false )]
    [InlineData( 0x0000_1111u, 0x2222_3333u, 0xBBBB_CCCCu, 0xDDDD_EEEFu, 0xFFFF_EEEE_DDDD_CCCCuL, 0x4444_3333_2222_1111uL, true )]
    public void Int128( uint i0, uint i1, uint i2, uint i3, ulong l0, ulong l1, bool neg ) =>
        Assert.Equal( new Integer( [i0, i1, i2, i3], neg ), new System.Int128( l0, l1 ) );

    [Theory]
    [InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u )]
    [InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0000u, 0x0000_0001u )]
    [InlineData( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u )]
    [InlineData( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u )]
    [InlineData( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u )]
    [InlineData( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u )]
    [InlineData( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u )]
    [InlineData( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0x4444_3333u, 0x2222_1111u )]
    public void Natural( uint i0, uint i1, uint i2, uint i3 ) =>
        Assert.Equal<Integer>( FListInteger( false, i0, i1, i2, i3 ), FListNatural( i0, i1, i2, i3 ) );

    [Fact]
    public void IntegerToBigInteger() {
        Assert.Equal<Integer>(
            System.Numerics.BigInteger.Zero,
            Integer.Zero
        );
        Assert.Equal<Integer>(
            System.Numerics.BigInteger.One,
            Integer.Unit
        );
        Assert.Equal<Integer>(
            System.Numerics.BigInteger.MinusOne,
            Integer.NegativeOne
        );

        Assert.Equal<Integer>(
            new System.Numerics.BigInteger(
                [
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x00
                ],
                true,
                true
            ),
            FListInteger( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u )
        );
        Assert.Equal<Integer>(
            new System.Numerics.BigInteger(
                [
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00
                ],
                true,
                true
            ),
            FListInteger( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u )
        );
        Assert.Equal<Integer>(
            new System.Numerics.BigInteger(
                [
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00
                ],
                true,
                true
            ),
            FListInteger( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u )
        );
        Assert.Equal<Integer>(
            new System.Numerics.BigInteger(
                [
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x01
                ],
                true,
                true
            ),
            FListInteger( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u )
        );
        Assert.Equal<Integer>(
            new System.Numerics.BigInteger(
                [
                    0x12, 0x00, 0x03, 0x40,
                    0x05, 0x60, 0x00, 0x78,
                    0x12, 0x34, 0x56, 0x78,
                    0x9A, 0xBC, 0xDE, 0xF0
                ],
                true,
                true
            ),
            FListInteger( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u )
        );

        // NOTE: The different between these last two are whether the BI is negative
        // This is done with the change of the IsUnsigned paramater
        // That is why the "same" BigInteger produces different Integers
        Assert.Equal<Integer>(
            new System.Numerics.BigInteger(
                [
                    0xFF, 0xFF, 0xEE, 0xEE,
                    0xDD, 0xDD, 0xCC, 0xCC,
                    0x44, 0x44, 0x33, 0x33,
                    0x22, 0x22, 0x11, 0x11
                ],
                true,
                true
            ),
            FListInteger( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0x4444_3333u, 0x2222_1111u )
        );
        Assert.Equal<Integer>(
            new System.Numerics.BigInteger(
                [
                    0xFF, 0xFF, 0xEE, 0xEE,
                    0xDD, 0xDD, 0xCC, 0xCC,
                    0x44, 0x44, 0x33, 0x33,
                    0x22, 0x22, 0x11, 0x11
                ],
                false,
                true
            ),
            FListInteger( true, 0x0000_1111u, 0x2222_3333u, 0xBBBB_CCCCu, 0xDDDD_EEEFu )
        );
    }

    [Fact]
    public void BigIntegerToInteger() {
        Assert.Equal<System.Numerics.BigInteger>(
            Integer.Zero,
            System.Numerics.BigInteger.Zero
        );
        Assert.Equal<System.Numerics.BigInteger>(
            Integer.Unit,
            System.Numerics.BigInteger.One
        );
        Assert.Equal<System.Numerics.BigInteger>(
            Integer.NegativeOne,
            System.Numerics.BigInteger.MinusOne
        );

        Assert.Equal<System.Numerics.BigInteger>(
            FListInteger( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u ),
            new System.Numerics.BigInteger(
                [
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x00
                ],
                true,
                true
            )
        );
        Assert.Equal<System.Numerics.BigInteger>(
            FListInteger( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u ),
            new System.Numerics.BigInteger(
                [
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00
                ],
                true,
                true
            )
        );
        Assert.Equal<System.Numerics.BigInteger>(
            FListInteger( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u ),
            new System.Numerics.BigInteger(
                [
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00
                ],
                true,
                true
            )
        );
        Assert.Equal<System.Numerics.BigInteger>(
            FListInteger( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u ),
            new System.Numerics.BigInteger(
                [
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x01,
                    0x00, 0x00, 0x00, 0x01
                ],
                true,
                true
            )
        );
        Assert.Equal<System.Numerics.BigInteger>(
            FListInteger( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u ),
            new System.Numerics.BigInteger(
                [
                    0x12, 0x00, 0x03, 0x40,
                    0x05, 0x60, 0x00, 0x78,
                    0x12, 0x34, 0x56, 0x78,
                    0x9A, 0xBC, 0xDE, 0xF0
                ],
                true,
                true
            )
        );

        // NOTE: The different between these last two are whether the Integer is negative
        Assert.Equal<System.Numerics.BigInteger>(
            FListInteger( 0xFFFF_EEEEu, 0xDDDD_CCCCu, 0x4444_3333u, 0x2222_1111u ),
            new System.Numerics.BigInteger(
                [
                    0xFF, 0xFF, 0xEE, 0xEE,
                    0xDD, 0xDD, 0xCC, 0xCC,
                    0x44, 0x44, 0x33, 0x33,
                    0x22, 0x22, 0x11, 0x11
                ],
                true,
                true
            )
        );
        Assert.Equal<System.Numerics.BigInteger>(
            FListInteger( true, 0x0000_1111u, 0x2222_3333u, 0xBBBB_CCCCu, 0xDDDD_EEEFu ),
            new System.Numerics.BigInteger(
                [
                    0xFF, 0xFF, 0xEE, 0xEE,
                    0xDD, 0xDD, 0xCC, 0xCC,
                    0x44, 0x44, 0x33, 0x33,
                    0x22, 0x22, 0x11, 0x11
                ],
                false,
                true
            )
        );
    }
}