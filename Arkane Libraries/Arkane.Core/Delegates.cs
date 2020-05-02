#region header

// Arkane.Core - Delegates.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 9:45 AM

#endregion

#region using

using System ;
using System.Reflection ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Additional non-extension methods for <see cref="System.Delegate" /> (delegate).
    /// </summary>
    [PublicAPI]
    public static class Delegates
    {
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

        #region CreateDelegate (lots of overloads!)

        /// <summary>
        ///     See <see cref="MethodInfo.CreateDelegate(System.Type)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="method">Method to create delegate for.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MissingMethodException">The Invoke method of <typeparamref name="T" /> is not found. </exception>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="method" />.
        /// </exception>
        [JetBrains.Annotations.NotNull]
        public static T CreateDelegate <T> (
            MethodInfo method)
            where T : Delegate => (T) method.CreateDelegate (typeof (T)) ;

        /// <summary>
        ///     See <see cref="MethodInfo.CreateDelegate(System.Type,object)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="method">Method to create delegate for.</param>
        /// <param name="target">
        ///     The target for the delegate (for instance methods) or the first argument
        ///     (for static methods).
        /// </param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MissingMethodException">The Invoke method of <paramref name="target" /> is not found. </exception>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="method" />.
        /// </exception>
        [JetBrains.Annotations.NotNull]
        public static T CreateDelegate <T> (
            object     target,
            MethodInfo method)
            where T : Delegate => (T) method.CreateDelegate (typeof (T), target) ;

        /// <summary>
        ///     See <see cref="Delegate.CreateDelegate(System.Type,object,string)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="target">The target for the delegate.</param>
        /// <param name="methodName">Name of instance method to create delegate for.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MissingMethodException">The Invoke method of <paramref name="target" /> is not found. </exception>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="methodName" />.
        /// </exception>
        [JetBrains.Annotations.NotNull]
        public static T CreateDelegate <T> (
            object            target,
            [Required] string methodName)
            where T : Delegate => (T) Delegate.CreateDelegate (typeof (T), target, methodName) ;

        /// <summary>
        ///     See <see cref="Delegate.CreateDelegate(System.Type,System.Type,string)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="targetType">The type containing the static method.</param>
        /// <param name="methodName">Name of static method to create delegate for.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MissingMethodException">The Invoke method of <paramref name="targetType" /> is not found. </exception>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="methodName" />.
        /// </exception>
        [JetBrains.Annotations.NotNull]
        public static T CreateDelegate <T> (
            Type              targetType,
            [Required] string methodName)
            where T : Delegate => (T) Delegate.CreateDelegate (typeof (T), targetType, methodName) ;

        /// <summary>
        ///     See <see cref="Delegate.CreateDelegate(System.Type,System.Reflection.MethodInfo,bool)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="method">Method to create delegate for.</param>
        /// <param name="throwOnBindFailure">Whether or not to throw an exception on bind failure.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MissingMethodException">The Invoke method of <typeparamref name="T" /> is not found. </exception>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="method" />.
        /// </exception>
        public static T? CreateDelegate <T> (
            MethodInfo method,
            bool       throwOnBindFailure)
            where T : Delegate => (T) Delegate.CreateDelegate (typeof (T), method, throwOnBindFailure) ;

        /// <summary>
        ///     See <see cref="Delegate.CreateDelegate(System.Type,object,System.Reflection.MethodInfo,bool)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="method">Method to create delegate for.</param>
        /// <param name="target">
        ///     The target for the delegate (for instance methods) or the first argument
        ///     (for static methods).
        /// </param>
        /// <param name="throwOnBindFailure">Whether or not to throw an exception on bind failure.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="method" />.
        /// </exception>
        /// <exception cref="MissingMethodException">The Invoke method of <paramref name="target" /> is not found. </exception>
        public static T? CreateDelegate <T> (
            object     target,
            MethodInfo method,
            bool       throwOnBindFailure)
            where T : Delegate => (T) Delegate.CreateDelegate (typeof (T), target, method, throwOnBindFailure) ;

        /// <summary>
        ///     See <see cref="Delegate.CreateDelegate(System.Type,object,string,bool)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="target">The target for the delegate.</param>
        /// <param name="methodName">Name of instance method to create delegate for.</param>
        /// <param name="ignoreCase">Whether the name should be matched in a case-insensitive manner.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MissingMethodException">The Invoke method of <paramref name="target" /> is not found. </exception>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="methodName" />.
        /// </exception>
        public static T? CreateDelegate <T> (
            object            target,
            [Required] string methodName,
            bool              ignoreCase)
            where T : Delegate => (T) Delegate.CreateDelegate (typeof (T), target, methodName, ignoreCase) ;

        /// <summary>
        ///     See <see cref="Delegate.CreateDelegate(System.Type,System.Type,string,bool)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="targetType">The type containing the static method.</param>
        /// <param name="methodName">Name of static method to create delegate for.</param>
        /// <param name="ignoreCase">Whether the name should be matched in a case-insensitive manner.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="methodName" />.
        /// </exception>
        /// <exception cref="MissingMethodException">The Invoke method of <paramref name="targetType" /> is not found. </exception>
        public static T? CreateDelegate <T> (
            Type              targetType,
            [Required] string methodName,
            bool              ignoreCase)
            where T : Delegate => (T) Delegate.CreateDelegate (typeof (T), targetType, methodName, ignoreCase) ;

        /// <summary>
        ///     See <see cref="Delegate.CreateDelegate(System.Type,object,string,bool,bool)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="target">The target for the delegate.</param>
        /// <param name="methodName">Name of instance method to create delegate for.</param>
        /// <param name="ignoreCase">Whether the name should be matched in a case-insensitive manner.</param>
        /// <param name="throwOnBindFailure">Whether or not to throw an exception on bind failure.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="methodName" />.
        /// </exception>
        /// <exception cref="MissingMethodException">The Invoke method of <paramref name="target" /> is not found. </exception>
        public static T? CreateDelegate <T> (
            object            target,
            [Required] string methodName,
            bool              ignoreCase,
            bool              throwOnBindFailure)
            where T : Delegate
            => (T) Delegate.CreateDelegate (typeof (T), target, methodName, ignoreCase, throwOnBindFailure) ;

        /// <summary>
        ///     See <see cref="Delegate.CreateDelegate(System.Type,System.Type,string,bool,bool)" />.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="targetType">The type containing the static method.</param>
        /// <param name="methodName">Name of static method to create delegate for.</param>
        /// <param name="ignoreCase">Whether the name should be matched in a case-insensitive manner.</param>
        /// <param name="throwOnBindFailure">Whether or not to throw an exception on bind failure.</param>
        /// <returns>A delegate for the given method.</returns>
        /// <exception cref="MethodAccessException">
        ///     The caller does not have the permissions necessary to access
        ///     <paramref name="methodName" />.
        /// </exception>
        /// <exception cref="MissingMethodException">The Invoke method of <paramref name="targetType" /> is not found. </exception>
        public static T? CreateDelegate <T> (
            Type              targetType,
            [Required] string methodName,
            bool              ignoreCase,
            bool              throwOnBindFailure)
            where T : Delegate
            => (T) Delegate.CreateDelegate (typeof (T), targetType, methodName, ignoreCase, throwOnBindFailure) ;

        #endregion

        #endregion
    }
}
