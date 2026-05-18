using System;
using Xunit;

namespace Natural.Tests;

using Natural = Freestylecoding.Math.Natural;
using static Freestylecoding.Math.CSharp.Tests.Helpers;

/// <summary>Sanity tests for Natural type</summary>
/// <remarks>
///     <para>
///         The sanity tests are here to do low level tests of a few parts.
///         This lets me isolate those tests so I can use those parts in the rest of the tests.
///     </para>
///     <para>
///         In other words:
///         IF ANY OF THE SANITY TESTS FAIL, DON'T TRUST ANY OTHER TEST!
///     </para>
/// </remarks>
[Trait( "Type", "Sanity" )]
public class Sanity {
	private static readonly UInt32 maxUInt = UInt32.MaxValue;

	[Fact]
	public void LeftShift() =>
		Assert.Equal(
			FListNatural(  4u, 0x8000_0000u, 0u  ),
			FListNatural(  9u  ) << 63
		);

	[Fact]
	public void RightShift() =>
		Assert.Equal(
			FListNatural(  9u  ),
			FListNatural(  4u, 0x8000_0000u, 0u  ) >> 63
		);

	[Fact]
	public void GreaterThanTrue() =>
		Assert.True(
			FListNatural(  0xDEADBEEFu, 0xBADu  ) > FListNatural(  0xBADu, 0xDEADBEEFu  )
		);

	[Fact]
	public void GreaterThanFalseByLessThan() =>
		Assert.False(
			FListNatural(  0xBADu, 0xDEADBEEFu  ) > FListNatural(  0xDEADBEEFu, 0xBADu  )
		);

	[Fact]
	public void GreaterThanFalseByEquals() =>
		Assert.False(
			FListNatural(  0xBADu, 0xDEADBEEFu  ) > FListNatural(  0xBADu, 0xDEADBEEFu  )
		);

    [Fact]
	public void Addition() =>
		Assert.Equal(
			FListNatural(  2u, 0u, 0u  ),
            FListNatural(  1u, 1u  ) + FListNatural(  1u, maxUInt - 1u, maxUInt  )
        );

	[Fact]
	public void Subtraction() =>
		Assert.Equal(
			FListNatural(  maxUInt, maxUInt  ),
			FListNatural(  1u, 0u, 0u  ) - FListNatural(  1u  )
		);

	[Fact]
	public void Multiplication() =>
		Assert.Equal(
			FListNatural(  0x75CD9046u, 0x541D5980u  ),
			FListNatural(  0xFEDCBA98u  ) * FListNatural(  0x76543210u  )
		);

	[Fact]
	public void DivisionModulo() =>
		// TODO: Make a better named DivMod static method for C#
		Assert.Equal(
			new Tuple<Natural, Natural> (
				FListNatural(  0xFEDCBA98u  ),
				FListNatural(  0x12345678u  )
			),
			Natural.op_DividePercent(
				FListNatural(  0x75CD9046u, 0x6651AFF8u  ),
				// /%
				FListNatural(  0x76543210u  )
			)
		);

	[Fact( DisplayName = "ToString" )]
	public void ToStringTest() =>
		Assert.Equal(
			"1234567890123456789",
			FListNatural(  0x112210F4u, 0x7DE98115u  ).ToString()
		);

	[Fact]
	public void Parse() =>
		Assert.Equal(
			FListNatural(  0x112210F4u, 0x7DE98115u  ),
			Natural.Parse( "1234567890123456789" )
		);
}