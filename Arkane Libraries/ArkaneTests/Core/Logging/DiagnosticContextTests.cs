using System;
using System.Collections.Generic;
using System.Text;

using ArkaneSystems.Arkane.Logging ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace ArkaneTests.Core.Logging
{
    [TestClass]
    public class DiagnosticContextTests
    {
        [TestMethod]
        public void CanOpenNestedDiagnosticContext ()
        {
            using (LogProvider.OpenNestedContext ("a"))
            { }
        }

        [TestMethod]
        public void CanOpenMappedDiagnosticContext ()
        {
            using (LogProvider.OpenMappedContext ("a", "b"))
            { }
        }
    }
}
