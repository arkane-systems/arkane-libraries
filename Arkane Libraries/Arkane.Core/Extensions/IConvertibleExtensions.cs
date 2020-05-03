#region header

// Arkane.Core - IConvertibleExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 2:50 PM

#endregion

#region using

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
        ///     Returns an object of the specified type and whose value is equivalent to the specified object;
        ///     throwing an exception if conversion fails.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The object converted to a new type.</returns>
        /// <exception cref="InvalidCastException">This conversion is not supported.</exception>
        /// <exception cref="OverflowException">
        ///     This object represents a number that is out of the range of
        ///     <typeparamref name="T" />.
        /// </exception>
        [CLSCompliant (false)]
        public static T ConvertTo <T> (this IConvertible obj) => (T) Convert.ChangeType (obj, typeof (T)) ;

        /// <summary>
        ///     Returns an object of the specified type and whose value is equivalent to the specified object;
        ///     returning default(T) if conversion fails.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The object converted to a new type.</returns>
        /// <exception cref="Exception">An unexpected exception occurred.</exception>
        [CLSCompliant (false)]
        public static T ConvertToOrDefault <T> (this IConvertible obj)
        {
            try
            {
                return obj.ConvertTo <T> () ;
            }

            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                if (ex is InvalidCastException ||
                    ex is FormatException      ||
                    ex is OverflowException    ||
                    ex is ArgumentNullException)
                    return default! ;

                // ReSharper disable once ThrowingSystemException
                throw ;
            }
        }

        /// <summary>
        ///     Returns an object of the specified type and whose value is equivalent to the specified object;
        ///     returning default(T) and the applicable exception object if conversion fails.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <param name="exception">The exception describing the failure, if conversion fails.</param>
        /// <returns>The object converted to a new type.</returns>
        /// <exception cref="Exception">An unexpected exception occurred.</exception>
        [CLSCompliant (false)]
        public static T ConvertToOrDefault <T> (this IConvertible obj, out Exception? exception)
        {
            try
            {
                exception = null ;
                return obj.ConvertTo <T> () ;
            }

            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                if (ex is InvalidCastException ||
                    ex is FormatException      ||
                    ex is OverflowException    ||
                    ex is ArgumentNullException)
                {
                    exception = ex ;
                    return default! ;
                }

                // ReSharper disable once ThrowingSystemException
                throw ;
            }
        }

        /// <summary>
        ///     Returns an object of the specified type and whose value is equivalent to the specified object;
        ///     returning default(T) if conversion fails.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <param name="other">The default value to return if conversion fails.</param>
        /// <returns>The object converted to a new type.</returns>
        /// <exception cref="Exception">An unexpected exception occurred.</exception>
        [CLSCompliant (false)]
        public static T ConvertToOrOther <T> (this IConvertible obj, T other)
        {
            try
            {
                return obj.ConvertTo <T> () ;
            }

            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                if (ex is InvalidCastException ||
                    ex is FormatException      ||
                    ex is OverflowException    ||
                    ex is ArgumentNullException)
                    return other ;

                // ReSharper disable once ThrowingSystemException
                throw ;
            }
        }

        /// <summary>
        ///     Returns an object of the specified type and whose value is equivalent to the specified object;
        ///     returning a specified default and the applicable exception object if conversion fails.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <param name="other">The default value to return if conversion fails.</param>
        /// <param name="exception">The exception describing the failure, if conversion fails.</param>
        /// <returns>The object converted to a new type.</returns>
        /// <exception cref="Exception">An unexpected exception occurred.</exception>
        [CLSCompliant (false)]
        public static T ConvertToOrOther <T> (this IConvertible obj, T other, out Exception? exception)
        {
            try
            {
                exception = null ;
                return obj.ConvertTo <T> () ;
            }

            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                if (ex is InvalidCastException ||
                    ex is FormatException      ||
                    ex is OverflowException    ||
                    ex is ArgumentNullException)
                {
                    exception = ex ;
                    return other ;
                }

                // ReSharper disable once ThrowingSystemException
                throw ;
            }
        }
    }
}
