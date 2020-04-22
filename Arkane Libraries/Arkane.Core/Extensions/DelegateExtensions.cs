#region header

// Arkane.Core - DelegateExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 3:13 PM

#endregion

#region using

using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Linq ;

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
        #region Parallel

        /// <summary>Dynamically invokes (late-bound) in parallel the methods represented by the delegate.</summary>
        /// <param name="multicastDelegate">The delegate to be invoked.</param>
        /// <param name="args">An array of objects that are the arguments to pass to the delegates.</param>
        /// <returns>The return value of one of the delegate invocations.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [NotNull]
        public static object ParallelDynamicInvoke <T> (
            [NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this T multicastDelegate,
            [NotNull] [PostSharp.Patterns.Contracts.NotNull]
            params object[] args) where T : Delegate
        {
            return multicastDelegate.GetInvocationList ()
                                    .AsParallel ()
                                    .AsOrdered ()
                                    .Select (d => d.DynamicInvoke (args))
                                    .Last () ;
        }

        #endregion

        #region UnconstrainedMelody

        // Delegate extensions in this section based on code from Unconstrained Melody, (c) 2009-2011 Jonathan Skeet.
        // https://code.google.com/p/unconstrained-melody/
        // Made available under the Apache License, Version 2.0.
        // Original license:
        // 
        // Unconstrained Melody
        // Copyright (c) 2009-2011 Jonathan Skeet. All rights reserved.
        // 
        // Licensed under the Apache License, Version 2.0 (the "License");
        // you may not use this file except in compliance with the License.
        // You may obtain a copy of the License at
        // 
        //     http://www.apache.org/licenses/LICENSE-2.0
        // 
        // Unless required by applicable law or agreed to in writing, software
        // distributed under the License is distributed on an "AS IS" BASIS,
        // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
        // See the License for the specific language governing permissions and
        // limitations under the License.
        //

        /// <summary>
        ///     Returns the individual delegates comprising the specified value.
        ///     Each returned delegate will represent a single method invocation.
        ///     This method is effectively a strongly-typed wrapper around
        ///     <see cref="Delegate.GetInvocationList" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="value">Delegate to split.</param>
        /// <returns>A strongly typed array of single delegates.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is null.</exception>
        [ItemNotNull]
        [NotNull]
        public static T[] GetInvocationArray <T> (
            [NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this T value)
            where T : Delegate
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            Delegate[] delegates = value.GetInvocationList () ;

            // ReSharper disable ExceptionNotDocumented
            var ret = new T[delegates.Length] ;
            delegates.CopyTo (ret, 0) ;

            // ReSharper restore ExceptionNotDocumented
            return ret ;
        }

        /// <summary>
        ///     Returns the individual delegates comprising the specified value as an immutable list.
        ///     Each returned delegate will represent a single method invocation.
        ///     This method is effectively a wrapper around
        ///     <see cref="Delegate.GetInvocationList" />, but returning an immutable list instead
        ///     of an array.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="value">Delegate to split.</param>
        /// <returns>An immutable list of single delegates.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is null.</exception>
        [ItemNotNull]
        [NotNull]
        public static IList <T> GetReadOnlyInvocationList <T> (
            [NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this T value) where T : Delegate
            => new ReadOnlyCollection <T> (value.GetInvocationArray ()) ;

        #endregion
    }
}
