#region header

// Arkane.Core - ObjectExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 7:31 PM

#endregion

#region using

#endregion

// ReSharper disable once CheckNamespace

#region using

using System.Collections ;
using System.Collections.Generic ;
using System.Reflection ;

using JetBrains.Annotations ;

#endregion

namespace System
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static partial class ας_System
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        ///     Get the <see cref="TypeInfo" /> representation of the <see cref="Type" /> of the specified <see cref="Object" />.
        /// </summary>
        /// <param name="this">The object to return the type information of.</param>
        /// <returns>The <see cref="TypeInfo" /> pertaining to the type of the specified object.</returns>
        public static TypeInfo GetUnderlyingTypeInfo (this object @this) =>
            @this.GetType ().GetTypeInfo () ;

        #region Casting

        /// <summary>
        ///     Cast-by-example; cast an object to an anonymous type containing the matching fields.
        /// </summary>
        /// <param name="this">The object variable containing the anonymous type.</param>
        /// <param name="anonymousType">The anonymous type to cast to, defined by example; i.e., 'new { Test = "" }'. </param>
        /// <typeparam name="T">The anonymous type to cast to, provided implicitly.</typeparam>
        /// <returns>An object of the specified anonymous type.</returns>
        [NotNull]
        public static T CastByExample <T> (this object @this,
#pragma warning disable IDE0060 // Remove unused parameter
                                           T anonymousType) => (T) @this! ;
#pragma warning restore IDE0060 // Remove unused parameter

        /// <summary>
        ///     If <paramref name="obj" /> is not null and castable to the type <typeparamref name="T" />,
        ///     return <paramref name="obj" /> as a <typeparamref name="T" />. Otherwise, return the default
        ///     value of <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type to attempt to cast <paramref name="obj" /> to.</typeparam>
        /// <param name="obj">The object to attempt to cast.</param>
        /// <returns><paramref name="obj" /> as a <typeparamref name="T" />, if possible; if not, default(T).</returns>

        // ReSharper disable once MergeConditionalExpression
        [CanBeNull]
        public static T CastOrDefault <T> (this object obj) => obj is T ofType ? ofType : default! ;

        #endregion Casting

        #region Stringify (for debugging)

        private const int MaximumNumberOfRecursiveCalls = 16 ;

        /// <summary>
        ///     Transforms an object into a string representation that can be used to represent it's value in an
        ///     exception message. When the value is a null reference, the string "null" will be returned, when
        ///     the specified value is a string or a char, it will be surrounded with single quotes.
        /// </summary>
        /// <param name="value">The value to be transformed.</param>
        /// <returns>A string representation of the supplied <paramref name="value" />.</returns>
        [NotNull]
        public static string Stringify (this object? value)
        {
            string StringifyInternal (object? val, int maximumNumberOfRecursiveCalls)
            {
                if (val == null)
                    return "null" ;

                if (maximumNumberOfRecursiveCalls < 0)
                    throw new InvalidOperationException () ;

                if (val is string || val is char)
                    return "'" + value + "'" ;

                if (val is IEnumerable collection)
                    return StringifyCollection (collection, maximumNumberOfRecursiveCalls) ;

                return val.ToString () ;
            }

            string StringifyCollection (IEnumerable collection, int maximumNumberOfRecursiveCalls)
            {
                var stringifiedElements = new List <string> () ;

                // ReSharper disable once PossibleNullReferenceException
                foreach (object o in collection)
                {
                    // Recursive call to StringifyInternal.
                    string stringifiedElement = StringifyInternal (o, maximumNumberOfRecursiveCalls - 1) ;

                    stringifiedElements.Add (stringifiedElement) ;
                }

                return "{" + string.Join (",", stringifiedElements.ToArray ()) + "}" ;
            }

            try
            {
                return StringifyInternal (value, ας_System.MaximumNumberOfRecursiveCalls) ;
            }
            catch (InvalidOperationException)
            {
                // Stack overflow prevented. We cannot build a string representation of the supplied object.
                // We return the default representation of the object.

                // ReSharper disable once PossibleNullReferenceException
                return value?.ToString () ?? "<null>" ;
            }
        }

        #endregion
    }
}
