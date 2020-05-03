#region header

// Arkane.Core - RandomProvider.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 2:44 PM

#endregion

#region using

using System ;
using System.Threading ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Provide a per-thread singleton <see cref="System.Random" /> instance to avoid the problem of repeated values that
    ///     comes with initializing multiple instances before the default seed (current date and time) changes.
    /// </summary>
    /// <remarks>
    ///     Done per-thread to avoid the need for locking, since Random is not thread-safe.
    /// </remarks>
    [PublicAPI]
    [ExplicitlySynchronized]
    public static class RandomProvider
    {
        private static int seed = Environment.TickCount ;

        private static readonly ThreadLocal <Random> RandomWrapper =
            new ThreadLocal <Random> (() => new Random (Interlocked.Increment (ref RandomProvider.seed))) ;

        /// <summary>
        ///     Get the per-thread singleton instance of <see cref="System.Random" />, properly seeded.
        /// </summary>
        /// <returns>The per-thread singleton instance of <see cref="System.Random" />.</returns>
        /// <remarks>Thread-safe.</remarks>
        public static Random GetInstance () => RandomProvider.RandomWrapper.Value ;
    }
}
