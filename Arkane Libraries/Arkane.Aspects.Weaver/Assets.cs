#region header

// Arkane.Aspects.Weaver - Assets.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 3:19 PM

#endregion

#region using

using PostSharp.Sdk.CodeModel ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver
{
    internal class Assets
    {
        public Assets (ModuleDeclaration module)
        {
            TypeDefDeclaration stringType = module.Cache.GetType (typeof (string)).GetTypeDefinition () ;
            this.String_Format = module.FindMethod (stringType,
                                                    "Format",
                                                    m =>
                                                        (m.Parameters.Count == 2) &&
                                                        (m.Parameters[1].ParameterType.TypeSignatureElementKind ==
                                                         TypeSignatureElementKind.Array)) ;
        }

        /// <summary>
        ///     Gets the method <see cref="string.Format(string,object[])" />.
        /// </summary>
        public IMethod String_Format { get ; }
    }
}
