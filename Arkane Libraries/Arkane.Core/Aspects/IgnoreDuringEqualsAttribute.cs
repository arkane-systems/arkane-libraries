#region header

// Arkane.Core - IgnoreDuringEqualsAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:12 AM

#endregion

#region using

using JetBrains.Annotations ;

using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     The annotated field or property is ignored during the generation of Equals and GetHashCode methods.
    /// </summary>
    [MulticastAttributeUsage (MulticastTargets.Field | MulticastTargets.Property)]
    [PublicAPI]
    public sealed class IgnoreDuringEqualsAttribute : MulticastAttribute
    { }
}
