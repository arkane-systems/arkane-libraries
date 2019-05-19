#region header

// Arkane.Annotations - AuthorAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 6:18 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     The author of the marked code.
    /// </summary>
    /// <remarks>
    ///     It is suggested that this be applied to the assembly for the overall author, and then
    ///     deviations from this be marked out by individual usages.
    /// </remarks>
    [PublicAPI]
    public sealed class AuthorAttribute : Attribute
    {
        /// <summary>
        ///     Creates an AuthorAttribute, setting the name and e-mail address of the marked code's author.
        /// </summary>
        /// <param name="name">The name of the code's author.</param>
        /// <param name="email">An e-mail address for the code's author.</param>
        public AuthorAttribute ([JetBrains.Annotations.NotNull] [Required]
                                string name,
                                [JetBrains.Annotations.NotNull] [Required] [EmailAddress]
                                string email)
        {
            this.Name         = name ;
            this.EmailAddress = email ;
        }

        /// <summary>
        ///     The name of the code's author.
        /// </summary>
        public string Name { get ; }

        /// <summary>
        ///     An e-mail address for the code's author.
        /// </summary>
        public string EmailAddress { get ; }

        /// <inheritdoc />
        public override string ToString () => $"Author: {this.Name} <{this.EmailAddress}>" ;
    }
}
