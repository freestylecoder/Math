using System;
using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class Explicit {
	[Fact]
	public void ToBigInteger() {
		BigInteger testValue;

		testValue = BigInteger.Zero;
		Assert.Equal(
			Natural.Zero,
			(Natural)testValue
		);

		testValue = BigInteger.One;
		Assert.Equal(
			Natural.Unit,
			(Natural)testValue
		);

		testValue = new BigInteger(
			new byte[] {
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x01,
				0x00, 0x00, 0x00, 0x00
			},
			true,
			true
		);
		Assert.Equal(
			FListNatural( 0x0000_0000u, 0x0000_0000u, 0x0000_0001u, 0x0000_0000u ),
			(Natural)testValue
		);

		testValue = new BigInteger(
			new byte[] {
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x01,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00
			},
			true,
			true
		);
		Assert.Equal(
			FListNatural( 0x0000_0000u, 0x0000_0001u, 0x0000_0000u, 0x0000_0000u ),
			(Natural)testValue
		);

		testValue = new BigInteger(
			new byte[] {
				0x00, 0x00, 0x00, 0x01,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00
			},
			true,
			true
		);
		Assert.Equal(
			FListNatural( 0x0000_0001u, 0x0000_0000u, 0x0000_0000u, 0x0000_0000u ),
			(Natural)testValue
		);

		testValue = new BigInteger(
			new byte[] {
				0x00, 0x00, 0x00, 0x01,
				0x00, 0x00, 0x00, 0x01,
				0x00, 0x00, 0x00, 0x01,
				0x00, 0x00, 0x00, 0x01
			},
			true,
			true
		);
		Assert.Equal(
			FListNatural( 0x0000_0001u, 0x0000_0001u, 0x0000_0001u, 0x0000_0001u ),
			(Natural)testValue
		);

		testValue = new BigInteger(
			new byte[] {
				0x12, 0x00, 0x03, 0x40,
				0x05, 0x60, 0x00, 0x78,
				0x12, 0x34, 0x56, 0x78,
				0x9A, 0xBC, 0xDE, 0xF0
			},
			true,
			true
		);
		Assert.Equal(
			FListNatural( 0x1200_0340u, 0x0560_0078u, 0x1234_5678u, 0x9ABC_DEF0u ),
			(Natural)testValue
		);
	}

	[Fact]
	public void ToBigInteger_Negative() {
		Exception exc = Record.Exception(
			() => {
				Natural overflow = (Natural)BigInteger.MinusOne;
			}
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}
}
