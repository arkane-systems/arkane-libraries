#region header

// Arkane.Core - LoggerExecutionWrapper.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 12:14 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Properties ;

#endregion

namespace ArkaneSystems.Arkane.Logging
{
    internal class LoggerExecutionWrapper : ILog
    {
        #region Nested type: CallSiteExtension

        private class CallSiteExtension : ICallSiteExtension
        {
            bool ICallSiteExtension.Log (Logger        logger,
                                         LogLevel      logLevel,
                                         Func <string> messageFunc,
                                         Exception?    exception,
                                         object[]      formatParameters) =>
                logger (logLevel, messageFunc, exception, formatParameters) ;
        }

        #endregion

        #region Nested type: ICallSiteExtension

        private interface ICallSiteExtension
        {
            bool Log (Logger         logger,
                      LogLevel       logLevel,
                      Func <string?> messageFunc,
                      Exception?     exception,
                      object[]       formatParameters) ;
        }

        #endregion

        internal static readonly string FailedToGenerateLogMessage = Resources.LoggerExecutionWrapper_FailedToGenerateLogMessage ;

        internal LoggerExecutionWrapper (Logger logger, Func <bool>? getIsDisabled = null)
        {
            this.WrappedLogger  = logger ;
            this.callSiteLogger = new CallSiteExtension () ;
            this.getIsDisabled  = getIsDisabled ?? (() => false) ;
        }

        private readonly ICallSiteExtension callSiteLogger ;

        private readonly Func <bool> getIsDisabled ;

        private Func <string>? lastExtensionMethod ;

        internal Logger WrappedLogger { get ; }

        /// <inheritdoc />
        public bool Log (LogLevel        logLevel,
                         Func <string>?  messageFunc,
                         Exception?      exception = null,
                         params object[] formatParameters)
        {
            if (this.getIsDisabled ())
                return false ;

            if (messageFunc == null)
                return this.WrappedLogger (logLevel, null, null, LogExtensions.EmptyParams) ;

            // Callsite HACK - Using the messageFunc to provide the callsite-logger-type
            Func <string>? lastExtensionMethod = this.lastExtensionMethod ;
            if (lastExtensionMethod?.Equals (messageFunc) != true)
            {
                // Callsite HACK - Cache the last validated messageFunc as Equals is faster than type-check
                lastExtensionMethod = null ;
                var methodType = messageFunc.Method.DeclaringType ;
                if ((methodType == typeof (LogExtensions)) ||
                    ((methodType != null) && (methodType.DeclaringType == typeof (LogExtensions))))
                    lastExtensionMethod = messageFunc ;
            }

            if (lastExtensionMethod != null)
            {
                // Callsite HACK - LogExtensions has called virtual ILog interface method to get here, callsite-stack is good
                this.lastExtensionMethod = lastExtensionMethod ;
                return this.WrappedLogger (logLevel,
                                           LogExtensions.WrapLogSafeInternal (this, messageFunc),
                                           exception,
                                           formatParameters) ;
            }

            var wrappedMessageFunc = new Func <string?> (() =>
                                                         {
                                                             try
                                                             {
                                                                 return messageFunc () ;
                                                             }
                                                             catch (Exception ex)
                                                             {
                                                                 this.WrappedLogger (LogLevel.Error,
                                                                                     () => LoggerExecutionWrapper
                                                                                        .FailedToGenerateLogMessage,
                                                                                     ex) ;
                                                             }

                                                             return null ;
                                                         }) ;

            // Callsite HACK - Need to ensure proper callsite stack without inlining, so calling the logger within a virtual interface method
            return this.callSiteLogger.Log (this.WrappedLogger, logLevel, wrappedMessageFunc, exception, formatParameters) ;
        }
    }
}
