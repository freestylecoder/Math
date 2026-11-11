using System;
using Freestylecoding.Math.CSharp.Tests;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class Comparable : Freestylecoding.Math.Interface.Tests.TComparable<Natural> {
	private static readonly Random random = new Random();

	private static Func<int,bool> eq = (int actual) => actual == 0;
	private static Func<int,bool> gt = (int actual) => actual > 0;
	private static Func<int,bool> lt = (int actual) => actual < 0;

	public override Natural smallest => Natural.Zero;
	public override Natural small => Natural.Unit;
	public override Natural medium => SmallNatural;
	public override Natural large => MediumNatural;
	public override Natural largest => LargeNatural;

	[Fact( DisplayName = "Equals" )]
	public void EqualsTest() {
		byte b = Convert.ToByte( random.Next( 0, 256 ) );
		IComparable left = new Natural( b ) as IComparable;

		Assert.True( eq( left.CompareTo( new Natural( b ) ) ) );
		Assert.True( eq( left.CompareTo( b ) ) );
		Assert.True( eq( left.CompareTo( (ushort)b ) ) );
		Assert.True( eq( left.CompareTo( (uint)b ) ) );
		Assert.True( eq( left.CompareTo( (ulong)b ) ) );
		Assert.True( eq( left.CompareTo( new UInt128( 0uL, (ulong)b ) ) ) );
		Assert.True( eq( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );
	}

	[Fact]
	public void GreaterThan() {
		IComparable left = new Natural( Convert.ToUInt32( random.Next( 128, 256 ) ) ) as System.IComparable;
		byte b = Convert.ToByte( random.Next( 0, 127 ) );


		Assert.True( gt( left.CompareTo( new Natural( b ) ) ) );
		Assert.True( gt( left.CompareTo( b ) ) );
		Assert.True( gt( left.CompareTo( (ushort)b ) ) );
		Assert.True( gt( left.CompareTo( (uint)b ) ) );
		Assert.True( gt( left.CompareTo( (ulong)b ) ) );
		Assert.True( gt( left.CompareTo( new UInt128( 0uL, (ulong)b ) ) ) );

		Assert.True( gt( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );
	}

	[Fact]
	public void GreaterThan_NegativeBigInteger() {
		IComparable left = Natural.Unit as IComparable;
		Assert.True( gt( left.CompareTo( new System.Numerics.BigInteger( -1m ) ) ) );
	}

	[Fact]
	public void LessThan() {
		IComparable left = new Natural( Convert.ToUInt32( random.Next( 0, 127 ) ) ) as IComparable;
		byte b = Convert.ToByte( random.Next( 128, 256 ) );


		Assert.True( lt( left.CompareTo( new Natural( b ) ) ) );
		Assert.True( lt( left.CompareTo( b ) ) );
		Assert.True( lt( left.CompareTo( (ushort)b ) ) );
		Assert.True( lt( left.CompareTo( (uint)b ) ) );
		Assert.True( lt( left.CompareTo( (ulong)b ) ) );
		Assert.True( lt( left.CompareTo( new UInt128( 0uL, (ulong)b ) ) ) );

		Assert.True( lt( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );
	}

	[Fact]
	public void LessThan_NegativeBigInteger() {
		IComparable left = Natural.Unit as IComparable;
		Assert.False( lt( left.CompareTo( new System.Numerics.BigInteger( -1m ) ) ) );
	}

	[Theory]
	[InlineData( 1 )]
	[InlineData( 1L )]
	[InlineData( 1F )]
	[InlineData( 1.0 )]
	[InlineData( "1" )]
	public void Incompatible( dynamic value ) {
		IComparable one = Natural.Unit as System.IComparable;
		Exception exc = Record.Exception(
			() => one.CompareTo( value )
		);

		Assert.NotNull( exc );
		Assert.IsType<System.ArgumentException>( exc );
	}
}