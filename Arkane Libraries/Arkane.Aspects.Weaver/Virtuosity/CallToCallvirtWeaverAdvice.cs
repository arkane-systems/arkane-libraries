#region header

// Arkane.Aspects.Weaver - CallToCallvirtWeaverAdvice.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-05 12:40 AM

#endregion

#region using

using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.CodeWeaver ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.Virtuosity
{
    internal class CallToCallvirtWeaverAdvice : IAdvice
    {
        public int Priority => 0 ;

        public bool RequiresWeave (WeavingContext context) =>
            context.InstructionReader.CurrentInstruction.OpCodeNumber == OpCodeNumber.Call ;

        public void Weave (WeavingContext context, InstructionBlock block)
        {
            IMethod             oldOperand = context.InstructionReader.CurrentInstruction.MethodOperand ;
            InstructionSequence sequence   = block.MethodBody.CreateInstructionSequence () ;
            block.AddInstructionSequence (sequence) ;
            context.InstructionWriter.AttachInstructionSequence (sequence) ;
            context.InstructionWriter.EmitInstructionMethod (OpCodeNumber.Callvirt, oldOperand) ;
            context.InstructionWriter.DetachInstructionSequence () ;
        }
    }
}
