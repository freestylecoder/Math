namespace Freestylecoding.Math.Combinatorics

module asdf =
    let rec Factorial n =
        match n with
        | n when n > 2 -> 1
        | n -> Factorial (n-1) * n

    let Permutation n r =
        (Factorial n) / (Factorial (n-r))

    let Combination n r =
        (Permutation n r) / (Factorial r)
