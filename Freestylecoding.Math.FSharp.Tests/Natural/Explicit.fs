namespace Natural

open Xunit
open System
open Freestylecoding.Math

type public Explicit() =
    // Yes, these look a little funny
    // F# doesn't handle Implicit conversions very well
    // These methods mostly exist for C#

    [<Fact>]
    member public this.BigInteger () =
        Assert.Equal(
            Natural( [0x0000_0000u; 0x0000_0000u; 0x0000_0000u; 0x0000_0000u] ),
            Natural.op_Explicit(
                Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Natural( [0x0000_0000u; 0x0000_0000u; 0x0000_0000u; 0x0000_0001u] ),
            Natural.op_Explicit(
                Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Natural( [0x0000_0000u; 0x0000_0000u; 0x0000_0001u; 0x0000_0000u] ),
            Natural.op_Explicit(
                Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Natural( [0x0000_0000u; 0x0000_0001u; 0x0000_0000u; 0x0000_0000u] ),
            Natural.op_Explicit(
                Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Natural( [0x0000_0001u; 0x0000_0000u; 0x0000_0000u; 0x0000_0000u] ),
            Natural.op_Explicit(
                Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                        0x00uy; 0x00uy; 0x00uy; 0x00uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Natural( [0x0000_0001u; 0x0000_0001u; 0x0000_0001u; 0x0000_0001u] ),
            Natural.op_Explicit(
                Numerics.BigInteger(
                    [|
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                        0x00uy; 0x00uy; 0x00uy; 0x01uy;
                    |],
                    true,
                    true
                )
            )
        )
        Assert.Equal(
            Natural( [0x1200_0340u; 0x0560_0078u; 0x1234_5678u; 0x9ABC_DEF0u] ),
            Natural.op_Explicit(
                Numerics.BigInteger(
                    [|
                        0x12uy; 0x00uy; 0x03uy; 0x40uy;
                        0x05uy; 0x60uy; 0x00uy; 0x78uy;
                        0x12uy; 0x34uy; 0x56uy; 0x78uy;
                        0x9Auy; 0xBCuy; 0xDEuy; 0xF0uy;
                    |],
                    true,
                    true
                )
            )
        )

    [<Fact>]
    member public this.BigInteger_Negative () =
        let exc = Record.Exception(
            fun () ->
                Natural.op_Explicit( Numerics.BigInteger( -1m ) )
                |> ignore
        )
        Assert.NotNull( exc )
        Assert.IsType<System.OverflowException>( exc )
