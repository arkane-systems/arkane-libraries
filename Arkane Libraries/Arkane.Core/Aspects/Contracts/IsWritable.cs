#region header

// Arkane.Core - IsWritable.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 1:14 AM

#endregion

#region using

using System ;
using System.IO ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Patterns.Contracts ;
using PostSharp.Reflection ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Contracts
{
    /// <summary>
    ///     Custom aspect that, when added to a field, property, or parameter of type <see cref="Stream" />, throws an
    ///     <see cref="ArgumentException" /> is the target is assigned a <see cref="Stream" /> that is not writable.
    /// </summary>
    [ProvideAspectRole (StandardRoles.Validation)]
    [CLSCompliant (false)]
    [LinesOfCodeAvoided (2)]
    [PublicAPI]
    public sealed class IsWritableAttribute : LocationContractAttribute, ILocationValidationAspect <Stream>
    {
        #region ILocationValidationAspect<Stream> Members

        /// <inheritdoc />
        public Exception? ValidateValue (Stream?                   value,
                                         string                    locationName,
                                         LocationKind              locationKind,
                                         LocationValidationContext context)
        {
            if (value?.CanWrite != true)
                return null ;

            if (locationKind == LocationKind.ReturnValue)
                return this.CreatePostconditionFailedException (value, locationName, locationKind) ;

            return this.CreateArgumentException (value, locationName, locationKind) ;
        }

        #endregion

        #region Overrides of LocationContractAttribute

        /// <inheritdoc />
        [JetBrains.Annotations.NotNull]
        protected override string GetErrorMessage () => Resources.IsWritableAttribute_GetErrorMessage_StreamNotWritable ;

        #endregion
    }
}
