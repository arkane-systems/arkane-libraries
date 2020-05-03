#region header

// Arkane.Core - HasTypeAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 2:46 PM

#endregion

#region using

using System ;
using System.Collections.ObjectModel ;
using System.Linq ;
using System.Reflection ;
using System.Text ;

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
    ///     when the value does not implement the expected type(s).
    /// </summary>
    [CLSCompliant (false)]
    [ProvideAspectRole (StandardRoles.Validation)]
    [PublicAPI]
    public sealed class HasTypeAttribute : LocationContractAttribute, ILocationValidationAspect
    {
        /// <summary>
        ///     Initializes a new <see cref="HasTypeAttribute" /> and specifies the required types.
        /// </summary>
        /// <param name="requiredTypes">A list of required types, or a Type[] containing them.</param>
        public HasTypeAttribute (params Type[] requiredTypes) =>
            this.RequiredTypes = new ReadOnlyCollection <Type> (requiredTypes) ;

        /// <summary>
        ///     Gets the required types.
        /// </summary>
        public ReadOnlyCollection <Type> RequiredTypes { get ; }

        /// <inheritdoc cref="ValidateValue" />
        public Exception? ValidateValue (object? value, string locationName, LocationKind locationKind)
        {
            // Type is irrelevant in this case.
            if (value == null)
                return null ;

            TypeInfo actualType = value.GetType ().GetTypeInfo () ;

            if (this.RequiredTypes.All (type => type.GetTypeInfo ().IsAssignableFrom (actualType)))
                return null ;

            if (locationKind == LocationKind.ReturnValue)
                return this.CreatePostconditionFailedException (value, locationName, locationKind) ;

            return this.CreateArgumentException (value, locationName, locationKind) ;
        }

        #region Overrides of LocationContractAttribute

        /// <inheritdoc />
        [JetBrains.Annotations.NotNull]
        protected override string GetErrorMessage ()
            => "Object {0} does not implement required type(s) (required: {4})." ;

        /// <inheritdoc />
        [JetBrains.Annotations.NotNull]
        protected override object[] GetErrorMessageArguments ()
        {
            var sb = new StringBuilder () ;

            return new object[1] {this.RequiredTypes.Aggregate (sb, (s, t) => s.Append ($"{t}, ")).ToString (0, sb.Length - 2)} ;
        }

        #endregion
    }
}
