#region header

// ArkaneTests - HierarchyTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:50 AM

#endregion

#region using

using System.Reflection ;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.Virtuosity
{
    [TestClass]
    public class HierarchyTests
    {
        #region Nested type: A

        [Virtual]
        public class A
        {
            public virtual int M () => 1 ;
        }

        #endregion

        #region Nested type: B

        [Virtual]
        public class B : A
        {
            public new int M () => 2 ;
        }

        #endregion

        #region Nested type: C

        [Virtual]
        public abstract class C : B
        {
            public new virtual int M () => 3 ;
        }

        #endregion

        #region Nested type: D

        [Virtual]
        public class D : C
        {
            public override int M () => 4 ;
        }

        #endregion

        #region Nested type: E

        [Virtual]
        public class E : D
        {
            public sealed override int M () => 5 ;
        }

        #endregion

        #region Nested type: F

        [Virtual]
        public class F : E
        {
            public new int M () => 6 ;
        }

        #endregion

        #region Nested type: G

        [Virtual]
        public class G : F
        {
            public new int M () => 7 ;
        }

        #endregion

        [TestMethod]
        public void BaseVirtualMethod_WillNotBeChanged ()
        {
            // already virtual: no effect
            MethodInfo? method = typeof (A).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "A.M is Virtual") ;
            Assert.IsTrue ((method.Attributes  & MethodAttributes.NewSlot) != 0, "A.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "A.M is Final") ;
        }

        [TestMethod]
        public void HidingMethod_WillNotBeChanged ()
        {
            // method specifically made new even though it could have been override: no effect
            MethodInfo? method = typeof (B).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "B.M is Virtual") ;
            Assert.IsTrue ((method.Attributes  & MethodAttributes.NewSlot) != 0, "B.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "B.M is Final") ;
        }

        [TestMethod]
        public void NewSlotVirtual_WillNotBeChanged ()
        {
            // method specifically made new: no effect
            MethodInfo? method = typeof (C).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "C.M is Virtual") ;
            Assert.IsTrue ((method.Attributes  & MethodAttributes.NewSlot) != 0, "C.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "C.M is Final") ;
        }

        [TestMethod]
        public void Override_WillNotBeChanged ()
        {
            // method already virtual: no effect
            MethodInfo? method = typeof (D).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "D.M is Virtual") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.NewSlot) != 0, "D.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "D.M is Final") ;
        }

        [TestMethod]
        public void SealedOverride_WillBeUnsealed ()
        {
            // Sealed methods are unsealed, just like in Fody
            MethodInfo? method = typeof (E).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "E.M is Virtual") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.NewSlot) != 0, "E.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "E.M is Final") ;
        }

        [TestMethod]
        public void HidingMethodAfterSealed_WillBeVirtualOnly ()
        {
            // The base is no longer sealed, so we can override it:
            MethodInfo? method = typeof (F).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "F.M is Virtual") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.NewSlot) != 0, "F.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "F.M is Final") ;
        }

        [TestMethod]
        public void OverrideAfterHiding_WillBeVirtual ()
        {
            // Base was affected by us, so we change new to override:
            MethodInfo? method = typeof (G).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "G.M is Virtual") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.NewSlot) != 0, "G.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "G.M is Final") ;
        }

        [TestMethod]
        public void ReturnValues ()
        {
            Assert.AreEqual (1, (new G () as A).M ()) ;
            Assert.AreEqual (2, (new G () as B).M ()) ;
            Assert.AreEqual (7, (new G () as C).M ()) ;
            Assert.AreEqual (7, (new G () as D).M ()) ;
            Assert.AreEqual (7, (new G () as E).M ()) ;
            Assert.AreEqual (7, (new G () as F).M ()) ;
            Assert.AreEqual (7, new G ().M ()) ;
        }
    }
}
