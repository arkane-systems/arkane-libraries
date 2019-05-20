#region header

// Arkane.Annotations - SoftwareReliability.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-20 11:48 AM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     An enum passing subjective judgment on code reliability.
    /// </summary>
    /// <remarks>
    ///     Terminology derived from the Jargon File, 4.4.8.
    /// </remarks>
    [PublicAPI]
    public enum SoftwareReliability
    {
        /// <summary>
        ///     "1. Not working according to design (of programs). This is the mainstream sense.
        ///     2. Improperly designed, This sense carries a more or less disparaging implication that the designer should have
        ///     known better, while sense 1 doesn't necessarily assign blame. Which of senses 1 or 2 is intended is conveyed by
        ///     context and nonverbal cues."
        /// </summary>
        Broken,

        /// <summary>
        ///     "Subject to frequent lossage. This use is of course related to the common slang use of the word to describe a
        ///     person as eccentric, crazy, or just unreliable. A system that is flaky is working, sort of — enough that you are
        ///     tempted to try to use it — but fails frequently enough that the odds in favor of finishing what you start are low."
        /// </summary>
        Flaky,

        /// <summary>
        ///     "Said of software that is functional but easily broken by changes in operating environment or configuration, or by
        ///     any minor tweak to the software itself. Also, any system that responds inappropriately and disastrously to abnormal
        ///     but expected external stimuli; e.g., a file system that is usually totally scrambled by a power failure is said to
        ///     be brittle [fragile]."
        /// </summary>
        Fragile,

        /// <summary>
        ///     The assumed default reliability.
        /// </summary>
        Solid,

        /// <summary>
        ///     "Said of a system that has demonstrated an ability to recover gracefully from the whole range of exceptional inputs
        ///     and situations in a given environment. One step below bulletproof. Carries the additional connotation of elegance
        ///     in addition to just careful attention to detail."
        /// </summary>
        Robust,

        /// <summary>
        ///     "Used of an algorithm or implementation considered extremely robust; lossage-resistant; capable of correctly
        ///     recovering from any imaginable exception condition — a rare and valued quality. Implies that the programmer has
        ///     thought of all possible errors, and added code to protect against each one."
        /// </summary>
        Bulletproof
    }
}
