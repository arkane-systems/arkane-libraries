#region header

// Arkane.Annotations - CodeQualityAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-20 11:57 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Annotations.Properties ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     The quality and reliability of the marked code.
    /// </summary>
    [PublicAPI]
    public sealed class CodeQualityAttribute : Attribute
    {
        /// <summary>
        ///     Creates a CodeQualityAttribute, setting the quality and reliability as specified.
        /// </summary>
        /// <param name="quality">The subjective quality of the marked code.</param>
        /// <param name="reliability">The subjective reliability of the marked code.</param>
        /// <exception cref="T:System.ArgumentException">Perfection is unattainable.</exception>
        public CodeQualityAttribute (
            [EnumDataType (typeof (SoftwareQuality))]
            SoftwareQuality quality = SoftwareQuality.Win,
            [EnumDataType (typeof (SoftwareReliability))]
            SoftwareReliability reliability = SoftwareReliability.Solid)
        {
            if (quality == SoftwareQuality.Perfection)
                throw new ArgumentException (
                                             Resources.CodeQualityAttribute_PerfectionIsUnattainable,
                                             nameof (quality)) ;

            this.Quality     = quality ;
            this.Reliability = reliability ;
        }

        /// <summary>
        ///     The subjective quality of the marked code.
        /// </summary>
        public SoftwareQuality Quality { get ; }

        /// <summary>
        ///     The subjective reliability of the marked code.
        /// </summary>
        public SoftwareReliability Reliability { get ; }
    }
}
