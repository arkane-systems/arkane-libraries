#region header

// Arkane.Core - Enums.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 9:51 AM

#endregion

#region using

using System ;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Linq ;
using System.Linq.Expressions ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Extensibility ;
using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Additional non-extension methods for <see cref="System.Enum" /> (enum).
    /// </summary>
    [PublicAPI]
    public static class Enums
    {
        #region UnconstrainedMelody

        #region Nested type: EnumInternals

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

        /// <summary>
        ///     Shared constants used by Flags and Enums.
        /// </summary>
        internal static class EnumInternals <T> where T : Enum
        {
            static EnumInternals ()
            {
                EnumInternals <T>.Values = new ReadOnlyCollection <T> ((T[]) Enum.GetValues (typeof (T))) ;
                EnumInternals <T>.Names  = new ReadOnlyCollection <string> (Enum.GetNames (typeof (T))) ;

                EnumInternals <T>.ValueToDescriptionMap = new ConcurrentDictionary <T, string?> () ;
                EnumInternals <T>.DescriptionToValueMap = new ConcurrentDictionary <string, T> () ;

                foreach (T value in EnumInternals <T>.Values)
                {
                    string? description = EnumInternals <T>.GetDescription (value) ;
                    EnumInternals <T>.ValueToDescriptionMap[value] = description ;

                    if ((description != null) && !EnumInternals <T>.DescriptionToValueMap.ContainsKey (description))
                        EnumInternals <T>.DescriptionToValueMap[description] = value ;
                }

                EnumInternals <T>.UnderlyingType = Enum.GetUnderlyingType (typeof (T)) ;
                EnumInternals <T>.IsFlags        = typeof (T).GetTypeInfo ().IsDefined (typeof (FlagsAttribute), false) ;

                // Parameters for various expression trees
                ParameterExpression param1 = Expression.Parameter (typeof (T), "x") ;
                ParameterExpression param2 = Expression.Parameter (typeof (T), "y") ;

                Expression convertedParam1 = Expression.Convert (param1, EnumInternals <T>.UnderlyingType) ;
                Expression convertedParam2 = Expression.Convert (param2, EnumInternals <T>.UnderlyingType) ;

                EnumInternals <T>.Equality =
                    Expression.Lambda <Func <T, T, bool>> (Expression.Equal (convertedParam1, convertedParam2),
                                                           param1,
                                                           param2)
                              .Compile () ;

                EnumInternals <T>.Or =
                    Expression.Lambda <Func <T, T, T>> (
                                                        Expression.Convert (
                                                                            Expression.Or (convertedParam1,
                                                                                           convertedParam2),
                                                                            typeof (T)),
                                                        param1,
                                                        param2)
                              .Compile () ;

                EnumInternals <T>.And =
                    Expression.Lambda <Func <T, T, T>> (
                                                        Expression.Convert (
                                                                            Expression.And (convertedParam1,
                                                                                            convertedParam2),
                                                                            typeof (T)),
                                                        param1,
                                                        param2)
                              .Compile () ;

                EnumInternals <T>.Not =
                    Expression.Lambda <Func <T, T>> (Expression.Convert (Expression.Not (convertedParam1), typeof (T)),
                                                     param1)
                              .Compile () ;

                EnumInternals <T>.IsEmpty = Expression.Lambda <Func <T, bool>> (Expression.Equal (convertedParam1,
                                                                                                  Expression.Constant (
                                                                                                                       Activator
                                                                                                                          .CreateInstance
                                                                                                                               (
                                                                                                                                EnumInternals
                                                                                                                                    <
                                                                                                                                        T
                                                                                                                                    >
                                                                                                                                   .UnderlyingType))),
                                                                                param1)
                                                      .Compile () ;

                EnumInternals <T>.UsedBits = default! ;
                foreach (T value in Enums.GetValues <T> ())
                    EnumInternals <T>.UsedBits = EnumInternals <T>.Or (EnumInternals <T>.UsedBits, value) ;

                EnumInternals <T>.AllBits = EnumInternals <T>.Not (default!) ;

                EnumInternals <T>.UnusedBits = EnumInternals <T>.And (EnumInternals <T>.AllBits,
                                                                      EnumInternals <T>.Not (EnumInternals <T>.UsedBits)) ;
            }

            private static string? GetDescription ([JetBrains.Annotations.NotNull] T value)
            {
                FieldInfo field = typeof (T).GetTypeInfo ().GetDeclaredField (value.ToString ()) ;
                return field.GetCustomAttributes (typeof (DescriptionAttribute), false)
                            .Cast <DescriptionAttribute> ()
                            .Select (x => x.Description)
                            .FirstOrDefault () ;
            }

            // ReSharper disable StaticMemberInGenericType
            // ReSharper disable once MemberHidesStaticFromOuterClass
            internal static readonly bool IsFlags ;

            internal static readonly Func <T, T, T>    Or         = null! ;
            internal static readonly Func <T, T, T>    And        = null! ;
            internal static readonly Func <T, T>       Not        = null! ;
            internal static readonly T                 UsedBits   = default! ;
            internal static readonly T                 AllBits    = default! ;
            internal static readonly T                 UnusedBits = default! ;
            internal static readonly Func <T, T, bool> Equality   = null! ;
            internal static readonly Func <T, bool>    IsEmpty    = null! ;
            internal static readonly IList <T>         Values     = null! ;
            internal static readonly IList <string>    Names      = null! ;

            internal static readonly Type UnderlyingType = null! ;

            [SuppressWarning ("THR027")]
            internal static readonly ConcurrentDictionary <T, string?> ValueToDescriptionMap = null! ;

            [SuppressWarning ("THR027")]
            internal static readonly ConcurrentDictionary <string, T> DescriptionToValueMap = null! ;

            // ReSharper restore StaticMemberInGenericType
        }

        #endregion

        #region Enums in general

        /// <summary>
        ///     Returns the underlying type for the enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>The underlying type (Byte, Int32 etc) for the enum</returns>
        public static Type GetUnderlyingType <T> () where T : Enum
            => EnumInternals <T>.UnderlyingType ;

        /// <summary>
        ///     Returns the values for the given enum as an immutable list.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        public static IList <T> GetValues <T> () where T : Enum
            => EnumInternals <T>.Values ;

        /// <summary>
        ///     Returns the names for the given enum as an immutable list.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>An array of names in the enum.</returns>
        [ItemNotNull]
        public static IList <string> GetNames <T> () where T : Enum
            => EnumInternals <T>.Names ;

        /// <summary>
        ///     Returns an array of the values in the enum.
        /// </summary>
        /// <typeparam name="T">An enum type.</typeparam>
        /// <returns>An array of the values in the enum.</returns>
        public static T[] GetValuesArray <T> () where T : Enum
            => (T[]) Enum.GetValues (typeof (T)) ;

        /// <summary>
        ///     Returns an array of names in the enum.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>An array of names in the enum.</returns>
        [ItemNotNull]
        public static string[] GetNamesArray <T> () where T : Enum
            => Enum.GetNames (typeof (T)) ;

        /// <summary>
        ///     Parses the name of an enum value.
        /// </summary>
        /// <remarks>
        ///     This method only considers named values: it does not parse comma-separated
        ///     combinations of flags enums.
        /// </remarks>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>The parsed value.</returns>
        /// <exception cref="ArgumentException">The name could not be parsed.</exception>
        public static T ParseName <T> (
            [JetBrains.Annotations.NotNull] [Required]
            string name) where T : Enum
        {
            if (!Enums.TryParseName (name, out T value))
                throw new ArgumentException (Resources.Enums_ParseName_UnknownValueName, nameof (name)) ;

            return value ;
        }

        /// <summary>
        ///     Attempts to find a value for the specified name.
        ///     Only names are considered - not numeric values.
        /// </summary>
        /// <remarks>
        ///     If the name is not parsed, <paramref name="value" /> will
        ///     be set to the zero value of the enum. This method only
        ///     considers named values: it does not parse comma-separated
        ///     combinations of flags enums.
        /// </remarks>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="name">Name to parse</param>
        /// <param name="value">Enum value corresponding to given name (on return)</param>
        /// <returns>Whether the parse attempt was successful or not</returns>
        public static bool TryParseName <T> (
            [Required] string name,
            out        T      value)
            where T : Enum
        {
            // TODO: Speed this up for big enums
            int index = EnumInternals <T>.Names.IndexOf (name) ;

            if (index == -1)
            {
                value = default! ;
                return false ;
            }

            value = EnumInternals <T>.Values[index] ;

            return true ;
        }

        /// <summary>
        ///     Attempts to find a value with the given description.
        /// </summary>
        /// <remarks>
        ///     More than one value may have the same description. In this unlikely
        ///     situation, the first value with the specified description is returned.
        /// </remarks>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="description">Description to find.</param>
        /// <param name="value">Enum value corresponding to given description (on return).</param>
        /// <returns>
        ///     True if a value with the given description was found,
        ///     false otherwise.
        /// </returns>
        public static bool TryParseDescription <T> (
            [Required]      string description,
            [CanBeNull] out T      value)
            where T : Enum => EnumInternals <T>.DescriptionToValueMap.TryGetValue (description, out value) ;

        #endregion

        #region Flags

        // Provides a set of static methods for use with "flags" enums,
        // i.e. those decorated with <see cref="FlagsAttribute"/>.
        // Other than <see cref="IsValidCombination{T}"/>, methods in this
        // class throw <see cref="TypeArgumentException" />.

        /// <summary>
        ///     Returns whether or not the specified enum is a "flags" enum,
        ///     i.e. whether it has FlagsAttribute applied to it.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>
        ///     True if the enum type is decorated with
        ///     FlagsAttribute; False otherwise.
        /// </returns>
        [Pure]
        public static bool IsFlags <T> () where T : Enum
            => EnumInternals <T>.IsFlags ;

        // ReSharper disable once ExceptionNotThrown
        /// <summary>
        ///     Returns all the bits used in any flag values
        /// </summary>
        /// internal static
        /// <returns>A flag value with all the bits set that are ever set in any defined value</returns>
        /// <exception cref="TypeArgumentException"><typeparamref name="T" /> is not a flags enum.</exception>
        public static T GetUsedBits <T> () where T : Enum
        {
            // TODO: replace with IsFlags aspect
            if (!Enums.IsFlags <T> ())
                throw new TypeArgumentException (Resources.Enums_GetUsedBits_CannotCallForNonFlags,
                                                 nameof (T)) ;

            return EnumInternals <T>.UsedBits ;
        }

        #endregion

        #endregion Unconstrained Melody
    }
}
