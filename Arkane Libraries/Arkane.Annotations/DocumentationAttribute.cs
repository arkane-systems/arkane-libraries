#region header

// Arkane.Annotations - DocumentationAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 3:02 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     The location of the documentation for this assembly.
    /// </summary>
    [PublicAPI]
    [AttributeUsage (AttributeTargets.Assembly)]
    public sealed class DocumentationAttribute : Attribute
    {
        /// <summary>
        ///     Creates a <see cref="DocumentationAttribute" />, setting the URL of the marked
        ///     assembly's documentation.
        /// </summary>
        /// <param name="location">A URL at which documentation for the assembly can be found.</param>
        public DocumentationAttribute ([JetBrains.Annotations.NotNull] [Required] [Url]
                                       string location) =>
            this.Location = new Uri (location) ;

        /// <summary>
        ///     A URI giving the location of the documentation for this assembly.
        /// </summary>
        public Uri Location { get ; }

        /// <inheritdoc />
        public override string ToString () => $"Documentation located at: ${this.Location}" ;
    }
}
