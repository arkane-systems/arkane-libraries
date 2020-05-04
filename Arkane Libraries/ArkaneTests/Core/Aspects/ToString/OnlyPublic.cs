#region header

// ArkaneTests - OnlyPublic.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 4:01 PM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

using PostSharp.Extensibility ;

#endregion

namespace ArkaneTests.Core.Aspects.ToString
{
    [TestClass]
    [ToString (PropertyNameToValueSeparator                = ":")]
    [IgnoreDuringToString (AttributeTargetMemberAttributes = MulticastAttributes.Private)]
    public class OnlyPublic
    {
        public  int X = 2 ;
#pragma warning disable 414
        private int Y = 2 ;
#pragma warning restore 414

        [TestMethod]
        public void TestSelf ()
        {
            OnlyPublic o = new OnlyPublic () ;
            Assert.AreEqual ("{OnlyPublic; X:2}", o.ToString ()) ;
        }
    }
}
