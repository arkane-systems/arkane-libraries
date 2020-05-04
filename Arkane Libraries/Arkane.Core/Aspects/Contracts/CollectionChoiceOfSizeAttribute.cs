#region header

// Arkane.Core - CollectionChoiceOfSizeAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 9:32 AM

#endregion

#region using

using System ;
using System.Collections ;
using System.Collections.ObjectModel ;
using System.Linq ;
using System.Text ;

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
    ///     if the target is assigned an <see cref="ICollection" /> containing a number of items not found in the list
    ///     specified.
    /// </summary>
    [ProvideAspectRole (StandardRoles.Validation)]
    [PublicAPI]
    [CLSCompliant (false)]
    public sealed class CollectionChoiceOfSizeAttribute : LocationContractAttribute,
                                                          ILocationValidationAspect <ICollection>
    {
        /// <summary>
        ///     Initializes a new <see cref="CollectionChoiceOfSizeAttribute" /> and specifies the permissible lengths.
        /// </summary>
        /// <param name="permissibleSizes">A list of permissible lengths, or an int[] containing them.</param>
        public CollectionChoiceOfSizeAttribute (
            params int[] permissibleSizes) =>
            this.PermissibleSizes = new ReadOnlyCollection <int> (permissibleSizes) ;

        /// <summary>
        ///     Gets the permissible lengths.
        /// </summary>
        public ReadOnlyCollection <int>
            PermissibleSizes { get ; }

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

            if (this.PermissibleSizes.Contains (value.Count))
                return null ;

            if (locationKind == LocationKind.ReturnValue)
                return this.CreatePostconditionFailedException (value, locationName, locationKind) ;

            return this.CreateArgumentException (value, locationName, locationKind) ;
        }

        #endregion

        #region Overrides of LocationContractAttribute

        /// <inheritdoc />
        protected override string GetErrorMessage ()
            => Resources.CollectionChoiceOfSizeAttribute_GetErrorMessage_CollectionNotOfPermissibleSize ;

        /// <inheritdoc />
        protected override object[] GetErrorMessageArguments ()
        {
            var sb = new StringBuilder () ;

            return new object[]
                   {
                       this.PermissibleSizes.Aggregate (sb,
                                                        (s, i) => s.Append ($"{i}, "))
                           .ToString (0, sb.Length - 2)
                   } ;
        }

        #endregion
    }
}
