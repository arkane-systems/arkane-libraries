using System;
using System.Collections.Generic;
using System.Text;

using ArkaneSystems.Arkane.Annotations ;
using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace ArkaneTests.Core.Aspects
{
    [TestClass]
    public class OnlyOnPlatformTests
    {
        [OnlyOnPlatform ("Windows")]
        private void OnWindows (ref int a) { a++ ; }

        [OnlyOnPlatform ("Linux")]
        private void OnLinux (ref int a) { a++ ; }

        [OnlyOnPlatform ("Windows", "Linux")]
        private void OnWindowsOrLinux (ref int a) { a++ ; }

        [OnlyOnPlatform ("Linux", ThrowIfIncompatible = true)]
        private void OnLinuxBoom (ref int a) { a++ ; }

        [TestMethod]
        [Information ("This test depends on the test run being performed under Windows.")]
        public void WorksOnWindows ()
        {
            var a = 0 ;
            this.OnWindows (ref a) ;

            Assert.AreEqual (1, a) ;
        }

        [TestMethod]
        [Information ("This test depends on the test run being performed under Windows.")]
        public void WorksOnLinux ()
        {
            var a = 0 ;
            this.OnLinux (ref a) ;

            Assert.AreEqual (0, a) ;
        }

        [TestMethod]
        [Information ("This test depends on the test run being performed under Windows.")]
        public void WorksOnWindowsOrLinux ()
        {
            var a = 0 ;
            this.OnWindowsOrLinux (ref a) ;

            Assert.AreEqual (1, a) ;
        }

        [TestMethod]
        [Information ("This test depends on the test run being performed under Windows.")]
        [ExpectedException (typeof (PlatformNotSupportedException))]
        public void WorksOnLinuxException ()
        {
            var a = 0 ;
            this.OnLinuxBoom (ref a) ;
        }
    }
}
