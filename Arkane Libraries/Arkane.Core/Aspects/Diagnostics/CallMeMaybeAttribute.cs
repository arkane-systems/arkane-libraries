#region header

// Arkane.Core - CallMeMaybeAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 12:18 PM

#endregion

#region using

using System ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Diagnostics
{
    /// <summary>
    ///     An aspect for use in testing of variable-reliability conditions, by succeeding in calling the
    ///     underlying method only part of the time, returning an exception the rest of the time.
    /// </summary>
    [LinesOfCodeAvoided (13)]
    [PSerializable]
    [PublicAPI]
    public sealed class CallMeMaybeAttribute : MethodInterceptionAspect
    {
        /// <summary>
        ///     An aspect for use in testing of variable-reliability conditions, by succeeding in calling the
        ///     underlying method only part of the time, returning an exception the rest of the time.
        /// </summary>
        /// <param name="probability">The probability that the marked method WILL be called.</param>
        /// <param name="exceptionType">
        ///     The exception to be thrown if the method is NOT called (defaults to
        ///     InvalidOperationException).
        /// </param>
        /// <exception cref="ArgumentException">Only exceptions can be thrown by this aspect.</exception>
        public CallMeMaybeAttribute (double probability = 0.9, Type? exceptionType = null)
        {
            this.probability   = probability ;
            this.exceptionType = exceptionType ?? typeof (InvalidOperationException) ;

            if (!typeof (Exception).GetTypeInfo ().IsAssignableFrom (this.exceptionType.GetTypeInfo ()))
                throw new ArgumentException (Resources.CallMeMaybeAttribute_CallMeMaybeAttribute_MustBeExceptionType,
                                             nameof (exceptionType)) ;
        }

        /// <inheritdoc />
        public override void OnInvoke ([NotNull] MethodInterceptionArgs args)
        {
            double chance = new Random ().NextDouble () ;

            if (chance < this.probability)
            {
                args.Proceed () ;
            }
            else
            {
                // Replace with exception.
                var ex = (Exception) Activator.CreateInstance (this.exceptionType) ;

                // ReSharper disable once ThrowingSystemException
                throw ex ;
            }
        }

        // The below cannot be readonly because the class is PSerializable.
#pragma warning disable IDE0044 // Add readonly modifier
        private Type   exceptionType ;
        private double probability ;
#pragma warning restore IDE0044 // Add readonly modifier
    }
}
