using System;
using System.Collections.Generic;
using System.Text;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

using PostSharp.Aspects ;

namespace ArkaneTests.Core.Aspects.ToString
{
    [TestClass]
    [ToString]
    public class FieldPromotionTests
    {
        [TestMethod]
        public void TestSelf () { Assert.AreEqual ("{FieldPromotionTests; Field: 1}", new FieldPromotionTests ().ToString ()) ; }

        [LIA]
        public int Field ;
    }

    [Serializable]
    public class LIA : LocationInterceptionAspect
    {
        public override void OnGetValue (LocationInterceptionArgs args) { args.Value = 1 ; }
    }
}
