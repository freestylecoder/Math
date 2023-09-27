namespace Freestylecoding.Math.FSharp.Tests

open Xunit
open Freestylecoding.Math

module Natural =
    module ToString =
        [<Theory>]
        [<InlineData( 0u, "0" )>]          // Sanity
        [<InlineData( 1u, "1" )>]          // Sanity
        [<InlineData( 123u, "123" )>]      // multiple bits
        [<InlineData( 45678u, "45678" )>]  // rev
        let Sanity n s =
            Assert.Equal( s, Natural([n]).ToString() )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal( "1234567890123456789", Natural( [ 0x112210F4u; 0x7DE98115u ] ).ToString() )

    module Parse =
        [<Theory>]
        [<InlineData( 0u, "0" )>]          // Sanity
        [<InlineData( 1u, "1" )>]          // Sanity
        [<InlineData( 123u, "123" )>]      // multiple bits
        [<InlineData( 45678u, "45678" )>]  // rev
        let Sanity n s =
            Assert.Equal( Natural([n]), Natural.Parse(s) )
    
        [<Fact>]
        let Bigger () =
            Assert.Equal( Natural( [ 0x112210F4u; 0x7DE98115u ] ), Natural.Parse("1234567890123456789") )
