using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class AdditiveIdentity {
	[Fact]
	public void IsIdentityCorrect() =>
		Assert.Equal(
			Natural.Zero,
			Natural.AdditiveIdentity
		);
}