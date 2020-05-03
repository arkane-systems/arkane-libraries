#region header

// ArkaneTests - EmptyTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 8:22 PM

#endregion

#region using

using ArkaneSystems.Arkane ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core
{
    [TestClass]
    public class EmptyTest
    {
        [TestMethod]
        public void CheckEquality ()
        {
            Assert.AreEqual (default,                              new Empty ()) ;
            Assert.AreEqual (new Empty (),                         new Empty ()) ;
            Assert.AreEqual ((object) new Empty ().GetHashCode (), new Empty ().GetHashCode ()) ;
        }

        [TestMethod]
        public void CheckEqualityOperators ()
        {
// ReSharper disable once EqualExpressionComparison
            Assert.IsTrue (new Empty () == new Empty ()) ;

// ReSharper disable once EqualExpressionComparison
            Assert.IsFalse (new Empty () != new Empty ()) ;

            Assert.IsTrue (new Empty ().Equals (new Empty ())) ;
            Assert.IsFalse (new Empty ().Equals (new object ())) ;
        }
    }
}
