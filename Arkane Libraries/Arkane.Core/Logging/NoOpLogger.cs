#region header

// Arkane.Core - NoOpLogger.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 12:06 AM

#endregion

#region using

using System ;

#endregion

namespace ArkaneSystems.Arkane.Logging
{
    /// <summary>
    ///     A logger to /dev/null.
    /// </summary>
    internal class NoOpLogger : ILog
    {
        internal static readonly NoOpLogger Instance = new NoOpLogger () ;

        /// <inheritdoc />
        public bool Log (LogLevel        logLevel,
                         Func <string>?  messageFunc,
                         Exception?      exception = null,
                         params object[] formatParameters) => false ;
    }
}
