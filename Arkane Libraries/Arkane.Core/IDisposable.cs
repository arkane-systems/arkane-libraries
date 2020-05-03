#region header

// Arkane.Core - IDisposable.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:53 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Encapsulates a read-only value, enabling consumers to signal when they are done with the value through the use of
    ///     the <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    [PublicAPI]
    public interface IDisposable <out T> : IDisposable
    {
        /// <summary>
        ///     The encapsulated value.
        /// </summary>
        T Value { get ; }
    }
}
