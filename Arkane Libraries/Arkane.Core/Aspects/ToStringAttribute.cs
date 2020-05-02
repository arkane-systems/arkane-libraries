#region header

// Arkane.Core - ToStringAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:41 AM

#endregion

#region using

using JetBrains.Annotations ;

using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Annotating a type with this attribute causes a ToString method to be generated in it. If the type already
    ///     has a ToString method, it's kept and nothing is generated.
    /// </summary>
    [MulticastAttributeUsage (MulticastTargets.Class | MulticastTargets.Struct)]
    [PublicAPI]
    [RequirePostSharp ("Arkane.Aspects.Weaver", "ToStringTask")]
    public class ToStringAttribute : AbstractBaseToStringAttribute
    { }
}
