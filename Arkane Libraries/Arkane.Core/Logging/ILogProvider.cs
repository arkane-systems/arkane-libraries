#region header

// Arkane.Core - ILogProvider.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 7:40 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Logging
{
    /// <summary>
    ///     Represents a means to get a <see cref="Logger" />.
    /// </summary>
    [PublicAPI]
    public interface ILogProvider
    {
        /// <summary>
        ///     Gets the specified named logger.
        /// </summary>
        /// <param name="name">Name of the logger to fetch.</param>
        /// <returns>The logger reference.</returns>
        Logger GetLogger (string name) ;

        /// <summary>
        ///     Open a nested diagnostics context.
        /// </summary>
        /// <param name="message">The message to add to the diagnostics context.</param>
        /// <returns>A disposable that, when disposed, removes the message from the context.</returns>
        IDisposable OpenNestedContext (string message) ;

        /// <summary>
        ///     Open a mapped diagnostics context.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <param name="value">A value.</param>
        /// <param name="destructure">Determines whether or not to call the destructor when the context is disposed.</param>
        /// <returns>A disposable that, when disposed, removes the map from the context.</returns>
        IDisposable OpenMappedContext (string key, object value, bool destructure = false) ;
    }
}
