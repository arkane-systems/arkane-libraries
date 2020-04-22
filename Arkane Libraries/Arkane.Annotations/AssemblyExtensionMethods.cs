#region header

// Arkane.Annotations - AssemblyExtensionMethods.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 8:01 PM

#endregion

#region using

using ArkaneSystems.Arkane.Annotations ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    /// <summary>
    ///     Extension methods for reading <see cref="Assembly" /> attributes.
    /// </summary>
    [PublicAPI]

    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static class αο_Annotations
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        ///     Get the author for the specified assembly, as applied by the <see cref="AuthorAttribute"/>.
        /// </summary>
        /// <param name="this">The assembly to examine.</param>
        /// <returns>A tuple of the author name and e-mail address for the assembly.</returns>
        public static (string author, string emailAddress) GetAuthor ([JetBrains.Annotations.NotNull] [Required]
                                                                      this Assembly @this)

        {
            AuthorAttribute attr = @this.GetCustomAttribute <AuthorAttribute> () ??
                                   new AuthorAttribute ("Unknown", "noone@example.com") ;
            return (attr.Name, attr.EmailAddress) ;
        }

        /// <summary>
        ///     Get the URI for the documentation for the specified assembly, as applied by the <see cref="DocumentationAttribute"/>.
        /// </summary>
        /// <param name="this">The assembly to examine.</param>
        /// <returns>The URI for the documentation for the assembly.</returns>
        [CanBeNull]
        public static Uri GetDocumentation ([JetBrains.Annotations.NotNull] [Required]
                                            this Assembly @this) =>
            @this.GetCustomAttribute <DocumentationAttribute> ()?.Location ;

        /// <summary>
        ///     Get the build stamp for the specified assembly, as applied by the <see cref="AddGitStampAttribute" />.
        /// </summary>
        /// <param name="this">The assembly to examine.</param>
        /// <returns>The trademark information for the assembly.</returns>
        [JetBrains.Annotations.NotNull]
        public static string GetBuildStamp ([JetBrains.Annotations.NotNull] [Required]
                                            this Assembly @this)
            => (@this.GetCustomAttribute <AssemblyGitVersionAttribute> () ??
                new AssemblyGitVersionAttribute (string.Empty)).GitVersion ;
    }
}
