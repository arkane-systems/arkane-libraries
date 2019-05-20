#region header

// Arkane.Annotations - LegacyWrapperAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 7:47 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     Indicates that the following class or method exists to insulate one from the liveliest awfulness of
    ///     the legacy code it's wrapped around.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Struct)]
    [PublicAPI]
    public sealed class LegacyWrapperAttribute : Attribute
    {
        /// <summary>
        ///     Creates a LegacyWrapperAttribute, setting the programmer's comments on the code.
        /// </summary>
        /// <param name="comments">Comments on exactly why this legacy code is so damned legacy, anyway.</param>
        public LegacyWrapperAttribute ([JetBrains.Annotations.NotNull] [Required]
                                       string comments) =>
            this.Comments = comments ;

        /// <summary>
        ///     Comments on exactly why this legacy code is so damned legacy, anyway.
        /// </summary>
        public string Comments { get ; }
    }
}
