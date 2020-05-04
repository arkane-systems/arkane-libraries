#region header

// ArkaneTests - ObjectExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 9:58 AM

#endregion

#region using

using System ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Extensions
{
    [TestClass]
    public class ObjectExtTest
    {
        private object ReturnAnonymous () => new {Test = "Monkey Hat Fish!"} ;

        [TestMethod]
        public void Object_CastByExample ()
        {
            // Get instance of anonymous type.
            object o = this.ReturnAnonymous () ;

            // Cast it.
            var typed = o.CastByExample (new {Test = ""}) ;

            Assert.AreEqual (typed.Test, "Monkey Hat Fish!", false) ;
        }

        #region EqualsAnyOf

        [TestMethod]
        public void Object_EqualsAnyOfIntSucceeeds () { Assert.IsTrue (2.EqualsAnyOf (2, 4, 6)) ; }

        [TestMethod]
        public void Object_EqualsAnyOfIntFails () { Assert.IsFalse (3.EqualsAnyOf (2, 4, 6)) ; }

        [TestMethod]
        public void Object_EqualsAnyOfStringSucceeds () { Assert.IsTrue ("foo".EqualsAnyOf ("foo", "bar", "baz")) ; }

        [TestMethod]
        public void Object_EqualsAnyOfStringFails () { Assert.IsFalse ("quux".EqualsAnyOf ("foo", "bar", "baz")) ; }

        #endregion

        #region EqualsNoneOf

        [TestMethod]
        public void Object_EqualsNoneOfIntSucceeds () { Assert.IsTrue (3.EqualsNoneOf (2, 4, 6)) ; }

        [TestMethod]
        public void Object_EqualsNoneOfIntFails () { Assert.IsFalse (2.EqualsNoneOf (2, 4, 6)) ; }

        [TestMethod]
        public void Object_EqualsNoneOfStringSucceeds () { Assert.IsTrue ("quux".EqualsNoneOf ("foo", "bar", "baz")) ; }

        [TestMethod]
        public void Object_EqualsNoneOfStringFails () { Assert.IsFalse ("foo".EqualsNoneOf ("foo", "bar", "baz")) ; }

        #endregion

        #region ThrowIfNull

        [TestMethod]
        [ExpectedException (typeof (ArgumentNullException))]
        public void Object_ThrowIfNullSucceeds ()
        {
            object? sot = null ;

            // ReSharper disable once ExpressionIsAlwaysNull
            sot.ThrowIfNull () ;
        }

        [TestMethod]
        public void Object_ThrowIfNullFails ()
        {
            var sot = new object () ;

            sot.ThrowIfNull () ;
        }

        #endregion
    }
}
