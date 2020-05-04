#region header

// ArkaneTests - NoFields.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 4:07 PM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.ToString
{
    [TestClass]
    [ToString]
    public class NoFields
    {
        [TestMethod]
        public void TestEmpty () { Assert.AreEqual ("{NoFields}", new NoFields ().ToString ()) ; }
    }
}
