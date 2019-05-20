Simple Attributes
=================

These attributes simply incorporate the associated information into the assembly, and have no active function of their own.

Author
------

    [Author (string name, string email)]

This attribute embeds the authorship of an entire assembly or specific class, method, etc. Both parameters are required, and the *email* parameter must be a valid e-mail address. This information can be read out using the *Name* and *EmailAddress* properties.

Documentation
-------------

    [Documentation (string uri)]

This attribute, applicable only to assemblies, embeds the URL where documentation for that assembly may be found. The *uri* parameter must be a valid URL using the http:, https:, or ftp: protocol. This information can be read out using the *Location* property.