#region header

// ArkaneTests - PostCompile.cs
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

using ArkaneSystems.Arkane.Aspects.PostCompile ;

#endregion

namespace ArkaneTests.Core.Aspects
{
    public class PostCompile
    {
        [PostCompile]
        private static void RunAtCompileTime () { Console.Beep () ; }

        public void Monkey () { PostCompile.RunAtCompileTime () ; }
    }
}
