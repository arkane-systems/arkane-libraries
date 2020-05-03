#region header

// Arkane.Core - UnsealableConstraint.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:03 PM

#endregion

#region using

using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Constraints ;
using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Constraints
{
    /// <summary>
    ///     A constraint which prevents subclasses of the class which it has been applied to from being sealed.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    [MulticastAttributeUsage (MulticastTargets.Class)]
    [PublicAPI]
    public sealed class UnsealableConstraintAttribute : ReferentialConstraint
    {
        /// <inheritdoc />
        public override void ValidateCode (object target, Assembly assembly)
        {
            TypeInfo targetType = ((Type) target).GetTypeInfo () ;

            List <TypeInfo> sealedSubclasses =
                assembly.DefinedTypes.Where (t => t.IsSealed).Where (t => targetType.IsAssignableFrom (t)).ToList () ;

            sealedSubclasses.ForEach (
                                      c =>
                                          Message.Write (c,
                                                         SeverityType.Error,
                                                         "3001",
                                                         Resources
                                                            .UnsealableConstraintAttribute_ValidateCode_SubclassesCannotBeSealed,
                                                         c.FullName,
                                                         targetType.FullName)) ;
        }
    }
}
