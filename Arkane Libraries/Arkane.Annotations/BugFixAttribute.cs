#region header

// Arkane.Annotations - BugFixAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-20 1:55 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     An attribute identifying a program element to which a b-u-g-fix has been applied, relating back to a
    ///     particular case in the Arkane Systems internal b-u-g-tracking system.
    /// </summary>
    [AttributeUsage (AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    [PublicAPI]
    public sealed class BugFixAttribute : Attribute
    {
        /// <summary>
        ///     Creates a BugFixCaseAttribute, setting the case number in the Arkane Systems internal b-u-g-tracking
        ///     system.
        /// </summary>
        /// <param name="caseNumber">The case number.</param>
        public BugFixAttribute ([StrictlyPositive] int caseNumber) => this.CaseNumber = caseNumber ;

        /// <summary>
        ///     The case number of this particular b-u-g-fix in the Arkane Systems internal b-u-g-tracking system.
        /// </summary>
        public int CaseNumber { get ; }

        /// <summary>
        ///     Embedded comments regarding the b-u-g-fix.
        /// </summary>
        public int Comments { get ; set ; }
    }
}
