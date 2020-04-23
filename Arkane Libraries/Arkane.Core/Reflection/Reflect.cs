#region header

// Arkane.Core - Reflect.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 11:13 AM

#endregion

#region using

using System ;
using System.Collections.Generic ;
using System.Linq ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Reflection
{
    /// <summary>
    ///     Top level methods to perform reflection-based operations.
    /// </summary>
    [PublicAPI]
    public static class Reflect
    {
        /// <summary>
        ///     Find a type using a type name and assembly name to search.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <param name="assemblyName">The name of the assembly to search.</param>
        /// <returns>The requested type, or null if it was not found.</returns>
        public static Type? FindType (string typeName, string assemblyName) =>
            Reflect.FindType (typeName, new[] {assemblyName}) ;

        /// <summary>
        ///     Find a type using a type name and a list of assembly names to search.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="assemblyNames"></param>
        /// <returns>The requested type, or null if it was not found.</returns>
        public static Type? FindType (string typeName, IReadOnlyList <string> assemblyNames) =>
            assemblyNames.Select (assemblyName => Type.GetType ($"{typeName}, {assemblyName}"))
                         .FirstOrDefault (type => type != null) ??
            Type.GetType (typeName) ;
    }
}
