#region header

// Arkane.Aspects.Weaver - DeepSerializableTask.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:32 AM

#endregion

#region using

using System.Collections.Generic ;
using System.Reflection ;

using JetBrains.Annotations ;

using PostSharp.Extensibility ;
using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.CodeModel.TypeSignatures ;
using PostSharp.Sdk.Extensibility ;
using PostSharp.Sdk.Extensibility.Configuration ;
using PostSharp.Sdk.Extensibility.Tasks ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.DeepSerializable
{
    [UsedImplicitly]
    [ExportTask (Phase                                  = TaskPhase.CustomTransform, TaskName = nameof (DeepSerializableTask))]
    [TaskDependency (TaskNames.AspectWeaver, IsRequired = false, Position                     = DependencyPosition.After)]
    public class DeepSerializableTask : Task
    {
        private readonly HashSet <TypeDefDeclaration> examinedTypes = new HashSet <TypeDefDeclaration> () ;

        [ImportService]
#pragma warning disable IDE0044 // Add readonly modifier
        private IAnnotationRepositoryService? annotationService ;
#pragma warning restore IDE0044 // Add readonly modifier

        public override string CopyrightNotice => "PostSharp Technologies" ;

        public override bool Execute ()
        {
            IEnumerator <IAnnotationInstance> annotations =
                this.annotationService!.GetAnnotationsOfType (typeof (DeepSerializableAttribute), false, true) ;

            while (annotations.MoveNext ())
            {
                IAnnotationInstance annotation = annotations.Current ;
                // ReSharper disable once PossibleNullReferenceException
                this.MakeSerializableRecursively ((annotation.TargetElement as TypeDefDeclaration)!) ;
            }

            return base.Execute () ;
        }

        private void MakeSerializableRecursively (TypeDefDeclaration type)
        {
            if (this.examinedTypes.Contains (type))

                // We've already analyzed this type.
                return ;

            this.examinedTypes.Add (type) ;

            this.MakeSerializable (type) ;

            foreach (FieldDefDeclaration field in type.Fields)
                this.IdentifyAndMakeSerializableRecursively (field.FieldType, field) ;

            this.IdentifyAndMakeSerializableRecursively (type.BaseType, type) ;
        }

        private void IdentifyAndMakeSerializableRecursively (ITypeSignature type, MessageLocation location)
        {
            switch (type.TypeSignatureElementKind)
            {
                case TypeSignatureElementKind.Intrinsic:
                    // This works automatically for most, but:
                    // Consider an error for object, IntPtr, but also consider that those fields may be marked as [NonSerialized].
                    // In the future, we might want to exclude such fields.
                    break ;

                case TypeSignatureElementKind.TypeDef:
                    var typeDef = (TypeDefDeclaration) type ;
                    if (typeDef.DeclaringAssembly == this.Project.Module.DeclaringAssembly)
                        this.MakeSerializableRecursively (typeDef) ;
                    else
                        this.VerifySerializable (typeDef, location) ;
                    break ;

                case TypeSignatureElementKind.TypeRef:
                    this.IdentifyAndMakeSerializableRecursively (type.GetTypeDefinition (), location) ;
                    break ;

                case TypeSignatureElementKind.GenericInstance:
                    var
                        genericInstanceSignature = type as GenericTypeInstanceTypeSignature ;
                    this.IdentifyAndMakeSerializableRecursively (genericInstanceSignature!.ElementType, location) ;
                    foreach (ITypeSignature argument in genericInstanceSignature.GenericArguments)
                        this.IdentifyAndMakeSerializableRecursively (argument, location) ;
                    break ;

                case TypeSignatureElementKind.Array:
                    var arraySignature = type as ArrayTypeSignature ;
                    this.IdentifyAndMakeSerializableRecursively (arraySignature!.ElementType, location) ;
                    break ;
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void VerifySerializable (TypeDefDeclaration type, MessageLocation location)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (!DeepSerializableTask.IsSerializable (type))
            {
                // This does not work properly on .NET Core.
                // PostSharp has difficulty understanding that some classes in .NET Core are serializable.

                // In addition, some fields are not meant to be serialized so it's better not to emit a warning for those
                // as well.

                // Message.Write(location, SeverityType.Warning, "DSER001",
                //     "A type (" + type.Name +
                //     ") is not serializable, but it's not in the same assembly so I cannot modify it.");
            }
        }

        private void MakeSerializable (TypeDefDeclaration type)
        {
            if (!DeepSerializableTask.IsSerializable (type))
                type.Attributes |= TypeAttributes.Serializable ;
        }

        private static bool IsSerializable (TypeDefDeclaration type) => (type.Attributes & TypeAttributes.Serializable) != 0 ;
    }
}
