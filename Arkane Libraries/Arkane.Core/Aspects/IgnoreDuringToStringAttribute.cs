#region header

// Arkane.Core - IgnoreDuringToStringAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:43 AM

#endregion

#region using

using JetBrains.Annotations ;

using PostSharp.Constraints ;
using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     The target field or property is excluded from the generation of code in ToString.
    /// </summary>
    [MulticastAttributeUsage (MulticastTargets.Field | MulticastTargets.Property)]
    [PublicAPI]
    public class IgnoreDuringToStringAttribute : ScalarConstraint
    {
        /// <inheritdoc />
        public override void ValidateCode (object target) =>
            PostSharpHelpers.RequireArkaneAspectsWeaver (this.GetType (),
                                                         target,
                                                         "IgnoreDuringToStringAttribute: Skip this field or property from the generation of code in ToString().") ;
    }
}
