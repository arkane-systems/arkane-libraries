#region header

// Arkane.Aspects.Weaver - UsableField.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 3:31 PM

#endregion

#region using

using PostSharp.Sdk.CodeModel ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver
{
    /// <summary>
    ///     A field in a type or one of its base types + a generic map to access that field from the original enhanced type.
    /// </summary>
    internal class UsableField
    {
        public UsableField (FieldDefDeclaration fieldDefinition, GenericMap mapToAccessTheFieldFromMostDerivedClass)
        {
            this.FieldDefinition                         = fieldDefinition ;
            this.MapToAccessTheFieldFromMostDerivedClass = mapToAccessTheFieldFromMostDerivedClass ;
        }

        public FieldDefDeclaration FieldDefinition { get ; }

        public GenericMap MapToAccessTheFieldFromMostDerivedClass { get ; }
    }
}
