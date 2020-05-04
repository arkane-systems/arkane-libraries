#region header

// Arkane.Core - InstantiateIfNullAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 9:45 AM

#endregion

#region using

using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Reflection ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     Aspect that ensures that a property getter always returns a valid instance that matches the property
    ///     type. This is useful to avoid redundant code for property getters, setters, and the relative field when
    ///     all we want is to be sure that a complex property is always readable.
    /// </summary>
    [AspectRoleDependency (AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.DataBinding)]
    [AspectTypeDependency (AspectDependencyAction.Commute, typeof (InstantiateIfNullAttribute))]
    [PSerializable]
    [PublicAPI]
    public sealed class InstantiateIfNullAttribute : LocationInterceptionAspect
    {
        /// <summary>
        ///     Aspect that ensures that a property getter always returns a valid instance that matches the property
        ///     type. This is useful to avoid redundant code for property getters, setters, and the relative field when
        ///     all we want is to be sure that a complex property is always readable.
        /// </summary>
        /// <param name="concreteType">Type used to instantiate the object when the property type is an interface.</param>
        public InstantiateIfNullAttribute (Type? concreteType = null) => this.ConcreteType = concreteType ;

        /// <summary>
        ///     Type used to instantiate the object when the property type is an interface.
        /// </summary>
        public Type? ConcreteType { get ; set ; }

        /// <inheritdoc />
        /// <exception cref="T:System.InvalidOperationException">The InstantiateIfNullAttribute can only be applied to properties.</exception>
        public override bool CompileTimeValidate (LocationInfo locationInfo)
        {
            if (locationInfo.PropertyInfo == null)
                throw new InvalidOperationException (Resources.InstantiateIfNullAttribute_CompileTimeValidate_OnlyForProperties) ;

            ConstructorInfo? defCtor = locationInfo.PropertyInfo.PropertyType.GetTypeInfo ()
                                                   .DeclaredConstructors
                                                   .Single (c => c.GetParameters ().Length == 0) ;

            if (defCtor == null)
                throw new InvalidOperationException (
                                                     $@"Type {locationInfo.PropertyInfo.PropertyType.Name} does not have a default constructor. InstantiateIfNullAttribute cannot be used.") ;

            return true ;
        }

        /// <inheritdoc />
        public override void OnGetValue (LocationInterceptionArgs args)
        {
            if (args.GetCurrentValue () == null)
            {
                object newValue = this.CreateValue (args) ;
                args.SetNewValue (newValue) ;
            }

            args.ProceedGetValue () ;
        }

        private object CreateValue (LocationInterceptionArgs args)
        {
            Type     propType = args.Location.PropertyInfo.PropertyType ;
            TypeInfo propInfo = propType.GetTypeInfo () ;

            if (propInfo.IsInterface)
                if (this.ConcreteType != null)
                    propType = this.ConcreteType ;
                else if (propType.IsDerivedFromGenericType (typeof (IList <>)))
                    propType = typeof (List <>).MakeGenericType (propInfo.GenericTypeArguments) ;

            object newValue ;

            if (propType.IsArray)
                newValue = Activator.CreateInstance (propType, 0) ;
            else if (propType == typeof (string))
                newValue = new string (new char[] { }) ;
            else
                newValue = Activator.CreateInstance (propType) ;

            return newValue ;
        }
    }
}
