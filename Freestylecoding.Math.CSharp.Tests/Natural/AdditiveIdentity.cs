using Xunit;

namespace Natural.Tests;

using static Freestylecoding.Math.CSharp.Tests.Helpers;
using Natural = Freestylecoding.Math.Natural;

public class AdditiveIdentity : Freestylecoding.Math.Interface.Tests.TAdditiveIdentitySelf<Natural> {
	public override Natural resultIdentity => Natural.Zero;
}