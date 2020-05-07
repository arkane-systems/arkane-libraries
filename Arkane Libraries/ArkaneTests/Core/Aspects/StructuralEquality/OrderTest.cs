#region header

// ArkaneTests - OrderTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-07 12:04 AM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void TestOrder ()
        {
            // tests that types are processed in inheritance order
            Assert.AreEqual (new A_New {b    = 3}, new A_New {b = 3}) ;
            Assert.AreNotEqual (new A_New {b = 3}, new A_New {b = 4}) ;
        }
    }

    [StructuralEquality (DoNotAddEqualityOperators = true)]
    public class A_New : B_Old
    { }

    [StructuralEquality (DoNotAddEqualityOperators = true)]
    public class B_Old
    {
        public int b ;
    }
}
