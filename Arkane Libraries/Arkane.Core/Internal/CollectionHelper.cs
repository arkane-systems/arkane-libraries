#region header

// Arkane.Core - CollectionHelper.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 7:21 AM

#endregion

#region using

using System.Collections ;

using ArkaneSystems.Arkane.Aspects ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Internal
{
    /// <summary>
    ///     Helper static class used by code synthesized by <see cref="StructuralEqualityAttribute" />.
    /// </summary>
    [UsedImplicitly]
    public static class CollectionHelper
    {
        /// <summary>
        ///     Returns true if both enumerables have the same number of elements and all of those elements are equal, in order.
        /// </summary>
        /// <param name="left">One enumerable.</param>
        /// <param name="right">The second enumerable.</param>
        /// <returns>True if the collections are equal.</returns>
        public static bool Equals (IEnumerable? left, IEnumerable? right)
        {
            if (left == null)
                return right == null ;

            if (right == null)
                return false ;

            if (left is ICollection leftCollection && right is ICollection rightCollection)
                if (leftCollection.Count != rightCollection.Count)
                    return false ;

            var leftEnumerator  = left.GetEnumerator () ;
            var rightEnumerator = right.GetEnumerator () ;
            while (true)
            {
                bool leftHasNext  = leftEnumerator.MoveNext () ;
                bool rightHasNext = rightEnumerator.MoveNext () ;

                if (leftHasNext && rightHasNext && object.Equals (leftEnumerator.Current, rightEnumerator.Current))
                    continue ;

                return !leftHasNext && !rightHasNext ;
            }
        }
    }
}
