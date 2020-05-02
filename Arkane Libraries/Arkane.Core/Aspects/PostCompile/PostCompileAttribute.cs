#region header

// Arkane.Core - PostCompileAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:16 AM

#endregion

#region using

using System ;
using System.Reflection ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Extensibility ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.PostCompile
{
    /// <summary>
    ///     An aspect that invokes the marked method upon compilation.
    ///     The marked method must not require any parameters, must return nothing, and must be static.
    /// </summary>
    [PublicAPI]
    [PSerializable]
    [AspectRoleDependency (AspectDependencyAction.Conflict, AspectRoles.InvokesOnPostCompile)]
    [AttributeUsage (AttributeTargets.Method)]
    [ProvideAspectRole (AspectRoles.InvokesOnPostCompile)]
    [LinesOfCodeAvoided (0)]
    public sealed class PostCompileAttribute : MethodLevelAspect
    {
        /// <summary>
        ///     In this aspect, invokes the on-compilation method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <returns>True if the invocation is successful; false if it is not.</returns>
        public override bool CompileTimeValidate (MethodBase method)
        {
            //if (method == null)
            //{
            //    Message.Write (MessageLocation.Unknown,
            //                   SeverityType.Error,
            //                   "PC001",
            //                   "The PostCompile aspect can only be applied to methods.") ;

            //    return false ;
            //}

            if (!(method is MethodInfo))
            {
                Message.Write (MessageLocation.Of (method.DeclaringType),
                               SeverityType.Error,
                               "PC004",
                               Resources.PostCompileAttribute_CompileTimeValidate_CannotApplyToConstructorsDestructors) ;

                return false ;
            }

            if (!method.IsStatic)
            {
                Message.Write (MessageLocation.Of (method),
                               SeverityType.Error,
                               "PC002",
                               Resources.PostCompileAttribute_CompileTimeValidate_CanOnlyApplyToStaticMethods) ;

                return false ;
            }

            if (method.GetParameters ().Length > 0)
            {
                Message.Write (MessageLocation.Of (method),
                               SeverityType.Error,
                               "PC003",
                               Resources.PostCompileAttribute_CompileTimeValidate_CanOnlyApplyToParameterless) ;

                return false ;
            }

            if (((MethodInfo) method).ReturnType != typeof (void))
            {
                Message.Write (MessageLocation.Of (method),
                               SeverityType.Error,
                               "PC005",
                               Resources.PostCompileAttribute_CompileTimeValidate_CanOnlyApplyToVoidReturn) ;

                return false ;
            }

            try
            {
                method.Invoke (null, null) ;

                Message.Write (MessageLocation.Of (method),
                               SeverityType.ImportantInfo,
                               "PC000",
                               Resources.PostCompileAttribute_CompileTimeValidate_LastPostCompileRunSucceeded,
                               DateTime.Now) ;
            }
            catch (TargetInvocationException ex)
            {
                Message.Write (MessageLocation.Of (method),
                               SeverityType.Error,
                               "PC006",
                               Resources.PostCompileAttribute_CompileTimeValidate_LastPostCompileRunFailed,
                               DateTime.Now,
                               ex.InnerException?.Message) ;
                return false ;
            }

            return true ;
        }
    }
}
