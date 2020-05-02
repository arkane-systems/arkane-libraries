#region header

// Arkane.Core - EnumExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 10:03 AM

#endregion

#region using

using System.Linq ;

using ArkaneSystems.Arkane ;

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
        #region UnconstrainedMelody

        //
        // Enum extensions in this section based on code from Unconstrained Melody, (c) 2009-2011 Jonathan Skeet.
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

        #region Enums in general

        /// <summary>
        ///     Returns the description for the given value,
        ///     as specified by DescriptionAttribute, or null
        ///     if no description is present.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="item">Value to fetch description for</param>
        /// <returns>
        ///     The description of the value, or null if no description
        ///     has been specified (but the value is a named value).
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="item" />
        ///     is not a named member of the enum
        /// </exception>
        public static string? GetDescription <T> ([NotNull] this T item) where T : Enum
        {
            if (Enums.EnumInternals <T>.ValueToDescriptionMap.TryGetValue (item, out string? description))
                return description ;

            throw new ArgumentOutOfRangeException (nameof (item)) ;
        }

        /// <summary>
        ///     Checks whether the value is a named value for the type.
        /// </summary>
        /// <remarks>
        ///     For flags enums, it is possible for a value to be a valid
        ///     combination of other values without being a named value
        ///     in itself. To test for this possibility, use IsValidCombination.
        /// </remarks>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="value">Value to test.</param>
        /// <returns>True if this value has a name, False otherwise.</returns>
        public static bool IsNamedValue <T> (this T value) where T : Enum
            => Enums.GetValues <T> ().Contains (value) ;

        #endregion

        #region Flags

        // Provides a set of static methods for use with "flags" enums,
        // i.e. those decorated with <see cref="FlagsAttribute"/>.
        // Other than <see cref="IsValidCombination{T}"/>, methods in this
        // class throw <see cref="TypeArgumentException" />.

        /// <summary>
        ///     Determines whether the given value only uses bits covered
        ///     by named values.
        /// </summary>
        /// internal static
        /// <param name="values">Combination to test</param>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static bool IsValidCombination <T> (this T values)
            where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            return values.And (Enums.EnumInternals <T>.UnusedBits).IsEmpty () ;
        }

        /// <summary>
        ///     Determines whether the two specified values have any flags in common.
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <param name="desiredFlags">Flags we wish to find</param>
        /// <returns>Whether the two specified values have any flags in common.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static bool HasAny <T> (this T value, T desiredFlags)
            where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            return value.And (desiredFlags).IsNotEmpty () ;
        }

        /// <summary>
        ///     Determines whether all of the flags in <paramref name="desiredFlags" />
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <param name="desiredFlags">Flags we wish to find</param>
        /// <returns>Whether all the flags in <paramref name="desiredFlags" /> are in <paramref name="value" />.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static bool HasAll <T> (this T value, T desiredFlags)
            where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            // ReSharper disable once EventExceptionNotDocumented
            return Enums.EnumInternals <T>.Equality (value.And (desiredFlags), desiredFlags) ;
        }

        // ReSharper disable once ExceptionNotThrown
        /// <summary>
        ///     Returns the bitwise "and" of two values.
        /// </summary>
        /// internal static
        /// <param name="first">First value</param>
        /// <param name="second">Second value</param>
        /// <returns>The bitwise "and" of the two values</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T And <T> (this T first, T second) where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            // ReSharper disable once EventExceptionNotDocumented
            return Enums.EnumInternals <T>.And (first, second) ;
        }

        // ReSharper disable once ExceptionNotThrown
        /// <summary>
        ///     Returns the bitwise "or" of two values.
        /// </summary>
        /// internal static
        /// <param name="first">First value</param>
        /// <param name="second">Second value</param>
        /// <returns>The bitwise "or" of the two values</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T Or <T> (this T first, T second) where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            // ReSharper disable once EventExceptionNotDocumented
            return Enums.EnumInternals <T>.Or (first, second) ;
        }

        // ReSharper disable once ExceptionNotThrown
        /// <summary>
        ///     Returns the inverse of a value, with no consideration for which bits are used
        ///     by values within the enum (i.e. a simple bitwise negation).
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value to invert</param>
        /// <returns>The bitwise negation of the value</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T AllBitsInverse <T> (this T value) where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            // ReSharper disable once EventExceptionNotDocumented
            return Enums.EnumInternals <T>.Not (value) ;
        }

        /// <summary>
        ///     Returns the inverse of a value, but limited to those bits which are used by
        ///     values within the enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value to invert</param>
        /// <returns>The restricted inverse of the value</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T UsedBitsInverse <T> (this T value) where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            return value.AllBitsInverse ().And (Enums.EnumInternals <T>.UsedBits) ;
        }

        // ReSharper disable once ExceptionNotThrown
        /// <summary>
        ///     Returns whether this value is an empty set of fields, i.e. the zero value.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value to test</param>
        /// <returns>True if the value is empty (zero); False otherwise.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static bool IsEmpty <T> (this T value) where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            // ReSharper disable once EventExceptionNotDocumented
            return Enums.EnumInternals <T>.IsEmpty (value) ;
        }

        /// <summary>
        ///     Returns whether this value has any fields set, i.e. is not zero.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value to test</param>
        /// <returns>True if the value is non-empty (not zero); False otherwise.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static bool IsNotEmpty <T> (this T value) where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            return !value.IsEmpty () ;
        }

        #endregion

        #endregion UnconstrainedMelody

        #region Other flags

        /// <summary>
        ///     Clears a flag and returns the new value.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="value">Value to clear.</param>
        /// <param name="flag">Flag to clear.</param>
        /// <returns>The value with the flag cleared.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T ClearFlag <T> (this T value, T flag) where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            return value.ClearFlags (flag) ;
        }

        /// <summary>
        ///     Clears flags and returns the new value.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="value">Value to clear.</param>
        /// <param name="flags">Flags to clear.</param>
        /// <returns>The value with the flags cleared.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T ClearFlags <T> (this   T   value,
                                        params T[] flags)
            where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            T result = default! ;

            // ReSharper disable once HeapView.SlowDelegateCreation
            result = flags.Aggregate (result, (current, flag) => current.Or (flag)).AllBitsInverse () ;

            return value.And (result) ;
        }

        /// <summary>
        ///     Sets a flag and returns the new value.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="value">Value to set.</param>
        /// <param name="flag">Flag to set.</param>
        /// <returns>The value with the flag set.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T SetFlag <T> (this T value, T flag) where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            return value.SetFlags (flag) ;
        }

        /// <summary>
        ///     Sets flags and returns the new value.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="value">Value to set.</param>
        /// <param name="flags">Flags to set.</param>
        /// <returns>The value with the flags set.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T SetFlags <T> (this T value,
                                      [NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                      params T[] flags)
            where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            T result = default! ;

            // ReSharper disable once HeapView.SlowDelegateCreation
            result = flags.Aggregate (result, (current, flag) => current.Or (flag)) ;

            return value.Or (result) ;
        }

        /// <summary>
        ///     Determines whether the two specified values have any flags in common.
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <param name="desiredFlags">Flags we wish to find</param>
        /// <returns>Whether the two specified values have any flags in common.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static bool HasAny <T> (this T value,
                                       [NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                       params T[] desiredFlags)
            where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            T aggregate = default! ;

            // ReSharper disable once HeapView.SlowDelegateCreation
            aggregate = desiredFlags.Aggregate (aggregate, (current, flag) => current.Or (flag)) ;

            return value.HasAny (aggregate) ;
        }

        /// <summary>
        ///     Determines whether all of the flags in <paramref name="desiredFlags" />
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <param name="desiredFlags">Flags we wish to find</param>
        /// <returns>Whether all the flags in <paramref name="desiredFlags" /> are in <paramref name="value" />.</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static bool HasAll <T> (this T value,
                                       [NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                       params T[] desiredFlags)
            where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (ArkaneSystems.Arkane.Properties.Resources.ας_System_CannotCallForNonFlagsEnum,
                                                 nameof (T)) ;

            T aggregate = default! ;

            // ReSharper disable once HeapView.SlowDelegateCreation
            aggregate = desiredFlags.Aggregate (aggregate, (current, flag) => current.Or (flag)) ;

            return value.HasAll (aggregate) ;
        }

        #endregion Other flags
    }
}
