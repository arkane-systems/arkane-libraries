#region header

// ArkaneTests - LoggerExecutionWrapperTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 8:52 PM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Logging ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Logging
{
    public class FakeLogger : ILog
    {
        public LogLevel LogLevel { get ; private set ; }

        public string? Message { get ; private set ; }

        public Exception? Exception { get ; private set ; }

        public bool Log (LogLevel logLevel, Func <string>? messageFunc, Exception? exception, params object[] formatParameters)
        {
            string? message = messageFunc?.Invoke () ;

            if (message != null)
            {
                this.LogLevel  = logLevel ;
                this.Message   = messageFunc! () ?? this.Message ;
                this.Exception = exception ;
            }

            return true ;
        }
    }

    [TestClass]
    public class LoggerExecutionWrapperTests
    {
        public LoggerExecutionWrapperTests ()
        {
            this._fakeLogger = new FakeLogger () ;
            this._sut        = new LoggerExecutionWrapper (this._fakeLogger.Log) ;
        }

        private readonly FakeLogger             _fakeLogger ;
        private readonly LoggerExecutionWrapper _sut ;

        [TestMethod]
        public void LogExceptionWhenLoggingAndMessageFactoryThrows ()
        {
            var loggingException = new Exception ("Message") ;
            this._sut.Log (LogLevel.Info, () => { throw loggingException ; }) ;

            Assert.AreEqual (loggingException,                                  this._fakeLogger.Exception) ;
            Assert.AreEqual (LoggerExecutionWrapper.FailedToGenerateLogMessage, this._fakeLogger.Message) ;
        }

        [TestMethod]
        public void LogExceptionWhenLoggingWithExceptionAndMessageFactoryThrows ()
        {
            var appException     = new Exception ("Message") ;
            var loggingException = new Exception ("Message") ;
            this._sut.Log (LogLevel.Info, () => { throw loggingException ; }, appException) ;

            Assert.AreEqual (loggingException,                                  this._fakeLogger.Exception) ;
            Assert.AreEqual (LoggerExecutionWrapper.FailedToGenerateLogMessage, this._fakeLogger.Message) ;
        }

        [TestMethod]
        public void DoNotWrapMessageWhenAskingIfLogLevelIsEnabled ()
        {
            this._sut.IsDebugEnabled () ;

            Assert.AreNotEqual (LoggerExecutionWrapper.FailedToGenerateLogMessage, this._fakeLogger.Message) ;
        }
    }
}
