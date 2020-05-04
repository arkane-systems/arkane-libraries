#region header

// Arkane.Core - NullTerminatorAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 9:47 AM

#endregion

#region using

using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Linq ;
using System.Reflection ;

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
    ///     An aspect applied to a property or field (of an appropriate type; see list in source code) to prevent it from
    ///     ever returning nulls.  Null strings are converted to String.Empty; null arrays are converted to zero-length
    ///     arrays; null generic collections are converted to empty generic collections.
    /// </summary>
    /// <remarks>
    ///     Builds all the empties it needs at compile-time, so should be lightweight in use.
    /// </remarks>
    [AttributeUsage (AttributeTargets.Field | AttributeTargets.Property)]
    [ProvideAspectRole (StandardRoles.Caching)]
    [PSerializable]
    [PublicAPI]
    public sealed class NullTerminatorAttribute : LocationInterceptionAspect
    {
        /// <summary>
        ///     The various types and interfaces that this aspect is applicable to, as a field or property.
        /// </summary>

        // ReSharper disable once MemberInitializerValueIgnored
        public static Type[] ApplicableTo = null! ;

        static NullTerminatorAttribute () =>
            NullTerminatorAttribute.ApplicableTo = new[]
                                                   {
                                                       typeof (string),
                                                       typeof (Array),
                                                       typeof (IEnumerable <>),
                                                       typeof (ICollection <>),
                                                       typeof (IReadOnlyCollection <>),
                                                       typeof (Collection <>),
                                                       typeof (ReadOnlyCollection <>),
                                                       typeof (IList <>),
                                                       typeof (List <>),
                                                       typeof (IReadOnlyList <>),
                                                       typeof (SortedList <,>),
                                                       typeof (IDictionary <,>),
                                                       typeof (Dictionary <,>),
                                                       typeof (IReadOnlyDictionary <,>),
                                                       typeof (SortedDictionary <,>)
                                                   } ;

        private object? cachedNullSubstitute ;

        /// <inheritdoc />
        public override bool CompileTimeValidate (LocationInfo locationInfo)
        {
            if ((locationInfo.LocationKind == LocationKind.Property) && (locationInfo.PropertyInfo.CanRead != true))
            {
                // Error out, can only be used on readable properties.
                Message.Write (locationInfo,
                               SeverityType.Fatal,
                               @"MustBeReadable",
                               Resources.NullTerminatorAttribute_CompileTimeValidate_MustBeReadableProperty,
                               locationInfo) ;

                return false ;
            }

            Type fundamentalType = locationInfo.LocationType.GetTypeInfo ().IsGenericType
                                       ? locationInfo.LocationType.GetGenericTypeDefinition ()
                                       : locationInfo.LocationType ;

            // Fixup for arrays.
            if (fundamentalType.GetTypeInfo ().IsSubclassOf (typeof (Array)))
                fundamentalType = typeof (Array) ;

            // Checking.
            if (!NullTerminatorAttribute.ApplicableTo.Contains (fundamentalType))
            {
                // Error out, can only handle certain types.
                Message.Write (locationInfo,
                               SeverityType.Fatal,
                               @"MustBeCompatibleType",
                               Resources.NullTerminatorAttribute_CompileTimeValidate_MustBeCompatibleType,
                               locationInfo) ;

                return false ;
            }

            return true ;
        }

        #region Overrides of LocationInterceptionAspect

        /// <inheritdoc />
        public override void OnGetValue (LocationInterceptionArgs args)
        {
            args.ProceedGetValue () ;

            // if the property is null, return the cached null substitute.
            args.Value ??= this.cachedNullSubstitute ;
        }

        /// <inheritdoc />
        /// <exception cref="T:System.InvalidOperationException">Impossible non-generic type in NullTerminatorAttribute.</exception>
        public override void CompileTimeInitialize (LocationInfo targetLocation, AspectInfo aspectInfo)
        {
            base.CompileTimeInitialize (targetLocation, aspectInfo) ;

            // Get the type of the location we're targeting.
            Type locationType = targetLocation.LocationType ;

            // Generate the null substitute value.

            // typeof (string)
            if (locationType == typeof (string))
            {
                this.cachedNullSubstitute = string.Empty ;
                return ;
            }

            // typeof (array)
            if (locationType.GetTypeInfo ().IsSubclassOf (typeof (Array)))
            {
                this.cachedNullSubstitute = Activator.CreateInstance (locationType, 0) ;
                return ;
            }

            // Beyond this point, all we should have are generics.
            if (!locationType.IsConstructedGenericType)
                throw new InvalidOperationException (Resources.NullTerminatorAttribute_CompileTimeInitialize_ImpossibleNonGenericType) ;

            Type fundamentalType = locationType.GetGenericTypeDefinition () ;
            Type firstType       = locationType.GenericTypeArguments[0] ;

            // typeof (IEnumerable<>)
            if (fundamentalType == typeof (IEnumerable <>))
            {
                MethodInfo method = typeof (Enumerable).GetRuntimeMethod ("Empty", new Type[] { }) ;

                //MethodInfo method = typeof (Enumerable).GetMethod ("Empty", BindingFlags.Static | BindingFlags.Public);
                method                    = method.MakeGenericMethod (firstType) ;
                this.cachedNullSubstitute = method.Invoke (null, null) ;
                return ;
            }

            // typeof (Collection<>)
            // typeof (ICollection<>)
            if ((fundamentalType == typeof (Collection <>)) || (locationType == typeof (ICollection <>)))
            {
                Type collectionType = typeof (Collection <>).MakeGenericType (firstType) ;
                this.cachedNullSubstitute = Activator.CreateInstance (collectionType) ;
                return ;
            }

            // typeof (ReadOnlyCollection<>)
            // typeof (IReadOnlyCollection<>)
            if ((fundamentalType == typeof (ReadOnlyCollection <>)) || (locationType == typeof (IReadOnlyCollection <>)))
            {
                Type collectionType = typeof (ReadOnlyCollection <>).MakeGenericType (firstType) ;
                this.cachedNullSubstitute = Activator.CreateInstance (collectionType) ;
                return ;
            }

            // typeof (List<>)
            // typeof (IList<>)
            // typeof (IReadOnlyList<>)
            if ((fundamentalType == typeof (List <>))  ||
                (locationType    == typeof (IList <>)) ||
                (locationType    == typeof (IReadOnlyList <>)))
            {
                Type collectionType = typeof (List <>).MakeGenericType (firstType) ;
                this.cachedNullSubstitute = Activator.CreateInstance (collectionType) ;
                return ;
            }

            // Beyond this point, all we should have are two-parameter generics.
            if (locationType.GenericTypeArguments.Length <= 1)
                throw new InvalidOperationException (Resources.NullTerminatorAttribute_CompileTimeInitialize_ImpossibleSingleParameterGenericType) ;

            Type secondType = locationType.GenericTypeArguments[1] ;

            // typeof (IDictionary<,>)
            // typeof (Dictionary<,>)
            // typeof (IReadOnlyDictionary<,>)
            if ((fundamentalType == typeof (Dictionary <,>))  ||
                (fundamentalType == typeof (IDictionary <,>)) ||
                (fundamentalType == typeof (IReadOnlyDictionary <,>)))
            {
                Type collectionType = typeof (Dictionary <,>).MakeGenericType (firstType, secondType) ;
                this.cachedNullSubstitute = Activator.CreateInstance (collectionType) ;
                return ;
            }

            // typeof (SortedList<,>)
            if (fundamentalType == typeof (SortedList <,>))
            {
                Type collectionType = typeof (SortedList <,>).MakeGenericType (firstType, secondType) ;
                this.cachedNullSubstitute = Activator.CreateInstance (collectionType) ;
                return ;
            }

            // typeof (SortedDictionary<,>)
            // typeof (SortedList<,>)
            if (fundamentalType == typeof (SortedDictionary <,>))
            {
                Type collectionType = typeof (SortedDictionary <,>).MakeGenericType (firstType, secondType) ;
                this.cachedNullSubstitute = Activator.CreateInstance (collectionType) ;
                return ;
            }

            throw new InvalidOperationException (Resources.NullTerminatorAttribute_CompileTimeInitialize_ImpossibleMultipleParameterGenericType) ;
        }

        #endregion
    }
}
