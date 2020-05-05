#region header

// Arkane.Core - DeepSerializableAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:54 AM

#endregion

#region using

using JetBrains.Annotations ;

using PostSharp.Constraints ;
using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     The target classes, and all classes that those classes reference, are marked as [Serializable] at build time.
    ///     <para>
    ///         Specifically, the following types will be marked as serializable: The annotated type, the types of its fields
    ///         recursively, its base type recursively. If any of those types is a generic signature, then all types present in
    ///         those generic signatures are marked as serializable recursively.
    ///     </para>
    ///     <para>
    ///         Types that are in a different assembly cannot be modified this way so remain as they are.
    ///     </para>
    /// </summary>
    [MulticastAttributeUsage (MulticastTargets.Class | MulticastTargets.Struct)]
    [PublicAPI]
    [RequirePostSharp ("Arkane.Aspects.Weaver", "DeepSerializableTask")]
    public class DeepSerializableAttribute : ScalarConstraint
    {
        /// <inheritdoc />
        public override void ValidateCode (object target) =>
            PostSharpHelpers.RequireArkaneAspectsWeaver (this.GetType (),
                                                         target,
                                                         "DeepSerializableAttribute: Makes this class and all classes that this class references [Serializable] at build time.") ;
    }
}
