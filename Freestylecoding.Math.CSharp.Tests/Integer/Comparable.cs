using System;
using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;
using Integer = Freestylecoding.Math.Integer;

public class Comparable {
	private static bool eq( int actual ) => actual == 0;
	private static bool gt( int actual ) => actual > 0;
	private static bool lt( int actual ) => actual < 0;

	private static Random random = new Random();

	[Theory]
	[InlineData( 0, 0, 0 )]
	[InlineData( 1, 1, 0 )]
	[InlineData( 1, 0, 1 )]
	[InlineData( 0, 1, -1 )]
	[InlineData( -1, 1, -1 )]
	[InlineData( 1, -1, 1 )]
	[InlineData( -1, -1, 0 )]
	public void Sanity( int left, int right, int expected ) {
		bool test( int actual ) =>
			expected switch {
				0 => eq( actual ),
				int p when p > 0 => gt( actual ),
				int n when n < 0 => lt( actual ),
				_ => throw new Exception( "Not Possible" ),
			};

		Assert.True(
			test( ( new Integer( left ) as IComparable ).CompareTo( new Integer( right ) ) )
		);
	}

	[Fact( DisplayName = "Equals" )]
	public void EqualsTests() {
		// NOTE: This only goes to 127 so that I know the cast to signed byte will work
		byte b = Convert.ToByte( random.Next( 0, 127 ) );
		IComparable left = new Integer( (Int16)b ) as IComparable;

		Assert.True( eq( left.CompareTo( new Natural( b ) ) ) );
		Assert.True( eq( left.CompareTo( new Integer( (Int16)b ) ) ) );

		Assert.True( eq( left.CompareTo( b ) ) );
		Assert.True( eq( left.CompareTo( (UInt16)b ) ) );
		Assert.True( eq( left.CompareTo( (UInt32)b ) ) );
		Assert.True( eq( left.CompareTo( (UInt64)b ) ) );
		Assert.True( eq( left.CompareTo( new UInt128( 0uL, (UInt64)b ) ) ) );
		Assert.True( eq( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );

		Assert.True( eq( left.CompareTo( (SByte)b ) ) );
		Assert.True( eq( left.CompareTo( (Int16)b ) ) );
		Assert.True( eq( left.CompareTo( (Int32)b ) ) );
		Assert.True( eq( left.CompareTo( (Int64)b ) ) );
		Assert.True( eq( left.CompareTo( new Int128( 0uL, (UInt64)b ) ) ) );
	}

	[Fact]
	public void NegativeEquals() {
		// NOTE: This only goes to 127 so that I know the cast to signed byte will work
		sbyte b = Convert.ToSByte( random.Next( -127, 0 ) );
		IComparable left = new Integer( b ) as IComparable;

		Assert.True( eq( left.CompareTo( new Integer( b ) ) ) );
		Assert.True( eq( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );

		Assert.True( eq( left.CompareTo( (SByte)b ) ) );
		Assert.True( eq( left.CompareTo( (Int16)b ) ) );
		Assert.True( eq( left.CompareTo( (Int32)b ) ) );
		Assert.True( eq( left.CompareTo( (Int64)b ) ) );
		Assert.True( eq( left.CompareTo( Int128.CreateChecked( b ) ) ) );
	}

	[Fact]
	public void GreaterThan() {
		IComparable left = new Integer( Convert.ToInt32( random.Next( 128, 256 ) ) ) as IComparable;
		byte b = Convert.ToByte( random.Next( 0, 127 ) );

		Assert.True( gt( left.CompareTo( new Natural( b ) ) ) );
		Assert.True( gt( left.CompareTo( new Integer( (Int16)b ) ) ) );

		Assert.True( gt( left.CompareTo( b ) ) );
		Assert.True( gt( left.CompareTo( (UInt16)b ) ) );
		Assert.True( gt( left.CompareTo( (UInt32)b ) ) );
		Assert.True( gt( left.CompareTo( (UInt64)b ) ) );
		Assert.True( gt( left.CompareTo( new UInt128( 0uL, (UInt64)b ) ) ) );
		Assert.True( gt( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );

		Assert.True( gt( left.CompareTo( (SByte)b ) ) );
		Assert.True( gt( left.CompareTo( (Int16)b ) ) );
		Assert.True( gt( left.CompareTo( (Int32)b ) ) );
		Assert.True( gt( left.CompareTo( (Int64)b ) ) );
	}

	[Fact]
	public void NegativeGreaterThan() {
		IComparable left = new Integer( Convert.ToInt32( random.Next( -64, 0 ) ) ) as IComparable;
		sbyte b = Convert.ToSByte( random.Next( -127, -65 ) );

		Assert.True( gt( left.CompareTo( new Integer( b ) ) ) );

		Assert.True( gt( left.CompareTo( (SByte)b ) ) );
		Assert.True( gt( left.CompareTo( (Int16)b ) ) );
		Assert.True( gt( left.CompareTo( (Int32)b ) ) );
		Assert.True( gt( left.CompareTo( (Int64)b ) ) );
		Assert.True( gt( left.CompareTo( Int128.CreateChecked( b ) ) ) );
		Assert.True( gt( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );
	}

	[Fact]
	public void LessThan() {
		IComparable left = new Integer( Convert.ToInt32( random.Next( 0, 63 ) ) ) as IComparable;
		byte b = Convert.ToByte( random.Next( 64, 128 ) );

		Assert.True( lt( left.CompareTo( new Natural( b ) ) ) );
		Assert.True( lt( left.CompareTo( new Integer( (Int16)b ) ) ) );

		Assert.True( lt( left.CompareTo( b ) ) );
		Assert.True( lt( left.CompareTo( (UInt16)b ) ) );
		Assert.True( lt( left.CompareTo( (UInt32)b ) ) );
		Assert.True( lt( left.CompareTo( (UInt64)b ) ) );
		Assert.True( lt( left.CompareTo( new UInt128( 0uL, (UInt64)b ) ) ) );
		Assert.True( lt( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );

		Assert.True( lt( left.CompareTo( (SByte)b ) ) );
		Assert.True( lt( left.CompareTo( (Int16)b ) ) );
		Assert.True( lt( left.CompareTo( (Int32)b ) ) );
		Assert.True( lt( left.CompareTo( (Int64)b ) ) );
		Assert.True( lt( left.CompareTo( new Int128( 0uL, (UInt64)b ) ) ) );
	}

	[Fact]
	public void NegativeLessThan() {
		IComparable left = new Integer( Convert.ToInt32( random.Next( -127, -65 ) ) ) as IComparable;
		sbyte b = Convert.ToSByte( random.Next( -64, 0 ) );

		Assert.True( lt( left.CompareTo( new Integer( b ) ) ) );

		Assert.True( lt( left.CompareTo( (SByte)b ) ) );
		Assert.True( lt( left.CompareTo( (Int16)b ) ) );
		Assert.True( lt( left.CompareTo( (Int32)b ) ) );
		Assert.True( lt( left.CompareTo( (Int64)b ) ) );
		Assert.True( lt( left.CompareTo( Int128.CreateChecked( b ) ) ) );
		Assert.True( lt( left.CompareTo( new System.Numerics.BigInteger( b ) ) ) );
	}

	[Theory]
	[InlineData( 1F )]
	[InlineData( 1.0 )]
	[InlineData( "1" )]
	public void Incompatible( object value ) {
		IComparable one = Integer.Unit as IComparable;
		Exception exc = Record.Exception(
			() => one.CompareTo( value )
		);

		Assert.NotNull( exc );
		Assert.IsType<ArgumentException>( exc );
	}
}