#region header

// ArkaneTests - CollectionHelperTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-07 1:47 AM

#endregion

#region using

using ArkaneSystems.Arkane.Internal ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [TestClass]
    public class CollectionHelperTests
    {
        [TestMethod]
        public void Nulls ()
        {
            Assert.IsTrue (CollectionHelper.Equals (null,        null)) ;
            Assert.IsFalse (CollectionHelper.Equals (new int[0], null)) ;
            Assert.IsFalse (CollectionHelper.Equals (null,       new int[0])) ;
            Assert.IsTrue (CollectionHelper.Equals (new int[0],  new int[0])) ;
        }

        [TestMethod]
        public void ArrayWithNull ()
        {
            Assert.IsFalse (CollectionHelper.Equals (new string[] {null}, new[] {"hello "})) ;
            Assert.IsTrue (CollectionHelper.Equals (new string[] {null},  new string[] {null})) ;
        }

        [TestMethod]
        public void ArrayWithDifferentSizes ()
        {
            Assert.IsFalse (CollectionHelper.Equals (new[] {"hello ", "hi"}, new[] {"hello "})) ;
            Assert.IsFalse (CollectionHelper.Equals (new[] {"hi"},           new[] {"hello"})) ;
        }

        //[TestMethod]
        //public void CollectionUsesCount ()
        //{
        //    var oneCollection = A.Fake <ICollection <string>> () ;
        //    A.CallTo (() => oneCollection.Count).Returns (2) ;
        //    A.CallTo (() => oneCollection.GetEnumerator ()).Throws <Exception> () ;
        //    var str = new string[3] ;
        //    Assert.IsFalse (CollectionHelper.Equals (oneCollection, str)) ;
        //}
    }
}
