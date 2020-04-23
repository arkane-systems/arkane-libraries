#region header

// Arkane.Core - GenericExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 8:04 PM

#endregion

#region using

using System.Collections.Generic ;
using System.Linq ;
using System.Runtime.CompilerServices ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

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
        ///     Repeats an object; returns an enumeration of the specified object with the specified number of elements.
        /// </summary>
        /// <typeparam name="T">Type of the object to repeat.</typeparam>
        /// <param name="this">The object to repeat.</param>
        /// <param name="times">The number of times to repeat the object.</param>
        /// <returns>An enumeration of the repeated object.</returns>
        [ItemNotNull]
        [JetBrains.Annotations.NotNull]
        public static IEnumerable <T> Repeat <T> (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this T @this,
            [Positive] int times)
        {
            for (var i = 0; i < times; i++)
                yield return @this ;
        }

        /// <summary>
        ///     Swap the values of two references.
        /// </summary>
        /// <typeparam name="T">Type of the references to swap.</typeparam>
        /// <param name="first">The first reference to swap.</param>
        /// <param name="second">The second reference to swap.</param>
        public static void Swap <T> (ref T first, ref T second)
        {
            T tmp = first ;
            first  = second ;
            second = tmp ;
        }

        /// <summary>
        ///     Perform a given action or sequence of actions on a specified object.
        /// </summary>
        /// <typeparam name="T">Type of the specified object.</typeparam>
        /// <param name="obj">The object to act upon.</param>
        /// <param name="action">The action(s) to take.</param>
        /// <remarks>
        ///     <para>Intended usage:</para>
        ///     <para>
        ///         someVeryVeryLongVariableName.With(x => {
        ///         x.Int = 123;
        ///         x.Str = "Hello";
        ///         x.Str2 = " World!";
        ///         });
        ///     </para>
        ///     <para>This emulates the Visual Basic.NET With statement.</para>
        /// </remarks>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void With <T> ([JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                     this T obj,
                                     [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                     Action <T>
                                         action) where T : class => action (obj) ;

        #region If

        /// <summary>
        ///     Determines if the object fulfills the predicate and, if it does, returns itself.  Otherwise, returns the
        ///     default value.
        /// </summary>
        /// <param name="this">The object to test.</param>
        /// <param name="predicate">The predicate with which to test the object.</param>
        /// <param name="defaultValue">The default value to return if the predicate fails.</param>
        /// <typeparam name="T">The type of the object to test.</typeparam>
        /// <returns>The original object if <paramref name="predicate" /> returns true; the default value otherwise.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [CanBeNull]
        public static T If <T> ([JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                this T @this,
                                [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                Predicate <T>
                                    predicate,
                                [CanBeNull] T defaultValue = default) => predicate (@this) ? @this : defaultValue ;

        /// <summary>
        ///     Determines if the object fulfills the predicate and, if it does NOT, returns itself.  Otherwise, returns the
        ///     default value.
        /// </summary>
        /// <param name="this">The object to test.</param>
        /// <param name="predicate">The predicate with which to test the object.</param>
        /// <param name="defaultValue">The default value to return if the predicate passes.</param>
        /// <typeparam name="T">The type of the object to test.</typeparam>
        /// <returns>The original object if <paramref name="predicate" /> returns false; the default value otherwise.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [CanBeNull]
        public static T IfNot <T> ([JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                   this T @this,
                                   [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                   Predicate <T>
                                       predicate,
                                   [CanBeNull] T defaultValue = default) => predicate (@this) ? defaultValue : @this ;

        #endregion If

        #region List membership

        /// <summary>
        ///     Tests to see if this object is contained within the supplied list.
        /// </summary>
        /// <typeparam name="T">Type of the object to test.</typeparam>
        /// <param name="obj">The object to test.</param>
        /// <param name="list">The list to test the object against the membership of.</param>
        /// <returns>True if <paramref name="obj" /> is equal to a member of <paramref name="list" />; false otherwise.</returns>
        public static bool EqualsAnyOf <T> ([CanBeNull] this T obj,
                                            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                            params T[] list) =>
            list.Contains (obj) ;

        /// <summary>
        ///     Tests to see if this object is not contained within the supplied list.
        /// </summary>
        /// <typeparam name="T">Type of the object to test.</typeparam>
        /// <param name="obj">The object to test.</param>
        /// <param name="list">The list to test the object against the membership of.</param>
        /// <returns>False if <paramref name="obj" /> is equal to a member of <paramref name="list" />; true otherwise.</returns>
        public static bool EqualsNoneOf <T> ([CanBeNull] this T obj,
                                             [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                             params T[] list) =>
            !obj.EqualsAnyOf (list) ;

        #endregion List membership

        #region Null handling

        /// <summary>
        ///     Return the value of <paramref name="func" /> if <paramref name="obj" /> is not null;
        ///     otherwise, if it is null, return <paramref name="elseValue" />.
        /// </summary>
        /// <typeparam name="TIn">Type of the object (<paramref name="obj" />) that is tested.</typeparam>
        /// <typeparam name="TReturn">Type of the result returned by <paramref name="func" /> and of <paramref name="elseValue" />.</typeparam>
        /// <param name="obj">The object to test.</param>
        /// <param name="func">The function to apply to <paramref name="obj" /> if it is not null.</param>
        /// <param name="elseValue">
        ///     The value to return if <paramref name="obj" /> is null (defaulting to default{T} if not
        ///     specified).
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [CanBeNull]
        public static TReturn NullOr <TIn, TReturn> (this TIn? obj,
                                                     [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                                     Func <TIn, TReturn> func,
                                                     [CanBeNull] TReturn elseValue = default)
            where TIn : class =>
            obj != null ? func (obj) : elseValue ;

        /// <summary>
        ///     Throws an exception if the object reference called upon is null.
        /// </summary>
        /// <typeparam name="T">The class of the calling reference.</typeparam>
        /// <param name="obj">The object reference to call upon.</param>
        /// <param name="caller">The name of the calling member.</param>
        /// <exception cref="ArgumentNullException">Thrown if object reference called upon is null.</exception>
        [ContractAnnotation ("obj:null=>halt")]
        public static void ThrowIfNull <T> (this T? obj, [CallerMemberName] string? caller = null)
            where T : class
        {
            if (obj == null)
                throw new ArgumentNullException (nameof (obj), $@"Object unexpectedly null in {caller}.") ;
        }

        #endregion Null handling

        #region Wrap object

        private static T Return <T> (this T value) => value ;

        /// <summary>
        ///     Return a wrapper function which returns a given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to wrap.</typeparam>
        /// <param name="value">The object to wrap.</param>
        /// <returns>The wrapper function.</returns>
        /// <remarks>
        ///     Avoids the closure allocation; see https://gist.github.com/AArnott/d285feef75c18f6ecd2b
        /// </remarks>
        public static Func <T> AsFunc <T> (this T value) where T : class => value.Return ;

        /// <summary>
        ///     Wraps an object instance into an <see cref="IEnumerable{T}" /> consisting of a single item.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="item">The object instance to wrap.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{T}" /> consisting of a single item, or an empty enumerable if
        ///     <paramref name="item" /> is null.
        /// </returns>
        [ItemNotNull]
        public static IEnumerable <T> AsSingleton <T> ([CanBeNull] this T item)
        {
            // ReSharper disable once CompareNonConstrainedGenericWithNull; intended semantics.
            if (item == null)
                yield break ;

            yield return item ;
        }

        #endregion Wrap object
    }
}
