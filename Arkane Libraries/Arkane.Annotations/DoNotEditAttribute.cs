#region header

// Arkane.Annotations - DoNotEditAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-20 2:01 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     Indicates that the attached code should not be edited without consulting the specified person,
    ///     for the given reason.
    /// </summary>
    [AttributeUsage (AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    [PublicAPI]
    public sealed class DoNotEditAttribute : Attribute
    {
        /// <summary>
        ///     Create a DoNotEditAttribute, setting the name of the person ordering this code off-limits, and the
        ///     reason why it is.
        /// </summary>
        /// <param name="byOrderOf">The name (or other identifier) of the person setting this code off-limits.</param>
        /// <param name="reason">The reason for the code being set off-limits.</param>
        public DoNotEditAttribute ([JetBrains.Annotations.NotNull] [Required]
                                   string byOrderOf,
                                   [EnumDataType (typeof (Uneditability))]
                                   Uneditability
                                       reason)
        {
            this.ByOrderOf = byOrderOf ;
            this.Reason    = reason ;
        }

        /// <summary>
        ///     The name (or other identifier) of the person setting this code off-limits.
        /// </summary>
        public string ByOrderOf { get ; }

        /// <summary>
        ///     The reason for the code being set off-limits.
        /// </summary>
        public Uneditability Reason { get ; }
    }
}
