#region header

// Arkane.Annotations - SourceLanguageAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 7:06 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     An attribute identifying the original programming language in which an assembly was developed.
    /// </summary>
    [AttributeUsage (AttributeTargets.Assembly)]
    [PublicAPI]
    public sealed class SourceLanguageAttribute : Attribute
    {
        /// <summary>
        ///     Creates a SourceLanguageAttribute, setting the programming language.
        /// </summary>
        /// <param name="language">The programming language this assembly was written in.</param>
        public SourceLanguageAttribute ([EnumDataType (typeof (ProgrammingLanguages))]
                                        ProgrammingLanguages language =
                                            ProgrammingLanguages.CSharp) =>
            this.Language = language ;

        /// <summary>
        ///     The programming language this assembly was written in.
        /// </summary>
        public ProgrammingLanguages Language { get ; }

        /// <summary>
        ///     Has this assembly been rewritten post-compilation by PostSharp?
        /// </summary>
        public bool RewrittenByPostSharp { get ; set ; }

        /// <summary>
        ///     Has this assembly been obfuscated post-compilation?
        /// </summary>
        public bool Obfuscated { get ; set ; }

        /// <summary>
        ///     How has this assembly been modified post-compilation?
        /// </summary>
        public string PostCompilationModifications { get ; set ; } = "None." ;
    }
}
