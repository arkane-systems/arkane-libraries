#region header

// Arkane.Core - TimeHelpers.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 2:57 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Non-extension helper methods for <see cref="System.DateTime" /> and <see cref="System.TimeSpan" />.
    /// </summary>
    [PublicAPI]
    public static class TimeHelpers
    {
        /// <summary>
        ///     Returns a Timespan initialized to the specified number of weeks, days, hours, minutes, seconds, and milliseconds.
        /// </summary>
        /// <remarks>
        ///     Based on code found here:
        ///     http://matt-hickford.github.io/programming/csharp/2012/03/08/csharp-timespan-and-python-timedelta.html .
        ///     Ultimately based on Python's timedelta.
        /// </remarks>
        public static TimeSpan Delta (int weeks        = 0,
                                      int days         = 0,
                                      int hours        = 0,
                                      int minutes      = 0,
                                      int seconds      = 0,
                                      int milliseconds = 0)
            => new TimeSpan (weeks * 7 + days, hours, minutes, seconds, milliseconds) ;
    }
}
