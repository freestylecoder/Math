namespace Freestylecoding.Math

open System
open System.Runtime.CompilerServices
open Microsoft.FSharp.NativeInterop

// I do bad things with memory here.
// I know I do bad things with memory here.
// I'm "ok" with the bad things I do with memory here
// This whole class is a hack around issues with F# and ReadOnlySpan<'T>
#nowarn "9"

[<Extension>]
type ReadOnlySpanExtensions() =
    [<Extension>]
    static member ToList<'T when 'T : unmanaged> (self:ReadOnlySpan<'T>) : 'T list =
        use fixedSelf = fixed self
        let len = self.Length - 1

        [
            for i in 0 .. len do
                NativePtr.get fixedSelf i
        ]

    [<Extension>]
    static member ToList<'T when 'T : unmanaged> (self:ReadOnlySpan<'T>, ranges:Span<Range>) : 'T list =
        use fixedSelf = fixed self
        use fixedRanges = fixed ranges

        let len = ranges.Length - 1

        [
            for i = 0 to len do
                let range = NativePtr.get fixedRanges i

                for j = range.Start.Value to ( range.End.Value - 1 ) do
                    NativePtr.get fixedSelf j
        ]

    [<Extension>]
    static member ToSeq<'T when 'T : unmanaged> (self:ReadOnlySpan<'T>) : 'T seq =
        use fixedSelf = fixed self
        let len = self.Length - 1

        seq {
            for i in 0 .. len do
                NativePtr.get fixedSelf i
        }

    [<Extension>]
    static member IndexOf<'T when 'T :> IEquatable<'T> and 'T : unmanaged> (self:ReadOnlySpan<'T>, predicate:('T -> bool)) : int =
        self.ToSeq()
        |> Seq.findIndex predicate

    [<Extension>]
    static member LastIndexOf<'T when 'T :> IEquatable<'T> and 'T : unmanaged> (self:ReadOnlySpan<'T>, predicate:('T -> bool)) : int =
        self.ToSeq()
        |> Seq.findIndexBack predicate
