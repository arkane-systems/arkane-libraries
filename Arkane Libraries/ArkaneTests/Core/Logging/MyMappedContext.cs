#region header

// ArkaneTests - MyMappedContext.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 1:31 AM

#endregion

#region using

using ArkaneSystems.Arkane.Logging ;

#endregion

namespace ArkaneTests.Core.Logging
{
    internal class MyMappedContext
    {
        public int ThirtySeven => 37 ;

        public string Name { get ; set ; } = "World" ;

        public LogLevel Level { get ; set ; } = LogLevel.Trace ;

        public override string ToString () => this.Name ;
    }
}
