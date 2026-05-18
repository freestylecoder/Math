using System.Numerics;
using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class ShiftOperators {
	private static T LS<T>( T left, int right ) where T : IShiftOperators<T, int, T> =>
		left << right;
	private static T RS<T>( T left, int right ) where T : IShiftOperators<T, int, T> =>
		left >> right;
	private static T URS<T>( T left, int right ) where T : IShiftOperators<T, int, T> =>
		left >>> right;

	// LEFT SHIFT

	[Theory]
	[InlineData( 1u, 1, 2u )]        // Sanity
	[InlineData( 0xFu, 2, 0x3Cu )]   // multiple bits
	public void LS_Sanity( uint left, int right, uint expected ) =>
		Assert.Equal( new Natural( expected ), LS( new Natural( left ), right ) );

	[Fact]
	public void LS_Overflow() =>
		Assert.Equal( FListNatural( 1u, 0xFFFFFFFEu ), LS( new Natural( 0xFFFFFFFFu ), 1 ) );

	[Fact]
	public void LS_MultipleOverflow() =>
		Assert.Equal( FListNatural( 0x5u, 0xFFFFFFF8u ), LS( new Natural( 0xBFFFFFFFu ), 3 ) );

	[Fact]
	public void LS_OverOneUInt() =>
		Assert.Equal( FListNatural( 8u, 0u, 0u ), LS( Natural.Unit, 67 ) );

	// RIGHT SHIFT

	[Theory]
	[InlineData( 1u, 1, 0u )]        // Sanity
	[InlineData( 0xFu, 2, 0x3u )]    // multiple bits
	[InlineData( 0x3Cu, 2, 0xFu )]   // multiple bits
	public void RS_Sanity( uint left, int right, uint expected ) =>
		Assert.Equal( new Natural( expected ), RS( new Natural( left ), right ) );

	[Fact]
	public void RS_Underflow() =>
		Assert.Equal( new Natural( 0x7FFFFFFFu ), RS( new Natural( 0xFFFFFFFFu ), 1 ) );

	[Fact]
	public void RS_MultipleUnderflow() =>
		Assert.Equal( FListNatural( 0x1u, 0x5FFFFFFFu ), RS( FListNatural( 0xAu, 0xFFFFFFFFu ), 3 ) );

	[Fact]
	public void RS_OverOneUInt() =>
		Assert.Equal( Natural.Unit, RS( FListNatural( 0x10u, 0u, 0u ), 68 ) );

	[Fact]
	public void RS_ReduceToZero() =>
		Assert.Equal( Natural.Zero, RS( FListNatural( 0x10u, 0u, 0u ), 99 ) );

	// UNSIGNED RIGHT SHIFT
	// (Since the type is unsigned, this is the same as RightShift

	[Theory]
	[InlineData( 1u, 1, 0u )]        // Sanity
	[InlineData( 0xFu, 2, 0x3u )]    // multiple bits
	[InlineData( 0x3Cu, 2, 0xFu )]   // multiple bits
	public void URS_Sanity( uint left, int right, uint expected ) =>
		Assert.Equal( new Natural( expected ), URS( new Natural( left ), right ) );

	[Fact]
	public void URS_Underflow() =>
		Assert.Equal( new Natural( 0x7FFFFFFFu ), URS( new Natural( 0xFFFFFFFFu ), 1 ) );

	[Fact]
	public void URS_MultipleUnderflow() =>
		Assert.Equal( FListNatural( 0x1u, 0x5FFFFFFFu ), URS( FListNatural( 0xAu, 0xFFFFFFFFu ), 3 ) );

	[Fact]
	public void URS_OverOneUInt() =>
		Assert.Equal( Natural.Unit, URS( FListNatural( 0x10u, 0u, 0u ), 68 ) );

	[Fact]
	public void URS_ReduceToZero() =>
		Assert.Equal( Natural.Zero, URS( FListNatural( 0x10u, 0u, 0u ), 99 ) );

	[Fact]
	public void URS_ExtremeReduceToZero() =>
		// NOTE: This is an extreme reductionary case to hit one specific edge case in the base operator
		// Specifically, this part:
		//              var rec chomp n l =
		//                  match n with
		//                  | x when x > (List.length l) -> [0u]
		// It's only tested here, so that this test suite really has a reason to be

		Assert.Equal( Natural.Zero, URS( FListNatural( 0x10u, 0u, 0u ), 999 ) );
}