#region header

// Arkane.Core - LoggingException.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 11:59 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Logging.LogProviders
{
    /// <summary>
    ///     Exception used to report logging infrastructure errors.
    /// </summary>
    [PublicAPI]
    public class LoggingException : Exception
    {
        /// <summary>
        ///     Initializes a new LibLogException with the specified message.
        /// </summary>
        /// <param name="message">The message</param>
        public LoggingException (string message)
            : base (message)
        { }

        /// <summary>
        ///     Initializes a new LibLogException with the specified message and inner exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public LoggingException (string message, Exception inner)
            : base (message, inner)
        { }
    }
}
