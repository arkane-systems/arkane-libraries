#region header

// ArkaneTests - PropertiesTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 2:29 PM

#endregion

#region using

using System.Runtime.CompilerServices ;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [TestClass]
    public class PropertiesTest
    {
        [TestMethod]
        public void TestDoNotAddEquals ()
        {
            PAA paa = new PAA {Property = 2} ;
            PAA b   = new PAA {Property = 2} ;
            Assert.IsFalse (paa.Equals (b)) ;
            Assert.AreEqual (paa.GetHashCode (), b.GetHashCode ()) ;
        }

        [TestMethod]
        public void TestDoNotAddHashCode ()
        {
            PBB a   = new PBB {Property = 2} ;
            PBB pbb = new PBB {Property = 2} ;
            Assert.IsTrue (a.Equals (pbb)) ;
            Assert.AreEqual (a.GetHashCode (), RuntimeHelpers.GetHashCode (a)) ;
        }
    }

    [StructuralEquality (DoNotAddEquals = true, DoNotAddEqualityOperators = true)]
    public class PAA
    {
        public int Property { get ; set ; }
    }

    [StructuralEquality (DoNotAddGetHashCode = true, DoNotAddEqualityOperators = true)]
    public class PBB
    {
        public int Property { get ; set ; }
    }
}
