using System;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class Addition {
	[Theory]
	[InlineData( 0u, 0u, 0u )]     // Sanity
	[InlineData( 1u, 1u, 2u )]     // Sanity
	[InlineData( 1u, 0u, 1u )]     // Sanity
	[InlineData( 0u, 1u, 1u )]     // Sanity
	[InlineData( 1u, 5u, 6u )]     // l < r
	[InlineData( 9u, 2u, 11u )]     // l > r
	public void Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			new Natural( left ) + new Natural( right )
		);

	[Fact]
	public void Simple() =>
		Assert.Equal(
			Natural.Unit,
			Natural.Zero + Natural.Unit
		);

	[Fact]
	public void Overflow() =>
		Assert.Equal(
			FListNatural( 1u, 2u ),
			new Natural( 0xFFFF_FFFFu ) + new Natural( 3u )
		);

	[Fact]
	public void LeftBiggerNoOverflow() =>
		Assert.Equal(
			FListNatural( 0xFu, 0xFF0Fu ),
			FListNatural( 0xFu, 0xFu ) + new Natural( 0xFF00u )
		);

	[Fact]
	public void RightBiggerNoOverflow() =>
		Assert.Equal(
			FListNatural( 0xFu, 0xFF0Fu ),
			new Natural( 0xFF00u ) + FListNatural( 0xFu, 0xFu )
		);

	[Fact]
	public void CascadingOverflow() =>
		Assert.Equal(
			FListNatural( 1u, 0u, 0u ),
			new Natural( 1u ) + FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu )
		);

	[Fact]
	public void OverflowCausedOverflow() =>
		Assert.Equal(
			FListNatural( 2u, 0u, 0u ),
			FListNatural( 1u, 1u ) + FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0xFFFF_FFFFu )
		);

	[Fact]
	public void EdgeOfOverflow() =>
		Assert.Equal(
			FListNatural( 1u, 0xFFFF_FFFFu, 1u ),
			FListNatural( 1u, 1u ) + FListNatural( 1u, 0xFFFF_FFFFu - 1u, 0u )
		);
}

public class Subtraction {
	[Theory]
	[InlineData( 1u, 1u, 0u )]    // Sanity
	[InlineData( 1u, 0u, 1u )]    // Sanity
	[InlineData( 9u, 2u, 7u )]    // l > r
	public void Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			new Natural( left ) - new Natural( right )
		);

	[Fact]
	public void SingleItemBadUnderflow() {
		Exception exc = Record.Exception(
			() => Natural.Zero - Natural.Unit
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Fact]
	public void MultiItemNoUnderflow() =>
		Assert.Equal(
			FListNatural( 2u, 2u ),
			FListNatural( 3u, 4u ) - FListNatural( 1u, 2u )
		);

	[Fact]
	public void MultiItemSafeUnderflow() =>
		Assert.Equal(
			FListNatural( 0x2u, 0xFFFFFFFFu ),
			FListNatural( 4u, 2u ) - FListNatural( 1u, 3u )
		);

	[Fact]
	public void MultiItemSafeCascadingUnderflow() =>
		Assert.Equal(
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ),
			FListNatural( 1u, 0u, 0u ) - Natural.Unit
		);

	[Fact]
	public void MultiItemUnsafeUnderflow() {
		Exception exc = Record.Exception(
			() => FListNatural( 1u, 2u ) - FListNatural( 1u, 3u )
		);

		Assert.NotNull( exc );
		Assert.IsType<System.OverflowException>( exc );
	}

	[Fact]
	public void LargeWithUnderflows() =>
		Assert.Equal(
			FListNatural( 0x1u, 0xFFFF_FFFFu, 0xFFFF_FFFEu ),
			FListNatural( 3u, 2u, 1u ) - FListNatural( 1u, 2u, 3u )
		);
}

public class Multiply {
	[Theory]
	[InlineData( 1u, 1u, 1u )]        // Sanity
	[InlineData( 1u, 0u, 0u )]        // Sanity
	[InlineData( 0u, 1u, 0u )]        // Sanity
	[InlineData( 6u, 7u, 42u )]       // multiple bits
	public void Sanity( uint left, uint right, uint expected ) =>
		Assert.Equal(
			new Natural( expected ),
			new Natural( left ) * new Natural( right )
		);

	[Fact]

	public void Big() =>
		Assert.Equal(
			FListNatural( 0x75CD9046u, 0x541D5980u ),
			new Natural( 0xFEDCBA98u ) * new Natural( 0x76543210u )
		);
}

public class Division {
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
			new Natural( dividend ) / new Natural( divisor )
		);

	[Fact]
	public void Zero() =>
		Assert.Equal(
			Natural.Zero,
			new Natural( 5u ) / new Natural( 10u )
		);

	[Fact]
	public void DivideByZero() {
		Exception exc = Record.Exception(
			() => Natural.Unit / Natural.Zero
		);

		Assert.NotNull( exc );
		Assert.IsType<System.DivideByZeroException>( exc );
	}

	[Fact]
	public void Big() =>
		Assert.Equal(
			new Natural( 0xFEDCBA98u ),
			FListNatural( 0x75CD9046u, 0x541D5980u ) / new Natural( 0x76543210u )
		);
}

