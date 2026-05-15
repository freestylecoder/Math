using System;
using System.Numerics;
using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class ModulusOperators {
	private static T Mod<T>( T left, T right ) where T : IModulusOperators<T, T, T> =>
		left % right;

	[Theory]
	[InlineData( 1u, 1u, 0u )]  // Sanity
	[InlineData( 0u, 1u, 0u )]  // Sanity
	[InlineData( 44u, 7u, 2u )]  // multiple bits
	[InlineData( 52u, 5u, 2u )]  // rev
	[InlineData( 52u, 10u, 2u )]  // rev
	public void Sanity( uint dividend, uint divisor, uint remainder ) =>
		Assert.Equal(
			new Natural( remainder ),
			Mod(
				new Natural( dividend ),
				new Natural( divisor )
			)
		);

	[Fact]
	public void Zero() =>
		Assert.Equal(
			new Natural( 0u ),
			Mod(
				new Natural( 20u ),
				new Natural( 10u )
			)
		);

	[Fact]
	public void DivideByZero() {
		Exception exc = Record.Exception(
			() =>
				Mod( Natural.Unit, Natural.Zero )
		);

		Assert.NotNull( exc );
		Assert.IsType<DivideByZeroException>( exc );
	}

	[Fact]
	public void Big() =>
		Assert.Equal(
			new Natural( 0x12345678u ),
			Mod(
				FListNatural( 0x75CD9046u, 0x6651AFF8u ),
				new Natural( 0x76543210u )
			)
		);
}