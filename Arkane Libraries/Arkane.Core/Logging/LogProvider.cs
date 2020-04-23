#region header

// Arkane.Core - LogProvider.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 8:37 PM

#endregion

#region using

using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Runtime.CompilerServices ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Logging
{
    /// <summary>
    ///     Provides a mechanism to set the <see cref="ILogProvider" />
    ///     and create instances of <see cref="ILog" /> objects.
    /// </summary>
    [PublicAPI]
    public static class LogProvider
    {
        /// <summary>
        ///     Gets or sets a value indicating whether logging is disabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> is logging is disabled; otherwise <c>false</c>.
        /// </value>
        public static bool IsDisabled { get ; set ; }

        /// <summary>
        ///     Sets the current log provider.
        /// </summary>
        /// <param name="logProvider">The new current log provider.</param>
        public static void SetCurrentLogProvider (ILogProvider logProvider)
        {
            LogProvider.currentLogProvider = logProvider ;
            LogProvider.RaiseOnCurrentLogProviderSet () ;
        }

        /// <summary>
        ///     Gets a logger for the specified type.
        /// </summary>
        /// <typeparam name="T">The type whose name will be used for the logger.</typeparam>
        /// <returns>An instance of <see cref="ILog" />.</returns>
        public static ILog For <T> () => LogProvider.GetLogger (typeof (T)) ;

        /// <summary>
        ///     Gets a logger for the current class.
        /// </summary>
        /// <returns>An instance of <see cref="ILog" />.</returns>
        [MethodImpl (MethodImplOptions.NoInlining)]
        public static ILog GetCurrentClassLogger ()
        {
            var stackFrame = new StackFrame (1, false) ;
            return LogProvider.GetLogger (stackFrame.GetMethod ().DeclaringType) ;
        }

        /// <summary>
        ///     Gets a logger for the specified type.
        /// </summary>
        /// <param name="type">The type whose name will be used for the logger.</param>
        /// <param name="fallbackTypeName">If the type is null then this name will be used instead.</param>
        /// <returns>An instance of <see cref="ILog" />.</returns>
        public static ILog GetLogger (Type? type, string fallbackTypeName = "System.Object") =>
            LogProvider.GetLogger (type != null ? type.ToString () : fallbackTypeName) ;

        /// <summary>
        ///     Gets a logger with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>An instance of <see cref="ILog" />.</returns>
        public static ILog GetLogger (string name)
        {
            var logProvider = LogProvider.CurrentLogProvider ?? LogProvider.ResolveLogProvider () ;
            return logProvider == null
                       ? NoOpLogger.Instance
                       : (ILog) new LoggerExecutionWrapper (logProvider.GetLogger (name), () => LogProvider.IsDisabled) ;
        }

        /// <summary>
        ///     Opens a nested diagnostics context.
        /// </summary>
        /// <param name="message">A message.</param>
        /// <returns>An <see cref="IDisposable" /> that closes the context when disposed.</returns>
        public static IDisposable OpenNestedContext (string message)
        {
            var logProvider = LogProvider.CurrentLogProvider ?? LogProvider.ResolveLogProvider () ;

            return logProvider == null
                       ? new DisposableAction (() => { })
                       : logProvider.OpenNestedContext (message) ;
        }

        public static IDisposable OpenMappedContext (string key, object value, bool destructure = false)
        {
            var logProvider = LogProvider.CurrentLogProvider ?? LogProvider.ResolveLogProvider () ;

            return logProvider == null
                       ? new DisposableAction (() => { })
                       : logProvider.OpenMappedContext (key, value, destructure) ;
        }

        #region Internal helpers

        private static readonly Lazy <ILogProvider?> ResolvedLogProvider =
            new Lazy <ILogProvider?> (LogProvider.ForceResolveLogProvider) ;

        private static ILogProvider?           currentLogProvider ;
        private static Action <ILogProvider?>? onCurrentLogProviderSet ;

        [CanBeNull]
        internal static ILogProvider? CurrentLogProvider
        {
            [DebuggerStepThrough]
            get => LogProvider.currentLogProvider ;
        }

        /// <summary>
        ///     Sets an action that is invoked when a consumer of your library has called SetCurrentLogProvider. It is important
        ///     that you hook into this if you are using child libraries (especially ILmerged ones) that are using LibLog (or
        ///     other logging abstraction) so you adapt and delegate to them. See <see cref="SetCurrentLogProvider" />.
        /// </summary>
        internal static Action <ILogProvider?> OnCurrentLogProviderSet
        {
            set
            {
                LogProvider.onCurrentLogProviderSet = value ;
                LogProvider.RaiseOnCurrentLogProviderSet () ;
            }
        }

        internal delegate bool IsLoggerAvailable () ;

        internal delegate ILogProvider CreateLogProvider () ;

        internal static readonly List <Tuple <IsLoggerAvailable, CreateLogProvider>> LogProviderResolvers =
            new List <Tuple <IsLoggerAvailable, CreateLogProvider>>
            {
                new Tuple <IsLoggerAvailable, CreateLogProvider> (SerilogLogProvider.IsLoggerAvailable, () => new SerilogLogProvider())
            };

        private static void RaiseOnCurrentLogProviderSet ()
        {
            LogProvider.onCurrentLogProviderSet?.Invoke (LogProvider.currentLogProvider) ;
        }

        internal static ILogProvider? ResolveLogProvider () => LogProvider.ResolvedLogProvider.Value ;

        internal static ILogProvider? ForceResolveLogProvider ()
        {
            try
            {
                foreach (var providerResolver in LogProvider.LogProviderResolvers)
                {
                    if (providerResolver.Item1 ())
                        return providerResolver.Item2 () ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine (Resources.LogProvider_ForceResolveLogProvider_ExceptionResolvingLogProvider,
                                   typeof (LogProvider).Assembly.FullName,
                                   ex) ;
            }

            return null ;
        }

        #endregion Internal helpers
    }
}
