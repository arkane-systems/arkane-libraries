#region header

// Arkane.Core - StructuralEqualityOperator.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:08 AM

#endregion

#region using

using System ;
using System.Diagnostics.CodeAnalysis ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Allows you to have PostSharp auto-generate <c>==</c> and <c>!=</c> operators for you.
    /// </summary>
    [PublicAPI]
    public static class EqualityOperator
    {
        /// <summary>
        ///     Add the following code to a type annotated with <c>[StructuralEquality]</c> to auto-generate equality operators.
        ///     <code>
        /// public static bool operator ==(YourClass left, YourClass right) => StructuralEqualityOperator.Weave(left, right);
        /// public static bool operator !=(YourClass left, YourClass right) => StructuralEqualityOperator.Weave(left, right);
        /// </code>
        ///     Calls to this method are replaced at build time with appropriate code by PostSharp.
        /// </summary>
        [SuppressMessage ("Style", "IDE0060:Remove unused parameter", Justification = "Parameter is used in postcompile.")]
        public static bool Weave <T> (T left, T right) => throw EqualityOperator.WeavingNotWorkingException () ;

        private static Exception WeavingNotWorkingException () =>
            new
                Exception ("StructuralEquality was supposed to replace this method call with an implementation. Either weaving has not worked or you have called this method from an unsupported place. The only supported places are implementations of the `==` and `!=` operators in a type annotated with [StructuralEquality].") ;
    }
}
