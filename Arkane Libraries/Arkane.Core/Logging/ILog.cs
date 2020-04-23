﻿#region header

// Arkane.Core - ILog.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 7:31 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Logging
{
    /// <summary>
    ///     A simple interface that represents a logger.
    /// </summary>
    [PublicAPI]
    public interface ILog
    {
        /// <summary>
        ///     Log a message at the specified log level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="messageFunc">The message function.</param>
        /// <param name="exception">An optional exception.</param>
        /// <param name="formatParameters">Optional format parameters for the message generated by <paramref name="messageFunc" />.</param>
        /// <returns>true if the message was logged. Otherwise false.</returns>
        /// <remarks>
        ///     <p>
        ///         Note to implementers: the <paramref name="messageFunc" /> should not be called if the <see cref="logLevel" />
        ///         is not enabled, so as not to incur performance penalties.
        ///     </p>
        ///     <p>To check IsEnabled, call Log with only LogLevel and check the return value. No event will be written.</p>
        /// </remarks>
        bool Log (LogLevel logLevel, Func <string>? messageFunc, Exception? exception = null, params object[] formatParameters) ;
    }
}
