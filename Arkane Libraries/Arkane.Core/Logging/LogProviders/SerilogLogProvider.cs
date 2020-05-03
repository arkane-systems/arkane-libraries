#region header

// Arkane.Core - SerilogLogProvider.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 11:38 AM

#endregion

#region using

using System ;
using System.Linq.Expressions ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;
using ArkaneSystems.Arkane.Reflection ;

#endregion

namespace ArkaneSystems.Arkane.Logging.LogProviders
{
    internal class SerilogLogProvider : LogProviderBase
    {
        #region Nested type: SerilogLogger

        internal class SerilogLogger
        {
            private static          object? debugLevel ;
            private static          object? errorLevel ;
            private static          object? fatalLevel ;
            private static          object? informationLevel ;
            private static          object? verboseLevel ;
            private static          object? warningLevel ;
            private static          Func <object, object, bool>? isEnabled ;
            private static          Action <object, object, string, object[]>? write ;
            private static          Action <object, object, Exception, string, object[]>? writeException ;
            private static readonly Lazy <bool> Initialized = new Lazy <bool> (SerilogLogger.Initialize) ;
            private static          Exception? initializeException ;

            internal SerilogLogger (object logger) => this.logger = logger ;

            private readonly object logger ;

            private static bool Initialize ()
            {
                try
                {
                    Type? logEventLevelType = Reflect.FindType (@"Serilog.Events.LogEventLevel", @"Serilog") ;

                    if (logEventLevelType == null)
                        throw new LoggingException (Resources.SerilogLogger_Initialize_LogLevelEventTypeNotFound) ;

                    SerilogLogger.debugLevel       = Enum.Parse (logEventLevelType, @"Debug",       false) ;
                    SerilogLogger.errorLevel       = Enum.Parse (logEventLevelType, @"Error",       false) ;
                    SerilogLogger.fatalLevel       = Enum.Parse (logEventLevelType, @"Fatal",       false) ;
                    SerilogLogger.informationLevel = Enum.Parse (logEventLevelType, @"Information", false) ;
                    SerilogLogger.verboseLevel     = Enum.Parse (logEventLevelType, @"Verbose",     false) ;
                    SerilogLogger.warningLevel     = Enum.Parse (logEventLevelType, @"Warning",     false) ;

                    // Func<object, object, bool> isEnabled = (logger, level) => { return ((Serilog.ILogger)logger).IsEnabled(level); }
                    Type? loggerType = Reflect.FindType (@"Serilog.ILogger", @"Serilog") ;

                    if (loggerType == null)
                        throw new LoggingException (Resources.SerilogLogger_Initialize_ILoggerNotFound) ;

                    MethodInfo?          isEnabledMethodInfo = loggerType.GetMethod (@"IsEnabled", logEventLevelType) ;
                    ParameterExpression  instanceParam       = Expression.Parameter (typeof (object)) ;
                    UnaryExpression      instanceCast        = Expression.Convert (instanceParam, loggerType) ;
                    ParameterExpression  levelParam          = Expression.Parameter (typeof (object)) ;
                    UnaryExpression      levelCast           = Expression.Convert (levelParam, logEventLevelType) ;
                    MethodCallExpression isEnabledMethodCall = Expression.Call (instanceCast, isEnabledMethodInfo!, levelCast) ;
                    SerilogLogger.isEnabled = Expression
                                             .Lambda <Func <object, object, bool>> (isEnabledMethodCall, instanceParam, levelParam)
                                             .Compile () ;

                    // Action<object, object, string> Write =
                    // (logger, level, message, params) => { ((SeriLog.ILoggerILogger)logger).Write(level, message, params); }
                    MethodInfo? writeMethodInfo =
                        loggerType.GetMethod (@"Write", logEventLevelType, typeof (string), typeof (object[])) ;
                    ParameterExpression messageParam        = Expression.Parameter (typeof (string)) ;
                    ParameterExpression propertyValuesParam = Expression.Parameter (typeof (object[])) ;
                    MethodCallExpression writeMethodExp = Expression.Call (
                                                                           instanceCast,
                                                                           writeMethodInfo!,
                                                                           levelCast,
                                                                           messageParam,
                                                                           propertyValuesParam) ;
                    Expression <Action <object, object, string, object[]>> expression =
                        Expression.Lambda <Action <object, object, string, object[]>> (
                                                                                       writeMethodExp,
                                                                                       instanceParam,
                                                                                       levelParam,
                                                                                       messageParam,
                                                                                       propertyValuesParam) ;
                    SerilogLogger.write = expression.Compile () ;

                    // Action<object, object, string, Exception> WriteException =
                    // (logger, level, exception, message) => { ((ILogger)logger).Write(level, exception, message, new object[]); }
                    MethodInfo? writeExceptionMethodInfo = loggerType.GetMethod (@"Write",
                                                                                 logEventLevelType,
                                                                                 typeof (Exception),
                                                                                 typeof (string),
                                                                                 typeof (object[])) ;
                    ParameterExpression exceptionParam = Expression.Parameter (typeof (Exception)) ;
                    writeMethodExp = Expression.Call (
                                                      instanceCast,
                                                      writeExceptionMethodInfo!,
                                                      levelCast,
                                                      exceptionParam,
                                                      messageParam,
                                                      propertyValuesParam) ;
                    SerilogLogger.writeException = Expression.Lambda <Action <object, object, Exception, string, object[]>> (
                                                                                                                             writeMethodExp,
                                                                                                                             instanceParam,
                                                                                                                             levelParam,
                                                                                                                             exceptionParam,
                                                                                                                             messageParam,
                                                                                                                             propertyValuesParam)
                                                             .Compile () ;
                }
                catch (Exception ex)
                {
                    SerilogLogger.initializeException = ex ;
                    return false ;
                }

                return true ;
            }

