namespace Freestylecoding.Math.FSharp.Tests

open Xunit
open Freestylecoding.Math

module Integer =
    module Parse =
        [<Theory>]
        [<InlineData( "0", 0u, false )>]          // Sanity
        [<InlineData( "1", 1u, false )>]          // Sanity
        [<InlineData( "123", 123u, false )>]      // multiple bits
        [<InlineData( "45678", 45678u, false )>]  // rev
        [<InlineData( "-0", 0u, false )>]          // Sanity
        [<InlineData( "-1", 1u, true )>]          // Sanity
        [<InlineData( "-123", 123u, true )>]      // multiple bits
        [<InlineData( "-45678", 45678u, true )>]  // rev
        let Sanity str data negative =
            Assert.Equal( Integer([data], negative), Integer.Parse(str) )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal( Integer( [ 0x112210F4u; 0x7DE98115u ] ), Integer.Parse("1234567890123456789") )

        [<Fact>]
        let BiggerNegative () =
            Assert.Equal( Integer( [ 0x112210F4u; 0x7DE98115u ], true ), Integer.Parse("-1234567890123456789") )
