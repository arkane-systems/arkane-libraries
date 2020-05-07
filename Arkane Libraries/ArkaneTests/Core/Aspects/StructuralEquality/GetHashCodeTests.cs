#region header

// ArkaneTests - GetHashCodeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-07 1:58 AM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [TestClass]
    public class SpecialCasesTests
    {
        #region Nested type: HashCodeAlreadyImplemented

        [StructuralEquality (DoNotAddEqualityOperators = true)]
        private class HashCodeAlreadyImplemented
        {
            private int aField = 21 ;

            public override int GetHashCode () => 42 ;
        }

        #endregion

        #region Nested type: Multi

        [StructuralEquality (DoNotAddEqualityOperators = true)]
        public class Multi
        {
            public int H { get ; } = 2 ;

            [AdditionalGetHashCodeMethod]
            public int Oi () => 3 ;

            [AdditionalGetHashCodeMethod]
            public int Uk () => 5 ;
        }

        #endregion

        [TestMethod]
        public void TestDoNotOverwrite () { Assert.AreEqual (42, new HashCodeAlreadyImplemented ().GetHashCode ()) ; }

        [TestMethod]
        public void MultipleCustom ()
        {
            Multi one = new Multi () ;
            Multi two = new Multi () ;
            Assert.AreEqual (one.GetHashCode (), two.GetHashCode ()) ;
            Assert.AreNotEqual (0, one.GetHashCode ()) ;
            Assert.AreNotEqual (2, one.GetHashCode ()) ;
        }
    }
}
