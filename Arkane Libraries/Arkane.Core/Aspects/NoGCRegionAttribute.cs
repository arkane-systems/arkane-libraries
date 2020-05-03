#region header

// Arkane.Core - NoGCRegionAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 6:06 PM

#endregion

#region using

using System ;
using System.Runtime ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect to execute a function within a no-GC region, with the specified amount of preallocated memory.
    ///     A garbage collection will be triggered if the function allocates more than this amount of memory.
    /// </summary>
    [Serializable]
    [PublicAPI]

    // ReSharper disable once InconsistentNaming
    public sealed class NoGCRegionAttribute : MethodInterceptionAspect
    {
        /// <summary>
        ///     An aspect to execute a function within a no-GC region, with the specified amount of preallocated memory.
        ///     A garbage collection will be triggered if the function allocates more than this amount of memory.
        /// </summary>
        /// <param name="desiredMemory">
        ///     The amount of preallocated memory desired (in bytes). Defaults to 4K (4096 bytes).
        /// </param>
        /// <param name="isCritical">
        ///     If true, throws an exception if the no-GC region cannot be allocated or ends early. Defaults to false.
        /// </param>
        public NoGCRegionAttribute (long desiredMemory = 4096, bool isCritical = false)
        {
            this.desiredMemory = desiredMemory ;
            this.isCritical    = isCritical ;
        }

        private readonly long desiredMemory ;
        private readonly bool isCritical ;

        #region Overrides of MethodInterceptionAspect

        /// <summary>
        ///     Method invoked
        ///     <i>instead</i>
        ///     of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">
        ///     Advice arguments.
        /// </param>
        /// <exception cref="GCRegionException">An error occurred entering or within the no GC region.</exception>
        public override void OnInvoke (MethodInterceptionArgs args)
        {
            // Are we already in no GC region latency mode?
            if (GCSettings.LatencyMode == GCLatencyMode.NoGCRegion)
            {
                // skip the rest.
                args.Proceed () ;
                return ;
            }

            bool obtained = GC.TryStartNoGCRegion (this.desiredMemory) ;

            if (!obtained && this.isCritical)
                throw new GCRegionException (Resources.NoGCRegionAttribute_OnInvoke_NotEnoughCommitableMemory) ;

            args.Proceed () ;

            if (GCSettings.LatencyMode == GCLatencyMode.NoGCRegion)
            {
                GC.EndNoGCRegion () ;
            }
            else
            {
                // exited early, because a garbage collection was induced or we overran the area
                if (this.isCritical)
                    throw
                        new GCRegionException (Resources.NoGCRegionAttribute_OnInvoke_RegionEndedEarly) ;
            }
        }

        #endregion
    }
}
