#region header

// Arkane.Core - Memento.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 1:13 PM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Remember an action to be performed on disposal; returned from a method needing later closure.
    /// </summary>
    [PublicAPI]
    [ThreadAffine]
    public sealed class Memento : IDisposable
    {
        /// <summary>
        ///     Create a memento that performs the given action on disposal.
        /// </summary>
        /// <param name="action">The action to perform on disposal.</param>
        public Memento (Action action) => this.Action = action ;

        private bool Disposed { get ; set ; }

        private Action Action { get ; }

        #region IDisposable Members

        void IDisposable.Dispose ()
        {
            // Since this is not simply resource cleanup, throw on double-calls.
            if (this.Disposed)
                throw new ObjectDisposedException (Resources.Memento_Dispose_AttemptedSecondDisposal) ;

            this.Disposed = true ;
            this.Action () ;
        }

        #endregion
    }
}
