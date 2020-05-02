#region header

// Arkane.Core - PackerUtility.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:29 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Contains the method used to initialize the Packer.
    /// </summary>
    [PublicAPI]
    public static class PackerUtility
    {
        /// <summary>
        ///     Call this to initialize the Packer. Use this if you're not using a module initializer. If you use this,
        ///     you must call this before using any class that references something from a packed-in assembly.
        /// </summary>
        public static void Initialize ()
        {
            throw new
                Exception (Resources.PackerUtility_Initialize_CodeNotReplaced) ;
        }
    }
}
