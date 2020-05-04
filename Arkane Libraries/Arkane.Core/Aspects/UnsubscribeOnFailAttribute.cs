#region header

// Arkane.Core - UnsubscribeOnFailAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 1:55 AM

#endregion

#region using

using System ;
using System.Linq ;
using System.Reflection ;

using ArkaneSystems.Arkane.Logging ;
using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect automatically unsubscribing event handlers which throw exceptions so that they do
    ///     not cause future issues.
    /// </summary>
    [PublicAPI]
    [PSerializable]
    public sealed class UnsubscribeOnFailAttribute : EventInterceptionAspect
    {
        /// <summary>
        ///     Method invoked when the event to which the current aspect is applied is fired, <i>for each</i> delegate
        ///     of this event, and <i>instead of</i> invoking this delegate.
        /// </summary>
        /// <param name="args">Handler arguments.</param>
        public override void OnInvokeHandler (EventInterceptionArgs args)
        {
            try
            {
                base.OnInvokeHandler (args) ;
            }

            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                LogProvider.GetLogger (args.Event.DeclaringType, "UnsubscribeOnFailAttribute")
                           .Debug (
                                   Resources.UnsubscribeOnFailAttribute_OnInvokeHandler_EventHandlerFailedUnsubscribing,

                                   // ReSharper disable once ExceptionNotDocumented
                                   // cannot happen
                                   args.Handler.GetMethodInfo ()!.Name,
                                   string.Join (", ",
                                                args.Arguments.Select (
                                                                       a => a?.ToString () ?? "null")),
                                   ex.GetType ().Name) ;

                args.RemoveHandler (args.Handler) ;

                // ReSharper disable once ExceptionNotDocumented
                // ReSharper disable once ThrowingSystemException
                throw ;
            }
        }
    }
}
