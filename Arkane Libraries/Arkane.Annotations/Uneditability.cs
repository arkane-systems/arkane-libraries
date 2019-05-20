#region header

// Arkane.Annotations - Uneditability.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-20 1:44 AM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     The reason why you shouldn't edit this code.
    /// </summary>
    /// <remarks>
    ///     Terminology derived (in several cases) from the Jargon File, 4.4.8.
    /// </remarks>
    [PublicAPI]
    public enum Uneditability
    {
        /// <summary>
        ///     "An awesomely arcane technique central to a program or system, esp. one neither generally published nor available
        ///     to hackers at large (compare black art); one that could only have been composed by a true wizard. Compiler
        ///     optimization techniques and many aspects of OS design used to be deep magic; many techniques in cryptography,
        ///     signal processing, graphics, and AI still are."
        /// </summary>
        DeepMagic,

        /// <summary>
        ///     Like deep magic, but more so.  Treat even more carefully than instances of deep magic in the same code.
        /// </summary>
        DeeperMagic,

        /// <summary>
        ///     "A technique that works, though nobody really understands why."
        ///     Mostly ad-hoc. Since we don't know how it works, if it breaks, we probably can't fix it. All of which is to say -
        ///     like deep magic, but also evil.
        /// </summary>
        BlackMagic,

        /// <summary>
        ///     "Said of software that is functional but easily broken by changes in operating environment or configuration, or by
        ///     any minor tweak to the software itself. Also, any system that responds inappropriately and disastrously to abnormal
        ///     but expected external stimuli; e.g., a file system that is usually totally scrambled by a power failure is said to
        ///     be brittle [fragile]."
        /// </summary>
        Fragile,

        /// <summary>
        ///     Relies on knowledge not kept anywhere but the implementer's head.  At any rate, you don't have it.
        /// </summary>
        Undocumented,

        /// <summary>
        ///     "The canonical comment describing something magic or too complicated to bother explaining properly. From an
        ///     infamous comment in the context-switching code of the V6 Unix kernel."
        ///     Don't go there, okay?  Just don't.
        /// </summary>
        YouAreNotExpectedToUnderstandThis
    }
}
