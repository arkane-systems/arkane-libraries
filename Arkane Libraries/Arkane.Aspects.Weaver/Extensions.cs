#region header

// Arkane.Aspects.Weaver - Extensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 3:22 PM

#endregion

#region using

using System ;
using System.Collections.Generic ;

using PostSharp.Sdk.CodeModel ;
using PostSharp.Sdk.Extensibility.Tasks ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Weaver
{
    internal static class Extensions
    {
        /// <summary>
        ///     Gets the list of annotations of an attribute <paramref name="type" /> as a list rather than an enumerator.
        /// </summary>
        public static List <IAnnotationInstance> GetAnnotations (
            this IAnnotationRepositoryService service,
            Type                              type)
        {
            var l          = new List <IAnnotationInstance> () ;
            var enumerator = service.GetAnnotationsOfType (type, false, true) ;
            while (enumerator.MoveNext ())
            {
                IAnnotationInstance instance = enumerator.Current ;
                l.Add (instance) ;
            }

            return l ;
        }
    }
}
