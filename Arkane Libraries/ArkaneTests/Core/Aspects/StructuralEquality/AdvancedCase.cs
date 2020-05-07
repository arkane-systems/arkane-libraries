#region header

// ArkaneTests - AdvancedCase.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-07 1:55 AM

#endregion

#region using

using System ;
using System.Collections.Generic ;

using ArkaneSystems.Arkane.Aspects ;

#endregion

namespace ArkaneTests.Core.Aspects.StructuralEquality
{
    [StructuralEquality (TypeCheck = TypeCheck.ExactlyTheSameTypeAsThis)]
    public class AdvancedCase : AdvancedBaseClass
    {
        protected string field ;

        public List <List <object>> lists { get ; } = new List <List <object>> () ;

        [IgnoreDuringEquals]
        public float DoNotUse { get ; set ; }

        [AdditionalEqualsMethod]
        public bool AndFloatWithinRange (AdvancedCase other) => Math.Abs (this.DoNotUse - other.DoNotUse) < 0.1f ;

        public static bool operator == (AdvancedCase left, AdvancedCase right) => EqualityOperator.Weave (left, right) ;

        public static bool operator != (AdvancedCase left, AdvancedCase right) => EqualityOperator.Weave (left, right) ;
    }

    [StructuralEquality (DoNotAddEqualityOperators = true, DoNotAddGetHashCode = true)]
    public class AdvancedBaseClass
    {
        private int baseField ;
    }
}
