#region header

// ArkaneTests - DeepSerializableTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:59 AM

#endregion

#region using

using System.Collections.Generic ;
using System.IO ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Aspects.DeepSerializable
{
    [TestClass]
    public class DeepSerializableTests
    {
        [TestMethod]
        public void MainTest ()
        {
            Player p1 = new Player {Hero = new Creature (), ControlledCreatures = new[] {new Creature ()}} ;
            GameState state = new GameState
                              {
                                  Players = new List <Player> {p1}, MasterPlayer = p1, TurnsElapsed = 3, Weather = new Weather ()
                              } ;
            BinaryFormatter bf                  = new BinaryFormatter () ;
            MemoryStream    serializationStream = new MemoryStream () ;
            bf.Serialize (serializationStream, state) ;
            serializationStream.Close () ;
        }

        [TestMethod]
        public void MainControlTest ()
        {
            NonSerializablePlayer p1 = new NonSerializablePlayer
                                       {
                                           Hero = new Creature (), ControlledCreatures = new[] {new Creature ()}
                                       } ;
            NonDeepGameState state = new NonDeepGameState
                                     {
                                         Players      = new List <NonSerializablePlayer> {p1},
                                         MasterPlayer = p1,
                                         TurnsElapsed = 3,
                                         Weather      = new Weather ()
                                     } ;
            BinaryFormatter bf                  = new BinaryFormatter () ;
            MemoryStream    serializationStream = new MemoryStream () ;
            Assert.ThrowsException <SerializationException> (() =>
                                                             {
                                                                 bf.Serialize (serializationStream, state) ;
                                                                 serializationStream.Close () ;
                                                             }) ;
        }
    }
}
