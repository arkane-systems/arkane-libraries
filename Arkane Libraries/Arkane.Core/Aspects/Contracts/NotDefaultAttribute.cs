#region header

// Arkane.Core - NotDefaultAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 1:33 AM

#endregion

#region using

using System ;
using System.Reflection ;

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
    ///     if the value is of the default value for its type.
    /// </summary>
    [ProvideAspectRole (StandardRoles.Validation)]
    [CLSCompliant (false)]
    [LinesOfCodeAvoided (2)]
    [PublicAPI]
    public sealed class NotDefaultAttribute : LocationContractAttribute, ILocationValidationAspect
    {
        /// <inheritdoc cref="ValidateValue" />
        public Exception? ValidateValue (object? value, string locationName, LocationKind locationKind)
        {
            if (value == null)
                goto except ;

            Type type = value.GetType () ;

            if (!type.GetTypeInfo ().IsValueType)
                return null ;

            // value type
            if (value != Activator.CreateInstance (type))
                return null ;

            except:

            if (locationKind == LocationKind.ReturnValue)
                return this.CreatePostconditionFailedException (value, locationName, locationKind) ;

            return this.CreateArgumentException (value, locationName, locationKind) ;
        }

        #region Overrides of LocationContractAttribute

        /// <inheritdoc />
        [JetBrains.Annotations.NotNull]
        protected override string GetErrorMessage () =>
            Resources.NotDefaultAttribute_GetErrorMessage__HasAndCannotHaveDefaultValue ;

        #endregion
    }
}
