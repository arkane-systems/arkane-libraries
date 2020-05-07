#region header

// ArkaneTests - ReadmeEqualityTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 2:22 PM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [TestClass]
    public class ReadmeEqualityTest
    {
        [TestMethod]
        public void SimpleCase () { Assert.AreEqual (new EnhancedIntegerHolder (2, 2), new EnhancedIntegerHolder (2, 2)) ; }

        [TestMethod]
        public void ControlCase ()
        {
            Assert.AreNotEqual (new NotEnhancedIntegerHolder (2, 2), new NotEnhancedIntegerHolder (2, 2)) ;
        }
    }

    [StructuralEquality (DoNotAddEqualityOperators = true)]
    public class EnhancedIntegerHolder
    {
        public EnhancedIntegerHolder (int x, int y)
        {
            this.X = x ;
            this.Y = y ;
        }

        public int X { get ; }

        public int Y { get ; }
    }

    public class NotEnhancedIntegerHolder
    {
        public NotEnhancedIntegerHolder (int x, int y)
        {
            this.X = x ;
            this.Y = y ;
        }

        public int X { get ; }

        public int Y { get ; }
    }
}
