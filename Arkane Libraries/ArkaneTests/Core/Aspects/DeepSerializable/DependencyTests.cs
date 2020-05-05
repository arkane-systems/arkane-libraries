#region header

// ArkaneTests - DependencyTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 1:06 AM

#endregion

#region using

using System ;
using System.IO ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;

using ArkaneSystems.Arkane ;
using ArkaneSystems.Arkane.Aspects ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.DeepSerializable
{
    [TestClass]
    public class DependencyTests
    {
        [TestMethod]
        public void OutsiderNotSerializable ()
        {
            BinaryFormatter bf                  = new BinaryFormatter () ;
            MemoryStream    serializationStream = new MemoryStream () ;
            Assert.ThrowsException <SerializationException> (() =>
                                                             {
                                                                 bf.Serialize (serializationStream, new ContainsOutsider ()) ;
                                                                 serializationStream.Close () ;
                                                             }) ;
        }

        [TestMethod]
        public void NonSerializedOutsiderIsOk ()
        {
            BinaryFormatter bf                  = new BinaryFormatter () ;
            MemoryStream    serializationStream = new MemoryStream () ;
            bf.Serialize (serializationStream, new ContainsNonSerializedOutsider ()) ;
            serializationStream.Close () ;
        }
    }

    [DeepSerializable]
    public class ContainsOutsider
    {
        public GameState GameState { get ; set ; } = new GameState () ;

        public Empty Outsider { get ; set ; } = new Empty () ;
    }

    [DeepSerializable]
    public class ContainsNonSerializedOutsider
    {
        public GameState GameState { get ; set ; } = new GameState () ;

        [field: NonSerialized]
        public Empty Outsider { get ; set ; } = new Empty () ;
    }
}
