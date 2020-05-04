#region header

// Arkane.Aspects.Weaver - UsableProperty.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 3:32 PM

#endregion

#region using

using PostSharp.Sdk.CodeModel ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver
{
    /// <summary>
    ///     A property in a type or one of its base types + a generic map to access that property from the original enhanced
    ///     type.
    /// </summary>
    internal class UsableProperty
    {
        public UsableProperty (PropertyDeclaration propertyDefinition, GenericMap mapToAccessThisPropertyFromMostDerivedClass)
        {
            this.PropertyDefinition                          = propertyDefinition ;
            this.MapToAccessThisPropertyFromMostDerivedClass = mapToAccessThisPropertyFromMostDerivedClass ;
        }

        public PropertyDeclaration PropertyDefinition { get ; }

        public GenericMap MapToAccessThisPropertyFromMostDerivedClass { get ; }
    }
}
