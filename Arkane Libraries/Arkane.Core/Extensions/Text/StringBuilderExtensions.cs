#region header

// Arkane.Core - StringBuilderExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 10:17 AM

#endregion

#region using

#endregion

// ReSharper disable once CheckNamespace
using System.Collections.Generic ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

// ReSharper disable once CheckNamespace
namespace System.Text
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static partial class ας_System_Text
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        ///     Appends each string in an <see cref="IEnumerable{T}" /> of <see cref="String" /> to this instance.
        /// </summary>
        /// <param name="this">The StringBuilder to append to.</param>
        /// <param name="strings">The strings to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder Append (
            this StringBuilder @this,
            IEnumerable <string> strings)
        {
            foreach (string s in strings)
                @this.Append (s) ;

            return @this ;
        }

        /// <summary>
        ///     Appends the string returned by processing a composite format string, which contains zero or more
        ///     format items, to this instance, followed by the default line terminator. Each format item is
        ///     replaced by the string representation of a corresponding argument in a parameter array.
        /// </summary>
        /// <param name="this">The StringBuilder to append to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        [StringFormatMethod ("format")]
        public static StringBuilder AppendLine (
            this StringBuilder @this,
            string format,
            params object[] args)
        {
            @this.AppendFormat (format, args) ;
            @this.AppendLine () ;

            return @this ;
        }

        /// <summary>
        ///     Appends the string representation of a specified object to this instance, followed by the
        ///     default line terminator.
        /// </summary>
        /// <param name="this">The StringBuilder to append to.</param>
        /// <param name="obj">The object to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder AppendLine (
            this StringBuilder @this,
            object? obj)
        {
            @this.Append (obj) ;
            @this.AppendLine () ;

            return @this ;
        }

        /// <summary>
        ///     Append a specified number of blank lines to the <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="this">The StringBuilder to append to.</param>
        /// <param name="lines">
        ///     The number of blank lines to append. (Zero will append a single
        ///     <see cref="Environment.NewLine" />.) Defaults to one.
        /// </param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder AppendLines (
            this StringBuilder @this,
            [Positive] int lines = 1)
        {
            @this.Append (Environment.NewLine.Repeat (lines + 1)) ;

            return @this ;
        }

        /// <summary>
        ///     Remove <paramref name="chopSize" /> characters from the end of the StringBuilder.
        /// </summary>
        /// <param name="this">The StringBuilder to chop.</param>
        /// <param name="chopSize">The number of characters (default 1) to chop.</param>
        /// <returns>A reference to this instance after the chop operation has completed.</returns>
        public static StringBuilder Chop (
            this StringBuilder @this,
            [Positive] int chopSize = 1)
        {
            // TODO: replace this by a contract aspect
            if (chopSize >= @this.Length)
                throw new ArgumentOutOfRangeException (nameof (chopSize), ArkaneSystems.Arkane.Properties.Resources.ας_System_Text_Chop_ChopLongerThanString) ;

            @this.Remove (@this.Length - chopSize, chopSize) ;

            return @this ;
        }
    }
}
