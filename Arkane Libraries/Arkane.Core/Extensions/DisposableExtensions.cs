#region header

// Arkane.Core - DisposableExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:46 PM

#endregion

#region using

using System.Collections.Generic ;

using JetBrains.Annotations ;

#endregion

// ReSharper disable once CheckNamespace
namespace System
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static partial class ας_System
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        ///     Wraps an object that implements <see cref="IDisposable" /> in an enumerable to make it safe
        ///     for use in LINQ expressions.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="target">The target to wrap.</param>
        /// <returns>An enumeration with the target as its single entry.</returns>
        [ItemNotNull]
        public static IEnumerable <T> AutoDispose <T> (this T target)
            where T : IDisposable
        {
            try
            {
                yield return target ;
            }
            finally
            {
                target.Dispose () ;
            }
        }

        #region TryDispose

        /// <summary>
        ///     Attempts to dispose of an object.
        /// </summary>
        /// <param name="this">The object to dispose.</param>
        /// <returns>True if the object was disposed successfully; false otherwise (an exception was thrown).</returns>
        public static bool TryDispose (this IDisposable? @this)
            => @this.TryDispose (out var _) ;

        /// <summary>
        ///     Attempts to dispose of an object.
        /// </summary>
        /// <param name="this">The object to dispose.</param>
        /// <param name="handler">An exception handler called if disposal fails.</param>
        /// <returns>True if the object was disposed successfully; false otherwise (an exception was thrown).</returns>
        public static bool TryDispose (this IDisposable?  @this,
                                       Action <Exception> handler)
        {
            bool retval = @this.TryDispose (out Exception? exception) ;

            if (!retval)
                handler (exception!) ;

            return retval ;
        }

        /// <summary>
        ///     Attempts to dispose of an object.
        /// </summary>
        /// <param name="this">The object to dispose.</param>
        /// <param name="handler">An exception handler called if disposal fails.</param>
        /// <returns>True if the object was disposed successfully; false otherwise (an exception was thrown).</returns>
        public static bool TryDispose (this IDisposable?         @this,
                                       Lazy <Action <Exception>> handler)
            => @this.TryDispose (handler.Value) ;

        /// <summary>
        ///     Attempts to dispose of an object.
        /// </summary>
        /// <param name="this">The object to dispose.</param>
        /// <param name="exception">
        ///     If the disposal fails, contains the exception thrown, or <see cref="NullReferenceException" />
        ///     if @this is null.
        /// </param>
        /// <returns>True if the object was disposed successfully; false otherwise (an exception was thrown).</returns>
        public static bool TryDispose (this IDisposable? @this, out Exception? exception)
        {
            exception = null ;

            try
            {
                if (@this != null)
                {
                    @this.Dispose () ;
                    return true ;
                }

                exception = new NullReferenceException (ArkaneSystems.Arkane.Properties.Resources
                                                                     .ας_System_TryDispose_NullIDisposablePassedToTryDispose) ;
            }
            catch (Exception ex)
            {
                exception = ex ;
            }

            return false ;
        }

        #endregion TryDispose
    }
}
