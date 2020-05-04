using System;
using System.Collections.Generic;
using System.Text;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace ArkaneTests.Core.Aspects
{
    [TestClass]
    public class RetryTest
    {
        private bool isRetry ;

        [TestMethod]
        [Retry]
        public void WorkFirstTime () { }

        [TestMethod]
        [Retry]
        public void FailFirstTime ()
        {
            if (!this.isRetry)
            {
                this.isRetry = true ;
                throw new ApplicationException () ;
            }
        }

        [TestMethod]
        [ExpectedException (typeof (ApplicationException))]
        [Retry]
        public void FailEveryTime () { throw new ApplicationException () ; }
    }
}
