using System ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace ArkaneTests.Core
{
    [TestClass]
    public class ModuleSwitches
    {
        public ModuleSwitches()
        {
            Console.WriteLine($@"force loading: {ArkaneSystems.Arkane.Math.Constants.π}");
        }

        [TestMethod]
        public void ArkaneCorePresence()
        {
            bool received = AppContext.TryGetSwitch("Switch.ArkaneSystems.Arkane.Core.Presence", out var switchValue);

            Assert.IsTrue(received);
            Assert.IsTrue(switchValue);
        }

        [TestMethod]
        public void ArkaneFishAbsence()
        {
            bool received = AppContext.TryGetSwitch ("Switch.ArkaneSystems.Arkane.Fish.Presence", out var switchValue);

            Assert.IsFalse(received);
            Assert.IsFalse(switchValue);
        }
    }
}
