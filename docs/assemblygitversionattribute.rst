AssemblyGitVersionAttribute
===========================

.. note::

  This attribute is automatically applied to the assembly post-compilation by the :doc:`addgitstampattribute`. It should not be added in the source code.

Properties
----------

    `public string GitVersion { get; }`

A git version string, as returned by `git log -1 --format="%d %H"`. If the repository has been modified and the changes not yet committed, the string " modified" is appended.
