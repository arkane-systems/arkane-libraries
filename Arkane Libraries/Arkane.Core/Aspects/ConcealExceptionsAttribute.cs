#region header

// Arkane.Core - ConcealExceptionsAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 9:30 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect concealing the details of inner exceptions from the end-user by wrapping them
    ///     in an ApplicationException with a user-level message.
    /// </summary>
    [ProvideAspectRole (StandardRoles.ExceptionHandling)]
    [PSerializable]
    [PublicAPI]
    public sealed class ConcealExceptionsAttribute : OnExceptionAspect
    {
        /// <summary>
        ///     An aspect concealing the details of inner exceptions from the end-user by wrapping them
        ///     in an ApplicationException with the message "An unknown error has occurred."
        /// </summary>
        public ConcealExceptionsAttribute ()
            : this (Resources.ConcealExceptionsAttribute_ConcealExceptionsAttribute_UnknownErrorHasOccurred)
        { }

        /// <summary>
        ///     An aspect concealing the details of inner exceptions from the end-user by wrapping them
        ///     in an ApplicationException with a specified user-level message.
        /// </summary>
        /// <param name="message">The user-level message.</param>
        public ConcealExceptionsAttribute (string message) => this.UserLevelMessage = message ;

        private string UserLevelMessage { get ; set ; }

        /// <exception cref="ApplicationException">
        ///     A (wrapped) inner exception occurred.
        ///     This exception contains user-level details.
        /// </exception>
        public override void OnException (MethodExecutionArgs args)
        {
            throw new ApplicationException (this.UserLevelMessage, args.Exception) ;
        }
    }
}
