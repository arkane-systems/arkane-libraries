#region header

// Arkane.Core - AdditionalGetHashCodeMethodAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:14 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Custom method marker. The annotated method must have the signature <c>int MethodName()</c>. The method
    ///     is called by the auto-generated GetHashCode method after all generated code, and it's combined with the generated
    ///     hash code with a variant of the Fowler–Noll–Vo algorithm.
    /// </summary>
    [AttributeUsage (AttributeTargets.Method)]
    [PublicAPI]
    public sealed class AdditionalGetHashCodeMethodAttribute : Attribute
    { }
}
