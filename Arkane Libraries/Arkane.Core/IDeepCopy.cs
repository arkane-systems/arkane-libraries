#region header

// Arkane.Core - IDeepCopy.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:19 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Supports deep copying, which creates a new instance of a class whose fields reference new copies of the original
    ///     object's fields.
    /// </summary>
    /// <typeparam name="T">The type of the deep-copyable object.</typeparam>
    /// <remarks>
    ///     <para>
    ///         DO NOT implement this on the same class as <see cref="IShallowCopy{T}" />.
    ///     </para>
    /// </remarks>
    [PublicAPI]
    public interface IDeepCopy <out T> : ICloneable
    {
        /// <summary>
        ///     Creates a deep copy of an object, a new instance of the class whose fields reference new copies of the original
        ///     object's fields.
        /// </summary>
        /// <returns>A deep copy of the object.</returns>
        T DeepCopy () ;

        /// <inheritdoc />
        object? ICloneable.Clone () => this.DeepCopy () ;
    }
}
