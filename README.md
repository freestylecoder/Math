# Math

F# math libraries for dealing with non-standard things.

The library is also 95% compatiable with C#. A few of the custom operators are not supported in C#, due to C# not supporting custom operators.

This began as a project to give me a random prime number. Basically, I needed everything you see in the Primes project.

From there I began to tinker. I wanted to add more things that normally don't come in a math library. This led to the Combinatorics project.

However, results from combinatoric functions get REALLY big. This got me to create my own data type that can handle integers of any size. That's where we are now.

This Library is not fully ready for primetime. The Natural and Integer types should be working enough for general use, but there is still a lot of cleanup and optimization that could be done.
