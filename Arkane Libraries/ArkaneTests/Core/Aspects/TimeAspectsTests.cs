#region header

// ArkaneTests - TimeAspectsTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 10:26 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Aspects.Contracts ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects
{
    [TestClass]
    public class TimeAspectsTests
    {
        [Future]
        private DateTime future ;

        [Past]
        private DateTime past ;

        [Future (true)]
        private DateTime tomorrow ;

        [Past (true)]
        private DateTime yesterday ;

        [TestMethod]
        public void Past_InPast () { this.past = DateTime.Now.Subtract (new TimeSpan (28, 0, 0, 0)) ; }

        [TestMethod]
        [ExpectedException (typeof (ArgumentException))]
        public void Past_InFuture () { this.past = DateTime.Now.Add (new TimeSpan (28, 0, 0, 0)) ; }

        [TestMethod]
        [ExpectedException (typeof (ArgumentException))]
        public void Future_InPast () { this.future = DateTime.Now.Subtract (new TimeSpan (28, 0, 0, 0)) ; }

        [TestMethod]
        public void Future_InFuture () { this.future = DateTime.Now.Add (new TimeSpan (28, 0, 0, 0)) ; }

        [TestMethod]
        public void Yesterday_Yesterday () { this.yesterday = DateTime.Now.AddDays (-1) ; }

        [TestMethod]
        [ExpectedException (typeof (ArgumentException))]
        public void Yesterday_InRecentPast () { this.yesterday = DateTime.Now.AddSeconds (-1) ; }

        [TestMethod]
        public void Tomorrow_Tomorrow () { this.tomorrow = DateTime.Now.AddDays (1) ; }

        [TestMethod]
        [ExpectedException (typeof (ArgumentException))]
        public void Tomorrow_InRecentFuture () { this.tomorrow = DateTime.Now.AddSeconds (1) ; }
    }
}
