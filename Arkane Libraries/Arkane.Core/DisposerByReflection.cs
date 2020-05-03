#region header

// Arkane.Core - DisposerByReflection.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 1:38 PM

#endregion

#region using

using System ;
using System.Linq ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;
using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Serves as a wrapper around objects that require disposal but that do not
    ///     implement <see cref="System.IDisposable" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of
    ///     <see cref="DisposerBase{T}.Object" />
    /// </typeparam>
    [PublicAPI]
    [ExplicitlySynchronized]
    public class DisposerByReflection <T> : DisposerBase <T>
    {
        /// <summary>
        ///     Instantiates a new <see cref="DisposerByReflection{T}" /> object with the
        ///     specified <paramref name="obj" /> and the specified public,
        ///     parameterless instance <see cref="Method" /> named
        ///     <paramref name="methodName" />.
        /// </summary>
        /// <param name="obj">
        ///     The value of
        ///     <see cref="DisposerBase{T}.Object" />.
        /// </param>
        /// <param name="methodName">
        ///     The name of <see cref="Method" />; must be a
        ///     public, parameterless instance method.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="obj" /> or
        ///     <paramref name="methodName" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="obj" /> does not
        ///     have a public, parameterless instance method named
        ///     <paramref name="methodName" />.
        /// </exception>
        public DisposerByReflection ([JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
                                     T obj,
                                     [Required] string methodName = "Dispose")
            : this (obj, DisposerByReflection <T>.GetPublicParameterlessInstanceMethod (obj, methodName))
        { }

        /// <summary>
        ///     Instantiates a new <see cref="DisposerByReflection{T}" /> object with the
        ///     specified <paramref name="obj" /> and <paramref name="method" />.
        /// </summary>
        /// <param name="obj">
        ///     The value of
        ///     <see cref="DisposerBase{T}.Object" />.
        /// </param>
        /// <param name="method">
        ///     The value of <see cref="Method" />; must be a member
        ///     of <paramref name="obj" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="obj" /> or
        ///     <paramref name="method" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="method" /> is
        ///     static, has parameters, or has generic parameters.
        /// </exception>
        public DisposerByReflection (T          obj,
                                     MethodInfo method)
            : base (obj)
        {
            if (method.IsStatic)
                throw new ArgumentException (Resources.DisposerByReflection_DisposerByReflection_CantBeStaticMethod,
                                             nameof (method)) ;

            if (method.GetParameters ().Length != 0)
                throw new ArgumentException (Resources.DisposerByReflection_DisposerByReflection_CantBeMethodWithParameters,
                                             nameof (method)) ;

            if (method.ContainsGenericParameters)
                throw new ArgumentException (Resources
                                                .DisposerByReflection_DisposerByReflection_disposer_CantBeMethodWithUnassignedTypeParams,
                                             nameof (method)) ;

            this.Method = method ;
        }

        /// <summary>
        ///     The <see cref="System.Reflection.MethodInfo" /> to which
        ///     <see cref="DisposerBase{T}.Dispose()" /> will be delegated.
        /// </summary>
        public MethodInfo Method { get ; }

        private static MethodInfo GetPublicParameterlessInstanceMethod (
            [JetBrains.Annotations.NotNull] [PostSharp.Patterns.Contracts.NotNull]
            T obj,
            [Required] string methodName)
        {
            MethodInfo method = obj!.GetType ()
                                    .GetTypeInfo ()
                                    .DeclaredMethods
                                    .SingleOrDefault (m => m.IsPublic &&
                                                           !m.IsStatic &&
                                                           m.CallingConvention.HasFlag (CallingConventions.HasThis) &&
                                                           (m.GetParameters ().Length == 0)) ;

            if (method == null)
                throw new ArgumentException (
                                             string.Format (
                                                            Resources
                                                               .DisposerByReflection_GetPublicParameterlessInstanceMethod_NoPublicParamlessInstanceMethod,
                                                            methodName)) ;

            return method ;
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the
        ///     <see cref="DisposerByReflection{T}" /> and optionally releases the
        ///     managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     If <c>true</c>, releases both managed and
        ///     unmanaged resources, otherwise releases only unmanaged
        ///     resources.
        /// </param>
        /// <exception cref="Exception">Delegated disposer threw an exception.</exception>
        protected override void DisposeImplementation (bool disposing)
        {
            if (!disposing)
                return ;

            try
            {
                this.Method.Invoke (this.Object, null) ;
            }
            catch (TargetInvocationException ex)
            {
                // ReSharper disable once ThrowingSystemException
                if (ex.InnerException != null)
                    throw ex.InnerException ;

                throw ;
            }
        }
    }
}
