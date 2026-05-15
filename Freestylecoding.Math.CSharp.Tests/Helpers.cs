using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.FSharp.Collections;

namespace Freestylecoding.Math.CSharp.Tests {
	internal static class Helpers {
		internal static readonly Math.Natural SmallNatural  = FListNatural( 0x4996_02D2u );
		internal const string SmallNaturalString = "1234567890";

		internal static readonly Math.Natural MediumNatural = FListNatural( 0xAB54_A98Cu, 0xEB1F_0AD2u );
		internal const string MediumNaturalString = "12345678901234567890";

		internal static readonly Math.Natural LargeNatural  = FListNatural( 0x0000_0001u, 0x8EE9_0FF6u, 0xC373_E0EEu, 0x4E3F_0AD2u );
		internal const string LargeNaturalString = "123456789012345678901234567890";

		// Used to initialize anything byref so we can see if it changes to 0
		internal static readonly Math.Natural DeadBeef = new Math.Natural( 0xDeadBeeful );

		internal static readonly CultureInfo US_Culture = new CultureInfo( "en-US" ); // US
		internal static readonly CultureInfo UK_Culture = new CultureInfo( "en-GB" ); // UK
		internal static readonly CultureInfo FR_Culture = new CultureInfo( "fr-FR" ); // France
		internal static readonly CultureInfo LU_Culture = new CultureInfo( "fr-LU" ); // Luxembourg

		internal static readonly IEnumerable<CultureInfo> Cultures = new[] { US_Culture, UK_Culture, FR_Culture, LU_Culture };

		internal static readonly NumberFormatInfo CurrentCultureNumberFormat = CultureInfo.CurrentCulture.NumberFormat;

		internal static readonly IEnumerable<object[]> Whitespace = [
            [ " " ],
            [ "  " ],
            [ "\t" ],
            [ "\t " ],
            [ "\t\t" ],
            [ "\n" ],
            [ "\r" ],
            [ "\r\n" ],
            [ "\n\r" ],
            [ "\n\t\r" ]
        ];

		internal static FSharpList<T> FList<T>( params T[] items ) {
			if( items.Any() )
				return new FSharpList<T>( items.First(), FList( items.Skip( 1 ).ToArray() ) );

			return FSharpList<T>.Empty;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Math.Natural FListNatural( params uint[] items ) =>
			new Math.Natural( FList( items ) );
	}
}
