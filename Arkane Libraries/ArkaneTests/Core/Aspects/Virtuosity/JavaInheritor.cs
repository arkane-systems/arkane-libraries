#region header

// ArkaneTests - JavaInheritor.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:47 AM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

#endregion

namespace ArkaneTests.Core.Aspects.Virtuosity
{
    [Virtual]
    public class JavaInheritor : JavaLikeClass
    {
#pragma warning disable 108,114
        public string Ha () => "Subclass" ;
#pragma warning restore 108,114

        public new string He () => "Subclass" ;
    }
}
