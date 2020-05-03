#region header

// ArkaneTests - Unsealable.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 8:26 PM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects.Constraints ;

#endregion

namespace ArkaneTests.Core.Aspects
{
    [UnsealableConstraint]
    public class Unsealable
    { }

    // Since this will fail to compile if it's working, it'll normally be commented out.
    // Uncomment it and see if the file fails to run the test.

    //public sealed class ShouldNotBeSealed : Unsealable
    //{ }
}
