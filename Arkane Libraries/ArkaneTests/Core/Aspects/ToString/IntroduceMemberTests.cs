#region header

// ArkaneTests - IntroduceMemberTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 4:22 PM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Advices ;

#endregion

namespace ArkaneTests.Core.Aspects.ToString
{
    [TestClass]
    public class IntroduceMemberTests
    {
        [TestMethod]
        public void TestIntroduceProperty () { Assert.AreEqual ("{TargetClass; One: 1}", new TargetClass ().ToString ()) ; }
    }

    [ToString]
    [AspectClass]
    public class TargetClass
    { }

    [Serializable]
    public class AspectClass : InstanceLevelAspect
    {
        [IntroduceMember]
        public int One => 1 ;
    }
}
