#region header

// Arkane.Core - Async.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 10:21 AM

#endregion

#region using

using System ;
using System.Threading.Tasks ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Special helper methods to enable asynchronous lambdas to function as asynchronous blocks.
    /// </summary>
    /// <remarks>
    ///     <para>Usage:</para>
    ///     <code>
    /// async Task FooAsync()
    /// {
    ///    Console.WriteLine(“Starting FooAsync”);
    ///    Task t1 = AsyncBlock(async delegate {
    ///    Console.WriteLine(“Starting first async block”);
    ///    await Task.Delay(1000);
    ///        Console.WriteLine(“Done first block”);
    ///    });
    ///    Task t2 = AsyncBlock(async delegate {
    ///        Console.WriteLine(“Starting second async block”);
    ///        await Task.Delay(2000);
    ///        Console.WriteLine(“Done second block”);
    ///    });
    ///    await Task.WhenAll(t1, t2);
    ///    Console.WriteLine(“Done FooAsync”);
    /// }
    /// </code>
    /// </remarks>
    [PublicAPI]
    public static class Async
    {
        /// <summary>
        ///     Introduce an async block/lambda that returns void.
        /// </summary>
        /// <param name="asyncMethod">The async block/lambda.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void BlockReturningVoid ([Required] Action asyncMethod)
            => asyncMethod () ;

        /// <summary>
        ///     Introduce an async block/lambda that returns a <see cref="Task" />.
        /// </summary>
        /// <param name="asyncMethod">The async block/lambda.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static Task Block ([Required] Func <Task> asyncMethod)
            => asyncMethod () ;

        /// <summary>
        ///     Introduce an async block/lambda that returns a <see cref="Task{T}" />.
        /// </summary>
        /// <param name="asyncMethod">The async block/lambda.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [ItemCanBeNull]
        public static Task <T> Block <T> ([Required] Func <Task <T>> asyncMethod)
            => asyncMethod () ;
    }
}
