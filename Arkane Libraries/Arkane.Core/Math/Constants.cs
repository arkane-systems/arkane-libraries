#region header

// Arkane.Core - Constants.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:39 AM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Math
{
    /// <summary>
    ///     Mathematical and physical constants.
    /// </summary>
    [PublicAPI]
    public static class Constants
    {
        /// <summary>
        ///     Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π.
        /// </summary>
        /// <remarks>
        ///     Sodding gratuitous. Just use System.Math.PI.
        /// </remarks>

        // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
        public static double π => System.Math.PI ;
#pragma warning restore IDE1006 // Naming Styles


        /// <summary>
        ///     Represents Palais' circle constant, equal to 2π; the number of radians in a single turn.
        /// </summary>

        // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
        public static double τ => Constants.Tau ;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        ///     Represents Palais' circle constant, equal to 2π; the number of radians in a single turn.
        /// </summary>
        public static double Tau => System.Math.PI * 2.0d ;
    }
}
