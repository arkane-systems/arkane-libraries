#region header

// Arkane.Core - UnlessDisposedAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 1:23 AM

#endregion

#region using

using System ;
using System.Diagnostics ;
using System.Linq ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Extensibility ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect which blocks calls to all public methods of a class implementing <see cref="IDisposable" /> once the
    ///     class has been disposed, instead throwing <see cref="ObjectDisposedException" />. The class upon which this aspect
    ///     is placed must use the common pattern in which a private boolean field named 'disposed' is used to hold the
    ///     disposed state.
    /// </summary>
    /// <remarks>
    ///     Private methods, static methods, and finalizers can still run when the class is disposed, as they may
    ///     be required for cleanup.
    /// </remarks>
    [MulticastAttributeUsage (MulticastTargets.Method)]
    [ProvideAspectRole (StandardRoles.ExceptionHandling)]
    [PSerializable]
    [PublicAPI]
    public sealed class UnlessDisposedAttribute : MethodInterceptionAspect
    {
        /// <inheritdoc />
        public override bool CompileTimeValidate (MethodBase method)
        {
            // Only validate if declaring type implements IDisposable.
            Type type = method.DeclaringType ;
            Debug.Assert (type != null, "type != null") ;

            TypeInfo typeInfo = type.GetTypeInfo () ;

            if (!typeInfo.ImplementedInterfaces.Contains (typeof (IDisposable)))
            {
                Message.Write (MessageLocation.Of (method.DeclaringType),
                               SeverityType.Error,
                               "UD000",
                               Resources.UnlessDisposedAttribute_CompileTimeValidate_OnlyDisposableTypes) ;

                return false ;
            }

            FieldInfo? disposedField = typeInfo.DeclaredFields
                                               .Where (fi => fi.IsPrivate && !fi.IsStatic && (fi.FieldType == typeof (bool)))
                                               .Single (fi => fi.Name == "disposed") ;

            if (disposedField == null)
            {
                Message.Write (MessageLocation.Of (method.DeclaringType),
                               SeverityType.Error,
                               "UD001",
                               Resources.UnlessDisposedAttribute_CompileTimeValidate_RequiresDisposedField) ;

                return false ;
            }

            return base.CompileTimeValidate (method) ;
        }

        /// <inheritdoc />
        /// <exception cref="T:System.ObjectDisposedException">Object has already been disposed.</exception>
        public override void OnInvoke (MethodInterceptionArgs args)
        {
            // Let calls to private and/or static methods, or the constructor, or the finalizer, proceed immediately.
            if (args.Method.IsPrivate     ||
                args.Method.IsStatic      ||
                args.Method.IsConstructor ||
                (args.Method.Name == "Finalize"))
            {
                args.Proceed () ;
                return ;
            }

            // ReSharper disable once PossibleNullReferenceException
            var hasBeenDisposed = (bool) (args.Method?.DeclaringType?.GetTypeInfo ()
                                              .GetDeclaredField ("disposed")
                                              .GetValue (args.Instance) ??
                                          false) ;

            if (hasBeenDisposed)
                throw new ObjectDisposedException (args.Instance.GetType ().FullName) ;

            args.Proceed () ;
        }
    }
}
