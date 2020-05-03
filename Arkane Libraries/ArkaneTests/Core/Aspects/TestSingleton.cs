#region header

// ArkaneTests - TestSingleton.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 8:26 PM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Aspects.Constraints ;

#endregion

namespace ArkaneTests.Core.Aspects
{
    [SingletonConstraint]
    public class TestSingleton
    {
        private static readonly Lazy <TestSingleton> Lazy = new Lazy <TestSingleton> (() => new TestSingleton ()) ;

        private TestSingleton () { }

        public static TestSingleton Instance => TestSingleton.Lazy.Value ;
    }
}