            public bool Log (LogLevel        logLevel,
                             Func <string>?  messageFunc,
                             Exception?      exception,
                             params object[] formatParameters)
            {
                if (!SerilogLogger.Initialized.Value)
                    throw new LoggingException (LogProviderBase.ErrorInitializingProvider, SerilogLogger.initializeException!) ;

                object? translatedLevel = SerilogLogger.TranslateLevel (logLevel) ;
                if (messageFunc == null)
                    return SerilogLogger.isEnabled! (this.logger, translatedLevel) ;

                if (!SerilogLogger.isEnabled! (this.logger, translatedLevel))
                    return false ;

                if (exception != null)
                    this.LogException (translatedLevel, messageFunc, exception, formatParameters) ;
                else
                    this.LogMessage (translatedLevel, messageFunc, formatParameters) ;

                return true ;
            }

            private void LogMessage (object translatedLevel, Func <string> messageFunc, object[] formatParameters)
            {
                SerilogLogger.write! (this.logger, translatedLevel, messageFunc (), formatParameters) ;
            }

            private void LogException (object        logLevel,
                                       Func <string> messageFunc,
                                       Exception     exception,
                                       object[]      formatParams)
            {
                SerilogLogger.writeException! (this.logger, logLevel, exception, messageFunc (), formatParams) ;
            }

            private static object TranslateLevel (LogLevel logLevel)
            {
                return (logLevel switch
                        {
                            LogLevel.Fatal => SerilogLogger.fatalLevel!,
                            LogLevel.Error => SerilogLogger.errorLevel!,
                            LogLevel.Warn  => SerilogLogger.warningLevel!,
                            LogLevel.Info  => SerilogLogger.informationLevel!,
                            LogLevel.Trace => SerilogLogger.verboseLevel!,
                            var _          => SerilogLogger.debugLevel!,
                        })! ;
            }
        }

        #endregion

        private static Func <string, object, bool, IDisposable>? pushProperty ;

