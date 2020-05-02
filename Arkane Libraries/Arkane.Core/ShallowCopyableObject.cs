#region header

// Arkane.Core - ShallowCopyableObject.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:34 AM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     A base class for shallow-copyable objects, providing a simple implementation of <see cref="ShallowCopy()" />.
    /// </summary>
    [PublicAPI]
    public abstract class ShallowCopyableObject : object, IShallowCopy <ShallowCopyableObject>
    {
        /// <summary>
        ///     Creates a shallow copy of an object, a new instance of the class whose fields reference the same objects as the
        ///     original object.
        /// </summary>
        /// <returns>A shallow copy of the object.</returns>
        [NotNull]
        public ShallowCopyableObject ShallowCopy () => (ShallowCopyableObject) this.MemberwiseClone () ;
    }
}
