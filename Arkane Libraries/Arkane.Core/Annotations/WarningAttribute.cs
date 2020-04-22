#region header

// Arkane.Core - WarningAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 2:57 PM

#endregion

#region using

using System ;

using PostSharp.Constraints ;
using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     Causes a compile-time warning to be attached to the target code element.
    /// </summary>
    [Serializable]
    [AttributeUsage (AttributeTargets.All)]
    [MulticastAttributeUsage (MulticastTargets.All)]
    public sealed class WarningAttribute : ScalarConstraint
    {
        /// <summary>
        ///     Causes a compile-time warning to be attached to the target code element.
        /// </summary>
        /// <param name="reason">The text of the compile-time warning.</param>
        public WarningAttribute (string reason = "No specific warning text has been specified.") => this.reason = reason ;

        private readonly string reason ;

        #region Overrides of Aspect

        /// <inheritdoc />
        public override void ValidateCode (object target) =>
            Message.Write (MessageLocation.Of (target), SeverityType.Warning, "WA000", this.reason) ;

        #endregion
    }
}
