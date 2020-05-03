#region header

// Arkane.Core - GCRegionException.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 5:12 PM

#endregion

#region using

using System ;
using System.Runtime.Serialization ;
using System.Security.Permissions ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An exception which indicates a problem making use of a GC region.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [PrivateThreadAware]

    // ReSharper disable once InconsistentNaming
    public sealed class GCRegionException : Exception
    {
        /// <inheritdoc />
        public GCRegionException ()
            : base (Resources.GCRegionException_GCRegionException_CriticalErrorInGCRegion)
        { }

        /// <inheritdoc />
        public GCRegionException (string message)
            : base (message)
        { }

        /// <inheritdoc />
        public GCRegionException (string message, Exception innerException)
            : base (message, innerException)
        { }

        /// <inheritdoc />
        [SecurityPermission (SecurityAction.Demand, SerializationFormatter = true)]

        // ReSharper disable once UnusedMember.Local
        private GCRegionException (SerializationInfo info, StreamingContext context)
            : base (info, context)
        { }
    }
}
