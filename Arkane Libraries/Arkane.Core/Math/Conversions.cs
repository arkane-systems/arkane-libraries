#region header

// Arkane.Core - Conversions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 9:40 AM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Math
{
    /// <summary>
    ///     Unit conversions.
    /// </summary>
    [PublicAPI]
    public static class Conversions
    {
        #region Angles

        /// <summary>
        ///     Convert an angular measurement in radians to degrees.
        /// </summary>
        /// <param name="radians">The angular measurement in radians.</param>
        /// <returns>The angular measurement converted to degrees.</returns>
        public static double ToDegrees (double radians) => 180 / System.Math.PI * radians ;

        /// <summary>
        ///     Convert an angular measurement in degrees to radians.
        /// </summary>
        /// <param name="degrees">The angular measurement in degrees.</param>
        /// <returns>The angular measurement converted to radians.</returns>
        public static double ToRadians (double degrees) => System.Math.PI / 180 * degrees ;

        #endregion Angles
    }
}
