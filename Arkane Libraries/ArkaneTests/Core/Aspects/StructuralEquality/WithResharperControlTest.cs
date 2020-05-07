#region header

// ArkaneTests - WithResharperControlTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 2:07 AM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [TestClass]
    public class WithReSharperControlTest
    {
        [TestMethod]
        public void EnhancedCase ()
        {
            EnhancedCase a = new EnhancedCase {a = 2, B = "Hello", C = SomeType.Instance} ;
            EnhancedCase b = new EnhancedCase {a = 2, B = "Hello", C = SomeType.Instance} ;
            Assert.AreEqual (a, b) ;
        }

        [TestMethod]
        public void ControlCase ()
        {
            ControlCase a = new ControlCase {a = 2, B = "Hello", C = SomeType.Instance} ;
            ControlCase b = new ControlCase {a = 2, B = "Hello", C = SomeType.Instance} ;
            Assert.AreNotEqual (a, b) ;
        }

        [TestMethod]
        public void ResharperCase ()
        {
            ReSharperCreated a = new ReSharperCreated {a = 2, B = "Hello", C = SomeType.Instance} ;
            ReSharperCreated b = new ReSharperCreated {a = 2, B = "Hello", C = SomeType.Instance} ;
            Assert.AreEqual (a, b) ;
        }
    }

    [StructuralEquality (DoNotAddEqualityOperators = true)]
    public class EnhancedCase
    {
        public int a ;

        public string B { get ; set ; }

        public SomeType C { get ; set ; }
    }

    public class ControlCase
    {
        public int a ;

        public string B { get ; set ; }

        public SomeType C { get ; set ; }
    }

    public class SomeType
    {
        public static SomeType Instance { get ; } = new SomeType () ;
    }

    public class ReSharperCreated
    {
        public int a ;

        public string B { get ; set ; }

        public SomeType C { get ; set ; }

        protected bool Equals (ReSharperCreated other) =>
            (this.a == other.a) && (this.B == other.B) && object.Equals (this.C, other.C) ;

        public override bool Equals (object obj)
        {
            if (object.ReferenceEquals (null, obj))
                return false ;
            if (object.ReferenceEquals (this, obj))
                return true ;
            if (obj.GetType () != this.GetType ())
                return false ;

            return this.Equals ((ReSharperCreated) obj) ;
        }

        public override int GetHashCode ()
        {
            unchecked
            {
                int hashCode = 0 ^ this.a ;
                hashCode = (hashCode * 397) ^ (this.B != null ? this.B.GetHashCode () : 0) ;
                hashCode = (hashCode * 397) ^ (this.C != null ? this.C.GetHashCode () : 0) ;
                return hashCode ;
            }
        }
    }
}
