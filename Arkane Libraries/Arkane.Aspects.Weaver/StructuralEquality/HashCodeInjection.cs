#region header

// Arkane.Aspects.Weaver - HashCodeInjection.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 1:27 AM

#endregion

#region using

using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Reflection ;

using PostSharp.Extensibility ;
using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.CodeModel.Collections ;
using PostSharp.Sdk.CodeModel.Helpers ;
using PostSharp.Sdk.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.StructuralEquality
{
    public class HashCodeInjection
    {
        public HashCodeInjection (Project project)
        {
            this.project = project ;

            // Find Object.GetHashCode():
            ModuleDeclaration module  = this.project.Module ;
            INamedType        tObject = module.Cache.GetIntrinsic (IntrinsicType.Object).GetTypeDefinition () ;
            this.Object_GetHashCode = module.FindMethod (tObject, "GetHashCode") ;

            // Find IEnumerator.MoveNext() and IEnumerator.Current:
            this.IEnumeratorType = (INamedType) module.Cache.GetType (typeof (IEnumerator)) ;
            this.MoveNext        = module.FindMethod (this.IEnumeratorType, "MoveNext") ;
            this.GetCurrent      = module.FindMethod (this.IEnumeratorType, "get_Current") ;

            // Find IEnumerable.GetEnumerator()
            var tEnumerable = (INamedType) module.Cache.GetType (typeof (IEnumerable)) ;
            this.GetEnumerator = module.FindMethod (tEnumerable, "GetEnumerator") ;
        }

        private readonly IMethod    GetCurrent ;
        private readonly IMethod    GetEnumerator ;
        private readonly INamedType IEnumeratorType ;
        private readonly int        magicNumber = 397 ;
        private readonly IMethod    MoveNext ;
        private readonly IMethod    Object_GetHashCode ;
        private readonly Project    project ;

        public void AddGetHashCodeTo (TypeDefDeclaration          enhancedType,
                                      StructuralEqualityAttribute config,
                                      ISet <FieldDefDeclaration>  ignoredFields)
        {
            if (enhancedType.Methods.Any <IMethod> (m => (m.Name           == "GetHashCode") &&
                                                         (m.ParameterCount == 0)))

                // GetHashCode already present, just keep it.
                return ;

            // Create signature
            var method = new MethodDefDeclaration
                         {
                             Name              = "GetHashCode",
                             CallingConvention = CallingConvention.HasThis,
                             Attributes        = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig
                         } ;
            enhancedType.Methods.Add (method) ;
            CompilerGeneratedAttributeHelper.AddCompilerGeneratedAttribute (method) ;
            method.ReturnParameter =
                ParameterDeclaration.CreateReturnParameter (enhancedType.Module.Cache.GetIntrinsic (IntrinsicType.Int32)) ;

            // Generate ReSharper-style Fowler–Noll–Vo hash:
            using (var writer = InstructionWriter.GetInstance ())
            {
                CreatedEmptyMethod   getHashCodeData = MethodBodyCreator.CreateModifiableMethodBody (writer, method) ;
                LocalVariableSymbol? resultVariable  = getHashCodeData.ReturnVariable ;
                writer.AttachInstructionSequence (getHashCodeData.PrincipalBlock.AddInstructionSequence ()) ;

                // Start with 0
                writer.EmitInstruction (OpCodeNumber.Ldc_I4_0) ;
                writer.EmitInstructionLocalVariable (OpCodeNumber.Stloc, resultVariable) ;
                var first = true ;

                // Add base.GetHashCode():
                if (!config.IgnoreBaseClass)
                {
                    bool ignorable = (enhancedType.BaseTypeDef.Name   == "System.Object") ||
                                     (enhancedType.IsValueTypeSafe () == true) ;
                    if (!ignorable)
                    {
                        IGenericMethodDefinition? baseHashCode = this.project.Module.FindMethod (enhancedType.BaseTypeDef,
                                                                                                 "GetHashCode",
                                                                                                 BindingOptions.DontThrowException,
                                                                                                 0) ;

                        // TODO Gael says: using FindOverride would be better
                        if (baseHashCode != null)
                        {
                            writer.EmitInstructionLocalVariable (OpCodeNumber.Ldloc, resultVariable) ;
                            writer.EmitInstruction (OpCodeNumber.Ldarg_0) ;

                            // TODO what if it is two steps removed? then we won't call it!
                            writer.EmitInstructionMethod (OpCodeNumber.Call,
                                                          baseHashCode.GetGenericInstance (enhancedType
                                                                                          .BaseType.GetGenericContext ())) ;
                            writer.EmitInstruction (OpCodeNumber.Add) ;
                            writer.EmitInstructionLocalVariable (OpCodeNumber.Stloc, resultVariable) ;
                            first = false ;
                        }
                    }
                }

                // For each field, do "hash = hash * 397 ^ field?.GetHashCode();
                foreach (FieldDefDeclaration field in enhancedType.Fields)
                {
                    if (field.IsConst || field.IsStatic || ignoredFields.Contains (field))
                        continue ;

                    this.AddFieldCode (field, first, writer, resultVariable!, method, enhancedType) ;
                    first = false ;
                }

                // Now custom logic:
                foreach (MethodDefDeclaration customLogic in enhancedType.Methods)
                {
                    if (customLogic.CustomAttributes.GetOneByType (typeof (AdditionalGetHashCodeMethodAttribute)
                                                                      .FullName) !=
                        null)
                    {
                        writer.EmitInstructionLocalVariable (OpCodeNumber.Ldloc, resultVariable) ;
                        writer.EmitInstructionInt32 (OpCodeNumber.Ldc_I4, this.magicNumber) ;
                        writer.EmitInstruction (OpCodeNumber.Mul) ;
                        this.AddCustomLogicCall (enhancedType, writer, customLogic) ;
                        writer.EmitInstruction (OpCodeNumber.Xor) ;
                        writer.EmitInstructionLocalVariable (OpCodeNumber.Stloc, resultVariable) ;
                    }
                }

                // Return the hash:
                writer.EmitBranchingInstruction (OpCodeNumber.Br, getHashCodeData.ReturnSequence) ;
                writer.DetachInstructionSequence () ;
            }
        }

        private void AddCustomLogicCall (TypeDefDeclaration   enhancedType,
                                         InstructionWriter    writer,
                                         MethodDefDeclaration customMethod)
        {
            ParameterDeclarationCollection parameters = customMethod.Parameters ;
            if (parameters.Count != 0)
            {
                Message.Write (enhancedType,
                               SeverityType.Error,
                               "EQU1",
                               "The signature of a method annotated with ["  +
                               nameof (AdditionalGetHashCodeMethodAttribute) +
                               "] must be 'int MethodName()'. It can't have parameters.") ;
                writer.EmitInstruction (OpCodeNumber.Ldc_I4_0) ;
                return ;
            }

            if (!customMethod.ReturnParameter.ParameterType.IsIntrinsic (IntrinsicType.Int32))
            {
                Message.Write (enhancedType,
                               SeverityType.Error,
                               "EQU2",
                               "The signature of a method annotated with ["  +
                               nameof (AdditionalGetHashCodeMethodAttribute) +
                               "] must be 'int MethodName()'. Its return type must be 'int'.") ;
                writer.EmitInstruction (OpCodeNumber.Ldc_I4_0) ;
                return ;
            }

            writer.EmitInstruction (OpCodeNumber.Ldarg_0) ;
            writer.EmitInstructionMethod (
                                          enhancedType.IsValueTypeSafe () == true ? OpCodeNumber.Call : OpCodeNumber.Callvirt,
                                          customMethod.GetCanonicalGenericInstance ()) ;
        }

        private void AddFieldCode (FieldDefDeclaration  field,
                                   bool                 isFirst,
                                   InstructionWriter    writer,
                                   LocalVariableSymbol  resultVariable,
                                   MethodDefDeclaration method,
                                   TypeDefDeclaration   enhancedType)
        {
            bool           isCollection ;
            ITypeSignature propType           = field.FieldType ;
            bool           isValueType        = propType.IsValueTypeSafe () == true ;
            var            isGenericParameter = false ;

            if ((propType.TypeSignatureElementKind == TypeSignatureElementKind.GenericParameter) ||
                (propType.TypeSignatureElementKind == TypeSignatureElementKind.GenericParameterReference))
            {
                // TODO what does this mean?
                // maybe something else also needs to be checked?
                isCollection       = false ;
                isGenericParameter = true ;
            }
            else
            {
                isCollection = propType.IsCollection () ||
                               (propType.TypeSignatureElementKind == TypeSignatureElementKind.Array) ;
            }

            this.AddMultiplicityByMagicNumber (isFirst, writer, resultVariable, isCollection) ;

            if (propType.GetReflectionName ().StartsWith ("System.Nullable", StringComparison.Ordinal))
            {
                this.AddNullableProperty (field, writer) ;
            }
            else if (isCollection && (propType.GetReflectionName () != "System.String"))
            {
                this.AddCollectionCode (field, writer, resultVariable, method, enhancedType) ;
            }
            else if (isValueType || isGenericParameter)
            {
                this.LoadVariable (field, writer) ;
                if (propType.GetReflectionName () != "System.Int32")
                {
                    writer.EmitInstructionType (OpCodeNumber.Box, propType) ;
                    writer.EmitInstructionMethod (OpCodeNumber.Callvirt, this.Object_GetHashCode) ;
                }
            }
            else
            {
                this.LoadVariable (field, writer) ;
                this.AddNormalCode (field, writer, enhancedType) ;
            }

            if (!isFirst && !isCollection)
                writer.EmitInstruction (OpCodeNumber.Xor) ;

            if (!isCollection)
                writer.EmitInstructionLocalVariable (OpCodeNumber.Stloc, resultVariable) ;
        }

        private void AddCollectionCode (FieldDefDeclaration  field,
                                        InstructionWriter    writer,
                                        LocalVariableSymbol  resultVariable,
                                        MethodDefDeclaration method,
                                        TypeDefDeclaration   enhancedType)
        {
            if (field.FieldType.IsValueTypeSafe () == true)
            {
                this.AddCollectionCodeInternal (field, resultVariable, method, enhancedType, writer) ;
            }
            else
            {
                this.LoadVariable (field, writer) ;
                writer.IfNotZero (
                                  thenw =>
                                  {
                                      this.AddCollectionCodeInternal (field, resultVariable, method, enhancedType, thenw) ;
                                  },
                                  elsew => { }) ;
            }
        }

        private void AddCollectionCodeInternal (FieldDefDeclaration  field,
                                                LocalVariableSymbol  resultVariable,
                                                MethodDefDeclaration method,
                                                TypeDefDeclaration   enhancedType,
                                                InstructionWriter    writer)
        {
            this.LoadVariable (field, writer) ;

            LocalVariableSymbol enumeratorVariable =
                method.MethodBody.RootInstructionBlock.DefineLocalVariable (this.IEnumeratorType, "enumeratorVariable") ;
            LocalVariableSymbol currentVariable =
                method.MethodBody.RootInstructionBlock.DefineLocalVariable (
                                                                            method.Module.Cache.GetIntrinsic (IntrinsicType.Object),
                                                                            "enumeratorObject") ;

            this.AddGetEnumerator (writer, enumeratorVariable, field) ;

            this.AddCollectionLoop (resultVariable, writer, enumeratorVariable, currentVariable) ;
        }

        private void AddCollectionLoop (LocalVariableSymbol resultVariable,
                                        InstructionWriter   t,
                                        LocalVariableSymbol enumeratorVariable,
                                        LocalVariableSymbol currentVariable)
        {
            t.WhileNotZero (
                            c =>
                            {
                                c.EmitInstructionLocalVariable (OpCodeNumber.Ldloc, enumeratorVariable) ;
                                c.EmitInstructionMethod (OpCodeNumber.Callvirt, this.MoveNext) ;
                            },
                            b =>
                            {
                                b.EmitInstructionLocalVariable (OpCodeNumber.Ldloc, resultVariable) ;
                                b.EmitInstructionInt32 (OpCodeNumber.Ldc_I4, this.magicNumber) ;
                                b.EmitInstruction (OpCodeNumber.Mul) ;

                                b.EmitInstructionLocalVariable (OpCodeNumber.Ldloc, enumeratorVariable) ;
                                b.EmitInstructionMethod (OpCodeNumber.Callvirt, this.GetCurrent) ;
                                b.EmitInstructionLocalVariable (OpCodeNumber.Stloc, currentVariable) ;

                                b.EmitInstructionLocalVariable (OpCodeNumber.Ldloc, currentVariable) ;

                                b.IfNotZero (
                                             bt =>
                                             {
                                                 bt.EmitInstructionLocalVariable (OpCodeNumber.Ldloc, currentVariable) ;
                                                 bt.EmitInstructionMethod (OpCodeNumber.Callvirt, this.Object_GetHashCode) ;
                                             },
                                             et => { et.EmitInstruction (OpCodeNumber.Ldc_I4_0) ; }) ;

                                b.EmitInstruction (OpCodeNumber.Xor) ;
                                b.EmitInstructionLocalVariable (OpCodeNumber.Stloc, resultVariable) ;
                            }) ;
        }

        private void AddGetEnumerator (InstructionWriter ins, LocalVariableSymbol variable, FieldDefDeclaration field)
        {
            if (field.FieldType.IsValueTypeSafe () == true)
                ins.EmitInstructionType (OpCodeNumber.Box, field.FieldType) ;

            ins.EmitInstructionMethod (OpCodeNumber.Callvirt, this.GetEnumerator) ;
            ins.EmitInstructionLocalVariable (OpCodeNumber.Stloc, variable) ;
        }

        private void AddNullableProperty (FieldDefDeclaration field, InstructionWriter writer)
        {
            IMethodSignature getHasValue = new MethodSignature (field.Module,
                                                                CallingConvention.HasThis,
                                                                field.Module.Cache.GetIntrinsic (IntrinsicType.Boolean),
                                                                new List <ITypeSignature> (),
                                                                0) ;
            GenericMethodReference hasValueMethod = field.FieldType.FindMethod ("get_HasValue", getHasValue) ;
            writer.EmitInstruction (OpCodeNumber.Ldarg_0) ;
            writer.EmitInstructionField (OpCodeNumber.Ldflda, field.GetCanonicalGenericInstance ()) ;
            writer.EmitInstructionMethod (OpCodeNumber.Call,
                                          hasValueMethod.GetInstance (field.Module, hasValueMethod.GenericMap)) ;

            writer.IfNotZero (then =>
                              {
                                  writer.EmitInstruction (OpCodeNumber.Ldarg_0) ;
                                  writer.EmitInstructionField (OpCodeNumber.Ldfld, field.GetCanonicalGenericInstance ()) ;
                                  writer.EmitInstructionType (OpCodeNumber.Box, field.FieldType) ;
                                  writer.EmitInstructionMethod (OpCodeNumber.Callvirt, this.Object_GetHashCode) ;
                              },
                              elseBranch => { writer.EmitInstruction (OpCodeNumber.Ldc_I4_0) ; }) ;
        }

        private void AddNormalCode (FieldDefDeclaration field, InstructionWriter writer, TypeDefDeclaration enhancedType)
        {
            writer.IfNotZero (
                              thenw =>
                              {
                                  this.LoadVariable (field, thenw) ;
                                  thenw.EmitInstructionMethod (OpCodeNumber.Callvirt, this.Object_GetHashCode) ;
                              },
                              elsew => { elsew.EmitInstruction (OpCodeNumber.Ldc_I4_0) ; }) ;
        }

        private void LoadVariable (FieldDefDeclaration field, InstructionWriter writer)
        {
            writer.EmitInstruction (OpCodeNumber.Ldarg_0) ;
            writer.EmitInstructionField (OpCodeNumber.Ldfld, field.GetCanonicalGenericInstance ()) ;
        }

        private void AddMultiplicityByMagicNumber (bool                isFirst,
                                                   InstructionWriter   writer,
                                                   LocalVariableSymbol resultVariable,
                                                   bool                isCollection)
        {
            if (!isFirst && !isCollection)
            {
                writer.EmitInstructionLocalVariable (OpCodeNumber.Ldloc, resultVariable) ;
                writer.EmitInstructionInt32 (OpCodeNumber.Ldc_I4, this.magicNumber) ;
                writer.EmitInstruction (OpCodeNumber.Mul) ;
            }
        }
    }
}
