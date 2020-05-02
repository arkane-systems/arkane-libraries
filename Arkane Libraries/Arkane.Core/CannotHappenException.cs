#region header

// Arkane.Core - CannotHappenException.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:45 AM

#endregion

#region using

using System ;
using System.Runtime.Serialization ;
using System.Security.Permissions ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Exception thrown to indicate that something has happened that cannot happen.  This exception should be
    ///     used to indicate code branches which it is logically impossible, within the structure of the program, for
    ///     execution to reach; e.g., the default branch of switch statements which provide a case for every member
    ///     of an enum, and the like.
    /// </summary>
    /// <remarks>
    ///     This is treated as a special case of <see cref="InvalidOperationException" />.
    /// </remarks>
    [Serializable]
    [PublicAPI]
    [PrivateThreadAware]
    public sealed class CannotHappenException : InvalidOperationException
    {
        /// <inheritdoc />
        public CannotHappenException ()
            : base (Resources.CannotHappenException_CannotHappenException_SomethingHappened)
        { }

        /// <inheritdoc />
        public CannotHappenException (string message)
            : base (message)
        { }

        /// <inheritdoc />
        public CannotHappenException (string message, Exception innerException)
            : base (message, innerException)
        { }

        /// <inheritdoc />
        [SecurityPermission (SecurityAction.Demand, SerializationFormatter = true)]
        private CannotHappenException (SerializationInfo info, StreamingContext context)
            : base (info, context)
        { }
    }
}
