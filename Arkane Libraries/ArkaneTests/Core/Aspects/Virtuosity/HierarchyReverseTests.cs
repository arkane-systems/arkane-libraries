#region header

// ArkaneTests - HierarchyReverseTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:54 AM

#endregion

#region using

using System.Reflection ;

using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.Virtuosity
{
    [TestClass]
    public class HierarchyReverseTests
    {
        #region Nested type: A

        [Virtual]
        public class A : B
        {
            public new int M () => 7 ;
        }

        #endregion

        #region Nested type: B

        [Virtual]
        public class B : C
        {
            public new int M () => 6 ;
        }

        #endregion

        #region Nested type: C

        [Virtual]
        public class C : D
        {
            public sealed override int M () => 5 ;
        }

        #endregion

        #region Nested type: D

        [Virtual]
        public class D : E
        {
            public override int M () => 4 ;
        }

        #endregion

        #region Nested type: E

        [Virtual]
        public abstract class E : F
        {
            public new virtual int M () => 3 ;
        }

        #endregion

        #region Nested type: F

        [Virtual]
        public class F : G
        {
            public new int M () => 2 ;
        }

        #endregion

        #region Nested type: G

        [Virtual]
        public class G
        {
            public virtual int M () => 1 ;
        }

        #endregion

        [TestMethod]
        public void BaseVirtualMethod_WillNotBeChanged ()
        {
            MethodInfo? method = typeof (G).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "G.M is Virtual") ;
            Assert.IsTrue ((method.Attributes  & MethodAttributes.NewSlot) != 0, "G.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "G.M is Final") ;
        }

        [TestMethod]
        public void HidingMethod_WillNotBeChanged ()
        {
            MethodInfo? method = typeof (F).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "F.M is Virtual") ;
            Assert.IsTrue ((method.Attributes  & MethodAttributes.NewSlot) != 0, "F.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "F.M is Final") ;
        }

        [TestMethod]
        public void NewSlotVirtual_WillNotBeChanged ()
        {
            MethodInfo? method = typeof (E).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "E.M is Virtual") ;
            Assert.IsTrue ((method.Attributes  & MethodAttributes.NewSlot) != 0, "E.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "E.M is Final") ;
        }

        [TestMethod]
        public void Override_WillNotBeChanged ()
        {
            MethodInfo? method = typeof (D).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "D.M is Virtual") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.NewSlot) != 0, "D.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "D.M is Final") ;
        }

        [TestMethod]
        public void SealedOverride_WillBeUnsealed ()
        {
            MethodInfo? method = typeof (C).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "C.M is Virtual") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.NewSlot) != 0, "C.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "C.M is Final") ;
        }

        [TestMethod]
        public void HidingMethodAfterSealed_WillBeUsingThatMethod ()
        {
            MethodInfo? method = typeof (B).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "B.M is Virtual") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.NewSlot) != 0, "B.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "B.M is Final") ;
        }

        [TestMethod]
        public void OverrideAfterHiding_WillBeVirtual ()
        {
            MethodInfo? method = typeof (A).GetMethod ("M") ;

            Assert.IsTrue ((method.Attributes  & MethodAttributes.Virtual) != 0, "A.M is Virtual") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.NewSlot) != 0, "A.M is NewSlot") ;
            Assert.IsFalse ((method.Attributes & MethodAttributes.Final)   != 0, "A.M is Final") ;
        }

        [TestMethod]
        public void ReturnValues ()
        {
            Assert.AreEqual (1, (new A () as G).M ()) ;
            Assert.AreEqual (2, (new A () as F).M ()) ;
            Assert.AreEqual (7, (new A () as E).M ()) ;
            Assert.AreEqual (7, (new A () as D).M ()) ;
            Assert.AreEqual (7, (new A () as C).M ()) ;
            Assert.AreEqual (7, (new A () as B).M ()) ;
            Assert.AreEqual (7, new A ().M ()) ;
        }
    }
}
