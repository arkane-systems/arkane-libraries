#region header

// Arkane.Core - TypeExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-23 11:39 AM

#endregion

#region using

using System.Collections.Generic ;
using System.Linq ;
using System.Reflection ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

#endregion

// ReSharper disable once CheckNamespace
namespace System
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static partial class ας_System
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        ///     Get the inheritance hierarchy of a type; in reverse order, starting with the specified type
        ///     and proceeding to <see cref="System.Object" />.
        /// </summary>
        /// <param name="this">The type whose inheritance hierarchy to get.</param>
        /// <returns>The inheritance hierarchy of the specified type.</returns>
        /// C# Cookbook Third Edition, 13.6.
        [ItemNotNull]
        [JetBrains.Annotations.NotNull]
        public static IEnumerable <Type> GetBaseTypes (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this)
        {
            Type current = @this ;

            while (current != null)
            {
                yield return current ;

                current = current.GetTypeInfo ().BaseType ;
            }
        }

        /// <summary>
        ///     Get the types which subclass the specified type that are currently loaded into this
        ///     application domain.
        /// </summary>
        /// <param name="this">The superclass to check.</param>
        /// <returns>The subclasses of the specified superclass.</returns>
        /// Based on C# Cookbook Third Edition, 13.7.
        /// <exception cref="ReflectionTypeLoadException">
        ///     The assembly contains one or more types that cannot be loaded. The array
        ///     returned by the <see cref="P:System.Reflection.ReflectionTypeLoadException.Types" /> property of this exception
        ///     contains a <see cref="T:System.Type" /> object for each type that was loaded and null for each type that could not
        ///     be loaded, while the <see cref="P:System.Reflection.ReflectionTypeLoadException.LoaderExceptions" /> property
        ///     contains an exception for each type that could not be loaded.
        /// </exception>
        [ItemNotNull]
        [JetBrains.Annotations.NotNull]
        public static IEnumerable <Type> GetLoadedSubclasses (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this)
        {
            // ReSharper disable once ExceptionNotDocumented
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies () ;

            return from a in assemblies
                   from type in a.GetTypes ()
                   where type.IsSubclassOf (@this)
                   select type ;
        }

        /// <summary>
        ///     Get the name of a type _as it would be seen in the source code_ ; i.e., with the names of generic types
        ///     duly unmangled.
        /// </summary>
        /// <param name="this">The type whose name to get.</param>
        /// <returns>The type name as it would appear in source code.</returns>
        [JetBrains.Annotations.NotNull]
        public static string GetSourceCodeName (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this)
        {
            if (@this.GetTypeInfo ().IsGenericType)
            {
                string name = @this.GetGenericTypeDefinition ().Name ;
                name = name.Substring (0, name.IndexOf ('`')) ;

                Type[] arguments     = @this.GetTypeInfo ().GenericTypeArguments ;
                string innerTypeName = string.Join (",", arguments.Select (x => x.GetSourceCodeName ()).ToArray ()) ;

                return $"{name}<{innerTypeName}>" ;
            }

            return @this.Name ;
        }

        /// <summary>
        ///     Check if a given type has a default constructor.
        /// </summary>
        /// <param name="this">The type to check.</param>
        /// <returns>True if the type has a default constructor; false otherwise.</returns>
        public static bool HasDefaultConstructor (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this)
            => @this.GetTypeInfo ()
                    .DeclaredConstructors
                    .Where (c => c.IsPublic && !c.IsStatic)
                    .Any (x => x.GetParameters ().Length == 0) ;

        /// <summary>
        ///     Check if a given type has a specific other type as a generic type argument.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="genericType">The type to check for.</param>
        /// <returns>True if type is generic, is closed, and has genericType as a type argument; false otherwise.</returns>
        public static bool HasGenericTypeArgument (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type type,
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            Type genericType)
        {
            if (type.GetTypeInfo ().IsGenericType)
            {
                Type[] genericParameters = type.GenericTypeArguments ;

                return
                    genericParameters.Where (typeParam => !typeParam.IsGenericParameter)
                                     .Any (typeParam => typeParam.FullName == genericType.FullName) ;
            }

            return false ;
        }

        /// <summary>
        ///     Returns a value indicating if typeToCheck descends from a specific generic type.
        ///     Calls are like this: myType.IsDerivedFromGenericType(typeof(MyGenericType&lt;&gt;)).
        /// </summary>
        /// <param name="typeToCheck">The type to check</param>
        /// <param name="genericType">The generic type to check against (e.g. typeof(List&lt;&gt;)).</param>
        internal static bool IsDerivedFromGenericType (this Type? typeToCheck, Type genericType)
        {
            /* This method derived from code from here: http://www.postsharp.net/blog/post/Validation-with-PostSharp */

            if (typeToCheck == typeof (object))
                return false ;
            if (typeToCheck == null)
                return false ;
            if (typeToCheck.GetTypeInfo ().IsGenericType && (typeToCheck.GetGenericTypeDefinition () == genericType))
                return true ;

            return typeToCheck.GetTypeInfo ().BaseType.IsDerivedFromGenericType (genericType) ;
        }

        /// <summary>
        ///     Selects non-abstract types from an IEnumerable{Type} marked with the specified attribute <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The attribute to select on the basis of.</typeparam>
        /// <param name="types">An IEnumerable of types to select from.</param>
        /// <param name="includeInherited">If true (the default), includes inherited attributes.</param>
        /// <returns>The subset of the input types marked with the specified attribute.</returns>
        [JetBrains.Annotations.NotNull]
        public static IEnumerable <Type> MarkedWith <T> (
            [ItemNotNull] [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this IEnumerable <Type>
                types,
            bool includeInherited = true) where T : Attribute
            => types.Where (type => !type.GetTypeInfo ().IsAbstract &&
                                    type.GetTypeInfo ().IsDefined (typeof (T), includeInherited)) ;

        #region Instantiation

        /// <summary>
        ///     Create an instance of the specified object type.
        /// </summary>
        /// <param name="type">The type to create an instance of.</param>
        /// <param name="constructorParameters">Optional constructor parameters.</param>
        /// <returns>The created instance.</returns>
        /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
        /// <exception cref="T:System.MethodAccessException">
        ///     In the [.NET for Windows Store apps](http://go.microsoft.com/fwlink/?LinkID=247912) or the [Portable Class
        ///     Library](~/docs/standard/cross-platform/cross-platform-development-with-the-portable-class-library.md), catch the
        ///     base class exception, <see cref="T:System.MemberAccessException"></see>, instead.
        ///     The caller does not have permission to call this constructor.
        /// </exception>
        /// <exception cref="T:System.MemberAccessException">
        ///     Cannot create an instance of an abstract class, or this member was
        ///     invoked with a late-binding mechanism.
        /// </exception>
        /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">
        ///     The COM type was not obtained through
        ///     <see cref="System.Type.GetTypeFromProgID(string)"></see> or
        ///     <see cref="System.Type.GetTypeFromCLSID(Guid)"></see>.
        /// </exception>
        /// <exception cref="T:System.MissingMethodException">
        ///     In the [.NET for Windows Store apps](http://go.microsoft.com/fwlink/?LinkID=247912) or the [Portable Class
        ///     Library](~/docs/standard/cross-platform/cross-platform-development-with-the-portable-class-library.md), catch the
        ///     base class exception, <see cref="T:System.MissingMemberException"></see>, instead.
        ///     No matching public constructor was found.
        /// </exception>
        /// <exception cref="T:System.Runtime.InteropServices.COMException">
        ///     <paramref name="type">type</paramref> is a COM object
        ///     but the class identifier used to obtain the type is invalid, or the identified class is not registered.
        /// </exception>
        /// <exception cref="T:System.TypeLoadException"><paramref name="type">type</paramref> is not a valid type.</exception>
        [JetBrains.Annotations.NotNull]
        public static object CreateInstance (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type type,
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            params object[] constructorParameters)
            => Activator.CreateInstance (type, constructorParameters) ;

        /// <summary>
        ///     Create an instance of the specified generic object type.
        /// </summary>
        /// <param name="type">The generic object type to create an instance of.</param>
        /// <param name="typeArguments">The type arguments to use with the generic object type.</param>
        /// <param name="constructorParameters">Optional constructor parameters.</param>
        /// <returns>The created instance.</returns>
        /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
        /// <exception cref="T:System.MethodAccessException">
        ///     In the [.NET for Windows Store apps](http://go.microsoft.com/fwlink/?LinkID=247912) or the [Portable Class
        ///     Library](~/docs/standard/cross-platform/cross-platform-development-with-the-portable-class-library.md), catch the
        ///     base class exception, <see cref="T:System.MemberAccessException"></see>, instead.
        ///     The caller does not have permission to call this constructor.
        /// </exception>
        /// <exception cref="T:System.MemberAccessException">
        ///     Cannot create an instance of an abstract class, or this member was
        ///     invoked with a late-binding mechanism.
        /// </exception>
        /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">
        ///     The COM type was not obtained through
        ///     <see cref="System.Type.GetTypeFromProgID(string)"></see> or
        ///     <see cref="System.Type.GetTypeFromCLSID(Guid)"></see>.
        /// </exception>
        /// <exception cref="T:System.MissingMethodException">
        ///     In the [.NET for Windows Store apps](http://go.microsoft.com/fwlink/?LinkID=247912) or the [Portable Class
        ///     Library](~/docs/standard/cross-platform/cross-platform-development-with-the-portable-class-library.md), catch the
        ///     base class exception, <see cref="T:System.MissingMemberException"></see>, instead.
        ///     No matching public constructor was found.
        /// </exception>
        /// <exception cref="T:System.Runtime.InteropServices.COMException">
        ///     <paramref name="type">type</paramref> is a COM object
        ///     but the class identifier used to obtain the type is invalid, or the identified class is not registered.
        /// </exception>
        /// <exception cref="T:System.TypeLoadException"><paramref name="type">type</paramref> is not a valid type.</exception>
        /// <exception cref="T:System.ArgumentException">type is not an open generic type.</exception>
        [JetBrains.Annotations.NotNull]
        public static object CreateGenericInstance (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type type,
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            Type[] typeArguments,
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            params object[] constructorParameters)
        {
            if (!type.GetTypeInfo ().IsGenericTypeDefinition)
                throw new ArgumentException (@"type is not an open generic type.", nameof (type)) ;

            Type closedType = type.MakeGenericType (typeArguments) ;
            return Activator.CreateInstance (closedType, constructorParameters) ;
        }

        #endregion Instantiation

        #region Method overrides

        /// <summary>
        ///     Return a collection of the methods which the specified type overrides.
        /// </summary>
        /// <param name="this">The type to check.</param>
        /// <returns>The method which this type overrides.</returns>
        /// Based on C# Cookbook Third Edition, 13.3.
        [JetBrains.Annotations.NotNull]
        public static IEnumerable <MethodInfo> FindMethodOverrides (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this)
        {
            MethodInfo[] methods =
                @this.GetMethods (BindingFlags.Instance  |
                                  BindingFlags.NonPublic |
                                  BindingFlags.Public    |
                                  BindingFlags.DeclaredOnly) ;

            IEnumerable <MethodInfo> mis = from ms in methods
                                           where ms != ms.GetBaseDefinition ()
                                           select ms.GetBaseDefinition () ;

            return mis ;
        }

        /// <summary>
        ///     Determine if a specific method on a specific type overrides a method of its base class.
        /// </summary>
        /// <param name="this">The type to check.</param>
        /// <param name="methodName">The name of the method to check.</param>
        /// <param name="paramTypes">An array of the parameter types of the method to check.</param>
        /// <returns>True if the specified method is an override; false otherwise.</returns>
        /// Based on C# Cookbook Third Edition, 13.3.
        /// <exception cref="AmbiguousMatchException">
        ///     More than one method is found with the specified name and specified
        ///     parameters.
        /// </exception>
        /// <exception cref="ArgumentException">Specified method does not exist.</exception>
        public static bool IsMethodOverride (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this,
            [JetBrains.Annotations.NotNull] [Required]
            string methodName,
            Type[]? paramTypes)
        {
            MethodInfo method = @this.GetMethod (methodName, paramTypes ?? new Type[0]) ;

            if (method == null)
                throw new ArgumentException (@"Specified method does not exist.") ;

            MethodInfo baseDef = method.GetBaseDefinition () ;

            if (baseDef != method)
            {
                IEnumerable <ParameterInfo> match = from p in baseDef.GetParameters ()
                                                    join op in paramTypes
                                                        on p.ParameterType.UnderlyingSystemType equals
                                                        op.UnderlyingSystemType
                                                    select p ;

                if (match.Any ())
                    return true ;
            }

            return false ;
        }

        #endregion Method overrides

        #region Attributes

        /// <summary>
        ///     Gets the first attribute of type <typeparamref name="T" /> defined on the type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to return.</typeparam>
        /// <param name="this">The type to return the attribute of.</param>
        /// <param name="includeInherited">If true (the default), includes inherited attributes.</param>
        /// <returns>An attribute of type <typeparamref name="T" />, or null if none was found.</returns>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        public static T? GetAttribute <T> (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this,
            bool includeInherited = true) where T : Attribute
            => @this.GetAttributes <T> (includeInherited).FirstOrDefault () ;

        /// <summary>
        ///     Gets all attributes of type <typeparamref name="T" /> defined on the type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to return.</typeparam>
        /// <param name="this">The type to return the attributes of.</param>
        /// <param name="includeInherited">If true (the default), includes inherited attributes.</param>
        /// <returns>An enumerable of attributes of type <typeparamref name="T" />.</returns>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        [JetBrains.Annotations.NotNull]
        public static IEnumerable <T> GetAttributes <T> (
                [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                this Type @this,
                bool includeInherited = true)

            // ReSharper disable once ExceptionNotDocumented
            where T : Attribute => @this.GetTypeInfo ().GetCustomAttributes (typeof (T), includeInherited).Cast <T> () ;

        #endregion Attributes

        #region Params alternatives

        /// <summary>
        ///     Searches for a public instance constructor whose parameters match the specified types.
        /// </summary>
        /// <param name="type">Type to search.</param>
        /// <param name="types">Parameter types for the constructor.</param>
        /// <returns>A <see cref="ConstructorInfo" /> representing the constructor, or null if none is found.</returns>
        public static ConstructorInfo? GetConstructor (this Type type, params Type[] types) =>
            type.GetConstructor (types) ;

        /// <summary>
        ///     Searches for a public method whose parameters match the specified types.
        /// </summary>
        /// <param name="type">Type to search.</param>
        /// <param name="name">Name of the method to search for.</param>
        /// <param name="types">Parameter types for the method.</param>
        /// <returns>A <see cref="MethodInfo" /> representing the method, or null if none is found.</returns>
        public static MethodInfo? GetMethod (this Type type, string name, params Type[] types) =>
            type.GetMethod (name, types) ;

        #endregion

        #region Nullability

        /// <summary>
        ///     Returns whether or not the specified type is <see cref="Nullable{T}" />.
        /// </summary>
        /// <param name="this">A <see cref="Type" />.</param>
        /// <returns>True if the specified type is <see cref="Nullable{T}" />; otherwise, false.</returns>
        /// <remarks>Use <see cref="Nullable.GetUnderlyingType" /> to access the underlying type.</remarks>
        public static bool IsNullableType (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this)
            => @this.GetTypeInfo ().IsGenericType && (@this.GetGenericTypeDefinition () == typeof (Nullable <>)) ;

        /// <summary>
        ///     Returns whether or not the specified type permits null values; i.e., is a Nullable or a reference type.
        /// </summary>
        /// <param name="this">A <see cref="Type" />.</param>
        /// <returns>True if the specified type permits null values; false, otherwise.</returns>
        public static bool AllowsNullValue (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            this Type @this)
            => !@this.GetTypeInfo ().IsValueType || @this.IsNullableType () ;

        #endregion Nullability
    }
}
