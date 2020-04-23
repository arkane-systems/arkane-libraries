#region header

// Arkane.Core - EventArgs.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 3:22 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Model ;
using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Generic event argument structure holding a single value.
    /// </summary>
    /// <typeparam name="T">The type of the contained value.</typeparam>
    [PublicAPI]
    [Immutable]
    public class EventArgs <T> : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the ArkaneSystems.Arkane.Base.EventArgs&lt;T&gt; class.
        /// </summary>
        /// <param name="value">The value of the event argument.</param>
        public EventArgs (T value) => this.Value = value ;

        /// <summary>
        ///     The typed value contained in the EventArgs{T}.
        /// </summary>
        [Reference]
        public T Value { get ; }
    }
}
