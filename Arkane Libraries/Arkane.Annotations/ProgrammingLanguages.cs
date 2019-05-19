#region header

// Arkane.Annotations - ProgrammingLanguages.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 6:47 PM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     An enumeration of programming languages.
    /// </summary>
    /// <remarks>
    ///     Principally intended for use with <see cref="SourceLanguageAttribute" />.
    /// </remarks>
    [PublicAPI]
    public enum ProgrammingLanguages
    {
        /// <summary>
        ///     Indicates the C♯ language.
        /// </summary>
        CSharp,

        /// <summary>
        ///     Indicates the Visual Basic.NET language.
        /// </summary>
        VisualBasic,

        /// <summary>
        ///     Indicates the F♯ language.
        /// </summary>
        FSharp,

        /// <summary>
        ///     Indicates raw IL, written directly.
        /// </summary>
        Il,

        /// <summary>
        ///     Indicates the Python language.
        /// </summary>
        Python,

        /// <summary>
        ///     Indicates the PowerShell scripting language.
        /// </summary>
        PowerShell,

        /// <summary>
        ///     Indicates the JavaScript language.
        /// </summary>
        JavaScript
    }
}
