#region header

// Arkane.Aspects.Weaver - EqualityConfiguration.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 1:59 AM

#endregion

#region using

using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.CodeModel.Collections ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.StructuralEquality
{
    public static class EqualityConfiguration
    {
        /// <summary>
        ///     Returns information about properties set on an [StructuralEquality] attribute instance.
        /// </summary>
        /// <param name="trueAttribute">An attribute instance in user's code.</param>
        /// <returns>An image of what the user's attribute looked like.</returns>
        public static StructuralEqualityAttribute ExtractFrom (IAnnotationValue trueAttribute)
        {
            StructuralEqualityAttribute equality       = new StructuralEqualityAttribute () ;
            MemberValuePairCollection   namedArguments = trueAttribute.NamedArguments ;
            equality.DoNotAddEquals =
                (bool) (namedArguments[nameof (StructuralEqualityAttribute.DoNotAddEquals)]?.Value.Value ?? false) ;
            equality.DoNotAddGetHashCode =
                (bool) (namedArguments[nameof (StructuralEqualityAttribute.DoNotAddGetHashCode)]?.Value.Value ?? false) ;
            equality.IgnoreBaseClass =
                (bool) (namedArguments[nameof (StructuralEqualityAttribute.IgnoreBaseClass)]?.Value.Value ?? false) ;
            equality.TypeCheck = (TypeCheck) (namedArguments[nameof (StructuralEqualityAttribute.TypeCheck)]?.Value.Value ??
                                              TypeCheck.ExactlyTheSameTypeAsThis) ;
            equality.DoNotAddEqualityOperators =
                (bool) (namedArguments[nameof (StructuralEqualityAttribute.DoNotAddEqualityOperators)]?.Value.Value ?? false) ;
            return equality ;
        }
    }
}
