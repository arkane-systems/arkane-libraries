using System;
using System.Collections.Generic;
using System.Text;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace ArkaneTests.Core.Aspects.ToString
{
    [TestClass]
    [ToString]
    public class GenericsInFields : MidField <float>
    {
        [TestMethod]
        public void TestItWorks ()
        {
            var f = new GenericsInFields () ;
            Assert.AreEqual ("{GenericsInFields; Y: null, X: null}", f.ToString ()) ;
        }
    }

    public class MidField <K> : LowField <List <K>>
    {
        public Tuple <K, K> Y ;
    }

    public class LowField <T>
    {
        public T X ;
    }
}