        public SerilogLogProvider ()
        {
            if (!SerilogLogProvider.IsLoggerAvailable ())
                throw new LoggingException (Resources.SerilogLogProvider_SerilogLogProvider_LogNotFound) ;

            this.getLoggerByNameDelegate    = SerilogLogProvider.GetForContextMethodCall () ;
            SerilogLogProvider.pushProperty = SerilogLogProvider.GetPushProperty () ;

            SerilogLogProvider.ProviderIsAvailableOverride = true ;
        }

        private readonly Func <string, object> getLoggerByNameDelegate ;

        public static bool ProviderIsAvailableOverride { get ; set ; }

        /// <inheritdoc />
        public override Logger GetLogger (string name) => new SerilogLogger (this.getLoggerByNameDelegate (name)).Log ;

        internal static bool IsLoggerAvailable () =>
            SerilogLogProvider.ProviderIsAvailableOverride && (SerilogLogProvider.GetLogManagerType () != null) ;

        /// <inheritdoc />
        protected override OpenNdc GetOpenNdcMethod () => message => SerilogLogProvider.pushProperty! (@"NDC", message, false) ;

        protected override OpenMdc GetOpenMdcMethod () =>
            (key, value, destructure) => SerilogLogProvider.pushProperty! (key, value, destructure) ;

        private static Func <string, object, bool, IDisposable> GetPushProperty ()
        {
            Type? ndcContextType = Reflect.FindType (@"Serilog.Context.LogContext", new[] {@"Serilog", @"Serilog.FullNetFx"}) ;

            if (ndcContextType == null)
                throw new LoggingException (Resources.SerilogLogProvider_GetPushProperty_LogContextNotFound) ;

            MethodInfo? pushPropertyMethod =
                ndcContextType.GetMethod (@"PushProperty", typeof (string), typeof (object), typeof (bool)) ;

            ParameterExpression nameParam              = Expression.Parameter (typeof (string), @"name") ;
            ParameterExpression valueParam             = Expression.Parameter (typeof (object), @"value") ;
            ParameterExpression destructureObjectParam = Expression.Parameter (typeof (bool),   @"destructureObjects") ;

            MethodCallExpression pushPropertyMethodCall =
                Expression.Call (null, pushPropertyMethod!, nameParam, valueParam, destructureObjectParam) ;
            Func <string, object, bool, IDisposable> pushPropertyFunc = Expression
                                                                       .Lambda <Func <string, object, bool, IDisposable>
                                                                        > (pushPropertyMethodCall,
                                                                           nameParam,
                                                                           valueParam,
                                                                           destructureObjectParam)
                                                                       .Compile () ;

            return (key, value, destructure) => pushPropertyFunc (key, value, destructure) ;
        }

        private static Type? GetLogManagerType () => Reflect.FindType (@"Serilog.Log", @"Serilog") ;

        private static Func <string, object> GetForContextMethodCall ()
        {
            Type? logManagerType = SerilogLogProvider.GetLogManagerType () ;

            if (logManagerType == null)
                throw new LoggingException (Resources.SerilogLogProvider_GetForContextMethodCall_LogNotFound) ;

            MethodInfo?         method = logManagerType.GetMethod (@"ForContext", typeof (string), typeof (object), typeof (bool)) ;
            ParameterExpression propertyNameParam = Expression.Parameter (typeof (string), @"propertyName") ;
            ParameterExpression valueParam = Expression.Parameter (typeof (object), @"value") ;
            ParameterExpression destructureObjectsParam = Expression.Parameter (typeof (bool), @"destructureObjects") ;
            MethodCallExpression methodCall = Expression.Call (null,
                                                               method!,
                                                               new Expression[]
                                                               {
                                                                   propertyNameParam, valueParam, destructureObjectsParam
                                                               }) ;
            Func <string, object, bool, object> func = Expression.Lambda <Func <string, object, bool, object>> (
                                                                                                                methodCall,
                                                                                                                propertyNameParam,
                                                                                                                valueParam,
                                                                                                                destructureObjectsParam)
                                                                 .Compile () ;
            return name => func (@"SourceContext", name, false) ;
        }
    }
}
