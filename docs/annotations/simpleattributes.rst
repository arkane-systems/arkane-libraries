Simple Attributes
=================

These attributes simply incorporate the associated information into the assembly, and have no active function of their own.

Author
------

    [Author (string name, string email)]

This attribute embeds the authorship of an entire assembly or specific class, method, etc. Both parameters are required, and the *email* parameter must be a valid e-mail address.

This information can be read out using the *Name* and *EmailAddress* properties.

BugFix
------

    [BugFix (int caseNumber, string Comments = "Comments.")]

This attribute embeds the case number of a bug fix - for the specific class, method, etc. - in an issue-tracking system. The caseNumber parameter is required; additional comments may optionally be given.

This information can be read out of the *CaseNumber* and *Comments* properties.

Documentation
-------------

    [Documentation (string uri)]

This attribute, applicable only to assemblies, embeds the URL where documentation for that assembly may be found. The *uri* parameter must be a valid URL using the http:, https:, or ftp: protocol.

This information can be read out using the *Location* property.

LegacyWrapper
-------------

    [LegacyWrapper (string comments)]

This attribute indicates that the following class or method exists to insulate one from the liveliest awfulness of the legacy code that it's wrapped around.

Comments can be read out using the *Comments* property.

ObligatoryQuotation
-------------------

    [ObligatoryQuotation (string quotation, string source, string citation)]

It had to be said, so I did.

Why? Why not just in the comments? Well, it's because this sort of thing is useful insight into the mind of the developer, and - assuming we're not obfuscating - that should be available right there in the assembly as well as the source.

The citation for the original quotation should preferably, not necessarily, be supplied in URI format, and may be null.


The quotation can be read out using the *Quotation*, *Source*, and *Citation* properties.
