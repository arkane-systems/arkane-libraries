#region header

// Arkane.Annotations - SoftwareQuality.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-20 11:33 AM

#endregion

#region using

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     An enum passing subjective judgment on code quality.
    /// </summary>
    /// <remarks>
    ///     Terminology derived from the Jargon File, 4.4.8.
    /// </remarks>
    [PublicAPI]
    public enum SoftwareQuality
    {
        /// <summary>
        ///     "A ridiculously elephantine program or system, esp. one that is buggy or only marginally functional."
        /// </summary>
        Monstrosity,

        /// <summary>
        ///     "Obviously wrong; cretinous; demented. There is an implication that the person responsible must have suffered brain
        ///     damage, because he should have known better. Calling something brain-damaged is really bad; it also implies it is
        ///     unusable,
        ///     and that its failure to work is due to poor design rather than some accident. “Only six monocase characters per
        ///     file name? Now that's brain-damaged!”"
        /// </summary>
        BrainDamage,

        /// <summary>
        ///     "A lose, usually in software. Especially used for user-visible misbehavior caused by a b-u-g or misfeature."
        /// </summary>
        Screw,

        /// <summary>
        ///     "An unwanted and unintended property of a program or piece of hardware, esp. one that causes it to malfunction.
        ///     Antonym of feature."
        /// </summary>
        Bug,

        /// <summary>
        ///     "To be exceptionally unaesthetic or crocky."
        /// </summary>
        Lose,

        /// <summary>
        ///     "A feature that eventually causes lossage, possibly because it is not adequate for a new situation that has
        ///     evolved. Since it results from a deliberate and properly implemented feature, a misfeature is not a b-u-g. Nor is
        ///     it
        ///     a simple
        ///     unforeseen side effect; the term implies that the feature in question was carefully planned, but its long-term
        ///     consequences
        ///     were not accurately or adequately predicted (which is quite different from not having thought ahead at all). A
        ///     misfeature
        ///     can be a particularly stubborn problem to resolve, because fixing it usually involves a substantial philosophical
        ///     change to
        ///     the structure of the system involved."
        /// </summary>
        Misfeature,

        /// <summary>
        ///     "1. An awkward feature or programming technique that ought to be made cleaner. For example, using small integers to
        ///     represent error codes without the program interpreting them to the user (as in, for example, Unix make(1), which
        ///     returns code 139 for a process that dies due to segfault).
        ///     2.  A technique that works acceptably, but which is quite prone to failure if disturbed in the least."
        /// </summary>
        Crock,

        /// <summary>
        ///     "1. A Rube Goldberg (or Heath Robinson) device, whether in hardware or software.
        ///     2. A clever programming trick intended to solve a particular nasty case in an expedient, if not clear, manner.
        ///     Often used to repair bugs. Often involves ad-hockery and verges on being a crock.
        ///     3. Something that works for the wrong reason."
        /// </summary>
        Kluge,

        /// <summary>
        ///     "1. Originally, a quick job that produces what is needed, but not well.
        ///     2. An incredibly good, and perhaps very time-consuming, piece of work that produces exactly what is needed."
        /// </summary>
        Hack,

        /// <summary>
        ///     The assumed default quality.
        ///     "1. To succeed. A program wins if no unexpected conditions arise, or (especially) if it is sufficiently robust to
        ///     take exceptions in stride.
        ///     2. n. Success, or a specific instance thereof. A pleasing outcome."
        /// </summary>
        Win,

        /// <summary>
        ///     "1. A good property or behavior (as of a program). Whether it was intended or not is immaterial.
        ///     2. An intended property or behavior (as of a program). Whether it is good or not is immaterial (but if bad, it is
        ///     also a misfeature)."
        /// </summary>
        Feature,

        /// <summary>
        ///     "Combining simplicity, power, and a certain ineffable grace of design. Higher praise than ‘clever’, ‘winning’, or
        ///     even cuspy.
        ///     The French aviator, adventurer, and author Antoine de Saint-Exupery, probably best known for his classic children's
        ///     book The Little Prince, was also an aircraft designer. He gave us perhaps the best definition of engineering
        ///     elegance when he said “A designer knows he has achieved perfection not when there is nothing left to add, but when
        ///     there is nothing left to take away.”
        /// </summary>
        Elegance,

        /// <summary>
        ///     Unattainably brilliant.
        /// </summary>
        /// <remarks>
        ///     To reflect that, you can't actually use SoftwareQuality.Perfection in the CodeQualityAttribute.
        /// </remarks>
        Perfection
    }
}
