#region header

// ArkaneTests - VirtuosityTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:44 AM

#endregion

#region using

using System.Reflection ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.Virtuosity
{
    [TestClass]
    public class VirtuosityTests
    {
        [TestMethod]
        public void Reflection ()
        {
            Assert.IsTrue (typeof (JavaLikeClass).GetMethod ("Ha").IsVirtual) ;
            foreach (MethodInfo methodInfo in typeof (JavaLikeClass).GetRuntimeMethods ())
            {
                Assert.IsTrue (methodInfo.IsVirtual ||
                             methodInfo.IsPrivate ||
                             (methodInfo.DeclaringType != typeof (JavaLikeClass)),
                             methodInfo.ToString ()) ;
            }
        }

        [TestMethod]
        public void CallsAreVirtual ()
        {
            JavaLikeClass instance = new JavaInheritor () ;
            Assert.AreEqual ("Subclass", instance.Ha ()) ;
            Assert.AreEqual ("Subclass", instance.He ()) ;
        }
    }
}
