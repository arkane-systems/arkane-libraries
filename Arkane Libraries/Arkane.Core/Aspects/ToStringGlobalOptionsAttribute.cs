#region header

// Arkane.Core - ToStringGlobalOptionsAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:40 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Properties set on this attribute are used as default for all <see cref="ToStringAttribute" /> instances that
    ///     don't have those properties set. Properties whose names start with "Attribute" are ignored on this attribute.
    /// </summary>
    [AttributeUsage (AttributeTargets.Assembly)]
    [MulticastAttributeUsage (MulticastTargets.Assembly)]
    [PublicAPI]
    public class ToStringGlobalOptionsAttribute : AbstractBaseToStringAttribute
    { }
}
