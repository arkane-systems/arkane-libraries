#region header

// Arkane.Core - AbstractBaseToStringAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:37 AM

#endregion

#region using

using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Base class for <see cref="ToStringAttribute" /> and <see cref="ToStringGlobalOptionsAttribute" />.
    /// </summary>
#pragma warning disable CS3015 // Type has no accessible constructors which use only CLS-compliant types
    public abstract class AbstractBaseToStringAttribute : MulticastAttribute
#pragma warning restore CS3015 // Type has no accessible constructors which use only CLS-compliant types
    {
        private protected AbstractBaseToStringAttribute () { }

        /// <summary>
        ///     The separator between a property or field name and value. The default is ":", as in "answer:42".
        /// </summary>
        public string PropertyNameToValueSeparator { get ; set ; } = ":" ;

        /// <summary>
        ///     The separator between two properties or fields. The default is ",", as in "answer:42,another:54".
        /// </summary>
        public string PropertiesSeparator { get ; set ; } = "," ;

        /// <summary>
        ///     If true, the short name of the type is added to the ToString. The default is true, as in "MyType1; answer:42".
        /// </summary>
        public bool WriteTypeName { get ; set ; } = true ;

        /// <summary>
        ///     If true, the ToString is result is wrapped in curly braces. The default is true, as in "{MyType1; answer:42}".
        /// </summary>
        public bool WrapWithBraces { get ; set ; } = true ;

        /// <summary>
        ///     If true, private members are also included in the ToString method. Default false (they are excluded).
        /// </summary>
        public bool IncludePrivate { get ; set ; } = false ;
    }
}
