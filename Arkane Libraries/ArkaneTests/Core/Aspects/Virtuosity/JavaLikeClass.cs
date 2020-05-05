#region header

// ArkaneTests - JavaLikeClass.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:46 AM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

#endregion

namespace ArkaneTests.Core.Aspects.Virtuosity
{
    [Virtual]
    public class JavaLikeClass
    {
        public virtual int A { get ; set ; }

        public int B { get ; set ; }

        public virtual void Hello () { }

        public string Ha () => "BaseClass" ;

        public string He () => "BaseClass" ;

        private void Noro () { }
    }
}
