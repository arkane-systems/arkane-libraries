#region header

// Arkane.Core - DisposerByDelegation.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 1:33 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Serves as a wrapper around objects that require disposal but that do not
    ///     implement <see cref="System.IDisposable" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of <see cref="object" />
    /// </typeparam>
    [PublicAPI]
    [ExplicitlySynchronized]
    public class DisposerByDelegation <T> : DisposerBase <T>
    {
        /// <summary>
        ///     Invoked when <see cref="DisposerBase{T}.Dispose()" /> is called.
        /// </summary>
        protected readonly Action <T> DisposeAction ; // ReSharper disable ExceptionNotThrown

        /// <summary>
        ///     Instantiates a new <see cref="DisposerByDelegation{T}" /> object with the
        ///     specified <paramref name="obj" />.
        /// </summary>
        /// <param name="obj">
        ///     The value of the specified object. />.
        /// </param>
        /// <param name="disposeAction">
        ///     The value of
        ///     <see cref="DisposeAction" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="obj" /> is
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="disposeAction" /> is
        ///     <c>null</c>.
        /// </exception>

        // ReSharper disable restore ExceptionNotThrown
        public DisposerByDelegation (T          obj,
                                     Action <T> disposeAction)
            : base (obj) => this.DisposeAction = disposeAction ;

        /// <summary>
        ///     Releases the unmanaged resources used by the
        ///     <see cref="DisposerByDelegation{T}" /> and optionally releases the
        ///     managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     If <c>true</c>, releases both managed and
        ///     unmanaged resources, otherwise releases only unmanaged
        ///     resources.
        /// </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected override void DisposeImplementation (bool disposing)
        {
            if (!disposing)
                return ;

            this.DisposeAction?.Invoke (this.Object) ;
        }
    }
}
