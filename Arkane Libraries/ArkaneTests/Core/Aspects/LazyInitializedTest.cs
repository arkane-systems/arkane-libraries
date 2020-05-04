using System;
using System.Collections.Generic;
using System.Text;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace ArkaneTests.Core.Aspects
{
    [TestClass]
    public class LazyInitializedTest
    {
        private int rightFirst = 2 ;

        [LazyInitialized]
        private int LazyProp => this.rightFirst++ ;

        [TestMethod]
        public void ReturnsCorrectValue ()
        {
            Assert.IsTrue (this.LazyProp == 2) ;
            Assert.IsTrue (this.LazyProp == 2) ;
        }
    }
}
