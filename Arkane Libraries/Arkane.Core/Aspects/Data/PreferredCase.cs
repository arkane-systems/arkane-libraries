using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations ;

namespace ArkaneSystems.Arkane.Aspects.Data
{
    /// <summary>
    ///     The capitalization for a <see cref="StringManipulationAttribute" /> to force a string into.
    /// </summary>
    [PublicAPI]
    public enum PreferredCase
    {
        /// <summary>
        ///     The string as is.
        /// </summary>
        Unchanged,

        /// <summary>
        ///     The string to uppercase.
        /// </summary>
        Uppercase,

        /// <summary>
        ///     The string to lowercase.
        /// </summary>
        Lowercase,

        /// <summary>
        ///     Capitalize only the first letter of the string.
        /// </summary>
        Sentence

        // TODO: Add Proper Case, PascalCase, camelCase
    }
}
