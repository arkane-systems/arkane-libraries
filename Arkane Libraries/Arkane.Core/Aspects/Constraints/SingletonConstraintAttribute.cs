#region header

// Arkane.Core - SingletonConstraintAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 10:24 AM

#endregion

#region using

using System ;
using System.Linq ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Constraints ;
using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Constraints
{
    /// <summary>
    ///     A constraint which verifies that the class to which it is applied obeys the Singleton pattern
    ///     we use.
    /// </summary>
    /// <remarks>
    ///     The singleton pattern is as follows:
    ///     private static readonly Lazy{$CLASS$} lazy = new Lazy{$CLASS$} (() => new $CLASS$());
    ///     public static $CLASS$ Instance { get { return lazy.Value; } }
    ///     private $CLASS$ ()
    ///     {}
    /// </remarks>
    [AttributeUsage (AttributeTargets.Class)]
    [MulticastAttributeUsage (MulticastTargets.Class, Inheritance = MulticastInheritance.None)]
    [PublicAPI]
    public sealed class SingletonConstraintAttribute : ScalarConstraint
    {
        /// <summary>
        ///     Validates the element of code to which the constraint is applied.
        /// </summary>
        /// <param name="target">
        ///     Element of code to which the constraint is applied (<see cref="T:System.Reflection.Assembly" />,
        ///     <see cref="T:System.Type" />,
        ///     <see cref="T:System.Reflection.MethodInfo" />, <see cref="T:System.Reflection.ConstructorInfo" />,
        ///     <see cref="T:System.Reflection.PropertyInfo" />,
        ///     <see cref="T:System.Reflection.EventInfo" />, <see cref="T:System.Reflection.FieldInfo" />,
        ///     <see cref="T:System.Reflection.ParameterInfo" />).
        /// </param>
        public override void ValidateCode (object target)
        {
            var      targetType     = (Type) target ;
            TypeInfo targetTypeInfo = targetType.GetTypeInfo () ;

            // Check for private static readonly field, named lazy, typed Lazy<T>.
            // Make required generic type.
            Type generic = typeof (Lazy <>).MakeGenericType (targetType) ;

            FieldInfo field = targetTypeInfo.DeclaredFields.Single (f => f.Name == "Lazy") ;

            if ((field            == null)    ||
                (field.IsPrivate  != true)    ||
                (field.IsStatic   != true)    ||
                (field.FieldType  != generic) ||
                (field.IsInitOnly != true))
                Message.Write (targetType,
                               SeverityType.Error,
                               "2001",
                               Resources.SingletonConstraintAttribute_ValidateCode_NoInstanceCreationMethod,
                               targetType.Name) ;

            // Check for public static property, read-only, named Instance, returning T.
            try
            {
                // ReSharper disable once ExceptionNotDocumented
                PropertyInfo property = targetTypeInfo.DeclaredProperties.Single (f => f.Name == "Instance") ;

                if ((property                    == null) ||
                    (property.GetMethod.IsStatic != true) ||
                    (property.GetMethod.IsPublic != true) ||
                    (property.CanRead            != true) ||
                    property.CanWrite)
                    Message.Write (targetType,
                                   SeverityType.Error,
                                   "2002",
                                   Resources.SingletonConstraintAttribute_ValidateCode_NoInstanceProperty,
                                   targetType.Name) ;
            }
            catch (AmbiguousMatchException)
            {
                Message.Write (targetType,
                               SeverityType.Error,
                               "2004",
                               Resources.SingletonConstraintAttribute_ValidateCode_AmbiguousInstanceProperty,
                               targetType.Name) ;
            }

            // Check for private parameterless constructor and absence of other constructors.
            ConstructorInfo[] constructors = targetTypeInfo.DeclaredConstructors.Where (c => c.IsStatic == false).ToArray () ;

            var ppc   = false ;
            var other = false ;

            foreach (ConstructorInfo c in constructors)
            {
                if (c.IsPrivate && (c.GetParameters ().Length == 0))
                    ppc = true ;
                else
                    other = true ;
            }

            if (!ppc || other)
                Message.Write (targetType,
                               SeverityType.Error,
                               "2003",
                               Resources.SingletonConstraintAttribute_ValidateCode_NoPrivateConstructor,
                               targetType.Name) ;
        }
    }
}
