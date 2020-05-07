#region header

// Arkane.Aspects.Weaver - StructuralEqualityTask.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 1:24 AM

#endregion

#region using

using System.Collections.Generic ;

using JetBrains.Annotations ;

using PostSharp.Extensibility ;
using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.Extensibility ;
using PostSharp.Sdk.Extensibility.Compilers ;
using PostSharp.Sdk.Extensibility.Configuration ;
using PostSharp.Sdk.Extensibility.Tasks ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.StructuralEquality
{
    [ExportTask (Phase                                  = TaskPhase.CustomTransform, TaskName = nameof (StructuralEqualityTask))]
    [TaskDependency ("AnnotationRepository", IsRequired = true, Position                      = DependencyPosition.Before)]
    [UsedImplicitly]
    public class StructuralEqualityTask : Task
    {
        [ImportService]
        private IAnnotationRepositoryService? annotationRepositoryService ;

        [ImportService]
        private ICompilerAdapterService? compilerAdapterService ;

        public override string CopyrightNotice => "Rafał Jasica, Simon Cropp, SharpCrafters s.r.o. and contributors" ;

        public override bool Execute ()
        {
            // Find ignored fields
            ISet <FieldDefDeclaration> ignoredFields =
                IgnoredFields.GetIgnoredFields (this.annotationRepositoryService!, this.compilerAdapterService!) ;

            // Sort types by inheritance hierarchy
            List <EqualsType> toEnhance = this.GetTypesToEnhance () ;

            var hashCodeInjection = new HashCodeInjection (this.Project) ;
            var equalsInjection   = new EqualsInjection (this.Project) ;
            var operatorInjection = new OperatorInjection (this.Project) ;

            foreach (EqualsType enhancedTypeData in toEnhance)
            {
                TypeDefDeclaration          enhancedType = enhancedTypeData.EnhancedType ;
                StructuralEqualityAttribute config       = enhancedTypeData.Config ;
                try
                {
                    if (!config.DoNotAddEquals)
                        equalsInjection.AddEqualsTo (enhancedType, config, ignoredFields) ;

                    if (!config.DoNotAddEqualityOperators)
                        operatorInjection.ProcessEqualityOperators (enhancedType, config) ;

                    if (!config.DoNotAddGetHashCode)
                        hashCodeInjection.AddGetHashCodeTo (enhancedType, config, ignoredFields) ;
                }
                catch (InjectionException exception)
                {
                    Message.Write (enhancedType, SeverityType.Error, exception.ErrorCode, exception.Message) ;
                    return false ;
                }
            }

            return true ;
        }

        /// <summary>
        ///     Gets types for which Equals or GetHashCode should be synthesized, in such an order that base classes come before
        ///     derived classes. This way, when Equals for a derived class is being created, you can be already sure that
        ///     the Equals for the base class was already created (if the base class was target of [StructuralEquality].
        /// </summary>
        private List <EqualsType> GetTypesToEnhance ()
        {
            IEnumerator <IAnnotationInstance> annotationsOfType =
                this.annotationRepositoryService!.GetAnnotationsOfType (typeof (StructuralEqualityAttribute), false, false) ;

            // TODO: Change the List into a StructuredDeclarationDictionary, because then Visit takes the order of inheritance into
            // account. But first we would need to make that public in PostSharp.Compiler.Engine.

            var toEnhance = new List <EqualsType> () ;

            while (annotationsOfType.MoveNext ())
            {
                IAnnotationInstance annotation = annotationsOfType.Current ;
                if (annotation?.TargetElement is TypeDefDeclaration enhancedType)
                {
                    StructuralEqualityAttribute config  = EqualityConfiguration.ExtractFrom (annotation.Value) ;
                    var                         newType = new EqualsType (enhancedType, config) ;
                    toEnhance.Add (newType) ;
                }
            }

            toEnhance.Sort ((first, second) =>
                            {
                                if (first.EnhancedType.IsAssignableTo (second.EnhancedType))
                                {
                                    if (second.EnhancedType.IsAssignableTo (first.EnhancedType))
                                        return 0 ;

                                    return 1 ;
                                }

                                return -1 ;
                            }) ;

            return toEnhance ;
        }
    }
}
