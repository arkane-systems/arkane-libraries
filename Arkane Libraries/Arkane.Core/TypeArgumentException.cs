#region header

// Arkane.Core - TypeArgumentException.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 12:56 AM

#endregion

#region using

using System ;
using System.Runtime.Serialization ;
using System.Security.Permissions ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Patterns.Threading ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Exception thrown to indicate that an inappropriate type argument was used for a type parameter
    ///     to a generic type or method.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is intended to be used in cases where constraints are not available or sufficient to
    ///         constrain the type parameter; for example, where it is necessary to constrain to <see cref="Enum" />
    ///         or to types bearing specific attributes.
    ///     </para>
    ///     <para>
    ///         This exception is intended to be analogous to <see cref="ArgumentException" />.
    ///     </para>
    /// </remarks>
    [Serializable]
    [PublicAPI]
    [PrivateThreadAware]
    public sealed class TypeArgumentException : Exception //, ISerializable
    {
        /// <inheritdoc />
        public TypeArgumentException ()
            : base (Resources.TypeArgumentException_TypeArgumentException_InvalidTypeArgument)
        { }

        /// <inheritdoc />
        public TypeArgumentException (string message)
            : base (message)
        { }

        /// <inheritdoc />
        [SecurityPermission (SecurityAction.Demand, SerializationFormatter = true)]
        private TypeArgumentException ([NotNull] SerializationInfo info, StreamingContext context)
            : base (info, context) =>
            this.ParamName = info.GetString ("ParamName") ;

        /// <inheritdoc />
        public TypeArgumentException (string message, Exception innerException)
            : base (message, innerException)
        { }

        /// <summary>
        ///     Initializes a new instance of the ArgumentException class with a specified error message and the name of
        ///     the type parameter that causes this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the type parameter that caused the current exception.</param>
        public TypeArgumentException (string message, string paramName = "")
            : base (message) =>
            this.ParamName = paramName ;

        /// <summary>
        ///     Initializes a new instance of the TypeArgumentException class with a specified error message, the type parameter
        ///     name, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the type parameter that caused the current exception.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception. If the
        ///     innerException parameter is not a null reference, the current exception is raised in a catch block
        ///     that handles the inner exception.
        /// </param>
        public TypeArgumentException ([CanBeNull] string    message,
                                      [CanBeNull] string    paramName,
                                      [CanBeNull] Exception innerException)
            : base (message, innerException) =>
            this.ParamName = paramName ;

        /// <summary>
        ///     Gets the name of the type parameter that causes this exception.
        /// </summary>
        /// <remarks>
        ///     Every ArgumentException should carry the name of the type parameter that causes this exception.
        ///     This property is read-only, and returns the same value as was passed into the constructor. Overriding methods
        ///     should be used solely to customize the format of the parameter name.
        ///     The parameter name should not be translated for localization purposes.
        /// </remarks>
        public string ParamName { get ; } = "" ;

        /// <inheritdoc />
        [SecurityPermission (SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData ([NotNull] SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException (nameof (info)) ;

            info.AddValue ("ParamName", this.ParamName) ;

            base.GetObjectData (info, context) ;
        }
    }
}
