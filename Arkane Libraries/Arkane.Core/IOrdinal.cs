#region header

// Arkane.Core - IOrdinal.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 1:51 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     An interface defining classes whose instances are ordered by an ordinal index.
    /// </summary>
    /// <remarks>
    ///     It is permissible to throw <see cref="InvalidOperationException" /> from the setter if a computed ordinal is being
    ///     used.
    /// </remarks>
    [PublicAPI]
    public interface IOrdinal
    {
        /// <summary>
        ///     The ordinal index defining the ordering of the instances.
        /// </summary>
        int Ordinal { get ; set ; }
    }
}
