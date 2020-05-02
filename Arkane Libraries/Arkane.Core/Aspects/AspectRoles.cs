#region header

// Arkane.Core - AspectRoles.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:17 AM

#endregion

#region using

using JetBrains.Annotations ;

using PostSharp.Aspects.Dependencies ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Defines additional aspect roles (in addition to the <see cref="StandardRoles" />).
    /// </summary>
    [PublicAPI]
    public static class AspectRoles

    {
        /// <summary>
        ///     The aspect invokes some processing to be completed at post-compile time, rather than runtime.
        /// </summary>
        public const string InvokesOnPostCompile = "PostCompile" ;
    }
}
