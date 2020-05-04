﻿#region header

// Arkane.Aspects.Weaver - Configuration.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 3:23 PM

#endregion

#region using

using System.Collections.Generic ;

using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.CodeModel.Collections ;
using PostSharp.Sdk.Extensibility.Tasks ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver.ToString
{
    internal class Configuration
    {
        public string PropertiesSeparator { get ; set ; } = "," ;

        public string NameValueSeparator { get ; set ; } = ":" ;

        public bool WrapWithBraces { get ; set ; } = true ;

        public bool WriteTypeName { get ; set ; } = true ;

        public bool IncludePrivate { get ; set ; }

        /// <summary>
        ///     Gets ToString configuration from an attribute, merging with the higher-level configuration
        ///     <paramref name="baseConfiguration" />.
        ///     This higher-level configuration can come from:
        ///     <list type="bullet">
        ///         <item>[<see cref="ToStringGlobalOptionsAttribute" />].</item>
        ///         <item>Default values.</item>
        ///     </list>
        /// </summary>
        public static Configuration ReadConfiguration (IAnnotationValue value, Configuration baseConfiguration)
        {
            Configuration             config         = new Configuration () ;
            MemberValuePairCollection namedArguments = value.NamedArguments ;
            config.WrapWithBraces = (bool) (namedArguments[nameof (ToStringAttribute.WrapWithBraces)]?.Value.Value ??
                                            baseConfiguration.WrapWithBraces) ;
            config.WriteTypeName = (bool) (namedArguments[nameof (ToStringAttribute.WriteTypeName)]?.Value.Value ??
                                           baseConfiguration.WriteTypeName) ;
            config.IncludePrivate = (bool) (namedArguments[nameof (ToStringAttribute.IncludePrivate)]?.Value.Value ??
                                            baseConfiguration.IncludePrivate) ;
            config.NameValueSeparator =
                (string) (namedArguments[nameof (AbstractBaseToStringAttribute.PropertyNameToValueSeparator)]?.Value.Value ??
                          baseConfiguration.NameValueSeparator) ;
            config.PropertiesSeparator = (string) (namedArguments[nameof (ToStringAttribute.PropertiesSeparator)]?.Value.Value ??
                                                   baseConfiguration.PropertiesSeparator) ;
            return config ;
        }

        public static Configuration FindGlobalConfiguration (IAnnotationRepositoryService annotationRepositoryService)
        {
            List <IAnnotationInstance> global =
                annotationRepositoryService.GetAnnotations (typeof (ToStringGlobalOptionsAttribute)) ;
            Configuration basicConfig = new Configuration () ;
            if (global.Count > 0)
                basicConfig = Configuration.ReadConfiguration (global[0].Value, basicConfig) ;

            return basicConfig ;
        }
    }
}
