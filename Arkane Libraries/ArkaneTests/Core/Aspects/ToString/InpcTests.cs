using System;
using System.Collections.Generic;
using System.Text;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

using PostSharp.Patterns.Model ;

namespace ArkaneTests.Core.Aspects.ToString
{
    [TestClass]
    public class InpcTests
    {
        [TestMethod]
        public void TestNPC () { Assert.AreEqual ("{DO; One: 1, Three: 3, Two: 2}", new DO ().ToString ()) ; }
    }

    [NotifyPropertyChanged, ToString]
    public class DO
    {
        public int One { get ; set ; } = 1 ;

        public int Two = 2 ;

        public int Three => One + Two ;
    }
}
