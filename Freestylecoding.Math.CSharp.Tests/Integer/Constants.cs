using Xunit;

namespace Integer.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Integer = Freestylecoding.Math.Integer;

public class Constants {
    [Fact]
    public void ZeroConstant() =>
        Assert.Equal( new Integer( 0 ), Integer.Zero );

    [Fact]
    public void UnitConstant() =>
        Assert.Equal( new Integer( 1 ), Integer.Unit );

    [Fact]
    public void NegativeOneConstant() =>
        Assert.Equal( new Integer( -1 ), Integer.NegativeOne );
}