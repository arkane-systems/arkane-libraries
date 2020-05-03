#region header

// Arkane.Base.Tests - TypeExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2013.  All rights reserved.
// 
// Created: 2013-09-14 2:52 PM

#endregion

using System ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace ArkaneTests.Core.Extensions
{
    [TestClass]
    public class TypeExtensions
    {
        [TestMethod]
        public void IsNullableType_True ()
        {
            Assert.IsTrue (typeof (int?).IsNullableType ());
        }

        [TestMethod]
        public void IsNullableType_False ()
        {
            Assert.IsFalse (typeof (int).IsNullableType ());
        }
    }
}
