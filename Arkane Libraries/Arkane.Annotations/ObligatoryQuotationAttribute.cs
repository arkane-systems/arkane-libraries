#region header

// Arkane.Annotations - ObligatoryQuotationAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 7:53 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     It had to be said, so I did.
    /// </summary>
    /// <comment>
    ///     Why? Why not just in the comments? Well, it's because this sort of thing is useful insight
    ///     into the mind of the developer, and - assuming we're not obfuscating - that should be
    ///     available right there in the assembly as well as the source.
    /// </comment>
    [PublicAPI]
    public sealed class ObligatoryQuotationAttribute : Attribute
    {
        /// <summary>
        ///     Creates an ObligatoryQuotationAttribute, setting the quotation, source, and (optional) citation.
        /// </summary>
        /// <param name="quotation">The text of the obligatory quotation.</param>
        /// <param name="source">The original source of the obligatory quotation.</param>
        /// <param name="citation">
        ///     A citation for the original quotation, preferably but not necessarily in URI format. (May be
        ///     null.)
        /// </param>
        public ObligatoryQuotationAttribute ([JetBrains.Annotations.NotNull] [Required]
                                             string quotation,
                                             [JetBrains.Annotations.NotNull] [Required]
                                             string source,
                                             [CanBeNull] string citation)
        {
            this.Quotation = quotation ;
            this.Source    = source ;
            this.Citation  = citation ;
        }

        /// <summary>
        ///     The text of the obligatory quotation.
        /// </summary>
        public string Quotation { get ; }

        /// <summary>
        ///     The original source of the obligatory quotation.
        /// </summary>
        public string Source { get ; }

        /// <summary>
        ///     A citation for the original quotation, preferably but not necessarily in URI format.
        /// </summary>
        /// <remarks>
        ///     May be null.
        /// </remarks>
        public string Citation { get ; }
    }
}
