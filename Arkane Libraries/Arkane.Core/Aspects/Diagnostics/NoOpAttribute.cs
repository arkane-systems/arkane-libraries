#region header

// Arkane.Core - NoOpAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 7:53 AM

#endregion

#region using

using ArkaneSystems.Arkane.Logging ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Diagnostics
{
    /// <summary>
    ///     An aspect that no-ops a method call, logging the no-opping to the debug logger.
    /// </summary>
    [PSerializable]
    [PublicAPI]

    // ReSharper disable once InheritdocConsiderUsage
    public sealed class NoOpAttribute : MethodInterceptionAspect
    {
        /// <inheritdoc />
        public override void OnInvoke ([NotNull] MethodInterceptionArgs args) =>
            LogProvider.For <NoOpAttribute> ().Debug ($"{args.Method.Name}: execution skipped (no-op).") ;
    }
}
