#region header

// Arkane.Aspects.Weaver - VirtuosityTask.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:37 AM

#endregion

#region using

using System.Collections.Generic ;
using System.Reflection ;

using JetBrains.Annotations ;

using PostSharp.Reflection ;
using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.CodeWeaver ;
using PostSharp.Sdk.Extensibility ;
using PostSharp.Sdk.Extensibility.Tasks ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.Virtuosity
{
    [UsedImplicitly]
    [ExportTask (Phase = TaskPhase.CustomTransform, TaskName = nameof (VirtuosityTask))]
    public class VirtuosityTask : Task
    {
        [ImportService]
#pragma warning disable IDE0044 // Add readonly modifier
        private IAnnotationRepositoryService? annotationService ;
#pragma warning restore IDE0044 // Add readonly modifier

        public override string CopyrightNotice => "Simon Cropp, SharpCrafters s.r.o., and contributors" ;

        public override bool Execute ()
        {
            List <MethodDefDeclaration> alteredMethods = this.FindAndProcessMethodsToVirtualize () ;
            this.ConvertCallToCallvirt (alteredMethods) ;
            this.ConvertNewToOverride (alteredMethods) ;
            return true ;
        }

        private void ConvertNewToOverride (List <MethodDefDeclaration> alteredMethods)
        {
            foreach (MethodDefDeclaration methodDefDeclaration in alteredMethods)
            {
                if (methodDefDeclaration.IsAbstract ||
                    !methodDefDeclaration.HasBody   ||
                    !methodDefDeclaration.IsNew)

                    // Nothing to be done.
                    continue ;

                foreach (MethodDefDeclaration baseMethod in alteredMethods)
                {
                    if ((methodDefDeclaration.DeclaringType.BaseTypeDef != null) &&
                        (methodDefDeclaration.DeclaringType.BaseType    == baseMethod.DeclaringType))
                        if (((IMethodSignature) methodDefDeclaration).DefinitionMatchesReference (baseMethod))
                        {
                            // No longer new
                            methodDefDeclaration.Attributes &= ~MethodAttributes.NewSlot ;
                            break ;
                        }
                }
            }
        }

        private void ConvertCallToCallvirt (List <MethodDefDeclaration> alteredMethods)
        {
            // TODO: Handle Ldftn -> Ldvirtftn

            using PostSharp.Sdk.CodeWeaver.Weaver weaver = new PostSharp.Sdk.CodeWeaver.Weaver (this.Project) ;
            weaver.AddMethodLevelAdvice (new CallToCallvirtWeaverAdvice (), null, JoinPointKinds.InsteadOfCall, alteredMethods) ;
            weaver.Weave () ;
        }

        private List <MethodDefDeclaration> FindAndProcessMethodsToVirtualize ()
        {
            List <MethodDefDeclaration> alteredMethods = new List <MethodDefDeclaration> () ;
            var tor = this.annotationService!.GetAnnotationsOfType (typeof (VirtualAttribute), false, false) ;
            while (tor.MoveNext ())
            {
                var possibleTarget = tor.Current!.TargetElement ;
                if (possibleTarget is MethodDefDeclaration method)
                    if (this.ProcessMethod (method))
                        alteredMethods.Add (method) ;
            }

            return alteredMethods ;
        }

        private bool ProcessMethod (MethodDefDeclaration method)
        {
            if (method.IsSealed && method.IsVirtual && !method.DeclaringType.IsSealed)
            {
                method.Attributes &= ~MethodAttributes.Final ;
            }
            else if ((method.Name == ".ctor")                  ||
                     method.IsSealed                           ||
                     method.IsVirtual                          ||
                     method.IsStatic                           ||
                     method.DeclaringType.IsSealed             ||
                     (method.Visibility == Visibility.Private) ||
                     this.MethodIsSerializationCallback (method))
            {
                return false ;
            }
            else
            {
                method.Attributes |= MethodAttributes.Virtual ;
                method.Attributes |= MethodAttributes.NewSlot ;
            }

            // Further processing
            return true ;
        }

        private bool MethodIsSerializationCallback (MethodDefDeclaration method) =>
            this.MethodContainsSerializationAttribute (method, "OnSerializingAttribute")   ||
            this.MethodContainsSerializationAttribute (method, "OnSerializedAttribute")    ||
            this.MethodContainsSerializationAttribute (method, "OnDeserializingAttribute") ||
            this.MethodContainsSerializationAttribute (method, "OnDeserializedAttribute") ;

        private bool MethodContainsSerializationAttribute (MethodDefDeclaration method, string simpleName) =>
            method.CustomAttributes.GetOneByType ("System.Runtime.Serialization." + simpleName) != null ;
    }
}
