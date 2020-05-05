using System ;
using System.Linq ;

using PostSharp.Extensibility ;

namespace ArkaneSystems.Arkane.Aspects
{
    internal static class PostSharpHelpers
    {
        public static void RequireArkaneAspectsWeaver (Type annotationClass, object target, string functionDescription)
        {
            var weavingSymbols = PostSharpEnvironment.CurrentProject.GetService <IWeavingSymbolsService> () ;

            if (!(AppDomain.CurrentDomain.GetAssemblies ().Single (a => a.GetName ().Name == "Arkane.Aspects.Weaver") != null))
                functionDescription += "\nUse of this aspect requires the Arkane.Aspect.Weaver PostSharp plugin." ;

            weavingSymbols.PushAnnotation (target,
                                           annotationClass,
                                           description:
                                           functionDescription) ;
        }
    }
}
