#region header

// ArkaneTests - ByDefaultHidden.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 4:14 PM

#endregion

#region using

using System.Runtime.CompilerServices ;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.ToString
{
    [TestClass]
    public class ByDefaultHiddenTests
    {
        [TestMethod]
        public void TestHide () { Assert.AreEqual ("{HideThem; Three: 3, Two: 2}", new HideThem ().ToString ()) ; }

        [TestMethod]
        public void TestShow () { Assert.AreEqual ("{ShowThem; One: 1, Three: 3, Two: 2}", new ShowThem ().ToString ()) ; }
    }

    [ToString]
    public class HideThem
    {
        private int One = 1 ;

        public int Three = 3 ;

        [CompilerGenerated]
        public int Two = 2 ;
    }

    [ToString (IncludePrivate = true)]
    public class ShowThem
    {
        private int One = 1 ;

        public int Three = 3 ;

        [CompilerGenerated]
        public int Two = 2 ;
    }
}
