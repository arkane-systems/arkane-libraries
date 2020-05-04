#region header

// ArkaneTests - UnsubscribeOnFailTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 10:03 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects
{
    [TestClass]
    public class UnsubscribeOnFailTest
    {
        [UnsubscribeOnFail]
        public event EventHandler <EventArgs>? TestEvent ;

        [TestMethod]
        public void CallAndCount ()
        {
            this.TestEvent += this.WorkingHandler ;
            this.TestEvent += this.FailingHandler ;

            try
            {
                this.TestEvent.RaiseEvent (this, new EventArgs ()) ;
            }
            catch (InvalidOperationException)
            {
                // Lose the InvalidOperationException first time around.
            }

            // Should not throw exception, since FailingHandler removed last time around.
            this.TestEvent.RaiseEvent (this, new EventArgs ()) ;
        }

        public void WorkingHandler (object? sender, EventArgs e)
        {
            // Do nothing.
        }

        public void FailingHandler (object? sender, EventArgs e) { throw new InvalidOperationException () ; }
    }
}
