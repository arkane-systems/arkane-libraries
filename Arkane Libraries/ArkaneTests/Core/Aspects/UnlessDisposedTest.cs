#region header

// ArkaneTests - UnlessDisposedTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 10:28 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects
{
    public class ExampleDisposableClass : IDisposable
    {
#pragma warning disable 414
        private bool disposed ;
#pragma warning restore 414

        public void Dispose () { this.disposed = true ; }

        [UnlessDisposed]
        public void DoSomething () { }
    }


    [TestClass]
    public class UnlessDisposed
    {
        private ExampleDisposableClass example ;

        [TestMethod]
        public void UnlessDisposed_BeforeDispose ()
        {
            this.example = new ExampleDisposableClass () ;
            this.example.DoSomething () ;
            this.example.Dispose () ;
        }

        [TestMethod]
        [ExpectedException (typeof (ObjectDisposedException))]
        public void UnlessDisposed_AfterDispose ()
        {
            this.example = new ExampleDisposableClass () ;
            this.example.Dispose () ;
            this.example.DoSomething () ;
        }
    }
}
