#region header

// Arkane.Core - DeepCopyableObject.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:33 AM

#endregion

#region using

using System ;
using System.IO ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Security ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     A base class for deep-copyable objects, providing a simple implementation of <see cref="DeepCopy()" />.
    /// </summary>
    /// <remarks>
    ///     Requires descendant objects and all objects contained in descendant objects to be serializable.
    /// </remarks>
    [Serializable]
    [PublicAPI]
    public class DeepCopyableObject : object, IDeepCopy <DeepCopyableObject>
    {
        /// <summary>
        ///     Creates a deep copy of an object, a new instance of the class whose fields reference new copies of the original
        ///     object's fields.
        /// </summary>
        /// <returns>A deep copy of the object.</returns>
        /// <exception cref="SerializationException">
        ///     An error has occurred during serialization, such as if an object in the graph
        ///     is not marked as serializable.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        [NotNull]
        public DeepCopyableObject DeepCopy ()
        {
            var bf = new BinaryFormatter () ;
            var ms = new MemoryStream () ;

            bf.Serialize (ms, this) ;
            ms.Flush () ;
            ms.Position = 0 ;

            return (DeepCopyableObject) bf.Deserialize (ms) ;
        }
    }
}
