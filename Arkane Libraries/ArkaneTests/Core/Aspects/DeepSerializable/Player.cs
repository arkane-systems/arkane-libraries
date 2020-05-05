#region header

// ArkaneTests - Player.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 1:02 AM

#endregion

#region using

using System ;

#endregion

namespace ArkaneTests.Core.Aspects.DeepSerializable
{
    public class Player
    {
        public Player () { Console.WriteLine ("New player created.") ; }

        public Creature[] ControlledCreatures ;
        public Creature   Hero ;
    }

    public class NonSerializablePlayer
    {
        public NonSerializablePlayer () { Console.WriteLine ("New player created.") ; }

        public Creature[] ControlledCreatures ;
        public Creature   Hero ;
    }
}
