#region header

// ArkaneTests - GameState.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 1:01 AM

#endregion

#region using

using System ;
using System.Collections.Generic ;

using ArkaneSystems.Arkane.Aspects ;

#endregion

namespace ArkaneTests.Core.Aspects.DeepSerializable
{
    [DeepSerializable]
    public class GameState
    {
        public Player        MasterPlayer ;
        public List <Player> Players = new List <Player> () ;

        public int TurnsElapsed ;

        public Weather Weather ;
    }

    [Serializable]
    public class NonDeepGameState
    {
        public NonSerializablePlayer        MasterPlayer ;
        public List <NonSerializablePlayer> Players = new List <NonSerializablePlayer> () ;

        public int TurnsElapsed ;

        public Weather Weather ;
    }
}
