using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices ;
using System.Text;

using JetBrains.Annotations ;

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Supports shallow copying, which creates a new instance of a class whose fields reference the same objects as the
    ///     original object.
    /// </summary>
    /// <typeparam name="T">The type of the shallow-copyable object.</typeparam>
    /// <remarks>
    ///     <para>
    ///         DO NOT implement this on the same class as <see cref="IDeepCopy{T}" />.
    ///     </para>
    /// </remarks>
    [PublicAPI]
    public interface IShallowCopy <out T> : ICloneable
    {
        /// <summary>
        ///     Creates a shallow copy of an object, a new instance of the class whose fields reference the same objects as the
        ///     original object.
        /// </summary>
        /// <returns>A shallow copy of the object.</returns>
        T ShallowCopy () ;

        /// <inheritdoc />
        object? ICloneable.Clone () => this.ShallowCopy () ;
    }
}
