#region header

// Arkane.Core - Logger.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 7:44 PM

#endregion

#region using

using System ;

#endregion

namespace ArkaneSystems.Arkane.Logging
{
    /// <summary>
    ///     Logger delegate.
    /// </summary>
    /// <param name="logLevel">The log level.</param>
    /// <param name="messageFunc">The message function.</param>
    /// <param name="exception">The optional exception.</param>
    /// <param name="formatParameters">The optional format parameters.</param>
    /// <returns>true if the message was successfully logged. Otherwise false.</returns>
    public delegate bool Logger (LogLevel        logLevel,
                                 Func <string>?  messageFunc,
                                 Exception?      exception = null,
                                 params object[] formatParameters) ;
}
