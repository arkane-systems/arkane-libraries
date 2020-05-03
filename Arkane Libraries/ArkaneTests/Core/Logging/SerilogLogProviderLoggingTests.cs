#region header

// ArkaneTests - SerilogLogProviderLoggingTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 12:51 AM

#endregion

#region using

using System ;
using System.Collections.Generic ;
using System.Linq ;

using ArkaneSystems.Arkane.Logging ;
using ArkaneSystems.Arkane.Logging.LogProviders ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

using Serilog ;
using Serilog.Events ;

using Logger = Serilog.Core.Logger ;

#endregion

namespace ArkaneTests.Core.Logging
{
    [TestClass]
    public class SerilogLogProviderLoggingTests
    {
        // Forces provider to be available regardless of how previous tests left it.
        static SerilogLogProviderLoggingTests () => SerilogLogProvider.ProviderIsAvailableOverride = true ;

        public SerilogLogProviderLoggingTests ()
        {
            Logger logger = new LoggerConfiguration ()
                           .Enrich.FromLogContext ()
                           .MinimumLevel.Is (LogEventLevel.Verbose)
                           .WriteTo.Observers (obs => obs.Subscribe (logEvent => this._logEvent = logEvent))
                           .WriteTo.Console ()
                           .CreateLogger () ;

            Log.Logger        = logger ;
            this._logProvider = new SerilogLogProvider () ;
            this._sut         = new LoggerExecutionWrapper (this._logProvider.GetLogger ("Test")) ;
        }

        private readonly IEnumerable <LogLevel> _allLevels = Enum.GetValues (typeof (LogLevel)).Cast <LogLevel> ().ToList () ;

        private readonly IDictionary <LogLevel, Predicate <ILog>> _checkIsEnabledFor = new Dictionary <LogLevel, Predicate <ILog>>
                                                                                       {
                                                                                           {
                                                                                               LogLevel.Trace,
                                                                                               log => log.IsTraceEnabled ()
                                                                                           },
                                                                                           {
                                                                                               LogLevel.Debug,
                                                                                               log => log.IsDebugEnabled ()
                                                                                           },
                                                                                           {
                                                                                               LogLevel.Info,
                                                                                               log => log.IsInfoEnabled ()
                                                                                           },
                                                                                           {
                                                                                               LogLevel.Warn,
                                                                                               log => log.IsWarnEnabled ()
                                                                                           },
                                                                                           {
                                                                                               LogLevel.Error,
                                                                                               log => log.IsErrorEnabled ()
                                                                                           },
                                                                                           {
                                                                                               LogLevel.Fatal,
                                                                                               log => log.IsFatalEnabled ()
                                                                                           },
                                                                                       } ;

        private readonly SerilogLogProvider _logProvider ;
        private readonly ILog               _sut ;

        private readonly Tuple <LogLevel, LogEventLevel>[] matchedLevels =
        {
            new Tuple <LogLevel, LogEventLevel> (LogLevel.Trace,
                                                 LogEventLevel.Verbose),
            new Tuple <LogLevel, LogEventLevel> (LogLevel.Debug, LogEventLevel.Debug),
            new Tuple <LogLevel, LogEventLevel> (LogLevel.Info,  LogEventLevel.Information),
            new Tuple <LogLevel, LogEventLevel> (LogLevel.Warn,  LogEventLevel.Warning),
            new Tuple <LogLevel, LogEventLevel> (LogLevel.Error, LogEventLevel.Error),
            new Tuple <LogLevel, LogEventLevel> (LogLevel.Fatal, LogEventLevel.Fatal)
        } ;

        private LogEvent _logEvent ;

        [TestMethod]
        public void ShouldBeAbleToLogMessage ()
        {
            foreach (var ll in this.matchedLevels)
            {
                this._sut.Log (ll.Item1, () => "m") ;

                Assert.AreEqual (ll.Item2, this._logEvent.Level) ;
                Assert.AreEqual ("m",      this._logEvent.RenderMessage ()) ;
            }
        }

        [TestMethod]
        public void ShouldBeAbleToLogMessageWithParam ()
        {
            foreach (var ll in this.matchedLevels)
            {
                this._sut.Log (ll.Item1, () => "m {0}", null, "param") ;

                Assert.AreEqual (ll.Item2,      this._logEvent.Level) ;
                Assert.AreEqual ("m \"param\"", this._logEvent.RenderMessage ()) ;
            }
        }

        [TestMethod]
        public void ShouldBeAbleToLogMessageAndException ()
        {
            foreach (var ll in this.matchedLevels)
            {
                var exception = new Exception ("e") ;
                this._sut.Log (ll.Item1, () => "m", exception) ;

                Assert.AreEqual (ll.Item2,  this._logEvent.Level) ;
                Assert.AreEqual ("m",       this._logEvent.RenderMessage ()) ;
                Assert.AreEqual (exception, this._logEvent.Exception) ;
            }
        }

        [TestMethod]
        public void CanCheckLogLevelsEnabled ()
        {
            var loglevelEnabledActions = new Action[]
                                         {
                                             () => this._sut.IsTraceEnabled (),
                                             () => this._sut.IsDebugEnabled (),
                                             () => this._sut.IsInfoEnabled (),
                                             () => this._sut.IsWarnEnabled (),
                                             () => this._sut.IsErrorEnabled (),
                                             () => this._sut.IsFatalEnabled (),
                                         } ;

            foreach (var isLogLevelEnabled in loglevelEnabledActions)
                isLogLevelEnabled () ;
        }

