#region header

// ArkaneTests - LogProviderTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 8:52 PM

#endregion

#region using

using ArkaneSystems.Arkane.Logging ;
using ArkaneSystems.Arkane.Logging.LogProviders ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Logging
{
    [TestClass]
    public class LogProviderTests
    {
        [TestMethod]
        public void WhenSerilogIsAvailableShouldGetSerilog()
        {
            LogProvider.SetCurrentLogProvider(null);
            SerilogLogProvider.ProviderIsAvailableOverride = true ;
            ILogProvider? logProvider = LogProvider.ForceResolveLogProvider();

            Assert.IsInstanceOfType(logProvider, typeof(SerilogLogProvider));
        }

        [TestMethod]
        public void WhenNoLogProviderAvailableShouldGetNoOpLogger ()
        {
            LogProvider.SetCurrentLogProvider (null) ;
            SerilogLogProvider.ProviderIsAvailableOverride = false ;

            ILog logger = LogProvider.For <LogProviderTests> () ;

            Assert.IsInstanceOfType (logger, typeof (NoOpLogger)) ;
        }
    }
}
