#region header

// Arkane.Aspects.Weaver - EqualsType.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 1:57 AM

#endregion

#region using

using PostSharp.Sdk.CodeModel ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.StructuralEquality
{
    public class EqualsType
    {
        public EqualsType (TypeDefDeclaration enhancedType, StructuralEqualityAttribute config)
        {
            this.EnhancedType = enhancedType ;
            this.Config       = config ;
        }

        public TypeDefDeclaration EnhancedType { get ; }

        public StructuralEqualityAttribute Config { get ; }
    }
}
