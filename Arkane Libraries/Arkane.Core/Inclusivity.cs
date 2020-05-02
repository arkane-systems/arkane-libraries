#region header

// Arkane.Core - Inclusivity.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 9:42 AM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     An enumeration to define inclusivity of bounds when performing range-checks.
    /// </summary>
    [PublicAPI]
    public enum Inclusivity
    {
        /// <summary>
        ///     Both the lower bound and upper bound are included within the range.
        /// </summary>
        /// <remarks>
        ///     This is the default.
        /// </remarks>
        Inclusive,

        /// <summary>
        ///     Neither the lower bound nor the upper bound are included within the range.
        /// </summary>
        Exclusive,

        /// <summary>
        ///     The lower bound is included within the range; the upper bound is not.
        /// </summary>
        /// <remark>
        ///     This is the behavior exhibited by both Python and Common Lisp.
        /// </remark>
        InclusiveLowerOnly,

        /// <summary>
        ///     The upper bound is included within the range; the lower bound is not.
        /// </summary>
        InclusiveUpperOnly
    }
}
