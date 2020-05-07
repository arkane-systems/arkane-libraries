#region header

// Arkane.Aspects.Weaver - OperatorInjection.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-06 1:52 AM

#endregion

#region using

using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.CodeModel.Helpers ;
using PostSharp.Sdk.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.StructuralEquality
{
    /// <summary>
    ///     Overwrites equality operators with a call to the equality method.
    /// </summary>
    public class OperatorInjection
    {
        public OperatorInjection (Project project)
        {
            TypeDefDeclaration objectTypeDef = project.Module.Cache.GetIntrinsic (IntrinsicType.Object).GetTypeDefinition () ;
            this.staticEqualsMethod = project.Module.FindMethod (objectTypeDef, "Equals", 2) ;

            TypeDefDeclaration operatorTypeDef =
                project.Module.Cache.GetType (typeof (EqualityOperator)).GetTypeDefinition () ;
            this.weaveMethod = project.Module.FindMethod (operatorTypeDef, "Weave") ;
        }

        private readonly IGenericMethodDefinition staticEqualsMethod ;
        private readonly IGenericMethodDefinition weaveMethod ;

        private void ProcessOperator (TypeDefDeclaration enhancedType, string operatorName, string operatorSourceName, bool negate)
        {
            IMethod operatorMethod = OperatorInjection.GetOperatorMethod (enhancedType, operatorName) ;

            string operatorExample =
                $"public static bool operator {operatorSourceName}({enhancedType.ShortName} left, {enhancedType.ShortName} right) => EqualityOperator.Weave(left, right);" ;

            if (operatorMethod == null)
                throw new InjectionException ("EQU10",
                                              $"The equality operator was not found on type {enhancedType.Name}, implement it like this: {operatorExample}") ;

            MethodDefDeclaration operatorMethodDef = operatorMethod.GetMethodDefinition () ;
            this.CheckOperator (operatorMethodDef, operatorExample) ;

            CompilerGeneratedAttributeHelper.AddCompilerGeneratedAttribute (operatorMethodDef) ;

            this.ReplaceOperator (enhancedType, operatorMethodDef, negate) ;
        }

        public void ProcessEqualityOperators (TypeDefDeclaration enhancedType, StructuralEqualityAttribute config)
        {
            if (config.DoNotAddEqualityOperators)
                return ;

            this.ProcessOperator (enhancedType, "op_Equality",   "==", false) ;
            this.ProcessOperator (enhancedType, "op_Inequality", "!=", true) ;
        }

        private static IMethod GetOperatorMethod (TypeDefDeclaration enhancedType, string name)
        {
            return enhancedType.Methods.GetMethod (name,
                                                   BindingOptions.DontThrowException,
                                                   method => method.IsStatic              &&
                                                             (method.ParameterCount == 2) &&
                                                             method.ReturnType.IsIntrinsic (IntrinsicType.Boolean)) ;
        }

        private void ReplaceOperator (TypeDefDeclaration   enhancedType,
                                      MethodDefDeclaration equalityMethodDef,
                                      bool                 negate)
        {
            InstructionBlock originalCode = equalityMethodDef.MethodBody.RootInstructionBlock ;
            originalCode.Detach () ;

            InstructionBlock root = equalityMethodDef.MethodBody.CreateInstructionBlock () ;
            equalityMethodDef.MethodBody.RootInstructionBlock = root ;
            InstructionSequence newSequence = root.AddInstructionSequence () ;
            using (var writer = InstructionWriter.GetInstance ())
            {
                writer.AttachInstructionSequence (newSequence) ;

                if (enhancedType.IsValueType ())
                {
                    IType canonicalType = enhancedType.GetCanonicalGenericInstance () ;
                    writer.EmitInstruction (OpCodeNumber.Ldarg_0) ;
                    writer.EmitInstructionType (OpCodeNumber.Box, canonicalType) ;
                    writer.EmitInstruction (OpCodeNumber.Ldarg_1) ;
                    writer.EmitInstructionType (OpCodeNumber.Box, canonicalType) ;
                }
                else
                {
                    writer.EmitInstruction (OpCodeNumber.Ldarg_0) ;
                    writer.EmitInstruction (OpCodeNumber.Ldarg_1) ;
                }

                writer.EmitInstructionMethod (OpCodeNumber.Call, this.staticEqualsMethod) ;

                if (negate)
                {
                    writer.EmitInstruction (OpCodeNumber.Ldc_I4_0) ;
                    writer.EmitInstruction (OpCodeNumber.Ceq) ;
                }

                writer.EmitInstruction (OpCodeNumber.Ret) ;

                writer.DetachInstructionSequence () ;
            }
        }

        private void CheckOperator (MethodDefDeclaration operatorMethodDef, string operatorExample)
        {
            if (!operatorMethodDef.HasBody)
                throw new InjectionException ("EQU5",
                                              $"Type {operatorMethodDef.DeclaringType.Name} has an operator without a body, implement it like this: {operatorExample}") ;

            using (InstructionReader reader = operatorMethodDef.MethodBody.CreateInstructionReader ())
            {
                InstructionSequence sequence = operatorMethodDef.MethodBody.RootInstructionBlock.FirstInstructionSequence ;
                reader.EnterInstructionSequence (sequence) ;

                for (var i = 0; i < 4; i++)
                {
                    if (!reader.ReadInstruction ())
                        throw new InjectionException ("EQU6",
                                                      $"Type {operatorMethodDef.DeclaringType.Name} has an operator with incorrect body, implement it like this: {operatorExample}") ;

                    if (i == 2)
                        if ((reader.CurrentInstruction.OpCodeNumber != OpCodeNumber.Call) ||
                            !reader.CurrentInstruction.MethodOperand.GetMethodDefinition ()
                                   .Equals (this.weaveMethod.GetMethodDefinition ()))
                            throw new InjectionException ("EQU7",
                                                          $"Type {operatorMethodDef.DeclaringType.Name} has an operator with incorrect body, implement it like this: {operatorExample}") ;
                }
            }
        }
    }
}
