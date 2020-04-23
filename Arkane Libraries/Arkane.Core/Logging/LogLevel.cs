#region header

// Arkane.Core - LogLevel.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 7:33 PM

#endregion

#region using

using System.ComponentModel ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Logging
{
    /// <summary>
    ///     The current logging level.
    /// </summary>
    [PublicAPI]
    public enum LogLevel
    {
        /// <summary>
        ///     Trace.
        /// </summary>
        [Description ("Trace")]
        Trace,

        /// <summary>
        ///     Debugging message.
        /// </summary>
        [Description ("Debugging")]
        Debug,

        /// <summary>
        ///     Informational message.
        /// </summary>
        [Description ("Informational")]
        Info,

        /// <summary>
        ///     Warning.
        /// </summary>
        [Description ("Warning")]
        Warn,

        /// <summary>
        ///     Error.
        /// </summary>
        [Description ("Error")]
        Error,

        /// <summary>
        ///     Fatal error.
        /// </summary>
        [Description ("Fatal error")]
        Fatal
    }
}
