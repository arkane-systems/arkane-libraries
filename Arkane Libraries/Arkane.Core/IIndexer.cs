#region header

// Arkane.Core - IIndexer.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2020-04-22 2:25 PM

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     An interface defining a type with a key-based indexer.
    /// </summary>
    /// <typeparam name="TKey">The key of the desired value.</typeparam>
    /// <typeparam name="TValue">The value corresponding to the key.</typeparam>
    public interface IIndexer <in TKey, TValue>
    {
        TValue this [TKey key] { get ; set ; }
    }

    /// <summary>
    ///     An interface defining a type with a read-only key-based indexer.
    /// </summary>
    /// <typeparam name="TKey">The key of the desired value.</typeparam>
    /// <typeparam name="TValue">The value corresponding to the key.</typeparam>
    public interface IReadOnlyIndexer <in TKey, out TValue>
    {
        TValue this [TKey key] { get ; }
    }
}
