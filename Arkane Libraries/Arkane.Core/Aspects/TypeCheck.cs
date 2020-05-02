#region header

// Arkane.Core - TypeCheck.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:06 AM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Defines requirements on the type of the comparand in the <see cref="object.Equals(object)" /> method.
    /// </summary>
    [PublicAPI]
    public enum TypeCheck
    {
        /// <summary>
        ///     The 'this' and 'other' comparands must have the exact same type for equality to pass.
        /// </summary>
        ExactlyTheSameTypeAsThis,

        /// <summary>
        ///     The 'other' comparand must be of the exact type that's annotated with this
        ///     <see cref="StructuralEqualityAttribute" />.
        /// </summary>
        ExactlyOfType,

        /// <summary>
        ///     The 'other' comparand must be of the type that's annotated with this <see cref="StructuralEqualityAttribute" /> or
        ///     it must be a subtype of this type.
        /// </summary>
        SameTypeOrSubtype
    }
}
