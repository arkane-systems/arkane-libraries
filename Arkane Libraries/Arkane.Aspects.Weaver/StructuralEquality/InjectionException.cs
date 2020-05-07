#region header

// Arkane.Aspects.Weaver - InjectionException.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 1:44 AM

#endregion

#region using

using System ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.StructuralEquality
{
    /// <summary>
    ///     Exceptions of this class are caught and turned to <c>Message.Write()</c> calls so that they show up as error
    ///     messages.
    /// </summary>
    internal class InjectionException : Exception
    {
        public InjectionException (string errorCode, string message) : base (message) => this.ErrorCode = errorCode ;

        public string ErrorCode { get ; }
    }
}
