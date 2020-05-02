#region header

// Arkane.Core - IFluentInterface.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:10 AM

#endregion

#region using

using System ;
using System.ComponentModel ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Hides methods of <see cref="object" /> that only get in the way when seen on a fluent interface.
    /// </summary>
    [PublicAPI]
    public interface IFluentInterface
    {
#pragma warning disable 1591

        // ReSharper disable CodeAnnotationAnalyzer

        /// <inheritdoc cref="System.Object" />
        [EditorBrowsable (EditorBrowsableState.Never)]
        bool Equals ([CanBeNull] object obj) ;

        /// <inheritdoc cref="System.Object" />
        [EditorBrowsable (EditorBrowsableState.Never)]
        int GetHashCode () ;

        /// <inheritdoc cref="System.Object" />
        [EditorBrowsable (EditorBrowsableState.Never)]
        string ToString () ;

        /// <inheritdoc cref="System.Object" />
        [EditorBrowsable (EditorBrowsableState.Never)]
        Type GetType () ;

        // ReSharper restore CodeAnnotationAnalyzer
#pragma warning restore 1591
    }
}
