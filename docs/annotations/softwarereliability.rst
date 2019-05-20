SoftwareReliability
===================

    public enum SoftwareReliability

An enumeration of subjective assessments of code reliability, for use with the :doc:`codequalityattribute`.

The terms used in this enumeration are derived, in several cases, from the Jargon File (http://www.catb.org/jargon/html/), 4.4.8.

The values of the enumeration are as follows:

.. glossary::

  Broken
    "1. Not working according to design (of programs). This is the mainstream sense.
    2. Improperly designed, This sense carries a more or less disparaging implication that the designer should have known better, while sense 1 doesn't necessarily assign blame. Which of senses 1 or 2 is intended is conveyed by context and nonverbal cues."

  Flaky
    "Subject to frequent lossage. This use is of course related to the common slang use of the word to describe a person as eccentric, crazy, or just unreliable. A system that is flaky is working, sort of — enough that you are tempted to try to use it — but fails frequently enough that the odds in favor of finishing what you start are low."

  Fragile
    "Said of software that is functional but easily broken by changes in operating environment or configuration, or by any minor tweak to the software itself. Also, any system that responds inappropriately and disastrously to abnormal but expected external stimuli; e.g., a file system that is usually totally scrambled by a power failure is said to be brittle [fragile]."

  Solid
    The assumed default reliability.

  Robust
    "Said of a system that has demonstrated an ability to recover gracefully from the whole range of exceptional inputs and situations in a given environment. One step below bulletproof. Carries the additional connotation of elegance in addition to just careful attention to detail."

  Bulletproof
    "Used of an algorithm or implementation considered extremely robust; lossage-resistant; capable of correctly recovering from any imaginable exception condition — a rare and valued quality. Implies that the programmer has thought of all possible errors, and added code to protect against each one."

