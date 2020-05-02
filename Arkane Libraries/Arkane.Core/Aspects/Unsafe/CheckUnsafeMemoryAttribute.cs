#region header

// Arkane.Core - CheckUnsafeMemoryAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:23 AM

#endregion

#region using

using System ;

using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Unsafe
{
    /// <summary>
    ///     If this attribute is defined, it means that pointer write instructions should be instrumented and should throw
    ///     an exception if access to uncontrolled memory is attempted.
    /// </summary>
    [AttributeUsage (AttributeTargets.Assembly)]
    [RequirePostSharp ("Arkane.Aspects.Weaver", "CheckUnsafeMemoryAccessTask")]
    public class CheckUnsafeMemoryAttribute : Attribute
    { }
}
