#region header

// Arkane.Core - VirtualAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:48 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     All non-private properties and methods of the classes affected by this attribute will be changed to virtual.
    ///     Affected methods that hide these methods are going to be changed to <c>override</c>.
    /// </summary>
    [AttributeUsage (AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property,
                     AllowMultiple = true)]
    [MulticastAttributeUsage (MulticastTargets.Method | MulticastTargets.Property)]
    [PublicAPI]
    [RequirePostSharp ("Arkane.Aspects.Weaver", "VirtuosityTask")]
    public class VirtualAttribute : MulticastAttribute
    { }
}
