#region header

// Arkane.Core - StringExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 7:40 PM

#endregion

#region using

using System.Collections.Generic ;
using System.Diagnostics.CodeAnalysis ;
using System.Globalization ;
using System.IO ;
using System.Linq ;
using System.Text ;
using System.Text.RegularExpressions ;
using System.Xml ;
using System.Xml.Linq ;
using System.Xml.XPath ;

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
        ///     Returns <see cref="String.Empty" /> if the string is null,
        ///     otherwise, returns the original string.
        /// </summary>
        /// <param name="this"></param>
        /// <returns><see cref="string.Empty" /> if the string is null, otherwise, returns the original string</returns>
        public static string AsEmptyIfNull (this string? @this) => @this ?? string.Empty ;

        /// <summary>
        ///     Capitalize the first character of a string.
        /// </summary>
        /// <param name="this">The string to capitalize.</param>
        /// <returns>The capitalized string.</returns>
        public static string Capitalize (
            this string @this)
        {
            if (@this.Length <= 1)
                return @this.ToUpper () ;

            return @this.Substring (0, 1).ToUpper () + @this.Substring (1) ;
        }

        /// <summary>
        ///     Expands tabs by replacing them with spaces.
        /// </summary>
        /// <param name="this">The string to expand.</param>
        /// <param name="tabSize">The number of spaces corresponding to each tab (default: 4).</param>
        /// <returns>The expanded string.</returns>
        public static string ExpandTabs (
            this       string @this,
            [Positive] int    tabSize = 4)
        {
            string tabReplacement = " ".Repeat (tabSize) ;

            return @this.Replace ("\t", tabReplacement) ;
        }

        /// <summary>
        ///     Compute the Levenshtein distance between two strings.
        /// </summary>
        /// <param name="this">The first string to compare.</param>
        /// <param name="that">The second string to compare.</param>
        /// <returns>The Levenshtein distance between this and that.</returns>
        /// <remarks>
        ///     http://en.wikipedia.org/wiki/Levenshtein_distance
        /// </remarks>
        [Pure]
        [return: Positive]
        [SuppressMessage ("Microsoft.Performance",
                          "CA1814:PreferJaggedArraysOverMultidimensional",
                          MessageId = "Body")]
        public static int GetLevenshteinDistance (
            this string @this,
            string      that)
        {
            var matrix = new int[@this.Length + 1, that.Length + 1] ;
            for (var x = 0; x <= @this.Length; ++x)
                matrix[x, 0] = x ;
            for (var x = 0; x <= that.Length; ++x)
                matrix[0, x] = x ;

            for (var x = 1; x <= @this.Length; ++x)
            {
                for (var y = 1; y <= that.Length; ++y)
                {
                    int cost = @this[x - 1] == that[y - 1] ? 0 : 1 ;
                    matrix[x, y] =
                        new[] {matrix[x - 1, y] + 1, matrix[x, y - 1] + 1, matrix[x - 1, y - 1] + cost}.Min () ;
                    if ((x > 1) && (y > 1) && (@this[x - 1] == that[y - 2]) && (@this[x - 2] == that[y - 1]))
                        matrix[x, y] = new[] {matrix[x, y], matrix[x - 2, y - 2] + cost}.Min () ;
                }
            }

            return matrix[@this.Length, that.Length] ;
        }

        /// <summary>
        ///     Returns the number of times a pattern occurs within the string.
        /// </summary>
        /// <param name="this">The string to examine.</param>
        /// <param name="that">The pattern (regex) to search for.</param>
        /// <returns>The number of times the pattern occurs.</returns>
        [Pure]
        [return: Positive]
        public static int GetOccurrenceCount (this           string? @this,
                                              [RegexPattern] string  that)
            => @this.HasValue () ? new Regex (that).Matches (@this).Count : 0 ;

        /// <summary>
        ///     Test whether a string has a value; i.e., is not null, empty, or composed solely of whitespace.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>
        ///     True if the string is not null, not empty, and not composed entirely of whitespace
        ///     characters; false otherwise.
        /// </returns>
        [Pure]
        [ContractAnnotation ("null=>false")]
        public static bool HasNonWhitespaceValue (this string? value)
            => string.IsNullOrWhiteSpace (value) == false ;

        /// <summary>
        ///     Test whether a string has a value; i.e., is not null, or empty.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>True if the string is neither null or empty; false otherwise.</returns>
        [Pure]
        [ContractAnnotation ("null=>false")]
        public static bool HasValue (this string? value) => string.IsNullOrEmpty (value) == false ;

        /// <summary>
        ///     Compare two strings for equality.
        /// </summary>
        /// <param name="this">The first string to compare.</param>
        /// <param name="that">The second string to compare.</param>
        /// <param name="caseSensitive">If true, compare case-sensitively.</param>
        /// <param name="currentCulture">
        ///     If true, compare using the current culture rather
        ///     than the invariant culture.
        /// </param>
        /// <returns>True if the strings are equal; false otherwise.</returns>
        public static bool IsEqualTo (
            this string @this,
            string      that,
            bool        caseSensitive  = false,
            bool        currentCulture = false)
        {
            CompareOptions options = caseSensitive ? CompareOptions.None : CompareOptions.IgnoreCase ;
            CultureInfo    culture = currentCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture ;

            return string.Compare (@this, that, culture, options) == 0 ;
        }

        /// <summary>
        ///     Removes all non-alphanumeric characters from the given string. The only characters preserved will be A-Z, a-z and
        ///     0-9.
        /// </summary>
        /// <param name="value">The string whose text will be replaced</param>
        public static string RemoveNonAlphaNumeric (this string value)
        {
            MatchCollection col = Regex.Matches (value, "[A-Za-z0-9]+") ;
            var             sb  = new StringBuilder () ;
            foreach (Match m in col)
                sb.Append (m.Value) ;

            return sb.ToString () ;
        }

        /// <summary>
        ///     Repeats this string a given number of times.
        /// </summary>
        /// <param name="this">The string to repeat.</param>
        /// <param name="times">The number of times to repeat the string.</param>
        /// <returns>This string, repeated <paramref name="times" /> times.</returns>
        public static string Repeat (
            this       string @this,
            [Positive] int    times = 1)
        {
            var output = new StringBuilder (@this.Length * times) ;

            while (times-- > 0)
                output.Append (@this) ;

            return output.ToString () ;
        }

        /// <summary>
        ///     Reverses/mirrors a string.
        /// </summary>
        /// <param name="this">The string to reverse.</param>
        /// <returns>The reversed string.</returns>
        /// <remarks>
        ///     Unlike LINQ reversal, properly handles Unicode combining characters and
        ///     surrogates.
        /// </remarks>
        public static string ReverseAsString (
            this string @this)
            => string.Join ("", @this.ToGraphemeClusters ().Reverse ().ToArray ()) ;

        /// <summary>
        ///     Checks that the value of a substring at an index matches the supplied string.
        /// </summary>
        /// <param name="this">The string to inspect.</param>
        /// <param name="start">The index at which to check for the substring.</param>
        /// <param name="sub">The string to use for comparison.</param>
        /// <returns>True if string <paramref name="sub" /> is found at <paramref name="this" />[<paramref name="start" />].</returns>
        public static bool SliceEquals (
            this       string @this,
            [Positive] int    start,
            string            sub)
        {
            if (sub.Length > @this.Length - start)
                return false ;

            for (int i = start, j = 0; j < sub.Length; ++i, ++j)
            {
                if (@this[i] != sub[j])
                    return false ;
            }

            return true ;
        }

        /// <summary>
        ///     Break up a string into Unicode grapheme clusters.
        /// </summary>
        /// <param name="this">The string to break up.</param>
        /// <returns>An enumerable of the string's grapheme clusters.</returns>
        [ItemNotNull]
        public static IEnumerable <string> ToGraphemeClusters (
            this string @this)
        {
            TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator (@this) ;
            while (enumerator.MoveNext ())
                yield return (string) enumerator.Current ;
        }

        /// <summary>
        ///     Transform a string with a recognizer and a transformer function.
        /// </summary>
        /// <param name="this">The string to transform.</param>
        /// <param name="recognizer">A predicate which recognizes transformation-candidate words.</param>
        /// <param name="transformer">A predicate which transforms candidate words.</param>
        /// <returns>Returns a new string with the transformed values.</returns>
        /// <example>
        ///     <code>
        ///         var str = "White Red Blue Green Yellow Black Gray";
        ///         var achromaticColors = new[] {"White", "Black", "Gray"};
        ///         str.Transform (r => r.EqualsAnyOf (achromaticColors), t => "[" + t + "]") ;
        ///         // str == "[White] Red Blue Green Yellow [Black] [Gray]"
        ///     </code>
        /// </example>
        [JetBrains.Annotations.NotNull]
        public static string Transform (
            this string           @this,
            Predicate <string>    recognizer,
            Func <string, string> transformer)
        {
            var      sb    = new StringBuilder () ;
            string[] words = @this.Split (new string[] { }, StringSplitOptions.None) ;

            foreach (string transformed in words.Select (word => recognizer (word) ? transformer (word) : word))
                sb.AppendFormat ("{0} ", transformed) ;

            sb.Chop () ;

            return sb.ToString () ;
        }

        /// <summary>
        ///     Copies a string up to a specific number of characters, optionally adding a suffix in case of truncation.
        /// </summary>
        /// <param name="input">The string to copy.</param>
        /// <param name="maxLength">The maximum length of the string to return, excluding the suffix.</param>
        /// <param name="suffixToUseWhenTooLong">The suffix to add to the result, in case of truncation.</param>
        [JetBrains.Annotations.NotNull]
        public static string UpTo (this string? input,
                                   int          maxLength,
                                   string       suffixToUseWhenTooLong = "...")
        {
            if (string.IsNullOrEmpty (input))
                return string.Empty ;

            if (input.Length > maxLength)
                return input.Substring (0, maxLength) + suffixToUseWhenTooLong ;

            return input ;
        }

        /// <summary>
        ///     Convert a camelCase or PascalCase string to Proper Case.
        /// </summary>
        /// <param name="this">A camelCase or PascalCase string.</param>
        /// <returns>The string converted to Proper Case.</returns>
        public static string Wordify (
            this string @this)
        {
            // If there are less than two characters, just return the string.
            if (@this.Length < 2)
                return @this.ToUpper () ;

            // If string is all caps, just return the string.
            if (@this == @this.ToUpper ())
                return @this ;

            var retval = new StringBuilder (@this.Length * 2) ;

            retval.Append (@this.Substring (0, 1).ToUpper ()) ;

            foreach (char c in @this.Substring (1))
            {
                if (char.IsUpper (c))
                    retval.Append (' ') ;
                retval.Append (c) ;
            }

            return retval.ToString () ;
        }

        #region Byte array interconversion

        /// <summary>
        ///     Converts a string to a byte[], using the specified encoding. If no encoding is specified, the UTF8
        ///     encoding is used.
        /// </summary>
        /// <param name="this">The string to convert to a byte array.</param>
        /// <param name="encoding">The encoding to use to convert the string. If none is specified, UTF8 is used.</param>
        /// <returns>The converted byte array.</returns>
        [JetBrains.Annotations.NotNull]
        public static byte[] ConvertToByteArray (this string? @this, Encoding? encoding = null)
        {
            if (!@this.HasValue ())
                return new byte[0] ;

            encoding ??= Encoding.UTF8 ;

            return encoding.GetBytes (@this) ;
        }

        #endregion Byte array interconversion

        #region AsNull

        /// <summary>
        ///     Returns null if the string is null or empty,
        ///     otherwise, returns the original string.
        /// </summary>
        /// <param name="this">The original string.</param>
        /// <returns>Null if empty; otherwise, the original string.</returns>
        public static string? AsNullIfEmpty (this string? @this)
            => string.IsNullOrEmpty (@this) ? null : @this ;

        /// <summary>
        ///     Returns null if the string is null or empty, or consists entirely of white-space characters;
        ///     otherwise, returns the original string.
        /// </summary>
        /// <param name="this">The original string.</param>
        /// <returns>Null if empty or whitespace; otherwise, the original string.</returns>
        public static string? AsNullIfWhitespace (this string? @this)
            => string.IsNullOrWhiteSpace (@this) ? null : @this ;

        #endregion AsNull

        #region Ensure

        /// <summary>
        ///     Ensures that a string starts with a given prefix.
        /// </summary>
        /// <param name="this">The string value to check.</param>
        /// <param name="prefix">The prefix value to check for.</param>
        /// <returns>The string value including the prefix</returns>
        /// <example>
        ///     <code>
        ///         var extension = "txt";
        ///         var fileName = string.Concat(file.Name, extension.EnsureStartsWith("."));
        ///     </code>
        /// </example>
        public static string EnsureStartsWith (
            this string @this,
            string      prefix)
            => @this.StartsWith (prefix, StringComparison.Ordinal) ? @this : string.Concat (prefix, @this) ;

        /// <summary>
        ///     Ensures that a string ends with a given suffix.
        /// </summary>
        /// <param name="this">The string value to check.</param>
        /// <param name="suffix">The suffix value to check for.</param>
        /// <returns>The string value including the suffix</returns>
        /// <example>
        ///     <code>
        ///         var url = "http://www.example.com";
        ///         url = url.EnsureEndsWith("/"));
        ///     </code>
        /// </example>
        public static string EnsureEndsWith (
            this string @this,
            string      suffix)
            => @this.EndsWith (suffix, StringComparison.Ordinal) ? @this : string.Concat (@this, suffix) ;

        #endregion Ensure

        #region FillWith

        /// <summary>
        ///     Formats a string with a list of literal placeholders.
        /// </summary>
        /// <param name="template">The extension template</param>
        /// <param name="args">The argument list</param>
        /// <returns>The formatted string</returns>
        [Obsolete ("Use string interpolation.")]
        [StringFormatMethod ("template")]
        public static string FillWith (
            this   string   template,
            params object[] args)
            => string.Format (template, args) ;

        /// <summary>
        ///     Formats a string with a list of literal placeholders.
        /// </summary>
        /// <param name="template">The extension template</param>
        /// <param name="provider">The format provider</param>
        /// <param name="args">The argument list</param>
        /// <returns>The formatted string</returns>
        [StringFormatMethod ("template")]
        public static string FillWith (
            this string     template,
            IFormatProvider provider,
            params object[] args)
            => string.Format (provider, template, args) ;

        #endregion FillWith

        #region Filters

        /// <summary>
        ///     Eliminate all text not matching the filter regex from the input.
        /// </summary>
        /// <param name="this">The input string.</param>
        /// <param name="filter">A regex specifying what should not be filtered out.</param>
        /// <returns>The input test filtered for inclusion.</returns>
        public static string FilterIn (
            this string @this,
            string      filter)
        {
            var sb = new StringBuilder (@this.Length) ;

            MatchCollection matches = new Regex (filter).Matches (@this) ;
            foreach (Match match in matches)
                sb.Append (match.Value) ;

            return sb.ToString () ;
        }

        /// <summary>
        ///     Eliminate all text matching the filter regex from the input.
        /// </summary>
        /// <param name="this">The input string.</param>
        /// <param name="filter">A regex specifying what should be filtered out.</param>
        /// <returns>The input text filtered for removal.</returns>
        [JetBrains.Annotations.NotNull]
        public static string FilterOut (
            this string @this,
            string      filter)
            => new Regex (filter).Replace (@this, string.Empty) ;

        #endregion Filters

        #region Remove

        /// <summary>
        ///     Remove any instance of the given string from the string.
        /// </summary>
        /// <param name="this">The string to process.</param>
        /// <param name="toRemove">The string(s) to remove.</param>
        /// <returns>The string without the removed instances.</returns>
        public static string Remove (
            this   string   @this,
            params string[] toRemove)
            => toRemove.Aggregate (@this, (current, c) => current.Replace (c, string.Empty)) ;

        /// <summary>
        ///     Remove any instances of the given character(s) from the string.
        /// </summary>
        /// <param name="this">The string to process.</param>
        /// <param name="toRemove">The character(s) to remove.</param>
        /// <returns>The string with the specified characters removed.</returns>
        public static string Remove (
            this   string @this,
            params char[] toRemove)
            => toRemove.Aggregate (@this, (current, c) => current.Remove (c.ToString ())) ;

        /// <summary>
        ///     Perform a disemvowelment on a string.
        /// </summary>
        /// <param name="this">The string to process.</param>
        /// <returns>The string with the vowels removed.</returns>
        [JetBrains.Annotations.NotNull]
        public static string Disemvowel (
            this string @this)
            => @this.Remove ('a', 'e', 'i', 'o', 'u') ;

        #endregion Remove

        #region Sentinels

        /// <summary>
        ///     Gets the trimmed string after the given sentinel string.  If the sentinel string
        ///     is not found, returns the empty string.
        /// </summary>
        /// <param name="this">The string to look in.</param>
        /// <param name="sentinel">The sentinel string.</param>
        /// <returns>The trimmed string after the given sentinel string.</returns>
        public static string AfterSentinel (
            this string @this,
            string      sentinel)
        {
            int lastSentinel = @this.LastIndexOf (sentinel, StringComparison.Ordinal) ;

            if (lastSentinel == -1)
                return string.Empty ;

            lastSentinel += sentinel.Length ;

            return @this.Substring (lastSentinel).Trim () ;
        }

        /// <summary>
        ///     Gets the trimmed string before the given sentinel string. If the sentinel
        ///     string is not found, gets the whole string.
        /// </summary>
        /// <param name="this">The string to look in.</param>
        /// <param name="sentinel">The sentinel string.</param>
        /// <returns>The trimmed string before the given sentinel string.</returns>
        public static string BeforeSentinel (
            this string @this,
            string      sentinel)
        {
            int sentinelStart = @this.IndexOf (sentinel, StringComparison.Ordinal) ;
            return (sentinelStart == -1 ? @this : @this.Left (sentinelStart)).Trim () ;
        }

        /// <summary>
        ///     Gets the trimmed string in between the two sentinel strings.  If the first sentinel
        ///     string is not found, returns the empty string.  If the second sentinel string is not
        ///     found, returns the string up until the end.
        /// </summary>
        /// <param name="this">The string to look in.</param>
        /// <param name="leadingSentinel">The sentinel string defining the start of the text.</param>
        /// <param name="trailingSentinel">The sentinel string defining the end of the text.</param>
        /// <returns>The trimmed string in between the sentinel strings.</returns>
        /// <remarks>The sentinel strings cannot be identical.</remarks>
        public static string BetweenSentinels (
            this string @this,
            string      leadingSentinel,
            string      trailingSentinel)
        {
            if (leadingSentinel == trailingSentinel)
                throw new ArgumentException (ArkaneSystems.Arkane.Properties.Resources
                                                          .ας_System_BetweenSentinels_SentinelsCannotBeIdentical) ;

            string post = @this.AfterSentinel (leadingSentinel).AsEmptyIfNull () ;

            return post != string.Empty ? post.BeforeSentinel (trailingSentinel) : string.Empty ;
        }

        #endregion Sentinels

        #region Substring helpers

        /// <summary>
        ///     Return a substring of the string <paramref name="s" />, starting at the left, of length <paramref name="length" />.
        /// </summary>
        /// <param name="s">The string to return a substring of.</param>
        /// <param name="length">The length of the desired substring.</param>
        /// <returns>A substring of length <paramref name="length" />, taken from the LHS of string <paramref name="s" />.</returns>
        /// <exception cref="ArgumentException">Substring length must be less than the length of the string.</exception>
        public static string Left (this       string s,
                                   [Positive] int    length)
        {
            if (s.Length >= length)
                return s.Substring (0, length) ;

            throw new ArgumentException (
                                         ArkaneSystems.Arkane.Properties.Resources.ας_System_Left_LengthMustBeLess) ;
        }

        /// <summary>
        ///     Return a substring of the string <paramref name="s" />, ending at the right, of length <paramref name="length" />.
        /// </summary>
        /// <param name="s">The string to return a substring of.</param>
        /// <param name="length">The length of the desired substring.</param>
        /// <returns>A substring of length <paramref name="length" />, taken from the RHS of string <paramref name="s" />.</returns>
        /// <exception cref="ArgumentException">Substring length must be less than the length of the string.</exception>
        public static string Right (
            this       string s,
            [Positive] int    length)
        {
            if (s.Length >= length)
                return s.Substring (s.Length - length, length) ;

            throw new ArgumentException (
                                         ArkaneSystems.Arkane.Properties.Resources.ας_System_Left_LengthMustBeLess) ;
        }

        /// <summary>
        ///     Returns a substring starting at the specified start index and ending and the specified end
        ///     index.
        /// </summary>
        /// <param name="s">The string to retrieve the substring from.</param>
        /// <param name="startIndex">The specified start index for the substring.</param>
        /// <param name="endIndex">The specified end index for the substring.</param>
        /// <returns>
        ///     A substring starting at the specified start index and ending at the specified end
        ///     index.
        /// </returns>
        public static string SubstringByRange (
            this       string s,
            [Positive] int    startIndex,
            [Positive] int    endIndex)
        {
            if (startIndex > s.Length)
                throw new ArgumentOutOfRangeException (nameof (startIndex)) ;

            if (startIndex > endIndex)
                throw new ArgumentOutOfRangeException (nameof (startIndex),
                                                       ArkaneSystems.Arkane.Properties.Resources
                                                                    .ας_System_SubstringByRange_EndIndexMustBePastStartIndex) ;
            if (startIndex > s.Length - startIndex - endIndex)
                throw new ArgumentException (nameof (startIndex)) ;

            return s.Substring (startIndex, endIndex - startIndex) ;
        }

        #endregion Substring helpers

        #region ToEnum

        /// <summary>
        ///     Parses a string into an enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">String value to parse.</param>
        /// <returns>The enum corresponding to the string.</returns>
        [JetBrains.Annotations.NotNull]
        public static T ParseAsEnum <T> (
            [Required] this string value) where T : Enum => value.ParseAsEnum <T> (false) ;

        /// <summary>
        ///     Parses a string into an enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">String value to parse.</param>
        /// <param name="ignoreCase">Ignore case in parsing.</param>
        /// <returns>The enum corresponding to the string.</returns>
        [JetBrains.Annotations.NotNull]
        public static T ParseAsEnum <T> (
            [Required] this string value,
            bool                   ignoreCase) where T : Enum
        {
            value = value.Trim () ;
            Type t = typeof (T) ;

            return (T) Enum.Parse (t, value, ignoreCase) ;
        }

        #endregion ToEnum

        #region ToXml

        /// <summary>
        ///     Loads the string into a LINQ to XML XDocument
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The XML document object model (XDocument)</returns>
        public static XDocument ToXDocument ([Required] this string xml)
            => XDocument.Parse (xml) ;

        /// <summary>
        ///     Loads the string into a XML DOM object (XmlDocument)
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The XML document object model (XmlDocument)</returns>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
        public static XmlDocument ToXmlDocument ([Required] this string xml)
        {
            var document = new XmlDocument () ;
            document.LoadXml (xml) ;
            return document ;
        }

        /// <summary>
        ///     Loads the string into a XML XPath DOM (XPathDocument)
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The XML XPath document object model (XPathNavigator)</returns>
        /// <exception cref="XmlException">
        ///     An error was encountered in the XML data. The
        ///     <see cref="T:System.Xml.XPath.XPathDocument" /> remains empty.
        /// </exception>
        public static XPathNavigator ToXPathDocument ([Required] this string xml)
        {
            var document = new XPathDocument (new StringReader (xml)) ;
            return document.CreateNavigator () ;
        }

        #endregion ToXml
    }
}
