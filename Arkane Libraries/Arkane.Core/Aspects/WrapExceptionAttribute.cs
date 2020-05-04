#region header

// Arkane.Core - WrapExceptionAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 9:37 AM

#endregion

#region using

using System ;
using System.Reflection ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Wrap a given target exception type in another exception type, with a specific message in the outer exception.
    /// </summary>
    [ProvideAspectRole (StandardRoles.ExceptionHandling)]
    [PSerializable]
    [PublicAPI]
    public sealed class WrapExceptionAttribute : OnExceptionAspect
    {
        /// <summary>
        ///     Wrap a given target exception type in another exception type, with a specific message in the outer exception.
        /// </summary>
        /// <param name="expectedExceptionType">The target exception type to wrap.</param>
        /// <param name="wrapInExceptionType">The exception type to wrap around the target exception.</param>
        /// <param name="message">The message to place in the outer exception.</param>
        public WrapExceptionAttribute (Type   expectedExceptionType,
                                       Type   wrapInExceptionType,
                                       string message)
        {
            this.expectedException = expectedExceptionType ;
            this.wrapInException   = wrapInExceptionType ;
            this.message           = message ;
        }

        // This method checks to see if the exception that was thrown from the method the 
        // attribute was applied on matches the expected exception type we passed into the 
        // constructor of the attribute.
        /// <inheritdoc />
        public override Type GetExceptionType (MethodBase targetMethod) => this.expectedException ;

        // This method is called when we have guaranteed the exception type thrown matches the
        // expected exception type passed in the constructor.  It wraps the thrown exception 
        // into a new exception and adds the custom message that was passed into the constructor.
        /// <inheritdoc />
        public override void OnException (MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue ;
            var newException = (Exception) Activator.CreateInstance (
                                                                     this.wrapInException,
                                                                     this.message,
                                                                     args.Exception) ;

            // ReSharper disable once ExceptionNotDocumented
            // ReSharper disable once ThrowingSystemException
            throw newException ;
        }

        //! These fields cannot be made read-only, because the class is PSerializable.
        // ReSharper disable FieldCanBeMadeReadOnly.Local
#pragma warning disable IDE0044 // Add readonly modifier
        private Type   expectedException ;
        private string message ;
        private Type   wrapInException ;
#pragma warning restore IDE0044 // Add readonly modifier

        // ReSharper restore FieldCanBeMadeReadOnly.Local
    }
}
