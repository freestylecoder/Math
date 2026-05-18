using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class DivisionOperators {
	private static T Divide<T>( T left, T right ) where T : IDivisionOperators<T, T, T> =>
		left / right;
	private static T CheckedDivide<T>( T left, T right ) where T : IDivisionOperators<T, T, T> =>
		checked(left / right);

	[Theory]
	[InlineData( 1u, 1u, 1u )] // Sanity
	[InlineData( 0u, 1u, 0u )] // Sanity
	[InlineData( 42u, 7u, 6u )] // multiple bits
	[InlineData( 50u, 5u, 10u )] // rev
	[InlineData( 50u, 10u, 5u )] // rev
	[InlineData( 54u, 5u, 10u )] // has remainder
	public void Sanity( uint dividend, uint divisor, uint quotient ) =>
		Assert.Equal(
			new Natural( quotient ),
			Divide(
				new Natural( dividend ),
				new Natural( divisor )
			)
		);

	[Fact]
	public void Zero() =>
		Assert.Equal(
			Natural.Zero,
			Divide(
				new Natural( 5u ),
				new Natural( 10u )
			)
		);

	[Fact]
	public void DivideByZero() {
		System.Exception exc = Record.Exception(
			() =>
				Divide(
					Natural.Unit,
					Natural.Zero
				)
		);

		Assert.NotNull( exc );
		Assert.IsType<System.DivideByZeroException>( exc );
	}

	[Fact]
	public void Big() =>
		Assert.Equal(
			new Natural( 0xFEDCBA98u ),
			Divide(
				FListNatural( 0x75CD9046u, 0x541D5980u ),
				new Natural( 0x76543210u )
			)
		);

	[Theory]
	[InlineData( 1u, 1u, 1u )] // Sanity
	[InlineData( 0u, 1u, 0u )] // Sanity
	[InlineData( 42u, 7u, 6u )] // multiple bits
	[InlineData( 50u, 5u, 10u )] // rev
	[InlineData( 50u, 10u, 5u )] // rev
	[InlineData( 54u, 5u, 10u )] // has remainder
	public void CheckedSanity( uint dividend, uint divisor, uint quotient ) =>
		Assert.Equal(
			new Natural( quotient ),
			CheckedDivide(
				new Natural( dividend ),
				new Natural( divisor )
			)
		);

	[Fact]
	public void CheckedZero() =>
		Assert.Equal(
			Natural.Zero,
			CheckedDivide(
				new Natural( 5u ),
				new Natural( 10u )
			)
		);

	[Fact]
	public void CheckedDivideByZero() {
		System.Exception exc = Record.Exception(
			() =>
				CheckedDivide(
					Natural.Unit,
					Natural.Zero
				)
		);

		Assert.NotNull( exc );
		Assert.IsType<System.DivideByZeroException>( exc );
	}

	[Fact]
	public void CheckedBig() =>
		Assert.Equal(
			new Natural( 0xFEDCBA98u ),
			CheckedDivide(
				FListNatural( 0x75CD9046u, 0x541D5980u ),
				new Natural( 0x76543210u )
			)
		);
}