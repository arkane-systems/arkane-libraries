#region header

// Arkane.Core - DisposableValue.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 1:17 PM

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
    ///     Implements <see cref="IDisposable{T}" />; encapsulates a read-only value, enabling consumers to signal when they
    ///     are done with the value through the use of the <see cref="IDisposable.Dispose" /> method.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    [PublicAPI]
    [ThreadUnsafe]
    public class DisposableValue <T> : IDisposable <T>
    {
        /// <summary>
        ///     Encapsulates a read-only value, enabling consumers to dispose of it via the <see cref="IDisposable.Dispose" />
        ///     method of the passed <see cref="IDisposable" />. Exceptions thrown during disposal will be silently ignored.
        /// </summary>
        /// <param name="value">The value to encapsulate.</param>
        /// <param name="dispose">The disposal <see cref="IDisposable" />.</param>
        public DisposableValue (T value, IDisposable dispose)
            : this (value, () => dispose.TryDispose ())
        { }

        /// <summary>
        ///     Encapsulates a read-only value, enabling consumers to dispose of it via the supplied disposal
        ///     <see cref="Action" />.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dispose"></param>
        public DisposableValue (T value, Action dispose)
        {
            this.Value   = value ;
            this.dispose = dispose ;
        }

        private readonly Action dispose ;

        /// <summary>
        ///     The encapsulated value.
        /// </summary>
        [Child]
        public T Value { get ; }

        /// <inheritdoc />
        public void Dispose () => this.dispose () ;
    }
}
