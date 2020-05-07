#region header

// ArkaneTests - InheritanceTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-07 1:43 AM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [TestClass]
    public class InheritanceTests
    {
        [TestMethod]
        public void TestEqualsInheritance ()
        {
            var child1 = new SimpleChild {BaseValue = 1, ChildValue = 2} ;
            var child2 = new SimpleChild {BaseValue = 1, ChildValue = 2} ;
            var child3 = new SimpleChild {BaseValue = 1, ChildValue = 3} ;
            var child4 = new SimpleChild {BaseValue = 2, ChildValue = 2} ;

            Assert.IsTrue (child1.Equals (child2)) ;
            Assert.IsFalse (child1.Equals (child3)) ;
            Assert.IsFalse (child1.Equals (child4)) ;
        }

        [TestMethod]
        public void TestEqualsMultilevelInheritance ()
        {
            var child1 = new GenericMultilevelGrandChild <int> {BaseValue = 1, GrandChildValue = 2} ;
            var child2 = new GenericMultilevelGrandChild <int> {BaseValue = 1, GrandChildValue = 2} ;
            var child3 = new GenericMultilevelGrandChild <int> {BaseValue = 1, GrandChildValue = 3} ;
            var child4 = new GenericMultilevelGrandChild <int> {BaseValue = 2, GrandChildValue = 2} ;

            Assert.IsTrue (child1.Equals (child2)) ;
            Assert.IsFalse (child1.Equals (child3)) ;
            Assert.IsFalse (child1.Equals (child4)) ;
        }
    }

    [StructuralEquality (DoNotAddEqualityOperators = true)]
    public class SimpleBase
    {
        public int BaseValue ;
    }

    [StructuralEquality (DoNotAddEqualityOperators = true)]
    public class SimpleChild : SimpleBase
    {
        public int ChildValue ;
    }

    [StructuralEquality (DoNotAddEqualityOperators = true, TypeCheck = TypeCheck.SameTypeOrSubtype)]
    public class GenericMultilevelBase <T>
    {
        public T BaseValue ;
    }

    public class GenericMultilevelChild <T> : GenericMultilevelBase <T>
    { }

    [StructuralEquality (DoNotAddEqualityOperators = true)]
    public class GenericMultilevelGrandChild <T> : GenericMultilevelChild <T>
    {
        public T GrandChildValue ;
    }
}
