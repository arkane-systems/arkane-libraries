#region header

// ArkaneTests - WrapExceptionTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 10:30 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects
{
    [TestClass]
    public class WrapExceptionTest
    {
        [WrapException (typeof (InvalidOperationException), typeof (ApplicationException), "User-friendly error message.")]
        private void InnerMethod () { throw new InvalidOperationException ("Internal-only message.") ; }

        [TestMethod]
        [ExpectedException (typeof (ApplicationException))]
        public void IsWrappedSuccessfully () { this.InnerMethod () ; }
    }
}
