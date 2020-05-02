#region header

// Arkane.Core - StructuralEqualityAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:03 AM

#endregion

#region using

using JetBrains.Annotations ;

using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     PostSharp creates an Equals and GetHashCode implementation for the target annotated type. See the GitHub
    ///     repository for this add-in for options and details on how the implementation works.
    /// </summary>
    [MulticastAttributeUsage (MulticastTargets.Class | MulticastTargets.Struct)]
    [PublicAPI]
    [RequirePostSharp ("Arkane.Aspects.Weaver", "StructuralEqualityTask")]
    public class StructuralEqualityAttribute : MulticastAttribute
    {
        /// <summary>
        ///     If true, PostSharp does not create the <see cref="object.GetHashCode" /> method. If you supply your own GetHashCode
        ///     method in the annotated type, you don't need to set this property. Your code will take precedence.
        /// </summary>
        public bool DoNotAddGetHashCode { get ; set ; }

        /// <summary>
        ///     If true, PostSharp does not create the <see cref="object.Equals(object)" /> method. If you supply your own Equals
        ///     method in the annotated type, you don't need to set this property. Your code will take precedence.
        /// </summary>
        public bool DoNotAddEquals { get ; set ; }

        /// <summary>
        ///     Specifies requirements on the type of the comparand in the <see cref="object.Equals(object)" /> method.
        /// </summary>
        public TypeCheck TypeCheck { get ; set ; }

        /// <summary>
        ///     If true, <c>base.Equals()</c> is not called in the generated <c>Equals</c> method. The same is true for
        ///     <c>GetHashCode</c>.
        /// </summary>
        public bool IgnoreBaseClass { get ; set ; }

        /// <summary>
        ///     If true, equality operators are neither checked nor replaced.
        /// </summary>
        public bool DoNotAddEqualityOperators { get ; set ; }
    }
}
