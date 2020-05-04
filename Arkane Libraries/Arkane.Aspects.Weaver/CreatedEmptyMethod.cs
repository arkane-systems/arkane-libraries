#region header

// Arkane.Aspects.Weaver - CreatedEmptyMethod.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 3:26 PM

#endregion

#region using

using PostSharp.Sdk.CodeModel ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver
{
    /// <summary>
    ///     Represents a newly created empty method and contains points in the method that callers might want to use.
    ///     The method is of the form
    ///     <code>
    /// {
    ///    var returnVariable;
    ///    principal-block: {
    /// 
    ///    }
    ///    return-sequence:
    ///     return returnVariable;
    /// }
    /// </code>
    /// </summary>
    internal class CreatedEmptyMethod
    {
        public CreatedEmptyMethod (MethodDefDeclaration methodDeclaration,
                                   InstructionBlock     principalBlock,
                                   LocalVariableSymbol? returnVariable,
                                   InstructionSequence  returnSequence)
        {
            this.MethodDeclaration = methodDeclaration ;
            this.PrincipalBlock    = principalBlock ;
            this.ReturnVariable    = returnVariable ;
            this.ReturnSequence    = returnSequence ;
        }

        /// <summary>
        ///     Gets the created method.
        /// </summary>
        public MethodDefDeclaration MethodDeclaration { get ; }

        /// <summary>
        ///     Gets the instruction block that's before <see cref="ReturnSequence" />.
        /// </summary>
        public InstructionBlock PrincipalBlock { get ; }

        /// <summary>
        ///     Gets the variable that the principal block should assign before ending. It will be returned by the return sequence.
        /// </summary>
        public LocalVariableSymbol? ReturnVariable { get ; }

        /// <summary>
        ///     Gets the label to the filled-in sequence that returns <see cref="ReturnVariable" /> using <c>ldloc</c> and
        ///     <c>ret</c>.
        /// </summary>
        public InstructionSequence ReturnSequence { get ; }
    }
}
