using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

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