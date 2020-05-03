﻿#region header

// Arkane.Core - CrossThreadException.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 7:43 AM

#endregion

#region using

using System ;
using System.Runtime.Serialization ;
using System.Security.Permissions ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane.Threading
{
    /// <summary>
    ///     Special exception used to wrap exceptions being marshaled across threads in order to
    ///     preserve the stack trace, etc., information generated by the original exception.
    /// </summary>
    [Serializable]
    [PublicAPI]
    [ThreadUnsafe]
    public sealed class CrossThreadException : Exception
    {
        /// <summary>
        ///     Creates a wrapper exception for a cross-thread marshaled exception.
        /// </summary>
        /// <param name="innerException">The exception marshaled across from the other thread.</param>
        public CrossThreadException (Exception innerException)
            : base (
                    string.Format (
                                   Resources.CrossThreadException_CrossThreadException_ExceptionHasBeenMarshaled,
                                   innerException.Message),
                    innerException)
        { }

        /// <inheritdoc />
        [SecurityPermission (SecurityAction.Demand, SerializationFormatter = true)]
        private CrossThreadException ([NotNull] SerializationInfo info, StreamingContext context)
            : base (info, context)
        { }
    }
}