        [TestMethod]
        public void CanOpenNestedDiagnosticsContext ()
        {
            using (_logProvider.OpenNestedContext ("context"))
            {
                _sut.Info ("m") ;

                Assert.IsTrue (this._logEvent.Properties.Keys.Contains ("NDC"));
                Assert.AreEqual ("\"context\"", this._logEvent.Properties["NDC"].ToString ()) ;
            }
        }

        [TestMethod]
        public void CanOpenMappedDiagnosticsContext ()
        {
            using (_logProvider.OpenMappedContext ("key", "value"))
            {
                _sut.Info ("m") ;

                Assert.IsTrue (this._logEvent.Properties.Keys.Contains ("key")) ;
                Assert.AreEqual ("\"value\"", this._logEvent.Properties["key"].ToString ()) ;
            }
        }

        [TestMethod]
        public void CanOpenMappedDiagnosticsContextDestructured ()
        {
            var context = new MyMappedContext ();

            using (_logProvider.OpenMappedContext ("key", context, true))
            {
                _sut.Info ("m") ;

                Assert.IsTrue (this._logEvent.Properties.Keys.Contains ("key")) ;
                Assert.AreEqual ("MyMappedContext { ThirtySeven: 37, Name: \"World\", Level: Trace }", this._logEvent.Properties["key"].ToString ()) ;
            }
        }

        [TestMethod]
        public void CanOpenMappedDiagnosticsContextNotDestructured ()
        {
            var context = new MyMappedContext () ;

            using (_logProvider.OpenMappedContext ("key", context, false))
            {
                _sut.Info ("m") ;

                Assert.IsTrue (this._logEvent.Properties.Keys.Contains ("key")) ;
                Assert.AreEqual ("\"World\"",
                                 this._logEvent.Properties["key"].ToString ()) ;
            }
        }

        [TestMethod]
        public void CanLogStructuredMessage ()
        {
            this._sut.Info ("Structured {data} message", "log");

            Assert.AreEqual ("Structured \"log\" message", this._logEvent.RenderMessage ()) ;
            Assert.IsTrue (this._logEvent.Properties.Keys.Contains ("data")) ;
            Assert.AreEqual ("\"log\"",
                             this._logEvent.Properties["data"].ToString ()) ;
        }

        [TestMethod]
        public void CanLogStructuredSerializedMessage ()
        {
            this._sut.Info ("Structured {@data} message", new {Log = "log", Count = "1"}) ;

            Assert.AreEqual ("Structured { Log: \"log\", Count: \"1\" } message", this._logEvent.RenderMessage ()) ;
            Assert.IsTrue (this._logEvent.Properties.Keys.Contains ("data")) ;
        }

        //[Theory]
        //[InlineData (LogEventLevel.Verbose,
        //             new[] {LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal})]
        //[InlineData (LogEventLevel.Debug, new[] {LogLevel.Debug, LogLevel.Info, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal})]
        //[InlineData (LogEventLevel.Information, new[] {LogLevel.Info, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal})]
        //[InlineData (LogEventLevel.Warning, new[] {LogLevel.Warn, LogLevel.Error, LogLevel.Fatal})]
        //[InlineData (LogEventLevel.Error, new[] {LogLevel.Error, LogLevel.Fatal})]
        //[InlineData (LogEventLevel.Fatal, new[] {LogLevel.Fatal})]
        //public void Should_enable_self_and_above_when_setup_with (LogEventLevel minimum, LogLevel[] expectedEnabledLevels)
        //{
        //    AutoRollbackLoggerSetup (minimum,
        //                             log =>
        //                             {
        //                                 foreach (var expectedEnabled in expectedEnabledLevels)
        //                                 {
        //                                     _checkIsEnabledFor[expectedEnabled] (log)
        //                                        .ShouldBeTrue () ;

        //                                     // "loglevel: '{0}' should be enabled when minimum (serilog) level is '{1}'", expectedEnabled, minimum);
        //                                 }

        //                                 foreach (var expectedDisabled in _allLevels.Except (expectedEnabledLevels))
        //                                 {
        //                                     _checkIsEnabledFor[expectedDisabled] (log)
        //                                        .ShouldBeFalse () ;

        //                                     //"loglevel '{0}' should be diabled when minimum (serilog) level is '{1}'", expectedDisabled, minimum);
        //                                 }
        //                             }) ;
        //}

        //private static void AutoRollbackLoggerSetup (LogEventLevel minimumLevel, Action <ILog> @do)
        //{
        //    var originalLogger = Log.Logger ;
        //    try
        //    {
        //        Log.Logger = new LoggerConfiguration ()
        //                    .WriteTo.Console ()
        //                    .MinimumLevel.Is (minimumLevel)
        //                    .CreateLogger () ;

        //        @do (new LoggerExecutionWrapper (new SerilogLogProvider ().GetLogger ("Test"))) ;
        //    }
        //    finally
        //    {
        //        Log.Logger = originalLogger ;
        //    }
        //}
    }
}
