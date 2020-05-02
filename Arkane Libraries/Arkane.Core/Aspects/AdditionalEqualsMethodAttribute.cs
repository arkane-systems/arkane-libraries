#region header

// Arkane.Core - AdditionalEqualsMethodAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:15 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Custom method marker. The annotated method must have the signature <c>bool MethodName(Type)</c>, where <c>Type</c>
    ///     is the type that contains the method. The method is called by the auto-generated equality comparison after
    ///     all generated code.
    /// </summary>
    [AttributeUsage (AttributeTargets.Method)]
    [PublicAPI]
    public sealed class AdditionalEqualsMethodAttribute : Attribute
    { }
}
