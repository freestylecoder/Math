using Xunit;

namespace Freestylecoding.Math.CSharp.Tests.Natural;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class MultiplicativeIdentity {
	[Fact]
	public void IsIdentityCorrect() =>
		Assert.Equal(
			Natural.Unit,
			Natural.MultiplicativeIdentity
		);
}