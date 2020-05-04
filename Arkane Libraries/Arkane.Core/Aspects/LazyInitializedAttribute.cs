#region header

// Arkane.Core - LazyInitializedAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 2:12 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Extensibility ;
using PostSharp.Reflection ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect applied to a property to make it lazy-load; i.e., only run its get method the first time it
    ///     is called, and subsequently pull its value from a backing field instead.
    /// </summary>
    [AttributeUsage (AttributeTargets.Property)]
    [ProvideAspectRole (StandardRoles.Caching)]
    [PSerializable]
    [PublicAPI]
    public sealed class LazyInitializedAttribute : LocationInterceptionAspect, IInstanceScopedAspect
    {
        [PNonSerialized]
        private object? backing ;

        [PNonSerialized]
        private object? fieldLock ;

        /// <inheritdoc />
        public override void OnGetValue (LocationInterceptionArgs args)
        {
            // Double-checked locking pattern.
            if (this.backing == null)
                lock (this.fieldLock!)
                {
                    if (this.backing == null)
                    {
                        args.ProceedGetValue () ;
                        this.backing = args.Value ;
                    }
                }

            args.Value = this.backing ;
        }

        /// <inheritdoc />
        public override bool CompileTimeValidate (LocationInfo locationInfo)
        {
            if ((locationInfo.PropertyInfo.CanRead != true) || locationInfo.PropertyInfo.CanWrite)
            {
                // Error out, can only be used on read-only properties.
                Message.Write (locationInfo,
                               SeverityType.Fatal,
                               @"MustBeReadOnly",
                               Resources.LazyInitializedAttribute_CompileTimeValidate_MustBeReadOnlyProperty,
                               locationInfo) ;

                return false ;
            }

            return true ;
        }

        #region IInstanceScopedAspect Members

        /// <inheritdoc />
        [NotNull]
        public object CreateInstance (AdviceArgs adviceArgs) => this.MemberwiseClone () ;

        /// <inheritdoc />
        public void RuntimeInitializeInstance () { this.fieldLock = new object () ; }

        #endregion
    }
}
