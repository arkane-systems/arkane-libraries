#region header

// Arkane.Core - LogProviderBase.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 1:00 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Logging.LogProviders
{
    /// <summary>
    ///     Base class for specific log providers.
    /// </summary>
    [PublicAPI]
    public abstract class LogProviderBase : ILogProvider
    {
        #region Nested type: OpenMdc

        /// <summary>
        ///     Delegate defining the signature of the method opening a mapped diagnostics context.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <param name="value">A value.</param>
        /// <param name="destructure">Determines whether to call the destructor or not.</param>
        /// <returns>A disposable that when disposed removes the map from the context.</returns>
        protected delegate IDisposable OpenMdc (string key, object value, bool destructure) ;

        #endregion

        #region Nested type: OpenNdc

        /// <summary>
        ///     Delegate defining the signature of the method opening a nested diagnostics context.
        /// </summary>
        /// <param name="message">The message to add to the diagnostics context.</param>
        /// <returns>A disposable that when disposed removes the message from the context.</returns>
        protected delegate IDisposable OpenNdc (string message) ;

        #endregion

        private static readonly IDisposable NoOpDisposableInstance = new DisposableAction () ;

        /// <summary>
        ///     Initialize an instance of the <see cref="LogProviderBase" /> class by initializing the references to the nested
        ///     and mapped diagnostics context-obtaining functions.
        /// </summary>
        protected LogProviderBase ()
        {
            this.lazyOpenMdcMethod = new Lazy <OpenMdc> (this.GetOpenMdcMethod) ;
            this.lazyOpenNdcMethod = new Lazy <OpenNdc> (this.GetOpenNdcMethod) ;
        }

        /// <summary>
        ///     Error message should initializing the log provider fail.
        /// </summary>
        protected static readonly string ErrorInitializingProvider =
            Resources.LogProviderBase_ErrorInitializingProvider_ProblemInitializingLogProvider ;

        private readonly Lazy <OpenMdc> lazyOpenMdcMethod ;
        private readonly Lazy <OpenNdc> lazyOpenNdcMethod ;

        /// <summary>
        ///     Gets the specified named logger.
        /// </summary>
        /// <param name="name">Name of the logger.</param>
        /// <returns>The logger reference.</returns>
        public abstract Logger GetLogger (string name) ;

        /// <summary>
        ///     Opens a nested diagnostics context.
        /// </summary>
        /// <param name="message">The message to add to the diagnostics context.</param>
        /// <returns>A disposable that when disposed removes the message from the context.</returns>
        public IDisposable OpenNestedContext (string message) => this.lazyOpenNdcMethod.Value (message) ;

        /// <summary>
        ///     Opens a mapped diagnostics context.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <param name="value">A value.</param>
        /// <param name="destructure">Determines whether to call the destructor or not.</param>
        /// <returns>A disposable that when disposed removes the map from the context.</returns>
        public IDisposable OpenMappedContext (string key, object value, bool destructure = false) =>
            this.lazyOpenMdcMethod.Value (key, value, destructure) ;

        /// <summary>
        ///     Returns the provider-specific method to open a nested diagnostics context.
        /// </summary>
        /// <returns>A provider-specific method to open a nested diagnostics context.</returns>
        protected virtual OpenNdc GetOpenNdcMethod () => _ => LogProviderBase.NoOpDisposableInstance ;

        /// <summary>
        ///     Returns the provider-specific method to open a mapped diagnostics context.
        /// </summary>
        /// <returns>A provider-specific method to open a mapped diagnostics context.</returns>
        protected virtual OpenMdc GetOpenMdcMethod () => (_, __, ___) => LogProviderBase.NoOpDisposableInstance ;
    }
}
