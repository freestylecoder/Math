using Xunit;
namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Integer = Freestylecoding.Math.Integer;
using Natural = Freestylecoding.Math.Natural;

public class ComparisonOperatorsIntegerIntegerBool {
    static private bool op_GT<T>( T left, T right ) where T : System.Numerics.IComparisonOperators<T, T, bool> =>
        left > right;
    static private bool op_GTE<T>( T left, T right ) where T : System.Numerics.IComparisonOperators<T, T, bool> =>
        left >= right;
    static private bool op_LT<T>( T left, T right ) where T : System.Numerics.IComparisonOperators<T, T, bool> =>
        left < right;
    static private bool op_LTE<T>( T left, T right ) where T : System.Numerics.IComparisonOperators<T, T, bool> =>
        left <= right;

    static private Integer BigIntegerToInteger( System.Numerics.BigInteger bi ) {
        Integer baToList( Microsoft.FSharp.Collections.FSharpList<byte> ba ) {
            if( 0 == ba.Length )
                return new Integer( 0 );

            return ( baToList( ba.Tail ) << 2 ) + new Integer( (int)ba.Head );
        }

        Integer i = baToList(
            Microsoft.FSharp.Collections.ArrayModule.ToList(
                System.Numerics.BigInteger
                    .Abs( bi )
                    .ToByteArray()
            )
        );

        return bi.Sign < 0 ? -i : i;
    }

    static public object[][] IntPatterns = [
        new object[] { -1, -1 },
        new object[] { -1,  0 },
        new object[] { -1,  1 },
        new object[] {  0, -1 },
        new object[] {  0,  0 },
        new object[] {  0,  1 },
        new object[] {  1, -1 },
        new object[] {  1,  0 },
        new object[] {  1,  1 }
    ];

	static public object[][] BigIntPatterns = [
		new object[] { new byte[] { 0xFF, 0xBE, 0xAD, 0xDE, 0xAD, 0x0B }, new byte[] { 0xFF, 0xBE, 0xAD, 0xDE, 0x00 } },
		new object[] { new byte[] { 0xFF, 0xBE, 0xAD, 0xDE, 0x00 }, new byte[] { 0xFF, 0xBE, 0xAD, 0xDE, 0xAD, 0x0B } },
		new object[] { new byte[] { 0x01, 0x00, 0x00, 0x00, 0x01 }, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01 } },
		new object[] { new byte[] { 0x01, 0x00, 0x00, 0x00, 0x01 }, new byte[] { 0x01, 0x00, 0x00, 0x00, 0x01 } },
		new object[] { new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01 }, new byte[] { 0x01, 0x00, 0x00, 0x00, 0x01 } },
		new object[] { new byte[] { 0xFF, 0xBE, 0xAD, 0xDE, 0x00 }, new byte[] { 0x11, 0x41, 0x52, 0x21, 0xFF } },
		new object[] { new byte[] { 0x11, 0x41, 0x52, 0x21, 0xFF }, new byte[] { 0xFF, 0xBE, 0xAD, 0xDE, 0x00 } },
		new object[] { new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF }, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFE } },
		new object[] { new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFE }, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFE } },
		new object[] { new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFE }, new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF } }
	];


    [Theory]
    [MemberData( nameof( IntPatterns ) )]
    public void GT_Sanity( int left, int right ) =>
        Assert.Equal(
            op_GT( left, right ),
            op_GT( new Integer( left ), new Integer( right ) )
        );

    [Theory]
    [MemberData( nameof( BigIntPatterns ) )]
    public void GT_BigSanity( byte[] left, byte[] right ) {
        System.Numerics.BigInteger leftBI = new System.Numerics.BigInteger( left );
        Integer leftI = BigIntegerToInteger( leftBI );

        System.Numerics.BigInteger rightBI = new System.Numerics.BigInteger( right );
        Integer rightI = BigIntegerToInteger( rightBI );

        Assert.Equal(
            op_GT( leftBI, rightBI ),
            op_GT( leftI, rightI )
        );
    }

    [Theory]
    [MemberData( nameof( IntPatterns ) )]
    public void LT_Sanity( int left, int right ) =>
        Assert.Equal(
            op_LT( left, right ),
            op_LT(
                new Integer( left ),
                new Integer( right )
            )
        );

    [Theory]
    [MemberData( nameof( BigIntPatterns ) )]
    public void LT_BigSanity( byte[] left, byte[] right ) {
        System.Numerics.BigInteger leftBI = new System.Numerics.BigInteger( left );
        Integer leftI = BigIntegerToInteger( leftBI );

        System.Numerics.BigInteger rightBI = new System.Numerics.BigInteger( right );
        Integer rightI = BigIntegerToInteger( rightBI );

        Assert.Equal(
            op_LT( leftBI, rightBI ),
            op_LT( leftI, rightI )
        );
    }

    [Theory]
    [MemberData( nameof( IntPatterns ) )]
    public void GTE_Sanity( int left, int right ) =>
        Assert.Equal(
            op_GTE( left, right ),
            op_GTE(
                new Integer( left ),
                new Integer( right )
            )
        );

    [Theory]
    [MemberData( nameof( BigIntPatterns ) )]
    public void GTE_BigSanity( byte[] left, byte[] right ) {
        System.Numerics.BigInteger leftBI = new System.Numerics.BigInteger( left );
        Integer leftI = BigIntegerToInteger( leftBI );

        System.Numerics.BigInteger rightBI = new System.Numerics.BigInteger( right );
        Integer rightI = BigIntegerToInteger( rightBI );

        Assert.Equal(
            op_GTE( leftBI, rightBI ),
            op_GTE( leftI, rightI )
        );
    }

    [Theory]
    [MemberData( nameof( IntPatterns ) )]
    public void LTE_Sanity( int left, int right ) =>
        Assert.Equal(
            op_LTE( left, right ),
            op_LTE(
                new Integer( left ),
                new Integer( right )
            )
        );

    [Theory]
    [MemberData( nameof( BigIntPatterns ) )]
    public void LTE_BigSanity( byte[] left, byte[] right ) {
        System.Numerics.BigInteger leftBI = new System.Numerics.BigInteger( left );
        Integer leftI = BigIntegerToInteger( leftBI );

        System.Numerics.BigInteger rightBI = new System.Numerics.BigInteger( right );
        Integer rightI = BigIntegerToInteger( rightBI );

        Assert.Equal(
            op_LTE( leftBI, rightBI ),
            op_LTE( leftI, rightI )
        );
    }
}