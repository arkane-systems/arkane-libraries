#region header

// Arkane.Annotations - InformationAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 1:11 AM

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
    ///     Causes a compile-time informational message to be attached to the target code element.
    /// </summary>
    [AspectRoleDependency (AspectDependencyAction.Conflict, "PostCompile")]
    [AttributeUsage (AttributeTargets.All)]
    [MulticastAttributeUsage (MulticastTargets.All)]
    [ProvideAspectRole ("PostCompile")]
    [PublicAPI]
    public sealed class InformationAttribute : ScalarConstraint
    {
        /// <summary>
        ///     Causes a compile-time informational message to be attached to the target code element.
        /// </summary>
        /// <param name="reason">The text of the compile-time message.</param>
        /// <param name="important">
        ///     Display the message at <see cref="SeverityType.ImportantInfo" /> rather than
        ///     <see cref="SeverityType.Info" />.
        /// </param>
        public InformationAttribute (string reason    = "No specific informational text has been specified.",
                                     bool   important = true)
        {
            this.reason    = reason ;
            this.important = important ;
        }

        private readonly bool important ;

        private readonly string reason ;

        #region Overrides of Aspect

        /// <inheritdoc />
        public override void ValidateCode (object target)
        {
            Message.Write (MessageLocation.Of (target),
                           this.important ? SeverityType.ImportantInfo : SeverityType.Info,
                           "WA001",
                           this.reason) ;
        }

        #endregion
    }
}
