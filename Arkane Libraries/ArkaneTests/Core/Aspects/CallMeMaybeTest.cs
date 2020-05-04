using System;
using System.Collections.Generic;
using System.Text;

using ArkaneSystems.Arkane.Aspects.Diagnostics ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace ArkaneTests.Core.Aspects
{
    [TestClass]
    public class CallMeMaybe
    {
        [CallMeMaybe (1.0)]
        private bool DoItOne () => true ;

        [TestMethod]
        public void TestWithCertainty () { Assert.IsTrue (this.DoItOne ()) ; }

        [CallMeMaybe (0.0)]
        private bool DoItTwo () => true ;

        [ExpectedException (typeof (InvalidOperationException))]
        [TestMethod]
        public void TestWithUncertainty () { this.DoItTwo () ; }

        [CallMeMaybe (0.0, typeof (ApplicationException))]
        public bool DoItThree () => true ;

        [ExpectedException (typeof (ApplicationException))]
        [TestMethod]
        public void TestWithCustomException () { this.DoItThree () ; }
    }
}
