#region header

// Arkane.Annotations - AssemblyGitVersionAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 7:18 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     An attribute containing the automatically-stamped Git version (commit details) of the assembly.
    /// </summary>
    [PublicAPI]
    [AttributeUsage (AttributeTargets.Assembly)]
    public sealed class AssemblyGitVersionAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new <see cref="AssemblyGitVersionAttribute" />.
        /// </summary>
        /// <param name="gitVersion">The automatically-stamped Git version (commit details) of the assembly.</param>
        public AssemblyGitVersionAttribute (string gitVersion) => this.GitVersion = gitVersion ;

        /// <summary>
        ///     The automatically-stamped Git version (commit details) of the assembly.
        /// </summary>
        public string GitVersion { get ; }
    }
}
