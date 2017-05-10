namespace Freestylecoding.Math.Random

module Primes =
    /// <summary>A list of all the prime numbers less than 10,000.</summary>
    let ListOfPrimes =
        let rec RemoveComposites (s:int) (n:int) (a:int array) =
            match n with
            | x when x > (a.Length-1) -> a
            | _ ->
                a.[n] <- 0
                RemoveComposites s (n+s) a

        let rec CalcPrimes n a =
            match n with
            | 1 -> a
            | x -> 
                CalcPrimes (x-1) a
                |> RemoveComposites x (x+x)

        let rec CreateList n (a:int array) =
            match n with
            | p when p > (a.Length-1) -> []
            | p ->
                match a.[n] with
                | 0 -> CreateList (n+1) a
                | _ -> p :: CreateList (n+1) a

        let init = [| for i in 0 .. 10000 -> 1 |]
        init.[0] <- 0
        init.[1] <- 0
        CalcPrimes 10000 init
        |> CreateList 2

    /// <summary>Selects a random prime number from the ListOfPrimes.</summary>
    /// <returns>A prime number.</returns>
    let RandomPrime () =
        ListOfPrimes.[ Global.RandomNumberGenerator.Next( 0, ListOfPrimes.Length ) ]
    
    /// <summary>Deturmines if two numbers are relatively prime.</summary>
    /// <returns>Returns true if the inputs are retatively prime, otherwise false.</returns>
    let AreRelativelyPrime a b =
        match Global.GCD a b with
        | 1 -> true
        | _ -> false

    /// <summary>Finds a number that is relatively prime to the input.</summary>
    /// <returns>A value relatively prime to the input.</returns>
    /// <remarks>
    ///     <para>
    ///         This function only uses prime numbers to find the relatively prime number.
    ///         This is done for speed, as it is much more likely a prime number will be relatively prime to an arbitrary number.
    ///     </para>
    ///     <para>
    ///         This function does not call AreRelativelyPrime.
    ///         This was to simplify the function by not needing to let the RandomPrime value.
    ///     </para>
    /// </remarks>
    let rec RelativelyPrime i =
        match RandomPrime () with
        | x when 1 <> Global.GCD i x -> RelativelyPrime i
        | x -> x
