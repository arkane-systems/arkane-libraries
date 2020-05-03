#region header

// ArkaneTests - NonNullableTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 8:14 PM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

// testing obsolete class
#pragma warning disable 618

namespace ArkaneTests.Core
{
    [TestClass]
    public class NonNullableTest
    {
        [TestMethod]
        public void NonNullConstructionAndConversion ()
        {
            var x       = new string ('x', 1) ;
            var subject = new NonNullable <string> (x) ;
            Assert.AreSame (x, (string) subject) ;
            Assert.AreSame (x, subject.Value) ;
        }

        [TestMethod]
        public void NonNullConversionFromAndTo ()
        {
            var                  x       = new string ('x', 1) ;
            NonNullable <string> subject = x ;
            Assert.AreSame (x, (string) subject) ;
            Assert.AreSame (x, subject.Value) ;
        }

        [TestMethod]
        [ExpectedException (typeof (ArgumentNullException))]
        public void NullConstruction ()
        {
            var _ = new NonNullable <string> (null!) ;
            Assert.Fail ("Expected exception") ;
        }

        [TestMethod]
        [ExpectedException (typeof (ArgumentNullException))]
        public void NullConversion ()
        {
            string x = null! ;

            NonNullable <string> y = x ;
            Assert.Fail ("Expected exception:" + y) ;
        }

        [TestMethod]
        [ExpectedException (typeof (NullReferenceException))]
        public void DefaultConstructionAndValueProperty ()
        {
            var x = new NonNullable <string> () ;

            string y = x.Value ;
            Assert.Fail ("Expected exception:" + y) ;
        }

        [TestMethod]
        [ExpectedException (typeof (NullReferenceException))]
        public void DefaultConstructionAndImplicitConversion ()
        {
            var x = new NonNullable <string> () ;

            string y = x ;
            Assert.Fail ("Expected exception:" + y) ;
        }

        [TestMethod]
        public void EqualityOperator ()
        {
            var x = new string ('1', 10) ;
            var y = new string ('1', 10) ;
            var z = new string ('z', 10) ;

            NonNullable <string> xx = x ;
            NonNullable <string> yy = y ;
            NonNullable <string> zz = z ;
            var                  nn = new NonNullable <string> () ;

#pragma warning disable 1718
            Assert.IsTrue (xx == xx) ;
            Assert.IsTrue (yy == yy) ;
            Assert.IsTrue (zz == zz) ;
            Assert.IsTrue (nn == nn) ;
#pragma warning restore 1718

            Assert.IsFalse (xx == yy) ;
            Assert.IsFalse (yy == zz) ;
            Assert.IsFalse (zz == nn) ;
            Assert.IsFalse (nn == xx) ;
        }

        [TestMethod]
        public void InequalityOperator ()
        {
            var x = new string ('1', 10) ;
            var y = new string ('1', 10) ;
            var z = new string ('z', 10) ;

            NonNullable <string> xx = x ;
            NonNullable <string> yy = y ;
            NonNullable <string> zz = z ;
            var                  nn = new NonNullable <string> () ;

#pragma warning disable 1718
            Assert.IsFalse (xx != xx) ;
            Assert.IsFalse (yy != yy) ;
            Assert.IsFalse (zz != zz) ;
            Assert.IsFalse (nn != nn) ;
#pragma warning restore 1718

            Assert.IsTrue (xx != yy) ;
            Assert.IsTrue (yy != zz) ;
            Assert.IsTrue (zz != nn) ;
            Assert.IsTrue (nn != xx) ;
        }

        [TestMethod]
        public void EqualityMethod ()
        {
            var x = new string ('1', 10) ;
            var y = new string ('1', 10) ;
            var z = new string ('z', 10) ;

            NonNullable <string> xx = x ;
            NonNullable <string> yy = y ;
            NonNullable <string> zz = z ;
            var                  nn = new NonNullable <string> () ;

            Assert.IsTrue (xx.Equals (xx)) ;
            Assert.IsTrue (xx.Equals (yy)) ;
            Assert.IsFalse (xx.Equals (zz)) ;
            Assert.IsFalse (xx.Equals (nn)) ;
            Assert.IsFalse (nn.Equals (xx)) ;
        }

        [TestMethod]
        public void TestGetHashCode ()
        {
            Assert.AreEqual ("hi".GetHashCode (), new NonNullable <string> ("hi").GetHashCode ()) ;
            Assert.AreEqual (0,                   new NonNullable <string> ().GetHashCode ()) ;
        }

        [TestMethod]
        public void TestToString ()
        {
            Assert.AreEqual ("hi", new NonNullable <string> ("hi").ToString ()) ;
            Assert.AreEqual ("",   new NonNullable <string> ().ToString ()) ;
        }

        [TestMethod]
        public void BoxingIsUnpleasant ()
        {
            NonNullable <string> x = "hi" ;
            object               y = x ;
            Assert.IsFalse (y is string) ;
        }
    }
}
