#region header

// Arkane.Core - DisposerBase.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 1:24 PM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Logging ;
using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Serves as a wrapper around objects that require disposal but that do not
    ///     implement <see cref="System.IDisposable" />.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Object" /></typeparam>
    [PublicAPI]
    [ExplicitlySynchronized]
    public abstract class DisposerBase <T> : IDisposable
    {
        /// <summary>
        ///     Instantiates a new <see cref="DisposerBase{T}" /> object with the
        ///     specified <paramref name="obj" />.
        /// </summary>
        /// <param name="obj">The value of <see cref="Object" />.</param>
        protected DisposerBase (T obj) => this.Object = obj ;

        /// <summary>
        ///     Indicates that <see cref="Object" /> is disposed.
        /// </summary>
        protected bool IsDisposed ;

        /// <summary>
        ///     The object to be disposed.
        /// </summary>
        public T Object { get ; }

        #region IDisposable Members

        /// <summary>
        ///     Releases all resources used by the <see cref="DisposerBase{T}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        ///     <see cref="Object" /> is
        ///     already disposed.
        /// </exception>
        public void Dispose ()
        {
            GC.SuppressFinalize (this) ;
            this.Dispose (true) ;
        }

        #endregion

        /// <summary>
        ///     Finalizer for <see cref="DisposerBase{T}" />.
        /// </summary>
        /// <remarks>
        ///     Should not be reached; object should be properly disposed.  If it is reached, writes out
        ///     a debug warning.
        /// </remarks>
        ~DisposerBase ()
        {
            // Warning in debug mode.
            LogProvider.For <DisposerBase <T>> ()
                       .Debug (Resources.DisposerBase__DisposerBase_ObjectWasNotDisposed,
                               typeof (IDisposable).FullName!,
                               this.GetType ().FullName!) ;

            this.Dispose (false) ;
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the
        ///     <see cref="DisposerBase{T}" /> and optionally releases the managed
        ///     resources.
        /// </summary>
        /// <param name="disposing">
        ///     If <c>true</c>, releases both managed and
        ///     unmanaged resources, otherwise releases only unmanaged
        ///     resources.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        ///     <see cref="Object" /> is
        ///     already disposed.
        /// </exception>
        protected void Dispose (bool disposing)
        {
            if (this.IsDisposed)
                throw new ObjectDisposedException (this.GetType ().FullName) ;

            this.IsDisposed = true ;
            this.DisposeImplementation (disposing) ;
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the
        ///     <see cref="DisposerBase{T}" /> and optionally releases the managed
        ///     resources.
        /// </summary>
        /// <param name="disposing">
        ///     If <c>true</c>, releases both managed and
        ///     unmanaged resources, otherwise releases only unmanaged
        ///     resources.
        /// </param>
        protected abstract void DisposeImplementation (bool disposing) ;
    }
}
