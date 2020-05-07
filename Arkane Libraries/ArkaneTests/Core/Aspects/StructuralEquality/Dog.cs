#region header

// ArkaneTests - Dog.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 2:03 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [StructuralEquality]
    internal class Dog
    {
        public string? Name { get ; set ; }

        public int Age { get ; set ; }

        public static bool operator == (Dog left, Dog right) => EqualityOperator.Weave (left, right) ;
        public static bool operator != (Dog left, Dog right) => EqualityOperator.Weave (left, right) ;
    }

    [TestClass]
    public class DogTest
    {
        [TestMethod]
        public void TwoDogs ()
        {
            // This is here to avoid optimization, always returns "do".
            string d = new Random ().Next (0, 1) == 0 ? "do" : "" ;
            string f = "Fi" + d ;
            Assert.AreEqual (new Dog {Name    = "Fido"}, new Dog {Name = f}) ;
            Assert.AreNotEqual (new Dog {Name = "Azor"}, new Dog {Name = "Fido"}) ;
        }
    }
}
