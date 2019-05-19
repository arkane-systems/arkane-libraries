#region header

// Arkane.Annotations - WarningAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 1:08 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Aspects.Dependencies ;
using PostSharp.Constraints ;
using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     Causes a compile-time warning to be attached to the target code element.
    /// </summary>
    [AspectRoleDependency (AspectDependencyAction.Conflict, "PostCompile")]
    [AttributeUsage (AttributeTargets.All)]
    [MulticastAttributeUsage (MulticastTargets.All)]
    [ProvideAspectRole ("PostCompile")]
    [PublicAPI]
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
        public override void ValidateCode (object target) => Message.Write (MessageLocation.Of (target),
                                                                            SeverityType.Warning,
                                                                            "WA000",
                                                                            this.reason) ;

        #endregion
    }
}
