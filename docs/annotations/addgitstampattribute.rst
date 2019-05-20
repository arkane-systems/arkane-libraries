AddGitStampAttribute
====================

    [AddGitStamp]

When added to an assembly, this attribute marks it, post-compilation, with precise version information derived from its git repository. Specifically, it extracts the git information from the repo, combines it with the assembly version, and places it in the :doc:`assemblygitversionattribute`, which see for more details.

.. note::

  The use of this attribute requires git to be in the PATH at compile-time.

The function of this attribute/aspect was derived from that of the Fody equivalent, Fody.Stamp ( https://github.com/Fody/Stamp ), although independently implemented.
