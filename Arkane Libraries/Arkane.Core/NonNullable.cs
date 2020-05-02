#region header

// Arkane.Core - NonNullable.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:31 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Encapsulates a reference compatible with the type parameter. The reference
    ///     is guaranteed to be non-null unless the value has been created with the
    ///     parameterless constructor (e.g. as the default value of a field or array).
    ///     Implicit conversions are available to and from the type parameter. The
    ///     conversion to the non-nullable type will throw ArgumentNullException
    ///     when presented with a null reference. The conversion from the non-nullable
    ///     type will throw NullReferenceException if it contains a null reference.
    ///     This type is a value type (to avoid taking any extra space) and as the CLR
    ///     unfortunately has no knowledge of it, it will be boxed as any other value
    ///     type. The conversions are also available through the Value property and the
    ///     parameterized constructor.
    /// </summary>
    /// <typeparam name="T">Type of non-nullable reference to encapsulate.</typeparam>
    [PublicAPI]
    [Obsolete ("Use C# 8 nullability instead.")]
    public struct NonNullable <T> : IEquatable <NonNullable <T>> where T : class
    {
        private readonly T? value ;

        /// <summary>
        ///     Creates a non-nullable value encapsulating the specified reference.
        /// </summary>
        public NonNullable ([NotNull] T value) => this.value = value ;

        /// <summary>
        ///     Retrieves the encapsulated value, or throws a NullReferenceException if
        ///     this instance was created with the parameterless constructor or by default.
        /// </summary>
        [NotNull]
        public T Value
        {
            get
            {
                if (this.value == null)
                    throw new NullReferenceException () ;

                return this.value ;
            }
        }

        #region IEquatable<NonNullable<T>> Members

        /// <summary>
        ///     Type-safe (and non-boxing) equality check.
        /// </summary>
        public bool Equals (NonNullable <T> other) => object.Equals (this.value, other.value) ;

        #endregion

        /// <summary>
        ///     Implicit conversion from the specified reference.
        /// </summary>
        public static implicit operator NonNullable <T> ([PostSharp.Patterns.Contracts.NotNull] T value)
            => new NonNullable <T> (value) ;

        /// <summary>
        ///     Implicit conversion to the type parameter from the encapsulated value.
        /// </summary>
        [NotNull]
        public static implicit operator T (NonNullable <T> wrapper)
        {
            if (wrapper.value == null)
                throw new NullReferenceException () ;

            return wrapper.Value ;
        }

        /// <summary>
        ///     Equality operator, which performs an identity comparison on the encapsulated
        ///     references. No exception is thrown even if the references are null.
        /// </summary>
        public static bool operator == (NonNullable <T> first, NonNullable <T> second) => first.value == second.value ;

        /// <summary>
        ///     Inequality operator, which performs an identity comparison on the encapsulated
        ///     references. No exception is thrown even if the references are null.
        /// </summary>
        public static bool operator != (NonNullable <T> first, NonNullable <T> second) => first.value != second.value ;

        /// <summary>
        ///     Equality is deferred to encapsulated references, but there is no equality
        ///     between a NonNullable[T] and a T. This method never throws an exception,
        ///     even if a null reference is encapsulated.
        /// </summary>
        public override bool Equals (object obj) => obj is NonNullable <T> other && this.Equals (other) ;

        /// <summary>
        ///     Type-safe (and non-boxing) static equality check.
        /// </summary>
        public static bool Equals (NonNullable <T> first, NonNullable <T> second)
            => object.Equals (first.value, second.value) ;

        /// <summary>
        ///     Defers to the GetHashCode implementation of the encapsulated reference, or 0 if
        ///     the reference is null.
        /// </summary>
        public override int GetHashCode () => this.value?.GetHashCode () ?? 0 ;

        /// <summary>
        ///     Defers to the ToString implementation of the encapsulated reference, or an
        ///     empty string if the reference is null.
        /// </summary>
        public override string ToString () => this.value?.ToString () ?? "" ;
    }
}
