#region header

// Arkane.Core - CollectionSizeAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 2:44 AM

#endregion

#region using

using System ;
using System.Collections ;

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
    ///     Custom aspect that, when added to a field, property, or parameter, throws an <see cref="ArgumentException" />
    ///     if the target is assigned an <see cref="ICollection" /> containing a different number of items than that
    ///     specified.
    /// </summary>
    [ProvideAspectRole (StandardRoles.Validation)]
    [PublicAPI]
    [CLSCompliant (false)]
    public sealed class CollectionSizeAttribute : LocationContractAttribute, ILocationValidationAspect <ICollection>
    {
        /// <summary>
        ///     Initializes a new <see cref="CollectionSizeAttribute" /> and specifies the permissible maximum length.
        /// </summary>
        /// <param name="maximumSize">Maximum size.</param>
        public CollectionSizeAttribute (int maximumSize)
        {
            this.MaximumSize = maximumSize ;
            this.MinimumSize = 0 ;
        }

        /// <summary>
        ///     Initializes a new <see cref="CollectionSizeAttribute" /> and specifies the permissible maximum and minimum lengths.
        /// </summary>
        /// <param name="minimumSize"></param>
        /// <param name="maximumSize"></param>
        public CollectionSizeAttribute (int minimumSize, int maximumSize)
        {
            this.MaximumSize = maximumSize ;
            this.MinimumSize = minimumSize ;
        }

        /// <summary>
        ///     Gets the permissible maximum size.
        /// </summary>
        public int MaximumSize { get ; private set ; }

        /// <summary>
        ///     Gets the permissible minimum size.
        /// </summary>
        public int MinimumSize { get ; private set ; }

        #region ILocationValidationAspect<ICollection> Members

        /// <inheritdoc />
        public Exception? ValidateValue (ICollection?              value,
                                         string                    locationName,
                                         LocationKind              locationKind,
                                         LocationValidationContext context)
        {
            // For validating this, if necessary, we rely on the [Required] aspect.
            if (value == null)
                return null ;

            if ((value.Count >= this.MinimumSize) && (value.Count <= this.MaximumSize))
                return null ;

            if (locationKind == LocationKind.ReturnValue)
                return this.CreatePostconditionFailedException (value, locationName, locationKind) ;

            return this.CreateArgumentException (value, locationName, locationKind) ;
        }

        #endregion

        #region Overrides of LocationContractAttribute

        /// <inheritdoc />
        protected override string GetErrorMessage ()
        {
            if (this.MinimumSize == 0)
                return Resources.CollectionSizeAttribute_GetErrorMessage_ExceedsMaximumSize ;

            if (this.MaximumSize == int.MaxValue)
                return Resources.CollectionSizeAttribute_GetErrorMessage_BelowMinimumSize ;

            return Resources.CollectionSizeAttribute_GetErrorMessage_OutsidePermissibleSize ;
        }

        /// <inheritdoc />
        [JetBrains.Annotations.NotNull]
        protected override object[] GetErrorMessageArguments ()
        {
            if (this.MinimumSize == 0)
                return new object[] {this.MaximumSize} ;

            if (this.MaximumSize == int.MaxValue)
                return new object[] {this.MinimumSize} ;

            return new object[] {this.MinimumSize, this.MaximumSize} ;
        }

        #endregion
    }
}
