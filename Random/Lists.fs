namespace Freestylecoding.Math.Random

module Lists =
    /// <summary>Randomizes the elements of a list.</summary>
    /// <param name="l">A list, the type of the elements does not matter.</param>
    /// <returns>A new list containng all the elements of the input list in a random order.</returns>
    let Shuffle (l:'a list) =
        let start = Global.RandomNumberGenerator.Next( 0, l.Length )
        let step = Primes.RelativelyPrime l.Length

        let rec func (l:'a list) (i:int list) =
            match i with
            | [] -> []
            | head :: tail ->
                l.Item( head ) :: func l tail

        [ start .. step .. start + ( step * ( l.Length - 1 ) ) ]
        |> List.map ( fun i -> i % l.Length )
        |> func l 
