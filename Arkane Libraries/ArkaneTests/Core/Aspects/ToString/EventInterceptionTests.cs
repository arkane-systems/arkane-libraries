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
    public class EventInterceptionTests
    {
        [TestMethod]
        public void TestIgnoreEvents () { Assert.AreEqual ("{EventInterceptionTests}", new EventInterceptionTests ().ToString ()) ; }

        public event Action FieldLikeEvent ;

        [EIA]
        public event Action EnhancedFieldLikeEvent ;
    }

    [Serializable]
    public class EIA : EventInterceptionAspect
    {
        public override void OnAddHandler (EventInterceptionArgs args) { base.OnAddHandler (args) ; }
    }
}