public class Modulo() {
	[Theory]
	[InlineData( 1u, 1u, 0u )]  // Sanity
	[InlineData( 0u, 1u, 0u )]  // Sanity
	[InlineData( 44u, 7u, 2u )]  // multiple bits
	[InlineData( 52u, 5u, 2u )]  // rev
	[InlineData( 52u, 10u, 2u )]  // rev
	public void Sanity( uint dividend, uint divisor, uint remainder ) =>
		Assert.Equal(
			new Natural( remainder ),
			new Natural( dividend ) % new Natural( divisor )
		);

	[Fact]
	public void Zero() =>
		Assert.Equal(
			new Natural( 0u ),
			new Natural( 20u ) % new Natural( 10u )
		);

	[Fact]
	public void DivideByZero() {
		Exception exc = Record.Exception(
			() => Natural.Unit % Natural.Zero
		);

		Assert.NotNull( exc );
		Assert.IsType<System.DivideByZeroException>( exc );
	}

	[Fact]
	public void Big() =>
		Assert.Equal(
			new Natural( 0x12345678u ),
			FListNatural( 0x75CD9046u, 0x6651AFF8u ) % FListNatural( 0x76543210u )
		);
}

public class DivisionModulo {
	[Theory]
	[InlineData( 1u, 1u, 1u, 0u )] // Sanity
	[InlineData( 0u, 1u, 0u, 0u )] // Sanity
	[InlineData( 44u, 7u, 6u, 2u )] // multiple bits
	[InlineData( 52u, 5u, 10u, 2u )] // rev
	[InlineData( 52u, 10u, 5u, 2u )] // rev
	public void Sanity( uint dividend, uint divisor, uint quotient, uint remainder ) =>
		Assert.Equal(
			new Tuple<Natural, Natural>( new Natural( quotient ), new Natural( remainder ) ),
			Natural.op_DividePercent( new Natural( dividend ), new Natural( divisor ) )
		);

	[Fact]
	public void DivideByZero() {
		Exception exc = Record.Exception(
			() => Natural.op_DividePercent( Natural.Unit, Natural.Zero )
		);

		Assert.NotNull( exc );
		Assert.IsType<System.DivideByZeroException>( exc );
	}

	[Fact]
	public void Big() =>
		Assert.Equal(
			new Tuple<Natural, Natural>( new Natural( 0xFEDCBA98u ), new Natural( 0x12345678u ) ),
			Natural.op_DividePercent( FListNatural( 0x75CD9046u, 0x6651AFF8u ), new Natural( 0x76543210u ) )
		);
}

public class Increment {
	[Theory]
	[InlineData( 0u,  1u )]   // Sanity
	[InlineData( 1u,  2u )]   // Sanity
	[InlineData( 9u, 10u )]   // Sanity
	public void Sanity( uint value, uint expected ) {
		Natural actual = new Natural( value );
		Assert.Equal(
			new Natural( expected ),
			++actual
		);
	}

	[Fact]
	public void Overflow() {
		Natural actual = new Natural( 0xFFFF_FFFFu );
		Assert.Equal(
			FListNatural( 1u, 0u ),
			++actual
		);
	}

	[Fact]
	public void CascadingOverflow() {
		Natural actual = FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu );
		Assert.Equal(
			FListNatural( 1u, 0u, 0u ),
			++actual
		);
	}

	[Fact( Skip = "Postfix won't work until Natural is a ValueType" )]
	public void Postfix() {
		Natural actual = Natural.Zero;
		Assert.Equal(
			Natural.Zero,
			actual++
		);
		Assert.Equal(
			Natural.One,
			actual
		);
	}
}

public class Decrement {
	[Theory]
	[InlineData(  1u, 0u )]   // Sanity
	[InlineData(  2u, 1u )]   // Sanity
	[InlineData( 10u, 9u )]   // Sanity
	public void Sanity( uint value, uint expected ) {
		Natural actual = new Natural( value );
		Assert.Equal(
			new Natural( expected ),
			--actual
		);
	}

	[Fact]
	public void Overflow() {
		Natural actual = FListNatural( 1u, 0u );
		Assert.Equal(
			new Natural( 0xFFFF_FFFFu ),
			--actual
		);
	}

	[Fact]
	public void CascadingOverflow() {
		Natural actual = FListNatural( 1u, 0u, 0u );
		Assert.Equal(
			FListNatural( 0xFFFF_FFFFu, 0xFFFF_FFFFu ),
			--actual
		);
	}

	[Fact( Skip = "Postfix won't work until Natural is a ValueType" )]
	public void Postfix() {
		Natural actual = Natural.One;
		Assert.Equal(
			Natural.One,
			actual--
		);
		Assert.Equal(
			Natural.Zero,
			actual
		);
	}
}
