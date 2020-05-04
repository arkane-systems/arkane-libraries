#region header

// Arkane.Core - SingleCallAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 12:59 AM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect to permit a method to be called once and only once per instance.
    /// </summary>
    [LinesOfCodeAvoided (5)]
    [PSerializable]
    [PublicAPI]
    public sealed class SingleCallAttribute : MethodInterceptionAspect, IInstanceScopedAspect
    {
        private bool calledAlready ;

        /// <inheritdoc />
        public object CreateInstance (AdviceArgs adviceArgs) => this.MemberwiseClone () ;

        /// <inheritdoc />
        public void RuntimeInitializeInstance () { this.calledAlready = false ; }

        /// <inheritdoc />
        /// <exception cref="T:System.InvalidOperationException">Method called more than once.</exception>
        public override void OnInvoke (MethodInterceptionArgs args)
        {
            if (!this.calledAlready)
                args.Proceed () ;
            else
                throw new InvalidOperationException ($"The method {args.Method.Name} can only be called once.") ;

            this.calledAlready = true ;
        }
    }
}
