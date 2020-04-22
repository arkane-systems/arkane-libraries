#region header

// Arkane.Core - Empty.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 3:05 PM

#endregion

#region using

using System ;
using System.Diagnostics.CodeAnalysis ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     An empty value (equivalent to void).
    /// </summary>
    [PublicAPI]
    public struct Empty : IEquatable <Empty>
    {
        #region IEquatable<Empty> Members

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals (Empty other) => true ;

        #endregion

        /// <summary>
        ///     Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        public override bool Equals (object obj) => obj is Empty ;

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode () => typeof (Empty).GetHashCode () ;

        /// <summary>
        ///     Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> containing a fully qualified type name.
        /// </returns>
        public override string ToString () => @"<empty>" ;


        /// <summary>
        ///     Equality operator. Two <see cref="Empty" /> instances are always equal.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True.</returns>
        [SuppressMessage ("Style",
                          "IDE0060:Remove unused parameter",
                          Justification = "Public API.")]
        public static bool operator == (Empty first, Empty second) => true ;

        /// <summary>
        ///     Inequality operator. Two <see cref="Empty" /> instances are never unequal.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>False.</returns>
        [SuppressMessage ("Style",
                          "IDE0060:Remove unused parameter",
                          Justification = "Public API.")]
        public static bool operator != (Empty first, Empty second) => false ;
    }
}
