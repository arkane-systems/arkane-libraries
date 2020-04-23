#region header

// Arkane.Core - DisposableAction.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 12:08 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    [PublicAPI]
    public class DisposableAction : IDisposable
    {
        public DisposableAction (Action? onDispose = null) => this.onDispose = onDispose ;

        private readonly Action? onDispose ;

        public void Dispose () { this.onDispose?.Invoke () ; }
    }
}
