#region header

// Arkane.Core - Mac48AddressAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 2:05 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects.Dependencies ;
using PostSharp.Patterns.Contracts ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Contracts
{
    /// <summary>
    ///     Custom aspect that, when added to a field, property, or parameter, throws an <see cref="ArgumentException" /> if
    ///     the target is assigned a value that is not a valid IEEE MAC-48 address in one of the two standard-compliant
    ///     formats. Null strings are accepted and do not throw an exception.
    /// </summary>
    [ProvideAspectRole (StandardRoles.Validation)]
    [CLSCompliant (false)]
    [PublicAPI]
    public sealed class Mac48AddressAttribute : RegularExpressionAttribute
    {
        private const string Pattern = @"^(?:[0-9a-fA-F]{2}([-:]))(?:[0-9a-fA-F]{2}\1){4}[0-9a-fA-F]{2}$" ;

        /// <summary>
        ///     Initializes a new <see cref="Mac48AddressAttribute" />.
        /// </summary>
        public Mac48AddressAttribute ()
            : base (Mac48AddressAttribute.Pattern)
        { }

        /// <inheritdoc />
        [JetBrains.Annotations.NotNull]
        protected override string GetErrorMessage ()
            => Resources.Mac48AddressAttribute_GetErrorMessage_MustBeValidMacAddress ;
    }
}
