#region header

// Arkane.Core - TypeExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 11:39 AM

#endregion

#region using

using System.Reflection ;

#endregion

// ReSharper disable once CheckNamespace
namespace System
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static partial class ας_System
#pragma warning restore IDE1006 // Naming Styles
    {
        #region Params alternatives

        /// <summary>
        ///     Searches for a public instance constructor whose parameters match the specified types.
        /// </summary>
        /// <param name="type">Type to search.</param>
        /// <param name="types">Parameter types for the constructor.</param>
        /// <returns>A <see cref="ConstructorInfo" /> representing the constructor, or null if none is found.</returns>
        public static ConstructorInfo? GetConstructor (this Type type, params Type[] types) =>
            type.GetConstructor (types) ;

        /// <summary>
        ///     Searches for a public method whose parameters match the specified types.
        /// </summary>
        /// <param name="type">Type to search.</param>
        /// <param name="name">Name of the method to search for.</param>
        /// <param name="types">Parameter types for the method.</param>
        /// <returns>A <see cref="MethodInfo" /> representing the method, or null if none is found.</returns>
        public static MethodInfo? GetMethod (this Type type, string name, params Type[] types) =>
            type.GetMethod (name, types) ;

        #endregion
    }
}
