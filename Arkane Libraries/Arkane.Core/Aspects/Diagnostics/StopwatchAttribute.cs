#region header

// Arkane.Core - StopwatchAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 8:56 AM

#endregion

#region using

using System.Diagnostics ;

using ArkaneSystems.Arkane.Logging ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Diagnostics
{
    /// <summary>
    ///     An aspect to time the execution of a method, in milliseconds.
    /// </summary>
    [ProvideAspectRole (StandardRoles.PerformanceInstrumentation)]
    [PSerializable]
    [PublicAPI]
    [Stopwatch (AttributeExclude = true)]
    public sealed class StopwatchAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        ///     Start the stopwatch upon entry to the method.
        /// </summary>
        public override void OnEntry (MethodExecutionArgs args) { args.MethodExecutionTag = Stopwatch.StartNew () ; }

        /// <summary>
        ///     Stop the stopwatch and report upon exit, successful or unsuccessful.
        /// </summary>
        public override void OnExit (MethodExecutionArgs args)
        {
            var sw = (Stopwatch) args.MethodExecutionTag ;
            sw.Stop () ;

            string output = $@"{args.Method.Name} executed in {sw.ElapsedMilliseconds} ms." ;
            LogProvider.GetLogger (args.Method.DeclaringType, "StopwatchAttribute").Debug (output) ;
        }
    }
}
