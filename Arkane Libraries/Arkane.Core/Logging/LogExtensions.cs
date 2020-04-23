#region header

// Arkane.Core - LogExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 7:53 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Logging
{
    /// <summary>
    ///     Extension methods for the <see cref="ILog" /> interface.
    /// </summary>
    [PublicAPI]
    public static class LogExtensions
    {
        #region Internal helpers

        internal static readonly object[] EmptyParams = new object[0] ;

        // Allow passing callsite-logger-type to LogProviderBase using messageFunc
        private static Func <string> WrapLogInternal (Func <string> messageFunc)
        {
            var wrappedMessageFunc = new Func <string> (() => { return messageFunc () ; }) ;
            return wrappedMessageFunc ;
        }

        // Allow passing callsite-logger-type to LogProviderBase using messageFunc
        internal static Func <string> WrapLogSafeInternal (LoggerExecutionWrapper logger, Func <string> messageFunc)
        {
            var wrappedMessageFunc = new Func <string> (() =>
                                                        {
                                                            try
                                                            {
                                                                return messageFunc () ;
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                logger.WrappedLogger (LogLevel.Error,
                                                                                      () => LoggerExecutionWrapper
                                                                                         .FailedToGenerateLogMessage,
                                                                                      ex,
                                                                                      LogExtensions.EmptyParams) ;
                                                            }

                                                            return string.Empty ;
                                                        }) ;

            return wrappedMessageFunc ;
        }

        #endregion Internal helpers

        #region Check if log level enabled

        /// <summary>
        ///     Checks if the <see cref="LogLevel.Debug" /> log level is enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to check with.</param>
        /// <returns>true if the log level is enabled; false otherwise.</returns>
        public static bool IsDebugEnabled (this ILog logger) => logger.Log (LogLevel.Debug, null, null, LogExtensions.EmptyParams) ;

        /// <summary>
        ///     Checks if the <see cref="LogLevel.Error" /> log level is enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to check with.</param>
        /// <returns>true if the log level is enabled; false otherwise.</returns>
        public static bool IsErrorEnabled (this ILog logger) => logger.Log (LogLevel.Error, null, null, LogExtensions.EmptyParams) ;

        /// <summary>
        ///     Checks if the <see cref="LogLevel.Fatal" /> log level is enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to check with.</param>
        /// <returns>true if the log level is enabled; false otherwise.</returns>
        public static bool IsFatalEnabled (this ILog logger) => logger.Log (LogLevel.Fatal, null, null, LogExtensions.EmptyParams) ;

        /// <summary>
        ///     Checks if the <see cref="LogLevel.Info" /> log level is enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to check with.</param>
        /// <returns>true if the log level is enabled; false otherwise.</returns>
        public static bool IsInfoEnabled (this ILog logger) => logger.Log (LogLevel.Info, null, null, LogExtensions.EmptyParams) ;

        /// <summary>
        ///     Checks if the <see cref="LogLevel.Trace" /> log level is enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to check with.</param>
        /// <returns>true if the log level is enabled; false otherwise.</returns>
        public static bool IsTraceEnabled (this ILog logger) => logger.Log (LogLevel.Trace, null, null, LogExtensions.EmptyParams) ;

        /// <summary>
        ///     Checks if the <see cref="LogLevel.Warn" /> log level is enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to check with.</param>
        /// <returns>true if the log level is enabled; false otherwise.</returns>
        public static bool IsWarnEnabled (this ILog logger) => logger.Log (LogLevel.Warn, null, null, LogExtensions.EmptyParams) ;

        #endregion Check if log level enabled

        #region Debug

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Debug" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="messageFunc">The message function.</param>
        public static void Debug (this ILog logger, Func <string> messageFunc)
        {
            if (logger.IsDebugEnabled ())
                logger.Log (LogLevel.Debug, LogExtensions.WrapLogInternal (messageFunc), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Debug" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        public static void Debug (this ILog logger, string message)
        {
            if (logger.IsDebugEnabled ())
                logger.Log (LogLevel.Debug, message.AsFunc (), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Debug" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Debug (this ILog logger, string message, params object[] args)
        {
            if (logger.IsDebugEnabled ())
                logger.Log (LogLevel.Debug, message.AsFunc (), null, args) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Debug" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Debug (this ILog logger, string message, Exception exception)
        {
            if (logger.IsDebugEnabled ())
                logger.Log (LogLevel.Debug, message.AsFunc (), exception, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Debug" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Debug (this ILog logger, string message, Exception exception, params object[] args)
        {
            if (logger.IsDebugEnabled ())
                logger.Log (LogLevel.Debug, message.AsFunc (), exception, args) ;
        }

        #endregion Debug

        #region Error

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Error" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="messageFunc">The message function.</param>
        public static void Error (this ILog logger, Func <string> messageFunc)
        {
            if (logger.IsErrorEnabled ())
                logger.Log (LogLevel.Error, LogExtensions.WrapLogInternal (messageFunc), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Error" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        public static void Error (this ILog logger, string message)
        {
            if (logger.IsErrorEnabled ())
                logger.Log (LogLevel.Error, message.AsFunc (), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Error" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Error (this ILog logger, string message, params object[] args)
        {
            if (logger.IsErrorEnabled ())
                logger.Log (LogLevel.Error, message.AsFunc (), null, args) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Error" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error (this ILog logger, string message, Exception exception)
        {
            if (logger.IsErrorEnabled ())
                logger.Log (LogLevel.Error, message.AsFunc (), exception, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Error" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Error (this ILog logger, string message, Exception exception, params object[] args)
        {
            if (logger.IsErrorEnabled ())
                logger.Log (LogLevel.Error, message.AsFunc (), exception, args) ;
        }

        #endregion Error

        #region Fatal

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Fatal" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="messageFunc">The message function.</param>
        public static void Fatal (this ILog logger, Func <string> messageFunc)
        {
            if (logger.IsFatalEnabled ())
                logger.Log (LogLevel.Fatal, LogExtensions.WrapLogInternal (messageFunc), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Fatal" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        public static void Fatal (this ILog logger, string message)
        {
            if (logger.IsFatalEnabled ())
                logger.Log (LogLevel.Fatal, message.AsFunc (), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Fatal" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Fatal (this ILog logger, string message, params object[] args)
        {
            if (logger.IsFatalEnabled ())
                logger.Log (LogLevel.Fatal, message.AsFunc (), null, args) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Fatal" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Fatal (this ILog logger, string message, Exception exception)
        {
            if (logger.IsFatalEnabled ())
                logger.Log (LogLevel.Fatal, message.AsFunc (), exception, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Fatal" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Fatal (this ILog logger, string message, Exception exception, params object[] args)
        {
            if (logger.IsFatalEnabled ())
                logger.Log (LogLevel.Fatal, message.AsFunc (), exception, args) ;
        }

        #endregion Fatal

        #region Info

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Info" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="messageFunc">The message function.</param>
        public static void Info (this ILog logger, Func <string> messageFunc)
        {
            if (logger.IsInfoEnabled ())
                logger.Log (LogLevel.Info, LogExtensions.WrapLogInternal (messageFunc), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Info" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        public static void Info (this ILog logger, string message)
        {
            if (logger.IsInfoEnabled ())
                logger.Log (LogLevel.Info, message.AsFunc (), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Info" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Info (this ILog logger, string message, params object[] args)
        {
            if (logger.IsInfoEnabled ())
                logger.Log (LogLevel.Info, message.AsFunc (), null, args) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Info" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Info (this ILog logger, string message, Exception exception)
        {
            if (logger.IsInfoEnabled ())
                logger.Log (LogLevel.Info, message.AsFunc (), exception, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Info" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Info (this ILog logger, string message, Exception exception, params object[] args)
        {
            if (logger.IsInfoEnabled ())
                logger.Log (LogLevel.Info, message.AsFunc (), exception, args) ;
        }

        #endregion Info

        #region Trace

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Trace" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="messageFunc">The message function.</param>
        public static void Trace (this ILog logger, Func <string> messageFunc)
        {
            if (logger.IsTraceEnabled ())
                logger.Log (LogLevel.Trace, LogExtensions.WrapLogInternal (messageFunc), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Trace" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        public static void Trace (this ILog logger, string message)
        {
            if (logger.IsTraceEnabled ())
                logger.Log (LogLevel.Trace, message.AsFunc (), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Trace" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Trace (this ILog logger, string message, params object[] args)
        {
            if (logger.IsTraceEnabled ())
                logger.Log (LogLevel.Trace, message.AsFunc (), null, args) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Trace" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Trace (this ILog logger, string message, Exception exception)
        {
            if (logger.IsTraceEnabled ())
                logger.Log (LogLevel.Trace, message.AsFunc (), exception, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Trace" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Trace (this ILog logger, string message, Exception exception, params object[] args)
        {
            if (logger.IsTraceEnabled ())
                logger.Log (LogLevel.Trace, message.AsFunc (), exception, args) ;
        }

        #endregion Trace

        #region Warn

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Warn" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="messageFunc">The message function.</param>
        public static void Warn (this ILog logger, Func <string> messageFunc)
        {
            if (logger.IsWarnEnabled ())
                logger.Log (LogLevel.Warn, LogExtensions.WrapLogInternal (messageFunc), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Warn" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        public static void Warn (this ILog logger, string message)
        {
            if (logger.IsWarnEnabled ())
                logger.Log (LogLevel.Warn, message.AsFunc (), null, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs a message at the <see cref="LogLevel.Warn" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Warn (this ILog logger, string message, params object[] args)
        {
            if (logger.IsWarnEnabled ())
                logger.Log (LogLevel.Warn, message.AsFunc (), null, args) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Warn" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Warn (this ILog logger, string message, Exception exception)
        {
            if (logger.IsWarnEnabled ())
                logger.Log (LogLevel.Warn, message.AsFunc (), exception, LogExtensions.EmptyParams) ;
        }

        /// <summary>
        ///     Logs an exception at the <see cref="LogLevel.Warn" /> log level, if enabled.
        /// </summary>
        /// <param name="logger">The <see cref="ILog" /> to use.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">Optional format parameters for the message.</param>
        public static void Warn (this ILog logger, string message, Exception exception, params object[] args)
        {
            if (logger.IsWarnEnabled ())
                logger.Log (LogLevel.Warn, message.AsFunc (), exception, args) ;
        }

        #endregion Warn
    }
}

/*
        
*/
