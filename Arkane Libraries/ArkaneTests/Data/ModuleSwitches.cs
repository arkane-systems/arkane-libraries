#region header

// ArkaneTests - ModuleSwitches.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 1:08 PM

#endregion

#region using

using System ;
using System.Reflection ;

using ArkaneSystems.Arkane.Data ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Data
{
    [TestClass]
    public class ModuleSwitches
    {
        public ModuleSwitches ()
        {
            // force loading
            Assembly data = Assembly.GetAssembly (typeof (EntityConstraintAttribute))! ;
        }

        [TestMethod]
        public void ArkaneDataPresence ()
        {
            bool received = AppContext.TryGetSwitch ("Switch.ArkaneSystems.Arkane.Data.Presence", out bool switchValue) ;

            Assert.IsTrue (received) ;
            Assert.IsTrue (switchValue) ;
        }
    }
}
