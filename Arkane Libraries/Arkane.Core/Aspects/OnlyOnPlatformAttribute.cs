#region header

// Arkane.Core - OnlyOnPlatformAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 2:09 PM

#endregion

#region using

using System ;
using System.Linq ;
using System.Runtime.InteropServices ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect for methods which apply to only a single platform, and allow them to be
    ///     executed only when running on that platform. Otherwise, they are a no-op.
    /// </summary>
    [LinesOfCodeAvoided (4)]
    [PSerializable]
    [PublicAPI]
    [CLSCompliant (false)]
    public sealed class OnlyOnPlatformAttribute : MethodInterceptionAspect
    {
        /// <summary>
        ///     An aspect for methods which apply to only a subset of platforms, and allow them to be
        ///     executed only when running on that platform. Otherwise, they are a no-op.
        /// </summary>
        /// <param name="platforms">The platform(s) on which this method is supported.</param>
        public OnlyOnPlatformAttribute (params string[] platforms) =>
            this.validOn = platforms ;

        // Cannot be readonly because class is PSerializable.
#pragma warning disable IDE0044 // Add readonly modifier
        private string[] validOn ;
#pragma warning restore IDE0044 // Add readonly modifier

        /// <summary>
        ///     If set to true, throw an exception instead of no-opping.
        /// </summary>
        public bool ThrowIfIncompatible { get ; set ; }

        /// <inheritdoc />
        /// <exception cref="T:System.PlatformNotSupportedException">
        ///     Tried to call method that is not supported on this platform,
        ///     with <paramref cref="ThrowIfIncompatible" /> set.
        /// </exception>
        public override void OnInvoke (MethodInterceptionArgs args)
        {
            foreach (OSPlatform p in from str in this.validOn
                                     select OSPlatform.Create (str.ToUpper ()))
            {
                if (RuntimeInformation.IsOSPlatform (p))
                {
                    args.Proceed () ;
                    return ;
                }
            }

            if (this.ThrowIfIncompatible)
                throw new PlatformNotSupportedException (Resources.OnlyOnPlatformAttribute_OnInvoke_NotSupportedOnThisPlatform) ;
        }
    }
}
