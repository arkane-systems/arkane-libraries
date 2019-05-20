SourceLanguageAttribute
=======================

    [SourceLanguage (:doc:`programminglanguages` language)]

An attribute, applicable only to assemblies, identifying the original programming language in which an assembly was developed, along with certain other related information.

Properties
----------

    public ProgrammingLanguages Language { get; }

The programming language in which this assembly was written.

    public bool RewrittenByPostSharp { get; set; }

Has this assembly been rewritten post-compilation by PostSharp?

    public bool Obfuscated { get; set; }

Has this assembly been obfuscated post-compilation in other ways, and how?

    public string PostCompilationModifications { get; set; }

