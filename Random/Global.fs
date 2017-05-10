namespace Freestylecoding.Math.Random

module internal Global =
    let RandomNumberGenerator = new System.Random()

    /// <summary>Computes the Greatest Common Divisor of two numbers</summary>
    /// <param name="a">First Value</param>
    /// <param name="b">Second Value</param>
    /// <returns>The GCD of <paramref name="a" /> and <paramref name="b" /></returns>
    /// <remarks>This function is insensitive to the order of the parameters.</remarks>
    let rec GCD a b =
        match b with
        | 0 -> a
        | x when x > a -> GCD b a
        | x -> GCD x (a%b)

